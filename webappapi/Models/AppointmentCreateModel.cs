// Models/AppointmentCreateModel.cs
using System;
using System.ComponentModel.DataAnnotations;

public class AppointmentCreateModel
{
    [Required]
    public DateTime Date { get; set; }

    // Add any additional properties or validation attributes if needed
}