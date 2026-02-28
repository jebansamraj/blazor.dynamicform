using eFieldData.DynamicForm.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFieldData.DynamicForm.API.Controllers;

[ApiController]
[Authorize]
[Route("api/submissions")]
public class SubmissionsController(IFormService formService) : ControllerBase
{
    [HttpGet("{submissionId:int}")]
    public async Task<IActionResult> GetSubmission(int submissionId, CancellationToken ct)
        => (await formService.GetSubmissionAsync(submissionId, ct)) is { } submission ? Ok(submission) : NotFound();
}
