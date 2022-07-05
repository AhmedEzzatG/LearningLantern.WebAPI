using System.Runtime.CompilerServices;
using AutoMapper;
using Azure.Storage.Blobs;
using LearningLantern.AzureBlobStorage;
using LearningLantern.Common.Response;
using LearningLantern.Video.Data;
using LearningLantern.Video.Data.Models;
using LearningLantern.Video.Utility;

namespace LearningLantern.Video.Repositories;

public class VideoRepository : IVideoRepository
{
    private readonly IBlobService _blobServiceClient;
    private readonly IVideoContext _context;
    private readonly ILogger<VideoRepository> _logger;
    private readonly IMapper _mapper;

    public VideoRepository(
        IVideoContext context, IBlobService blobServiceClient, IMapper mapper, ILogger<VideoRepository> logger)
    {
        _context = context;
        _blobServiceClient = blobServiceClient;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Response<string>> AddAsync(AddVideoDTO video)
    {
        var videoModel = new VideoModel()
        {
            BlobName = Guid.NewGuid().ToString(),
            QuizList = video.QuizList
        };

        var result = await _blobServiceClient.UploadBlobAsync(videoModel.BlobName, video.File);
        
        if (result == string.Empty)
            return ResponseFactory.Fail<string>(ErrorsList.CantUploadFile());
        
        videoModel.Path = result;

        var entity = await _context.Videos.AddAsync(videoModel);

        var saveResult = await _context.SaveChangesAsync();

        return saveResult != 0
            ? ResponseFactory.Ok(videoModel.BlobName)
            : ResponseFactory.Fail<string>();
    }

    public async Task<Response<VideoDTO>> GetAsync(int videoId)
    {
        var video = await _context.Videos.FindAsync(videoId);

        if (video is null)
            return ResponseFactory.Fail<VideoDTO>(ErrorsList.VideoNotFound(videoId));

        return ResponseFactory.Ok(_mapper.Map<VideoDTO>(video));
    }

    public async Task<Response> RemoveAsync(int videoId)
    {
        var video = await _context.Videos.FindAsync(videoId);

        if (video == null) return ResponseFactory.Ok();

        await  _blobServiceClient.DeleteBlobAsync(video.BlobName);

        _context.Videos.Remove(video);

        var result = await _context.SaveChangesAsync() != 0;

        return result ? ResponseFactory.Ok() : ResponseFactory.Fail();
    }
}