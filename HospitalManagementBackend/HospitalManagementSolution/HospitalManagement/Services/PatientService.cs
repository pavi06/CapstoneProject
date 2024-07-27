﻿using HospitalManagement.CustomExceptions;
using HospitalManagement.Interfaces;
using HospitalManagement.Models;
using HospitalManagement.Models.DTOs.AppointmentDTOs;
using HospitalManagement.Models.DTOs.MedicalRecordDTOs;
using System.Numerics;

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
            IRepositoryForCompositeKey<int, DateTime, DoctorAvailability> doctorAvailabilityRepository, IRepository<int, Patient> outPatientRepository) { 
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
                //check this!!
                if (!doctor.AvailableDays.Contains(appointmentDTO.AppointmentDate.DayOfWeek.ToString()))
                {
                    throw new ObjectNotAvailableException("Doctor");
                }
                if(await _patientRepository.Get(appointmentDTO.PatientId) == null)
                {
                    await _patientRepository.Add(new Patient(appointmentDTO.PatientId));
                }                
                var appointment = await _appointmentRepository.Add(new Appointment(appointmentDTO.AppointmentDate, TimeOnly.Parse(appointmentDTO.Slot), appointmentDTO.DoctorId, doctor.Specialization, appointmentDTO.PatientId, appointmentDTO.Description, "scheduled", appointmentDTO.AppointmentType, appointmentDTO.AppointmentMode));
                var availability  = await _doctorAvailabilityRepository.Get(appointmentDTO.DoctorId, appointmentDTO.AppointmentDate);
                if(availability == null)
                {
                    await _doctorAvailabilityRepository.Add(new DoctorAvailability(doctor.DoctorId, doctor.Slots.Except(new List<TimeOnly>() { TimeOnly.Parse(appointmentDTO.Slot) }).ToList()));
                }
                else
                {
                    availability.AvailableSlots.Remove(TimeOnly.Parse(appointmentDTO.Slot));
                }
                await _doctorAvailabilityRepository.Update(availability);
                if (appointment == null)
                {
                    throw new AppointmentAlreadyRaisedException();
                }
                var doctorDetails = await _userRepository.Get(appointment.DoctorId);
                if (appointmentDTO.AppointmentMode == "Online")
                {
                    //generate meet link 
                    var meetLink = "";
                    return new PatientOnlineAppointmentReturnDTO(appointment.AppointmentId, appointment.AppointmentDate, appointment.Slot, appointment.Description, doctorDetails.Name, appointment.Speciality, appointment.AppointmentStatus, appointment.AppointmentType, meetLink);
                }
                return new PatientAppointmentReturnDTO(appointment.AppointmentId, appointment.AppointmentDate, appointment.Slot, appointment.Description, doctorDetails.Name, appointment.Speciality, appointment.AppointmentStatus, appointment.AppointmentType);
                //send notification to doctor
            }
            catch(ObjectNotAvailableException e)
            {
                throw;
            }

        }

        public async Task<Doctor> GetDoctorAvailableOnThatSlot(string speciality, string preferredTime, DateTime appointmentDate)
        {
            var doctors = new List<Doctor>();
            switch (preferredTime.ToLower())
            {
                case "forenoon":
                    doctors = _doctorRepository.Get().Result.Where(d => d.Specialization == speciality && d.ShiftStartTime >= new TimeOnly(7, 0, 0) && d.ShiftEndTime < new TimeOnly(12, 0, 0)).OrderByDescending(d=>d.Experience).ToList();
                    break;
                case "afternoon":
                    doctors = _doctorRepository.Get().Result.Where(d => d.Specialization == speciality && d.ShiftStartTime >= new TimeOnly(14, 0, 0) && d.ShiftEndTime < new TimeOnly(18, 0, 0)).OrderByDescending(d => d.Experience).ToList();
                    break;
                case "evening":
                    doctors = _doctorRepository.Get().Result.Where(d => d.Specialization == speciality && d.ShiftStartTime >= new TimeOnly(18, 0, 0) && d.ShiftEndTime <= new TimeOnly(22, 0, 0)).OrderByDescending(d => d.Experience).ToList();
                    break;
                default:
                    throw new InvalidInputException();                    
            }
            foreach (var doctor in doctors)
            {
                var availability = await _doctorAvailabilityRepository.Get(doctor.DoctorId, appointmentDate);
                if (availability != null)
                {
                    if (availability.AvailableSlots.Count > 0)
                    {
                        return doctor;
                    }
                };
            }
            return null;
        }

        public async Task<Appointment> ChooseSlot(Doctor doctor, BookAppointmentBySpecDTO specAppointmentDTO)
        {
            var res = await _doctorAvailabilityRepository.Get(doctor.DoctorId, specAppointmentDTO.AppointmentDate);
            var appointment = new Appointment();
            if (res == null)
            {
                appointment = await _appointmentRepository.Add(new Appointment(specAppointmentDTO.AppointmentDate, doctor.Slots[0], doctor.DoctorId, specAppointmentDTO.Speciality, specAppointmentDTO.PatientId, specAppointmentDTO.Description, "scheduled", specAppointmentDTO.AppointmentType, specAppointmentDTO.AppointmentMode));
                await _doctorAvailabilityRepository.Add(new DoctorAvailability(doctor.DoctorId, doctor.Slots.Skip(1).ToList()));
            }
            else
            {
                if (res.AvailableSlots.FirstOrDefault() == null)
                {
                    throw new ObjectsNotAvailableException("slots");
                }
                appointment = await _appointmentRepository.Add(new Appointment(specAppointmentDTO.AppointmentDate, res.AvailableSlots.FirstOrDefault(), doctor.DoctorId, specAppointmentDTO.Speciality, specAppointmentDTO.PatientId, specAppointmentDTO.Description, "scheduled", specAppointmentDTO.AppointmentType, specAppointmentDTO.AppointmentMode));
                res.AvailableSlots.Remove(res.AvailableSlots.FirstOrDefault());
                await _doctorAvailabilityRepository.Update(res);
            }
            return appointment; 
        }

        public async Task<PatientAppointmentReturnDTO> BookAppointmentBySpeciality(BookAppointmentBySpecDTO specAppointmentDTO)
        {
            var doctor = await GetDoctorAvailableOnThatSlot(specAppointmentDTO.Speciality, specAppointmentDTO.PreferredTime, specAppointmentDTO.AppointmentDate);
            if(doctor == null)
            {
                throw new ObjectsNotAvailableException("Doctor");
            }
            var appointment = await ChooseSlot(doctor, specAppointmentDTO);
            if (specAppointmentDTO.AppointmentMode == "Online")
            {
                //generate meet link 
                var meetLink = "";
                return new PatientOnlineAppointmentReturnDTO(appointment.AppointmentId, appointment.AppointmentDate, appointment.Slot, appointment.Description, _userRepository.Get(appointment.DoctorId).Result.Name, appointment.Speciality, appointment.AppointmentStatus, appointment.AppointmentType, meetLink);
            }
            return new PatientAppointmentReturnDTO(appointment.AppointmentId, appointment.AppointmentDate, appointment.Slot, appointment.Description, _userRepository.Get(appointment.DoctorId).Result.Name, appointment.Speciality, appointment.AppointmentStatus, appointment.AppointmentType);            
            //send notification        
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
                docAvailability.AvailableSlots.Add(appointment.Slot);
                await _doctorAvailabilityRepository.Update(docAvailability);
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
            if(appointments == null)
            {
                throw new ObjectsNotAvailableException("Appointments");
            }
            try
            {
                var appointmentList = await Task.WhenAll(appointments.Select(async a =>
                {
                    var doctorDetails = await _userRepository.Get(a.DoctorId);
                    return new PatientAppointmentReturnDTO(a.AppointmentId, a.AppointmentDate, a.Slot, a.Description, doctorDetails.Name, a.Speciality, a.AppointmentStatus, a.AppointmentType);
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
                return new PrescriptionReturnDTO(prescription.PrescriptionId, prescription.PrescriptionFor,prescription.PatientId,patientDetails.Name, patientDetails.Age,doctorDetails.Name,prescription.Doctor.Specialization, prescription.Medications);
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
                var prescriptionList = await Task.WhenAll(prescriptions.Select(async p =>
                {
                    var patientDetails = await _userRepository.Get(p.PatientId);
                    var doctorDetails = await _userRepository.Get(p.DoctorId);
                    return new PrescriptionReturnDTO(p.PrescriptionId, p.PrescriptionFor,p.PatientId ,patientDetails.Name, patientDetails.Age,doctorDetails.Name, p.Doctor.Specialization, p.Medications);
                }));
                return prescriptionList.ToList();
            }
            catch (ObjectNotAvailableException e)
            {
                throw;
            }
        }
    }
}
