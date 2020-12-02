﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSnippets.Data.Models
{
    public class Tag : DatabaseEntity
    {
        public int TagId { get; set; }
        public string Name { get; set; }
        public ICollection<Snippet> Snippets { get; set; }


        public Tag() : base() 
        {
            Snippets = new List<Snippet>();
        }
    }
}
