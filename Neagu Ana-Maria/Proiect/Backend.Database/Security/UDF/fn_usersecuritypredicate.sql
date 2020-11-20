CREATE FUNCTION [security].[fn_usersecuritypredicate]
(
	@tenantId int
)
RETURNS TABLE WITH Schemabinding
As

	RETURN SELECT fn_usersecuritypredicate_result FROM
    (
		SELECT 1 AS fn_usersecuritypredicate_result
		FROM
			base.TenantUser tu
		WHERE
			
			CAST(SESSION_CONTEXT(N'UserId') AS UniqueIdentifier) = tu.UserId
			AND tu.TenantId = @tenantId

	) AS U
	WHERE U.fn_usersecuritypredicate_result = 1

