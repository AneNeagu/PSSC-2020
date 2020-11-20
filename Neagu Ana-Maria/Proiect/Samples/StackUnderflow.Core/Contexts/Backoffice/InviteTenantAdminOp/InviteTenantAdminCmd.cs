using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Access.Primitives.IO;
using EarlyPay.Primitives.ValidationAttributes;
using LanguageExt;
using StackUnderflow.EF.Models;

namespace StackUnderflow.Domain.Schema.Backoffice.InviteTenantAdminOp
{
    public struct InviteTenantAdminCmd
    {
        [OptionValidator(typeof(RequiredAttribute))]
        public Option<User> AdminUser { get; }

        public InviteTenantAdminCmd(Option<User> adminUser)
        {
            AdminUser = adminUser;
        }
    }

    public enum InviteTenantAdminCmdInput
    {
        Valid,
        UserIsNone
    }

    public class InviteTenantAdminInputGen : InputGenerator<InviteTenantAdminCmd, InviteTenantAdminCmdInput>
    {
        public InviteTenantAdminInputGen()
        {
            mappings.Add(InviteTenantAdminCmdInput.Valid, () =>
                new InviteTenantAdminCmd(
                    Option<User>.Some(new User()
                    {
                        DisplayName = Guid.NewGuid().ToString(),
                        Email = $"{Guid.NewGuid()}@mailinator.com"
                    }))
            );

            mappings.Add(InviteTenantAdminCmdInput.UserIsNone, () =>
                new InviteTenantAdminCmd(
                    Option<User>.None
                    )
            );
        }
    }
}
