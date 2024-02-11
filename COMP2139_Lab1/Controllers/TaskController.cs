using COMP2139_Lab1.Data;
using COMP2139_Lab1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_Lab1.Controllers
{

    

    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(int projectID)
        {
            var tasks = _context.ProjectTasks.
                Where(t => t.projectID == projectID).
                ToList();

            ViewBag.ProjectID = projectID;

            return View(tasks);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var task = _context.ProjectTasks.
                Include(t => t.Project).
                FirstOrDefault(task => task.projectID == id);

            if(task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        [HttpGet]
        public IActionResult Create(int projectID)
        {
            var project = _context.Projects.Find(projectID);
            if(project == null)
            {
                return NotFound();
            }

            var task = new ProjectTask()
            {
                projectID = projectID,

            };

            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title", "Description", "projectID")] ProjectTask task)
        {
            if(ModelState.IsValid)
            {
                _context.ProjectTasks.Add(task);
                _context.SaveChanges();
                return RedirectToAction("Index", new {projectID = task.projectID});
            }

            ViewBag.Projects = new SelectList(_context.Projects, "projectID", "Name", task.projectID);
            return View(task);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var task = _context.ProjectTasks.
                Include(t => t.Project).
                FirstOrDefault(t => t.ProjectTaskId == id);
            
            if(task == null)
            {
                return NotFound();
            }
            ViewBag.Projects = new SelectList(_context.Projects, "projectID", "Name", task.projectID);
            return View(task);


            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProjectTaskId", "Title", "Description", "projectID")] ProjectTask task)
        {
            if(id != task.ProjectTaskId)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                _context.ProjectTasks.Add(task);
                _context.SaveChanges();
                return RedirectToAction("Index", new {projectID = task.projectID});
            }

            ViewBag.Projects = new SelectList(_context.Projects, "projectID", "Name", task.projectID);
            return View(task);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var task = _context.ProjectTasks.
                Include(t => t.Project).
                FirstOrDefault(t => t.ProjectTaskId == id);

            if (task == null)
            {
                return NotFound();
            }

            ViewBag.Projects = new SelectList(_context.Projects, "projectID", "Name", task.projectID);
            return View(task);

        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int projectTaskID)
        {
            var task = _context.ProjectTasks.Find(projectTaskID);

            if (task != null)
            {
                _context.ProjectTasks.Remove(task);
                _context.SaveChanges();
                return RedirectToAction("Index", new { projectID = task.projectID });
            }

            return NotFound();

            
        }
    }
}
