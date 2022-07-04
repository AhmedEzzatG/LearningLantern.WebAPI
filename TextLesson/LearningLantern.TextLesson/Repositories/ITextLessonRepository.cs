using Azure.Storage.Blobs.Models;
using LearningLantern.Common.Response;
using LearningLantern.TextLesson.Data.Models;

namespace LearningLantern.TextLesson.Repositories;

public interface ITextLessonRepository
{
    Task<Response<TextLessonDTO>> AddAsync(string title, string classroomId);
    Task<Response<IFormFile>> AddAsync(AddTextLessonDTO textLesson);
    Task<Response<BlobDownloadInfo>> GetAsync(string textLessonId);
    Task<Response<List<TextLessonDTO>>> GetTextLessonsAsync(string classroomId);
    Task<Response> RemoveAsync(string textLessonId);
}