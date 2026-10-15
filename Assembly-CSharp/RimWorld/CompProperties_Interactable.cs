using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompProperties_Interactable : CompProperties
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__30 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompProperties_Interactable _003C_003E4__this;

		private ThingDef parentDef;

		public ThingDef _003C_003E3__parentDef;

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
		public _003CConfigErrors_003Ed__30(int _003C_003E1__state)
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
				int num = _003C_003E1__state;
				CompProperties_Interactable compProperties_Interactable = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((CompProperties)compProperties_Interactable).ConfigErrors(parentDef).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007c;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007c;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00e8;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_007c:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (compProperties_Interactable.compClass != null && !typeof(CompInteractable).IsAssignableFrom(compProperties_Interactable.compClass))
					{
						_003C_003E2__current = parentDef.defName + " has compClass but is not subclass of CompInteractable.";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00e8;
					IL_00e8:
					if (compProperties_Interactable.activateTexPath.NullOrEmpty())
					{
						_003C_003E2__current = parentDef.defName + " has no activate texture.";
						_003C_003E1__state = 3;
						return true;
					}
					break;
				}
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
			_003CConfigErrors_003Ed__30 _003CConfigErrors_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CConfigErrors_003Ed__ = this;
			}
			else
			{
				_003CConfigErrors_003Ed__ = new _003CConfigErrors_003Ed__30(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CConfigErrors_003Ed__.parentDef = _003C_003E3__parentDef;
			return _003CConfigErrors_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public int cooldownTicks;

	public int activeTicks;

	public int ticksToActivate;

	public int cooldownFleckSpawnIntervalTicks;

	public float cooldownFleckScale = 1f;

	public bool cooldownPreventsRefuel;

	public bool maintainProgress;

	public bool showMustBeActivatedByColonist = true;

	public bool forceNormalSpeedOnInteracting;

	public bool remainingSecondsInInspectString;

	public bool ignoreForbidden;

	public bool requiresPower;

	public TargetingParameters targetingParameters;

	public StatDef activateStat;

	public FleckDef fleckOnUsed;

	public float fleckOnUsedScale;

	public FleckDef cooldownFleck;

	public SoundDef soundActivate;

	public EffecterDef interactionEffecter;

	[MustTranslate]
	public string jobString;

	[MustTranslate]
	public string activatingStringPending;

	[MustTranslate]
	public string activatingString;

	[MustTranslate]
	public string activateLabelString;

	[MustTranslate]
	public string activateDescString;

	[MustTranslate]
	public string guiLabelString;

	[MustTranslate]
	public string onCooldownString;

	[MustTranslate]
	public string inspectString;

	[MustTranslate]
	public string messageCompletedString;

	[MustTranslate]
	public string messageCooldownEnded;

	[NoTranslate]
	public string activateTexPath;

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__30))]
	public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__30(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parentDef = parentDef
		};
	}
}
