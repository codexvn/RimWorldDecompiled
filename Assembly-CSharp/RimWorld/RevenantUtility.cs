using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020023CC RID: 9164
	public static class RevenantUtility
	{
		// Token: 0x0600C6B6 RID: 50870 RVA: 0x0039E81C File Offset: 0x0039CA1C
		public static int NumSpawnedUnhypnotizedColonists(Map map)
		{
			if (map == null)
			{
				return 0;
			}
			int num = 0;
			using (List<Pawn>.Enumerator enumerator = map.mapPawns.FreeColonistsSpawned.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.health.hediffSet.HasHediff(HediffDefOf.RevenantHypnosis, false))
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x0600C6B7 RID: 50871 RVA: 0x0039E890 File Offset: 0x0039CA90
		public static Pawn ScanForTarget(Pawn pawn, bool forced = false)
		{
			RevenantUtility.tmpTargets.Clear();
			TraverseParms traverseParms = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false);
			RegionTraverser.BreadthFirstTraverse(pawn.Position, pawn.Map, (Region from, Region to) => to.Allows(traverseParms, true), delegate(Region x)
			{
				List<Thing> list = x.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn3 = (Pawn)list[i];
					if (RevenantUtility.ValidTarget(pawn3))
					{
						RevenantUtility.tmpTargets.Add(pawn3);
					}
				}
				return false;
			}, 999999, RegionType.Set_Passable);
			Pawn pawn2;
			if (RevenantUtility.tmpTargets.TryRandomElement(out pawn2) && (forced || RevenantUtility.NearbyHumanlikePawnCount(pawn2.Position, pawn2.Map, 12f) < 5))
			{
				return pawn2;
			}
			return null;
		}

		// Token: 0x0600C6B8 RID: 50872 RVA: 0x0039E930 File Offset: 0x0039CB30
		public static Pawn GetClosestTargetInRadius(Pawn pawn, float radius)
		{
			List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Pawn);
			float num = float.MaxValue;
			Pawn result = null;
			foreach (Thing thing in list)
			{
				Pawn pawn2 = (Pawn)thing;
				if (RevenantUtility.ValidTarget(pawn2) && pawn.Position.InHorDistOf(pawn2.Position, radius) && (float)pawn2.Position.DistanceToSquared(pawn.Position) < num && GenSight.LineOfSightToThing(pawn.Position, pawn2, pawn.Map, false, null))
				{
					num = (float)pawn2.Position.DistanceToSquared(pawn.Position);
					result = pawn2;
				}
			}
			return result;
		}

		// Token: 0x0600C6B9 RID: 50873 RVA: 0x0039E9F8 File Offset: 0x0039CBF8
		public static IntVec3 FindEscapeCell(Pawn pawn)
		{
			IntVec3 invalid = IntVec3.Invalid;
			CellFinder.TryFindRandomCellNear(pawn.Position, pawn.Map, 200, (IntVec3 x) => x.Standable(pawn.Map) && RevenantUtility.NearbyHumanlikePawnCount(x, pawn.Map, 20f) == 0, out invalid, -1);
			return invalid;
		}

		// Token: 0x0600C6BA RID: 50874 RVA: 0x0039EA4C File Offset: 0x0039CC4C
		public static int NearbyHumanlikePawnCount(IntVec3 pos, Map map, float radius)
		{
			int num = 0;
			foreach (Thing thing in map.listerThings.ThingsInGroup(ThingRequestGroup.Pawn))
			{
				Pawn pawn = (Pawn)thing;
				if (RevenantUtility.ValidTarget(pawn) && !pawn.Downed && pos.InHorDistOf(pawn.Position, radius))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600C6BB RID: 50875 RVA: 0x0039EACC File Offset: 0x0039CCCC
		public static bool ValidTarget(Pawn pawn)
		{
			return pawn.RaceProps.Humanlike && pawn.Faction != Faction.OfEntities && !pawn.IsSubhuman && !pawn.health.hediffSet.HasHediff(HediffDefOf.RevenantHypnosis, false);
		}

		// Token: 0x0600C6BC RID: 50876 RVA: 0x0039EB0C File Offset: 0x0039CD0C
		public static void CreateRevenantSmear(Pawn pawn)
		{
			Filth filth;
			FilthMaker.TryMakeFilth(pawn.Position, pawn.Map, ThingDefOf.Filth_RevenantSmear, out filth, 1, FilthSourceFlags.None, true);
			if (filth == null)
			{
				return;
			}
			((FilthRevenantSmear)filth).revenant = pawn;
			pawn.TryGetComp<CompRevenant>().revenantLastLeftSmear = Find.TickManager.TicksGame;
		}

		// Token: 0x0600C6BD RID: 50877 RVA: 0x0039EB5C File Offset: 0x0039CD5C
		public static void OnRevenantDeath(Pawn pawn, Map map)
		{
			Find.LetterStack.ReceiveLetter("LetterLabelRevenantKilled".Translate(), "LetterRevenantKilled".Translate(), LetterDefOf.PositiveEvent, new LookTargets(pawn.PositionHeld, map), null, null, null, null, 0, true);
			((DyingRevenant)GenSpawn.Spawn(ThingDefOf.DyingRevenant, pawn.Position, map, WipeMode.Vanish)).InitWith(pawn);
		}

		// Token: 0x04008762 RID: 34658
		private const float SurroundingPawnsRadius = 12f;

		// Token: 0x04008763 RID: 34659
		public const float NearbyTargetRange = 10f;

		// Token: 0x04008764 RID: 34660
		private const int EscapeDist = 200;

		// Token: 0x04008765 RID: 34661
		public const float EscapedRadius = 20f;

		// Token: 0x04008766 RID: 34662
		public static readonly FloatRange SearchForTargetCooldownRangeDays = new FloatRange(1.8f, 3.2f);

		// Token: 0x04008767 RID: 34663
		public const int WaitBeforeBecomingInvisibleTicks = 140;

		// Token: 0x04008768 RID: 34664
		public const int CheckForAnyTargetMTB = 2500;

		// Token: 0x04008769 RID: 34665
		public const int CheckForNearbyTargetMTB = 900;

		// Token: 0x0400876A RID: 34666
		public static readonly SimpleCurve SpeedRangeFromBecameVisibleCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(30f, 2.8f),
				true
			}
		};

		// Token: 0x0400876B RID: 34667
		public static readonly SimpleCurve RevealRangeFromNumColonistsCurve = new SimpleCurve
		{
			{
				new CurvePoint(2f, 26f),
				true
			},
			{
				new CurvePoint(3f, 18f),
				true
			},
			{
				new CurvePoint(8f, 16f),
				true
			},
			{
				new CurvePoint(15f, 10f),
				true
			}
		};

		// Token: 0x0400876C RID: 34668
		public static readonly SimpleCurve HypnotizeDurationSecondsFromNumColonistsCurve = new SimpleCurve
		{
			{
				new CurvePoint(2f, 15f),
				true
			},
			{
				new CurvePoint(4f, 11f),
				true
			},
			{
				new CurvePoint(10f, 6f),
				true
			}
		};

		// Token: 0x0400876D RID: 34669
		private static HashSet<Pawn> tmpTargets = new HashSet<Pawn>();
	}
}
