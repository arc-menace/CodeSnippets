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
        public IActionResult AddSnippet(Snippet snippet)
        {
            if(ModelState.IsValid)
            {
                context.Snippets.Add(snippet);
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

        [HttpGet]
        public IActionResult UpdateSnippet(int id)
        {
            var viewModel = new AddEditSnippetViewModel();
            viewModel.Snippet = context.Snippets.Where(m => m.SnippetId == id).FirstOrDefault();

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

        [HttpGet]
        public IActionResult DeleteSnippet(int id)
        {
            var viewModel = new DeleteViewModel();
            viewModel.Snippet = context.Snippets.Where(m => m.SnippetId == id).FirstOrDefault();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult DeleteSnippet(Snippet snippet)
        {
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
