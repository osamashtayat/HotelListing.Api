using HotelListing.Api.Common.Results;

namespace HotelListing.Api.Tests;

public sealed class ResultTests
{
    [Fact]
    public void Combine_WhenAllSucceed_ReturnsSuccess()
    {
        var result = Result.Combine(Result.Success(), Result.Success());

        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Combine_WhenAnyFail_AggregatesErrors()
    {
        var first = new Error("first", "First failure");
        var second = new Error("second", "Second failure");

        var result = Result.Combine(Result.Failure(first), Result.Success(), Result.Failure(second));

        Assert.False(result.IsSuccess);
        Assert.Equal([first, second], result.Errors);
    }

    [Fact]
    public void GenericResult_Map_TransformsSuccessfulValue()
    {
        var result = Result<int>.Success(4).Map(value => value * 2);

        Assert.True(result.IsSuccess);
        Assert.Equal(8, result.Value);
    }

    [Fact]
    public void GenericResult_Ensure_ConvertsInvalidValueToFailure()
    {
        var error = new Error("range", "Value must be positive");

        var result = Result<int>.Success(-1).Ensure(value => value > 0, error);

        Assert.False(result.IsSuccess);
        Assert.Equal(error, Assert.Single(result.Errors));
    }
}
