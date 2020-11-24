using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Domain.Schema.Questions.SendReplyAuthorAcknowledgementOp
{
    [AsChoice]
    public static partial class SendReplyAuthorAcknowledgementResult
    {

        public interface ISendReplyAuthorAcknowledgementResult { }

        public class AcknowledgementSent : ISendReplyAuthorAcknowledgementResult
        {
            public AcknowledgementSent(int questionId, int questionOwnerId)
            {
                QuestionId = questionId;
                QuestionOwnerId = questionOwnerId;
            }

            public int QuestionId { get; }
            public int QuestionOwnerId { get; }
        }

        public class AcknowledgementNotSent : ISendReplyAuthorAcknowledgementResult
        {
            public AcknowledgementNotSent(string message)
            {
                Message = message;
            }

            public string Message { get; }
        }
    }
}
