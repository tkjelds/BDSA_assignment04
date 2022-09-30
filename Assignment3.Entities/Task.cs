

namespace Assignment3.Entities;

    public class Task
    {
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    public User? AssignedTo { get; set; }

    [MaxLength(int.MaxValue)]
    public string? Description { get; set; }

    [Required]
    public State State { get; set; }

    public IEnumerable<Tag> Tags {get; set;}

    public DateTime Created {get;set;}
    public DateTime StateUpdated {get;set;}
    }
