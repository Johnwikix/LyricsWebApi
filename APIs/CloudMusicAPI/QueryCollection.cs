using System.Collections.Generic;

namespace LyricsWeb.APIs.CloudMusicAPI;

internal sealed partial class QueryCollection : List<KeyValuePair<string, string>>
{
    public void Add(string key, string value) => Add(new KeyValuePair<string, string>(key, value));
}
