using EmployeeManagementAPI.Model;
using EmployeeManagementAPI.Exception;
using System.Reflection.Metadata.Ecma335;


namespace EmployeeManagementAPI.Services
{
    public interface IEmployeeService
    {
        Employee GetEmployee(Guid employeeId);
        void CreateEmployee(Employee employee);
        void UpdateEmployee(Employee employee);

        List<Employee> GetEmployees();
    }

    public class EmployeeService : IEmployeeService
    {
        private readonly Dictionary<Guid, Employee> _employeeStorage = new Dictionary<Guid, Employee>();

        public Employee GetEmployee(Guid employeeId)
        {
            if (_employeeStorage.ContainsKey(employeeId))
            {
                return _employeeStorage[employeeId];
            }

            throw new EmployeeNotFoundException($"Employee with ID {employeeId} not found.");
        }

        public List<Employee> GetEmployees()
        {
            return _employeeStorage.Values.ToList();
        }

        public void CreateEmployee(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));
             employee.EmployeeId = Guid.NewGuid();
            if (_employeeStorage.ContainsKey(employee.EmployeeId))
                throw new InvalidOperationException("Employee already exists.");

            _employeeStorage[employee.EmployeeId] = employee;
        }

        public void UpdateEmployee(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            if (!_employeeStorage.ContainsKey(employee.EmployeeId))
                throw new EmployeeNotFoundException($"Employee with ID {employee.EmployeeId} not found.");

            _employeeStorage[employee.EmployeeId] = employee;
        }
    }

}
