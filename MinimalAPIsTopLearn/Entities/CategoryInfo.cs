using System.ComponentModel.DataAnnotations;

namespace MinimalAPIsTopLearn.Entities
{
    public class CategoryInfo
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
