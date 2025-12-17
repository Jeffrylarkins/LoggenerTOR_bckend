using EmployeeManagementAPI.Model;
using EmployeeManagementAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagementAPI.Exception;

namespace EmployeeManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;
        private static readonly Random _random = new Random();

        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        // GET: api/employee
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            try
            {
                var employees = _employeeService.GetEmployees();
                return Ok(employees);  // Return the employee list with a 200 OK status
            }
            catch (System.Exception ex)
            {
                // Log the error if needed
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployee(Guid id)
        {
            var requestGuid = Request.Headers["Request-Guid"].ToString();  // Get the GUID from request headers
            var startTime = DateTime.UtcNow;  // Track request processing time

            // Get client IP address, handle possible null
            var clientIp = Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";  // Default to "Unknown" if null

            try
            {
                // Log the start of the request in JSON format
                _logger.LogInformation("{RequestGuid} - Processing GET request for Employee {EmployeeId} at {StartTime}, Client IP: {ClientIp}, Headers: {@Headers}",
                    requestGuid, id, startTime, clientIp, Request.Headers);

                // Randomly choose an exception or continue with the request
                if (_random.Next(1, 101) % 3 != 0) // If not divisible by 3, throw a random exception
                {
                    //ThrowRandomException();  // This method randomly throws an exception
                }

                var employee = _employeeService.GetEmployee(id);
              
                // Log the successful response in JSON format
                var timeTaken = DateTime.UtcNow - startTime;
                _logger.LogInformation("{RequestGuid} - Employee {EmployeeId} retrieved successfully. Time taken: {TimeTaken}, Response: {@Employee}",
                    requestGuid, id, timeTaken, employee);

                return Ok(employee);
            }
            catch (System.Exception ex)
            {
                // Log exception details in JSON format
                _logger.LogError(ex, "{RequestGuid} - Error retrieving employee {EmployeeId}. Client IP: {ClientIp}, Headers: {@Headers}",
                    requestGuid, id, clientIp, Request.Headers);
                return NotFound(new { Message = "Employee not found.", RequestGuid = requestGuid });
            }
        }

        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            var requestGuid = Request.Headers["Request-Guid"].ToString();  // Get the GUID from request headers
            var startTime = DateTime.UtcNow;  // Track request processing time

            // Get client IP address, handle possible null
            var clientIp = Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";  // Default to "Unknown" if null

            try
            {
                // Log the start of the request in JSON format
                _logger.LogInformation("{RequestGuid} - Processing POST request to create employee {EmployeeId} at {StartTime}, Client IP: {ClientIp}, Headers: {@Headers}, RequestBody: {@Employee}",
                    requestGuid, employee.EmployeeId, startTime, clientIp, Request.Headers, employee);

                // Randomly choose an exception or continue with the request
                if (_random.Next(1, 101) % 3 != 0) // If not divisible by 3, throw a random exception
                {
                    ThrowRandomException();  // This method randomly throws an exception
                }

                 _employeeService.CreateEmployee(employee);

                // Log the successful response in JSON format
                var timeTaken = DateTime.UtcNow - startTime;
                _logger.LogInformation("{RequestGuid} - Employee {EmployeeId} created successfully. Time taken: {TimeTaken}. Response: {@Employee}",
                    requestGuid, employee.EmployeeId, timeTaken, employee);

                return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employee);
            }
            catch (System.Exception ex)
            {
                // Log exception details in JSON format
                _logger.LogError(ex, "{RequestGuid} - Error creating employee {EmployeeId}. Client IP: {ClientIp}, Headers: {@Headers}",
                    requestGuid, employee.EmployeeId, clientIp, Request.Headers);
                return BadRequest(new { Message = "Invalid employee data.", RequestGuid = requestGuid });
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEmployee([FromRoute] Guid id, [FromBody] Employee employee)
        {
            var requestGuid = Request.Headers["Request-Guid"].ToString();  // Get the GUID from request headers
            var startTime = DateTime.UtcNow;  // Track request processing time

            // Get client IP address, handle possible null
            var clientIp = Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";  // Default to "Unknown" if null

            try
            {
                // Log the start of the request in JSON format
                _logger.LogInformation("{RequestGuid} - Processing PUT request to update employee {EmployeeId} at {StartTime}, Client IP: {ClientIp}, Headers: {@Headers}, RequestBody: {@Employee}",
                    requestGuid, employee.EmployeeId, startTime, clientIp, Request.Headers, employee);

                // Randomly choose an exception or continue with the request
                if (_random.Next(1, 101) % 3 != 0) // If not divisible by 3, throw a random exception
                {
                    ThrowRandomException();  // This method randomly throws an exception
                }
                employee.EmployeeId = id;
                _employeeService.UpdateEmployee(employee);

                // Log the successful response in JSON format
                var timeTaken = DateTime.UtcNow - startTime;
                _logger.LogInformation("{RequestGuid} - Employee {EmployeeId} updated successfully. Time taken: {TimeTaken}. Response: {@Employee}",
                    requestGuid, employee.EmployeeId, timeTaken, employee);

                return Ok(employee);
            }
            catch (System.Exception ex)
            {
                // Log exception details in JSON format
                _logger.LogError(ex, "{RequestGuid} - Error updating employee {EmployeeId}. Client IP: {ClientIp}, Headers: {@Headers}",
                    requestGuid, employee.EmployeeId, clientIp, Request.Headers);
                return NotFound(new { Message = "Employee not found.", RequestGuid = requestGuid });
            }
        }

        private void ThrowRandomException()
        {
            int exceptionChoice = _random.Next(1, 11); // Randomly choose an exception type (1-10)

            switch (exceptionChoice)
            {
                case 1:
                    throw new EmployeeNotFoundException("Employee not found in the system.");
                case 2:
                    throw new InvalidEmployeeDataException("Employee data is invalid.");
                case 3:
                    throw new TimeoutException("The operation timed out.");
                case 4:
                    throw new DatabaseConnectionException("Failed to connect to the database.");
                case 5:
                    throw new UnauthorizedAccessException("You are not authorized to perform this action.");
                case 6:
                    throw new InvalidOperationException("Invalid operation.");
                case 7:
                    throw new ArgumentNullException("One or more required arguments are null.");
                case 8:
                    throw new ArgumentOutOfRangeException("Argument is out of range.");
                case 9:
                    throw new FileNotFoundException("Required file was not found.");
                case 10:
                    throw new IndexOutOfRangeException("Index is out of range.");
            }
        }
    }



}
