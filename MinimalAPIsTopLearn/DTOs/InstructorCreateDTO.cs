namespace MinimalAPIsTopLearn.DTOs;

public class InstructorCreateDTO
{
    public required string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public IFormFile? Picture { get; set; }
}
