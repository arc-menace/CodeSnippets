using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeSnippets.Data.Models;

namespace CodeSnippets.Web.Models
{
    public class AddEditSnippetViewModel
    {
        public Snippet Snippet { get; set; }
        public string TagString { get; set; } = "";
        public string ErrorMessage { get; set; } = "";
        public bool ShowErrorMessage { get; set; } = false;

        public AddEditSnippetViewModel()
        {
            Snippet = new Snippet();
        }
    }
}
