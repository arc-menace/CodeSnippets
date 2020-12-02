using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSnippets.Data.Models
{
    public class Snippet
    {
        public int SnippetId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public void AutoGenerateTags()
        {
            if(Code == null)
            {
                return;
            }
            //check code against keywords useful for searching
        }
    }
}
