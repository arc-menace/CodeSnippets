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
            var viewModel = new DashboardViewModel();
            viewModel.Snippets = context.Snippets.Select(n => n).ToList();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Dashboard(DashboardViewModel viewModel)
        {
            List<Snippet> snippets = new List<Snippet>();

            //Filter list of snippets Here
            if(viewModel.SearchTerm != "" && viewModel.SearchTerm != null)
            {
                List<string> search = viewModel.SearchTerm.Split().ToList();

                foreach (var term in search)
                {
                    var tag = context.Tags.Where(m => m.Name == term).FirstOrDefault();
                    if(tag != null)
                    {
                        var joinTags = context.SnippetTags.Where(m => m.TagId == tag.TagId).ToList();
                        foreach(var joinTag in joinTags)
                        {
                            var snippet = context.Snippets.Where(m => m.SnippetId == joinTag.SnippetId).FirstOrDefault();
                            if(snippet != null)
                            {
                                snippets.Add(snippet);
                            }
                        }
                    }

                    var keyword = context.Keywords.Where(m => m.Word == term).FirstOrDefault();
                    if (keyword != null)
                    {
                        var joinKeywords = context.SnippetKeywords.Where(m => m.KeywordId == keyword.KeywordId).ToList();
                        foreach (var joinKeyword in joinKeywords)
                        {
                            var snippet = context.Snippets.Where(m => m.SnippetId == joinKeyword.SnippetId).FirstOrDefault();
                            if (snippet != null)
                            {
                                snippets.Add(snippet);
                            }
                        }
                    }
                }

                viewModel.Snippets = snippets.Distinct().ToList();
            }
            else
            {
                //Empty seach term, return all snippets
                viewModel.Snippets = context.Snippets.Select(n => n).ToList();
            }
            return View(viewModel);
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
                model.Snippet.AutoGenerateKeywords(context);
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
                tags.Add(context.Tags.Where(m => m.TagId == join.TagId).FirstOrDefault());
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
                    List<string> tagArr = model.TagString.Split(",").ToList();

                    if (model.TagString != null)
                    {
                        //tags sent as string array of form:
                        //"[{\"value\":\"tag1\"},{\"value\":\"tag2\"},{\"value\":\"tag3\"}]"
                        //First remove end brackets
                        model.TagString = model.TagString.Remove(model.TagString.Length - 1, 1);
                        model.TagString = model.TagString.Remove(0, 1);

                        //Break into individual tag values
                        for (int i = 0; i < tagArr.Count; i++)
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
                            var join = new SnippetTag();
                            join.TagId = existingTag.TagId;
                            join.SnippetId = model.Snippet.SnippetId;
                            context.SnippetTags.Add(join);
                        }

                        snippet.AutoGenerateKeywords(context);
                    }
                }

                context.SaveChanges();
                //generate Tag string
                joinModels = context.SnippetTags.Where(m => m.SnippetId == model.Snippet.SnippetId).ToList();
                var tags = new List<Tag>();
                foreach (var join in joinModels)
                {
                    tags.Add(context.Tags.Where(m => m.TagId == join.TagId).FirstOrDefault());
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
