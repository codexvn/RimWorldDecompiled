using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Waylith.Diagnostics;

public static class ProcessModules
{
	public static List<string> ForeignModuleLines()
	{
		List<string> list = new List<string>();
		string baseDirectory = AppContext.BaseDirectory;
		string[] array = ((!OperatingSystem.IsWindows()) ? ((!OperatingSystem.IsMacOS()) ? new string[3] { "/usr/", "/lib", "/lib64" } : new string[3] { "/System/", "/usr/", "/Library/Apple/" }) : new string[1] { Environment.GetFolderPath(Environment.SpecialFolder.Windows) });
		StringComparison comparisonType = (OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
		using Process process = Process.GetCurrentProcess();
		foreach (ProcessModule module in process.Modules)
		{
			string fileName;
			try
			{
				fileName = module.FileName;
			}
			catch
			{
				continue;
			}
			if (string.IsNullOrEmpty(fileName) || fileName.StartsWith(baseDirectory, comparisonType))
			{
				continue;
			}
			bool flag = false;
			string[] array2 = array;
			foreach (string value in array2)
			{
				flag |= !string.IsNullOrEmpty(value) && fileName.StartsWith(value, comparisonType);
			}
			if (!flag)
			{
				string text = "";
				try
				{
					text = module.FileVersionInfo.FileVersion ?? "";
				}
				catch
				{
				}
				list.Add(string.IsNullOrEmpty(text) ? fileName : (fileName + " (" + text + ")"));
			}
		}
		list.Sort(StringComparer.OrdinalIgnoreCase);
		return list;
	}
}
