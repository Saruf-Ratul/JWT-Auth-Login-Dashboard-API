// IAppointmentService.cs
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAppointmentService
{
    Task<List<Appointment>> GetAppointmentsByUser(string userId);

    Task<Appointment> CreateAppointment(string userId, AppointmentCreateModel appointmentCreateModel);
}