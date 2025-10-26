using System;
using System.Collections.Generic;
using ZLinq;

namespace LyricsWeb.APIs.CloudMusicAPI.Extensions;

internal static class HttpExtensions
{
    public static string ToQueryString(this IEnumerable<KeyValuePair<string, string>> queries)
    {
        ArgumentNullException.ThrowIfNull(queries);

        return string.Join(
            "&",
            queries.AsValueEnumerable().Select(t => Uri.EscapeDataString(t.Key) + "=" + Uri.EscapeDataString(t.Value)).ToArray()
        );
    }
}
