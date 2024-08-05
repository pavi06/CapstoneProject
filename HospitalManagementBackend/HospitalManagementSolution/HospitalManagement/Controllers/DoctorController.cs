using HospitalManagement.CustomExceptions;
using HospitalManagement.Interfaces;
using HospitalManagement.Models.DTOs.AppointmentDTOs;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HospitalManagement.Models.DTOs.MedicalRecordDTOs;
using HospitalManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Doctor")]
    [EnableCors("MyCors")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        //private readonly ILogger<DoctorController> _logger;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
           
        }

        [HttpGet("GetTodayAppointment")]
        [ProducesResponseType(typeof(List<DoctorAppointmentReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<DoctorAppointmentReturnDTO>>> GetAppointment(int doctorId)
        {
            try
            {
                List<DoctorAppointmentReturnDTO> result = await _doctorService.GetAllScheduledAppointments(doctorId);
                //_logger.LogInformation("Appointments retrieved successfully");
                return Ok(result);
            }
            catch (ObjectsNotAvailableException e)
            {
                //_logger.LogError("No Appointment Available");
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (ObjectNotAvailableException e)
            {
                //_logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }

        [HttpPut("CancelAppointment")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> CancelAppointment(int appointmentId)
        {
            try
            {
                var result = await _doctorService.CancelAppointment(appointmentId);
                //_logger.LogError("Appointment cancelled successfully");
                return Ok(result);
            }
            catch (ObjectNotAvailableException e)
            {
                //_logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }

        [HttpPost("CreateMedicalRecord")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> CreateMedicalRecords([FromBody] AppointmentMedicalRecordDTO recordDTO)
        {
            try
            {
                var result = await _doctorService.CreateMedicalRecord(recordDTO);
                //_logger.LogInformation("Medical record created successfully");
                return Ok(result);
            }
            catch (ObjectNotAvailableException e)
            {
                //_logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }


        [HttpGet("GetMedicalRecord")]
        [ProducesResponseType(typeof(List<MedicalRecordReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<MedicalRecordReturnDTO>>> GetMedicalRecords(int doctorId, int patientId)
        {
            try
            {
                List<MedicalRecordReturnDTO> result = await _doctorService.GetPatientMedicalRecords(patientId,doctorId);
                //_logger.LogInformation("Medical records are retrieved successfully");
                return Ok(result);
            }
            catch (ObjectsNotAvailableException e)
            {
                //_logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (ObjectNotAvailableException e)
            {
                //_logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }


        [HttpPost("UploadPrescription")]
        [ProducesResponseType(typeof(ProvidePrescriptionDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProvidePrescriptionDTO>> ProvidePrescription(ProvidePrescriptionDTO prescriptionDTO)
        {
            try
            {
                 var result = await _doctorService.ProvidePrescriptionForAppointment(prescriptionDTO);
                //_logger.LogInformation("Prescription provided successfully");
                return Ok(result);
            }
            catch (ObjectNotAvailableException e)
            {
                //_logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }

        [HttpPost("UpdatePrescription")]
        [ProducesResponseType(typeof(PrescriptionReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PrescriptionReturnDTO>> UpdatePrescription(UpdatePrescriptionDTO prescriptionDTO)
        {
            try
            {
                var result = await _doctorService.UpdatePrescription(prescriptionDTO);
                //_logger.LogInformation("Prescription updated successfully");
                return Ok(result);
            }
            catch (ObjectNotAvailableException e)
            {
                //_logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }

    }
}
