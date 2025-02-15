using InvterViewTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InvterViewTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public IConfiguration Configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            DataBaseConnecttionCls dataBaseConnecttionCls = new DataBaseConnecttionCls(Configuration);

            string query = "Select * from tbl_User";

            // Fetch list of employees
            List<tbl_User> employees = dataBaseConnecttionCls.GetData<tbl_User>(query);

            // Print employees
            //foreach (var emp in employees)
            //{
            //    Console.WriteLine($"ID: {emp.Id}, Name: {emp.Name}, Age: {emp.Age}");
            //}
            return View();
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