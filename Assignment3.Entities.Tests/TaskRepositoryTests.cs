using Microsoft.EntityFrameworkCore;

namespace Assignment3.Entities.Tests;

public class TaskRepositoryTests
{
    KanbanContext _context;
    TaskRepository _repo;
    public TaskRepositoryTests(){
        var connectionString = new DbContextOptionsBuilder<KanbanContext>().UseSqlite("DataSource=:memory:")
        .Options;
        _context = new KanbanContext(connectionString);
        _repo = new  TaskRepository(_context);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated(); 
    }
    [Fact]
    public void TaskCreateTest()
    {
        //Given
        var user = new User(){Name = "test",Email = "Test@test@Test",Id = 42};
        _context.Users.Add(user);
        _context.SaveChanges();
        //When
        TaskCreateDTO tcd = new TaskCreateDTO("test",42,"test",new [] {"test","test2"});
        var actual = _repo.Create(tcd);
        _context.Users.Remove(user);
        _context.SaveChanges();
        //Then
        Assert.Equal(Response.Created,actual.Response);
    }

    [Fact]
    public void TaskCreateFailUserNotExist()
    {
        //Given
        //When
        TaskCreateDTO tcd = new TaskCreateDTO("test",42,"test",new [] {"test","test2"});
        var actual = _repo.Create(tcd);
        _context.SaveChanges();
        //Then
        Assert.Equal(Response.BadRequest,actual.Response);
    }
    [Fact]
    public void TaskDeleteSetStateRemoved()
    {
        //Given
        var user = new User(){Name = "test",Email = "Test@test@Test",Id = 42};
        _context.Users.Add(user);
        _context.SaveChanges();
        //When
        TaskCreateDTO tcd = new TaskCreateDTO("test",42,"test",new [] {"test","test2"});
        var task = _repo.Create(tcd);
        _context.Users.Remove(user);
        _context.SaveChanges();
        var actual = _repo.Delete(task.TaskId);
        //Then
        Assert.Equal(Response.Deleted,actual);
    }
    // [Fact]
    // public void TaskUpdateSetStateActive()
    // {
    //     //Given
    //     KanbanContext _context = new KanbanContext();
    //     TaskRepository _repo = new TaskRepository(_context);
    //     _context.Tasks.RemoveRange(_context.Tasks);
    //     var user = new User(){Name = "test",Email = "Test@test@Test",Id = 42};
    //     _context.Users.Add(user);
    //     _context.SaveChanges();
    //     //When
    //     TaskCreateDTO tcd = new TaskCreateDTO("test",42,"test",new [] {"test","test2"});
    //     var task = _repo.Create(tcd);
    //     _context.Users.Remove(user);
    //     _context.SaveChanges();
    //     TaskUpdateDTO tud = new TaskUpdateDTO(task.TaskId,
    //                                           "test",
    //                                           null,
    //                                           "test",
    //                                           new[] { "test", "test2" },
    //                                           State.Active);
    //     var actual = _repo.Update(tud);
    //     _context.SaveChanges();
    //     //Then
    //     Assert.Equal(Response.Updated,actual);
    // }
}
