using LearningLantern.ApiGateway.Classroom.DTOs;
using LearningLantern.ApiGateway.Classroom.Models;
using LearningLantern.ApiGateway.Database;
using LearningLantern.ApiGateway.User.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LearningLantern.ApiGateway.Classroom.Repositories;

public class ClassroomRepository : IClassroomRepository
{
    private readonly LearningLanternContext _learningLanternContext;
    private readonly IUserRepository _userRepository;

    public ClassroomRepository(IUserRepository userRepository, LearningLanternContext learningLanternContext)
    {
        _userRepository = userRepository;
        _learningLanternContext = learningLanternContext;
    }

    public async Task<List<ClassroomDTO>> GetAsync(string userId) => await _learningLanternContext.ClassroomUsers
        .Where(classroomUser => classroomUser.UserId == userId)
        .Select(classroomUser => new ClassroomDTO(classroomUser.Classroom)).ToListAsync();

    public async Task<int?> AddAsync(AddClassroomDTO addClassroomDTO, string userId)
    {
        var user = await _userRepository.FindUserByIdAsync(userId);

        if (user == null) return null;

        var classroom = await _learningLanternContext.Classrooms.AddAsync(new ClassroomModel(addClassroomDTO));

        if (classroom == null || await _learningLanternContext.SaveChangesAsync() == 0) return 0;

        var classroomUser =
            await _learningLanternContext.ClassroomUsers.AddAsync(new ClassroomUserModel(classroom.Entity.Id, userId));

        return classroomUser != null && await _learningLanternContext.SaveChangesAsync() != 0 ? classroom.Entity.Id : 0;
    }

    public async Task<bool?> AddUserAsync(int classroomId, string requestUserId, string userId)
    {
        var classroomUser = await _learningLanternContext.ClassroomUsers.Where(classroomUser =>
            classroomUser.ClassroomId == classroomId && classroomUser.UserId == requestUserId).FirstOrDefaultAsync();

        if (classroomUser == null) return null;

        var addAsyncResult =
            await _learningLanternContext.ClassroomUsers.AddAsync(new ClassroomUserModel(classroomId, userId));

        return addAsyncResult != null && await _learningLanternContext.SaveChangesAsync() != 0;
    }

    public async Task<bool?> UpdateAsync(ClassroomDTO classroomDTO, string userId)
    {
        var classroomUser = await _learningLanternContext.ClassroomUsers
            .Where(classroomUser => classroomUser.ClassroomId == classroomDTO.Id && classroomUser.UserId == userId)
            .FirstOrDefaultAsync();

        if (classroomUser == null) return null;

        var classroom = _learningLanternContext.Classrooms.Update(new ClassroomModel(classroomDTO));

        return classroom != null && await _learningLanternContext.SaveChangesAsync() != 0;
    }

    public async Task<bool?> RemoveUserAsync(int classroomId, string requestUserId, string userId)
    {
        var requestUser = await _userRepository.FindUserByIdAsync(requestUserId);

        if (requestUser == null) return null;

        var classroomUser = await _learningLanternContext.ClassroomUsers
            .Where(classroomUser => classroomUser.UserId == userId && classroomUser.ClassroomId == classroomId)
            .FirstOrDefaultAsync();

        if (classroomUser == null) return null;

        classroomUser = _learningLanternContext.ClassroomUsers.Remove(classroomUser).Entity;

        return classroomUser != null && await _learningLanternContext.SaveChangesAsync() != 0;
    }

    public async Task<bool?> RemoveAsync(int classroomId, string userId)
    {
        var classroomUser = await _learningLanternContext.ClassroomUsers
            .Where(classroomUser => classroomUser.ClassroomId == classroomId && classroomUser.UserId == userId)
            .FirstOrDefaultAsync();

        if (classroomUser == null) return null;

        var classroom = _learningLanternContext.Classrooms.Remove(classroomUser.Classroom).Entity;

        return classroom != null && await _learningLanternContext.SaveChangesAsync() != 0;
    }
}