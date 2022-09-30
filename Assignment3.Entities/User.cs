

namespace Assignment3.Entities;

public class User
{
    public int Id {get;set;}
    [Required]
    [MaxLength(100)]
    public string Name {get;set;}

    [Required]
    [MaxLength(100)]
    public string Email {get;set;}

    public IEnumerable<Task> Tasks {get;set;}
}
