using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Access.Primitives.IO;
using StackUnderflow.Domain.Core.Contexts;
using StackUnderflow.Domain.Schema.Backoffice.InviteTenantAdminOp;
using static StackUnderflow.Domain.Schema.Backoffice.InviteTenantAdminOp.InviteTenantAdminResult;
using Access.Primitives.IO.Mocking;
using Access.Primitives.Extensions.ObjectExtensions;
using StackUnderflow.Domain.Schema.Backoffice;
using StackUnderflow.EF.Models;

namespace StackUnderflow.Adapters.InviteAdmin
{
    public partial class InviteTenantAdminAdapter : Adapter<InviteTenantAdminCmd, IInviteTenantAdminResult, BackofficeWriteContext, BackofficeDependencies>
    {

        public InviteTenantAdminAdapter()
        {
        }

        public override async Task<IInviteTenantAdminResult> Work(InviteTenantAdminCmd command, BackofficeWriteContext state, BackofficeDependencies dependencies)
        {
            var wf = from isValid in command.TryValidate()
                     from user in command.AdminUser.ToTryAsync()
                     let token = dependencies.GenerateInvitationToken()
                     let letter = GenerateInvitationLetter(user, token)
                     from invitationAck in dependencies.SendInvitationEmail(letter)
                     select (user, token, invitationAck);

            return await wf.Match(
                Succ: r => new TenantAdminInvited(r.user, r.token, r.invitationAck.Receipt),
                Fail: ex => (IInviteTenantAdminResult)new InvalidRequest(ex.ToString()));
        }

        private InvitationLetter GenerateInvitationLetter(User user, string token)
        {
            var link = $"https://stackunderflow/invite/{token}";
            var letter = @$"Dear {user.DisplayName}Please click on {link}";
            return new InvitationLetter(user.Email, letter, new Uri(link));
        }
        
        public override Task PostConditions(InviteTenantAdminCmd cmd, IInviteTenantAdminResult result, BackofficeWriteContext state)
        {
            return Task.CompletedTask;
        }
    }
}
