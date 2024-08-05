using HospitalManagement.CustomExceptions;
using HospitalManagement.Interfaces;
using HospitalManagement.Models.DTOs.DoctorDTOs;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HospitalManagement.Models.DTOs.MedicineDTOs;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Doctor")]
    [EnableCors("MyCors")]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineService _medicineService;
        //private readonly ILogger<MedicineController> _logger;

        public MedicineController(IMedicineService medicineService) {
            _medicineService = medicineService;
            //_logger = logger;
        }


        [HttpGet("GetAllMedicineNames")]
        [ProducesResponseType(typeof(List<MedicineReturnDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<MedicineReturnDTO>>> GetAllMedicines()
        {
            try
            {
                List<MedicineReturnDTO> result = await _medicineService.GetAllMedicineNames();
                //_logger.LogInformation("Medicine retrieved successfully");
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

        [HttpGet("GetDetailsOfMedicine")]
        [ProducesResponseType(typeof(MedicineDetailsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MedicineDetailsDTO>> GetMedicineDetails(int id)
        {
            try
            {
                MedicineDetailsDTO result = await _medicineService.GetMedicineDetailsById(id);
                //_logger.LogInformation("Medicine details retrieved successfully");
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
