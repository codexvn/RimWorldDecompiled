using System.Text.Json;

namespace UnityEngine;

public static class JsonUtility
{
	private static readonly JsonSerializerOptions _defaultOptions = new JsonSerializerOptions
	{
		WriteIndented = false,
		IncludeFields = true
	};

	private static readonly JsonSerializerOptions _prettyOptions = new JsonSerializerOptions
	{
		WriteIndented = true,
		IncludeFields = true
	};

	public static T0 FromJson<T0>(string json)
	{
		return JsonSerializer.Deserialize<T0>(json, _defaultOptions);
	}

	public static string ToJson(object obj, bool prettyPrint)
	{
		return JsonSerializer.Serialize(obj, prettyPrint ? _prettyOptions : _defaultOptions);
	}

	public static string ToJson(object obj)
	{
		return ToJson(obj, prettyPrint: false);
	}
}
