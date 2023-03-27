﻿namespace TimeRegistrationAPI.Models;

public class Employees
{
    public int EmployeeID { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Email { get; set; }
    
    public string Phone { get; set; }
    
    public int TimeRegistration { get; set; }
    
}