namespace MinimalAPIsTopLearn.DTOs;

public class CourseCreateDTO
{
    public required string Title { get; set; }
    public double Price { get; set; }
    public bool IsFree { get; set; }
    public IFormFile? Thumbnail { get; set; }
    public DateTime ReleaseDate { get; set; }
}
