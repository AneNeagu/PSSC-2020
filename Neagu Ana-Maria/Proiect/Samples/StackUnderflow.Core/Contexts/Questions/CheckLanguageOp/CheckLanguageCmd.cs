using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StackUnderflow.Domain.Schema.Questions.CheckLanguageOp
{
    public class CheckLanguageCmd
    {
        public CheckLanguageCmd(string text)
        {
            Text = text;
        }

        [Required]
        public string Text { get; }
    }
}
