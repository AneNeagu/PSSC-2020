using LanguageExt.Common;
using System;
using System.Collections.Generic;
using System.Text;
using static Question.Domain.CreateQuestionWorkflow.Votes;


namespace Question.Domain.CreateQuestionWorkflow
{
    public class VerifyVotesService
    {
        public Result<VerifiedVotes> VerifyVotes(UnverifiedVotes votes)
        {
            //send verification link
            //

            return new VerifiedVotes(votes);
        }
    }
}
