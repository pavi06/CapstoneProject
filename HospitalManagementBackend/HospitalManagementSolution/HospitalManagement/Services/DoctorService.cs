using HospitalManagement.CustomExceptions;
using HospitalManagement.Interfaces;
using HospitalManagement.Models;
using HospitalManagement.Models.DTOs.AppointmentDTOs;
using HospitalManagement.Models.DTOs.MedicalRecordDTOs;
using HospitalManagement.Models.DTOs.MedicineDTOs;
using HospitalManagement.Repositories;

namespace HospitalManagement.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IRepository<int, Appointment> _appointmentRepository;
        private readonly IRepository<int, User> _userDetailsRepository;
        private readonly IRepository<int, MedicalRecord> _medicalRecordsRepository;
        private readonly IRepository<int, Prescription> _prescriptionRepository;
        private readonly IRepository<int, Medication> _medicationRepository;

        public DoctorService(IRepository<int, Appointment> appointmentRepository, IRepository<int, User> userDetailsRepository, 
            IRepository<int, MedicalRecord> medicalRecordsRepository, IRepository<int, Prescription> prescriptionRepository,
            IRepository<int, Medication> medicationRepository)
        {
            _appointmentRepository = appointmentRepository;
            _userDetailsRepository = userDetailsRepository;
            _medicalRecordsRepository = medicalRecordsRepository;
            _prescriptionRepository = prescriptionRepository;
            _medicationRepository = medicationRepository;
        }

        public async Task<List<AppointmentReturnDTO>> GetAllTodayAppointments(int doctorid)
        {
            var appointments = _appointmentRepository.Get().Result.Where(a => a.DoctorId == doctorid && a.AppointmentDate == DateTime.Now.Date).ToList();
            if(appointments == null)
            {
                throw new ObjectsNotAvailableException("Appointments");
            }
            try
            {
                var appointmentList = await Task.WhenAll(appointments.Select(async a =>
                {
                    var patient = await _userDetailsRepository.Get(a.PatientId);
                    return new AppointmentReturnDTO(a.AppointmentId, a.AppointmentDate, a.Slot, a.PatientId, patient.Name, patient.Age, patient.ContactNo, a.Description, a.AppointmentType, a.AppointmentStatus);
                }));
                return appointmentList.ToList();
            }
            catch (ObjectNotAvailableException e)
            {
                throw;
            }
            
        }

        #region CancelAppointment
        public async Task<string> CancelAppointment(int appointmentId)
        {
            try
            {
                var appointment = await _appointmentRepository.Get(appointmentId);
                appointment.AppointmentStatus = "Cancelled";
                await _appointmentRepository.Update(appointment);
                return "Appointment status updated successfully!";
            }
            catch (ObjectNotAvailableException e)
            {
                throw;
            }
        }
        #endregion

        public async Task<List<MedicalRecordReturnDTO>> GetPatientMedicalRecords(int patientid, int doctorid)
        {
            var medicalRecords = _medicalRecordsRepository.Get().Result.Where(mr => mr.PatientId == patientid && mr.DoctorId == doctorid);
            if(medicalRecords == null)
            {
                throw new ObjectsNotAvailableException("Medical Records");
            }
            try
            {
                var medicalRecordList = await Task.WhenAll(medicalRecords.Select(async mr =>
                {
                    var patient = await _userDetailsRepository.Get(mr.PatientId);
                    return new MedicalRecordReturnDTO(mr.RecordId, mr.PatientId, patient.Name, patient.ContactNo, mr.Date, mr.Diagnosis, mr.Treatment, mr.Medication, mr.TreatmentStatus);
                }));
                return medicalRecordList.ToList();
            }
            
            catch(ObjectNotAvailableException e)
            {
                throw;
            }
        }

        public async Task<PrescriptionReturnDTO> ProvidePrescriptionForAppointment(ProvidePrescriptionDTO prescriptionDTO)
        {
            var prescriptionAlreadyAvailable = _prescriptionRepository.Get().Result.Where(p => p.PrescriptionFor == prescriptionDTO.PrescriptionFor).FirstOrDefault();
            try
            {
                if (prescriptionAlreadyAvailable == null)
                {
                    var prescription = await _prescriptionRepository.Add(new Prescription() { PrescriptionFor = prescriptionDTO.PrescriptionFor, PatientId = prescriptionDTO.PatientId, DoctorId = prescriptionDTO.DoctorId });
                    await Task.WhenAll(prescriptionDTO.prescribedMedicine.Select(async m =>
                    {
                        await _medicationRepository.Add(new Medication(m.MedicineName, m.Form, m.Dosage, m.Quantity, m.IntakeTiming, m.Intake, prescription.PrescriptionId));
                    }));
                    var patientDetails = await _userDetailsRepository.Get(prescription.PatientId);
                    return new PrescriptionReturnDTO(prescription.PrescriptionId, prescription.PrescriptionFor,prescription.PatientId,patientDetails.Name,  patientDetails.Age, _userDetailsRepository.Get(prescription.DoctorId).Result.Name, prescription.Doctor.Specialization, prescription.Medications);
                }
                else
                {
                    await Task.WhenAll(prescriptionDTO.prescribedMedicine.Select(async m =>
                    {
                        await _medicationRepository.Add(new Medication(m.MedicineName, m.Form, m.Dosage, m.Quantity, m.IntakeTiming, m.Intake, prescriptionAlreadyAvailable.PrescriptionId));
                    }));
                    var patientDetails = await _userDetailsRepository.Get(prescriptionAlreadyAvailable.PatientId);
                    return new PrescriptionReturnDTO(prescriptionAlreadyAvailable.PrescriptionId, prescriptionAlreadyAvailable.PrescriptionFor, prescriptionAlreadyAvailable.PatientId, patientDetails.Name, patientDetails.Age, _userDetailsRepository.Get(prescriptionAlreadyAvailable.DoctorId).Result.Name, prescriptionAlreadyAvailable.Doctor.Specialization, prescriptionAlreadyAvailable.Medications);
                }
            }
            catch(ObjectNotAvailableException e)
            {
                throw;
            }
            
        }
    }
}
