namespace EmployeeManagementAPI.Model
{
    public class Employee
    {
        public Guid EmployeeId { get; set; }  // Unique identifier for each employee
        public string? Name { get; set; }      // Employee's name
        public string? SSN { get; set; }       // Social Security Number (or any unique identifier)
        public DateTime DateOfBirth { get; set; }  // Date of birth
        public decimal Salary { get; set; }   // Salary
        public string? Designation { get; set; }  // Job title or position
    }
}
