using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompMechPowerCell : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__11 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompMechPowerCell _003C_003E4__this;

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
			CompMechPowerCell CS_0024_003C_003E8__locals10 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals10.parent.Faction != Faction.OfPlayer && !CS_0024_003C_003E8__locals10.Props.showGizmoOnNonPlayerControlled)
				{
					return false;
				}
				if (Find.Selector.SingleSelectedThing == CS_0024_003C_003E8__locals10.parent)
				{
					if (CS_0024_003C_003E8__locals10.gizmo == null)
					{
						CS_0024_003C_003E8__locals10.gizmo = new MechPowerCellGizmo(CS_0024_003C_003E8__locals10)
						{
							Order = -100f
						};
					}
					_003C_003E2__current = CS_0024_003C_003E8__locals10.gizmo;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_009b;
			case 1:
				_003C_003E1__state = -1;
				goto IL_009b;
			case 2:
			{
				_003C_003E1__state = -1;
				Command_Action command_Action = new Command_Action
				{
					defaultLabel = "DEV: Power left 100%",
					action = delegate
					{
						CS_0024_003C_003E8__locals10.powerTicksLeft = CS_0024_003C_003E8__locals10.Props.totalPowerTicks;
					}
				};
				_003C_003E2__current = command_Action;
				_003C_003E1__state = 3;
				return true;
			}
			case 3:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_009b:
				if (DebugSettings.ShowDevGizmos)
				{
					Command_Action command_Action2 = new Command_Action
					{
						defaultLabel = "DEV: Power left 0%",
						action = delegate
						{
							CS_0024_003C_003E8__locals10.powerTicksLeft = 0;
						}
					};
					_003C_003E2__current = command_Action2;
					_003C_003E1__state = 2;
					return true;
				}
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

	private int powerTicksLeft;

	public bool depleted;

	private MechPowerCellGizmo gizmo;

	public CompProperties_MechPowerCell Props => (CompProperties_MechPowerCell)props;

	public float PercentFull => (float)powerTicksLeft / (float)Props.totalPowerTicks;

	public int PowerTicksLeft => powerTicksLeft;

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		if (!ModLister.CheckBiotechOrAnomalyOrOdyssey("Mech power cell"))
		{
			parent.Destroy();
			return;
		}
		base.PostSpawnSetup(respawningAfterLoad);
		if (!respawningAfterLoad && !parent.BeingTransportedOnGravship)
		{
			powerTicksLeft = Props.totalPowerTicks;
		}
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

	public override void CompTick()
	{
		base.CompTick();
		if (depleted)
		{
			return;
		}
		powerTicksLeft--;
		if (powerTicksLeft <= 0)
		{
			if (Props.killWhenDepleted)
			{
				KillPowerProcessor();
				return;
			}
			powerTicksLeft = 0;
			depleted = true;
		}
	}

	private void KillPowerProcessor()
	{
		Pawn pawn = (Pawn)parent;
		List<BodyPartRecord> allParts = pawn.def.race.body.AllParts;
		for (int i = 0; i < allParts.Count; i++)
		{
			BodyPartRecord bodyPartRecord = allParts[i];
			if (bodyPartRecord.def.tags.Contains(BodyPartTagDefOf.BloodPumpingSource))
			{
				pawn.health.AddHediff(HediffDefOf.MissingBodyPart, bodyPartRecord, null);
			}
		}
		if (!pawn.Dead)
		{
			pawn.Kill(null);
		}
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref powerTicksLeft, "powerTicksLeft", 0);
		Scribe_Values.Look(ref depleted, "depleted", defaultValue: false);
	}
}
