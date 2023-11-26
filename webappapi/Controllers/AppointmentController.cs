// AppointmentController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

[ApiController]
[Route("api/appointments")]
[Authorize]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAppointments()
    {
        var userId = User.FindFirst(ClaimTypes.Name)?.Value;
        var appointments = await _appointmentService.GetAppointmentsByUser(userId);
        return Ok(appointments);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAppointment(AppointmentCreateModel appointmentCreateModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirst(ClaimTypes.Name)?.Value;
        var appointment = await _appointmentService.CreateAppointment(userId, appointmentCreateModel);

        return CreatedAtAction(nameof(GetAppointments), new { id = appointment.Id }, appointment);
    }
}