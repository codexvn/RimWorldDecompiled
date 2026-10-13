using System;
using System.Collections.Generic;
using LudeonTK;
using RimWorld;
using Unity.Collections;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x0200103D RID: 4157
	public static class CastPositionFinder
	{
		// Token: 0x06006569 RID: 25961 RVA: 0x002016AF File Offset: 0x001FF8AF
		static CastPositionFinder()
		{
			UnityData.DisposeStatic += delegate()
			{
				CastPositionFinder.emptyByteArray.Dispose();
			};
		}

		// Token: 0x0600656A RID: 25962 RVA: 0x002016E4 File Offset: 0x001FF8E4
		public static bool TryFindCastPosition(CastPositionRequest newReq, out IntVec3 dest)
		{
			CastPositionFinder.req = newReq;
			CastPositionFinder.casterLoc = CastPositionFinder.req.caster.Position;
			CastPositionFinder.targetLoc = CastPositionFinder.req.target.Position;
			CastPositionFinder.verb = CastPositionFinder.req.verb;
			AvoidGrid avoidGrid;
			CastPositionFinder.avoidGrid = (newReq.caster.TryGetAvoidGrid(out avoidGrid, false) ? avoidGrid.Grid : CastPositionFinder.emptyByteArray.AsReadOnly());
			if (CastPositionFinder.verb == null)
			{
				Pawn caster = CastPositionFinder.req.caster;
				Log.Error(((caster != null) ? caster.ToString() : null) + " tried to find casting position without a verb.");
				dest = IntVec3.Invalid;
				CastPositionFinder.req = default(CastPositionRequest);
				return false;
			}
			if (CastPositionFinder.req.maxRegions > 0)
			{
				Region region = CastPositionFinder.casterLoc.GetRegion(CastPositionFinder.req.caster.Map, RegionType.Set_Passable);
				if (region == null)
				{
					Log.Error("TryFindCastPosition requiring region traversal but root region is null.");
					dest = IntVec3.Invalid;
					CastPositionFinder.req = default(CastPositionRequest);
					CastPositionFinder.verb = null;
					return false;
				}
				CastPositionFinder.inRadiusMark = Rand.Int;
				RegionTraverser.MarkRegionsBFS(region, null, newReq.maxRegions, CastPositionFinder.inRadiusMark, RegionType.Set_Passable);
				if (CastPositionFinder.req.maxRangeFromLocus > 0.01f)
				{
					Region locusReg = CastPositionFinder.req.locus.GetRegion(CastPositionFinder.req.caster.Map, RegionType.Set_Passable);
					if (locusReg == null)
					{
						string str = "locus ";
						IntVec3 locus = CastPositionFinder.req.locus;
						Log.Error(str + locus.ToString() + " has no region");
						dest = IntVec3.Invalid;
						CastPositionFinder.req = default(CastPositionRequest);
						CastPositionFinder.verb = null;
						return false;
					}
					if (locusReg.mark != CastPositionFinder.inRadiusMark)
					{
						CastPositionFinder.inRadiusMark = Rand.Int;
						RegionTraverser.BreadthFirstTraverse(region, null, delegate(Region r)
						{
							r.mark = CastPositionFinder.inRadiusMark;
							CastPositionFinder.req.maxRegions = CastPositionFinder.req.maxRegions + 1;
							return r == locusReg;
						}, 999999, RegionType.Set_Passable);
					}
				}
			}
			CellRect cellRect = CellRect.WholeMap(CastPositionFinder.req.caster.Map);
			if (CastPositionFinder.req.maxRangeFromCaster > 0.01f)
			{
				int num = Mathf.CeilToInt(CastPositionFinder.req.maxRangeFromCaster);
				CellRect otherRect = new CellRect(CastPositionFinder.casterLoc.x - num, CastPositionFinder.casterLoc.z - num, num * 2 + 1, num * 2 + 1);
				cellRect.ClipInsideRect(otherRect);
			}
			int num2 = Mathf.CeilToInt(CastPositionFinder.req.maxRangeFromTarget);
			CellRect otherRect2 = new CellRect(CastPositionFinder.targetLoc.x - num2, CastPositionFinder.targetLoc.z - num2, num2 * 2 + 1, num2 * 2 + 1);
			cellRect.ClipInsideRect(otherRect2);
			if (CastPositionFinder.req.maxRangeFromLocus > 0.01f)
			{
				int num3 = Mathf.CeilToInt(CastPositionFinder.req.maxRangeFromLocus);
				CellRect otherRect3 = new CellRect(CastPositionFinder.targetLoc.x - num3, CastPositionFinder.targetLoc.z - num3, num3 * 2 + 1, num3 * 2 + 1);
				cellRect.ClipInsideRect(otherRect3);
			}
			CastPositionFinder.bestSpot = IntVec3.Invalid;
			CastPositionFinder.bestSpotPref = 0.001f;
			CastPositionFinder.maxRangeFromCasterSquared = CastPositionFinder.req.maxRangeFromCaster * CastPositionFinder.req.maxRangeFromCaster;
			CastPositionFinder.maxRangeFromTargetSquared = CastPositionFinder.req.maxRangeFromTarget * CastPositionFinder.req.maxRangeFromTarget;
			CastPositionFinder.maxRangeFromLocusSquared = CastPositionFinder.req.maxRangeFromLocus * CastPositionFinder.req.maxRangeFromLocus;
			CastPositionFinder.rangeFromTarget = (CastPositionFinder.req.caster.Position - CastPositionFinder.req.target.Position).LengthHorizontal;
			CastPositionFinder.rangeFromTargetSquared = (float)(CastPositionFinder.req.caster.Position - CastPositionFinder.req.target.Position).LengthHorizontalSquared;
			CastPositionFinder.optimalRangeSquared = CastPositionFinder.verb.EffectiveRange * 0.8f * (CastPositionFinder.verb.verbProps.range * 0.8f);
			if (CastPositionFinder.req.preferredCastPosition != null && CastPositionFinder.req.preferredCastPosition.Value.IsValid)
			{
				CastPositionFinder.EvaluateCell(CastPositionFinder.req.preferredCastPosition.Value);
				if (CastPositionFinder.bestSpot.IsValid && CastPositionFinder.bestSpotPref > 0.001f)
				{
					dest = CastPositionFinder.req.preferredCastPosition.Value;
					CastPositionFinder.req = default(CastPositionRequest);
					CastPositionFinder.verb = null;
					return true;
				}
			}
			CastPositionFinder.EvaluateCell(CastPositionFinder.req.caster.Position);
			if ((double)CastPositionFinder.bestSpotPref >= 1.0)
			{
				dest = CastPositionFinder.req.caster.Position;
				CastPositionFinder.req = default(CastPositionRequest);
				CastPositionFinder.verb = null;
				return true;
			}
			float slope = -1f / CellLine.Between(CastPositionFinder.req.target.Position, CastPositionFinder.req.caster.Position).Slope;
			CellLine cellLine = new CellLine(CastPositionFinder.req.target.Position, slope);
			bool flag = cellLine.CellIsAbove(CastPositionFinder.req.caster.Position);
			foreach (IntVec3 c in cellRect)
			{
				if (cellLine.CellIsAbove(c) == flag && cellRect.Contains(c))
				{
					CastPositionFinder.EvaluateCell(c);
				}
			}
			if (CastPositionFinder.bestSpot.IsValid && CastPositionFinder.bestSpotPref > 0.33f)
			{
				dest = CastPositionFinder.bestSpot;
				CastPositionFinder.req = default(CastPositionRequest);
				CastPositionFinder.verb = null;
				return true;
			}
			foreach (IntVec3 c2 in cellRect)
			{
				if (cellLine.CellIsAbove(c2) != flag && cellRect.Contains(c2))
				{
					CastPositionFinder.EvaluateCell(c2);
				}
			}
			if (CastPositionFinder.bestSpot.IsValid)
			{
				dest = CastPositionFinder.bestSpot;
				CastPositionFinder.req = default(CastPositionRequest);
				CastPositionFinder.verb = null;
				return true;
			}
			dest = CastPositionFinder.casterLoc;
			CastPositionFinder.req = default(CastPositionRequest);
			CastPositionFinder.verb = null;
			return false;
		}

		// Token: 0x0600656B RID: 25963 RVA: 0x00201D3C File Offset: 0x001FFF3C
		private static void EvaluateCell(IntVec3 c)
		{
			if (CastPositionFinder.req.validator != null && !CastPositionFinder.req.validator(c))
			{
				return;
			}
			if (CastPositionFinder.maxRangeFromTargetSquared > 0.01f && CastPositionFinder.maxRangeFromTargetSquared < 250000f && (float)(c - CastPositionFinder.req.target.Position).LengthHorizontalSquared > CastPositionFinder.maxRangeFromTargetSquared)
			{
				if (DebugViewSettings.drawCastPositionSearch)
				{
					CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, 0f, "range target", 50);
				}
				return;
			}
			if ((double)CastPositionFinder.maxRangeFromLocusSquared > 0.01 && (float)(c - CastPositionFinder.req.locus).LengthHorizontalSquared > CastPositionFinder.maxRangeFromLocusSquared)
			{
				if (DebugViewSettings.drawCastPositionSearch)
				{
					CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, 0.1f, "range home", 50);
				}
				return;
			}
			if (CastPositionFinder.maxRangeFromCasterSquared > 0.01f)
			{
				CastPositionFinder.rangeFromCasterToCellSquared = (float)(c - CastPositionFinder.req.caster.Position).LengthHorizontalSquared;
				if (CastPositionFinder.rangeFromCasterToCellSquared > CastPositionFinder.maxRangeFromCasterSquared)
				{
					if (DebugViewSettings.drawCastPositionSearch)
					{
						CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, 0.2f, "range caster", 50);
					}
					return;
				}
			}
			if (!c.WalkableBy(CastPositionFinder.req.caster.Map, CastPositionFinder.req.caster))
			{
				return;
			}
			if (CastPositionFinder.req.caster.Position != c && !c.InAllowedArea(CastPositionFinder.req.caster))
			{
				return;
			}
			if (CastPositionFinder.req.maxRegions > 0 && c.GetRegion(CastPositionFinder.req.caster.Map, RegionType.Set_Passable).mark != CastPositionFinder.inRadiusMark)
			{
				if (DebugViewSettings.drawCastPositionSearch)
				{
					CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, 0.64f, "reg radius", 50);
				}
				return;
			}
			if (!CastPositionFinder.req.caster.Map.reachability.CanReach(CastPositionFinder.req.caster.Position, c, PathEndMode.OnCell, TraverseParms.For(CastPositionFinder.req.caster, Danger.Some, TraverseMode.ByPawn, false, false, false, true)))
			{
				if (DebugViewSettings.drawCastPositionSearch)
				{
					CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, 0.4f, "can't reach", 50);
				}
				return;
			}
			float num = CastPositionFinder.CastPositionPreference(c);
			if (CastPositionFinder.avoidGrid.Length > 0)
			{
				byte b = CastPositionFinder.avoidGrid[CastPositionFinder.req.caster.Map.cellIndices.CellToIndex(c)];
				num *= Mathf.Max(0.1f, (37.5f - (float)b) / 37.5f);
			}
			if (DebugViewSettings.drawCastPositionSearch)
			{
				CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, num / 4f, num.ToString("F3"), 50);
			}
			if (num < CastPositionFinder.bestSpotPref)
			{
				return;
			}
			if (!CastPositionFinder.verb.CanHitTargetFrom(c, CastPositionFinder.req.target))
			{
				if (DebugViewSettings.drawCastPositionSearch)
				{
					CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, 0.6f, "can't hit", 50);
				}
				return;
			}
			if (!CastPositionFinder.req.caster.Map.pawnDestinationReservationManager.CanReserve(c, CastPositionFinder.req.caster, false))
			{
				if (DebugViewSettings.drawCastPositionSearch)
				{
					CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, num * 0.9f, "resvd", 50);
				}
				return;
			}
			if (PawnUtility.KnownDangerAt(c, CastPositionFinder.req.caster.Map, CastPositionFinder.req.caster))
			{
				if (DebugViewSettings.drawCastPositionSearch)
				{
					CastPositionFinder.req.caster.Map.debugDrawer.FlashCell(c, 0.9f, "danger", 50);
				}
				return;
			}
			CastPositionFinder.bestSpot = c;
			CastPositionFinder.bestSpotPref = num;
		}

		// Token: 0x0600656C RID: 25964 RVA: 0x00202148 File Offset: 0x00200348
		private static float CastPositionPreference(IntVec3 c)
		{
			bool flag = true;
			List<Thing> list = CastPositionFinder.req.caster.Map.thingGrid.ThingsListAtFast(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				Fire fire = thing as Fire;
				if (fire != null && fire.parent == null)
				{
					return -1f;
				}
				if (thing.def.passability == Traversability.PassThroughOnly)
				{
					flag = false;
				}
			}
			float num = 0.3f;
			if (CastPositionFinder.req.caster.kindDef.aiAvoidCover)
			{
				num += 8f - CoverUtility.TotalSurroundingCoverScore(c, CastPositionFinder.req.caster.Map);
			}
			if (CastPositionFinder.req.wantCoverFromTarget)
			{
				num += CoverUtility.CalculateOverallBlockChance(c, CastPositionFinder.req.target.Position, CastPositionFinder.req.caster.Map) * 0.55f;
			}
			float num2 = (CastPositionFinder.req.caster.Position - c).LengthHorizontal;
			if (CastPositionFinder.rangeFromTarget > 100f)
			{
				num2 -= CastPositionFinder.rangeFromTarget - 100f;
				if (num2 < 0f)
				{
					num2 = 0f;
				}
			}
			num *= Mathf.Pow(0.967f, num2);
			float num3 = 1f;
			CastPositionFinder.rangeFromTargetToCellSquared = (float)(c - CastPositionFinder.req.target.Position).LengthHorizontalSquared;
			float num4 = Mathf.Abs(CastPositionFinder.rangeFromTargetToCellSquared - CastPositionFinder.optimalRangeSquared) / CastPositionFinder.optimalRangeSquared;
			num4 = 1f - num4;
			num4 = 0.7f + 0.3f * num4;
			num3 *= num4;
			if (CastPositionFinder.rangeFromTargetToCellSquared < 25f)
			{
				num3 *= 0.5f;
			}
			num *= num3;
			if (CastPositionFinder.rangeFromCasterToCellSquared > CastPositionFinder.rangeFromTargetSquared)
			{
				num *= 0.4f;
			}
			if (!flag)
			{
				num *= 0.4f;
			}
			return num;
		}

		// Token: 0x0400487E RID: 18558
		private static CastPositionRequest req;

		// Token: 0x0400487F RID: 18559
		private static IntVec3 casterLoc;

		// Token: 0x04004880 RID: 18560
		private static IntVec3 targetLoc;

		// Token: 0x04004881 RID: 18561
		private static Verb verb;

		// Token: 0x04004882 RID: 18562
		private static float rangeFromTarget;

		// Token: 0x04004883 RID: 18563
		private static float rangeFromTargetSquared;

		// Token: 0x04004884 RID: 18564
		private static float optimalRangeSquared;

		// Token: 0x04004885 RID: 18565
		private static float rangeFromCasterToCellSquared;

		// Token: 0x04004886 RID: 18566
		private static float rangeFromTargetToCellSquared;

		// Token: 0x04004887 RID: 18567
		private static int inRadiusMark;

		// Token: 0x04004888 RID: 18568
		private static NativeArray<byte>.ReadOnly avoidGrid;

		// Token: 0x04004889 RID: 18569
		private static float maxRangeFromCasterSquared;

		// Token: 0x0400488A RID: 18570
		private static float maxRangeFromTargetSquared;

		// Token: 0x0400488B RID: 18571
		private static float maxRangeFromLocusSquared;

		// Token: 0x0400488C RID: 18572
		private static IntVec3 bestSpot = IntVec3.Invalid;

		// Token: 0x0400488D RID: 18573
		private static float bestSpotPref = 0.001f;

		// Token: 0x0400488E RID: 18574
		private static NativeArray<byte> emptyByteArray = NativeArrayUtility.EmptyArray<byte>();

		// Token: 0x0400488F RID: 18575
		private const float BaseAIPreference = 0.3f;

		// Token: 0x04004890 RID: 18576
		private const float MinimumPreferredRange = 5f;

		// Token: 0x04004891 RID: 18577
		private const float OptimalRangeFactor = 0.8f;

		// Token: 0x04004892 RID: 18578
		private const float OptimalRangeFactorImportance = 0.3f;

		// Token: 0x04004893 RID: 18579
		private const float CoverPreferenceFactor = 0.55f;
	}
}
