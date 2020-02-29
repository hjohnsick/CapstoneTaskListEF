using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EFCoreIdentityFramework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreIdentityFramework.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly IdentityTaskListDbContext _context;

        public TasksController(IdentityTaskListDbContext context)
        {
            _context = context;
        }

        
        public IActionResult Index()
        {
            //print out all tasks
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Tasks> thisUsersTasks = _context.Tasks.Where(x => x.OwnerId == id).ToList();
            return View(thisUsersTasks);
           
        }
        
        [HttpGet]
        public IActionResult AddTask()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddTask(Tasks newTask)
        {
            newTask.OwnerId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (ModelState.IsValid)
            {
                _context.Tasks.Add(newTask);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult EditTasks(int id)
        {
            Tasks foundTasks = _context.Tasks.Find(id);
            if (foundTasks != null)
            {
                return View(foundTasks);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditTasks(Tasks updatedTasks)
        {
            //user can update task or mark complete
            Tasks dbTasks = _context.Tasks.Find(updatedTasks.Id);
            if (ModelState.IsValid)
            {
                dbTasks.Id = updatedTasks.Id;
                dbTasks.TaskDescription = updatedTasks.TaskDescription;
                dbTasks.DueDate = updatedTasks.DueDate;
                dbTasks.Complete = updatedTasks.Complete;

                _context.Entry(dbTasks).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(dbTasks);
                _context.SaveChanges();

            }
            return RedirectToAction("Index");
        }

        public IActionResult DeleteTasks(int id)
        {
            //user can delete task
            Tasks foundTasks = _context.Tasks.Find(id);
            if (foundTasks !=null)
            {
                _context.Remove(foundTasks);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}