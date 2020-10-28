using System;

namespace Profile.Domain.CreateProfileWorkflow
{
    [Serializable]
    public class InvalidQuestionException : Exception
    {
        public InvalidQuestionException()
        {
        }

        public InvalidQuestionException(string question) : base($"The value \"{question}\" is invalid question format.")
        {
        }

    }
}
