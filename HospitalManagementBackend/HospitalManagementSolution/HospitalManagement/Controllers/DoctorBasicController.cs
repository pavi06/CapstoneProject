using HospitalManagement.CustomExceptions;
using HospitalManagement.Interfaces;
using HospitalManagement.Models;
using HospitalManagement.Models.DTOs.DoctorDTOs;
using HospitalManagement.Models.DTOs.PatientDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("MyCors")]
    [AllowAnonymous]
    [ApiController]
    public class DoctorBasicController : ControllerBase
    {
        private readonly IDoctorBasicService _hospitalBasicService;
        private readonly ILogger<DoctorBasicController> _logger;

        public DoctorBasicController(IDoctorBasicService hospitalBasicService, ILogger<DoctorBasicController> logger)
        {
            _hospitalBasicService = hospitalBasicService;
            _logger = logger;
        }

        [HttpPost("GetAllDoctorsBySpecialization")]
        [ProducesResponseType(typeof(List<DoctorReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<DoctorReturnDTO>>> GetAllDoctorsBySpecialization([FromBody] string specialization)
        {
            try
            {
                List<DoctorReturnDTO> result = await _hospitalBasicService.GetAllDoctorsBySpecialization(specialization);
                _logger.LogInformation("Doctors retrieved successfully");
                return Ok(result);
            }
            catch (ObjectsNotAvailableException e)
            {
                _logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
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


        [HttpPost("GetAllDoctorsBySpecializationWithLimit")]
        [ProducesResponseType(typeof(List<DoctorReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<DoctorReturnDTO>>> GetAllDoctorsBySpecialization([FromBody] string specialization, int limit, int skip)
        {
            try
            {
                List<DoctorReturnDTO> result = await _hospitalBasicService.GetAllDoctorsBySpecialization(specialization, limit, skip);
                _logger.LogInformation("Doctors retrieved successfully");
                return Ok(result);
            }
            catch (ObjectsNotAvailableException e)
            {
                _logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
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

        [HttpGet("GetAllSpecialization")]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<string>>> GetAllSpecialization()
        {
            try
            {
                List<string> result = await _hospitalBasicService.GetAllSpecializations();
                _logger.LogInformation("specializations retrieved successfully");
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

        [HttpPost("GetPatientId")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> GetPatientId(PatientFindDTO patientDTO)
        {
            try
            {
                int result = await _hospitalBasicService.GetPatientId(patientDTO);
                _logger.LogInformation("patientId retrieved successfully");
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

        [HttpPost("GetAdmissionId")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> GetAdmissionId(PatientFindDTO patientDTO)
        {
            try
            {
                int result = await _hospitalBasicService.GetAdmissionId(patientDTO);
                _logger.LogInformation("AdmissionId retrieved successfully");
                return Ok(result);
            }
            catch (ObjectsNotAvailableException e)
            {
                _logger.LogError(e.Message);
                return NotFound(new ErrorModel(404, e.Message));
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
