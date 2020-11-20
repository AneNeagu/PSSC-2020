
-------------------------------------------------------------------------------------------
/*

	Written By:	Neil Reynolds
	Date:		01/10/2020
	Reason:		Admin Security Predicate
				Takes TenantId, UserId is passed in via Session Context
				Checks IsAdmin flag for user 			
*/
-------------------------------------------------------------------------------------------

CREATE FUNCTION [security].[fn_adminsecuritypredicate]
(
	@tenantId int
)
RETURNS TABLE WITH Schemabinding
As

	RETURN SELECT fn_adminsecuritypredicate_result FROM
    (
		SELECT 1 AS fn_adminsecuritypredicate_result
		FROM
			base.TenantUser tu
			INNER JOIN base.[User] u ON u.Userid = tu.UserId
		WHERE
			
			CAST(SESSION_CONTEXT(N'UserId') AS UniqueIdentifier) = tu.UserId
			AND tu.TenantId = @tenantId
			AND u.Isadmin = 1

	) AS U
	WHERE U.fn_adminsecuritypredicate_result = 1

Go