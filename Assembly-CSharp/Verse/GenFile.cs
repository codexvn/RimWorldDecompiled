using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse;

public static class GenFile
{
	[CompilerGenerated]
	private sealed class _003CLinesFromFile_003Ed__3 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private string filePath;

		public string _003C_003E3__filePath;

		private IEnumerator<string> _003C_003E7__wrap1;

		string IEnumerator<string>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CLinesFromFile_003Ed__3(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					string text = TextFromResourceFile(filePath);
					_003C_003E7__wrap1 = GenText.LinesFromString(text).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					string current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CLinesFromFile_003Ed__3 _003CLinesFromFile_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CLinesFromFile_003Ed__ = this;
			}
			else
			{
				_003CLinesFromFile_003Ed__ = new _003CLinesFromFile_003Ed__3(0);
			}
			_003CLinesFromFile_003Ed__.filePath = _003C_003E3__filePath;
			return _003CLinesFromFile_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public static string TextFromRawFile(string filePath)
	{
		return File.ReadAllText(filePath);
	}

	public static string TextFromResourceFile(string filePath)
	{
		TextAsset textAsset = Resources.Load<TextAsset>("Text/" + filePath);
		if (textAsset == null)
		{
			Log.Message("Found no text asset in resources at " + filePath);
			return null;
		}
		return GetTextWithoutBOM(textAsset);
	}

	public static string GetTextWithoutBOM(TextAsset textAsset)
	{
		string text = null;
		using MemoryStream stream = new MemoryStream(textAsset.bytes);
		using StreamReader streamReader = new StreamReader(stream, detectEncodingFromByteOrderMarks: true);
		return streamReader.ReadToEnd();
	}

	[IteratorStateMachine(typeof(_003CLinesFromFile_003Ed__3))]
	public static IEnumerable<string> LinesFromFile(string filePath)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CLinesFromFile_003Ed__3(-2)
		{
			_003C_003E3__filePath = filePath
		};
	}

	public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, bool useLinuxLineEndings = false, Func<string, string> destFileNameGetter = null)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirName);
		DirectoryInfo[] directories = directoryInfo.GetDirectories();
		if (!directoryInfo.Exists)
		{
			throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
		}
		if (!Directory.Exists(destDirName))
		{
			Directory.CreateDirectory(destDirName);
		}
		FileInfo[] files = directoryInfo.GetFiles();
		foreach (FileInfo fileInfo in files)
		{
			string text = fileInfo.Name;
			if (destFileNameGetter != null)
			{
				text = destFileNameGetter(text);
			}
			string text2 = Path.Combine(destDirName, text);
			if (useLinuxLineEndings && (fileInfo.Extension == ".sh" || fileInfo.Extension == ".txt"))
			{
				if (!File.Exists(text2))
				{
					File.WriteAllText(text2, File.ReadAllText(fileInfo.FullName).Replace("\r\n", "\n").Replace("\r", "\n"));
				}
			}
			else
			{
				fileInfo.CopyTo(text2, overwrite: false);
			}
		}
		if (copySubDirs)
		{
			DirectoryInfo[] array = directories;
			foreach (DirectoryInfo directoryInfo2 in array)
			{
				string destDirName2 = Path.Combine(destDirName, directoryInfo2.Name);
				DirectoryCopy(directoryInfo2.FullName, destDirName2, copySubDirs, useLinuxLineEndings);
			}
		}
	}

	public static string SanitizedFileName(string fileName)
	{
		char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
		string text = "";
		for (int i = 0; i < fileName.Length; i++)
		{
			if (!invalidFileNameChars.Contains(fileName[i]))
			{
				text += fileName[i];
			}
		}
		if (text.Length == 0)
		{
			text = "unnamed";
		}
		return text;
	}

	public static string ResolveCaseInsensitiveFilePath(string dir, string targetFileName)
	{
		string text = Path.Combine(dir, targetFileName);
		if (File.Exists(text))
		{
			return text;
		}
		if (Directory.Exists(dir))
		{
			string[] files = Directory.GetFiles(dir);
			foreach (string path in files)
			{
				if (string.Compare(Path.GetFileName(path), targetFileName, StringComparison.CurrentCultureIgnoreCase) == 0)
				{
					return dir + Path.DirectorySeparatorChar + Path.GetFileName(path);
				}
			}
		}
		return dir + Path.DirectorySeparatorChar + targetFileName;
	}
}
