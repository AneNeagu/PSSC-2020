-- Begin - Drop RLS
CREATE PROCEDURE base.DropRLS
    @filterSufix varchar(100)
AS 
BEGIN
DECLARE @sql2 VARCHAR(MAX) = ''
        , @crlf2 VARCHAR(2) = CHAR(13) + CHAR(10) ;

SELECT @sql2 = @sql2 + 'DROP SECURITY POLICY ' + QUOTENAME(SCHEMA_NAME(schema_id)) + '.' + QUOTENAME(sp.name) +';' + @crlf2
FROM   sys.security_policies sp 
WHERE sp.name like '%' + @filterSufix

PRINT @sql2;
EXEC(@sql2);
END

GO
-- End - Drop RLS
-- Begin - Drop Views 

CREATE PROCEDURE base.DropViewsSp
    @schema varchar(100)
AS 
BEGIN

DECLARE @sql VARCHAR(MAX) = ''
DECLARE @crlf VARCHAR(2) = CHAR(13) + CHAR(10);




;WITH allviews as
( --just combining schema and name
SELECT
    object_id,
    '[' + SCHEMA_NAME(schema_id) + '].[' + name + ']' AS viewname
FROM sys.views 
WHERE SCHEMA_NAME(schema_id) = @schema
),
dependents AS
( 
SELECT
    referencing.viewname dependentname,
    referenced.viewname dependenton
FROM sys.sql_expression_dependencies r
    INNER JOIN allviews referencing
        ON referencing.object_id = r.referencing_id
    INNER JOIN allviews referenced
        ON referenced.object_id = r.referenced_id
)
,
nodependents 
AS
( 
SELECT
    viewname name
FROM allviews v
    LEFT JOIN dependents d
        ON d.dependentname = viewname
WHERE d.dependentname IS NULL
)
,hierarchy AS
( --the hierarchy recurses the dependencies
SELECT
    d.dependenton,
    d.dependentname,
    1 tier
FROM dependents d UNION ALL SELECT
    d.dependenton,
    d.dependentname,
    h.tier + 1
FROM dependents d
    INNER JOIN hierarchy h
        ON h.dependenton = d.dependentname
--best thing I could think to stop the recursion was to 
--stop when we reached an item with no dependents       
WHERE h.dependenton NOT IN (SELECT
    name
FROM nodependents)
    ),
combined as
( --need to add item with no dependents back in
SELECT
    0 tier,
    name
FROM nodependents UNION SELECT
    tier,
    dependentname
FROM hierarchy  
)
SELECT
    @sql = @sql + 'DROP VIEW ' + name + ';' + @crlf
FROM combined
GROUP BY name --need to group because of multiple dependency paths
ORDER BY MAX(tier) desc

PRINT @sql;

--commented out until I'm confident I want to run it
EXEC(@sql)

END

GO
EXEC base.DropRLS 'Policy'
--EXEC base.DropRLS 'EmployeeFilter'
GO
EXEC base.DropViewsSp 'Admin'
EXEC base.DropViewsSp 'User'
EXEC base.DropViewsSp 'base'
GO

DROP PROCEDURE base.DropViewsSp
DROP PROCEDURE base.DropRLS
-- End - Drop Views 
