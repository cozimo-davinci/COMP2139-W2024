using COMP2139_Lab1.Models;
using Microsoft.AspNetCore.Mvc;

namespace COMP2139_Lab1.Controllers
{
    public class ProjectController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var projects = new List<Project>()
            {
                new Project {projectID = 1, Name = "Project 1", Description = "My First Project"},
                new Project {projectID = 2, Name = "Data Miner", Description = "Advanced Data Scraper"},
                new Project {projectID = 3, Name = "Blockchain Scanner", Description = "AML Blockchain Scanner"}
            };

            return View(projects);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Project project)
        {
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var project = new Project { projectID = id, Name = "Project " + id, Description = "Details of project " + id };
            return View(project);
        }
    }
}
