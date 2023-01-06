using EmployeeManagement.Api.Models;

namespace EmployeeManagement.Api.Repositories
{
    public interface IEmployeeRepository
    {
        //Task<IEnumerable<Employee>> GetEmployees();

        IEnumerable<Employee> GetEmployees();

        Task<Employee> GetEmployee(int employeeId);
        Task<Employee> AddEmployee(Employee employee);
        Task<Employee> UpdateEmployee(Employee employee);
        Task DeleteEmployee(int employeeId);
        Task<Employee> GetEmployeeByEmail(string email);

    }

}
