using COMP2139_Lab1.Areas.ProjectManagement.Models;
using COMP2139_Lab1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_Lab1.Areas.ProjectManagement.Controllers
{

    [Area("ProjectManagement")]
    [Route("[area]/[controller]/[action]")]

    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index(int projectID)
        {
            var tasks = _context.ProjectTasks.
                Where(t => t.projectID == projectID).
                ToList();

            ViewBag.ProjectID = projectID;

            return View(tasks);
        }

        [HttpGet("Details/{id}")]
        public IActionResult Details(int id)
        {
            var task = _context.ProjectTasks.
                Include(t => t.Project).
                FirstOrDefault(task => task.projectID == id);

            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        [HttpGet("Create")]
        public IActionResult Create(int projectID)
        {
            var project = _context.Projects.Find(projectID);
            if (project == null)
            {
                return NotFound();
            }

            var task = new ProjectTask()
            {
                projectID = projectID,

            };

            return View(task);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title", "Description", "projectID")] ProjectTask task)
        {
            if (ModelState.IsValid)
            {
                _context.ProjectTasks.Add(task);
                _context.SaveChanges();
                return RedirectToAction("Index", new { task.projectID });
            }

            ViewBag.Projects = new SelectList(_context.Projects, "projectID", "Name", task.projectID);
            return View(task);
        }

        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
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

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProjectTaskId", "Title", "Description", "projectID")] ProjectTask task)
        {
            if (id == task.ProjectTaskId)
            {
                if (ModelState.IsValid)
                {
                    _context.ProjectTasks.Update(task);
                    _context.SaveChanges();
                    return RedirectToAction("Index", new { task.projectID });
                }

                ViewBag.Projects = new SelectList(_context.Projects, "projectID", "Name", task.projectID);
                return View(task);
            }

            return NotFound();
        }

        [HttpGet("Delete/{id}")]
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

        [HttpPost("DeleteConfirmed/{projectTaskID}"), ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int projectTaskID)
        {
            var task = _context.ProjectTasks.Find(projectTaskID);

            if (task != null)
            {
                _context.ProjectTasks.Remove(task);
                _context.SaveChanges();
                return RedirectToAction("Index", new { task.projectID });
            }

            return NotFound();


        }

        [HttpGet("Search/{searchString?}")]
        public async Task<IActionResult> Search(int? projectID, string searchString)
        {
            var taskQuery = _context.ProjectTasks.AsQueryable();

            // if projectID was passed 
            if (projectID.HasValue)
            {
                taskQuery = taskQuery.Where(t => t.projectID == projectID.Value);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                taskQuery = taskQuery.Where(t => t.Title.Contains(searchString)
                || t.Description.Contains(searchString));
            }

            var tasks = await taskQuery.ToListAsync();
            ViewBag.projectID = projectID;
            return View("Index", tasks);
        }
    }
}
