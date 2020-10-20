using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Question.Domain.CreateQuestionWorkflow
{
    public struct CreateQuestionCmd
    {
        [Required]
        [MaxLength(15)]
        public string Title{ get;  set; }
        [Required]
        [MaxLength(30)]
        public string Body { get; set; }
        [Required]
        [MaxLength(50)]
        public string Tags { get;  set; }
      
       
        public CreateQuestionCmd(string title, string body, string tags)
        {
            Title = title;
            Body = body;
            Tags = tags;
           
        }
    }
}
