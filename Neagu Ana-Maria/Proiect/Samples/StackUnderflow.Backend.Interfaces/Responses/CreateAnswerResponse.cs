using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Backend.Abstractions.Responses
{
    public class CreateAnswerResponse
    {
        public bool Successful { get; set; }
        public int AnswerId { get; set; }
        public string FailureText { get; set; }

        public CreateAnswerResponse(bool successful, int answerId, string failureText)
        {
            Successful = successful;
            AnswerId = answerId;
            FailureText = failureText;
        }
    }
}
