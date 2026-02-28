using eFieldData.DynamicForm.Application.Abstractions;
using eFieldData.DynamicForm.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFieldData.DynamicForm.API.Controllers;

[ApiController]
[Authorize]
[Route("api/forms")]
public class FormsController(IFormService formService) : ControllerBase
{
    [HttpGet] public async Task<IActionResult> Get(CancellationToken ct) => Ok(await formService.GetFormsAsync(ct));
    [HttpGet("{id:int}")] public async Task<IActionResult> Get(int id, CancellationToken ct) => (await formService.GetFormAsync(id, ct)) is { } f ? Ok(f) : NotFound();
    [HttpPost] [Authorize(Roles = "Admin")] public async Task<IActionResult> Post([FromBody] FormUpsertRequest request, CancellationToken ct) => Ok(await formService.CreateFormAsync(request, ct));
    [HttpPut("{id:int}")] [Authorize(Roles = "Admin")] public async Task<IActionResult> Put(int id, [FromBody] FormUpsertRequest request, CancellationToken ct) => (await formService.UpdateFormAsync(id, request, ct)) is { } f ? Ok(f) : NotFound();
    [HttpDelete("{id:int}")] [Authorize(Roles = "Admin")] public async Task<IActionResult> Delete(int id, CancellationToken ct) => await formService.DeleteFormAsync(id, ct) ? NoContent() : NotFound();
    [HttpPost("{id:int}/submit")] public async Task<IActionResult> Submit(int id, [FromBody] SubmitFormRequest request, CancellationToken ct) => (await formService.SubmitFormAsync(id, request, ct)) is { } sid ? Ok(new { submissionId = sid }) : NotFound();
    [HttpGet("{id:int}/submissions")] public async Task<IActionResult> Submissions(int id, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? status = null, [FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null, CancellationToken ct = default) => Ok(await formService.GetSubmissionsAsync(id, page, pageSize, status, fromDate, toDate, ct));
}
