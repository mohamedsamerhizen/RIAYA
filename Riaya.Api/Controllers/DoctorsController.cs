using Riaya.Api.Common;
using Riaya.Api.Constants;
using Riaya.Api.DTOs.Doctor;
using Riaya.Api.Extensions;
using Riaya.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Riaya.Api.Controllers;

[Route("api/[controller]")]
[Route("api/v1/[controller]")]
[ApiController]
[Authorize(Policy = AppPolicies.ClinicStaff)]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorsController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] DoctorQueryParams queryParams)
    {
        var doctors = await _doctorService.GetAllAsync(queryParams);
        return Ok(ApiResponse<object>.SuccessResponse(doctors));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var doctor = await _doctorService.GetByIdAsync(id);
        if (doctor is null)
            return NotFound(ApiResponse<string>.FailResponse("Doctor not found."));

        return Ok(ApiResponse<object>.SuccessResponse(doctor));
    }

    [HttpPost]
    [Authorize(Policy = AppPolicies.AdminOnly)]
    public async Task<IActionResult> Create(CreateDoctorDto dto)
    {
        var result = await _doctorService.CreateAsync(dto);
        if (!result.Success)
            return this.ToErrorResponse(result);

        return Created(
            $"/api/doctors/{result.Data!.Id}",
            ApiResponse<object>.SuccessResponse(result.Data, result.Message));
    }

    [HttpPut("{id}")]
    [Authorize(Policy = AppPolicies.AdminOnly)]
    public async Task<IActionResult> Update(int id, CreateDoctorDto dto)
    {
        var result = await _doctorService.UpdateAsync(id, dto);

        if (!result.Success)
            return this.ToErrorResponse(result);

        return Ok(ApiResponse<object>.SuccessResponse(result.Data, result.Message));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = AppPolicies.AdminOnly)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _doctorService.DeleteAsync(id);
        if (!result.Success)
            return this.ToErrorResponse(result);

        return Ok(ApiResponse<object>.SuccessResponse(null, result.Message));
    }
}
