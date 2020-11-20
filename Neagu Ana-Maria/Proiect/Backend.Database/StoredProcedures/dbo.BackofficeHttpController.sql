CREATE PROCEDURE dbo.BackofficeHttpController
	@OrganisationId uniqueidentifier
AS
BEGIN
	SELECT * FROM dbo.Tenant WHERE [OrganisationId] = @OrganisationId

	SELECT TU.* FROM dbo.TenantUser TU
	INNER JOIN dbo.Tenant T on TU.TenantId = T.TenantId
	WHERE T.[OrganisationId] = @OrganisationId

	SELECT * FROM dbo.[User] U
	INNER JOIN dbo.TenantUser TU on U.UserId = TU.UserId
	INNER JOIN dbo.Tenant T on TU.TenantId = T.TenantId
	WHERE T.[OrganisationId] = @OrganisationId
END