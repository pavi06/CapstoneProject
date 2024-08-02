using HospitalManagement.CustomExceptions;
using HospitalManagement.Models.DTOs.AppointmentDTOs;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HospitalManagement.Interfaces;
using HospitalManagement.Models.DTOs.DoctorDTOs;
using HospitalManagement.Models.DTOs.PatientDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using HospitalManagement.Models.DTOs.BillDTOs;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(Roles = "Receptionist")]
    [EnableCors("MyCors")]
    [ApiController]
    public class ReceptionistController : ControllerBase
    {
        private readonly IReceptionistService _receptionistService;
        private readonly ILogger<ReceptionistController> _logger;

        public ReceptionistController(IReceptionistService receptionistService, ILogger<ReceptionistController> logger) { 
            _receptionistService = receptionistService;
            _logger = logger;
        }

        [HttpPost("CheckDoctorAvailability")]
        [ProducesResponseType(typeof(List<DoctorAvailabilityDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<DoctorAvailabilityDTO>>> CheckDoctorAvailability([FromBody] string specialization, int limit, int skip)
        {
            try
            {
                List<DoctorAvailabilityDTO> result = await _receptionistService.CheckDoctoravailability(specialization, limit, skip);
                if (result.Count == 0)
                {
                    throw new ObjectsNotAvailableException("Doctors");
                }
                _logger.LogInformation("Available doctors retrieved");
                return Ok(result);
            }
            catch (ObjectNotAvailableException e)
            {
                _logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch(ObjectsNotAvailableException e)
            {
                _logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }


        [HttpPost("BookAppointment")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> BookAppointment([FromBody] ReceptionistBookAppointmentDTO appointmentDTO)
        {
            try
            {
                var result = await _receptionistService.BookAppointment(appointmentDTO);
                _logger.LogInformation("Appointment booked successfully");
                return Ok(result);
            }
            catch (ObjectNotAvailableException e)
            {
                _logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }

        [HttpGet("CheckBedAvilability")]
        [ProducesResponseType(typeof(Dictionary<string, int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Dictionary<string, int>>> CheckBedAvailability()
        {
            try
            {
                var result = await _receptionistService.CheckBedAvailability();
                _logger.LogInformation("Bed availability retrieved");
                return Ok(result);
            }
            catch (ObjectsNotAvailableException e)
            {
                _logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }


        [HttpPost("AdmissionForPatient")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> AdmissionForInPatient([FromBody]InPatientDTO patientDTO)
        {
            try
            {
                var result = await _receptionistService.CreateInPatient(patientDTO);
                _logger.LogInformation("InPatient registered successfully");
                return Ok(result);
            }
            catch (ObjectNotAvailableException e)
            {
                _logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }


        [HttpPut("UpdateInPatient")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> UpdateInPatient(UpdateInPatientDTO patientDTO)
        {
            try
            {
                var result = await _receptionistService.UpdateInPatient(patientDTO);
                _logger.LogInformation("InPatient details updated");
                return Ok(result);
            }
            catch (ObjectNotAvailableException e)
            {
                _logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }


        [HttpPost("BillForOutPatient")]
        [ProducesResponseType(typeof(OutPatientBillDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OutPatientBillDTO>> GenerateBillForOutPatient(int appointmentId)
        {
            try
            {
                var result = await _receptionistService.GenerateBillForOutPatient(appointmentId);
                _logger.LogInformation("Bill generated for outpatient successfully");
                return Ok(result);
            }
            catch (ObjectNotAvailableException e)
            {
                _logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }

        [HttpPost("BillForInPatient")]
        [ProducesResponseType(typeof(InPatientBillDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<InPatientBillDTO>> GenerateBillForInPatient(int inPatientid)
        {
            try
            {
                var result = await _receptionistService.GenerateBillForInPatient(inPatientid);
                _logger.LogInformation("Bill generated for inpatient successfully");
                return Ok(result);
            }
            catch (ObjectNotAvailableException e)
            {
                _logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }

        [HttpGet("GetAppointmentDetails")]
        [ProducesResponseType(typeof(ReceptAppointmentReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReceptAppointmentReturnDTO>> GetAppointmentDetails(int appointmentId)
        {
            try
            {
                var result = await _receptionistService.GetAppointmentDetails(appointmentId);
                _logger.LogInformation("Appointment details retrieved successfully");
                return Ok(result);
            }
            catch (ObjectNotAvailableException e)
            {
                _logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }

        [HttpGet("GetTodayAppointmentDetails")]
        [ProducesResponseType(typeof(List<ReceptAppointmentReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<ReceptAppointmentReturnDTO>>> GetAllTodayAppointmentDetails(int limit, int skip)
        {
            try
            {
                var result = await _receptionistService.GetAllTodayAppointments(limit, skip);
                _logger.LogInformation("Appointment details retrieved successfully");
                return Ok(result);
            }
            catch (ObjectNotAvailableException e)
            {
                _logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }


        [HttpGet("GetPendingBills")]
        [ProducesResponseType(typeof(List<PendingBillReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<PendingBillReturnDTO>>> GetAllPendingBills()
        {
            try
            {
                var result = await _receptionistService.GetPendingBills();
                _logger.LogInformation("Pending bills retrieved successfully");
                return Ok(result);
            }
            catch (ObjectNotAvailableException e)
            {
                _logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }

        [HttpGet("GetAllInPatientDetails")]
        [ProducesResponseType(typeof(List<InPatientReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<InPatientReturnDTO>>> GetAllInPatientDetails()
        {
            try
            {
                var result = await _receptionistService.GetAllInPatientDetails();
                _logger.LogInformation("Patient details retrieved successfully");
                return Ok(result);
            }
            catch (ObjectsNotAvailableException e)
            {
                _logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }

    }
}
