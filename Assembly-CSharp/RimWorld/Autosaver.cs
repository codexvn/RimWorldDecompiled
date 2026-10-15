using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Profile;

namespace RimWorld;

public sealed class Autosaver
{
	[CompilerGenerated]
	private sealed class _003CAutoSaveNames_003Ed__10 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private int _003Ci_003E5__2;

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
		public _003CAutoSaveNames_003Ed__10(int _003C_003E1__state)
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
				_003Ci_003E5__2 = 1;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				break;
			}
			if (_003Ci_003E5__2 <= Prefs.AutosavesCount)
			{
				_003C_003E2__current = "Autosave-" + _003Ci_003E5__2;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CAutoSaveNames_003Ed__10(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	private int ticksSinceSave;

	public const float MaxPermadeathModeAutosaveInterval = 1f;

	private float AutosaveIntervalDays
	{
		get
		{
			float num = Prefs.AutosaveIntervalDays;
			if (Current.Game.Info.permadeathMode && num > 1f)
			{
				num = 1f;
			}
			return num;
		}
	}

	private int AutosaveIntervalTicks => Mathf.RoundToInt(AutosaveIntervalDays * 60000f);

	public void AutosaverTick()
	{
		ticksSinceSave++;
		if (ticksSinceSave >= AutosaveIntervalTicks && !GameDataSaveLoader.SavingIsTemporarilyDisabled)
		{
			LongEventHandler.QueueLongEvent(DoAutosave, "Autosaving", doAsynchronously: false, null);
			ticksSinceSave = 0;
		}
	}

	public void DoAutosave()
	{
		string fileName = ((!Current.Game.Info.permadeathMode) ? NewAutosaveFileName() : Current.Game.Info.permadeathModeUniqueName);
		GameDataSaveLoader.SaveGame(fileName);
	}

	private void DoMemoryCleanup()
	{
		MemoryUtility.UnloadUnusedUnityAssets();
	}

	private string NewAutosaveFileName()
	{
		string text = AutoSaveNames().FirstOrDefault((string name) => !SaveGameFilesUtility.SavedGameNamedExists(name));
		if (text != null)
		{
			return text;
		}
		return AutoSaveNames().MinBy((string name) => new FileInfo(GenFilePaths.FilePathForSavedGame(name)).LastWriteTime);
	}

	[IteratorStateMachine(typeof(_003CAutoSaveNames_003Ed__10))]
	private IEnumerable<string> AutoSaveNames()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAutoSaveNames_003Ed__10(-2);
	}
}
