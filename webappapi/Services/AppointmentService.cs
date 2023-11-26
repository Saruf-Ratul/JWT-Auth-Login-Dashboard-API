// AppointmentService.cs
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AppointmentService : IAppointmentService
{
    private readonly AppDbContext _dbContext;

    public AppointmentService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Appointment>> GetAppointmentsByUser(string userId)
    {
        return await _dbContext.Appointments.Where(a => a.UserId == userId).ToListAsync();
    }

    public async Task<Appointment> CreateAppointment(string userId, AppointmentCreateModel appointmentCreateModel)
    {
        var appointment = new Appointment
        {
            UserId = userId,
            Date = appointmentCreateModel.Date,
            // Other properties
        };

        _dbContext.Appointments.Add(appointment);
        await _dbContext.SaveChangesAsync();

        return appointment;
    }
}