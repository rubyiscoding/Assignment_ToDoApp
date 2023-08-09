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

        /// <summary>
        /// This method is a landing page which returns a list of incompleted To Do items.
        /// </summary>
        /// <returns>list of incomplete To Do items</returns>
        public IActionResult Index()
        {
            var allToDos = GetToDos().ToList().OrderBy(p => p.CompletionDate);
            var incompleteToDoList = allToDos.Where(p => !p.IsCompleted).ToList();
            ViewBag.CompletedTodos = allToDos.Where(p => p.IsCompleted).Count();
            return View(incompleteToDoList);
        }

        /// <summary>
        /// The method GetToDos will return the list of To Do items from the database.
        /// </summary>
        /// <returns>List of To Do items</returns>
        private List<ToDo> GetToDos() => _db.ToDos.ToList();

        /// <summary>
        /// This method posts the To Do item
        /// </summary>
        /// <param name="toDo"></param>
        /// <returns>After the post is successful it returns JSON response data.</returns>
        [HttpPost]
        public IActionResult AddToDo(ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                // Add the new To-Do item to the ToDos table 
                var addedToDo = _db.ToDos.Add(toDo);
                //now the added ToDo item will be saved in database.
                _db.SaveChanges();

                // Return the added To-Do's details
                return Json(new { detail = addedToDo.Entity.Detail, addedToDo.Entity.CompletionDate, addedToDo.Entity.IsCompleted });
            }

            // Return an error response if the model state is not valid
            return Json(new { error = true });
        }

        /// <summary>
        /// This method posts the ToDo item to be completed
        /// </summary>
        /// <param name="id"> ToDo Item Id </param>
        /// <returns>Redirects to home page.</returns>
        [HttpPost]
        public IActionResult MarkAsCompleted(int id)
        {
            //Retrieve the To Do item with the provided Id.
            var toDo = _db.ToDos.Find(id);
            /*If the ToDo item with that Id is found, 
            then save to database with completion date 
            and mark IsCompleted checkbox.*/
            if (toDo != null)
            {
                // Mark the To-Do as completed and set the completion date to now.
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
