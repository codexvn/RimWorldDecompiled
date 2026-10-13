using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200364C RID: 13900
	public static class GenLeaving
	{
		// Token: 0x060134C6 RID: 79046 RVA: 0x005BBC80 File Offset: 0x005B9E80
		public static void DoLeavingsFor(Thing diedThing, Map map, DestroyMode mode, List<Thing> listOfLeavingsOut = null)
		{
			GenLeaving.DoLeavingsFor(diedThing, map, mode, diedThing.OccupiedRect().ExpandedBy(diedThing.def.killedLeavingsExpandRect), null, listOfLeavingsOut);
		}

		// Token: 0x060134C7 RID: 79047 RVA: 0x005BBCB0 File Offset: 0x005B9EB0
		public static void DoLeavingsFor(Thing diedThing, Map map, DestroyMode mode, CellRect leavingsRect, Predicate<IntVec3> nearPlaceValidator = null, List<Thing> listOfLeavingsOut = null)
		{
			if ((Current.ProgramState != ProgramState.Playing && mode != DestroyMode.Refund) || mode == DestroyMode.Vanish || mode == DestroyMode.QuestLogic)
			{
				return;
			}
			bool flag = mode == DestroyMode.KillFinalize || mode == DestroyMode.KillFinalizeLeavingsOnly;
			if (flag && diedThing.def.filthLeaving != null)
			{
				for (int i = leavingsRect.minZ; i <= leavingsRect.maxZ; i++)
				{
					for (int j = leavingsRect.minX; j <= leavingsRect.maxX; j++)
					{
						FilthMaker.TryMakeFilth(new IntVec3(j, 0, i), map, diedThing.def.filthLeaving, Rand.RangeInclusive(1, 3), FilthSourceFlags.None, true);
					}
				}
			}
			if (flag && diedThing.def.race != null && !diedThing.def.race.detritusLeavings.NullOrEmpty<DetritusLeavingType>())
			{
				DetritusLeavingType detritusLeavingType = diedThing.def.race.detritusLeavings.RandomElement<DetritusLeavingType>();
				if (Rand.Chance(detritusLeavingType.spawnChance))
				{
					IntVec3 loc;
					CellFinder.TryFindRandomCellNear(diedThing.Position, map, 1, (IntVec3 c) => c.Standable(map), out loc, -1);
					GenSpawn.Spawn(ThingMaker.MakeThing(detritusLeavingType.def, null), loc, map, WipeMode.Vanish).overrideGraphicIndex = new int?(detritusLeavingType.texOverrideIndex);
				}
			}
			ThingOwner<Thing> thingOwner = new ThingOwner<Thing>();
			if (flag)
			{
				List<ThingDefCountClass> list = new List<ThingDefCountClass>();
				Rand.PushState(diedThing.thingIDNumber);
				Pawn pawn = diedThing as Pawn;
				if ((pawn == null || !pawn.IsShambler) && Rand.Chance(diedThing.def.killedLeavingsChance))
				{
					if (diedThing.def.killedLeavings != null)
					{
						list.AddRange(diedThing.def.killedLeavings);
					}
					if (diedThing.HostileTo(Faction.OfPlayer) && !diedThing.def.killedLeavingsPlayerHostile.NullOrEmpty<ThingDefCountClass>())
					{
						list.AddRange(diedThing.def.killedLeavingsPlayerHostile);
					}
					if (diedThing.def.killedLeavingsRanges != null)
					{
						foreach (ThingDefCountRangeClass thingDefCountRangeClass in diedThing.def.killedLeavingsRanges)
						{
							int num = Mathf.RoundToInt((float)thingDefCountRangeClass.countRange.RandomInRange);
							if (num > 0)
							{
								list.Add(new ThingDefCountClass(thingDefCountRangeClass.thingDef, num));
							}
						}
					}
				}
				ThingWithComps thingWithComps = diedThing as ThingWithComps;
				if (thingWithComps != null)
				{
					list.AddRange(thingWithComps.GetAdditionalLeavings(mode));
				}
				if (ModsConfig.AnomalyActive)
				{
					Pawn pawn2 = diedThing as Pawn;
					if (pawn2 != null && pawn2.IsMutant && !pawn2.mutant.Def.killedLeavings.NullOrEmpty<ThingDefCountClass>())
					{
						list.AddRange(pawn2.mutant.Def.killedLeavings);
					}
				}
				for (int k = 0; k < list.Count; k++)
				{
					ThingDefCountClass thingDefCountClass = list[k];
					if (!thingDefCountClass.IsChanceBased || Rand.Chance(thingDefCountClass.DropChance))
					{
						Thing thing = ThingMaker.MakeThing(list[k].thingDef, null);
						thing.stackCount = list[k].count;
						thingOwner.TryAdd(thing, true);
					}
				}
				Rand.PopState();
			}
			if (GenLeaving.CanBuildingLeaveResources(diedThing, mode) && mode != DestroyMode.KillFinalizeLeavingsOnly)
			{
				Frame frame = diedThing as Frame;
				if (frame != null)
				{
					for (int l = frame.resourceContainer.Count - 1; l >= 0; l--)
					{
						int num2 = GenLeaving.GetBuildingResourcesLeaveCalculator(diedThing, mode)(frame.resourceContainer[l].stackCount);
						if (num2 > 0)
						{
							frame.resourceContainer.TryTransferToContainer(frame.resourceContainer[l], thingOwner, num2, true);
						}
					}
					frame.resourceContainer.ClearAndDestroyContents(DestroyMode.Vanish);
				}
				else
				{
					List<ThingDefCountClass> list2 = diedThing.CostListAdjusted();
					for (int m = 0; m < list2.Count; m++)
					{
						ThingDefCountClass thingDefCountClass2 = list2[m];
						List<ThingDef> forcedCostLeavings = diedThing.def.building.forcedCostLeavings;
						if (forcedCostLeavings != null && forcedCostLeavings.Contains(thingDefCountClass2.thingDef))
						{
							Thing thing2 = ThingMaker.MakeThing(thingDefCountClass2.thingDef, null);
							thing2.stackCount = thingDefCountClass2.count;
							thingOwner.TryAdd(thing2, true);
						}
						else
						{
							if (thingDefCountClass2.thingDef == ThingDefOf.ReinforcedBarrel && !Find.Storyteller.difficulty.classicMortars)
							{
								CompRefuelable compRefuelable = diedThing.TryGetComp<CompRefuelable>();
								if (compRefuelable != null && compRefuelable.Props.fuelIsMortarBarrel && compRefuelable.FuelPercentOfMax < 0.5f)
								{
									goto IL_596;
								}
							}
							BuildingProperties building = diedThing.def.building;
							if (((building != null) ? building.leavingsBlacklist : null) == null || !diedThing.def.building.leavingsBlacklist.Contains(thingDefCountClass2.thingDef))
							{
								int num3 = GenLeaving.GetBuildingResourcesLeaveCalculator(diedThing, mode)(thingDefCountClass2.count);
								if (num3 > 0 && mode == DestroyMode.KillFinalize && thingDefCountClass2.thingDef.slagDef != null)
								{
									int count = thingDefCountClass2.thingDef.slagDef.smeltProducts.First((ThingDefCountClass pro) => pro.thingDef == ThingDefOf.Steel).count;
									int num4 = num3 / count;
									num4 = Mathf.Min(num4, diedThing.def.Size.Area / 2);
									for (int n = 0; n < num4; n++)
									{
										thingOwner.TryAdd(ThingMaker.MakeThing(thingDefCountClass2.thingDef.slagDef, null), true);
									}
									num3 -= num4 * count;
								}
								if (num3 > 0)
								{
									Thing thing3 = ThingMaker.MakeThing(thingDefCountClass2.thingDef, null);
									thing3.stackCount = num3;
									thingOwner.TryAdd(thing3, true);
								}
							}
						}
						IL_596:;
					}
				}
			}
			GenLeaving.tmpKilledLeavings.Clear();
			List<IntVec3> list3 = leavingsRect.Cells.InRandomOrder(null).ToList<IntVec3>();
			int num5 = 0;
			while (thingOwner.Count > 0)
			{
				if (mode == DestroyMode.KillFinalize && !map.areaManager.Home[list3[num5]] && !diedThing.def.forceLeavingsAllowed)
				{
					thingOwner[0].SetForbidden(true, false);
				}
				Thing item;
				if (!thingOwner.TryDrop(thingOwner[0], list3[num5], map, ThingPlaceMode.Near, out item, null, nearPlaceValidator))
				{
					Log.Warning("Failed to place all leavings for destroyed thing " + ((diedThing != null) ? diedThing.ToString() : null) + " at " + leavingsRect.CenterCell.ToString());
					break;
				}
				GenLeaving.tmpKilledLeavings.Add(item);
				num5++;
				if (num5 >= list3.Count)
				{
					num5 = 0;
				}
			}
			if (listOfLeavingsOut != null)
			{
				listOfLeavingsOut.AddRange(GenLeaving.tmpKilledLeavings);
			}
			if (mode == DestroyMode.KillFinalize && GenLeaving.tmpKilledLeavings.Count > 0)
			{
				QuestUtility.SendQuestTargetSignals(diedThing.questTags, "KilledLeavingsLeft", diedThing.Named("DROPPER"), GenLeaving.tmpKilledLeavings.Named("SUBJECT"));
			}
			GenLeaving.tmpKilledLeavings.Clear();
		}

		// Token: 0x060134C8 RID: 79048 RVA: 0x005BC3B8 File Offset: 0x005BA5B8
		public static void DropThingsNear(Thing thing, List<ThingDefCountClass> things, List<Thing> spawnedThings = null, bool forbid = true, Predicate<IntVec3> nearPlaceValidator = null)
		{
			if (things.NullOrEmpty<ThingDefCountClass>())
			{
				return;
			}
			Rand.PushState(thing.thingIDNumber);
			ThingOwner<Thing> thingOwner = new ThingOwner<Thing>();
			foreach (ThingDefCountClass thingDefCountClass in things)
			{
				if (!thingDefCountClass.IsChanceBased || Rand.Chance(thingDefCountClass.DropChance))
				{
					Thing thing2 = ThingMaker.MakeThing(thingDefCountClass.thingDef, null);
					thing2.stackCount = thingDefCountClass.count;
					thingOwner.TryAdd(thing2, true);
				}
			}
			Rand.PopState();
			GenLeaving.DropThingOwnerContents(thingOwner, thing.Map, thing.OccupiedRect().ExpandedBy(1), spawnedThings, forbid, nearPlaceValidator);
		}

		// Token: 0x060134C9 RID: 79049 RVA: 0x005BC478 File Offset: 0x005BA678
		public static void DropThingOwnerContents(ThingOwner owner, Map map, CellRect leavingsRect, List<Thing> spawnedThings = null, bool forbid = true, Predicate<IntVec3> nearPlaceValidator = null)
		{
			List<IntVec3> list = leavingsRect.Cells.InRandomOrder(null).ToList<IntVec3>();
			int num = 0;
			while (owner.Count > 0)
			{
				if (forbid)
				{
					owner[0].SetForbidden(true, false);
				}
				Thing item;
				if (!owner.TryDrop(owner[0], list[num], map, ThingPlaceMode.Near, out item, null, nearPlaceValidator, true))
				{
					Log.Warning(string.Format("Failed to place all leavings for destroyed thing {0} at {1}", owner, leavingsRect.CenterCell));
					return;
				}
				if (spawnedThings != null)
				{
					spawnedThings.Add(item);
				}
				num++;
				if (num >= list.Count)
				{
					num = 0;
				}
			}
		}

		// Token: 0x060134CA RID: 79050 RVA: 0x005BC50C File Offset: 0x005BA70C
		public static void DoLeavingsFor(TerrainDef terrain, IntVec3 cell, Map map)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			ThingOwner<Thing> thingOwner = new ThingOwner<Thing>();
			List<ThingDefCountClass> list = terrain.CostListAdjusted(null, true);
			for (int i = 0; i < list.Count; i++)
			{
				ThingDefCountClass thingDefCountClass = list[i];
				int num = GenMath.RoundRandom((float)thingDefCountClass.count * terrain.resourcesFractionWhenDeconstructed);
				if (num > 0)
				{
					Thing thing = ThingMaker.MakeThing(thingDefCountClass.thingDef, null);
					thing.stackCount = num;
					thingOwner.TryAdd(thing, true);
				}
			}
			while (thingOwner.Count > 0)
			{
				Thing thing2;
				if (!thingOwner.TryDrop(thingOwner[0], cell, map, ThingPlaceMode.Near, out thing2, null, null))
				{
					string str = "Failed to place all leavings for removed terrain ";
					string str2 = (terrain != null) ? terrain.ToString() : null;
					string str3 = " at ";
					IntVec3 intVec = cell;
					Log.Warning(str + str2 + str3 + intVec.ToString());
					return;
				}
			}
		}

		// Token: 0x060134CB RID: 79051 RVA: 0x005BC5D8 File Offset: 0x005BA7D8
		public static bool CanBuildingLeaveResources(Thing destroyedThing, DestroyMode mode)
		{
			if (!(destroyedThing is Building))
			{
				return false;
			}
			if (mode == DestroyMode.Deconstruct && destroyedThing is Frame)
			{
				mode = DestroyMode.Cancel;
			}
			switch (mode)
			{
			case DestroyMode.Vanish:
				return false;
			case DestroyMode.WillReplace:
				return false;
			case DestroyMode.KillFinalize:
				return destroyedThing.def.leaveResourcesWhenKilled;
			case DestroyMode.KillFinalizeLeavingsOnly:
				return false;
			case DestroyMode.Deconstruct:
				return destroyedThing.def.resourcesFractionWhenDeconstructed != 0f;
			case DestroyMode.FailConstruction:
				return true;
			case DestroyMode.Cancel:
				return true;
			case DestroyMode.Refund:
				return true;
			case DestroyMode.QuestLogic:
				return false;
			default:
				throw new ArgumentException("Unknown destroy mode " + mode.ToString());
			}
		}

		// Token: 0x060134CC RID: 79052 RVA: 0x005BC678 File Offset: 0x005BA878
		private static Func<int, int> GetBuildingResourcesLeaveCalculator(Thing destroyedThing, DestroyMode mode)
		{
			if (!GenLeaving.CanBuildingLeaveResources(destroyedThing, mode))
			{
				return (int count) => 0;
			}
			if (mode == DestroyMode.Deconstruct && typeof(Frame).IsAssignableFrom(destroyedThing.GetType()))
			{
				mode = DestroyMode.Cancel;
			}
			switch (mode)
			{
			case DestroyMode.Vanish:
				return (int count) => 0;
			case DestroyMode.WillReplace:
				return (int count) => 0;
			case DestroyMode.KillFinalize:
				return (int count) => GenMath.RoundRandom((float)count * 0.25f);
			case DestroyMode.KillFinalizeLeavingsOnly:
				return (int count) => 0;
			case DestroyMode.Deconstruct:
				return (int count) => Mathf.Min(GenMath.RoundRandom((float)count * destroyedThing.def.resourcesFractionWhenDeconstructed), count);
			case DestroyMode.FailConstruction:
				return (int count) => Mathf.Max(GenMath.RoundRandom((float)count * 0.5f), 1);
			case DestroyMode.Cancel:
				return (int count) => GenMath.RoundRandom((float)count * 1f);
			case DestroyMode.Refund:
				return (int count) => count;
			case DestroyMode.QuestLogic:
				return (int count) => 0;
			default:
				throw new ArgumentException("Unknown destroy mode " + mode.ToString());
			}
		}

		// Token: 0x060134CD RID: 79053 RVA: 0x005BC83C File Offset: 0x005BAA3C
		public static void DropFilthDueToDamage(Thing t, float damageDealt)
		{
			if (!t.def.useHitPoints || !t.Spawned || t.def.filthLeaving == null)
			{
				return;
			}
			CellRect cellRect = t.OccupiedRect().ExpandedBy(1);
			GenLeaving.tmpCellsCandidates.Clear();
			foreach (IntVec3 intVec in cellRect)
			{
				if (intVec.InBounds(t.Map) && intVec.Walkable(t.Map))
				{
					GenLeaving.tmpCellsCandidates.Add(intVec);
				}
			}
			if (!GenLeaving.tmpCellsCandidates.Any<IntVec3>())
			{
				return;
			}
			int num = GenMath.RoundRandom(damageDealt * Mathf.Min(0.016666668f, 1f / ((float)t.MaxHitPoints / 10f)));
			for (int i = 0; i < num; i++)
			{
				FilthMaker.TryMakeFilth(GenLeaving.tmpCellsCandidates.RandomElement<IntVec3>(), t.Map, t.def.filthLeaving, 1, FilthSourceFlags.None, true);
			}
			GenLeaving.tmpCellsCandidates.Clear();
		}

		// Token: 0x0400D629 RID: 54825
		private const float LeaveFraction_Kill = 0.25f;

		// Token: 0x0400D62A RID: 54826
		private const float LeaveFraction_Cancel = 1f;

		// Token: 0x0400D62B RID: 54827
		public const float LeaveFraction_DeconstructDefault = 0.5f;

		// Token: 0x0400D62C RID: 54828
		private const float LeaveFraction_FailConstruction = 0.5f;

		// Token: 0x0400D62D RID: 54829
		private static List<Thing> tmpKilledLeavings = new List<Thing>();

		// Token: 0x0400D62E RID: 54830
		private static List<IntVec3> tmpCellsCandidates = new List<IntVec3>();
	}
}
