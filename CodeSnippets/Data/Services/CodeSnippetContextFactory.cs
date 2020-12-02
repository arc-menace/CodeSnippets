using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using CodeSnippets.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSnippets.Data.Services
{
    public class CodeSnippetContextFactory : IDesignTimeDbContextFactory<CodeSnippetContext>
    {
        //https://medium.com/oppr/net-core-using-entity-framework-core-in-a-separate-project-e8636f9dc9e5
        public CodeSnippetContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CodeSnippetContext>();
            var connectionString = Connection.ConnectionString();
            builder.UseSqlServer(connectionString);
            return new CodeSnippetContext(builder.Options);
        }
    }
}
