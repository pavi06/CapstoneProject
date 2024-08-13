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
            Appointment appointment = null;
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
                try
                {
                    if (appointmentDTO.AppointmentMode.ToLower() == "online")
                    {
                        var meetLink = "demoMeetLink";
                        appointment = await _appointmentRepository.Add(new Appointment(appointmentDTO.AppointmentDate, TimeOnly.Parse(appointmentDTO.Slot), appointmentDTO.DoctorId, doctor.Specialization, appointmentDTO.PatientId, appointmentDTO.Description, "scheduled", appointmentDTO.AppointmentType, appointmentDTO.AppointmentMode, meetLink));
                    }
                    else
                    {
                        appointment = await _appointmentRepository.Add(new Appointment(appointmentDTO.AppointmentDate, TimeOnly.Parse(appointmentDTO.Slot), appointmentDTO.DoctorId, doctor.Specialization, appointmentDTO.PatientId, appointmentDTO.Description, "scheduled", appointmentDTO.AppointmentType, appointmentDTO.AppointmentMode));
                    }
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
                    return new PatientAppointmentReturnDTO(appointment.AppointmentId, appointment.AppointmentDate, appointment.Slot.ToString(), appointment.Description, doctorDetails.Name, appointment.Speciality, appointment.AppointmentStatus, appointment.AppointmentMode, appointment.AppointmentType, appointment.MeetLink);
                }
                BackgroundJobs.NotificationForDoctor(doctorDetails.Name, doctorDetails.ContactNo, appointment.AppointmentDate, appointment.Slot);
                return new PatientAppointmentReturnDTO(appointment.AppointmentId, appointment.AppointmentDate, appointment.Slot.ToString(), appointment.Description, doctorDetails.Name, appointment.Speciality, appointment.AppointmentStatus, appointment.AppointmentMode, appointment.AppointmentType);
            }
            catch(ObjectNotAvailableException e)
            {
                if (appointment != null)
                {
                    await RevertAppointmentAdded(appointment.AppointmentId);
                }
                throw;
            }

        }

        public async Task RevertAppointmentAdded(int appointmentId)
        {
            var appointment = await _appointmentRepository.Get(appointmentId);
            await _appointmentRepository.Delete(appointment.AppointmentId);
            try
            {
                var availability = _doctorAvailabilityRepository.Get(appointment.DoctorId, appointment.AppointmentDate).Result;
                availability.AvailableSlots.Add(appointment.Slot);
                await _doctorAvailabilityRepository.Update(availability);
            }
            catch(Exception e)
            {
                throw;
            }           
        }


        public async Task<Doctor> ChooseSlotFromDoctors(List<Doctor> doctorsAvailable,DateTime date ,TimeOnly preferredTime)
        {
            Doctor doctorAvailable = null;
            foreach (var doctor in doctorsAvailable)
            {
                var availability = _doctorAvailabilityRepository.Get(doctor.DoctorId, date).Result;
                if(availability != null)
                {
                    doctorAvailable = availability.AvailableSlots.Contains(preferredTime) ? doctor : null;
                }
                else
                {
                    doctorAvailable = doctor.Slots.Contains(preferredTime)? doctor : null;
                }
            }
            return doctorAvailable;
        }

        public async Task<PatientAppointmentReturnDTO> BookAppointmentBySpeciality(BookAppointmentBySpecDTO specAppointmentDTO)
        {
            var doctorsAvailable = _doctorRepository.Get().Result.Where(d => d.Specialization.ToLower() == specAppointmentDTO.Speciality.ToLower() && d.AvailableDays.Contains(specAppointmentDTO.AppointmentDate.DayOfWeek.ToString()) && d.LanguagesKnown.Contains(specAppointmentDTO.PreferredLanguage)).ToList();
            var doctor = await ChooseSlotFromDoctors(doctorsAvailable, specAppointmentDTO.AppointmentDate , TimeOnly.Parse(specAppointmentDTO.PreferredTime));
            if (doctor == null)
            {
                throw new ObjectsNotAvailableException("Doctor");
            }
            Appointment appointment = null;
            if (specAppointmentDTO.AppointmentMode == "Online")
            {
                var meetLink = "demomeetlink";
                appointment = await _appointmentRepository.Add(new Appointment(specAppointmentDTO.AppointmentDate, TimeOnly.Parse(specAppointmentDTO.PreferredTime), doctor.DoctorId, doctor.Specialization, specAppointmentDTO.PatientId, specAppointmentDTO.Description, "scheduled", specAppointmentDTO.AppointmentType, specAppointmentDTO.AppointmentMode, meetLink));
                return new PatientAppointmentReturnDTO(appointment.AppointmentId, appointment.AppointmentDate, appointment.Slot.ToString(), appointment.Description, _userRepository.Get(appointment.DoctorId).Result.Name, appointment.Speciality, appointment.AppointmentStatus, appointment.AppointmentMode, appointment.AppointmentType, meetLink);
            }
            var doctorDetails = await _userRepository.Get(doctor.DoctorId);
            appointment = await _appointmentRepository.Add(new Appointment(specAppointmentDTO.AppointmentDate, TimeOnly.Parse(specAppointmentDTO.PreferredTime), doctor.DoctorId, doctor.Specialization, specAppointmentDTO.PatientId, specAppointmentDTO.Description, "scheduled", specAppointmentDTO.AppointmentType, specAppointmentDTO.AppointmentMode));
            BackgroundJobs.NotificationForDoctor(doctorDetails.Name, doctorDetails.ContactNo, appointment.AppointmentDate, appointment.Slot);
            return new PatientAppointmentReturnDTO(appointment.AppointmentId, appointment.AppointmentDate, appointment.Slot.ToString(), appointment.Description, _userRepository.Get(appointment.DoctorId).Result.Name, appointment.Speciality, appointment.AppointmentStatus, appointment.AppointmentMode, appointment.AppointmentType);
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
                var appointmentList = appointments.Select(a =>
                {
                    var doctorDetails = _userRepository.Get(a.DoctorId).Result;
                    if (a.AppointmentMode.ToLower() == "online")
                    {
                        return new PatientAppointmentReturnDTO(a.AppointmentId, a.AppointmentDate, a.Slot.ToString(), a.Description, doctorDetails.Name, a.Speciality, a.AppointmentStatus, a.AppointmentMode, a.AppointmentType, a.MeetLink);
                    }
                    return new PatientAppointmentReturnDTO(a.AppointmentId, a.AppointmentDate, a.Slot.ToString(), a.Description, doctorDetails.Name, a.Speciality, a.AppointmentStatus, a.AppointmentMode, a.AppointmentType);
                }).ToList();
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

        public async Task<List<PrescriptionsReturnDTO>> MyPrescriptions(int patientId, int limit, int skip)
        {
            var prescriptions = _prescriptionRepository.Get().Result.Where(p=>p.PatientId == patientId).Skip(skip).Take(limit).ToList();
            if (prescriptions == null)
            {
                throw new ObjectsNotAvailableException("Prescription");
            }
            try
            {
                var prescriptionList = prescriptions.Select(prescription =>
                {
                    var patientDetails = _userRepository.Get(prescription.PatientId).Result;
                    var doctorDetails = _userRepository.Get(prescription.DoctorId).Result;
                    return new PrescriptionsReturnDTO(prescription.PrescriptionId, prescription.Date, prescription.PrescriptionFor);
                }).ToList();
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
                        if (checkSlotsDTO.Date == DateTime.Now.Date)
                        {
                            if(slot > TimeOnly.FromTimeSpan(DateTime.Now.TimeOfDay))
                            {
                                availableSlots.Add(slot.ToString(), true);
                            }
                        }
                        else
                        {
                            availableSlots.Add(slot.ToString(), true);
                        }
                    }
                    foreach (var slot in doctor.Slots.Except(availability.AvailableSlots))
                    {
                        if (checkSlotsDTO.Date == DateTime.Now.Date)
                        {
                            if (slot > TimeOnly.FromTimeSpan(DateTime.Now.TimeOfDay))
                            {
                                availableSlots.Add(slot.ToString(), false);
                            }
                        }
                        else
                        {
                            availableSlots.Add(slot.ToString(), false);
                        }

                    }
                }
            }
            else
            {
                foreach (var slot in doctor.Slots)
                {
                    if (checkSlotsDTO.Date == DateTime.Now.Date)
                    {
                        if (slot > TimeOnly.FromTimeSpan(DateTime.Now.TimeOfDay))
                        {
                            availableSlots.Add(slot.ToString(), true);
                        }
                    }
                    else
                    {
                        availableSlots.Add(slot.ToString(), true);
                    }
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
                        m.IntakeTiming,
                        m.Intake,
                        m.MedicationId,
                        m.PrescriptionId);
                    }).ToList();
            return medicationsList;
        }
    }
}
