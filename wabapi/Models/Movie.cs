namespace wabapi.Models;

public record Movie
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public List<string>? Actors { get; set; }
    public decimal? Revenue { get; set; }
    public DateOnly? PrimeDate { get; set; }
    
    
}