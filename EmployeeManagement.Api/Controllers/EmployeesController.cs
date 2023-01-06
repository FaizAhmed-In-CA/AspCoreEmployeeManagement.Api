using EmployeeManagement.Api.Models;
using EmployeeManagement.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Hosting;





namespace EmployeeManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository employeeRepository;

        private IWebHostEnvironment webHostEnvironment;

        public EmployeesController(IEmployeeRepository employeeRepository, IWebHostEnvironment webHostEnvironment)
        {
            this.employeeRepository = employeeRepository;
            this.webHostEnvironment = webHostEnvironment;
        }

        //[HttpGet]
        //public async Task<ActionResult> GetEmployees()
        //{
        //    try
        //    {
        //        return Ok(await employeeRepository.GetEmployees());

        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            "Error retrieving data from the database");
        //    }
        //}

        [HttpGet]
        public IEnumerable<Employee> GetEmployees()
        {

            IEnumerable < Employee > employees = employeeRepository.GetEmployees();


            //return employeeRepository.GetEmployees();

            return employees;


            //    //try
            //    //{
            //    //    return  employeeRepository.GetEmployees();

            //    //}
            //    //catch (Exception)
            //    //{
            //    //    return StatusCode(StatusCodes.Status500InternalServerError,
            //    //        "Error retrieving data from the database");
            //    //}
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            try
            {
                var result = await employeeRepository.GetEmployee(id);

                if (result == null) return NotFound();

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest();
                }

                // Add custom model validation error
                var emp = employeeRepository.GetEmployeeByEmail(employee.Email);

                if (emp.Result != null)
                {
                    ModelState.AddModelError("email", "Employee email already in use");
                    return BadRequest(ModelState);
                }

                var createdEmployee = await employeeRepository.AddEmployee(employee);

                return CreatedAtAction(nameof(GetEmployee), new { id = createdEmployee.EmployeeId },
                    createdEmployee);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            try
            {
                if (id != employee.EmployeeId)
                    return BadRequest("Employee ID mismatch");

                var employeeToUpdate = await employeeRepository.GetEmployee(id);

                if (employeeToUpdate == null)
                    return NotFound($"Employee with Id = {id} not found");

                return await employeeRepository.UpdateEmployee(employee);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            try
            {
                var employeeToDelete = await employeeRepository.GetEmployee(id);

                if (employeeToDelete == null)
                {
                    return NotFound($"Employee with Id = {id} not found");
                }

                await employeeRepository.DeleteEmployee(id);
                
                return NoContent();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteEmployee(int id)
        //{
        //    var employeeToDelete = await employeeRepository.GetEmployee(id);

        //    if (employeeToDelete == null)
        //    {
        //        return NotFound();
        //    }

        //    employeeRepository.DeleteEmployee(id);

        //    //_context.TodoItems.Remove(todoItem);
        //    //await _context.SaveChangesAsync();

        //    return NoContent();
        //}


        [HttpPost("UploadFile")]
        public async Task<string> UploadFile([FromForm] IFormFile file)
        {
            string path = Path.Combine(webHostEnvironment.WebRootPath, "images/" + file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return "https://localhost:44354/images/" + file.FileName;
        }


    }

}
