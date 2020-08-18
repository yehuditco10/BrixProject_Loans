
/* TableNameVariable */

declare @tableName nvarchar(max) = '[' + @schema + '].[' + @tablePrefix + N'LoanPolicySaga]';
declare @tableNameWithoutSchema nvarchar(max) = @tablePrefix + N'LoanPolicySaga';


/* Initialize */

/* CreateTable */

if not exists
(
    select *
    from sys.objects
    where
        object_id = object_id(@tableName) and
        type in ('U')
)
begin
declare @createTable nvarchar(max);
set @createTable = '
    create table ' + @tableName + '(
        Id uniqueidentifier not null primary key,
        Metadata nvarchar(max) not null,
        Data nvarchar(max) not null,
        PersistenceVersion varchar(23) not null,
        SagaTypeVersion varchar(23) not null,
        Concurrency int not null
    )
';
exec(@createTable);
end

/* AddProperty LoanId */

if not exists
(
  select * from sys.columns
  where
    name = N'Correlation_LoanId' and
    object_id = object_id(@tableName)
)
begin
  declare @createColumn_LoanId nvarchar(max);
  set @createColumn_LoanId = '
  alter table ' + @tableName + N'
    add Correlation_LoanId uniqueidentifier;';
  exec(@createColumn_LoanId);
end

/* VerifyColumnType Guid */

declare @dataType_LoanId nvarchar(max);
set @dataType_LoanId = (
  select data_type
  from INFORMATION_SCHEMA.COLUMNS
  where
    table_name = @tableNameWithoutSchema and
    table_schema = @schema and
    column_name = 'Correlation_LoanId'
);
if (@dataType_LoanId <> 'uniqueidentifier')
  begin
    declare @error_LoanId nvarchar(max) = N'Incorrect data type for Correlation_LoanId. Expected uniqueidentifier got ' + @dataType_LoanId + '.';
    throw 50000, @error_LoanId, 0
  end

/* WriteCreateIndex LoanId */

if not exists
(
    select *
    from sys.indexes
    where
        name = N'Index_Correlation_LoanId' and
        object_id = object_id(@tableName)
)
begin
  declare @createIndex_LoanId nvarchar(max);
  set @createIndex_LoanId = N'
  create unique index Index_Correlation_LoanId
  on ' + @tableName + N'(Correlation_LoanId)
  where Correlation_LoanId is not null;';
  exec(@createIndex_LoanId);
end

/* PurgeObsoleteIndex */

declare @dropIndexQuery nvarchar(max);
select @dropIndexQuery =
(
    select 'drop index ' + name + ' on ' + @tableName + ';'
    from sysindexes
    where
        Id = object_id(@tableName) and
        Name is not null and
        Name like 'Index_Correlation_%' and
        Name <> N'Index_Correlation_LoanId'
);
exec sp_executesql @dropIndexQuery

/* PurgeObsoleteProperties */

declare @dropPropertiesQuery nvarchar(max);
select @dropPropertiesQuery =
(
    select 'alter table ' + @tableName + ' drop column ' + column_name + ';'
    from INFORMATION_SCHEMA.COLUMNS
    where
        table_name = @tableNameWithoutSchema and
        table_schema = @schema and
        column_name like 'Correlation_%' and
        column_name <> N'Correlation_LoanId'
);
exec sp_executesql @dropPropertiesQuery

/* CompleteSagaScript */
