
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace LyricsWeb.APIs.CloudMusicAPI;

public class CloudMusicSearchHelper
{
    private static readonly NeteaseCloudMusicApi _api = new();
    private const byte Limit = 10;

    private static async Task<JsonDocument> GetJsonElement(string keyWords)
    {
        var (_, result) = await _api.RequestAsync(
            CloudMusicApiProviders.Search,
            new Dictionary<string, string>
            {
                    { "keywords", keyWords },
                    { "limit",Limit.ToString() },
                    { "offset", "0" },
            }
        );
        JsonDocument document = JsonDocument.Parse(result.ToJsonString());
        return document;
    }


    public static async Task<byte[]> GetSongAlbum(string title, string album, string author, CancellationToken cancellationToken = default)
    {
        try
        {
            string keyWords = album + " " + author;
            cancellationToken.ThrowIfCancellationRequested();
            using JsonDocument document = await GetJsonElement(keyWords);
            JsonElement root = document.RootElement;
            cancellationToken.ThrowIfCancellationRequested();
            string albumId = SearchForAlbumId(root, album, author);
            cancellationToken.ThrowIfCancellationRequested();
            string albumcoverUrl = await GetAlbumUrl(albumId);
            cancellationToken.ThrowIfCancellationRequested();
            return await _api.GetImageBytesFromUrlAsync(albumcoverUrl, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static async Task<string> GetSongLyrics(string title, string album, string author, CancellationToken cancellationToken = default)
    {
        try
        {
            string keyWords = title + " " + album + " " + author;
            cancellationToken.ThrowIfCancellationRequested();
            using JsonDocument document = await GetJsonElement(keyWords);
            JsonElement root = document.RootElement;
            cancellationToken.ThrowIfCancellationRequested();
            string songId = SearchForSongId(root, title, author);
            cancellationToken.ThrowIfCancellationRequested();
            var lyrics = await GetLyricsUrl(songId, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return lyrics;
        }
        catch (OperationCanceledException)
        {
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static async Task<string> GetTranslateSongLyrics(string title, string album, string author, CancellationToken cancellationToken = default)
    {
        try
        {
            string keyWords = title + " " + album + " " + author;
            cancellationToken.ThrowIfCancellationRequested();
            using JsonDocument document = await GetJsonElement(keyWords);
            JsonElement root = document.RootElement;
            cancellationToken.ThrowIfCancellationRequested();
            string songId = SearchForSongId(root, title, author);
            cancellationToken.ThrowIfCancellationRequested();
            var lyrics = await GetTranslateLyrics(songId);
            cancellationToken.ThrowIfCancellationRequested();
            return lyrics;
        }
        catch (OperationCanceledException)
        {
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static string SearchForSongId(JsonElement root, string title, string artist)
    {
        try
        {
            JsonElement songsArray = root.GetProperty("result").GetProperty("songs");
            foreach (JsonElement songElement in songsArray.EnumerateArray())
            {
                string songName = songElement.GetProperty("name").GetString();
                if (string.Equals(songName, title, StringComparison.OrdinalIgnoreCase))
                {
                    JsonElement artistsArray = songElement.GetProperty("artists");
                    foreach (JsonElement artistElement in artistsArray.EnumerateArray())
                    {
                        string artistName = artistElement.GetProperty("name").GetString();
                        if (string.Equals(artistName, artist, StringComparison.OrdinalIgnoreCase))
                        {
                            return songElement.GetProperty("id").ToString();
                        }
                    }
                    return songElement.GetProperty("id").ToString();
                }
            }
            return songsArray[0].GetProperty("id").ToString();
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static string SearchForAlbumId(JsonElement root, string album, string artist)
    {
        try
        {
            JsonElement songsArray = root.GetProperty("result").GetProperty("songs");
            foreach (JsonElement songElement in songsArray.EnumerateArray())
            {
                JsonElement albumElement = songElement.GetProperty("album");
                string albumName = albumElement.GetProperty("name").GetString();
                if (string.Equals(albumName, album, StringComparison.OrdinalIgnoreCase))
                {
                    JsonElement artistsArray = songElement.GetProperty("artists");
                    foreach (JsonElement artistElement in artistsArray.EnumerateArray())
                    {
                        string artistName = artistElement.GetProperty("name").GetString();
                        if (string.Equals(artistName, artist, StringComparison.OrdinalIgnoreCase))
                        {
                            return albumElement.GetProperty("id").ToString();
                        }
                    }
                    return albumElement.GetProperty("id").ToString();
                }
            }
            return songsArray[0].GetProperty("album").GetProperty("id").ToString();
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static async Task<string> GetLyricsUrl(string songId, CancellationToken cancellationToken = default)
    {
        if (songId is not null)
        {
            var api = new NeteaseCloudMusicApi();
            var (_, lyricResult) = await api.RequestAsync(
                 CloudMusicApiProviders.Lyric,
                 new Dictionary<string, string> { { "id", $"{songId}" } }
             );
            string lyrics = string.Empty;
            lyrics = (string)lyricResult["lrc"]!["lyric"]!;
            try
            {
                lyrics += (string)lyricResult["tlyric"]!["lyric"]!;
            }
            catch (Exception)
            {
            }
            return lyrics;
        }
        else
        {
            return null;
        }
    }

    private static async Task<string> GetTranslateLyrics(string songId)
    {
        if (songId is not null)
        {
            var api = new NeteaseCloudMusicApi();
            var (_, lyricResult) = await api.RequestAsync(
                 CloudMusicApiProviders.Lyric,
                 new Dictionary<string, string> { { "id", $"{songId}" } }
             );
            try
            {
                return (string)lyricResult["tlyric"]!["lyric"]!;
            }
            catch (Exception)
            {
            }
        }
        return null;
    }

    public static async Task<string> GetAlbumUrl(string albumId)
    {
        if (albumId is not null)
        {
            var api = new NeteaseCloudMusicApi();
            var (_, albumResult) = await api.RequestAsync(
                CloudMusicApiProviders.Album,
                new Dictionary<string, string> { { "id", $"{albumId}" } }
            );
            return (string)albumResult["album"]!["picUrl"]!;
        }
        else
        {
            return null;
        }
    }

}
