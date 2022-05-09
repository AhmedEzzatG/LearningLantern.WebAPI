using System.ComponentModel.DataAnnotations;

namespace LearningLantern.TodoList.Data.Models;

public class TaskModel : TaskProperties
{
    [Required] [Key] public int Id { get; set; }
    [Required] public string UserId { get; set; }
    
    public void Update(TaskProperties taskProperties)
    {
        Title = taskProperties.Title;
        DueDate = taskProperties.DueDate;
        Note = taskProperties.Note;
        MyDay = taskProperties.MyDay;
        Completed = taskProperties.Completed;
        Important = taskProperties.Important;
        Repeated = taskProperties.Repeated;
    }

    public override IEnumerable<object?> GetEqualityComponents()
        => base.GetEqualityComponents().Append(Id).Append(UserId);

    public bool Equals(TaskProperties other)
    {
        return base.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }
}