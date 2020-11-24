using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Domain.Schema.Questions.SendReplyAuthorAcknowledgementOp
{
    public class SendReplyAuthorAcknowledgementCmd
    {
        public SendReplyAuthorAcknowledgementCmd(Guid replyAuthorId, int questionId, int answerId)
        {
            ReplyAuthorId = replyAuthorId;
            QuestionId = questionId;
            AnswerId = answerId;
        }

        public Guid ReplyAuthorId { get; }
        public int QuestionId { get; }
        public int AnswerId { get; }
    }
}
