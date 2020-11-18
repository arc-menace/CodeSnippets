using Microsoft.EntityFrameworkCore;
using CodeSnippets.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSnippets.Data.Services
{
    public class CodeSnippetContext: DbContext
    {
        public DbSet<Snippet> Snippets { get; set; }
        public DbSet<Tag> Tags { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost; Database=CodeSnippets; Trusted_Connection=True");
        }
    }
}
