using HospitalManagement.CustomExceptions;
using HospitalManagement.Interfaces;
using HospitalManagement.Models;
using HospitalManagement.Models.DTOs;

namespace HospitalManagement.Services
{
    public class PatientService : IPatientService
    {
        private readonly IRepository<int, Appointment> _appointmentRepository;
        private readonly IRepository<int, Doctor> _doctorRepository;
        private readonly IRepository<int, UserDetails> _userDetailsRepository;
        private readonly IRepository<int, Prescription> _prescriptionRepository;

        public PatientService(IRepository<int, Appointment> appointmentRepository, IRepository<int, Doctor> doctorRepository, IRepository<int, UserDetails> userDetailsRepository, IRepository<int, Prescription> prescriptionRepository) { 
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _userDetailsRepository = userDetailsRepository;
            _prescriptionRepository = prescriptionRepository;
        }
        public async Task<PatientAppointmentReturnDTO> BookAppointmentByDoctor(BookAppointmentDTO appointmentDTO)
        {
            var doctor = await _doctorRepository.Get(appointmentDTO.DoctorId);
            var appointment = await _appointmentRepository.Add(new Appointment(appointmentDTO.AppointmentDate, appointmentDTO.Slot, appointmentDTO.DoctorId, doctor.Specialization, appointmentDTO.PatientId, appointmentDTO.Description, "scheduled", appointmentDTO.AppointmentType, appointmentDTO.AppointmentMode));
            if(appointment == null)
            {
                throw new AppointmentAlreadyRaisedException();
            }
            var doctorDetails = await _userDetailsRepository.Get(appointment.DoctorId);
            return new PatientAppointmentReturnDTO(appointment.AppointmentId,appointment.AppointmentDate, appointment.Slot, appointment.Description, doctorDetails.Name,appointment.Speciality, appointment.AppointmentStatus, appointment.AppointmentType);
            //send notification to doctor
        }

        public Task<PatientAppointmentReturnDTO> BookAppointmentBySpeciality(BookAppointmentBySpecDTO specAppointmentDTO)
        {
            //var doctors = GetAllDoctorsAvailableOnThatSlot(specAppointmentDTO.Speciality, specAppointmentDTO.PreferredTime);
            throw new NotImplementedException();
        }

        public async Task<string> CancelAppointment(int appointmentId, string status)
        {
            var appointment = _appointmentRepository.Get(appointmentId).Result;
            appointment.AppointmentStatus = status;
            try
            {
                await _appointmentRepository.Update(appointment);
                return "Appointment Cancelled successfully";
            }
            catch (ObjectNotAvailableException e)
            {
                throw;
            }
            //send notification to doctor
        }

        public async Task<List<PatientAppointmentReturnDTO>> MyAppointments(int patientId, int limit, int skip)
        {
            var appointments = _appointmentRepository.Get().Result.Where(a=>a.PatientId == patientId).Skip(skip).Take(limit).ToList();
            var appointmentList = await Task.WhenAll(appointments.Select(async a =>
            {
                var doctorDetails = await _userDetailsRepository.Get(a.DoctorId);
                return new PatientAppointmentReturnDTO(a.AppointmentId, a.AppointmentDate, a.Slot, a.Description, doctorDetails.Name, a.Speciality, a.AppointmentStatus, a.AppointmentType);
            }));
            return appointmentList.ToList();
        }

        public async Task<Prescription> MyPrescriptionForAppointment(int patientId, int appointmentId)
        {
            var prescription = _prescriptionRepository.Get().Result.SingleOrDefault(p => p.AppointmentId == appointmentId && p.Appointment.PatientId == patientId);
            if(prescription == null)
            {
                throw new ObjectNotAvailableException("Prescription");
            }
            return prescription;
        }

        public async Task<List<Prescription>> MyPrescriptions(int patientId, int limit, int skip)
        {
            //check whether patientid needed in prescription
            var prescriptions = _prescriptionRepository.Get().Result.Where(p=>p.Appointment.PatientId == patientId).ToList();
            if (prescriptions == null)
            {
                throw new ObjectsNotAvailableException("Prescription");
            }
            return prescriptions;
        }
    }
}
