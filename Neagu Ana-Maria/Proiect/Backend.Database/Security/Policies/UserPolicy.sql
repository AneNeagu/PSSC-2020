CREATE SECURITY POLICY [security].[UserPolicy]

--User policies based on TenantId
ADD FILTER PREDICATE [security].[fn_usersecuritypredicate](TenantId) ON [user].[Post],
ADD FILTER PREDICATE [security].[fn_usersecuritypredicate](TenantId) ON [user].[PostTag],
ADD FILTER PREDICATE [security].[fn_usersecuritypredicate](TenantId) ON [user].[PostView],
ADD FILTER PREDICATE [security].[fn_usersecuritypredicate](TenantId) ON [user].[UserBadge],
ADD FILTER PREDICATE [security].[fn_usersecuritypredicate](TenantId) ON [user].[Vote],

--User policy based on UserId
ADD FILTER PREDICATE [security].[fn_usermaintenancesecuritypredicate](UserId) ON [user].[User]

WITH (STATE = ON, SCHEMABINDING = ON)