﻿using Microsoft.EntityFrameworkCore;
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
        public DbSet<SnippetTag> SnippetTags { get; set; }
        public DbSet<SnippetKeyword> SnippetKeywords { get; set; }
        public DbSet<Keyword> Keywords { get; set; }

        public CodeSnippetContext(DbContextOptions<CodeSnippetContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Connection.ConnectionString());
        }
    }
}
