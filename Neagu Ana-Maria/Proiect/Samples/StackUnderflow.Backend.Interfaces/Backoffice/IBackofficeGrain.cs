using System;
using System.Threading.Tasks;
using Orleans;
using StackUnderflow.Backend.Abstractions.Responses;
using StackUnderflow.Backend.Interfaces.Responses;

namespace StackUnderflow.Backend.Interfaces
{
    public interface IBackofficeGrain : IGrainWithGuidCompoundKey
    {
        Task<CreateTenantAndAdminResponse> CreateTenantAndAdmin(Guid organsiationId, string tenantName, string description, string adminEmail, string adminName, Guid userId);

        Task<InviteUserResponse> InviteUser(Guid organsiationId, Guid userId, string email, string userName);
    }
}
