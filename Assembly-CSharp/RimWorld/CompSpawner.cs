using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompSpawner : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__14 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompSpawner _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__14(int _003C_003E1__state)
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
			CompSpawner CS_0024_003C_003E8__locals3 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (DebugSettings.ShowDevGizmos)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Spawn " + CS_0024_003C_003E8__locals3.PropsSpawner.thingToSpawn.label,
						icon = TexCommand.DesirePower,
						action = delegate
						{
							CS_0024_003C_003E8__locals3.ResetCountdown();
							CS_0024_003C_003E8__locals3.TryDoSpawn();
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
			_003CCompGetGizmosExtra_003Ed__14 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__14(0)
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

	private int ticksUntilSpawn;

	public CompProperties_Spawner PropsSpawner => (CompProperties_Spawner)props;

	private bool PowerOn => parent.GetComp<CompPowerTrader>()?.PowerOn ?? false;

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		if (!respawningAfterLoad && !parent.BeingTransportedOnGravship)
		{
			ResetCountdown();
		}
	}

	public override void CompTick()
	{
		TickIntervalDelta(1);
	}

	public override void CompTickRare()
	{
		TickIntervalDelta(250);
	}

	private void TickIntervalDelta(int interval)
	{
		if (!parent.Spawned)
		{
			return;
		}
		CompCanBeDormant comp = parent.GetComp<CompCanBeDormant>();
		if (comp != null)
		{
			if (!comp.Awake)
			{
				return;
			}
		}
		else if (parent.Position.Fogged(parent.Map))
		{
			return;
		}
		if (!PropsSpawner.requiresPower || PowerOn)
		{
			ticksUntilSpawn -= interval;
			CheckShouldSpawn();
		}
	}

	private void CheckShouldSpawn()
	{
		if (ticksUntilSpawn <= 0)
		{
			ResetCountdown();
			TryDoSpawn();
		}
	}

	public bool TryDoSpawn()
	{
		if (!parent.Spawned)
		{
			return false;
		}
		if (PropsSpawner.spawnMaxAdjacent >= 0)
		{
			int num = 0;
			for (int i = 0; i < 9; i++)
			{
				IntVec3 c = parent.Position + GenAdj.AdjacentCellsAndInside[i];
				if (!c.InBounds(parent.Map))
				{
					continue;
				}
				List<Thing> thingList = c.GetThingList(parent.Map);
				for (int j = 0; j < thingList.Count; j++)
				{
					if (thingList[j].def == PropsSpawner.thingToSpawn)
					{
						num += thingList[j].stackCount;
						if (num >= PropsSpawner.spawnMaxAdjacent)
						{
							return false;
						}
					}
				}
			}
		}
		if (TryFindSpawnCell(parent, PropsSpawner.thingToSpawn, PropsSpawner.spawnCount, out var result))
		{
			Thing thing = ThingMaker.MakeThing(PropsSpawner.thingToSpawn);
			thing.stackCount = PropsSpawner.spawnCount;
			if (PropsSpawner.inheritFaction && thing.Faction != parent.Faction)
			{
				thing.SetFaction(parent.Faction);
			}
			if (!GenPlace.TryPlaceThing(thing, result, parent.Map, ThingPlaceMode.Direct, out var lastResultingThing, null, null, null))
			{
				return false;
			}
			if (PropsSpawner.spawnForbidden)
			{
				lastResultingThing.SetForbidden(value: true);
			}
			if (PropsSpawner.showMessageIfOwned && parent.Faction == Faction.OfPlayer)
			{
				Messages.Message("MessageCompSpawnerSpawnedItem".Translate(PropsSpawner.thingToSpawn.LabelCap), thing, MessageTypeDefOf.PositiveEvent);
			}
			return true;
		}
		return false;
	}

	public static bool TryFindSpawnCell(Thing parent, ThingDef thingToSpawn, int spawnCount, out IntVec3 result)
	{
		foreach (IntVec3 item in GenAdj.CellsAdjacent8Way(parent).InRandomOrder())
		{
			if (!item.Walkable(parent.Map))
			{
				continue;
			}
			Building edifice = item.GetEdifice(parent.Map);
			if ((edifice != null && (thingToSpawn.IsEdifice() || edifice is IHaulDestination || edifice is Building_Door { FreePassage: false })) || (parent.def.passability != Traversability.Impassable && !GenSight.LineOfSight(parent.Position, item, parent.Map)))
			{
				continue;
			}
			bool flag = false;
			List<Thing> thingList = item.GetThingList(parent.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (thing.def.category == ThingCategory.Item && (thing.def != thingToSpawn || thing.stackCount > thingToSpawn.stackLimit - spawnCount))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				result = item;
				return true;
			}
		}
		result = IntVec3.Invalid;
		return false;
	}

	private void ResetCountdown()
	{
		ticksUntilSpawn = PropsSpawner.spawnIntervalRange.RandomInRange;
	}

	public override void PostExposeData()
	{
		string text = (PropsSpawner.saveKeysPrefix.NullOrEmpty() ? null : (PropsSpawner.saveKeysPrefix + "_"));
		Scribe_Values.Look(ref ticksUntilSpawn, text + "ticksUntilSpawn", 0);
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__14))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__14(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string CompInspectStringExtra()
	{
		if (PropsSpawner.writeTimeLeftToSpawn && (!PropsSpawner.requiresPower || PowerOn))
		{
			return "NextSpawnedItemIn".Translate(GenLabel.ThingLabel(PropsSpawner.thingToSpawn, null, PropsSpawner.spawnCount)).Resolve() + ": " + ticksUntilSpawn.ToStringTicksToPeriod().Colorize(ColoredText.DateTimeColor);
		}
		return null;
	}
}
