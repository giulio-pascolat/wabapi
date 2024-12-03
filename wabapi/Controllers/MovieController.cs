using Microsoft.AspNetCore.Mvc;
using wabapi.Models;

namespace wabapi.Controllers;


[Route("api/[controller]")]
[ApiController]
public class MovieController : Controller
{

    [HttpGet("/{id}")]
    public IActionResult GetMovie([FromBody] int id )
    {
        var movie = new Movie();
        movie.Id = id;
        return Ok(movie);
    }


    [HttpPost]
    public IActionResult AddMovie([FromBody] Movie movie)
    {
        return Ok(movie.Id);
    }
    
    
}