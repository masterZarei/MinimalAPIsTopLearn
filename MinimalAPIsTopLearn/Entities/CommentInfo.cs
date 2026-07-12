using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalAPIsTopLearn.Entities;

public class CommentInfo
{
    public int Id { get; set; }
    public required string Body { get; set; }
    public int CourseId { get; set; }

    [ForeignKey(nameof(CourseId))]
    public CourseInfo Course { get; set; }
}
