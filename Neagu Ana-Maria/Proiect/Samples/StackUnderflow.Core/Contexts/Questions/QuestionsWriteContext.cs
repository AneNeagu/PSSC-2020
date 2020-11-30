using System;
using StackUnderflow.DatabaseModel.Models;
using System.Collections.Generic;
using System.Text;

namespace StackUnderflow.Domain.Core.Contexts.Questions
{
    public class QuestionsWriteContext
    {
       
        public ICollection<Reply> Replies { get; }
        public QuestionsWriteContext(ICollection<Reply> replies)
        {
            Replies = replies ?? new List<Reply>();
        }

        public ICollection<QuestionModel> Questions { get; }
        public QuestionsWriteContext(ICollection<QuestionModel> questions)
        {
            Questions = questions ?? new List<QuestionModel>();
        }
    }
}
