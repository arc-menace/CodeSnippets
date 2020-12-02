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

                if (model.TagString != null)
                {
                    //tags sent as string array of form:
                    //"[{\"value\":\"tag1\"},{\"value\":\"tag2\"},{\"value\":\"tag3\"}]"
                    //First remove end brackets
                    model.TagString = model.TagString.Remove(model.TagString.Length - 1, 1);
                    model.TagString = model.TagString.Remove(0, 1);

                    //Break into individual tag values
                    string[] tagArr = model.TagString.Split(",");
                    for (int i = 0; i < tagArr.Length; i++)
                    {
                        var tag = tagArr[i];
                        //Remove {\"value\":\"
                        tag = tag.Remove(0, 10); //values are lower than true length (13 characters) to do escapement
                                                 //Remove \"}
                        tag = tag.Substring(0, tag.Length - 2);

                        var existingTag = context.Tags.Where(m => m.Name == tag).FirstOrDefault();

                        if (existingTag == default(Tag))
                        {
                            existingTag = new Tag();
                            //Tag did not already exist. Create and add to context
                            existingTag.Name = tag;

                            context.Tags.Add(existingTag);
                            context.SaveChanges(); //Save to generate TagId
                        }

                        //Check if join already exists
                        var join = context.SnippetTags.Where(m => 
                            m.SnippetId == model.Snippet.SnippetId &&
                            m.TagId == existingTag.TagId)
                            .FirstOrDefault();

                        if (join == default(SnippetTag))
                        {
                            join = new SnippetTag();
                            join.TagId = existingTag.TagId;
                            join.SnippetId = model.Snippet.SnippetId;
                            context.SnippetTags.Add(join);
                        }


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

            //load snippet
            viewModel.Snippet = context.Snippets.Where(m => m.SnippetId == id).FirstOrDefault();

            //Load Tags
            var joinModels = context.SnippetTags.Where(m => m.SnippetId == id).ToList();
            var tags = new List<Tag>();
            foreach(var join in joinModels)
            {
                tags.Add(context.Tags.Where(m => m.TagId == join.TagId && m.Visible).FirstOrDefault());
            }

            //generate Tag string
            viewModel.TagString = "";
            foreach (var tag in tags)
            {
                if(tag != null)
                {
                    viewModel.TagString += tag.Name + ",";
                }
            }
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult UpdateSnippet(AddEditSnippetViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Db Items
                context.Snippets.Update(model.Snippet);
                var snippet = context.Snippets.Where(m => m.SnippetId == model.Snippet.SnippetId).FirstOrDefault();
                var joinModels = context.SnippetTags.Where(m => m.SnippetId == snippet.SnippetId).ToList();
                //delete existing relationships
                foreach(var join in joinModels)
                {
                    context.SnippetTags.Remove(join);
                }
                context.SaveChanges();


                //tags sent as string array of form:
                //"[{\"value\":\"tag1\"},{\"value\":\"tag2\"},{\"value\":\"tag3\"}]"
                //First remove end brackets
                if (model.TagString != null)
                {

                    model.TagString = model.TagString.Remove(model.TagString.Length - 1, 1);
                    model.TagString = model.TagString.Remove(0, 1);

                    //Break into individual tag values
                    string[] tagArr = model.TagString.Split(",");

                    if (model.TagString != null)
                    {
                        //tags sent as string array of form:
                        //"[{\"value\":\"tag1\"},{\"value\":\"tag2\"},{\"value\":\"tag3\"}]"
                        //First remove end brackets
                        model.TagString = model.TagString.Remove(model.TagString.Length - 1, 1);
                        model.TagString = model.TagString.Remove(0, 1);

                        //Break into individual tag values
                        for (int i = 0; i < tagArr.Length; i++)
                        {
                            var tag = tagArr[i];
                            //Remove {\"value\":\"
                            tag = tag.Remove(0, 10); //values are lower than true length (13 characters) to do escapement
                                                     //Remove \"}
                            tag = tag.Substring(0, tag.Length - 2);

                            var existingTag = context.Tags.Where(m => m.Name == tag).FirstOrDefault();

                            if (existingTag == default(Tag))
                            {
                                existingTag = new Tag();
                                //Tag did not already exist. Create and add to context
                                existingTag.Name = tag;
                                existingTag.Visible = true;
                                context.Tags.Add(existingTag);
                                context.SaveChanges(); //Save to generate TagId
                            }

                            var join = new SnippetTag();
                            join.TagId = existingTag.TagId;
                            join.SnippetId = model.Snippet.SnippetId;
                            context.SnippetTags.Add(join);
                        }
                    }
                }

                context.SaveChanges();
                //generate Tag string
                joinModels = context.SnippetTags.Where(m => m.SnippetId == model.Snippet.SnippetId).ToList();
                var tags = new List<Tag>();
                foreach (var join in joinModels)
                {
                    tags.Add(context.Tags.Where(m => m.TagId == join.TagId && m.Visible).FirstOrDefault());
                }
                model.TagString = "";
                foreach (var tag in tags)
                {
                    if(tag != null)
                    {
                        model.TagString += tag.Name + ",";
                    }
                }
                return View(model);
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
