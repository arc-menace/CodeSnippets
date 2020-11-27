using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSnippets.Data.Models
{
    public class Snippet : DatabaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public ICollection<Tag> Tags { get; set; }

        public Snippet(int creatorId) : base(creatorId) { }

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
