using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Assignment3.Entities.Tests;

public class UserRepositoryTests
{
    KanbanContext _context;
    UserRepository _repo;
    public UserRepositoryTests(){
        var ConnectionString = new DbContextOptionsBuilder<KanbanContext>().UseSqlite("DataSource=:memory:")
        .Options;
        _context = new KanbanContext(ConnectionString);
        _repo = new  UserRepository(_context);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated(); 
    }
    [Fact]
    public void CreateUserReturnsCreated()
    {
        // Given
        var user = new UserCreateDTO("Test","Test@test.com");
        // When
        var (response,userid) = _repo.Create(user);
        // Then
        Assert.Equal(Response.Created,response);
        Assert.Equal(1,userid);
    }

    [Fact]
    public void UserRepoCreateTest()
    {
        // Given
        UserCreateDTO userCreateDTO = new UserCreateDTO("test","test@test.com");
        // When
        var actual = _repo.Create(userCreateDTO);
        // Then
        Assert.Equal(Response.Created,  actual.Response);
    }
    [Fact]
    public void UserRepoDeleteWhenForceTest()
    {
        // Given
        UserCreateDTO userCreateDTO = new UserCreateDTO("test","test@test.com");
        // When
        var user = _repo.Create(userCreateDTO);
        var actual = _repo.Delete(user.UserId,true);
        // Then
        Assert.Equal(Response.Deleted,actual);
    }
    [Fact]
    public void UserRepoDeleteWhenNoForceTest()
    {
        // Given
        UserCreateDTO userCreateDTO = new UserCreateDTO("test","test@test.com");
        // When
        var user = _repo.Create(userCreateDTO);
        var actual = _repo.Delete(user.UserId,false);
        // Then
        Assert.Equal(Response.Conflict,actual);
    }
    [Fact]
    public void UserRepoReadTest()
    {
        // Given
        UserCreateDTO userCreateDTO = new UserCreateDTO("test","test@test.com");
        // When
        var user = _repo.Create(userCreateDTO);
        var actual = _repo.Read(user.UserId);
        // Then
        Assert.Equal("test@test.com",actual.Email);
    }
    [Fact]
    public void UserRepoReadFailTest()
    {
        // Given
        // When
        var actual = _repo.Read(-1);
        // Then
        Assert.Equal(null,actual);
    }
    [Fact]
    public void UserRepoUpdateTest()
    {
        // Given
        UserCreateDTO userCreateDTO = new UserCreateDTO("ChangeMe","test@test.com");
        // When
        var user = _repo.Create(userCreateDTO);
        var actual = _repo.Update(new UserUpdateDTO(user.UserId,"I am changed","test@test.com"));
        var changedUser = _repo.Read(user.UserId);
        // Then
        Assert.Equal("I am changed",changedUser.Name);
    }
}
