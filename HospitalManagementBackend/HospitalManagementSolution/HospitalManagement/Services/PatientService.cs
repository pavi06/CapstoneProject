using HospitalManagement.CustomExceptions;
using HospitalManagement.Interfaces;
using HospitalManagement.Jobs;
using HospitalManagement.Models;
using HospitalManagement.Models.DTOs.AppointmentDTOs;
using HospitalManagement.Models.DTOs.DoctorDTOs;
using HospitalManagement.Models.DTOs.MedicalRecordDTOs;
using HospitalManagement.Models.DTOs.MedicineDTOs;
using HospitalManagement.Repositories;
using System.Numerics;
using System.Xml.Linq;

namespace HospitalManagement.Services
{
    public class PatientService : IPatientService
    {
        private readonly IRepository<int, Appointment> _appointmentRepository;
        private readonly IRepository<int, Doctor> _doctorRepository;
        private readonly IRepository<int, User> _userRepository;
        private readonly IRepository<int, Prescription> _prescriptionRepository;
        private readonly IRepository<int, Patient> _patientRepository;
        private readonly IRepositoryForCompositeKey<int,DateTime,DoctorAvailability> _doctorAvailabilityRepository;

        public PatientService(IRepository<int, Appointment> appointmentRepository, IRepository<int, Doctor> doctorRepository, 
            IRepository<int, User> userDetailsRepository, IRepository<int, Prescription> prescriptionRepository,
            IRepositoryForCompositeKey<int, DateTime, DoctorAvailability> doctorAvailabilityRepository, IRepository<int, Patient> outPatientRepository)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _userRepository = userDetailsRepository;
            _prescriptionRepository = prescriptionRepository;
            _doctorAvailabilityRepository = doctorAvailabilityRepository;
            _patientRepository = outPatientRepository;
        }
        public async Task<PatientAppointmentReturnDTO> BookAppointmentByDoctor(BookAppointmentDTO appointmentDTO)
        {
            try
            {
                var doctor = await _doctorRepository.Get(appointmentDTO.DoctorId);
                if (!doctor.AvailableDays.Contains(appointmentDTO.AppointmentDate.DayOfWeek.ToString()))
                {
                    throw new ObjectNotAvailableException("Doctor");
                }
                if(await _patientRepository.Get(appointmentDTO.PatientId) == null)
                {
                    await _patientRepository.Add(new Patient(appointmentDTO.PatientId));
                }
                Appointment appointment = null;
                try
                {
                    appointment = await _appointmentRepository.Add(new Appointment(appointmentDTO.AppointmentDate, TimeOnly.Parse(appointmentDTO.Slot), appointmentDTO.DoctorId, doctor.Specialization, appointmentDTO.PatientId, appointmentDTO.Description, "scheduled", appointmentDTO.AppointmentType, appointmentDTO.AppointmentMode));
                }
                catch(ObjectAlreadyExistsException e)
                {
                    throw new AppointmentAlreadyRaisedException();
                }
                var availability  = await _doctorAvailabilityRepository.Get(appointmentDTO.DoctorId, appointmentDTO.AppointmentDate);
                if(availability == null)
                {
                    await _doctorAvailabilityRepository.Add(new DoctorAvailability(doctor.DoctorId,appointment.AppointmentDate,doctor.Slots.Except(new List<TimeOnly>() { TimeOnly.Parse(appointmentDTO.Slot) }).ToList()));
                }
                else
                {
                    availability.AvailableSlots.Remove(TimeOnly.Parse(appointmentDTO.Slot));
                    await _doctorAvailabilityRepository.Update(availability);
                }
                var doctorDetails = await _userRepository.Get(appointment.DoctorId);
                if (appointmentDTO.AppointmentMode == "Online")
                {
                    //generate meet link 
                    var meetLink = await ScheduledTask.SendEmail(_userRepository.Get(appointmentDTO.PatientId).Result.Name, appointmentDTO.AppointmentDate, TimeOnly.Parse(appointmentDTO.Slot), (DateTime.Now.Date-appointmentDTO.AppointmentDate.Date).Days);
                    return new PatientOnlineAppointmentReturnDTO(appointment.AppointmentId, appointment.AppointmentDate, appointment.Slot.ToString(), appointment.Description, doctorDetails.Name, appointment.Speciality, appointment.AppointmentStatus, appointment.AppointmentType, meetLink);
                }
                BackgroundJobs.NotificationForDoctor(doctorDetails.Name, doctorDetails.ContactNo, appointment.AppointmentDate, appointment.Slot);
                return new PatientAppointmentReturnDTO(appointment.AppointmentId, appointment.AppointmentDate, appointment.Slot.ToString(), appointment.Description, doctorDetails.Name, appointment.Speciality, appointment.AppointmentStatus, appointment.AppointmentType);
            }
            catch(ObjectNotAvailableException e)
            {
                await RevertAppointmentAdded(appointmentDTO);
                throw;
            }

        }

        public async Task RevertAppointmentAdded(BookAppointmentDTO appointmentDTO)
        {
            var appointment = _appointmentRepository.Get().Result.SingleOrDefault(a=>a.PatientId == appointmentDTO.PatientId && a.DoctorId == appointmentDTO.DoctorId
            && a.AppointmentDate == appointmentDTO.AppointmentDate && a.Date == DateTime.Now.Date && a.Slot == TimeOnly.Parse(appointmentDTO.Slot));
            await _appointmentRepository.Delete(appointment.AppointmentId);
            var availability = _doctorAvailabilityRepository.Get(appointment.DoctorId, appointment.AppointmentDate).Result;
            availability.AvailableSlots.Add(TimeOnly.Parse(appointmentDTO.Slot));
            await _doctorAvailabilityRepository.Update(availability);
        }

        public async Task<Doctor> GetDoctorAvailableOnThatSlot(string speciality, string preferredTime,string preferredLamguage, DateTime appointmentDate)
        {
            var doctors = new List<Doctor>();
            switch (preferredTime.ToLower())
            {
                case "forenoon":
                    doctors = _doctorRepository.Get().Result.Where(d => d.Specialization == speciality && d.ShiftStartTime >= new TimeOnly(7, 0, 0) && d.ShiftEndTime <= new TimeOnly(12, 0, 0) && d.LanguagesKnown.Contains(preferredLamguage) && d.AvailableDays.Contains(appointmentDate.DayOfWeek.ToString())).OrderByDescending(d=>d.Experience).ToList();
                    break;
                case "afternoon":
                    doctors = _doctorRepository.Get().Result.Where(d => d.Specialization == speciality && d.ShiftStartTime >= new TimeOnly(14, 0, 0) && d.ShiftEndTime <= new TimeOnly(18, 0, 0) && d.LanguagesKnown.Contains(preferredLamguage) && d.AvailableDays.Contains(appointmentDate.DayOfWeek.ToString())).OrderByDescending(d => d.Experience).ToList();
                    break;
                case "evening":
                    doctors = _doctorRepository.Get().Result.Where(d => d.Specialization == speciality && d.ShiftStartTime >= new TimeOnly(18, 0, 0) && d.ShiftEndTime <= new TimeOnly(22, 0, 0) && d.LanguagesKnown.Contains(preferredLamguage) && d.AvailableDays.Contains(appointmentDate.DayOfWeek.ToString())).OrderByDescending(d => d.Experience).ToList();
                    break;
                default:
                    throw new InvalidInputException();                    
            }
            foreach (var doctor in doctors)
            {
                var availability = await _doctorAvailabilityRepository.Get(doctor.DoctorId, appointmentDate);
                if (availability == null || (availability!=null && availability.AvailableSlots.Count > 0))
                {
                        return doctor;
                }
            }
            return null;
        }

        public async Task<Appointment> ChooseSlot(Doctor doctor, BookAppointmentBySpecDTO specAppointmentDTO)
        {
            var res = await _doctorAvailabilityRepository.Get(doctor.DoctorId, specAppointmentDTO.AppointmentDate);
            var appointment = new Appointment();
            if (res == null)
            {
                try
                {
                    appointment = await _appointmentRepository.Add(new Appointment(specAppointmentDTO.AppointmentDate, doctor.Slots[0], doctor.DoctorId, specAppointmentDTO.Speciality, specAppointmentDTO.PatientId, specAppointmentDTO.Description, "scheduled", specAppointmentDTO.AppointmentType, specAppointmentDTO.AppointmentMode));
                    await _doctorAvailabilityRepository.Add(new DoctorAvailability(doctor.DoctorId, appointment.AppointmentDate, doctor.Slots.Skip(1).ToList()));
                }
                catch (ObjectAlreadyExistsException e)
                {
                    if (appointment.AppointmentId > 1)
                    {
                        await _appointmentRepository.Delete(appointment.AppointmentId);
                    }
                    throw;
                }                
            }
            else
            {
                if (res.AvailableSlots.FirstOrDefault() == null)
                {
                    throw new ObjectsNotAvailableException("slots");
                }
                try
                {
                    appointment = await _appointmentRepository.Add(new Appointment(specAppointmentDTO.AppointmentDate, res.AvailableSlots.First(), doctor.DoctorId, specAppointmentDTO.Speciality, specAppointmentDTO.PatientId, specAppointmentDTO.Description, "scheduled", specAppointmentDTO.AppointmentType, specAppointmentDTO.AppointmentMode));
                    res.AvailableSlots.Remove(res.AvailableSlots.FirstOrDefault());
                    await _doctorAvailabilityRepository.Update(res);
                }
                catch (Exception e)
                {
                    if (appointment.AppointmentId > 1)
                    {
                        await _appointmentRepository.Delete(appointment.AppointmentId);
                        res.AvailableSlots.Add(appointment.Slot);
                        await _doctorAvailabilityRepository.Update(res);
                    }
                    throw;
                }
               
            }
            return appointment; 
        }

        public async Task<PatientAppointmentReturnDTO> BookAppointmentBySpeciality(BookAppointmentBySpecDTO specAppointmentDTO)
        {
            var doctor = await GetDoctorAvailableOnThatSlot(specAppointmentDTO.Speciality, specAppointmentDTO.PreferredTime,specAppointmentDTO.PreferredLanguage, specAppointmentDTO.AppointmentDate);
            if(doctor == null)
            {
                throw new ObjectsNotAvailableException("Doctor");
            }
            var appointment = await ChooseSlot(doctor, specAppointmentDTO);
            if (specAppointmentDTO.AppointmentMode == "Online")
            {
                var patientDetails = await _userRepository.Get(specAppointmentDTO.PatientId);
                ScheduledTask.SendEmail(patientDetails.Name, specAppointmentDTO.AppointmentDate,appointment.Slot,(DateTime.Now.Date-appointment.AppointmentDate).Days);
                //generate meet link 
                var meetLink = "";
                return new PatientOnlineAppointmentReturnDTO(appointment.AppointmentId, appointment.AppointmentDate, appointment.Slot.ToString(), appointment.Description, _userRepository.Get(appointment.DoctorId).Result.Name, appointment.Speciality, appointment.AppointmentStatus, appointment.AppointmentType, meetLink);
            }
            var doctorDetails = await _userRepository.Get(doctor.DoctorId);
            BackgroundJobs.NotificationForDoctor(doctorDetails.Name, doctorDetails.ContactNo, appointment.AppointmentDate, appointment.Slot);
            return new PatientAppointmentReturnDTO(appointment.AppointmentId, appointment.AppointmentDate, appointment.Slot.ToString(), appointment.Description, _userRepository.Get(appointment.DoctorId).Result.Name, appointment.Speciality, appointment.AppointmentStatus, appointment.AppointmentType);                    
        }

        public async Task<string> CancelAppointment(int appointmentId)
        {
            var appointment = _appointmentRepository.Get(appointmentId).Result;
            if(appointment.AppointmentStatus == "Cancelled")
            {
                throw new AppointmentAlreadyCancelledException();
            }
            appointment.AppointmentStatus = "Cancelled";
            try
            {
                await _appointmentRepository.Update(appointment);
                var docAvailability = await _doctorAvailabilityRepository.Get(appointment.DoctorId, appointment.AppointmentDate);
                if(docAvailability == null)
                {
                    throw new ObjectNotAvailableException("DoctorAvailability");
                }
                docAvailability.AvailableSlots.Add(appointment.Slot);
                await _doctorAvailabilityRepository.Update(docAvailability);
                var doctorDetails = await _userRepository.Get(appointment.DoctorId);
                BackgroundJobs.CancelNotificationForDoctor(doctorDetails.Name, doctorDetails.ContactNo, appointment.AppointmentDate, appointment.Slot);
                return "Appointment Cancelled successfully";
            }
            catch (ObjectNotAvailableException e)
            {
                appointment.AppointmentStatus = "scheduled";
                await _appointmentRepository.Update(appointment);
                throw;
            }
        }

        public async Task<List<PatientAppointmentReturnDTO>> MyAppointments(int patientId, int limit, int skip)
        {
            var appointments = _appointmentRepository.Get().Result.Where(a=>a.PatientId == patientId).Skip(skip).Take(limit).ToList();
            if(appointments == null)
            {
                throw new ObjectsNotAvailableException("Appointments");
            }
            try
            {
                var appointmentList = await Task.WhenAll(appointments.Select(async a =>
                {
                    var doctorDetails = await _userRepository.Get(a.DoctorId);
                    return new PatientAppointmentReturnDTO(a.AppointmentId, a.AppointmentDate, a.Slot.ToString(), a.Description, doctorDetails.Name, a.Speciality, a.AppointmentStatus, a.AppointmentType);
                }));
                return appointmentList.ToList();
            }
            catch (ObjectNotAvailableException e)
            {
                throw;
            }
            
        }

        public async Task<PrescriptionReturnDTO> MyPrescriptionForAppointment(int patientId, int appointmentId)
        {
            var prescription = _prescriptionRepository.Get().Result.SingleOrDefault(p => p.PrescriptionFor == appointmentId && p.PatientId == patientId);
            if(prescription == null)
            {
                throw new ObjectNotAvailableException("Prescription");
            }
            try
            {
                var patientDetails = await _userRepository.Get(prescription.PatientId);
                var doctorDetails = await _userRepository.Get(prescription.DoctorId);
                return new PrescriptionReturnDTO(prescription.PrescriptionId, prescription.PrescriptionFor,prescription.PatientId,patientDetails.Name, patientDetails.Age,doctorDetails.Name,prescription.Doctor.Specialization, await MapMedicationToDTO(prescription.Medications));
            }
            catch (ObjectNotAvailableException e)
            {
                throw;
            }
        }

        public async Task<List<PrescriptionReturnDTO>> MyPrescriptions(int patientId, int limit, int skip)
        {
            var prescriptions = _prescriptionRepository.Get().Result.Where(p=>p.PatientId == patientId).ToList();
            if (prescriptions == null)
            {
                throw new ObjectsNotAvailableException("Prescription");
            }
            try
            {
                var prescriptionList = await Task.WhenAll(prescriptions.Select(async prescription =>
                {
                    var patientDetails = await _userRepository.Get(prescription.PatientId);
                    var doctorDetails = await _userRepository.Get(prescription.DoctorId);
                    return new PrescriptionReturnDTO(prescription.PrescriptionId, prescription.PrescriptionFor, prescription.PatientId ,patientDetails.Name, patientDetails.Age,doctorDetails.Name, prescription.Doctor.Specialization, await MapMedicationToDTO(prescription.Medications));
                }));
                return prescriptionList.ToList();
            }
            catch (ObjectNotAvailableException e)
            {
                throw;
            }
        }

        public async Task<Dictionary<string, bool>> GetAvailableSlotsOfDoctor(CheckDoctorSlotsDTO checkSlotsDTO)
        {
            var doctor = await _doctorRepository.Get(checkSlotsDTO.DoctorId);
            if (!doctor.AvailableDays.Contains(checkSlotsDTO.Date.DayOfWeek.ToString()))
            {
                return new Dictionary<string, bool>();
            }
            Dictionary<string, bool> availableSlots = new Dictionary<string, bool>();
            var availability = await _doctorAvailabilityRepository.Get(doctor.DoctorId, checkSlotsDTO.Date);
            if (availability != null)
            {
                if (availability.AvailableSlots.Count > 0)
                {
                    foreach(var slot in availability.AvailableSlots)
                    {
                        availableSlots.Add(slot.ToString(), true);
                    }
                    foreach (var slot in doctor.Slots.Except(availability.AvailableSlots))
                    {
                        availableSlots.Add(slot.ToString(), false);
                    }
                }
            }
            else
            {
                foreach (var slot in doctor.Slots)
                {
                    availableSlots.Add(slot.ToString(), true);
                }
            }
            return availableSlots;
        }

        public async Task<List<MedicationMapperDTO>> MapMedicationToDTO(List<Medication> prescription)
        {
            var medicationsList = prescription
                    .Select(m => {
                        return new MedicationMapperDTO(
                        m.MedicineName,
                        m.Form,
                        m.Dosage,
                        m.Quantity,
                        m.IntakeTiming,
                        m.Intake,
                        m.MedicationId,
                        m.PrescriptionId);
                    }).ToList();
            return medicationsList;
        }
    }
}
