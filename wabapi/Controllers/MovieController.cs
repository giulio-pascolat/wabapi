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
        try
        {
            if (id <= 0)
            {
                return BadRequest("L'ID deve essere un valore positivo.");
            }

            Movie? movie = moviesRepository.GetMovieById(id);

            if (movie == null)
            {
                return NotFound($"Film con ID {id} non trovato.");
            }

            return Ok(movie);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Si è verificato un errore interno: {ex.Message}");
        }
    }

    [HttpGet]
    public IActionResult GetMovies()
    {
        try
        {

            var movies = moviesRepository.GetAllMovies();

            if (movies == null || !movies.Any())
            {
                return NotFound("Nessun film trovato.");
            }

            return Ok(movies);
        }
        catch (SqlException sqlEx)
        {

            return StatusCode(500, $"Errore di database: {sqlEx.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Errore interno del server: {ex.Message}");
        }
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
        try
        {
            var result = moviesRepository.AddMovie(movie);
            if (result >= 1)
            {
                return Ok(movie.Id);
            }
            else
            {
                return BadRequest("Errore nell'aggiunta del film.");
            }
        }
        catch (Exception ex)

        {
            return StatusCode(500, "Errore interno del server: " + ex.Message);
        }
    }


    [HttpDelete("{id}")]
    public IActionResult DeleteMovie(int id)
    {
        try
        {
            var result = moviesRepository.DeleteById(id);
            if (result >= 1)
            {
                return Ok($"Film con ID {id} eliminato con successo.");
            }

            return NotFound($"Film con ID {id} non trovato.");
        }
        catch (SqlException sqlEx)
        {
            return StatusCode(500, $"Errore di database: {sqlEx.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Errore interno del server: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateMovie(int id, [FromBody] Movie movie)
    {
        try
        {
            var existingMovie = moviesRepository.GetMovieById(id);
            if (existingMovie == null)
            {
                return NotFound($"Film con ID {id} non trovato.");
            }

            var result = moviesRepository.UpdateMovie(id, movie);

            if (result > 0)
            {
                return Ok($"Film con ID {id} aggiornato con successo.");
            }

            return BadRequest($"Errore nell'aggiornamento del film con ID {id}.");
        }
        catch (SqlException ex)
        {
            return StatusCode(500, $"Errore nel database: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Errore imprevisto: {ex.Message}");
        }
    }

}

