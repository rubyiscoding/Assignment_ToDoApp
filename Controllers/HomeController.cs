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

        public HomeController(ILogger<HomeController> logger, ToDoContext toDoContext)
        {
            _logger = logger;
            _db= toDoContext;
        }

        public IActionResult Index()
        {
            return View(GetToDos());
        }

        private List<ToDo> GetToDos(){
            var abc = _db.ToDos.ToList();
            return _db.ToDos.ToList();
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
    }
}
