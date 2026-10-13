using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003589 RID: 13705
	[DefOf]
	public static class TerrainDefOf
	{
		// Token: 0x06013377 RID: 78711 RVA: 0x005B5972 File Offset: 0x005B3B72
		static TerrainDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TerrainDefOf));
		}

		// Token: 0x0400CB68 RID: 52072
		public static TerrainDef Sand;

		// Token: 0x0400CB69 RID: 52073
		public static TerrainDef SoftSand;

		// Token: 0x0400CB6A RID: 52074
		public static TerrainDef Soil;

		// Token: 0x0400CB6B RID: 52075
		public static TerrainDef SoilRich;

		// Token: 0x0400CB6C RID: 52076
		public static TerrainDef Underwall;

		// Token: 0x0400CB6D RID: 52077
		public static TerrainDef Concrete;

		// Token: 0x0400CB6E RID: 52078
		public static TerrainDef AncientConcrete;

		// Token: 0x0400CB6F RID: 52079
		public static TerrainDef MetalTile;

		// Token: 0x0400CB70 RID: 52080
		public static TerrainDef AncientTile;

		// Token: 0x0400CB71 RID: 52081
		public static TerrainDef Gravel;

		// Token: 0x0400CB72 RID: 52082
		public static TerrainDef WaterDeep;

		// Token: 0x0400CB73 RID: 52083
		public static TerrainDef WaterShallow;

		// Token: 0x0400CB74 RID: 52084
		public static TerrainDef WaterMovingChestDeep;

		// Token: 0x0400CB75 RID: 52085
		public static TerrainDef WaterMovingShallow;

		// Token: 0x0400CB76 RID: 52086
		public static TerrainDef WaterOceanDeep;

		// Token: 0x0400CB77 RID: 52087
		public static TerrainDef WaterOceanShallow;

		// Token: 0x0400CB78 RID: 52088
		public static TerrainDef PavedTile;

		// Token: 0x0400CB79 RID: 52089
		public static TerrainDef WoodPlankFloor;

		// Token: 0x0400CB7A RID: 52090
		public static TerrainDef TileSandstone;

		// Token: 0x0400CB7B RID: 52091
		public static TerrainDef Ice;

		// Token: 0x0400CB7C RID: 52092
		public static TerrainDef Marsh;

		// Token: 0x0400CB7D RID: 52093
		public static TerrainDef Mud;

		// Token: 0x0400CB7E RID: 52094
		public static TerrainDef FlagstoneSandstone;

		// Token: 0x0400CB7F RID: 52095
		public static TerrainDef Bridge;

		// Token: 0x0400CB80 RID: 52096
		public static TerrainDef Sandstone_Smooth;

		// Token: 0x0400CB81 RID: 52097
		public static TerrainDef PackedDirt;

		// Token: 0x0400CB82 RID: 52098
		public static TerrainDef BrokenAsphalt;

		// Token: 0x0400CB83 RID: 52099
		public static TerrainDef Riverbank;

		// Token: 0x0400CB84 RID: 52100
		[MayRequireIdeology]
		public static TerrainDef FungalGravel;

		// Token: 0x0400CB85 RID: 52101
		[MayRequireAnomaly]
		public static TerrainDef Flesh;

		// Token: 0x0400CB86 RID: 52102
		[MayRequireAnomaly]
		public static TerrainDef Voidmetal;

		// Token: 0x0400CB87 RID: 52103
		[MayRequireAnomaly]
		public static TerrainDef GraySurface;

		// Token: 0x0400CB88 RID: 52104
		[MayRequireOdyssey]
		public static TerrainDef Substructure;

		// Token: 0x0400CB89 RID: 52105
		[MayRequireOdyssey]
		public static TerrainDef AncientMegastructure;

		// Token: 0x0400CB8A RID: 52106
		[MayRequireOdyssey]
		public static TerrainDef LavaDeep;

		// Token: 0x0400CB8B RID: 52107
		[MayRequireOdyssey]
		public static TerrainDef LavaShallow;

		// Token: 0x0400CB8C RID: 52108
		[MayRequireOdyssey]
		public static TerrainDef CooledLava;

		// Token: 0x0400CB8D RID: 52109
		[MayRequireOdyssey]
		public static TerrainDef VolcanicRock;

		// Token: 0x0400CB8E RID: 52110
		[MayRequireOdyssey]
		public static TerrainDef ThinIce;

		// Token: 0x0400CB8F RID: 52111
		[MayRequireOdyssey]
		public static TerrainDef ShallowFloodwater;

		// Token: 0x0400CB90 RID: 52112
		[MayRequireOdyssey]
		public static TerrainDef MarshFlood;

		// Token: 0x0400CB91 RID: 52113
		[MayRequireOdyssey]
		public static TerrainDef Space;

		// Token: 0x0400CB92 RID: 52114
		[MayRequireOdyssey]
		public static TerrainDef ToxicWaterShallow;

		// Token: 0x0400CB93 RID: 52115
		[MayRequireOdyssey]
		public static TerrainDef ToxicWaterDeep;

		// Token: 0x0400CB94 RID: 52116
		[MayRequireOdyssey]
		public static TerrainDef InsectSludge;

		// Token: 0x0400CB95 RID: 52117
		[MayRequireOdyssey]
		public static TerrainDef OrbitalPlatform;

		// Token: 0x0400CB96 RID: 52118
		[MayRequireOdyssey]
		public static TerrainDef MechanoidPlatform;

		// Token: 0x0400CB97 RID: 52119
		[MayRequireOdyssey]
		public static TerrainDef DryLakeBed;

		// Token: 0x0400CB98 RID: 52120
		[MayRequireOdyssey]
		public static TerrainDef HotSpring;

		// Token: 0x0400CB99 RID: 52121
		[MayRequireOdyssey]
		public static TerrainDef HeavyBridge;

		// Token: 0x0400CB9A RID: 52122
		[MayRequireOdyssey]
		public static TerrainDef Vacstone_Rough;

		// Token: 0x0400CB9B RID: 52123
		[MayRequireOdyssey]
		public static TerrainDef Vacstone_RoughHewn;
	}
}
