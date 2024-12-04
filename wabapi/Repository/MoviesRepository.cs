using Microsoft.Data.SqlClient;
using wabapi.Models;
using wabapi.SeedWork;
using System.Collections.Generic;

namespace wabapi.Repository
{
    public class MoviesRepository : IMovieRepository
    {
        private static readonly string ConnectionString = "Server=localhost,1433;Database=MoviesDb;User Id=SA;Password=Argo1203!;TrustServerCertificate=True;";
        private readonly SqlConnection _connection = new(ConnectionString);

        public Movie? GetMovieById(int id)
        {
            var query = "SELECT * FROM Movies WHERE Id = @id";
            _connection.Open();
            var sqlCommand = new SqlCommand(query, _connection);
            sqlCommand.Parameters.AddWithValue("@id", id);
            using var reader = sqlCommand.ExecuteReader();

            var movie = new Movie();

            if (reader.Read())
            {
                movie.Id = reader.GetInt32(0);
                movie.Title = reader.GetString(1);
                movie.Revenue = reader.GetDecimal(2);
                movie.PrimeDate = DateOnly.FromDateTime(reader.GetDateTime(3));
            }

            _connection.Close();
            return movie;
        }

        public int AddMovie(Movie movie)
        {
            var query = """
                         INSERT INTO [dbo].[Movies] ([Id], [Title], [Revenue], [PrimeDate])
                                         VALUES (@Id, @Title, @Revenue, @PrimeDate)
                        """;
            _connection.Open();
            var sqlCommand = new SqlCommand(query, _connection);
            sqlCommand.Parameters.AddWithValue("@Id", movie.Id);
            sqlCommand.Parameters.AddWithValue("@Title", movie.Title);
            sqlCommand.Parameters.AddWithValue("@Revenue", movie.Revenue);
            sqlCommand.Parameters.AddWithValue("@PrimeDate", movie.PrimeDate);

            var result = sqlCommand.ExecuteNonQuery();
            _connection.Close();
            return result;
        }

        public IEnumerable<Movie> GetAllMovies()
        {
            var query = "SELECT * FROM Movies";
            using var connection = new SqlConnection(ConnectionString); // connessione ogni volta
            connection.Open();
            var sqlCommand = new SqlCommand(query, connection);
            using var reader = sqlCommand.ExecuteReader();

            var movies = new List<Movie>();

            while (reader.Read())
            {
                movies.Add(new Movie()
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Revenue = reader.GetDecimal(2),
                    PrimeDate = DateOnly.FromDateTime(reader.GetDateTime(3)),
                });
            }

            return movies;
        }

        public int DeleteById(int id)
        {
            var query = "DELETE FROM Movies WHERE Id = @id";
            _connection.Open();
            var sqlCommand = new SqlCommand(query, _connection);
            sqlCommand.Parameters.AddWithValue("@id", id);
            var result = sqlCommand.ExecuteNonQuery();
            _connection.Close();
            return result;
        }

        public int UpdateMovie(int id, Movie movie)
        {
            var query = """
                         UPDATE [dbo].[Movies]
                         SET [Title] = @Title,
                             [Revenue] = @Revenue,
                             [PrimeDate] = @PrimeDate
                         WHERE [Id] = @Id;
                        """;
            
            _connection.Open();
            var sqlCommand = new SqlCommand(query, _connection);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@Title", movie.Title);
            sqlCommand.Parameters.AddWithValue("@Revenue", movie.Revenue);
            sqlCommand.Parameters.AddWithValue("@PrimeDate", movie.PrimeDate);
            return sqlCommand.ExecuteNonQuery();
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await _connection.DisposeAsync();
        }
    }
}
