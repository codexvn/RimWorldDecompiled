using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompSpawnerHives : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__16 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompSpawnerHives _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__16(int _003C_003E1__state)
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
			CompSpawnerHives CS_0024_003C_003E8__locals1 = _003C_003E4__this;
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
						defaultLabel = "DEV: Reproduce",
						icon = TexCommand.GatherSpotActive,
						action = delegate
						{
							CS_0024_003C_003E8__locals1.TrySpawnChildHive(ignoreRoofedRequirement: false, out var _);
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
			_003CCompGetGizmosExtra_003Ed__16 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__16(0)
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

	private int nextHiveSpawnTick = -1;

	public bool canSpawnHives = true;

	private bool wasActivated;

	public const int MaxHivesPerMap = 30;

	private CompProperties_SpawnerHives Props => (CompProperties_SpawnerHives)props;

	private bool CanSpawnChildHive
	{
		get
		{
			if (canSpawnHives && HiveUtility.TotalSpawnedHivesCount(parent.Map) < 30)
			{
				return Find.Storyteller.difficulty.enemyReproductionRateFactor > 0f;
			}
			return false;
		}
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		if (!respawningAfterLoad)
		{
			CalculateNextHiveSpawnTick();
		}
	}

	public override void CompTick()
	{
		base.CompTick();
		CompCanBeDormant comp = parent.GetComp<CompCanBeDormant>();
		if ((comp == null || comp.Awake) && !wasActivated)
		{
			CalculateNextHiveSpawnTick();
			wasActivated = true;
		}
		if ((comp == null || comp.Awake) && Find.TickManager.TicksGame >= nextHiveSpawnTick)
		{
			if (TrySpawnChildHive(ignoreRoofedRequirement: false, out var newHive))
			{
				Messages.Message("MessageHiveReproduced".Translate(), newHive, MessageTypeDefOf.NegativeEvent);
			}
			else
			{
				CalculateNextHiveSpawnTick();
			}
		}
	}

	public override string CompInspectStringExtra()
	{
		if (!canSpawnHives || Find.Storyteller.difficulty.enemyReproductionRateFactor <= 0f)
		{
			return "DormantHiveNotReproducing".Translate();
		}
		if (CanSpawnChildHive)
		{
			return "HiveReproducesIn".Translate() + ": " + (nextHiveSpawnTick - Find.TickManager.TicksGame).ToStringTicksToPeriod();
		}
		return null;
	}

	public void CalculateNextHiveSpawnTick()
	{
		Room room = parent.GetRoom();
		int num = 0;
		int num2 = GenRadial.NumCellsInRadius(9f);
		for (int i = 0; i < num2; i++)
		{
			IntVec3 intVec = parent.Position + GenRadial.RadialPattern[i];
			if (intVec.InBounds(parent.Map) && intVec.GetRoom(parent.Map) == room && intVec.GetThingList(parent.Map).Any((Thing t) => t is Hive))
			{
				num++;
			}
		}
		float num3 = Props.ReproduceRateFactorFromNearbyHiveCountCurve.Evaluate(num);
		if (Find.Storyteller.difficulty.enemyReproductionRateFactor > 0f)
		{
			nextHiveSpawnTick = Find.TickManager.TicksGame + (int)(Props.HiveSpawnIntervalDays.RandomInRange * 60000f / (num3 * Find.Storyteller.difficulty.enemyReproductionRateFactor));
		}
		else
		{
			nextHiveSpawnTick = Find.TickManager.TicksGame + (int)Props.HiveSpawnIntervalDays.RandomInRange * 60000;
		}
	}

	public bool TrySpawnChildHive(bool ignoreRoofedRequirement, out Hive newHive)
	{
		IntVec3 loc = FindChildHiveLocation(parent.Position, parent.Map, parent.def, Props, ignoreRoofedRequirement, allowUnreachable: false);
		return TrySpawnChildHive(loc, out newHive);
	}

	public bool TrySpawnChildHive(IntVec3 loc, out Hive newHive)
	{
		if (!CanSpawnChildHive)
		{
			newHive = null;
			return false;
		}
		if (!loc.IsValid)
		{
			newHive = null;
			return false;
		}
		newHive = (Hive)ThingMaker.MakeThing(parent.def);
		if (newHive.Faction != parent.Faction)
		{
			newHive.SetFaction(parent.Faction);
		}
		if (parent is Hive hive)
		{
			if (hive.CompDormant.Awake)
			{
				newHive.CompDormant.WakeUp();
			}
			newHive.questTags = hive.questTags;
		}
		GenSpawn.Spawn(newHive, loc, parent.Map, WipeMode.FullRefund);
		CalculateNextHiveSpawnTick();
		return true;
	}

	public static IntVec3 FindChildHiveLocation(IntVec3 pos, Map map, ThingDef parentDef, CompProperties_SpawnerHives props, bool ignoreRoofedRequirement, bool allowUnreachable)
	{
		IntVec3 result = IntVec3.Invalid;
		for (int i = 0; i < 3; i++)
		{
			float minDist = props.HiveSpawnPreferredMinDist;
			bool flag;
			if (i >= 2)
			{
				flag = allowUnreachable && CellFinder.TryFindRandomCellNear(pos, map, (int)props.HiveSpawnRadius, (IntVec3 c) => CanSpawnHiveAt(c, map, pos, parentDef, minDist, ignoreRoofedRequirement), out result);
			}
			else
			{
				if (i == 1)
				{
					minDist = 0f;
				}
				flag = CellFinder.TryFindRandomReachableNearbyCell(pos, map, props.HiveSpawnRadius, TraverseParms.For(TraverseMode.NoPassClosedDoors), (IntVec3 c) => CanSpawnHiveAt(c, map, pos, parentDef, minDist, ignoreRoofedRequirement), null, out result);
			}
			if (flag)
			{
				result = CellFinder.FindNoWipeSpawnLocNear(result, map, parentDef, Rot4.North, 2, (IntVec3 c) => CanSpawnHiveAt(c, map, pos, parentDef, minDist, ignoreRoofedRequirement));
				break;
			}
		}
		return result;
	}

	private static bool CanSpawnHiveAt(IntVec3 c, Map map, IntVec3 parentPos, ThingDef parentDef, float minDist, bool ignoreRoofedRequirement)
	{
		if ((!ignoreRoofedRequirement && !c.Roofed(map)) || !c.Walkable(map) || (minDist != 0f && !((float)c.DistanceToSquared(parentPos) >= minDist * minDist)) || c.GetFirstThing(map, ThingDefOf.InsectJelly) != null || c.GetFirstThing(map, ThingDefOf.GlowPod) != null)
		{
			return false;
		}
		for (int i = 0; i < 9; i++)
		{
			IntVec3 c2 = c + GenAdj.AdjacentCellsAndInside[i];
			if (!c2.InBounds(map))
			{
				continue;
			}
			List<Thing> thingList = c2.GetThingList(map);
			for (int j = 0; j < thingList.Count; j++)
			{
				if (thingList[j] is Hive || thingList[j] is TunnelHiveSpawner)
				{
					return false;
				}
			}
		}
		List<Thing> thingList2 = c.GetThingList(map);
		for (int k = 0; k < thingList2.Count; k++)
		{
			Thing thing = thingList2[k];
			if (thing.def.category == ThingCategory.Building && thing.def.passability == Traversability.Impassable && GenSpawn.SpawningWipes(parentDef, thing.def))
			{
				return true;
			}
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__16))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__16(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void PostExposeData()
	{
		Scribe_Values.Look(ref nextHiveSpawnTick, "nextHiveSpawnTick", 0);
		Scribe_Values.Look(ref canSpawnHives, "canSpawnHives", defaultValue: true);
		Scribe_Values.Look(ref wasActivated, "wasActivated", defaultValue: true);
	}
}
