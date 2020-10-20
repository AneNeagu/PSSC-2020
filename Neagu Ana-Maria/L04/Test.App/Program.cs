using System.Net;
using Profile.Domain.CreateProfileWorkflow;
using Question.Domain.CreateQuestionWorkflow;
using System;
using System.Collections.Generic;
using static Profile.Domain.CreateProfileWorkflow.CreateProfileResult;
using static Question.Domain.CreateQuestionWorkflow.CreateQuestionResult;

namespace Test.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var cmd = new CreateProfileCmd("Ion", string.Empty, "Ionescu", "ion.inonescu@company.com");
            var cmd2 = new CreateQuestionCmd("Titlu", "Intrebare", "Java");
            var result = CreateProfile(cmd);
            var result2 = CreateQuestion(cmd2);
           
            result.Match(
                    ProcessProfileCreated,
                    ProcessProfileNotCreated,
                    ProcessInvalidProfile
                );

            result2.Match(OnQuestionCreated,
                          OnQuestionNotCreated,
                          OnInvalidrequest);

            Console.ReadLine();
        }
        //Profile
        private static ICreateProfileResult ProcessInvalidProfile(ProfileValidationFailed validationErrors)
        {
            Console.WriteLine("Profile validation failed: ");
            foreach (var error in validationErrors.ValidationErrors)
            {
                Console.WriteLine(error);
            }
            return validationErrors;
        }

        private static ICreateProfileResult ProcessProfileNotCreated(ProfileNotCreated profileNotCreatedResult)
        {
            Console.WriteLine($"Profile not created: {profileNotCreatedResult.Reason}");
            return profileNotCreatedResult;
        }

        private static ICreateProfileResult ProcessProfileCreated(ProfileCreated profile)
        {
            Console.WriteLine($"Profile {profile.ProfileId}");
            return profile;
        }

        public static ICreateProfileResult CreateProfile(CreateProfileCmd createProfileCommand)
        {
            if (string.IsNullOrWhiteSpace(createProfileCommand.EmailAddress))
            {
                var errors = new List<string>() { "Invlaid email address" };
                return new ProfileValidationFailed(errors);
            }

            if (new Random().Next(10) > 1)
            {
                return new ProfileNotCreated("Email could not be verified");
            }

            var profileId = Guid.NewGuid();
            var result = new ProfileCreated(profileId, createProfileCommand.EmailAddress);
            return result;

        }  
        //Question
        private static ICreateQuestionResult OnInvalidrequest(QuestionValidationFailed validationErrors)
        {
            Console.WriteLine("Question validation failed: ");
            foreach (var error in validationErrors.ValidationErrors)
            {
                Console.WriteLine(error);
            }
            return validationErrors;
        }

        private static ICreateQuestionResult OnQuestionNotCreated(QuestionNotCreated questionNotCreatedResult)
        {
            Console.WriteLine($"Question not created: { questionNotCreatedResult.messingError}");
            return questionNotCreatedResult;
        }

        private static ICreateQuestionResult OnQuestionCreated(QuestionCreated question)
        {
            Console.WriteLine($"Question {question.QuestionId}");
            return question;
        }

        public static ICreateQuestionResult CreateQuestion(CreateQuestionCmd createQuestionCommand)
        {

            if (string.IsNullOrWhiteSpace(createQuestionCommand.Title))
            {
                var errors = new List<string>() { "Invlaid title" };
                return new QuestionValidationFailed(errors);
            }
            if (string.IsNullOrWhiteSpace(createQuestionCommand.Body))
            {
                var errors = new List<string>() { "Invlaid text" };
                return new QuestionValidationFailed(errors);
            }

            if (new Random().Next(10) > 1)
            {
                return new QuestionNotCreated("Text could not be verified");
            }

            var questionId = Guid.NewGuid();
            var result2 = new QuestionCreated(questionId, createQuestionCommand.Title);

                //execute logic
               
                return result2;               
            
            }
    }
}
