using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Domain.Schema.Questions.SendQuestionOwnerAcknowledgementOp
{
    public class SendQuestionOwnerAcknowledgementCmd
    {
        public SendQuestionOwnerAcknowledgementCmd(int questionId, int questionOwnerId)
        {
            QuestionId = questionId;
            QuestionOwnerId = questionOwnerId;
        }

        public int QuestionId { get; }
        public int QuestionOwnerId { get; }
    }
}
