using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public static class Utils
{
    public static List<T> GetResources<T>(string path) => Resources.LoadAll(path, typeof(T)).Cast<T>().ToList();

    public static T GetResource<T>(string path)
    {
        try
        {
            return GetResources<T>(path)[0];
        }
        catch (IndexOutOfRangeException)
        {
            throw new IndexOutOfRangeException($"Resource not found: {path}");
        }
    }

    public static string NewId() => Guid.NewGuid().ToString()[..8];

    public static string ToBase64(string data, bool reverse = false) => !reverse
        ? Convert.ToBase64String(Encoding.UTF8.GetBytes(data))
        : Encoding.UTF8.GetString(Convert.FromBase64String(data));


    public static string Humanize(string text)
    {
        return new CultureInfo("en-US", false).TextInfo.ToTitleCase(
            Regex.Replace(Regex.Replace(text, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"),
                @"(\p{Ll})(\P{Ll})", "$1 $2").Replace('_', ' '));
    }

    public static string NowIsoDate()
    {
        var localTime = DateTime.Now;
        var localTimeAndOffset = new DateTimeOffset(localTime, TimeZoneInfo.Local.GetUtcOffset(localTime));
        var str = localTimeAndOffset.ToString("O");
        return str[..26] + str[27..];
    }
}