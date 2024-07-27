using HospitalManagement.CustomExceptions;
using HospitalManagement.Models.DTOs.AppointmentDTOs;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HospitalManagement.Interfaces;
using HospitalManagement.Models.DTOs.DoctorDTOs;
using HospitalManagement.Models.DTOs.PatientDTOs;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<ActionResult<List<DoctorAvailabilityDTO>>> CheckDoctorAvailability([FromBody] string specialization)
        {
            try
            {
                List<DoctorAvailabilityDTO> result = await _receptionistService.CheckDoctoravailability(specialization);
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
        [ProducesResponseType(typeof(ReceptAppointmentReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReceptAppointmentReturnDTO>> BookAppointment([FromBody] BookAppointmentDTO appointmentDTO)
        {
            try
            {
                ReceptAppointmentReturnDTO result = await _receptionistService.BookAppointment(appointmentDTO);
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
        public async Task<ActionResult<string>> AdmissionForInPatient(InPatientDTO patientDTO)
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


    }
}
