using Hangfire;
using HospitalManagement.CustomExceptions;
using HospitalManagement.Interfaces;
using HospitalManagement.Jobs;
using HospitalManagement.Models;
using HospitalManagement.Models.DTOs.AppointmentDTOs;
using HospitalManagement.Models.DTOs.MedicalRecordDTOs;
using HospitalManagement.Models.DTOs.MedicineDTOs;
using HospitalManagement.Repositories;
using System.Linq;

namespace HospitalManagement.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IRepository<int, Appointment> _appointmentRepository;
        private readonly IRepository<int, User> _userDetailsRepository;
        private readonly IRepository<int, Doctor> _doctorRepository;
        private readonly IRepository<int, MedicalRecord> _medicalRecordsRepository;
        private readonly IRepository<int, Prescription> _prescriptionRepository;
        private readonly IRepository<int, Medication> _medicationRepository;

        public DoctorService(IRepository<int, Appointment> appointmentRepository, IRepository<int, User> userDetailsRepository, 
            IRepository<int, MedicalRecord> medicalRecordsRepository, IRepository<int, Prescription> prescriptionRepository,
            IRepository<int, Medication> medicationRepository, IRepository<int, Doctor> doctorRepository)
        {
            _appointmentRepository = appointmentRepository;
            _userDetailsRepository = userDetailsRepository;
            _medicalRecordsRepository = medicalRecordsRepository;
            _prescriptionRepository = prescriptionRepository;
            _medicationRepository = medicationRepository;
            _doctorRepository = doctorRepository;
        }

        public async Task<List<DoctorAppointmentReturnDTO>> GetAllScheduledAppointments(int doctorid)
        {
            var appointments = _appointmentRepository.Get().Result.Where(a => a.DoctorId == doctorid && (a.AppointmentDate > DateTime.Now.Date || (a.AppointmentDate == DateTime.Now.Date && a.Slot.Add(TimeSpan.FromMinutes(30)) > TimeOnly.FromTimeSpan(DateTime.Now.TimeOfDay)))).ToList();
            if(appointments == null)
            {
                throw new ObjectsNotAvailableException("Appointments");
            }
            try
            {
                var appointmentList = appointments.Select( a =>
                {
                    var patient = _userDetailsRepository.Get(a.PatientId).Result;
                    var prescription = _prescriptionRepository.Get().Result.SingleOrDefault(p => p.PrescriptionFor == a.AppointmentId);
                    var prescriptionStatus = false;
                    if(prescription != null)
                    {
                        prescriptionStatus = true;
                    }
                    return new DoctorAppointmentReturnDTO(a.AppointmentId, a.AppointmentDate, a.Slot, a.PatientId, patient.Name, patient.Age, patient.ContactNo, a.Description, a.AppointmentType, a.AppointmentStatus, prescriptionStatus);

                }).ToList();
                return appointmentList;
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
                var patientDetails = await _userDetailsRepository.Get(appointment.PatientId);
                BackgroundJobs.NotificationForPatient(patientDetails.Name, patientDetails.ContactNo, appointment.AppointmentDate);
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
                var medicalRecordList = medicalRecords.Select( mr =>
                {
                    var patient = _userDetailsRepository.Get(mr.PatientId).Result;
                    var medicationList = mr.Medication.Select(m =>
                    {
                        return new MedicationDTO(m.MedicineName, m.Form, m.Dosage, m.IntakeTiming, m.Intake);
                    });
                    return new MedicalRecordReturnDTO(mr.RecordId, mr.PatientId, patient.Name, patient.ContactNo, mr.Date, mr.Diagnosis, mr.Treatment, medicationList.ToList(), mr.TreatmentStatus);
                }).ToList();
                return medicalRecordList;
            }
            
            catch(ObjectNotAvailableException e)
            {
                throw;
            }
        }

        public async Task<PrescriptionReturnDTO> ProvidePrescriptionForAppointment(ProvidePrescriptionDTO prescriptionDTO)
        {
            Prescription prescription = null;
            var appointment = await _appointmentRepository.Get(prescriptionDTO.PrescriptionFor);
            if (DateTime.Now < appointment.AppointmentDate || TimeOnly.FromTimeSpan(DateTime.Now.TimeOfDay) < appointment.Slot)
            {
                throw new Exception("Cannot Provide prescription before appointment!");
            }
            try
            {                
                prescription = await _prescriptionRepository.Add(new Prescription() { PrescriptionFor = prescriptionDTO.PrescriptionFor, PatientId = prescriptionDTO.PatientId, DoctorId = prescriptionDTO.DoctorId });
                appointment.AppointmentStatus = "Completed";
                await _appointmentRepository.Update(appointment);   
                foreach(var m in prescriptionDTO.prescribedMedicine)
                {
                    await _medicationRepository.Add(new Medication(m.MedicineName, m.Form, m.Dosage, m.IntakeTiming, m.Intake, prescription.PrescriptionId));
                }
                var patientDetails = await _userDetailsRepository.Get(prescription.PatientId);
                var doctor = _doctorRepository.Get(prescription.DoctorId).Result;
                return new PrescriptionReturnDTO(prescription.PrescriptionId, prescription.PrescriptionFor, prescription.PatientId, patientDetails.Name, patientDetails.Age, _userDetailsRepository.Get(prescription.DoctorId).Result.Name, doctor.Specialization, await MapMedicationToDTO(prescription.PrescriptionId));
                 
            }
            catch (ObjectAlreadyExistsException e)
            {
                throw;
            }
            catch(Exception e)
            {
                if (prescription.PrescriptionId > 1)
                {
                    var medications = _medicationRepository.Get().Result.Where(m => m.PrescriptionId == prescription.PrescriptionId).ToList();
                    foreach (var m in medications)
                    {
                        await _medicationRepository.Delete(m.MedicationId);
                    }
                    await _prescriptionRepository.Delete(prescription.PrescriptionId);
                }                              
                throw;
            }            
        }

        public async Task<PrescriptionReturnDTO> UpdatePrescription(UpdatePrescriptionDTO prescriptionDTO)
        {
            var prescriptionAlreadyAvailable = await _prescriptionRepository.Get(prescriptionDTO.PrescriptionId);
            if (prescriptionAlreadyAvailable == null)
            {
                throw new ObjectNotAvailableException("Prescription");
            }
            var medications = _medicationRepository.Get().Result.Where(m => m.PrescriptionId == prescriptionAlreadyAvailable.PrescriptionId).ToList();
            foreach (var m in medications)
            {
                await _medicationRepository.Delete(m.MedicationId);
            }
            foreach (var m in prescriptionDTO.prescribedMedicine)
            {
                await _medicationRepository.Add(new Medication(m.MedicineName, m.Form, m.Dosage, m.IntakeTiming, m.Intake, prescriptionDTO.PrescriptionId));
            }
            var patientDetails = await _userDetailsRepository.Get(prescriptionAlreadyAvailable.PatientId);
            var doctor = _doctorRepository.Get(prescriptionAlreadyAvailable.DoctorId).Result;
            return new PrescriptionReturnDTO(prescriptionAlreadyAvailable.PrescriptionId, prescriptionAlreadyAvailable.PrescriptionFor, prescriptionAlreadyAvailable.PatientId, patientDetails.Name, patientDetails.Age, _userDetailsRepository.Get(prescriptionAlreadyAvailable.DoctorId).Result.Name, doctor.Specialization, await MapMedicationToDTO(prescriptionAlreadyAvailable.PrescriptionId));
            
        }

        public async Task<string> CreateMedicalRecord(AppointmentMedicalRecordDTO recordDTO)
        {
            try
            {
                var prescription = _prescriptionRepository.Get().Result.Where(m => m.PrescriptionFor == recordDTO.AppointmentId).FirstOrDefault();
                if(prescription.Medications == null)
                {
                    throw new ObjectNotAvailableException("Medication");
                }
                await _medicalRecordsRepository.Add(new MedicalRecord()
                {
                    PatientId = recordDTO.PatientId,
                    PatientType = recordDTO.PatientType,
                    DoctorId = recordDTO.DoctorId,
                    Diagnosis = recordDTO.Diagnosis,
                    Treatment = recordDTO.Treatment,
                    Medication = prescription.Medications,
                    TreatmentStatus = recordDTO.TreatmentStatus
                });
            }
            catch (Exception e)
            {
                throw new Exception("Error in adding the record! Try again later");
            }
            return $"Medical Record added for the patient - {recordDTO.PatientId}";
        }

        public async Task<List<MedicationMapperDTO>> MapMedicationToDTO(int prescriptionId)
        {
            var medicationsList = _medicationRepository.Get().Result.Where(m => m.PrescriptionId == prescriptionId)
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

