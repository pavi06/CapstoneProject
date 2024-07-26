using HospitalManagement.CustomExceptions;
using HospitalManagement.Interfaces;
using HospitalManagement.Models;
using HospitalManagement.Models.DTOs.AppointmentDTOs;
using HospitalManagement.Models.DTOs.MedicalRecordDTOs;

namespace HospitalManagement.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IRepository<int, Appointment> _appointmentRepository;
        private readonly IRepository<int, UserDetails> _userDetailsRepository;
        private readonly IRepository<int, MedicalRecord> _medicalRecordsRepository;
        private readonly IRepository<int, Prescription> _prescriptionRepository;

        public DoctorService(IRepository<int, Appointment> appointmentRepository, IRepository<int, UserDetails> userDetailsRepository, IRepository<int, MedicalRecord> medicalRecordsRepository, IRepository<int, Prescription> prescriptionRepository) { 
            _appointmentRepository = appointmentRepository;
            _userDetailsRepository = userDetailsRepository;
            _medicalRecordsRepository = medicalRecordsRepository;
            _prescriptionRepository = prescriptionRepository;
        }

        public async Task<List<AppointmentReturnDTO>> GetAllTodayAppointments(int doctorid)
        {
            var appointments = _appointmentRepository.Get().Result.Where(a => a.DoctorId == doctorid && a.AppointmentDate == DateTime.Now.Date).ToList();
            var appointmentList = await Task.WhenAll(appointments.Select(async a =>
            {
                var patient = await _userDetailsRepository.Get(a.PatientId);
                return new AppointmentReturnDTO(a.AppointmentId, a.AppointmentDate, a.Slot, a.PatientId, patient.Name, patient.Age, patient.ContactNo, a.Description, a.AppointmentType, a.AppointmentStatus);
            }));
            return appointmentList.ToList();
        }

        public async Task<List<MedicalRecordReturnDTO>> GetPatientMedicalRecords(int patientid, string patientType, int doctorid)
        {
            var medicalRecords = _medicalRecordsRepository.Get().Result.Where(mr => mr.PatientId == patientid && mr.PatientType == patientType && mr.DoctorId == doctorid);
            var medicalRecordList = await Task.WhenAll(medicalRecords.Select(async mr =>
            {
                var patient = await _userDetailsRepository.Get(mr.PatientId);
                return new MedicalRecordReturnDTO(mr.RecordId, mr.PatientId, patient.Name, patient.ContactNo, mr.PatientType, mr.Date, mr.Diagnosis, mr.Treatment, mr.Medication, mr.TreatmentStatus);
            }));
            return medicalRecordList.ToList();
        }

        public async Task<Prescription> UploadPrescriptionForAppointment(int appointmentid, string prescriptionImage)
        {
            var res = await _prescriptionRepository.Add(new Prescription() { AppointmentId = appointmentid, PrescriptionUrl = prescriptionImage});
            if (res == null)
            {
                throw new CannotUploadImageException();
                //check for already added!
            }
            return res;
        }
    }
}
