using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSnippets.Data.Models
{
    public class SnippetTag 
    {
        public int SnippetTagId { get; set; }
        public int SnippetId {get;set;}
        public int TagId { get; set; }       
    }
}
