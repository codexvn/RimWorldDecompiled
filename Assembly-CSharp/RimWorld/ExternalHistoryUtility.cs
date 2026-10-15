using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld;

public static class ExternalHistoryUtility
{
	[CompilerGenerated]
	private sealed class _003Cget_Files_003Ed__5 : IEnumerable<FileInfo>, IEnumerable, IEnumerator<FileInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FileInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private int _003Ci_003E5__2;

		FileInfo IEnumerator<FileInfo>.Current
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
		public _003Cget_Files_003Ed__5(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Ci_003E5__2 = 0;
				break;
			case 1:
			{
				_003C_003E1__state = -1;
				int num = _003Ci_003E5__2 + 1;
				_003Ci_003E5__2 = num;
				break;
			}
			}
			if (_003Ci_003E5__2 < cachedFiles.Count)
			{
				_003C_003E2__current = cachedFiles[_003Ci_003E5__2];
				_003C_003E1__state = 1;
				return true;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<FileInfo> IEnumerable<FileInfo>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003Cget_Files_003Ed__5(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FileInfo>)this).GetEnumerator();
		}
	}

	private static List<FileInfo> cachedFiles;

	private static int gameplayIDLength;

	private static string gameplayIDAvailableChars;

	public static IEnumerable<FileInfo> Files
	{
		[IteratorStateMachine(typeof(_003Cget_Files_003Ed__5))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Files_003Ed__5(-2);
		}
	}

	static ExternalHistoryUtility()
	{
		gameplayIDLength = 20;
		gameplayIDAvailableChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
		try
		{
			cachedFiles = GenFilePaths.AllExternalHistoryFiles.ToList();
		}
		catch (Exception ex)
		{
			Log.Error("Could not get external history files: " + ex.Message);
		}
	}

	public static ExternalHistory Load(string path)
	{
		ExternalHistory externalHistory = null;
		try
		{
			externalHistory = new ExternalHistory();
			Scribe.loader.InitLoading(path);
			try
			{
				Scribe_Deep.Look(ref externalHistory, "externalHistory");
				Scribe.loader.FinalizeLoading();
				return externalHistory;
			}
			catch
			{
				Scribe.ForceStop();
				throw;
			}
		}
		catch (Exception ex)
		{
			Log.Error("Could not load external history (" + path + "): " + ex.Message);
			return null;
		}
	}

	public static string GetRandomGameplayID()
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < gameplayIDLength; i++)
		{
			int index = Rand.Range(0, gameplayIDAvailableChars.Length);
			stringBuilder.Append(gameplayIDAvailableChars[index]);
		}
		return stringBuilder.ToString();
	}

	public static bool IsValidGameplayID(string ID)
	{
		if (ID.NullOrEmpty() || ID.Length != gameplayIDLength)
		{
			return false;
		}
		for (int i = 0; i < ID.Length; i++)
		{
			bool flag = false;
			for (int j = 0; j < gameplayIDAvailableChars.Length; j++)
			{
				if (ID[i] == gameplayIDAvailableChars[j])
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
		}
		return true;
	}

	public static string GetCurrentUploadDate()
	{
		return DateTime.UtcNow.ToString("yyMMdd");
	}

	public static int GetCurrentUploadTime()
	{
		return (int)(DateTime.UtcNow.TimeOfDay.TotalSeconds / 2.0);
	}
}
