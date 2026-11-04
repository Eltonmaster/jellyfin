using System;
using Jellyfin.Api.Controllers;
using MediaBrowser.Controller.Entities.Movies;
using Xunit;

namespace Jellyfin.Api.Tests.Controllers;

public class LibraryControllerTests
{
    [Theory]
    [InlineData("https://example.com/media/movie.mkv", null, null, "Movie (2020).mkv")]
    [InlineData("https://example.com/media/movie.mp4?token=abc", "video/x-matroska", "mkv", "Movie (2020).mp4")]
    [InlineData("https://example.com/stream/123", "video/x-matroska", null, "Movie (2020).mkv")]
    [InlineData("https://example.com/stream/123", "video/mp4", "mkv", "Movie (2020).mp4")]
    [InlineData("https://example.com/stream/123", "application/octet-stream", "mkv", "Movie (2020).mkv")]
    [InlineData("https://example.com/stream/123", null, "mov,mp4,m4a,3gp,3g2,mj2", "Movie (2020).mp4")]
    [InlineData("https://example.com/play.php?id=123", "text/html; charset=UTF-8", "avi", "Movie (2020).avi")]
    [InlineData("https://example.com/stream/123", "application/octet-stream", null, "Movie (2020)")]
    public void GetShortcutDownloadName_UsesStrmNameWithMediaExtension(string url, string? contentType, string? container, string expected)
    {
        var item = new Movie
        {
            Path = "/media/movies/Movie (2020).strm",
            IsShortcut = true,
            ShortcutPath = url,
            Container = container
        };

        Assert.Equal(expected, LibraryController.GetShortcutDownloadName(item, new Uri(url), contentType));
    }
}
