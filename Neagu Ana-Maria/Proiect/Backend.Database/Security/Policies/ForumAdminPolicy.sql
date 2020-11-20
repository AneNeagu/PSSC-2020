CREATE SECURITY POLICY [security].[ForumAdminPolicy]

--Admin policies based on TenantId

ADD FILTER PREDICATE [security].[fn_adminsecuritypredicate](TenantId) ON [Admin].[Tenant],
ADD FILTER PREDICATE [security].[fn_adminsecuritypredicate](TenantId) ON [Admin].[TenantUser],

-- Filter for User view that does not contain the TenantId
ADD FILTER PREDICATE [security].[fn_adminusermaintenance](UserId) ON [Admin].[User]



WITH (STATE = ON, SCHEMABINDING = ON)