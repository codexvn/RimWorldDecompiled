using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompGatherSpot : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__7 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompGatherSpot _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__7(int _003C_003E1__state)
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
			CompGatherSpot CS_0024_003C_003E8__locals4 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				Command_Toggle command_Toggle = new Command_Toggle
				{
					hotKey = KeyBindingDefOf.Command_TogglePower,
					defaultLabel = "CommandGatherSpotToggleLabel".Translate(),
					icon = TexCommand.GatherSpotActive,
					isActive = () => CS_0024_003C_003E8__locals4.Active,
					toggleAction = delegate
					{
						CS_0024_003C_003E8__locals4.Active = !CS_0024_003C_003E8__locals4.Active;
					}
				};
				if (CS_0024_003C_003E8__locals4.Active)
				{
					command_Toggle.defaultDesc = "CommandGatherSpotToggleDescActive".Translate();
				}
				else
				{
					command_Toggle.defaultDesc = "CommandGatherSpotToggleDescInactive".Translate();
				}
				_003C_003E2__current = command_Toggle;
				_003C_003E1__state = 1;
				return true;
			}
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
			_003CCompGetGizmosExtra_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__7(0)
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

	private bool active = true;

	public bool Active
	{
		get
		{
			return active;
		}
		set
		{
			if (value == active)
			{
				return;
			}
			active = value;
			if (parent.Spawned)
			{
				if (active)
				{
					parent.Map.gatherSpotLister.RegisterActivated(this);
				}
				else
				{
					parent.Map.gatherSpotLister.RegisterDeactivated(this);
				}
			}
		}
	}

	public override void PostExposeData()
	{
		Scribe_Values.Look(ref active, "active", defaultValue: false);
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		base.PostSpawnSetup(respawningAfterLoad);
		if (parent.Faction != Faction.OfPlayer && !respawningAfterLoad)
		{
			active = false;
		}
		if (Active)
		{
			parent.Map.gatherSpotLister.RegisterActivated(this);
		}
	}

	public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
	{
		base.PostDeSpawn(map, mode);
		if (Active)
		{
			map.gatherSpotLister.RegisterDeactivated(this);
		}
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__7))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__7(-2)
		{
			_003C_003E4__this = this
		};
	}
}
