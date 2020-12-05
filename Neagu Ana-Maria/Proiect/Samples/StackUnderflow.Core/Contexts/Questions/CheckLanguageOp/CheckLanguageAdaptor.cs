using Access.Primitives.IO;
using StackUnderflow.Domain.Schema.Questions.CheckLanguageOp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static StackUnderflow.Domain.Schema.Questions.CheckLanguageOp.CheckLanguageResult;

namespace StackUnderflow.Domain.Core.Contexts.Questions.CheckLanguageOp
{
    public class CheckLanguageAdaptor : Adapter<CheckLanguageCmd, ICheckLanguageResult, QuestionsWriteContext, QuestionsDependencies>
    {
        public override Task PostConditions(CheckLanguageCmd cmd, ICheckLanguageResult result, QuestionsWriteContext state)
        {
            return Task.CompletedTask;
        }

        public async override Task<ICheckLanguageResult> Work(CheckLanguageCmd cmd, QuestionsWriteContext state, QuestionsDependencies dependencies)
        {
            return new ValidationSucceeded("Valid");
        }
    }
}