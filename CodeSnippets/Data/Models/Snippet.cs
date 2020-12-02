using CodeSnippets.Data.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeSnippets.Data.Models
{
    public class Snippet
    {
        public int SnippetId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public void AutoGenerateKeywords(CodeSnippetContext context)
        {
            if(Code == null)
            {
                return;
            }
            string code = new string(Code); //create copy of Code string

            //check code against keywords useful for searching
            //Remove Comments
            //https://stackoverflow.com/questions/3524317/regex-to-strip-line-comments-from-c-sharp
            //Block
            code = Regex.Replace(code, @"/\*(.*?)\*/", "");
            //Single Line
            code = Regex.Replace(code, @"//(.*?)\r?\n", "");

            //Remove Strings
            //https://stackoverflow.com/questions/13024073/regex-c-sharp-extract-text-within-double-quotes/13024232
            code = Regex.Replace(code, "\"[^\"]*\"", "");

            //Replace { } [ ] : ; < > . and , with " " 
            code = Regex.Replace(code, @"[\[\]\(\)\{\};,.<>]", " ");

            code = Regex.Replace(code, @"\s+", " "); //Remove extra spaces from missing pieces

            List<string> Keywords = new List<string>(code.Split(" "));

            Keywords = Keywords.Distinct().ToList();

            var joinModels = context.SnippetKeywords.Where(m => m.SnippetId == SnippetId).ToList();
            //delete existing relationships
            foreach (var join in joinModels)
            {
                context.SnippetKeywords.Remove(join);
            }
            context.SaveChanges();

            for (int i = 0; i < Keywords.Count; i++)
            {
                var keyword = Keywords[i];                

                var existingKeyword = context.Keywords.Where(m => m.Word == keyword).FirstOrDefault();

                if (existingKeyword == default(Keyword))
                {
                    existingKeyword = new Keyword();
                    //Tag did not already exist. Create and add to context
                    existingKeyword.Word = keyword;

                    context.Keywords.Add(existingKeyword);
                    context.SaveChanges(); //Save to generate TagId
                }

                var join = new SnippetKeyword();
                join.KeywordId = existingKeyword.KeywordId;
                join.SnippetId = SnippetId;
                context.SnippetKeywords.Add(join);
            }

            context.SaveChanges();
        }
    }
}
