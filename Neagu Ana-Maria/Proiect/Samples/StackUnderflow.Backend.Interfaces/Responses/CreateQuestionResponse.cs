using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Backend.Abstractions.Responses
{
    public class CreateQuestionResponse
    {
        public bool Successful { get; set; }
        public int QuestionId { get; set; }
        public string FailureText { get; set; }

        public CreateQuestionResponse(bool successful, int questionId, string failureText)
        {
            Successful = successful;
            QuestionId = questionId;
            FailureText = failureText;
        }
    }
}
