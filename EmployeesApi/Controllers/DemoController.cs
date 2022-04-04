

namespace EmployeesApi.Controllers;

public class DemoController : ControllerBase
{
    // GET /status

    [HttpGet("status")]
    public async Task<ActionResult<GetStatusResponse>> GetTheStatus()
    {
        // fake classroom code (FOR NOW)
        var response = new GetStatusResponse(DateTime.Now, ServerStatus.Optimal, "Upgrade tonight");
        return Ok(response); // 200 Ok Status 
    }


    // GET /products/39389839  - Route Data

    // GET /products?category=candy - Query String Argument

    // GET /products (and data in the header)

    // What about stuff that won't fit in the URL, the query string, or the headers.

    // POST /products - having data in the request body.

}



public record GetStatusResponse(DateTime lastChecked, ServerStatus Status, string? Message);
public enum ServerStatus {  Optimal, UnderHeavyLoad, AboutDead, Dying } // Enumerated Constant
