using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSnippets.Data.Models
{
    public class Connection
    {
        public static string ConnectionString()
        {
            return "Server = localhost; Database = CodeSnippets; Trusted_Connection = True";
        }
    }
}
