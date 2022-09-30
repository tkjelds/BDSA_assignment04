using Microsoft.EntityFrameworkCore;

namespace Assignment3.Entities.Tests;

public class TagRepositoryTests
{
    KanbanContext _context;
    TagRepository _repo;
    public TagRepositoryTests(){
        var ConnectionString = new DbContextOptionsBuilder<KanbanContext>().UseSqlite("DataSource=:memory:")
        .Options;
        _context = new KanbanContext(ConnectionString);
        _repo = new  TagRepository(_context);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated(); 
    }
    [Fact]
    public void TagRepoCreateTest()
    {
        // Given
        var _tagDTO = new TagCreateDTO("test");
        // When 
        var response = _repo.Create(_tagDTO);
        var actual = _context.Tags.Where(t => t.Name == "test").First();
        // Then
        Assert.Equal((Response.Created),response.Response);
    }
    [Fact]
    public void TagRepoDeleteTestWhenForce()
    {
        // Given
        var _tagDTO = new TagCreateDTO("test");
        // When
        var create = _repo.Create(_tagDTO);
        var actual = _repo.Delete(create.TagId, true);
        // Then
        Assert.Equal(Response.Deleted,actual);
    }
    [Fact]
    public void TagRepoDeleteTestWhenNoForce()
    {
        // Given
        var _tagDTO = new TagCreateDTO("test");
        // When
        var create = _repo.Create(_tagDTO);
        var actual = _repo.Delete(create.TagId, false);
        // Then
        Assert.Equal(Response.Conflict,actual);
    }
    [Fact]
    public void TagRepoRead()
    {
        // Given
        var _tagDTO = new TagCreateDTO("test");
        _repo.Create(_tagDTO);
        // When
        var tagId = _context.Tags.Where(t => t.Name == "test").First();
        var actual = _repo.Read(tagId.Id);
        // Then
        Assert.Equal("test",actual.Name);
    }
    [Fact]
    public void TagRepoUpdate()
    {
        // Given
        var _tagDTO = new TagCreateDTO("update me");
        var created = _repo.Create(_tagDTO);
        // When
        var actual = _repo.Update(new TagUpdateDTO(created.TagId,"You have been updated"));
        var newTitle = _repo.Read(created.TagId).Name;
        // Then
        Assert.Equal((actual,"You have been updated"),(Response.Updated,newTitle));
    }
}
