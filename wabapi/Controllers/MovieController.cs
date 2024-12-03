using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using wabapi.Models;
using wabapi.Repository;
using wabapi.SeedWork;

namespace wabapi.Controllers;


[Route("api/[controller]")]
[ApiController]
public class MovieController(IMovieRepository moviesRepository) : Controller
{
    

    [HttpGet("/{id}")]
    public IActionResult GetMovie(int id)
    {
        return Ok(moviesRepository.GetMovieById(id));
    }
    
    // {
    //     "id": 150,
    //     "title": "Alien",
    //     "actors": [
    //     "Sigurney Weaver"
    //         ],
    //     "revenue": 108000000,
    //     "primeDate": "1979-09-06"
    // }


    [HttpPost]
    public IActionResult AddMovie([FromBody] Movie movie)
    {
        if (moviesRepository.AddMovie(movie) >= 1)
        {
            return Ok(movie.Id);
        }
        return BadRequest();
    }
    
    
}