using StackUnderflow.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Backend.Abstractions.Responses
{
    public class VoteResponse
    {
        public VoteResponse(bool successful, string failureText)
        {
            Successful = successful;
            FailureText = failureText;
        }

        public bool Successful { get; }
        public string FailureText { get; }
    }
}
