using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeSnippets.Data.Models;

namespace CodeSnippets.Web.Models
{
    public class DashboardViewModel
    {
        public List<Snippet> Snippets { get; set; }
        public string SearchTerm { get; set; } = "";

        public DashboardViewModel()
        {
            Snippets = new List<Snippet>();
        }
    }
}
