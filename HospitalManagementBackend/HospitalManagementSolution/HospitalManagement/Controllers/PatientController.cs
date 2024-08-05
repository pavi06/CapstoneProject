using HospitalManagement.CustomExceptions;
using HospitalManagement.Interfaces;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using HospitalManagement.Models.DTOs.AppointmentDTOs;
using HospitalManagement.Models.DTOs.MedicalRecordDTOs;
using HospitalManagement.Models.DTOs.DoctorDTOs;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("MyCors")]
    [Authorize(Roles = "Patient")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        //private readonly ILogger<PatientController> _logger;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
            //_logger = logger;
        }

        [HttpPost("GetDoctorSlots")]
        [ProducesResponseType(typeof(Dictionary<string, bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Dictionary<string, bool>>> GetDoctorSlots([FromBody] CheckDoctorSlotsDTO checkSlotsDTO)
        {
            try
            {
                Dictionary<string, bool> result = await _patientService.GetAvailableSlotsOfDoctor(checkSlotsDTO);
                if(result.Count == 0)
                {
                    throw new ObjectsNotAvailableException("Slots");
                }
                //_logger.LogInformation("Doctor slots retrieved successfully");
                return Ok(result);
            }
            catch (ObjectNotAvailableException e)
            {
                //_logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (ObjectsNotAvailableException e)
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

        [HttpPost("BookAppointmentByDoctor")]
        [ProducesResponseType(typeof(PatientAppointmentReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PatientAppointmentReturnDTO>> BookAppointment([FromBody] BookAppointmentDTO appointmentDTO)
        {
            try
            {
                PatientAppointmentReturnDTO result = await _patientService.BookAppointmentByDoctor(appointmentDTO);
                //_logger.LogInformation("Appointment Booked successfully");
                return Ok(result);
            }
            catch (ObjectNotAvailableException e)
            {
                //_logger.LogError("No Appointment Available");
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return BadRequest(new ErrorModel(400, ex.Message));
            }

        }

        [HttpPost("BookAppointmentBySpeciality")]
        [ProducesResponseType(typeof(PatientAppointmentReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PatientAppointmentReturnDTO>> BookAppointmentBySpeciality([FromBody] BookAppointmentBySpecDTO specAppointmentDTO)
        {
            try
            {
                PatientAppointmentReturnDTO result = await _patientService.BookAppointmentBySpeciality(specAppointmentDTO);
                //_logger.LogInformation("Appointment raised successfully");
                return Ok(result);
            }
            catch (ObjectsNotAvailableException e)
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
                string result = await _patientService.CancelAppointment(appointmentId);
                //_logger.LogInformation("Appointment cancelled successfully");
                return Ok(result);
            }
            catch (AppointmentAlreadyCancelledException e)
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

        [HttpGet("MyAppointments")]
        [ProducesResponseType(typeof(List<PatientAppointmentReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<PatientAppointmentReturnDTO>>> MyAppointments(int patientId, int limit, int skip)
        {
            try
            {
                var result = await _patientService.MyAppointments(patientId, limit, skip);
                //_logger.LogInformation("Appointments retrieved successfully");
                return Ok(result);
            }
            catch (ObjectNotAvailableException e)
            {
                //_logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (ObjectsNotAvailableException e)
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

        [HttpGet("MyPrescriptionForAppointment")]
        [ProducesResponseType(typeof(PrescriptionReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PrescriptionReturnDTO>> MyPrescription(int patientId, int appointmentId)
        {
            try
            {
                var result = await _patientService.MyPrescriptionForAppointment(patientId, appointmentId);
                //_logger.LogInformation("prescription retrieved successfully");
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


        [HttpGet("MyPrescriptions")]
        [ProducesResponseType(typeof(List<PrescriptionsReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<PrescriptionsReturnDTO>>> MyPrescriptions(int patientId, int limit, int skip)
        {
            try
            {
                var result = await _patientService.MyPrescriptions(patientId, limit, skip);
                //_logger.LogInformation("Prescriptions retrieved successfully");
                return Ok(result);
            }
            catch (ObjectNotAvailableException e)
            {
                //_logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
            }
            catch (ObjectsNotAvailableException e)
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
