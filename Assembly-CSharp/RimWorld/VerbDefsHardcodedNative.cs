using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public static class VerbDefsHardcodedNative
{
	[CompilerGenerated]
	private sealed class _003CAllVerbDefs_003Ed__0 : IEnumerable<VerbProperties>, IEnumerable, IEnumerator<VerbProperties>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private VerbProperties _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		VerbProperties IEnumerator<VerbProperties>.Current
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
		public _003CAllVerbDefs_003Ed__0(int _003C_003E1__state)
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
				_003C_003E2__current = new VerbProperties
				{
					verbClass = typeof(Verb_BeatFire),
					category = VerbCategory.BeatFire,
					range = 1.42f,
					noiseRadius = 3f,
					targetParams = 
					{
						canTargetFires = true,
						canTargetPawns = false,
						canTargetBuildings = false,
						mapObjectTargetsMustBeAutoAttackable = false
					},
					warmupTime = 0f,
					defaultCooldownTime = 1.1f,
					soundCast = SoundDefOf.Interact_BeatFire
				};
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = new VerbProperties
				{
					verbClass = typeof(Verb_Ignite),
					category = VerbCategory.Ignite,
					range = 1.42f,
					noiseRadius = 3f,
					targetParams = 
					{
						onlyTargetFlammables = true,
						canTargetBuildings = true,
						canTargetPawns = false,
						mapObjectTargetsMustBeAutoAttackable = false
					},
					warmupTime = 3f,
					defaultCooldownTime = 1.3f,
					soundCast = SoundDefOf.Interact_Ignite
				};
				_003C_003E1__state = 2;
				return true;
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
		IEnumerator<VerbProperties> IEnumerable<VerbProperties>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CAllVerbDefs_003Ed__0(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<VerbProperties>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CAllVerbDefs_003Ed__0))]
	public static IEnumerable<VerbProperties> AllVerbDefs()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllVerbDefs_003Ed__0(-2);
	}
}
