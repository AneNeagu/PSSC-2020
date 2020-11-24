using Access.Primitives.Extensions.ObjectExtensions;
using Access.Primitives.IO;
using StackUnderflow.Domain.Core.Contexts.Questions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static StackUnderflow.Domain.Core.Contexts.Questions.CreateQuestionsOp.CreateQuestionsResult;

namespace StackUnderflow.Domain.Core.Contexts.Questions.CreateQuestionsOp
{
    class CreateQuestionsAdaptor : Adapter<CreateQuestionsCmd, ICreateQuestionsResult, QuestionsWriteContext, QuestionsDependencies>
    {
        public override Task PostConditions(CreateQuestionsCmd cmd, ICreateQuestionsResult result, QuestionsWriteContext state)
        {
            return Task.CompletedTask;
        }

        public async override Task<ICreateQuestionsResult> Work(CreateQuestionsCmd cmd, QuestionsWriteContext state, QuestionsDependencies dependencies)
        {
            var workflow = from valid in cmd.TryValidate()
                           let t = AddQuestion(state, CreateQuestionsFromCmd(cmd))
                           select t;
            var result = await workflow.Match(
                Succ: r => r,
                Fail: er => new QuestionNotCreated(er.Message)
                );

            return result;
        }

        private ICreateQuestionsResult AddQuestion(QuestionsWriteContext state, object v)
        {
            return new QuestionCreated(new Guid("1"), "Titlu", "Descriere", "Tag-uri");
        }

        private object CreateQuestionsFromCmd(CreateQuestionsCmd cmd)
        {
            return new { };
        }
    }
}