using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld.BaseGen
{
	// Token: 0x02003984 RID: 14724
	public class SymbolResolver_WorkSite_Mining : SymbolResolver_WorkSite
	{
		// Token: 0x060143AB RID: 82859 RVA: 0x006096E0 File Offset: 0x006078E0
		public override void Resolve(ResolveParams rp)
		{
			SymbolResolver_WorkSite_Mining.<>c__DisplayClass8_0 CS$<>8__locals1 = new SymbolResolver_WorkSite_Mining.<>c__DisplayClass8_0();
			List<CellRect> orGenerateVar = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			CS$<>8__locals1.myUsedRects = new List<CellRect>(orGenerateVar);
			CS$<>8__locals1.myUsedRects.Add(rp.rect);
			CellRect rect = rp.rect;
			IntVec3 centerCell = rect.CenterCell;
			TechLevel techLevel = ThingDefOf.GroundPenetratingScanner.researchPrerequisites.Max((ResearchProjectDef r) => r.techLevel);
			if (rp.workSitePoints >= 500f && rp.faction.def.techLevel >= techLevel)
			{
				CellRect rect2 = rect.ExpandedBy(5).ClipInsideMap(BaseGen.globalSettings.map);
				CellRect rect3 = rect2.ExpandedBy(10).ClipInsideMap(BaseGen.globalSettings.map);
				int num = Mathf.FloorToInt(SymbolResolver_WorkSite_Mining.DeepDrillCountCurve.Evaluate(rp.workSitePoints));
				for (int i = 0; i < num; i++)
				{
					BaseGen.symbolStack.Push("thing", new ResolveParams
					{
						rect = rect3,
						singleThingDef = ThingDefOf.DeepDrill
					}, null);
				}
				BaseGen.symbolStack.Push("thing", new ResolveParams
				{
					rect = rect2,
					singleThingDef = ThingDefOf.GroundPenetratingScanner
				}, null);
			}
			CS$<>8__locals1.map = BaseGen.globalSettings.map;
			CS$<>8__locals1.tunnelCandidates = new List<SymbolResolver_WorkSite_Mining.TunnelCandidate>();
			using (IEnumerator<IntVec3> enumerator = GenRadial.RadialCellsAround(centerCell, 30f, false).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SymbolResolver_WorkSite_Mining.<>c__DisplayClass8_1 CS$<>8__locals2 = new SymbolResolver_WorkSite_Mining.<>c__DisplayClass8_1();
					CS$<>8__locals2.start = enumerator.Current;
					if (CS$<>8__locals2.start.InBounds(CS$<>8__locals1.map) && CS$<>8__locals1.<Resolve>g__HasRock|1(CS$<>8__locals2.start) && !CS$<>8__locals1.<Resolve>g__IsInsideUsedRects|2(CS$<>8__locals2.start) && !CS$<>8__locals1.tunnelCandidates.Any((SymbolResolver_WorkSite_Mining.TunnelCandidate tc) => tc.start.DistanceTo(CS$<>8__locals2.start) < 6f || tc.end.DistanceTo(CS$<>8__locals2.start) < 6f))
					{
						IntVec3 end = CS$<>8__locals2.start;
						IntVec3 intVec = centerCell - CS$<>8__locals2.start;
						if (intVec.x > 1)
						{
							intVec.x = 1;
						}
						else if (intVec.x < -1)
						{
							intVec.x = -1;
						}
						if (intVec.y > 1)
						{
							intVec.y = 1;
						}
						else if (intVec.y < -1)
						{
							intVec.y = -1;
						}
						if (intVec.z > 1)
						{
							intVec.z = 1;
						}
						else if (intVec.z < -1)
						{
							intVec.z = -1;
						}
						Rot4 rot = Rot4.FromIntVec3(intVec);
						int num2 = 0;
						while (CS$<>8__locals1.<Resolve>g__HasRock|1(end) && !CS$<>8__locals1.<Resolve>g__IsInsideUsedRects|2(end))
						{
							end += rot.FacingCell;
							if (!end.InBounds(CS$<>8__locals1.map) || end.DistanceTo(CS$<>8__locals1.map.Center) > 30f)
							{
								num2 = -1;
								end -= rot.FacingCell;
								break;
							}
							num2++;
						}
						if (num2 >= 4)
						{
							PawnPath pawnPath = PawnPath.NotFound;
							if (CS$<>8__locals1.map.reachability.CanReach(end, centerCell, PathEndMode.OnCell, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false)))
							{
								pawnPath = CS$<>8__locals1.map.pathFinder.FindPathNow(end, centerCell, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false), null, PathEndMode.OnCell, null);
							}
							PawnPath pawnPath2 = PawnPath.NotFound;
							if (CS$<>8__locals1.map.reachability.CanReach(CS$<>8__locals2.start, centerCell, PathEndMode.OnCell, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false)))
							{
								pawnPath2 = CS$<>8__locals1.map.pathFinder.FindPathNow(CS$<>8__locals2.start, centerCell, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false), null, PathEndMode.OnCell, null);
							}
							if (pawnPath.Found || pawnPath2.Found)
							{
								float pathCost;
								if (pawnPath.Found && !pawnPath2.Found)
								{
									pathCost = pawnPath.TotalCost;
								}
								else if (pawnPath2.Found && !pawnPath.Found)
								{
									pathCost = pawnPath2.TotalCost;
								}
								else
								{
									pathCost = Mathf.Min(pawnPath.TotalCost, pawnPath2.TotalCost);
								}
								SymbolResolver_WorkSite_Mining.TunnelCandidate item = new SymbolResolver_WorkSite_Mining.TunnelCandidate
								{
									start = CS$<>8__locals2.start,
									end = end,
									pathCost = pathCost
								};
								pawnPath.ReleaseToPool();
								pawnPath2.ReleaseToPool();
								CellRect cRect = item.Rect;
								if (!CS$<>8__locals1.tunnelCandidates.Contains(item) && !CS$<>8__locals1.tunnelCandidates.Any((SymbolResolver_WorkSite_Mining.TunnelCandidate tc) => tc.Rect.Overlaps(cRect) && !tc.Rect.AdjacentCells.Contains(CS$<>8__locals2.start) && !tc.Rect.AdjacentCells.Contains(end)))
								{
									CS$<>8__locals1.tunnelCandidates.Add(item);
								}
							}
						}
					}
				}
			}
			CS$<>8__locals1.excavatedCells = new List<IntVec3>();
			int num3 = Mathf.Min((int)Rand.Range(5f, 15f), CS$<>8__locals1.tunnelCandidates.Count);
			int j = 0;
			CS$<>8__locals1.<Resolve>g__SortCandidates|3();
			while (j < num3)
			{
				SymbolResolver_WorkSite_Mining.TunnelCandidate tunnelCandidate = CS$<>8__locals1.tunnelCandidates[0];
				CS$<>8__locals1.tunnelCandidates.RemoveAt(0);
				foreach (IntVec3 intVec2 in tunnelCandidate.Rect.ExpandedBy(1))
				{
					if (intVec2.InBounds(CS$<>8__locals1.map))
					{
						List<Thing> thingList = intVec2.GetThingList(CS$<>8__locals1.map);
						for (int k = thingList.Count - 1; k >= 0; k--)
						{
							Thing thing = thingList[k];
							if (thing.def.building != null && thing.def.building.isNaturalRock)
							{
								thing.Destroy(DestroyMode.Vanish);
							}
						}
						CS$<>8__locals1.excavatedCells.Add(intVec2);
					}
				}
				j++;
				if (j == num3)
				{
					break;
				}
				CS$<>8__locals1.<Resolve>g__SortCandidates|3();
			}
			base.Resolve(rp);
		}

		// Token: 0x0400DF84 RID: 57220
		private const int MinPointsDeepDrilling = 500;

		// Token: 0x0400DF85 RID: 57221
		private const int MinTunnelLength = 4;

		// Token: 0x0400DF86 RID: 57222
		private const int MaxTunnelExitDist = 30;

		// Token: 0x0400DF87 RID: 57223
		private const float RockTunnelCountMin = 5f;

		// Token: 0x0400DF88 RID: 57224
		private const float RockTunnelCountMax = 15f;

		// Token: 0x0400DF89 RID: 57225
		private const float MinDistBetweenTunnels = 6f;

		// Token: 0x0400DF8A RID: 57226
		private static readonly SimpleCurve DeepDrillCountCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(500f, 1f),
				true
			},
			{
				new CurvePoint(750f, 2f),
				true
			},
			{
				new CurvePoint(1000f, 3f),
				true
			}
		};

		// Token: 0x02003985 RID: 14725
		private struct TunnelCandidate : IEquatable<SymbolResolver_WorkSite_Mining.TunnelCandidate>
		{
			// Token: 0x17003334 RID: 13108
			// (get) Token: 0x060143AE RID: 82862 RVA: 0x00609E04 File Offset: 0x00608004
			public CellRect Rect
			{
				get
				{
					return new CellRect(Mathf.Min(this.start.x, this.end.x), Mathf.Min(this.start.z, this.end.z), Mathf.Abs(this.start.x - this.end.x), Mathf.Abs(this.start.z - this.end.z));
				}
			}

			// Token: 0x060143AF RID: 82863 RVA: 0x00609E84 File Offset: 0x00608084
			public bool Equals(SymbolResolver_WorkSite_Mining.TunnelCandidate other)
			{
				return (this.start == other.start && this.end == other.end) || (this.end == other.start && this.start == other.end);
			}

			// Token: 0x0400DF8B RID: 57227
			public IntVec3 start;

			// Token: 0x0400DF8C RID: 57228
			public IntVec3 end;

			// Token: 0x0400DF8D RID: 57229
			public float pathCost;
		}
	}
}
