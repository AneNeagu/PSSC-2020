using Orleans;
using StackUnderflow.Backend.Abstractions.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StackUnderflow.Backend.Abstractions.FrontOffice
{
    public interface IUserGrain : IGrainWithStringKey
    {
        //Task<CreateQuestionResponse> CreateQuestion(string title, string body);

    }
}
