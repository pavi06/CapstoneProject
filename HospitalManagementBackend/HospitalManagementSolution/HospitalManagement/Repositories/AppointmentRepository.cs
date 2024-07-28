using HospitalManagement.Contexts;
using HospitalManagement.CustomExceptions;
using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositories
{
    public class AppointmentRepository : AbstractRepository<int, Appointment>
    {
        public AppointmentRepository(HospitalManagementContext context) : base(context)
        {
        }
        public override Task<Appointment> Add(Appointment item)
        {
            if(_context.Appointments.Any(a=>a.DoctorId == item.DoctorId && a.PatientId == item.PatientId && a.Slot == item.Slot && a.AppointmentDate == item.AppointmentDate))
            {
                throw new ObjectAlreadyExistsException("Appointment");
            }
            return base.Add(item);
        }

        public override async Task<Appointment> Delete(int key)
        {
            try
            {
                var appointment = await Get(key);
                _context.Entry<Appointment>(appointment).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return appointment;

            }
            catch (ObjectNotAvailableException)
            {
                throw;
            }
        }

        public override async Task<Appointment> Get(int key)
        {
            var appointment = await _context.Appointments.Include(a => a.Doctor).Include(a=>a.Patient).SingleOrDefaultAsync(a => a.AppointmentId == key);
            if (appointment == null)
                throw new ObjectNotAvailableException("Doctor");
            return appointment;
        }

        public override async Task<IEnumerable<Appointment>> Get()
        {
            var appointments = await _context.Appointments.Include(a => a.Doctor).Include(a => a.Patient).ToListAsync();
            return appointments;

        }

        public override async Task<Appointment> Update(Appointment item)
        {
            try
            {
                if (await Get(item.AppointmentId) != null)
                {
                    _context.Entry<Appointment>(item).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return item;
                }
                throw new ObjectNotAvailableException("Appointment");
            }
            catch (ObjectNotAvailableException)
            {
                throw;
            }

        }
    }
}
