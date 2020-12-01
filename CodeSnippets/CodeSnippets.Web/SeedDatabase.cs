using CodeSnippets.Data.Services;
using CodeSnippets.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeSnippets.Web
{
    public class SeedDatabase
    {
        public static void Initialize()
        {
            var context = new CodeSnippetContext(new Microsoft.EntityFrameworkCore.DbContextOptions<CodeSnippetContext>());

            if(!context.Snippets.Any())
            {
                var snippet = new Snippet()
                {
                    Name = "Example",
                    Code = "print(\"Hello World\")"
                };

                context.Snippets.Add(snippet);
                context.SaveChanges();
            }
        }
    }
}
