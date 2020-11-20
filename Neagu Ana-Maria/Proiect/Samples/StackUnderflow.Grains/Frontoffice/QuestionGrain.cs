using Access.Primitives.EFCore;
using Access.Primitives.IO;
using Microsoft.EntityFrameworkCore;
using Orleans;
using Orleans.Streams;
using StackUnderflow.Backend.Abstractions.Fontoffice;
using StackUnderflow.Backend.Abstractions.Responses;
using StackUnderflow.Domain.Core.Contexts;
using StackUnderflow.Domain.Core.Contexts.FrontOffice;
using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StackUnderflow.Domain.Schema.Voting.VoteResult;

namespace StackUnderflow.Backend.Grains.Frontoffice
{
    public class QuestionGrain : Grain, IQuestionGrain
    {

        private readonly IInterpreterAsync _interpreter;
        private readonly StackUnderflowContext _dbContext;
        private QuestionsData _ctx;
        private QuestionGrainId _questionGrainId;
        private int _tenantId;

        public QuestionGrain(IInterpreterAsync interpreter, StackUnderflowContext dbContext)
        {
            _interpreter = interpreter;
            //_dbContext = new StackUnderflowContext();
            _dbContext = dbContext;
        }

        public override async Task OnActivateAsync()
        {

            _questionGrainId = QuestionGrainId.FromString(this.GetPrimaryKeyString());
            _tenantId = _dbContext.Tenant.AsNoTracking().First(t => t.OrganisationId == _questionGrainId.OrganisationId).TenantId;

            await _dbContext.Post.Where(p => p.PostId == _questionGrainId.QuestionId)
                .Include(p => p.InversePostNavigation) // answers and comments
                .Include(p => p.Vote) // votes
                .LoadAsync();

            _ctx = new QuestionsData(
                 new EFList<Post>(_dbContext.Post)
                );
        }
    }
}
