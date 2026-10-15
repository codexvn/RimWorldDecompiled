using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

[StaticConstructorOnStartup]
public class CompPlantPreventCutting : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__8 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompPlantPreventCutting _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__8(int _003C_003E1__state)
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
			int num = _003C_003E1__state;
			CompPlantPreventCutting CS_0024_003C_003E8__locals3 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Toggle
				{
					defaultLabel = "PreventCutting".Translate(),
					defaultDesc = "PreventCuttingDesc".Translate(),
					toggleAction = delegate
					{
						CS_0024_003C_003E8__locals3.PreventCutting = !CS_0024_003C_003E8__locals3.preventCutting;
					},
					isActive = () => CS_0024_003C_003E8__locals3.preventCutting,
					icon = IconTexture
				};
				_003C_003E1__state = 1;
				return true;
			case 1:
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
			_003CCompGetGizmosExtra_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__8(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	private bool preventCutting = true;

	private static readonly Texture2D IconTexture = ContentFinder<Texture2D>.Get("UI/Designators/PreventCutting");

	public CompProperties_PlantPreventCutting Props => (CompProperties_PlantPreventCutting)props;

	public bool PreventCutting
	{
		get
		{
			return preventCutting;
		}
		set
		{
			preventCutting = value;
			if (preventCutting)
			{
				parent.Map.designationManager.TryRemoveDesignationOn(parent, DesignationDefOf.CutPlant);
				parent.Map.designationManager.TryRemoveDesignationOn(parent, DesignationDefOf.HarvestPlant);
			}
		}
	}

	public override void PostExposeData()
	{
		Scribe_Values.Look(ref preventCutting, "canCut", defaultValue: false);
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__8))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__8(-2)
		{
			_003C_003E4__this = this
		};
	}
}
