using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSnippets.Data.Models
{
    public class SnippetKeyword
    {
        public int SnippetKeywordId { get; set; }
        public int SnippetId { get; set; }
        public int KeywordId { get; set; }
    }
}
