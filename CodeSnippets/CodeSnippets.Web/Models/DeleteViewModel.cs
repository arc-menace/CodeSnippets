using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeSnippets.Data.Models;

namespace CodeSnippets.Web.Models
{
    public class DeleteViewModel
    {
        public Snippet Snippet { get; set; }

        public DeleteViewModel()
        {
            Snippet = new Snippet();
        }
    }
}
