using HospitalManagement.CustomExceptions;
using HospitalManagement.Interfaces;
using HospitalManagement.Models;
using HospitalManagement.Models.DTOs.AppointmentDTOs;
using HospitalManagement.Models.DTOs.DoctorDTOs;
using HospitalManagement.Models.DTOs.PatientDTOs;
using HospitalManagement.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static HospitalManagement.Enums.DoctorFeeBySpecialization;

namespace HospitalManagement.Services
{
    public class ReceptionistService : IReceptionistService
    {
        private readonly IRepository<int, Appointment> _appointmentRepository;
        private readonly IRepository<int, Doctor> _doctorRepository;
        private readonly IRepository<int, UserDetails> _userDetailsRepository;
        private readonly IRepository<int, WardBedAvailability> _wardBedAvailabilityRepository;
        private readonly IRepositoryForCompositeKey<int, DateTime, DoctorAvailability> _doctorAvailabilityRepository;
        private readonly IRepository<int, Bill> _billRepository;
        private readonly IRepository<int, InPatient> _inPatientRepository;
        private readonly IRepository<int, InPatientDetails> _inPatientDetailsRepository;
        private readonly IRepository<int, Room> _roomRepository;

        public ReceptionistService(IRepository<int, Appointment> appointmentRepository, IRepository<int, Doctor> doctorRepository,
            IRepository<int, UserDetails> userDetailsRepository, IRepository<int, WardBedAvailability> wardBedAvailabilityRepository,
            IRepositoryForCompositeKey<int, DateTime, DoctorAvailability> doctorAvailabilityRepository, IRepository<int, Bill> billRepository,
            IRepository<int, InPatient> inPatientRepository, IRepository<int, InPatientDetails> inPatientDetailsRepository,
            IRepository<int, Room> roomRepository) {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _userDetailsRepository = userDetailsRepository;
            _wardBedAvailabilityRepository = wardBedAvailabilityRepository;
            _doctorAvailabilityRepository = doctorAvailabilityRepository;
            _billRepository = billRepository;
            _inPatientRepository = inPatientRepository;
            _inPatientDetailsRepository = inPatientDetailsRepository;
            _roomRepository = roomRepository;
        }

        #region BookAppointment
        public async Task<ReceptAppointmentReturnDTO> BookAppointment(BookAppointmentDTO appointmentDTO)
        {
            try
            {
                var doctor = await _doctorRepository.Get(appointmentDTO.DoctorId);
                var appointment = await _appointmentRepository.Add(new Appointment(appointmentDTO.AppointmentDate, appointmentDTO.Slot, appointmentDTO.DoctorId, doctor.Specialization, appointmentDTO.PatientId, appointmentDTO.Description, "scheduled", appointmentDTO.AppointmentType, appointmentDTO.AppointmentMode));
                var availability = await _doctorAvailabilityRepository.Get(appointmentDTO.DoctorId, appointmentDTO.AppointmentDate);
                if (availability == null)
                {
                    await _doctorAvailabilityRepository.Add(new DoctorAvailability(doctor.DoctorId, doctor.Slots.Except(new List<TimeOnly>() { appointmentDTO.Slot }).ToList()));
                }
                else
                {
                    availability.AvailableSlots.Remove(appointmentDTO.Slot);
                }
                await _doctorAvailabilityRepository.Update(availability);
                if (appointment == null)
                {
                    throw new AppointmentAlreadyRaisedException();
                }
                var doctorDetails = await _userDetailsRepository.Get(appointment.DoctorId);
                var patientDetails = await _userDetailsRepository.Get(appointment.PatientId);
                return new ReceptAppointmentReturnDTO(appointment.AppointmentId, appointment.AppointmentDate, appointment.Slot,
                    appointment.PatientId, patientDetails.Name, patientDetails.Age, patientDetails.ContactNo, appointment.Description, appointment.AppointmentType, appointment.AppointmentStatus, appointment.DoctorId, doctorDetails.Name, appointment.Speciality);
                //send notification to doctor
            }
            catch(ObjectNotAvailableException e)
            {
                throw;
            }

        }
        #endregion

        #region CheckBedAvailability
        public async Task<Dictionary<string, int>> CheckBedAvailability()
        {
            var availability = await _wardBedAvailabilityRepository.Get();
            if (availability.ToList().Count > 0)
            {
                var availabilityList = new Dictionary<string, int>();
                foreach (var item in availability)
                {
                    availabilityList.Add(item.WardBedType, item.BedsAvailable);
                }
                return availabilityList;
            }
            throw new ObjectsNotAvailableException("Beds");
           
        }
        #endregion

        #region CheckDoctorAvailability
        public async Task<List<Doctor>> GetAllDoctorsAvailableNow(string specialization, TimeOnly timeOfDay, DateTime date)
        {
            var doctors = _doctorRepository.Get().Result.Where(d => d.Specialization == specialization && d.ShiftStartTime >= timeOfDay && timeOfDay < d.ShiftEndTime && d.AvailableDays.Contains(date.DayOfWeek.ToString())).OrderByDescending(d => d.Experience).ToList();
            var availableDoctors = new List<Doctor>();
            if (doctors.Count > 0)
            {
                foreach (var doctor in doctors)
                {
                    var availability = await _doctorAvailabilityRepository.Get(doctor.DoctorId, date);
                    if (availability != null)
                    {
                        if (availability.AvailableSlots.Count > 0)
                        {
                            availableDoctors.Add(doctor);
                        }
                    }
                    else
                    {
                        availableDoctors.Add(doctor);
                    }
                }

            }
            throw new ObjectsNotAvailableException("Doctors");
        }

        public async Task<List<DoctorAvailabilityDTO>> CheckDoctoravailability(string specialization)
        {
            try
            {
                var availableDoctor = await GetAllDoctorsAvailableNow(specialization, TimeOnly.FromTimeSpan(DateTime.Now.TimeOfDay), DateTime.Now.Date);
                var doctorsList = await Task.WhenAll(availableDoctor.Select(async d =>
                {
                    var doctorDetails = await _userDetailsRepository.Get(d.DoctorId);
                    var doctorAvailableSlots = _doctorAvailabilityRepository.Get(d.DoctorId, DateTime.Now.Date).Result.AvailableSlots;
                    return new DoctorAvailabilityDTO(d.DoctorId, doctorDetails.Name, d.Specialization, d.Experience,
                        d.LanguagesKnown, d.AvailableDays, DateTime.Now.Date, doctorAvailableSlots);
                }));
                return doctorsList.ToList();
            }
            catch (ObjectNotAvailableException e)
            {
                throw;
            }
            catch (ObjectsNotAvailableException e)
            {
                throw;
            }
        }
        #endregion

        #region RegisterInPatient
        public async Task<string> CreateInPatient(InPatientDTO patientDTO)
        {
            try
            {
                await _inPatientRepository.Add(new InPatient(patientDTO.PatientId, patientDTO.DoctorId, patientDTO.Description));
                await AllocateRoomForPatient(patientDTO.PatientId, patientDTO.WardType, patientDTO.NoOfDays);
                return "Patient alloted successfully!";
            }
            catch (ObjectNotAvailableException e)
            {
                throw;
            }
        }
        #endregion

        #region UpdateResourceAfterDischarge
        public async Task<string> UpdateInPatientDetailsForDischarge(int inpatientId, int billId)
        {
            try
            {
                var inpatient = await _inPatientRepository.Get(inpatientId);
                inpatient.DischargeDate = DateTime.Now.Date;
                inpatient.IsActive = false;
                inpatient.BillId = billId;
                await _inPatientRepository.Update(inpatient);
                var inPatientDetailsToUpdate = inpatient.InPatientDetails.Where(d => d.IsActivePatient == true).ToList();
                inPatientDetailsToUpdate.Select(async d =>
                {
                    d.IsActivePatient = true;
                    await _inPatientDetailsRepository.Update(d);
                    var room = d.Room;
                    room.IsAllotted = false;
                    await _roomRepository.Update(room);
                    var wardBedAvailability = room.WardBed;
                    wardBedAvailability.BedsAvailable += 1;
                    await _wardBedAvailabilityRepository.Update(wardBedAvailability);
                });
                return "Updated successfully!";
            }
            catch (ObjectNotAvailableException e)
            {
                throw;
            }
        }
        #endregion

        #region GenerateBillForInPatient
        public async Task<InPatientBillDTO> GenerateBillForInPatient(int inPatientid)
        {
            try
            {
                var inpatient = await _inPatientRepository.Get(inPatientid);
                Dictionary<string, RoomRateDTO> bedAndCost = new Dictionary<string, RoomRateDTO>();
                var totalAmount = 0.0;
                foreach (var item in inpatient.InPatientDetails)
                {
                    var wardType = await _wardBedAvailabilityRepository.Get(item.Room.WardTypeId);
                    bedAndCost.Add(wardType.WardBedType, new RoomRateDTO(item.NoOfDays, wardType.CostsPerDay));
                    totalAmount += item.NoOfDays * wardType.CostsPerDay;
                }
                var doctorFee = (double)Enum.Parse(typeof(DoctorFee), _doctorRepository.Get(inpatient.DoctorId).Result.Specialization);
                totalAmount += doctorFee;
                var bill = await _billRepository.Add(new Bill(inPatientid, "InPatient", inpatient.Description, totalAmount));
                var patientDetails = await _userDetailsRepository.Get(inPatientid);
                await UpdateInPatientDetailsForDischarge(inPatientid, bill.BillId);
                return new InPatientBillDTO(bill.BillId, bill.Date, inPatientid, patientDetails.Name, patientDetails.Age, patientDetails.ContactNo,
                    inpatient.AdmittedDate, (DateTime.Now.Date - inpatient.AdmittedDate).Days, bedAndCost, doctorFee, totalAmount);
            }
            catch(ObjectNotAvailableException e)
            {
                throw;
            }
        }
        #endregion

        #region BillGenerationForOutPatient
        public async Task<OutPatientBillDTO> GenerateBillForOutPatient(int appointmentid)
        {
            try
            {
                var appointment = await _appointmentRepository.Get(appointmentid);
                var doctorSpecialization = appointment.Doctor.Specialization;
                var doctorFee = (double)Enum.Parse(typeof(DoctorFee), doctorSpecialization);
                var doctorDetails = await _userDetailsRepository.Get(appointment.DoctorId);
                var bill = await _billRepository.Add(new Bill(appointment.PatientId, "OutPatient", appointment.Description, doctorFee));
                var patientDetails = await _userDetailsRepository.Get(appointment.PatientId);
                return new OutPatientBillDTO(bill.BillId, appointment.PatientId, patientDetails.Name, patientDetails.Age, patientDetails.ContactNo, doctorDetails.Name, doctorSpecialization,doctorFee, doctorFee);
            }
            catch (ObjectNotAvailableException e)
            {
                throw;
            }

        }
        #endregion

        #region GetAppointmentDetails
        public async Task<ReceptAppointmentReturnDTO> GetAppointmentDetails(int appointmentid)
        {
            try
            {
                var appointment = await _appointmentRepository.Get(appointmentid);
                var doctorDetails = await _userDetailsRepository.Get(appointment.DoctorId);
                var patientDetails = await _userDetailsRepository.Get(appointment.PatientId);
                return new ReceptAppointmentReturnDTO(appointment.AppointmentId, appointment.AppointmentDate, appointment.Slot,
                    appointment.PatientId, patientDetails.Name, patientDetails.Age, patientDetails.ContactNo, appointment.Description,
                    appointment.AppointmentType, appointment.AppointmentStatus, appointment.DoctorId, doctorDetails.Name, appointment.Speciality);
            }
            catch (ObjectNotAvailableException e)
            {
                throw;
            }
        }
        #endregion

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
            catch (ObjectNotAvailableException e) {
                throw;
            }
        }
        #endregion

        #region UpdateInPatientDetails
        public async Task<string> UpdateInPatient(UpdateInPatientDTO patientDTO)
        {
            try
            {
                var inpatientDetails = _inPatientDetailsRepository.Get().Result.Where(pd => pd.InPatientId == patientDTO.PatientId && pd.IsActivePatient == true).ToList();
                foreach (var detail in inpatientDetails)
                {
                    if (_wardBedAvailabilityRepository.Get(detail.Room.WardTypeId).Result.WardBedType == patientDTO.WardType)
                    {
                        detail.NoOfDays = patientDTO.NoOfDays;
                        await _inPatientDetailsRepository.Update(detail);
                        return "Updated successfully!";
                    }
                }
                return await AllocateRoomForPatient(patientDTO.PatientId, patientDTO.WardType, patientDTO.NoOfDays);
            }
            catch (ObjectNotAvailableException e)
            {
                throw;
            }
            
        }
        #endregion

        #region AllocateRoomForPatient
        public async Task<string> AllocateRoomForPatient(int patientId, string wardType, int noOfDays)
        {
            try
            {
                var selectedRoom = _roomRepository.Get().Result.Where(r => r.WardBed.WardBedType == wardType && r.IsAllotted == false).Take(1).ToList();
                await _inPatientDetailsRepository.Add(new InPatientDetails(patientId, selectedRoom[0].RoomId, noOfDays));
                selectedRoom[0].IsAllotted = true;
                await _roomRepository.Update(selectedRoom[0]);
                var wardBedAvailability = await _wardBedAvailabilityRepository.Get(selectedRoom[0].WardTypeId);
                wardBedAvailability.BedsAvailable -= 1;
                await _wardBedAvailabilityRepository.Update(wardBedAvailability);
                return "UpdatedSuccessfully!";
            }
            catch(ObjectNotAvailableException e)
            {
                throw;
            }
            
        }
        #endregion

    }
}