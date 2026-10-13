using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004AD RID: 1197
	public class GenStep_ScatterThings : GenStep_Scatterer
	{
		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06001F31 RID: 7985 RVA: 0x000A3C12 File Offset: 0x000A1E12
		public override int SeedPart
		{
			get
			{
				return 1158116095;
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06001F32 RID: 7986 RVA: 0x000A3C1C File Offset: 0x000A1E1C
		private List<Rot4> PossibleRotations
		{
			get
			{
				if (this.possibleRotationsInt == null)
				{
					this.possibleRotationsInt = new List<Rot4>();
					if (this.thingDef.rotatable)
					{
						this.possibleRotationsInt.Add(Rot4.North);
						this.possibleRotationsInt.Add(Rot4.East);
						this.possibleRotationsInt.Add(Rot4.South);
						this.possibleRotationsInt.Add(Rot4.West);
					}
					else
					{
						this.possibleRotationsInt.Add(Rot4.North);
					}
				}
				return this.possibleRotationsInt;
			}
		}

		// Token: 0x06001F33 RID: 7987 RVA: 0x000A3CA4 File Offset: 0x000A1EA4
		public override void Generate(Map map, GenStepParams parms)
		{
			if (this.ShouldSkipMap(map))
			{
				return;
			}
			int count = this.CalculateFinalCount(map);
			IntRange one;
			if (this.thingDef.ingestible != null && this.thingDef.ingestible.IsMeal && this.thingDef.stackLimit <= 10)
			{
				one = IntRange.One;
			}
			else if (this.thingDef.stackLimit > 5)
			{
				one = new IntRange(Mathf.RoundToInt((float)this.thingDef.stackLimit * 0.5f), this.thingDef.stackLimit);
			}
			else
			{
				one = new IntRange(this.thingDef.stackLimit, this.thingDef.stackLimit);
			}
			List<int> list = GenStep_ScatterThings.CountDividedIntoStacks(count, one);
			for (int i = 0; i < list.Count; i++)
			{
				IntVec3 intVec;
				if (!this.TryFindScatterCell(map, out intVec))
				{
					return;
				}
				this.ScatterAt(intVec, map, parms, list[i]);
				this.usedSpots.Add(intVec);
			}
			this.usedSpots.Clear();
			this.clusterCenter = IntVec3.Invalid;
			this.leftInCluster = 0;
		}

		// Token: 0x06001F34 RID: 7988 RVA: 0x000A3DAC File Offset: 0x000A1FAC
		protected override bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			if (this.clusterSize > 1)
			{
				if (this.leftInCluster <= 0)
				{
					if (!base.TryFindScatterCell(map, out this.clusterCenter))
					{
						string str = "Could not find cluster center to scatter ";
						ThingDef thingDef = this.thingDef;
						Log.Error(str + ((thingDef != null) ? thingDef.ToString() : null));
					}
					this.leftInCluster = this.clusterSize;
				}
				this.leftInCluster--;
				result = CellFinder.RandomClosewalkCellNear(this.clusterCenter, map, 4, delegate(IntVec3 x)
				{
					Rot4 rot;
					return this.TryGetRandomValidRotation(x, map, out rot);
				});
				return result.IsValid;
			}
			return base.TryFindScatterCell(map, out result);
		}

		// Token: 0x06001F35 RID: 7989 RVA: 0x000A3E6C File Offset: 0x000A206C
		protected virtual Thing GenerateThing()
		{
			Thing thing = ThingMaker.MakeThing(this.thingDef, this.stuff);
			CompQuality compQuality;
			if (thing.TryGetComp(out compQuality))
			{
				compQuality.SetQuality(this.quality ?? QualityCategory.Normal, new ArtGenerationContext?(ArtGenerationContext.Outsider));
			}
			if (this.thingDef.IsIngestible && this.thingDef.ingestible.IsMeal)
			{
				FoodUtility.GenerateGoodIngredients(thing, Faction.OfPlayer.ideos.PrimaryIdeo);
			}
			if (this.minify && this.thingDef.Minifiable)
			{
				thing = thing.MakeMinified(DestroyMode.Vanish);
			}
			return thing;
		}

		// Token: 0x06001F36 RID: 7990 RVA: 0x000A3F10 File Offset: 0x000A2110
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int stackCount = 1)
		{
			Rot4 rot;
			if (!this.TryGetRandomValidRotation(loc, map, out rot))
			{
				string str = "Could not find any valid rotation for ";
				ThingDef thingDef = this.thingDef;
				Log.Warning(str + ((thingDef != null) ? thingDef.ToString() : null));
				return;
			}
			if (this.clearSpaceSize > 0)
			{
				foreach (IntVec3 c in GridShapeMaker.IrregularLump(loc, map, this.clearSpaceSize, null))
				{
					Building edifice = c.GetEdifice(map);
					if (edifice != null)
					{
						edifice.Destroy(DestroyMode.Vanish);
					}
				}
			}
			Thing thing = this.GenerateThing();
			if (thing.def.category == ThingCategory.Item)
			{
				thing.stackCount = stackCount;
				thing.SetForbidden(true, false);
				Thing thing2;
				GenPlace.TryPlaceThing(thing, loc, map, ThingPlaceMode.Near, out thing2, null, null, null, 1);
				if (this.nearPlayerStart && thing2 != null && thing2.def.category == ThingCategory.Item && TutorSystem.TutorialMode)
				{
					Find.TutorialState.AddStartingItem(thing2);
				}
			}
			else
			{
				GenSpawn.Spawn(thing, loc, map, rot, WipeMode.Vanish, false, false);
			}
			if (this.filthDef != null)
			{
				foreach (IntVec3 c2 in thing.OccupiedRect().ExpandedBy(this.filthExpandBy))
				{
					if (Rand.Chance(this.filthChance) && c2.InBounds(thing.Map))
					{
						FilthMaker.TryMakeFilth(c2, thing.Map, this.filthDef, 1, FilthSourceFlags.None, true);
					}
				}
			}
			if (this.createUsedRect)
			{
				MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects").Add(thing.OccupiedRect());
			}
		}

		// Token: 0x06001F37 RID: 7991 RVA: 0x000A40CC File Offset: 0x000A22CC
		protected override bool CanScatterAt(IntVec3 loc, Map map)
		{
			if (!base.CanScatterAt(loc, map))
			{
				return false;
			}
			if (!GenSpawn.CanSpawnAt(this.thingDef, loc, map, null, true))
			{
				return false;
			}
			Rot4 rot;
			if (!this.TryGetRandomValidRotation(loc, map, out rot))
			{
				return false;
			}
			if (this.terrainValidationRadius > 0f)
			{
				foreach (IntVec3 c in GenRadial.RadialCellsAround(loc, this.terrainValidationRadius, true))
				{
					if (c.InBounds(map))
					{
						TerrainDef terrain = c.GetTerrain(map);
						if (this.terrainValidationDisallowed != null)
						{
							foreach (string tag in this.terrainValidationDisallowed)
							{
								if (terrain.HasTag(tag))
								{
									return false;
								}
							}
						}
						if (this.terrainValidationAllowed != null)
						{
							foreach (string tag2 in this.terrainValidationAllowed)
							{
								if (!terrain.HasTag(tag2))
								{
									return false;
								}
							}
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06001F38 RID: 7992 RVA: 0x000A4228 File Offset: 0x000A2428
		private bool TryGetRandomValidRotation(IntVec3 loc, Map map, out Rot4 rot)
		{
			List<Rot4> possibleRotations = this.PossibleRotations;
			for (int i = 0; i < possibleRotations.Count; i++)
			{
				if (this.IsRotationValid(loc, possibleRotations[i], map))
				{
					GenStep_ScatterThings.tmpRotations.Add(possibleRotations[i]);
				}
			}
			if (GenStep_ScatterThings.tmpRotations.TryRandomElement(out rot))
			{
				GenStep_ScatterThings.tmpRotations.Clear();
				return true;
			}
			rot = Rot4.Invalid;
			return false;
		}

		// Token: 0x06001F39 RID: 7993 RVA: 0x000A4294 File Offset: 0x000A2494
		private bool IsRotationValid(IntVec3 loc, Rot4 rot, Map map)
		{
			CellRect cellRect = GenAdj.OccupiedRect(loc, rot, this.thingDef.size);
			if (!cellRect.InBounds(map))
			{
				return false;
			}
			if (this.avoidUsedRects && MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects").Any((CellRect ur) => ur.Overlaps(cellRect)))
			{
				return false;
			}
			if (this.thingDef.hasInteractionCell)
			{
				IntVec3 c2 = loc + this.thingDef.interactionCellOffset.RotatedBy(rot);
				if (!c2.InBounds(map) || !c2.Walkable(map))
				{
					return false;
				}
			}
			return !GenSpawn.WouldWipeAnythingWith(loc, rot, this.thingDef, map, (Thing x) => x.def == this.thingDef || (x.def.category != ThingCategory.Plant && x.def.category != ThingCategory.Filth)) && (!this.skipRoofed || !cellRect.Any((IntVec3 c) => c.Roofed(map))) && this.thingDef.CanSpawnAt(loc, rot, map);
		}

		// Token: 0x06001F3A RID: 7994 RVA: 0x000A43B0 File Offset: 0x000A25B0
		public static List<int> CountDividedIntoStacks(int count, IntRange stackSizeRange)
		{
			List<int> list = new List<int>();
			while (count > 0)
			{
				int num = Mathf.Min(count, stackSizeRange.RandomInRange);
				count -= num;
				list.Add(num);
			}
			if (stackSizeRange.max > 2)
			{
				for (int i = 0; i < list.Count * 4; i++)
				{
					int num2 = Rand.RangeInclusive(0, list.Count - 1);
					int num3 = Rand.RangeInclusive(0, list.Count - 1);
					if (num2 != num3 && list[num2] > list[num3])
					{
						int num4 = (int)((float)(list[num2] - list[num3]) * Rand.Value);
						List<int> list2 = list;
						int index = num2;
						list2[index] -= num4;
						list2 = list;
						index = num3;
						list2[index] += num4;
					}
				}
			}
			return list;
		}

		// Token: 0x04001ADF RID: 6879
		public ThingDef thingDef;

		// Token: 0x04001AE0 RID: 6880
		public ThingDef stuff;

		// Token: 0x04001AE1 RID: 6881
		public int clearSpaceSize;

		// Token: 0x04001AE2 RID: 6882
		public int clusterSize = 1;

		// Token: 0x04001AE3 RID: 6883
		public QualityCategory? quality;

		// Token: 0x04001AE4 RID: 6884
		public bool minify;

		// Token: 0x04001AE5 RID: 6885
		public bool avoidUsedRects;

		// Token: 0x04001AE6 RID: 6886
		public bool createUsedRect;

		// Token: 0x04001AE7 RID: 6887
		public bool skipRoofed;

		// Token: 0x04001AE8 RID: 6888
		public ThingDef filthDef;

		// Token: 0x04001AE9 RID: 6889
		public int filthExpandBy;

		// Token: 0x04001AEA RID: 6890
		public float filthChance = 0.5f;

		// Token: 0x04001AEB RID: 6891
		public float terrainValidationRadius;

		// Token: 0x04001AEC RID: 6892
		[NoTranslate]
		private List<string> terrainValidationDisallowed;

		// Token: 0x04001AED RID: 6893
		[NoTranslate]
		private List<string> terrainValidationAllowed;

		// Token: 0x04001AEE RID: 6894
		[Unsaved(false)]
		private IntVec3 clusterCenter;

		// Token: 0x04001AEF RID: 6895
		[Unsaved(false)]
		private int leftInCluster;

		// Token: 0x04001AF0 RID: 6896
		private const int ClusterRadius = 4;

		// Token: 0x04001AF1 RID: 6897
		private List<Rot4> possibleRotationsInt;

		// Token: 0x04001AF2 RID: 6898
		private static List<Rot4> tmpRotations = new List<Rot4>();
	}
}
