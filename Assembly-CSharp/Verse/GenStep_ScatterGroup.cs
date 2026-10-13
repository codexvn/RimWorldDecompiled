using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200049D RID: 1181
	public class GenStep_ScatterGroup : GenStep_Scatterer
	{
		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06001F04 RID: 7940 RVA: 0x000A2144 File Offset: 0x000A0344
		public override int SeedPart
		{
			get
			{
				return 1237834582;
			}
		}

		// Token: 0x06001F05 RID: 7941 RVA: 0x000A214C File Offset: 0x000A034C
		public override void Generate(Map map, GenStepParams parms)
		{
			try
			{
				base.Generate(map, parms);
			}
			finally
			{
				this.dontChooseIndoor = false;
			}
		}

		// Token: 0x06001F06 RID: 7942 RVA: 0x000A217C File Offset: 0x000A037C
		private bool IndoorRuinSpot(CellRect rect, Map map)
		{
			float sqrMagnitude = ((new Vector2((float)rect.minX, (float)rect.minZ) - new Vector2((float)rect.maxX, (float)rect.maxZ)) * 2f).sqrMagnitude;
			if (rect.CenterCell.GetTerrain(map).BuildableByPlayer)
			{
				using (List<Thing>.Enumerator enumerator = map.listerThings.ThingsOfDef(ThingDefOf.Wall).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if ((float)enumerator.Current.Position.DistanceToSquared(rect.CenterCell) < sqrMagnitude)
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06001F07 RID: 7943 RVA: 0x000A2240 File Offset: 0x000A0440
		private GenStep_ScatterGroup.ScatterGroup GetGroup(Map map)
		{
			GenStep_ScatterGroup.tmpScatterGroups.Clear();
			foreach (GenStep_ScatterGroup.ScatterGroup scatterGroup in this.groups)
			{
				if (!this.dontChooseIndoor || !scatterGroup.indoorRuin)
				{
					GenStep_ScatterGroup.tmpScatterGroups.Add(scatterGroup);
				}
			}
			Rand.PushState(Gen.HashCombineInt(Find.World.info.Seed, this.usedSpots.Count));
			GenStep_ScatterGroup.ScatterGroup result;
			try
			{
				result = GenStep_ScatterGroup.tmpScatterGroups.RandomElementByWeight((GenStep_ScatterGroup.ScatterGroup g) => g.weight);
			}
			finally
			{
				Rand.PopState();
			}
			return result;
		}

		// Token: 0x06001F08 RID: 7944 RVA: 0x000A2314 File Offset: 0x000A0514
		protected override bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			bool warnOnFail = this.warnOnFail;
			this.warnOnFail = false;
			try
			{
				this.dontChooseIndoor = false;
				if (this.GetGroup(map).indoorRuin)
				{
					if (base.TryFindScatterCell(map, out result))
					{
						return true;
					}
					this.dontChooseIndoor = true;
				}
				if (this.GetGroup(map) != null && base.TryFindScatterCell(map, out result))
				{
					return true;
				}
			}
			finally
			{
				this.warnOnFail = warnOnFail;
			}
			if (this.warnOnFail)
			{
				Log.Warning(string.Concat(new string[]
				{
					"Scatterer ",
					this.ToString(),
					" from def ",
					this.def.defName,
					" could not find cell to generate at."
				}));
			}
			result = default(IntVec3);
			return false;
		}

		// Token: 0x06001F09 RID: 7945 RVA: 0x000A23DC File Offset: 0x000A05DC
		private bool CalculateScatterInformation(IntVec3 loc, Map map, out CellRect rect, List<GenStep_ScatterGroup.ThingSpawn> outThingSpawns)
		{
			GenStep_ScatterGroup.<>c__DisplayClass17_0 CS$<>8__locals1;
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.outThingSpawns = outThingSpawns;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.loc = loc;
			GenStep_ScatterGroup.tmpWeightedThings.Clear();
			GenStep_ScatterGroup.ScatterGroup group = this.GetGroup(CS$<>8__locals1.map);
			rect = CellRect.CenteredOn(CS$<>8__locals1.loc, group.clusterRectRadius.RandomInRange);
			CS$<>8__locals1.localRect = rect;
			if (!rect.InBounds(CS$<>8__locals1.map))
			{
				return false;
			}
			if (group.indoorRuin != this.IndoorRuinSpot(rect, CS$<>8__locals1.map))
			{
				return false;
			}
			if (!group.things.NullOrEmpty<GenStep_ScatterGroup.ThingWeight>())
			{
				GenStep_ScatterGroup.tmpWeightedThings.AddRange(group.things);
			}
			if (group.spawnAtCenter != null)
			{
				if (!this.<CalculateScatterInformation>g__CanSpawn|17_0(group.spawnAtCenter, rect.CenterCell, Rot4.North, GenAdj.OccupiedRect(rect.CenterCell, Rot4.North, group.spawnAtCenter.size), ref CS$<>8__locals1))
				{
					return false;
				}
				CS$<>8__locals1.outThingSpawns.Add(new GenStep_ScatterGroup.ThingSpawn
				{
					def = group.spawnAtCenter,
					occupiedRect = GenAdj.OccupiedRect(rect.CenterCell, Rot4.North, group.spawnAtCenter.size),
					pos = rect.CenterCell,
					rotation = Rot4.North,
					sourceGroup = group,
					stackSize = 1,
					spawnForbidden = false
				});
				if (group.spawnAtCenterFilthDef != null)
				{
					CS$<>8__locals1.outThingSpawns.Add(new GenStep_ScatterGroup.ThingSpawn
					{
						def = group.spawnAtCenterFilthDef,
						occupiedRect = GenAdj.OccupiedRect(rect.CenterCell, Rot4.North, group.spawnAtCenterFilthDef.size),
						pos = rect.CenterCell,
						rotation = Rot4.North,
						sourceGroup = group
					});
				}
			}
			int num = 0;
			foreach (IntVec3 intVec in rect)
			{
				bool flag = false;
				foreach (GenStep_ScatterGroup.ThingWeight thingWeight in GenStep_ScatterGroup.tmpWeightedThings)
				{
					GenStep_ScatterGroup.<CalculateScatterInformation>g__WritePossibleRotations|17_1(thingWeight.thing);
					foreach (Rot4 rot in GenStep_ScatterGroup.tmpPossibleRotations)
					{
						if (this.<CalculateScatterInformation>g__CanSpawn|17_0(thingWeight.thing, intVec, rot, GenAdj.OccupiedRect(intVec, rot, thingWeight.thing.size), ref CS$<>8__locals1))
						{
							num++;
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			FloatRange coveredCellsPer10Cells = group.coveredCellsPer10Cells;
			int num2 = (int)((float)num / 10f * coveredCellsPer10Cells.TrueMin);
			int num3 = (int)((float)num / 10f * coveredCellsPer10Cells.TrueMax);
			int num4 = (int)((float)num / 10f * coveredCellsPer10Cells.RandomInRange);
			int i = 0;
			while (i < num4)
			{
				GenStep_ScatterGroup.tmpWeightedThingsRandom.Clear();
				foreach (GenStep_ScatterGroup.ThingWeight thingWeight2 in GenStep_ScatterGroup.tmpWeightedThings)
				{
					GenStep_ScatterGroup.tmpWeightedThingsRandom.Add(thingWeight2, thingWeight2.weight * Rand.Value);
				}
				GenStep_ScatterGroup.tmpWeightedThings.SortByDescending((GenStep_ScatterGroup.ThingWeight e) => GenStep_ScatterGroup.tmpWeightedThingsRandom[e]);
				bool flag2 = false;
				foreach (GenStep_ScatterGroup.ThingWeight thingWeight3 in GenStep_ScatterGroup.tmpWeightedThings)
				{
					if (i + thingWeight3.thing.size.Area > num3)
					{
						foreach (GenStep_ScatterGroup.ThingWeight thingWeight4 in GenStep_ScatterGroup.tmpWeightedThings)
						{
							if (thingWeight4 != thingWeight3 && i + thingWeight4.thing.size.Area <= num3)
							{
								break;
							}
						}
					}
					GenStep_ScatterGroup.<CalculateScatterInformation>g__WritePossibleRotations|17_1(thingWeight3.thing);
					foreach (IntVec3 intVec2 in rect.Cells.InRandomOrder(GenStep_ScatterGroup.tmpCellsRandomOrderWorkingList))
					{
						if (i + thingWeight3.thing.size.Area > num3)
						{
							break;
						}
						foreach (Rot4 rot2 in GenStep_ScatterGroup.tmpPossibleRotations)
						{
							CellRect occupiedRect = GenAdj.OccupiedRect(intVec2, rot2, thingWeight3.thing.size);
							if (this.<CalculateScatterInformation>g__CanSpawn|17_0(thingWeight3.thing, intVec2, rot2, occupiedRect, ref CS$<>8__locals1))
							{
								CS$<>8__locals1.outThingSpawns.Add(new GenStep_ScatterGroup.ThingSpawn
								{
									def = thingWeight3.thing,
									occupiedRect = occupiedRect,
									pos = intVec2,
									rotation = rot2,
									sourceGroup = group,
									stackSize = group.stackSizeRange.RandomInRange,
									spawnForbidden = group.spawnForbidden
								});
								i += occupiedRect.Area;
								flag2 = true;
								break;
							}
						}
						if (flag2)
						{
							break;
						}
					}
					if (flag2)
					{
						break;
					}
				}
				if (!flag2)
				{
					break;
				}
			}
			return i >= num2;
		}

		// Token: 0x06001F0A RID: 7946 RVA: 0x000A2A24 File Offset: 0x000A0C24
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			using (Rand.Block(map.NextGenSeed))
			{
				try
				{
					CellRect cellRect;
					if (!this.CalculateScatterInformation(loc, map, out cellRect, GenStep_ScatterGroup.tmpSpawns) && this.warnOnFail)
					{
						Log.Warning(string.Format("Tried scattering group from def {0} at {1} on map {2} which is an invalid location!", this.def.defName, loc, map));
					}
					else
					{
						foreach (GenStep_ScatterGroup.ThingSpawn thingSpawn in GenStep_ScatterGroup.tmpSpawns)
						{
							if (thingSpawn.def.IsFilth)
							{
								FilthMaker.TryMakeFilth(thingSpawn.pos, map, thingSpawn.def, 1, FilthSourceFlags.None, true);
							}
							else if (GenSpawn.CanSpawnAt(thingSpawn.def, thingSpawn.pos, map, new Rot4?(thingSpawn.rotation), true))
							{
								Thing thing = ThingMaker.MakeThing(thingSpawn.def, null);
								thing.stackCount = thingSpawn.stackSize;
								GenSpawn.Spawn(thing, thingSpawn.pos, map, thingSpawn.rotation, WipeMode.Vanish, false, false);
								if (thingSpawn.spawnForbidden && thing.HasComp<CompForbiddable>())
								{
									thing.SetForbidden(true, true);
								}
							}
							if (thingSpawn.sourceGroup.filthDef != null && !thingSpawn.def.IsFilth)
							{
								cellRect = thingSpawn.occupiedRect.ExpandedBy(thingSpawn.sourceGroup.filthExpandBy);
								foreach (IntVec3 c in cellRect)
								{
									if (Rand.Chance(thingSpawn.sourceGroup.filthChance) && c.InBounds(map))
									{
										FilthMaker.TryMakeFilth(c, map, thingSpawn.sourceGroup.filthDef, 1, FilthSourceFlags.None, true);
									}
								}
							}
						}
					}
				}
				finally
				{
					GenStep_ScatterGroup.tmpSpawns.Clear();
				}
			}
		}

		// Token: 0x06001F0B RID: 7947 RVA: 0x000A2C54 File Offset: 0x000A0E54
		protected override bool CanScatterAt(IntVec3 loc, Map map)
		{
			if (!base.CanScatterAt(loc, map))
			{
				return false;
			}
			bool result;
			using (Rand.Block(map.NextGenSeed))
			{
				try
				{
					CellRect cellRect;
					result = this.CalculateScatterInformation(loc, map, out cellRect, GenStep_ScatterGroup.tmpSpawns);
				}
				finally
				{
					GenStep_ScatterGroup.tmpSpawns.Clear();
				}
			}
			return result;
		}

		// Token: 0x06001F0E RID: 7950 RVA: 0x000A2D0C File Offset: 0x000A0F0C
		[CompilerGenerated]
		private bool <CalculateScatterInformation>g__CanSpawn|17_0(ThingDef def, IntVec3 cell, Rot4 rot, CellRect occupiedRect, ref GenStep_ScatterGroup.<>c__DisplayClass17_0 A_5)
		{
			if (!occupiedRect.InBounds(A_5.map) || !occupiedRect.FullyContainedWithin(A_5.localRect))
			{
				return false;
			}
			foreach (GenStep_ScatterGroup.ThingSpawn thingSpawn in A_5.outThingSpawns)
			{
				CellRect occupiedRect2 = thingSpawn.occupiedRect;
				if (occupiedRect2.Overlaps(occupiedRect))
				{
					return false;
				}
			}
			if (GenSpawn.WouldWipeAnythingWith(cell, rot, def, A_5.map, (Thing x) => x.def == def || (x.def.category != ThingCategory.Plant && x.def.category != ThingCategory.Filth)))
			{
				return false;
			}
			foreach (IntVec3 c in occupiedRect)
			{
				if (this.useFallback)
				{
					if (this.fallbackValidators != null)
					{
						for (int i = 0; i < this.fallbackValidators.Count; i++)
						{
							if (!this.fallbackValidators[i].Allows(A_5.loc, A_5.map))
							{
								return false;
							}
						}
					}
				}
				else if (this.validators != null)
				{
					for (int j = 0; j < this.validators.Count; j++)
					{
						if (!this.validators[j].Allows(A_5.loc, A_5.map))
						{
							return false;
						}
					}
				}
				foreach (Thing thing in c.GetThingList(A_5.map))
				{
					if (thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Pawn || thing.def.IsEdifice() || !thing.def.destroyable)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001F0F RID: 7951 RVA: 0x000A2F58 File Offset: 0x000A1158
		[CompilerGenerated]
		internal static void <CalculateScatterInformation>g__WritePossibleRotations|17_1(ThingDef thing)
		{
			GenStep_ScatterGroup.tmpPossibleRotations.Clear();
			if (thing.rotatable)
			{
				GenStep_ScatterGroup.tmpPossibleRotations.Add(Rot4.North);
				GenStep_ScatterGroup.tmpPossibleRotations.Add(Rot4.East);
				GenStep_ScatterGroup.tmpPossibleRotations.Add(Rot4.South);
				GenStep_ScatterGroup.tmpPossibleRotations.Add(Rot4.West);
			}
			else
			{
				GenStep_ScatterGroup.tmpPossibleRotations.Add(Rot4.North);
			}
			GenStep_ScatterGroup.tmpPossibleRotations.Shuffle<Rot4>();
		}

		// Token: 0x04001A97 RID: 6807
		public List<GenStep_ScatterGroup.ScatterGroup> groups;

		// Token: 0x04001A98 RID: 6808
		[Unsaved(false)]
		private bool dontChooseIndoor;

		// Token: 0x04001A99 RID: 6809
		private static List<GenStep_ScatterGroup.ThingSpawn> tmpSpawns = new List<GenStep_ScatterGroup.ThingSpawn>();

		// Token: 0x04001A9A RID: 6810
		private static List<GenStep_ScatterGroup.ScatterGroup> tmpScatterGroups = new List<GenStep_ScatterGroup.ScatterGroup>();

		// Token: 0x04001A9B RID: 6811
		private static List<GenStep_ScatterGroup.ThingWeight> tmpWeightedThings = new List<GenStep_ScatterGroup.ThingWeight>();

		// Token: 0x04001A9C RID: 6812
		private static Dictionary<GenStep_ScatterGroup.ThingWeight, float> tmpWeightedThingsRandom = new Dictionary<GenStep_ScatterGroup.ThingWeight, float>();

		// Token: 0x04001A9D RID: 6813
		private static List<IntVec3> tmpCellsRandomOrderWorkingList = new List<IntVec3>();

		// Token: 0x04001A9E RID: 6814
		private static List<Rot4> tmpPossibleRotations = new List<Rot4>();

		// Token: 0x0200049E RID: 1182
		private struct ThingSpawn
		{
			// Token: 0x04001A9F RID: 6815
			public IntVec3 pos;

			// Token: 0x04001AA0 RID: 6816
			public Rot4 rotation;

			// Token: 0x04001AA1 RID: 6817
			public CellRect occupiedRect;

			// Token: 0x04001AA2 RID: 6818
			public ThingDef def;

			// Token: 0x04001AA3 RID: 6819
			public int stackSize;

			// Token: 0x04001AA4 RID: 6820
			public bool spawnForbidden;

			// Token: 0x04001AA5 RID: 6821
			public GenStep_ScatterGroup.ScatterGroup sourceGroup;
		}

		// Token: 0x0200049F RID: 1183
		public class ThingWeight
		{
			// Token: 0x06001F10 RID: 7952 RVA: 0x000A2FCE File Offset: 0x000A11CE
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				XmlHelper.ParseElements(this, xmlRoot, "thing", "weight");
			}

			// Token: 0x04001AA6 RID: 6822
			public ThingDef thing;

			// Token: 0x04001AA7 RID: 6823
			public float weight;
		}

		// Token: 0x020004A0 RID: 1184
		public class ScatterGroup
		{
			// Token: 0x04001AA8 RID: 6824
			public List<GenStep_ScatterGroup.ThingWeight> things;

			// Token: 0x04001AA9 RID: 6825
			public IntRange stackSizeRange = IntRange.One;

			// Token: 0x04001AAA RID: 6826
			public bool spawnForbidden;

			// Token: 0x04001AAB RID: 6827
			public float weight = 1f;

			// Token: 0x04001AAC RID: 6828
			public ThingDef spawnAtCenter;

			// Token: 0x04001AAD RID: 6829
			public ThingDef spawnAtCenterFilthDef;

			// Token: 0x04001AAE RID: 6830
			public bool indoorRuin;

			// Token: 0x04001AAF RID: 6831
			public FloatRange coveredCellsPer10Cells;

			// Token: 0x04001AB0 RID: 6832
			public ThingDef filthDef;

			// Token: 0x04001AB1 RID: 6833
			public int filthExpandBy;

			// Token: 0x04001AB2 RID: 6834
			public float filthChance = 0.5f;

			// Token: 0x04001AB3 RID: 6835
			public IntRange clusterRectRadius;
		}
	}
}
