using CSharp.Choices;
using LanguageExt.Common;
using OpenTracing.Tag;
using Profile.Domain.CreateProfileWorkflow;
using Question.Domain.CreateQuestionWorkflow;
using System;
using System.Collections.Generic;
using System.Text;

namespace Question.Domain
{
    [AsChoice]
    public static partial class Question
    {

        public interface IQuestion { }

        public class UnverifiedQuestion : IQuestion
        {
            public string Title { get; private set; }
            public string Tags { get; private set; }
            

            private UnverifiedQuestion(string question,string tags)
            {
                Title = question;
                Tags = tags;
               
            }

            public static Result<UnverifiedQuestion> Create(string question,string tags)
            {
                if (IsQuestionValid(question))
                {
                    if (IsTagValid(tags))
                    {
                        return new UnverifiedQuestion(question, tags);
                    }
                    else
                    {
                        return new Result<UnverifiedQuestion>(new InvalidQuestionException($"The numbers of tags must be between 1 and 3"));
                    }
                }
                else
                {
                    return new Result<UnverifiedQuestion>(new InvalidQuestionException($"The question has more than 1000 characters."));
                }


               
            }

            private static bool IsQuestionValid(string question)
            {
                //TODO: validate email address based on RegEx

                //validate 
                if (question.Length <= 1000)
                {
                   
                    return true;
                }
                return false;
            }

            private static bool IsTagValid( string tags)
            {
                //TODO: validate email address based on RegEx

                //validate 
                
                if (tags.Split(' ').Length < 4 && tags.Split(' ').Length < 0)
                { 
                        return true;
                }
                return false;
            }
        }

        public class VerifiedQuestion : IQuestion
        {
            public string Title { get; private set; }

            internal VerifiedQuestion(string question)
            {
                Title = question;
            }
        }

        public class VerifiedTags : IQuestion
        {
            public string Tags { get; private set; }

            internal VerifiedTags(string tags)
            {
                Tags = tags;
            }
        }
    }
}
