namespace Assignment3.Entities;

public class UserRepository : IUserRepository
{
    private readonly KanbanContext _context;

    public UserRepository(KanbanContext context)
    {
        _context = context;
    }
    public (Response Response, int UserId) Create(UserCreateDTO user)
    {
        var _user = _context.Users.FirstOrDefault(u => u.Email == user.Email);
        if (_user is not null) return (Response.Conflict, _user.Id);
        var entity = new User(){Name = user.Name, Email = user.Email};
        _context.Users.Add(entity); 
        _context.SaveChanges();
        return (Response.Created,entity.Id);

    }

    public Response Delete(int userId, bool force = false)
    {
        // Check if force is used
        if(force != true) return Response.Conflict;
        // Check if user exists
        try{
        var _user = _context.Users.Where(u => u.Id == userId).First();
        _context.Users.Remove(_user);
        _context.SaveChanges();
        return Response.Deleted;
        } catch {
            return (Response.Conflict);
        }
    }

    public UserDTO Read(int userId)
    {
        // Check if user exists
        try {
        var _user = _context.Users.Where(u => u.Id == userId).First();
        return (new UserDTO(            
            _user.Id,
            _user.Name,
            _user.Email)

        );
        } catch {
            return null;
        }
    }

    public IReadOnlyCollection<UserDTO> ReadAll()
    {
        List<UserDTO> userDTOs = new List<UserDTO> {};
        foreach (var user in _context.Users)
        {
            userDTOs.Add(new UserDTO(user.Id,user.Name,user.Email));
        }
        return userDTOs;
    }

    public Response Update(UserUpdateDTO user)
    {
        try 
        {
        var _user = _context.Users.Where(u => u.Id == user.Id).First();
        _user.Email = user.Email;
        _user.Name = user.Name;
        _context.Users.Update(_user);
        _context.SaveChanges();
        return Response.Updated;
        } 
        catch
        {
        return Response.Conflict;
        }
    }
}
