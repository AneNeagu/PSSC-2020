using Access.Primitives.Extensions.Cloning;
using CSharp.Choices;
using StackUnderflow.EF.Models;

namespace StackUnderflow.Domain.Schema.Backoffice.InviteTenantAdminOp
{
    [AsChoice]
    public static partial class InviteTenantAdminResult
    {
        public interface IInviteTenantAdminResult { }

        public class TenantAdminInvited : IInviteTenantAdminResult
        {
            public User AdminUser { get; }
            public string ActivationCode { get; }

            public string InvitationAcknowlwedgement { get; set; }

            public TenantAdminInvited(User adminUser, string activationCode, string invitationAcknowledgement)
            {
                AdminUser = adminUser;
                ActivationCode = activationCode;
                InvitationAcknowlwedgement = invitationAcknowledgement;
            }
            ///TODO
        }

        public class TenantAdminNotInvited : IInviteTenantAdminResult
        {
            ///TODO
        }

        public class InvalidRequest : IInviteTenantAdminResult
        {
            public string Message { get; }

            public InvalidRequest(string message)
            {
                Message = message;
            }

        }
    }
}