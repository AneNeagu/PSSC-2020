using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Backend.Interfaces.Responses
{
    // This project doesn't have the proper naming - Interfaces
    // This is the reference that should be used by whoever consumes Orleans Grains and should contain abstractions exclusively.
    
   
    public class CreateTenantAndAdminResponse
    {
        public bool Successful { get; }
        public int TenantId { get; }
        public string AdminName { get; }

        public CreateTenantAndAdminResponse(bool successful, int tenantId, string adminName)
        {
            Successful = successful;
            TenantId = tenantId;
            AdminName = adminName;
        }
    }
}
