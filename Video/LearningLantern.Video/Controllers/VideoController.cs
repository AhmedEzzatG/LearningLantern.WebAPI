using LearningLantern.Common;
using LearningLantern.Common.Response;
using LearningLantern.Video.Data.Models;
using LearningLantern.Video.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LearningLantern.Video.Controllers;

//[Authorize]
[Route("api/v1/[controller]")]
public class VideoController : ApiControllerBase
{
    private readonly ILogger<VideoController> _logger;
    private readonly IVideoRepository _videoRepository;

    public VideoController(IVideoRepository videoRepository, ILogger<VideoController> logger)
    {
        _videoRepository = videoRepository;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromForm] AddVideoDTO video)
    {
        //video.QuizList = JsonConvert.SerializeObject(video.QuizList);
        //_logger.LogDebug(video.QuizList);
        var response = await _videoRepository.AddAsync(video);
        return response.Succeeded ? Ok(response.Data!.Id) : ResponseToIActionResult(response);
    }

    [HttpGet("{videoId:int}")]
    [ProducesResponseType(typeof(Response<VideoDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromRoute] int videoId)
    {
        var response = await _videoRepository.GetAsync(videoId);
        return ResponseToIActionResult(response);
    }

    [HttpDelete("{videoId:int}")]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove([FromRoute] int videoId)
    {
        var response = await _videoRepository.RemoveAsync(videoId);
        return ResponseToIActionResult(response);
    }
}