using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StackUnderflow.DatabaseModel.Models
{
    [Table("Reply")]
    public class Reply
    {
        public int ReplyId { get; set; }
        public int QuestionId { get; set; }
        public int AuthorUserId { get; set; }
        public string Body { get; set; }
    }
}