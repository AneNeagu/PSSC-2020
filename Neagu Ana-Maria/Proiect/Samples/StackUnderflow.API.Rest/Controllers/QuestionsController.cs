using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access.Primitives.Extensions.ObjectExtensions;
using Access.Primitives.IO;
using Microsoft.AspNetCore.Mvc;
using StackUnderflow.Domain.Core;
using StackUnderflow.Domain.Core.Contexts;
using StackUnderflow.Domain.Schema.Backoffice.CreateTenantOp;
using StackUnderflow.EF.Models;
using Access.Primitives.EFCore;
using StackUnderflow.Domain.Schema.Backoffice.InviteTenantAdminOp;
using StackUnderflow.Domain.Schema.Backoffice;
using LanguageExt;
using StackUnderflow.Domain.Schema.Questions.CreateAnswerOp;
using StackUnderflow.Domain.Core.Contexts.Questions;
using StackUnderflow.EF;
using Microsoft.EntityFrameworkCore;
using StackUnderflow.Domain.Core.Contexts.Questions.CreateQuestionsOp;
using StackUnderflow.DatabaseModel.Models;
using Orleans;
using StackUnderflow.Domain.Schema.Questions.CheckLanguageOp;
using StackUnderflow.Domain.Schema.Questions.SendReplyAuthorAcknowledgementOp;
using StackUnderflow.Domain.Schema.Questions.SendQuestionOwnerAcknowledgementOp;

namespace StackUnderflow.API.Rest.Controllers
{
    [ApiController]
    [Route("questions")]
    public class QuestionsController : ControllerBase
    {
        private readonly IInterpreterAsync _interpreter;
        private readonly DatabaseContext _dbContext;

        public QuestionsController(IInterpreterAsync interpreter, DatabaseContext dbContext)
        {
            _interpreter = interpreter;
            _dbContext = dbContext;
        }

        [HttpPost("createReply")]
        public async Task<IActionResult> CreateReply([FromBody] CreateReplyCmd cmd)
        {
            var dep = new QuestionsDependencies();
            var replies = await _dbContext.Replies.ToListAsync();

            //var ctx = new QuestionsWriteContext(replies);
            _dbContext.Replies.AttachRange(replies);

            var ctx = new QuestionsWriteContext(new EFList<Reply>(_dbContext.Replies));

            var expr = from createTenantResult in QuestionsContext.CreateReply(cmd)
                       from checkLanguageResult in QuestionsContext.CheckLanguage(new CheckLanguageCmd(cmd.Body))
                       from sendAckAuthor in QuestionsContext.SendReplyAuthorAcknowledgement(new SendReplyAuthorAcknowledgementCmd(Guid.NewGuid(), 1, 2))
                       select createTenantResult;


            var r = await _interpreter.Interpret(expr, ctx, dep);
            //_dbContext.Replies.Add(new DatabaseModel.Models.Reply { Body = cmd.Body, AuthorUserId = 1, QuestionId = cmd.QuestionId, ReplyId = 4 });
            //var reply = await _dbContext.Replies.Where(r => r.ReplyId == 4).SingleOrDefaultAsync();
            //reply.Body = "Text updated";
            //_dbContext.Replies.Update(reply);
            await _dbContext.SaveChangesAsync();


            return r.Match(
                succ => (IActionResult)Ok(succ.Body),
                fail => BadRequest("Reply could not be added")
                );
        }

        [HttpPost("createQuestion")]
        public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionsCmd cmd)
        {
            var dep = new QuestionsDependencies();

            var questions = await _dbContext.QuestionModel.ToListAsync();

            //var ctx = new QuestionsWriteContext(questions);
            _dbContext.QuestionModel.AttachRange(questions);
            var ctx = new QuestionsWriteContext(new EFList<QuestionModel>(_dbContext.QuestionModel));

            // var expr = from createTenantResult in QuestionsContext.CreateQuestion(cmd)
            // select createTenantResult;
            var expr = from CreateQuestionResult in QuestionsContext.CreateQuestion(cmd)
                           //let checkLanguageCmd = new CheckLanguageCmd()
                           //select CreateQuestionResult;
                       from checkLanguageResult in QuestionsContext.CheckLanguage(new CheckLanguageCmd(cmd.Description))
                       from sendAckToQuestionOwnerCmd in QuestionsContext.SendQuestionOwnerAcknowledgement(new SendQuestionOwnerAcknowledgementCmd(1, 2))
                       select CreateQuestionResult;
            var r = await _interpreter.Interpret(expr, ctx, dep);

            await _dbContext.SaveChangesAsync();

            _dbContext.QuestionModel.Add(new DatabaseModel.Models.QuestionModel { Id= cmd.QuestionId, Title = cmd.Title, Description = cmd.Description, Tags = cmd.Tags });
            await _dbContext.SaveChangesAsync();
            // var reply = await _dbContext.QuestionModel.Where(r => r.Id == 1).SingleOrDefaultAsync();
            //_dbContext.QuestionModel.Update(reply);

            return r.Match(
                succ => (IActionResult)Ok("Succeeded"),
                fail => BadRequest("Reply could not be added")
                );
        }
    }
}
