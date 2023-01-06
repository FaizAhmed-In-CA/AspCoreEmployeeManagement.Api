using EmployeeManagement.Api.Models;

namespace EmployeeManagement.Api.Repositories
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> GetDepartments();
        Department GetDepartment(int departmentId);
    }

}
