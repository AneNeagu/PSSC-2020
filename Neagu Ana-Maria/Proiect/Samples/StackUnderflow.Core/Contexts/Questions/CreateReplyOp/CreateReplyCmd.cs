using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StackUnderflow.Domain.Schema.Questions.CreateAnswerOp
{
    public class CreateReplyCmd
    {
        public CreateReplyCmd()
        {

        }
        public CreateReplyCmd(int questionId, Guid authorUserId, string body)
        {
            QuestionId = questionId;
            AuthorUserId = authorUserId;
            Body = body;
        }
        [Required]
        public int QuestionId { get; }
        [Required]
        public Guid AuthorUserId { get; }
        [Required]
        [MinLength(10)]
        [MaxLength(500)]
        public string Body { get; }
    }
}
