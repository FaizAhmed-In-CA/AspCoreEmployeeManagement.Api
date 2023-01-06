using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Controllers
{
    public class HomeController : Controller
    {
        public string Index()
        {
            return "Employee Management API is Running...";
        }
    }
}
