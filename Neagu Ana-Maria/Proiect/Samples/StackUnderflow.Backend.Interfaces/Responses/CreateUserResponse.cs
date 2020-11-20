using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Backend.Abstractions.Responses
{
    public class InviteUserResponse
    {
        public bool Successful { get; }
        public Guid UserId { get; }
        public string UserName { get; }

        public InviteUserResponse(bool successful, Guid userId, string userName)
        {
            Successful = successful;
            UserId = userId;
            UserName = userName;
        }
    }
}
