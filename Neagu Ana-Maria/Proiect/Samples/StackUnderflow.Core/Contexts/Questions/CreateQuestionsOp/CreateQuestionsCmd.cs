using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StackUnderflow.Domain.Core.Contexts.Questions.CreateQuestionsOp
{
    public class CreateQuestionsCmd
    {
        [Required]
        public string Title { get;  set; }
        [Required]
        public string Description { get;  set; }
        [Required]
        public string Tags { get;  set; }

        public CreateQuestionsCmd() { }
        public CreateQuestionsCmd(string Title, string Description, string Tags)
        {
            this.Title = Title;
            this.Description = Description;
            this.Tags = Tags;
        }
    }
}