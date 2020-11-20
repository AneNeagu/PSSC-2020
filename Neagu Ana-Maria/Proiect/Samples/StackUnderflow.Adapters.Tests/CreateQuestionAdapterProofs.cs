//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Access.Primitives.IO.Extensions.xUnit;
//using StackUnderflow.Adapters.CreateQuestion;
//using StackUnderflow.Domain.Core.Contexts;
//using StackUnderflow.Domain.Core.Contexts.FrontOffice;
//using StackUnderflow.Domain.Schema.Questions.CreateQuestionOp;
//using StackUnderflow.EF.Models;
//using StackUnderflow.Questions.Adapters;
//using Xunit;

//namespace StackUnderflow.Adapters.Tests
//{
//    public class CreateQuestionAdapterProofs: AdapterTest
//    {
//        public CreateQuestionAdapterProofs() : base(typeof(CreateQuestionAdapter).Assembly)
//        {
//        }


//        [Theory]
//        [CarthesianProductOf(typeof(CreateQuestionCmdInput), typeof(UserWriteContextInput))]
//        public async Task CreateQuestionAdapter(params object[] path)
//        {
//            var input = new CreateQuestionCmdGen().Get(path.OfType<CreateQuestionCmdInput>().Single());
//            var state = new UserWriteContextGen().Get(path.OfType<UserWriteContextInput>().Single());

//            var expr = from q in QuestionsDomain.CreateQuestion(input.Title, input.Body)
//                select q;

//            var result = await TestExpr(state, expr, path);
//        }
//    }
//}
