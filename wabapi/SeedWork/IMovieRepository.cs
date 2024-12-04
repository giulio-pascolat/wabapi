using wabapi.Models;

namespace wabapi.SeedWork;

public interface IMovieRepository : IDisposable, IAsyncDisposable
{
    public Movie? GetMovieById(int id);

    public int AddMovie(Movie movie);
    
    public IEnumerable<Movie>? GetAllMovies();

    public int DeleteById(int id);
    
    public int UpdateMovie(int id, Movie movie);


}