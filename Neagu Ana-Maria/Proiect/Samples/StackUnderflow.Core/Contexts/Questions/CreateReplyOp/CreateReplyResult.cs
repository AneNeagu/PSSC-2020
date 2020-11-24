using CSharp.Choices;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Domain.Schema.Questions.CreateAnswerOp
{
    [AsChoice]
    public static partial class CreateReplyResult
    {
        public interface ICreateReplyResult { }

        public class ReplyCreated : ICreateReplyResult
        {
            public ReplyCreated(int replyId, int questionId, int authorUserId, string body)
            {
                ReplyId = replyId;
                QuestionId = questionId;
                AuthorUserId = authorUserId;
                Body = body;
            }

            public int ReplyId { get; }
            public int QuestionId { get; }
            public int AuthorUserId { get; }
            public string Body { get; }
        }

        public class ReplyNotCreated : ICreateReplyResult
        {
            public ReplyNotCreated(string message)
            {
                Message = message;
            }

            public string Message { get; }
        }
    }

    
}
