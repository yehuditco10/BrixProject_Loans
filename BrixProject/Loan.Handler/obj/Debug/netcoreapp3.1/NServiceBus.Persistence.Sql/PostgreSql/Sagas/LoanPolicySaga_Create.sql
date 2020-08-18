
/* TableNameVariable */

/* Initialize */

/* CreateTable */

create or replace function pg_temp.create_saga_table_LoanPolicySaga(tablePrefix varchar, schema varchar)
    returns integer as
    $body$
    declare
        tableNameNonQuoted varchar;
        script text;
        count int;
        columnType varchar;
        columnToDelete text;
    begin
        tableNameNonQuoted := tablePrefix || 'LoanPolicySaga';
        script = 'create table if not exists "' || schema || '"."' || tableNameNonQuoted || '"
(
    "Id" uuid not null,
    "Metadata" text not null,
    "Data" jsonb not null,
    "PersistenceVersion" character varying(23),
    "SagaTypeVersion" character varying(23),
    "Concurrency" int not null,
    primary key("Id")
);';
        execute script;

/* AddProperty LoanId */

        script = 'alter table "' || schema || '"."' || tableNameNonQuoted || '" add column if not exists "Correlation_LoanId" uuid';
        execute script;

/* VerifyColumnType Guid */

        columnType := (
            select data_type
            from information_schema.columns
            where
            table_schema = schema and
            table_name = tableNameNonQuoted and
            column_name = 'Correlation_LoanId'
        );
        if columnType <> 'uuid' then
            raise exception 'Incorrect data type for Correlation_LoanId. Expected "uuid" got "%"', columnType;
        end if;

/* WriteCreateIndex LoanId */

        script = 'create unique index if not exists "' || tablePrefix || '_i_3E62ACD6578A4332DA607CC80350906564D6033A" on "' || schema || '"."' || tableNameNonQuoted || '" using btree ("Correlation_LoanId" asc);';
        execute script;
/* PurgeObsoleteIndex */

/* PurgeObsoleteProperties */

for columnToDelete in
(
    select column_name
    from information_schema.columns
    where
        table_name = tableNameNonQuoted and
        column_name LIKE 'Correlation_%' and
        column_name <> 'Correlation_LoanId'
)
loop
	script = '
alter table "' || schema || '"."' || tableNameNonQuoted || '"
drop column "' || columnToDelete || '"';
    execute script;
end loop;

/* CompleteSagaScript */

        return 0;
    end;
    $body$
language 'plpgsql';

select pg_temp.create_saga_table_LoanPolicySaga(@tablePrefix, @schema);
