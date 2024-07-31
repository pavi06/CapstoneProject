using HospitalManagement.CustomExceptions;
using HospitalManagement.Interfaces;
using HospitalManagement.Migrations;
using HospitalManagement.Models;
using HospitalManagement.Models.DTOs.AppointmentDTOs;
using HospitalManagement.Models.DTOs.DoctorDTOs;
using HospitalManagement.Models.DTOs.PatientDTOs;
using HospitalManagement.Repositories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using static HospitalManagement.Enums.DoctorFeeBySpecialization;

namespace HospitalManagement.Services
{
    public class ReceptionistService : IReceptionistService
    {
        private readonly IRepository<int, Appointment> _appointmentRepository;
        private readonly IRepository<int, Doctor> _doctorRepository;
        private readonly IRepository<int, User> _userRepository;
        private readonly IRepository<int, WardRoomsAvailability> _wardBedAvailabilityRepository;
        private readonly IRepositoryForCompositeKey<int, DateTime, DoctorAvailability> _doctorAvailabilityRepository;
        private readonly IRepository<int, Bill> _billRepository;
        private readonly IRepository<int, Admission> _admissionRepository;
        private readonly IRepository<int, AdmissionDetails> _admissionDetailsRepository;
        private readonly IRepository<int, Room> _roomRepository;
        private readonly IRepository<int, Patient> _patientRepository;
        private readonly IRepository<int, Payment> _paymentRepository;

        public ReceptionistService(IRepository<int, Appointment> appointmentRepository, IRepository<int, Doctor> doctorRepository,
            IRepository<int, User> userDetailsRepository, IRepository<int, WardRoomsAvailability> wardBedAvailabilityRepository,
            IRepositoryForCompositeKey<int, DateTime, DoctorAvailability> doctorAvailabilityRepository, IRepository<int, Bill> billRepository,
            IRepository<int, Admission> inPatientRepository, IRepository<int, AdmissionDetails> inPatientDetailsRepository,
            IRepository<int, Room> roomRepository, IRepository<int, Patient> patientRepository, IRepository<int, Payment> paymentRepository)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _userRepository = userDetailsRepository;
            _wardBedAvailabilityRepository = wardBedAvailabilityRepository;
            _doctorAvailabilityRepository = doctorAvailabilityRepository;
            _billRepository = billRepository;
            _admissionRepository = inPatientRepository;
            _admissionDetailsRepository = inPatientDetailsRepository;
            _roomRepository = roomRepository;
            _patientRepository = patientRepository;
            _paymentRepository = paymentRepository;
        }

        #region BookAppointment
        public async Task<int> BookAppointment(ReceptionistBookAppointmentDTO appointmentDTO)
        {
            try
            {
                var doctor = await _doctorRepository.Get(appointmentDTO.DoctorId);
                if(appointmentDTO.PatientId == null || appointmentDTO.PatientId == 0)
                {
                    DateTime today = DateTime.Today;
                    int age = today.Year - appointmentDTO.DateOfBirth.Year;
                    if (appointmentDTO.DateOfBirth.Date > today.AddYears(-age))
                    {
                        age--;
                    }
                    var user = await _userRepository.Add(new User(appointmentDTO.Name, appointmentDTO.DateOfBirth, age, appointmentDTO.Gender, appointmentDTO.ContactNo, appointmentDTO.Address));
                    appointmentDTO.PatientId = _patientRepository.Add(new Patient(user.UserId)).Result.PatientId;
                }
                if (await _patientRepository.Get(appointmentDTO.PatientId) == null)
                {
                    await _patientRepository.Add(new Patient(appointmentDTO.PatientId));
                }
                var appointment = await _appointmentRepository.Add(new Appointment(appointmentDTO.AppointmentDate, TimeOnly.Parse(appointmentDTO.Slot), appointmentDTO.DoctorId, doctor.Specialization, appointmentDTO.PatientId, appointmentDTO.Description, "Scheduled", appointmentDTO.AppointmentType, "Offline"));
                var availability = await _doctorAvailabilityRepository.Get(appointmentDTO.DoctorId, appointmentDTO.AppointmentDate);
                if (availability == null)
                {
                    await _doctorAvailabilityRepository.Add(new DoctorAvailability(doctor.DoctorId, appointment.AppointmentDate,doctor.Slots.Except(new List<TimeOnly>() { TimeOnly.Parse(appointmentDTO.Slot) }).ToList()));
                }
                else
                {
                    availability.AvailableSlots.Remove(TimeOnly.Parse(appointmentDTO.Slot));
                    await _doctorAvailabilityRepository.Update(availability);
                }                
                if (appointment == null)
                {
                    throw new AppointmentAlreadyRaisedException();
                }
                return appointment.AppointmentId;
                //send notification to doctor
            }
            catch(ObjectNotAvailableException e)
            {
                await RevertAppointmentAdded(appointmentDTO);
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
                    availabilityList.Add(item.WardType, item.RoomsAvailable);
                }
                return availabilityList;
            }
            throw new ObjectsNotAvailableException("Beds");
           
        }
        #endregion

        #region CheckDoctorAvailability
        public async Task<List<Doctor>> GetAllDoctorsAvailableNow(string specialization, TimeOnly timeOfDay, DateTime date, int skip, int limit)
        {
            var doctors = _doctorRepository.Get().Result.Where(d => d.Specialization.ToLower() == specialization.ToLower() && d.ShiftStartTime <= timeOfDay && timeOfDay < d.ShiftEndTime && d.AvailableDays.Contains(date.DayOfWeek.ToString())).OrderByDescending(d => d.Experience).Skip(skip).Take(limit).ToList();
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
            return availableDoctors;
        }

        public async Task<List<DoctorAvailabilityDTO>> CheckDoctoravailability(string specialization, int limit, int skip)
        {
            try
            {
                var availableDoctor = await GetAllDoctorsAvailableNow(specialization, TimeOnly.FromTimeSpan(DateTime.Now.TimeOfDay), DateTime.Now.Date, skip, limit);
                if (availableDoctor.Count == 0)
                {
                    throw new ObjectsNotAvailableException("Doctor");
                }
                List<DoctorAvailabilityDTO> doctors = new List<DoctorAvailabilityDTO>();
                foreach (var d in availableDoctor)
                {
                    var doctorDetails = await _userRepository.Get(d.DoctorId);
                    var doctorAvailableSlots = _doctorAvailabilityRepository.Get(d.DoctorId, DateTime.Now.Date).Result.AvailableSlots;
                    doctors.Add(new DoctorAvailabilityDTO(d.DoctorId, doctorDetails.Name, d.Specialization, d.Experience,
                        d.LanguagesKnown, d.AvailableDays, DateTime.Now.Date, doctorAvailableSlots));
                }
                return doctors.ToList();
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
                User patient = null;
                if(patientDTO.PatientId == null)
                {
                    DateTime today = DateTime.Today;
                    int age = today.Year - patientDTO.DateOfBirth.Year;
                    if (patientDTO.DateOfBirth.Date > today.AddYears(-age))
                    {
                        age--;
                    }
                    patient = await _userRepository.Add(new User(patientDTO.Name, patientDTO.DateOfBirth,age , patientDTO.Gender, patientDTO.ContactNo, patientDTO.Address));
                    patientDTO.PatientId = patient.UserId;
                }
                var res = await _admissionRepository.Add(new Admission(patientDTO.PatientId, patientDTO.Description));
                await AllocateRoomForPatient(res.AdmissionId, patientDTO.WardType, patientDTO.NoOfDays);
                return "Patient alloted successfully!";
            }
            catch (ObjectNotAvailableException e)
            {
                var admission = _admissionRepository.Get().Result.Where(a => a.PatientId == patientDTO.PatientId && a.AdmittedDate == DateTime.Now.Date).FirstOrDefault();
                if ( admission!= null)
                {
                    await _admissionRepository.Delete(admission.AdmissionId);
                }
                throw;
            }
        }
        #endregion

        #region UpdateResourceAfterDischarge
        public async Task<string> UpdateInPatientDetailsForDischarge(int patientId, int billId)
        {
            try
            {
                var admission = _admissionRepository.Get().Result.Where(a=>a.PatientId == patientId && a.IsActivePatient == true).FirstOrDefault();
                admission.DischargeDate = DateTime.Now.Date;
                admission.IsActivePatient = false;
                await _admissionRepository.Update(admission);
                var admissionDetailsToUpdate = admission.AdmissionDetails.Where(a => a.AdmissionId == admission.AdmissionId).ToList();
                admissionDetailsToUpdate.Select(async d =>
                {
                    var room = d.Room;
                    room.IsAllotted = false;
                    await _roomRepository.Update(room);
                    var wardBedAvailability = room.WardBed;
                    wardBedAvailability.RoomsAvailable += 1;
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
                var admission = _admissionRepository.Get().Result.Where(a => a.PatientId == inPatientid && a.IsActivePatient == true).FirstOrDefault();
                if (admission == null)
                {
                    throw new ObjectsNotAvailableException("Admission");
                }
                Dictionary<string, RoomRateDTO> bedAndCost = new Dictionary<string, RoomRateDTO>();
                var totalAmount = 0.0;
                foreach (var item in admission.AdmissionDetails)
                {
                    var room = _roomRepository.Get(item.RoomId).Result;
                    bedAndCost.Add(room.WardBed.WardType, new RoomRateDTO(room.WardBed.WardType,item.NoOfDays, room.CostsPerDay));
                    totalAmount += item.NoOfDays * room.CostsPerDay;
                }
                var doctorFee = (double)(int)Enum.Parse(typeof(DoctorFee), _doctorRepository.Get((int)admission.DoctorId).Result.Specialization.ToLower(), true);
                totalAmount += doctorFee;
                var bill = await _billRepository.Add(new Bill(admission.AdmissionId,inPatientid, "InPatient", admission.Description, totalAmount));
                var patientDetails = await _userRepository.Get(inPatientid);
                await UpdateInPatientDetailsForDischarge(inPatientid, bill.BillId);
                return new InPatientBillDTO(bill.BillId, bill.Date, inPatientid, patientDetails.Name, patientDetails.Age, patientDetails.ContactNo,
                    admission.AdmittedDate, (DateTime.Now.Date - admission.AdmittedDate).Days, bedAndCost, doctorFee, totalAmount-doctorFee);
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
                var doctorFee = (double)(int)Enum.Parse(typeof(DoctorFee), doctorSpecialization, true);
                var doctorDetails = await _userRepository.Get(appointment.DoctorId);
                var bill = await _billRepository.Add(new Bill(appointmentid,appointment.PatientId, "OutPatient", appointment.Description,doctorFee));
                var patientDetails = await _userRepository.Get(appointment.PatientId);
                return new OutPatientBillDTO(bill.BillId, appointment.PatientId, patientDetails.Name, patientDetails.Age, patientDetails.ContactNo, doctorDetails.Name, doctorSpecialization,doctorFee,doctorFee);
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
                return await MapAppointment(appointment);
            }
            catch (ObjectNotAvailableException e)
            {
                throw;
            }
        }
        #endregion

        #region UpdateInPatientDetails
        public async Task<string> UpdateInPatient(UpdateInPatientDTO patientDTO)
        {
            try
            {
                var admissionDetails = _admissionDetailsRepository.Get().Result.Where(ad => ad.AdmissionId == patientDTO.AdmissionId).ToList();
                foreach (var detail in admissionDetails)
                {
                    if (_wardBedAvailabilityRepository.Get(detail.Room.WardTypeId).Result.WardType == patientDTO.WardType)
                    {
                        detail.NoOfDays = patientDTO.NoOfDays;
                        detail.Date = DateTime.Now.Date;
                        await _admissionDetailsRepository.Update(detail);
                        return "Updated successfully!";
                    }
                }
                return await AllocateRoomForPatient(patientDTO.AdmissionId, patientDTO.WardType, patientDTO.NoOfDays);
            }
            catch (ObjectNotAvailableException e)
            {
                throw;
            }
            
        }
        #endregion

        #region AllocateRoomForPatient
        public async Task<string> AllocateRoomForPatient(int admissionId, string wardType, int noOfDays)
        {
            List<Room> selectedRooms = null; 
            try
            {
                selectedRooms = _roomRepository.Get().Result.Where(r => r.WardBed.WardType == wardType && r.IsAllotted == false).Take(1).ToList();
                await _admissionDetailsRepository.Add(new AdmissionDetails(admissionId, selectedRooms[0].RoomId, noOfDays));
                selectedRooms[0].IsAllotted = true;
                await _roomRepository.Update(selectedRooms[0]);
                var wardBedAvailability = await _wardBedAvailabilityRepository.Get(selectedRooms[0].WardTypeId);
                wardBedAvailability.RoomsAvailable -= 1;
                await _wardBedAvailabilityRepository.Update(wardBedAvailability);
                return "UpdatedSuccessfully!";
            }
            catch(ObjectNotAvailableException e)
            {
                foreach(var room in selectedRooms) {
                    room.IsAllotted = false;
                    await _roomRepository.Update(room);
                    var wardBedAvailability = await _wardBedAvailabilityRepository.Get(room.WardTypeId);
                    if (wardBedAvailability !=null)
                    {
                        wardBedAvailability.RoomsAvailable += 1;
                        await _wardBedAvailabilityRepository.Update(wardBedAvailability);
                    }                    
                }
                var ad = _admissionDetailsRepository.Get().Result.FirstOrDefault(ad => ad.AdmissionId == admissionId);
                if(ad != null)
                {
                    await _admissionDetailsRepository.Delete(ad.AdmissionDetailsId);
                }
                await _admissionRepository.Delete(admissionId);
                throw;
            }
            
        }
        #endregion

        public async Task RevertAppointmentAdded(ReceptionistBookAppointmentDTO appointmentDTO)
        {
            var appointment = _appointmentRepository.Get().Result.SingleOrDefault(a => a.PatientId == appointmentDTO.PatientId && a.DoctorId == appointmentDTO.DoctorId
           && a.AppointmentDate == appointmentDTO.AppointmentDate && a.Date == DateTime.Now.Date && a.Slot == TimeOnly.Parse(appointmentDTO.Slot));
            try
            {
                await _appointmentRepository.Delete(appointment.AppointmentId);
                var availability = _doctorAvailabilityRepository.Get(appointment.DoctorId, appointment.AppointmentDate).Result;
                availability.AvailableSlots.Add(TimeOnly.Parse(appointmentDTO.Slot));
                await _doctorAvailabilityRepository.Update(availability);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<ReceptAppointmentReturnDTO>> GetAllTodayAppointments(int limit, int skip)
        {
            List<ReceptAppointmentReturnDTO> appointmentList = new List<ReceptAppointmentReturnDTO>();
            try
            {
                var appointments = _appointmentRepository.Get().Result.Where(a => a.AppointmentDate.Date == DateTime.Now.Date).OrderBy(a=>a.Slot).Skip(skip).Take(limit).ToList();
                if (appointments.Count == 0)
                {
                    throw new ObjectsNotAvailableException("Appointments");
                }
                foreach(var appointment in appointments)
                {
                    appointmentList.Add(await MapAppointment(appointment));
                }
                return appointmentList;
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

        public async Task<ReceptAppointmentReturnDTO> MapAppointment(Appointment appointment)
        {
            var doctorDetails = await _userRepository.Get(appointment.DoctorId);
            var patientDetails = await _userRepository.Get(appointment.PatientId);
            return new ReceptAppointmentReturnDTO(appointment.AppointmentId, appointment.AppointmentDate, appointment.Slot,
                appointment.PatientId, patientDetails.Name, patientDetails.Age, patientDetails.ContactNo, appointment.Description,
                appointment.AppointmentType, appointment.AppointmentStatus, appointment.DoctorId, doctorDetails.Name, appointment.Speciality);
        }

        public async Task<List<PendingBillReturnDTO>> GetPendingBills()
        {
            var bills = _billRepository.Get().Result.Where(b => b.PaymentStatus.ToLower() == "not paid");
            var billsList = bills.Select(b =>
            {
                var patient = _userRepository.Get(b.PatientId).Result;
                var paidAmount = _paymentRepository.Get().Result.Where(p => p.BillId == b.BillId).Sum(p=>p.AmountPaid);
                return new PendingBillReturnDTO(b.PatientId,patient.Name ,patient.ContactNo,b.BillId, b.Date,b.Amount, b.Amount - paidAmount, b.PaymentStatus);
            }).ToList();
            return billsList;
        }

        public async Task<List<InPatientReturnDTO>> GetAllInPatientDetails()
        {
            var activePatients = _admissionRepository.Get().Result.Where(a => a.IsActivePatient == true).ToList();
            if(activePatients == null)
            {
                throw new ObjectsNotAvailableException("Patients");
            }
            var patientDetails = activePatients.Select( a =>
            {
                var maxDate = _admissionDetailsRepository.Get().Result.Where(ad => ad.AdmissionId == a.AdmissionId).Max(ad => ad.Date);
                var admissionDetails = _admissionDetailsRepository.Get().Result.SingleOrDefault(ad => ad.AdmissionId == a.AdmissionId && ad.Date == maxDate);
                return new InPatientReturnDTO(a.PatientId, _userRepository.Get(a.PatientId).Result.Name, _roomRepository.Get(admissionDetails.RoomId).Result.WardBed.WardType, admissionDetails.RoomId, a.AdmittedDate, a.Description);
            }).ToList();
            return patientDetails;
        }
    }
}