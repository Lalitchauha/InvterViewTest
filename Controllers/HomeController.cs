using InvterViewTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Testdepandencyinjections;

namespace InvterViewTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public IConfiguration Configuration;
        public ISingleTone singleTone;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, ISingleTone _singleTone)
        {
            _logger = logger;
            Configuration = configuration;
            singleTone = _singleTone;
        }

        public IActionResult Index()
        {
            DataBaseConnecttionCls dataBaseConnecttionCls = new DataBaseConnecttionCls(Configuration);

            string query = "Select * from tbl_User_New";
            tbl_User_New tbl_User = new tbl_User_New();
            // Fetch list of employees
            tbl_User.tbl_Userslst = dataBaseConnecttionCls.GetData<tbl_User_New>(query);

            // Print employees
            //foreach (var emp in employees)
            //{
            //    Console.WriteLine($"ID: {emp.Id}, Name: {emp.Name}, Age: {emp.Age}");
            //}
            return View("~/Views/Home/Index.cshtml", tbl_User);
        }

        [HttpPost]
        public JsonResult Create([FromBody] tbl_User_New model)
        {
            DataBaseConnecttionCls dataBaseConnecttionCls = new DataBaseConnecttionCls(Configuration);
            try
            {
                int response = 0;
                DateTime now = DateTime.Now;
                Dictionary<String, String> nationalitymodeldict = new Dictionary<string, string>();
                nationalitymodeldict.Add("UserName", model.UserName + "");
                nationalitymodeldict.Add("FName", model.FName + "");
                nationalitymodeldict.Add("LName", model.LName + "");
                nationalitymodeldict.Add("Department", model.Department + "");
                nationalitymodeldict.Add("MgrId", model.MgrId + "");
                nationalitymodeldict.Add("Seniority", model.Seniority + "");
                nationalitymodeldict.Add("EmpCode", model.EmpCode + "");
                nationalitymodeldict.Add("Role", model.Role + "");
                nationalitymodeldict.Add("LastLogin", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "");
                nationalitymodeldict.Add("DOJ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "");
                response = dataBaseConnecttionCls.SaveExecuteNonQuery<tbl_User_New>(nationalitymodeldict);
                // Simulate saving to database (Replace with actual DB save logic)
                return Json(new { success = true, message = "User created successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult insertCreate()
        {
            singleTone.printname("Lalit chauhan");
            return View("~/Views/CreatePage.cshtml");
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