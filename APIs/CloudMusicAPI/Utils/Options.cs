#pragma warning disable

using System.Net;
using LyricsWeb.APIs.CloudMusicAPI.Utils;

namespace LyricsWeb.APIs.CloudMusicAPI.Utils;

internal sealed class Options
{
    public string crypto;
    public CookieCollection cookie;
    public string ua;
    public string url;
}
