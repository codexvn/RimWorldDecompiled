using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public static class StorageSettingsClipboard
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass6_0
	{
		public StorageSettings s;

		internal void _003CCopyPasteGizmosFor_003Eb__0()
		{
			SoundDefOf.Tick_High.PlayOneShotOnCamera();
			Copy(s);
		}

		internal void _003CCopyPasteGizmosFor_003Eb__1()
		{
			SoundDefOf.Tick_High.PlayOneShotOnCamera();
			PasteInto(s);
		}
	}

	[CompilerGenerated]
	private sealed class _003CCopyPasteGizmosFor_003Ed__6 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private StorageSettings s;

		public StorageSettings _003C_003E3__s;

		private _003C_003Ec__DisplayClass6_0 _003C_003E8__1;

		Gizmo IEnumerator<Gizmo>.Current
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
		public _003CCopyPasteGizmosFor_003Ed__6(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
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
				_003C_003E8__1 = new _003C_003Ec__DisplayClass6_0();
				_003C_003E8__1.s = s;
				_003C_003E2__current = new Command_Action
				{
					icon = ContentFinder<Texture2D>.Get("UI/Commands/CopySettings"),
					defaultLabel = "CommandCopyZoneSettingsLabel".Translate(),
					defaultDesc = "CommandCopyZoneSettingsDesc".Translate(),
					action = delegate
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera();
						Copy(_003C_003E8__1.s);
					},
					hotKey = KeyBindingDefOf.Misc4
				};
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Command_Action command_Action = new Command_Action
				{
					icon = ContentFinder<Texture2D>.Get("UI/Commands/PasteSettings"),
					defaultLabel = "CommandPasteZoneSettingsLabel".Translate(),
					defaultDesc = "CommandPasteZoneSettingsDesc".Translate(),
					action = delegate
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera();
						PasteInto(_003C_003E8__1.s);
					},
					hotKey = KeyBindingDefOf.Misc5
				};
				if (!HasCopiedSettings)
				{
					command_Action.Disable();
				}
				_003C_003E2__current = command_Action;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
				_003C_003E1__state = -1;
				return false;
			}
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCopyPasteGizmosFor_003Ed__6 _003CCopyPasteGizmosFor_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCopyPasteGizmosFor_003Ed__ = this;
			}
			else
			{
				_003CCopyPasteGizmosFor_003Ed__ = new _003CCopyPasteGizmosFor_003Ed__6(0);
			}
			_003CCopyPasteGizmosFor_003Ed__.s = _003C_003E3__s;
			return _003CCopyPasteGizmosFor_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	private static StorageSettings clipboard = new StorageSettings();

	private static bool copied = false;

	public static bool HasCopiedSettings => copied;

	public static void Copy(StorageSettings s)
	{
		clipboard.CopyFrom(s);
		copied = true;
		Messages.Message("StorageSettingsCopiedToClipboard".Translate(), null, MessageTypeDefOf.NeutralEvent, historical: false);
	}

	public static void PasteInto(StorageSettings s)
	{
		s.CopyFrom(clipboard);
		Messages.Message("StorageSettingsPastedFromClipboard".Translate(), null, MessageTypeDefOf.NeutralEvent, historical: false);
	}

	[IteratorStateMachine(typeof(_003CCopyPasteGizmosFor_003Ed__6))]
	public static IEnumerable<Gizmo> CopyPasteGizmosFor(StorageSettings s)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCopyPasteGizmosFor_003Ed__6(-2)
		{
			_003C_003E3__s = s
		};
	}
}
