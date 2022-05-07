using LearningLantern.Common.Models.CalendarModels;
using Xunit;

namespace LearningLantern.Calendar.Tests;

public class EventModelTests
{
    [Fact]
    public void TestUpdateFunction()
    {
        // arrange
        var eventModel = new EventModel();
        var eventProperties = Helper.GenerateUpdateEventDTO();
        // act
        eventModel.Update(eventProperties);
        // assert
        Assert.Equal(eventModel.Title, eventProperties.Title);
        Assert.Equal(eventModel.Description, eventProperties.Description);
        Assert.Equal(eventModel.StartTime, eventProperties.StartTime);
        Assert.Equal(eventModel.EndTime, eventProperties.EndTime);
    }
}