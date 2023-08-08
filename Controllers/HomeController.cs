using System.Diagnostics;
using ToDoApp.Data;
using ToDoApp.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Models;

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
            _db= toDoContext;
        }

        public IActionResult Index()
        {
            return View(GetToDos());
        }
        
        /// <summary>
        /// The method GetToDos will return the list of To Do items from database.
        /// </summary>
        /// <returns>List of To Do items</returns>
        private List<ToDo> GetToDos() => _db.ToDos.ToList();
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
