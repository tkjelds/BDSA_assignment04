namespace Assignment3.Entities;

public class TaskRepository : ITaskRepository
{
    private readonly KanbanContext _context;

    public TaskRepository(KanbanContext context)
    {
        _context = context;
    }
    public (Response Response, int TaskId)  Create(TaskCreateDTO task)
    {
        // Check if user exists
        var user =  _context.Users.Where(u => u.Id == task.AssignedToId).FirstOrDefault();
        if (user == null) return (Response.BadRequest,-1);
        // Check if tags exist, if they dont, create them.
        var tagdict = _context.Tags.ToDictionary(t => t.Name, t => t.Id);
        List<Tag> tags_ = new List<Tag>();
        foreach (var item in task.Tags)
        {   
            if (!tagdict.ContainsKey(item)) _context.Tags.Add(new Tag { Name = item });
            
        }
        _context.SaveChanges();
        foreach (var tagName in task.Tags)
        {
            tags_.Add(_context.Tags.Where(t => t.Name == tagName).First());
        }
        // Create new task 
        Task tsk = new Task()
        {
            Title = task.Title,
            AssignedTo = user,
            Description = task.Description,
            State = State.New,
            Tags = tags_,
            Created = DateTime.Now,
            StateUpdated = DateTime.Now
        };
        // add task
        _context.Tasks.Add(tsk);
        _context.SaveChanges();
        return (Response.Created,_context.Tasks.Where(t => t.Title == task.Title).First().Id);
    }
    public Response Delete(int taskId)
    {
        // Check if task exists
        var task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
        if (task == null) return Response.BadRequest; // return bad request if task does not exist
        // If the task is active, set the state to 
        if(task.State == State.Active){
            task.State = State.Removed;
            task.StateUpdated = DateTime.Now;
            _context.Update(task);
            _context.SaveChanges();
            return Response.Updated;
        }
        if (task.State != State.New) return Response.Conflict;
        _context.Remove(task);
        _context.SaveChanges();
        return Response.Deleted;
    }

    public TaskDetailsDTO Read(int taskId)
    {
        //Check if task exists
        var task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
        if (task == null) return null;
        TaskDetailsDTO tdo = new TaskDetailsDTO(task.Id,
                                                task.Title,
                                                task.Description,
                                                task.Created,
                                                task.AssignedTo.Name,
                                                task.Tags.Select(t => t.Name).ToList(),
                                                task.State,
                                                task.StateUpdated);
        return tdo;
    }

    public IReadOnlyCollection<TaskDTO> ReadAll()
    {
        List<TaskDTO> tasks = new List<TaskDTO>();
        foreach (var Task in _context.Tasks)
        {
            tasks.Add(new TaskDTO(Task.Id,
                                  Task.Title,
                                  Task.AssignedTo.Name,
                                  Task.Tags.Select(t => t.Name).ToList(),
                                  Task.State));
        }
        return tasks;
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
    {
        List<TaskDTO> tasks = new List<TaskDTO>();
        foreach (var Task in _context.Tasks.Where(t => t.State == state))
        {
            tasks.Add(new TaskDTO(Task.Id,
                                  Task.Title,
                                  Task.AssignedTo.Name,
                                  Task.Tags.Select(t => t.Name).ToList(),
                                  Task.State));
        }
        return tasks;
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
    {
        List<TaskDTO> tasks = new List<TaskDTO>();
        foreach (var Task in _context.Tasks.Where(t => t.Tags.Select(t => t.Name).Contains(tag)))
        {
            tasks.Add(new TaskDTO(Task.Id,
                                  Task.Title,
                                  Task.AssignedTo.Name,
                                  Task.Tags.Select(t => t.Name).ToList(),
                                  Task.State));
        }
        return tasks;
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
    {
        List<TaskDTO> tasks = new List<TaskDTO>();
        foreach (var Task in _context.Tasks.Where(t => t.AssignedTo.Id == userId))
        {
            tasks.Add(new TaskDTO(Task.Id,
                                  Task.Title,
                                  Task.AssignedTo.Name,
                                  Task.Tags.Select(t => t.Name).ToList(),
                                  Task.State));
        }
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
    {
        List<TaskDTO> tasks = new List<TaskDTO>();
        foreach (var Task in _context.Tasks.Where(t => t.State == State.Removed))
        {
            tasks.Add(new TaskDTO(Task.Id,
                                  Task.Title,
                                  Task.AssignedTo.Name,
                                  Task.Tags.Select(t => t.Name).ToList(),
                                  Task.State));
        }
        throw new NotImplementedException();
    }

    public Response Update(TaskUpdateDTO task)
    {
        // Check if user exists
        var user =  _context.Users.Where(u => u.Id == task.AssignedToId).FirstOrDefault();
        // Check if tags exist, if they dont, create them.
        var tagdict = _context.Tags.ToDictionary(t => t.Name, t => t.Id);
        List<Tag> tags_ = new List<Tag>();
        foreach (var item in task.Tags)
        {   
            if (!tagdict.ContainsKey(item)) _context.Tags.Add(new Tag { Name = item });
            
        }
        _context.SaveChanges();
        foreach (var tagName in task.Tags)
        {
            tags_.Add(_context.Tags.Where(t => t.Name == tagName).First());
        }
        // Create new task
        Task tsk = new Task()
        {
            Id = task.Id,
            Title = task.Title,
            AssignedTo = user,
            Description = task.Description,
            Tags = tags_,
            State = task.State,
            StateUpdated = DateTime.Now
        };
        _context.Update(tsk);
        _context.SaveChanges();
        return Response.Updated;
    }
}
