// Appointment.cs
using System;
using System.ComponentModel.DataAnnotations;

public class Appointment
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public DateTime Date { get; set; }

    // Add any additional properties or methods related to the appointment if needed
}