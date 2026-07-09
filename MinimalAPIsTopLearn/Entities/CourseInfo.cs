namespace MinimalAPIsTopLearn.Entities;

public class CourseInfo
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public double Price { get; set; }
    public bool IsFree { get; set; }
    public string? Thumbnail { get; set; }
    public DateTime ReleaseDate { get; set; }
}
