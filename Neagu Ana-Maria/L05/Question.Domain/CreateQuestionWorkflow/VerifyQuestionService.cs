using LanguageExt.Common;
using System;
using System.Collections.Generic;
using System.Text;
using static Profile.Domain.CreateProfileWorkflow.EmailAddress;
using static Question.Domain.Question;

namespace Profile.Domain.CreateProfileWorkflow
{
    public class VerifyQuestionService
    {
        public Result<VerifiedQuestion> VerifyQuestion(UnverifiedQuestion question)
        {
            //send verification link
            //

            return new VerifiedQuestion(question.Title);
        }
    }
}
