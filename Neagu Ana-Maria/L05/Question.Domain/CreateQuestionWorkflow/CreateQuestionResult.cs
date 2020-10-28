using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using CSharp.Choices;
using Profile.Domain.CreateProfileWorkflow;

using static Profile.Domain.CreateProfileWorkflow.CreateProfileResult;

namespace Question.Domain.CreateQuestionWorkflow
{
    [AsChoice]
    public static partial class CreateQuestionResult
    {
        public interface ICreateQuestionResult { }

        public class QuestionCreated : ICreateQuestionResult
        {

            public Guid QuestionId { get; private set; }
            public string Title { get; private set; }
            public string Body { get; private set; }
            public string[] Tags { get; private set; }
            public int VoteCount { get; private set; }
            public IReadOnlyCollection<VoteEnum> AllVotes { get; private set; }


            public QuestionCreated(Guid questionId, string title, string body, string[] tags, int votes)
            {

                this.QuestionId = questionId;
                this.Title = title;
                this.Body = body;
                this.Tags = tags;
                this.VoteCount = votes;

            }
            public class QuestionNotCreated : ICreateQuestionResult
            {
                public string Reason { get; set; }

                public QuestionNotCreated(string reason)
                {
                    Reason = reason;
                }
            }

            public class QuestionValidationFailed : ICreateQuestionResult
            {
                public IEnumerable<string> ValidationErrors { get; private set; }

                public QuestionValidationFailed(IEnumerable<string> errors)
                {
                    ValidationErrors = errors.AsEnumerable();
                }
            }


        }
        
    }
}
