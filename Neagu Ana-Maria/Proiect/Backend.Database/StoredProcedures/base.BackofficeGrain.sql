CREATE PROCEDURE [base].BackofficeGrain
AS
BEGIN
	SELECT * FROM [base].Tenant
	SELECT * FROM [base].TenantUser
	SELECT * FROM [base].[User]
END