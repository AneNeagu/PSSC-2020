using System;
using System.Collections.Generic;
using System.Text;
using Access.Primitives.IO;
using LanguageExt;
using StackUnderflow.Domain.Schema.Backoffice.CreateTenantOp;
using StackUnderflow.Domain.Schema.Backoffice.InviteTenantAdminOp;
using static PortExt;
using static StackUnderflow.Domain.Schema.Backoffice.CreateTenantOp.CreateTenantResult;
using static StackUnderflow.Domain.Schema.Backoffice.InviteTenantAdminOp.InviteTenantAdminResult;

namespace StackUnderflow.Domain.Core
{
    public static class BackofficeDomain
    {
        public static Port<ICreateTenantResult> CreateTenant(CreateTenantCmd command)
        {
            return NewPort<CreateTenantCmd, ICreateTenantResult>(command);
        }

        public static Port<IInviteTenantAdminResult> InviteTenantAdmin(InviteTenantAdminCmd command) => NewPort<InviteTenantAdminCmd, IInviteTenantAdminResult>(command);
    }
}

