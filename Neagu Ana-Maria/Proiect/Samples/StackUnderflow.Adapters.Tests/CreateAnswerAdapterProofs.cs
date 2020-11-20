//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Access.Primitives.IO.Extensions.xUnit;
//using StackUnderflow.Domain.Core.Contexts;
//using StackUnderflow.Domain.Core.Contexts.FrontOffice;
//using StackUnderflow.Domain.Schema.Questions.CreateAnswerOp;
//using StackUnderflow.Questions.Adapters;
//using StackUnderflow.Questions.Adapters.CreateAnswer;
//using Xunit;

//namespace StackUnderflow.Adapters.Tests
//{
//    public class CreateAnswerAdapterProofs : AdapterTest
//    {
//        public CreateAnswerAdapterProofs() : base(typeof(CreateAnswerAdapter).Assembly) { }

//        [Theory]
//        [CarthesianProductOf(typeof(CreateAnswerInternalCmdInput), typeof(QuestionsDataInput))]
//        public async Task CreateAnswerAdapter(params object[] path)
//        {
//            var input = new CreateAnswerInternalCmdGen().Get(path.OfType<CreateAnswerInternalCmdInput>().Single());
//            var state = new QuestionsDataGen().Get(path.OfType<QuestionsDataInput>().Single());

//            var expr = from a in QuestionsDomain.CreateAnswer(input.TenantId, input.UserId, input.QuestionId, input.Answer)
//                       select a;

//            var result = await TestExpr(state, expr, path);
//        }
//    }
//}
