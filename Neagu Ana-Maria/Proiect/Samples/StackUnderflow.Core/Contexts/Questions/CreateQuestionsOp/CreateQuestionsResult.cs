using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Domain.Core.Contexts.Questions.CreateQuestionsOp
{
    [AsChoice]
    public static partial class CreateQuestionsResult
    {
        public interface ICreateQuestionsResult { }

        public class QuestionCreated : ICreateQuestionsResult
        {
            public Guid QuestionId { get; private set; }
            public string Title { get; private set; }
            public string Description { get; private set; }
            public string Tags { get; private set; }

            public QuestionCreated(Guid QuestionId, string Title, string Description, string Tags)
            {
                this.QuestionId = QuestionId;
                this.Title = Title;
                this.Description = Description;
                this.Tags = Tags;
            }
        }

        public class QuestionNotCreated : ICreateQuestionsResult
        {
            public string Message { get; }
            public QuestionNotCreated(string message)
            {
                Message = message;
            }
        }
    }
}