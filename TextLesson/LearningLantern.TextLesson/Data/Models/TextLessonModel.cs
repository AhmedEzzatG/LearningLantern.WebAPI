using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace LearningLantern.TextLesson.Data.Models;

public class TextLessonModel
{
    [Key] [Required] public string Id { get; set; }
    [Required] public string ClassroomId { get; set; }
    [Required] public string Title { get; set; } = null!;
    [Required] public string BlobName { get; set; } = null!;
}