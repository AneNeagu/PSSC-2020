CREATE FUNCTION [security].[fn_adminusermaintenance](@UserId AS UNIQUEIDENTIFIER)
RETURNS TABLE
WITH SCHEMABINDING
AS
RETURN SELECT fn_adminusermaintenance_result
       FROM
              (
                     SELECT TOP(1) 1 AS fn_adminusermaintenance_result
                     FROM
                            base.TenantUser AS tu
                            INNER JOIN
                            (SELECT tu2.TenantId 
                            FROM base.[User] AS u 
                                 INNER JOIN base.TenantUser AS tu2 ON tu2.UserId=u.UserId 
                            WHERE u.UserId=CAST(SESSION_CONTEXT(N'UserId') AS UNIQUEIDENTIFIER)AND u.IsAdmin=1) AS CurrentUser ON CurrentUser.TenantId=tu.TenantId
                      WHERE tu.UserID = @UserId
                     ) AS U
      WHERE
                U.fn_adminusermaintenance_result=1
