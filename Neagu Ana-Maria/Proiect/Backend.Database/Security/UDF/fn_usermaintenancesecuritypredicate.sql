CREATE FUNCTION [security].[fn_usermaintenancesecuritypredicate]
(
	@UserId AS UNIQUEIDENTIFIER
)
RETURNS TABLE WITH Schemabinding
As

	RETURN SELECT 1 AS fn_usermaintenancesecuritypredicate_result 
	WHERE CAST(SESSION_CONTEXT(N'UserId') AS UniqueIdentifier) = @UserId

