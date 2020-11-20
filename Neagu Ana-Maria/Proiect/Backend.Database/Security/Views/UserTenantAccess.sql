CREATE VIEW [admin].[UserTenantAccess]
AS
SELECT 
	t.TenantId

FROM 
	base.Tenant AS t
	INNER JOIN base.TenantUser AS tu ON tu.TenantId = t.TenantId
	INNER JOIN base.[user] AS u ON u.UserId = tu.UserId
