using System;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using Verse;

namespace RimWorldWaylith;

internal static class EarlyPrefs
{
	public static string? Read(string element)
	{
		try
		{
			string text = PrefsFilePath();
			return (!File.Exists(text)) ? null : XDocument.Load(text).Root?.Element(element)?.Value;
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Could not read " + element + " from prefs: " + ex.Message);
			return null;
		}
	}

	public static string PrefsFilePath()
	{
		string path;
		if (GenCommandLine.TryGetCommandLineArg("savedatafolder", out var value))
		{
			path = ((value.Length == 0) ? Path.DirectorySeparatorChar.ToString() : value);
		}
		else
		{
			string text = Bootstrap.ResolvePersistentDataRoot();
			path = (OperatingSystem.IsMacOS() ? Path.Combine(Directory.GetParent(text).FullName, "RimWorld") : text);
		}
		return Path.Combine(path, "Config", "Prefs.xml");
	}
}
