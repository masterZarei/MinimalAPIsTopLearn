namespace MinimalAPIsTopLearn.Entities;

public class InstructorInfo
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Picture { get; set; }
}
