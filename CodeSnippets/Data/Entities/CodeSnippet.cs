using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSnippets.Data.Entities
{
    public class CodeSnippet : DatabaseEntity
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual string Tag { get; set; }
    }
}
