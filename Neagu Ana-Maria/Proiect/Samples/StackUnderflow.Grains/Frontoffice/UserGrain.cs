using Access.Primitives.EFCore;
using Access.Primitives.IO;
using Microsoft.EntityFrameworkCore;
using Orleans;
using Orleans.Streams;
using StackUnderflow.Backend.Abstractions;
using StackUnderflow.Backend.Abstractions.FrontOffice;
using StackUnderflow.Backend.Abstractions.Responses;
using StackUnderflow.Backend.Interfaces;
using StackUnderflow.Domain.Core.Contexts;
using StackUnderflow.Domain.Core.Contexts.FrontOffice;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using LanguageExt;
using StackUnderflow.EF;

namespace StackUnderflow.Backend.Grains.Frontoffice
{
    public class UserGrainId
    {
        public Guid OrganisationId { get; }
        public Guid UserId { get; }
        public string Suffix { get; }

        public UserGrainId(Guid organisationId, Guid userId, string suffix)
        {
            OrganisationId = organisationId;
            UserId = userId;
            Suffix = suffix;
        }

        public static UserGrainId FromString(string id)
        {
            var split = id.Split('/');
            return new UserGrainId(Guid.Parse(split[0]), Guid.Parse(split[1]), split[2]);
        }

        public override string ToString()
        {
            return $"{OrganisationId}/{UserId}/{Suffix}";
        }
    }

    public class UserGrain : Grain, IUserGrain
    {

        private readonly IInterpreterAsync _interpreter;
        private readonly Port<UserDbContext> _dbContextPort;
        private StackUnderflowContext _dbContext;
        private UserWriteContext _ctx;
        private UserGrainId _userGrainId;

        public UserGrain(IInterpreterAsync interpreter, Port<UserDbContext> dbContextPort)
        {
            _interpreter = interpreter;
            _dbContextPort = dbContextPort;
        }

        public override async Task OnActivateAsync()
        {
            _dbContext = await _interpreter.Interpret(_dbContextPort, Unit.Default);
            _userGrainId = UserGrainId.FromString(this.GetPrimaryKeyString());

            await _dbContext.User.Where(u => u.UserId == _userGrainId.UserId)
                .Include(u => u.TenantUser).ThenInclude(tu => tu.Vote)
                .Include(u => u.TenantUser).ThenInclude(tu => tu.PostTenantUser)
                .LoadAsync();

            _ctx = new UserWriteContext(
                new EFList<User>(_dbContext.User)
            );
        }
    }
}
