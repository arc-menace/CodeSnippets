using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeSnippets.Web.Models;
using CodeSnippets.Data.Services;
using CodeSnippets.Data.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CodeSnippets.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private CodeSnippetContext context { get; set; }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            context = new CodeSnippetContext(new DbContextOptions<CodeSnippetContext>());
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            var snippets = context.Snippets.Select(n => n).ToList();
            return View(snippets);
        }

        [HttpGet]
        public IActionResult AddSnippet()
        {
            return View(new AddEditSnippetViewModel());
        }

        [HttpPost]
        public IActionResult AddSnippet(AddEditSnippetViewModel model)
        {
            if(ModelState.IsValid)
            {
                //Create Snippet to get generate snippet id
                context.Snippets.Add(model.Snippet);
                context.SaveChanges();

                //tags sent as string array of form:
                //"[{\"value\":\"tag1\"},{\"value\":\"tag2\"},{\"value\":\"tag3\"}]"
                //First remove end brackets
                model.TagString = model.TagString.Remove(model.TagString.Length - 1, 1);
                model.TagString = model.TagString.Remove(0, 1);

                //Break into individual tag values
                string[] tagArr = model.TagString.Split(",");
                for(int i = 0; i < tagArr.Length; i++)
                {
                    var tag = tagArr[i];
                    //Remove {\"value\":\"
                    tag = tag.Remove(0, 10); //values are lower than true length (13 characters) to do escapement
                    //Remove \"}
                    tag = tag.Substring(0, tag.Length - 2);

                    var existingTag = context.Tags.Where(m => m.Name == tag).FirstOrDefault();

                    if (existingTag == default(Tag))
                    {
                        Tag snippetTag = new Tag();
                        snippetTag.Name = tag;
                        snippetTag.SetCreatorId(0);
                        snippetTag.Snippets.Add(model.Snippet);

                        context.Tags.Add(snippetTag);
                        context.SaveChanges();

                        model.Snippet.Tags.Add(snippetTag);
                        context.Snippets.Update(model.Snippet);
                    }
                    else
                    {
                        existingTag.Snippets.Add(model.Snippet);
                        context.Tags.Update(existingTag);
                        model.Snippet.Tags.Add(existingTag);
                        context.Snippets.Update(model.Snippet);
                    }
                    
                }
                

                
                context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else
            {
                var viewModel = new AddEditSnippetViewModel();
                viewModel.Snippet = model.Snippet;
                viewModel.ShowErrorMessage = true;
                viewModel.ErrorMessage = "Invalid Model State";
                return View(viewModel);
            }
        }

        [HttpGet]
        public IActionResult UpdateSnippet(int id)
        {
            var viewModel = new AddEditSnippetViewModel();
            viewModel.Snippet = context.Snippets.Where(m => m.SnippetId == id).FirstOrDefault();

            //Load Tags
            viewModel.Snippet.Tags = context.Tags.Where(m => m.Snippets.Any(j => j.SnippetId == id)).ToList();

            foreach (var tag in viewModel.Snippet.Tags)
            {
                viewModel.TagString += tag.Name + ",";
            }
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult UpdateSnippet(Snippet snippet)
        {
            if (ModelState.IsValid)
            {
                context.Snippets.Update(snippet);
                context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else
            {
                var viewModel = new AddEditSnippetViewModel();
                viewModel.Snippet = snippet;
                viewModel.ShowErrorMessage = true;
                viewModel.ErrorMessage = "Invalid Model State";
                return View(viewModel);
            }
        }

        public IActionResult DeleteSnippet(int id)
        {
            var snippet = context.Snippets.Where(m => m.SnippetId == id).FirstOrDefault();
            context.Snippets.Remove(snippet);
            context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
