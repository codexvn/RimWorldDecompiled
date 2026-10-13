using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002058 RID: 8280
	public class GenStep_CrashedMechanoidPlatform : GenStep_LargeRuins
	{
		// Token: 0x17001BC2 RID: 7106
		// (get) Token: 0x0600B5D8 RID: 46552 RVA: 0x0031F50D File Offset: 0x0031D70D
		protected override IntRange RuinsMinMaxRange
		{
			get
			{
				return new IntRange(1, 3);
			}
		}

		// Token: 0x17001BC3 RID: 7107
		// (get) Token: 0x0600B5D9 RID: 46553 RVA: 0x00343E81 File Offset: 0x00342081
		public override int SeedPart
		{
			get
			{
				return 7937381;
			}
		}

		// Token: 0x17001BC4 RID: 7108
		// (get) Token: 0x0600B5DA RID: 46554 RVA: 0x00123991 File Offset: 0x00121B91
		protected override int MoveRangeLimit
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x17001BC5 RID: 7109
		// (get) Token: 0x0600B5DB RID: 46555 RVA: 0x00123991 File Offset: 0x00121B91
		protected override int ContractLimit
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x17001BC6 RID: 7110
		// (get) Token: 0x0600B5DC RID: 46556 RVA: 0x00335B70 File Offset: 0x00333D70
		protected override int MinRegionSize
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x17001BC7 RID: 7111
		// (get) Token: 0x0600B5DD RID: 46557 RVA: 0x00343E88 File Offset: 0x00342088
		protected override LayoutDef LayoutDef
		{
			get
			{
				return LayoutDefOf.CrashedMechanoidPlatform_Standard;
			}
		}

		// Token: 0x17001BC8 RID: 7112
		// (get) Token: 0x0600B5DE RID: 46558 RVA: 0x00003764 File Offset: 0x00001964
		protected override Faction Faction
		{
			get
			{
				return Faction.OfMechanoids;
			}
		}

		// Token: 0x17001BC9 RID: 7113
		// (get) Token: 0x0600B5DF RID: 46559 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool UseUsedRects
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001BCA RID: 7114
		// (get) Token: 0x0600B5E0 RID: 46560 RVA: 0x00343E8F File Offset: 0x0034208F
		protected override TerrainAffordanceDef MinAffordance
		{
			get
			{
				return TerrainAffordanceDefOf.Light;
			}
		}

		// Token: 0x0600B5E1 RID: 46561 RVA: 0x00343E96 File Offset: 0x00342096
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckOdyssey("Crashed Mechanoid Platform"))
			{
				return;
			}
			this.placedReactorLayout = false;
			base.Generate(map, parms);
		}

		// Token: 0x0600B5E2 RID: 46562 RVA: 0x00343EB4 File Offset: 0x003420B4
		protected override LayoutStructureSketch GenerateAndSpawn(CellRect rect, Map map, GenStepParams parms, LayoutDef layoutDef)
		{
			if (!this.placedReactorLayout)
			{
				this.placedReactorLayout = true;
				layoutDef = LayoutDefOf.CrashedMechanoidPlatform_Engine;
			}
			LayoutStructureSketch layoutStructureSketch = base.GenerateAndSpawn(rect, map, parms, layoutDef);
			MapGenUtility.DoRectEdgeLumps(map, TerrainDefOf.MechanoidPlatform, ref rect, GenStep_OrbitalPlatform.LandingPadBorderLumpLengthRange, GenStep_OrbitalPlatform.LandingPadBorderLumpOffsetRange, TerrainDefOf.MechanoidPlatform);
			GenStep_CrashedMechanoidPlatform.SpawnDirtDebrisCraters(rect, map, layoutStructureSketch);
			return layoutStructureSketch;
		}

		// Token: 0x0600B5E3 RID: 46563 RVA: 0x00343F0C File Offset: 0x0034210C
		private static void SpawnDirtDebrisCraters(CellRect rect, Map map, LayoutStructureSketch sketch)
		{
			foreach (IntVec3 intVec in rect.Cells)
			{
				if (!intVec.GetTerrain(map).natural)
				{
					map.terrainGrid.SetTerrain(intVec, TerrainDefOf.MechanoidPlatform);
					if (Rand.Range(0, 16) == 0)
					{
						ScatterDebrisUtility.ScatterFilthAroundCell(intVec, new IntVec2(6, 6), map, Rand.Bool ? ThingDefOf.Filth_Ash : ThingDefOf.Filth_LooseGround, 0.5f, int.MaxValue, null);
					}
				}
			}
			foreach (IntVec3 c in rect.Cells.InRandomOrder(null).Take((int)((float)rect.Area * 0.01f)))
			{
				if (c.InBounds(map))
				{
					map.terrainGrid.SetTerrain(c, TerrainDefOf.PackedDirt);
				}
			}
			CellRect container = rect.ExpandedBy(8).ClipInsideMap(map);
			foreach (IntVec3 intVec2 in rect.DifferenceCells(container).InRandomOrder(null).Take((int)((float)container.Area * 0.01f)))
			{
				if (intVec2.InBounds(map))
				{
					ScatterDebrisUtility.ScatterFilthAroundCell(intVec2, new IntVec2(6, 6), map, Rand.Bool ? ThingDefOf.Filth_Ash : ThingDefOf.Filth_LooseGround, 0.5f, int.MaxValue, null);
				}
			}
			foreach (IntVec3 loc in rect.ExpandedBy(4).ClipInsideMap(map).DifferenceCells(container).InRandomOrder(null).Take((int)((float)container.Area * 0.01f)))
			{
				int num = Rand.Range(0, 3);
				ThingDef def;
				if (num == 0)
				{
					def = ThingDefOf.CraterSmall;
				}
				else if (num == 1)
				{
					def = ThingDefOf.CraterMedium;
				}
				else
				{
					def = ThingDefOf.CraterLarge;
				}
				Thing thing;
				GenSpawn.TrySpawn(def, loc, map, out thing, WipeMode.Vanish, false);
			}
		}

		// Token: 0x04007CBE RID: 31934
		private bool placedReactorLayout;

		// Token: 0x04007CBF RID: 31935
		private const float RuinAreaDirtFactor = 0.01f;

		// Token: 0x04007CC0 RID: 31936
		private const float RuinAreaFilthFactor = 0.01f;

		// Token: 0x04007CC1 RID: 31937
		private const float AreaCraterFactor = 0.01f;
	}
}
