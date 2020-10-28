using CSharp.Choices;
using Renci.SshNet.Common;
using LanguageExt.Common;

namespace Question.Domain.CreateQuestionWorkflow
{
    [AsChoice]
    public static partial class Votes
    {
        public interface IVotes { }
        public class UnverifiedVotes : IVotes
        {
            public int Votes { get; private set; }
            private UnverifiedVotes(int votes)
            {
                Votes = votes;
            }

            public static Result<UnverifiedVotes> Create(int votes)
            {
                if (IsVotesValid(votes))
                {
                    return new UnverifiedVotes(votes);
                }
                else
                {
                    return new Result<UnverifiedVotes>(new InvalidVotesException(votes));
                }
            }
            private static bool IsVotesValid(int votes)
            {
                if (Votes == votes)
                {
                    return true;
                }
                return false;
            }
        }
        public class VerifiedVotes : IVotes
        {
            public int Votes { get; private set; }
            internal VerifiedVotes(int votes)
            {
                Votes = votes;
            }
        }
    }
}

