using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompSubstructureFootprint : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__11 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompSubstructureFootprint _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__11(int _003C_003E1__state)
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
			CompSubstructureFootprint CS_0024_003C_003E8__locals5 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals5.Props.displaySubstructureOverlayWhenSelected && CS_0024_003C_003E8__locals5.parent.Faction == Faction.OfPlayer)
				{
					_003C_003E2__current = new Command_Toggle
					{
						defaultLabel = "CommandShowSubstructureOverlay".Translate(),
						defaultDesc = "CommandShowSubstructureOverlayDesc".Translate(),
						icon = DisplaySubstructureTex.Texture,
						isActive = () => CS_0024_003C_003E8__locals5.displaySubstructureOverlay,
						toggleAction = delegate
						{
							CS_0024_003C_003E8__locals5.displaySubstructureOverlay = !CS_0024_003C_003E8__locals5.displaySubstructureOverlay;
						}
					};
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__11 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__11(0)
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

	[Unsaved(false)]
	private CompGravshipFacility facilityComp;

	private bool displaySubstructureOverlay = true;

	private static readonly CachedTexture DisplaySubstructureTex = new CachedTexture("UI/Commands/ShowSubstructureOverlay");

	public CompProperties_SubstructureFootprint Props => (CompProperties_SubstructureFootprint)props;

	public bool DisplaySubstructureOverlay
	{
		get
		{
			if (Props.displaySubstructureOverlayWhenSelected)
			{
				return displaySubstructureOverlay;
			}
			return false;
		}
	}

	public bool Valid
	{
		get
		{
			if (facilityComp != null)
			{
				return facilityComp.CanBeActive;
			}
			return true;
		}
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		base.PostSpawnSetup(respawningAfterLoad);
		parent.Map.substructureGrid?.MarkDirty();
		facilityComp = parent.TryGetComp<CompGravshipFacility>();
	}

	public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
	{
		base.PostDeSpawn(map, mode);
		map.substructureGrid?.MarkDirty();
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__11))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__11(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void PostExposeData()
	{
		Scribe_Values.Look(ref displaySubstructureOverlay, "displaySubstructureOverlay", defaultValue: true);
	}
}
