namespace Assignment3.Entities;

public class TagRepository : ITagRepository
{
    private readonly KanbanContext _context;

    public TagRepository(KanbanContext context)
    {
        _context = context;
    }
    public (Response Response, int TagId) Create(TagCreateDTO tag)
    {
        try 
        {
            _context.Tags.Add(new Tag{
                Name = tag.Name
            });
            _context.SaveChanges();
            var tagId = _context.Tags.Where(t => t.Name == tag.Name).First().Id;
            return (Response.Created,tagId);
        } 
        catch 
        {
            return (Response.Conflict, -1);
        } 
    }

    public Response Delete(int tagId, bool force = false)
    {
        if(force != true) return Response.Conflict;
        var tag = _context.Tags.Where(t=> t.Id == tagId).First();
        if(tag == null) return Response.NotFound;
        _context.Tags.Remove(tag);
        _context.SaveChanges();
        return Response.Deleted;
    }

    public TagDTO Read(int tagId)
    {
        var tag = _context.Tags.Where(t => t.Id == tagId).First();
        if (tag == null) return null;
        TagDTO _tagDTO = new TagDTO (tag.Id,tag.Name);
        return _tagDTO;
    }

    public IReadOnlyCollection<TagDTO> ReadAll()
    {
        List<TagDTO> tagDTOs = new List<TagDTO> {};
        foreach (var item in _context.Tags)
        {
            tagDTOs.Add(new TagDTO(item.Id,item.Name));
        }
        return tagDTOs;
    }

    public Response Update(TagUpdateDTO tag)
    {
        var _tag = _context.Tags.Where(t => t.Id == tag.Id).First();
        if(_tag == null) return Response.NotFound;
        _tag.Name = tag.Name;
        _context.Tags.Update(_tag);
        _context.SaveChanges();
        return Response.Updated;
    }
}
