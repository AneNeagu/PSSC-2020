namespace StackUnderflow.Domain.Schema.Backoffice.InviteTenantAdminOp
{
    public class InvitationAcknowledgement
    {
        public string Receipt { get; private set; }

        public InvitationAcknowledgement(string receipt)
        {
            Receipt = receipt;
        }
    }
}