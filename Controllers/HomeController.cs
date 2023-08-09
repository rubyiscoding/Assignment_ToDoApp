using System;
using System.Diagnostics;
using ToDoApp.Data;
using ToDoApp.Models;
using ToDoApp.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; 
using System.Collections.Generic;  

namespace ToDoApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ToDoContext _db;

        /// <summary>
        /// HomeController default constructor taking logger and ToDoContext
        /// Dependency Injection best practices
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="toDoContext">our database context for ToDo</param>
        public HomeController(ILogger<HomeController> logger, ToDoContext toDoContext)
        {
            _logger = logger;
            _db = toDoContext;
        }

        public IActionResult Index()
        {
            //return View(GetToDos());
            var incompleteToDos = GetToDos().Where(p => !p.IsCompleted).ToList();
            return View(incompleteToDos);
        }
        
        /// <summary>
        /// The method GetToDos will return the list of To Do items from the database.
        /// </summary>
        /// <returns>List of To Do items</returns>
        private List<ToDo> GetToDos() => _db.ToDos.ToList();
        
        [HttpPost]
        public IActionResult AddToDo(ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                // Add the new To-Do to the database
                var addedToDo = _db.ToDos.Add(toDo);
                _db.SaveChanges();
                
                // Return the added To-Do's details
                return Json(new { detail = addedToDo.Entity.Detail, addedToDo.Entity.CompletionDate, addedToDo.Entity.IsCompleted });
            }
            
            // Return an error response if the model state is not valid
            return Json(new { error = true });
        }
        
        [HttpPost]
        public IActionResult MarkAsCompleted(int id)
        {
            var toDo = _db.ToDos.Find(id);
            if (toDo != null)
            {
                // Mark the To-Do as completed and update the completion date
                toDo.IsCompleted = true;
                toDo.CompletionDate = DateTime.Now;

                _db.ToDos.Update(toDo); // Update the entity state
                _db.SaveChanges();
            }

            // Redirect back to the index page after marking as completed
            return RedirectToAction(nameof(Index));
        }
        
        public IActionResult Create()
        {
            // Display the Create.cshtml view for adding new To-Dos
            return View();
        }

        [HttpPost]
        public IActionResult Create(ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                // Add the new To-Do to the database
                var addedToDo = _db.ToDos.Add(toDo);
                _db.SaveChanges();
                
                // Redirect to the index page after adding the new To-Do
                return RedirectToAction(nameof(Index));
            }
            
            // Return the Create.cshtml view with validation errors if model state is not valid
            return View(toDo);
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult CompletedTodos()
{
        var completedToDos = GetToDos().Where(p => p.IsCompleted);
        return View(completedToDos);
}
    }

}
