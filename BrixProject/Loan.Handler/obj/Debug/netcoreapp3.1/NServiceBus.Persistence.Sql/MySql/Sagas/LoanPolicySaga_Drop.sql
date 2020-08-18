
/* TableNameVariable */

set @tableNameQuoted = concat('`', @tablePrefix, 'LoanPolicySaga`');
set @tableNameNonQuoted = concat(@tablePrefix, 'LoanPolicySaga');


/* DropTable */

set @dropTable = concat('drop table if exists ', @tableNameQuoted);
prepare script from @dropTable;
execute script;
deallocate prepare script;
