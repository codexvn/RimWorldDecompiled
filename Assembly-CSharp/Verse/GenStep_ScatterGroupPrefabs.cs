using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using RimWorld;
using Verse.Utility;

namespace Verse
{
	// Token: 0x020004A5 RID: 1189
	public class GenStep_ScatterGroupPrefabs : GenStep_Scatterer
	{
		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06001F1A RID: 7962 RVA: 0x000A307C File Offset: 0x000A127C
		public override int SeedPart
		{
			get
			{
				return 79797732;
			}
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x000A3084 File Offset: 0x000A1284
		public override void Generate(Map map, GenStepParams parms)
		{
			try
			{
				base.Generate(map, parms);
			}
			finally
			{
				this.dontChooseIndoor = false;
				GenStep_ScatterGroupPrefabs.tmpSpawns.Clear();
			}
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x000A30C0 File Offset: 0x000A12C0
		private GenStep_ScatterGroupPrefabs.ScatterGroup GetGroup()
		{
			GenStep_ScatterGroupPrefabs.tmpScatterGroups.Clear();
			using (List<GenStep_ScatterGroupPrefabs.ScatterGroup>.Enumerator enumerator = this.groups.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GenStep_ScatterGroupPrefabs.ScatterGroup group = enumerator.Current;
					if ((!this.dontChooseIndoor || !group.indoorRuin) && (group.max <= 0 || GenStep_ScatterGroupPrefabs.tmpSpawns.Count((GenStep_ScatterGroupPrefabs.PrefabSpawn spawn) => spawn.def == group.prefab) < group.max))
					{
						GenStep_ScatterGroupPrefabs.tmpScatterGroups.Add(group);
					}
				}
			}
			if (GenStep_ScatterGroupPrefabs.tmpScatterGroups.Empty<GenStep_ScatterGroupPrefabs.ScatterGroup>())
			{
				return null;
			}
			Rand.PushState(Gen.HashCombineInt(Find.World.info.Seed, this.usedSpots.Count));
			GenStep_ScatterGroupPrefabs.ScatterGroup result;
			try
			{
				result = GenStep_ScatterGroupPrefabs.tmpScatterGroups.RandomElementByWeight((GenStep_ScatterGroupPrefabs.ScatterGroup g) => g.weight);
			}
			finally
			{
				Rand.PopState();
			}
			return result;
		}

		// Token: 0x06001F1D RID: 7965 RVA: 0x000A31E8 File Offset: 0x000A13E8
		protected override bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			bool warnOnFail = this.warnOnFail;
			this.warnOnFail = false;
			try
			{
				this.dontChooseIndoor = false;
				GenStep_ScatterGroupPrefabs.ScatterGroup group = this.GetGroup();
				if (group == null)
				{
					result = default(IntVec3);
					return false;
				}
				if (group.indoorRuin)
				{
					if (base.TryFindScatterCell(map, out result))
					{
						return true;
					}
					this.dontChooseIndoor = true;
				}
				if (this.GetGroup() != null && base.TryFindScatterCell(map, out result))
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
				string[] array = new string[5];
				array[0] = "Scatterer ";
				array[1] = this.ToString();
				array[2] = " from def ";
				int num = 3;
				GenStepDef def = this.def;
				array[num] = ((def != null) ? def.defName : null);
				array[4] = " could not find cell to generate at.";
				Log.Warning(string.Concat(array));
			}
			result = default(IntVec3);
			return false;
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x000A32C8 File Offset: 0x000A14C8
		private bool CalculateScatterInformation(IntVec3 loc, Map map, out GenStep_ScatterGroupPrefabs.PrefabSpawn details)
		{
			GenStep_ScatterGroupPrefabs.<>c__DisplayClass14_0 CS$<>8__locals1;
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.loc = loc;
			CS$<>8__locals1.usedRects = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			GenStep_ScatterGroupPrefabs.tmpScatterGroups.Clear();
			GenStep_ScatterGroupPrefabs.ScatterGroup group = this.GetGroup();
			if (group == null)
			{
				details = default(GenStep_ScatterGroupPrefabs.PrefabSpawn);
				return false;
			}
			bool flag = false;
			foreach (GenStep_ScatterGroupPrefabs.ScatterGroup scatterGroup in GenStep_ScatterGroupPrefabs.tmpScatterGroups)
			{
				GenStep_ScatterGroupPrefabs.<CalculateScatterInformation>g__WritePossibleRotations|14_1(scatterGroup.prefab);
				foreach (Rot4 rot in GenStep_ScatterGroupPrefabs.tmpPossibleRotations)
				{
					if (this.<CalculateScatterInformation>g__CanSpawn|14_0(scatterGroup.prefab, CS$<>8__locals1.loc, rot, ref CS$<>8__locals1))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			GenStep_ScatterGroupPrefabs.tmpWeightedPrefabsRandom.Clear();
			foreach (GenStep_ScatterGroupPrefabs.ScatterGroup scatterGroup2 in GenStep_ScatterGroupPrefabs.tmpScatterGroups)
			{
				GenStep_ScatterGroupPrefabs.tmpWeightedPrefabsRandom.Add(scatterGroup2, scatterGroup2.weight * Rand.Value);
			}
			GenStep_ScatterGroupPrefabs.tmpScatterGroups.SortByDescending((GenStep_ScatterGroupPrefabs.ScatterGroup e) => GenStep_ScatterGroupPrefabs.tmpWeightedPrefabsRandom[e]);
			CellRect cellRect = CS$<>8__locals1.loc.RectAbout(new IntVec2(20, 20)).ClipInsideMap(CS$<>8__locals1.map);
			using (List<GenStep_ScatterGroupPrefabs.ScatterGroup>.Enumerator enumerator = GenStep_ScatterGroupPrefabs.tmpScatterGroups.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GenStep_ScatterGroupPrefabs.ScatterGroup t = enumerator.Current;
					if (t.max <= 0 || GenStep_ScatterGroupPrefabs.tmpSpawns.Count((GenStep_ScatterGroupPrefabs.PrefabSpawn spawn) => spawn.def == t.prefab) < t.max)
					{
						GenStep_ScatterGroupPrefabs.<CalculateScatterInformation>g__WritePossibleRotations|14_1(t.prefab);
						foreach (IntVec3 intVec in cellRect.Cells.InRandomOrder(GenStep_ScatterGroupPrefabs.tmpCellsRandomOrderWorkingList))
						{
							foreach (Rot4 rot2 in GenStep_ScatterGroupPrefabs.tmpPossibleRotations)
							{
								CellRect occupiedRect = GenAdj.OccupiedRect(intVec, rot2, t.prefab.size);
								if (this.<CalculateScatterInformation>g__CanSpawn|14_0(t.prefab, intVec, rot2, ref CS$<>8__locals1))
								{
									details = new GenStep_ScatterGroupPrefabs.PrefabSpawn
									{
										def = t.prefab,
										occupiedRect = occupiedRect,
										pos = intVec,
										rotation = rot2,
										sourceGroup = group
									};
									return true;
								}
							}
						}
					}
				}
			}
			details = default(GenStep_ScatterGroupPrefabs.PrefabSpawn);
			return false;
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x000A366C File Offset: 0x000A186C
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			using (Rand.Block(map.NextGenSeed))
			{
				GenStep_ScatterGroupPrefabs.PrefabSpawn prefabSpawn;
				if (!this.CalculateScatterInformation(loc, map, out prefabSpawn) && this.warnOnFail)
				{
					Log.Warning(string.Format("Tried scattering group from def {0} at {1} on map {2} which is an invalid location!", this.def.defName, loc, map));
				}
				else
				{
					if (PrefabUtility.CanSpawnPrefab(prefabSpawn.def, map, prefabSpawn.pos, prefabSpawn.rotation, true, 0))
					{
						PrefabUtility.SpawnPrefab(prefabSpawn.def, map, prefabSpawn.pos, prefabSpawn.rotation, null, null, null, null, false);
						GenStep_ScatterGroupPrefabs.tmpSpawns.Add(prefabSpawn);
					}
					if (prefabSpawn.sourceGroup.filthDef != null)
					{
						foreach (IntVec3 c in prefabSpawn.occupiedRect.ExpandedBy(prefabSpawn.sourceGroup.filthExpandBy))
						{
							if (Rand.Chance(prefabSpawn.sourceGroup.filthChance) && c.InBounds(map))
							{
								FilthMaker.TryMakeFilth(c, map, prefabSpawn.sourceGroup.filthDef, 1, FilthSourceFlags.None, true);
							}
						}
					}
				}
			}
		}

		// Token: 0x06001F20 RID: 7968 RVA: 0x000A37D0 File Offset: 0x000A19D0
		protected override bool CanScatterAt(IntVec3 loc, Map map)
		{
			if (!base.CanScatterAt(loc, map))
			{
				return false;
			}
			bool result;
			using (Rand.Block(map.NextGenSeed))
			{
				GenStep_ScatterGroupPrefabs.PrefabSpawn prefabSpawn;
				result = this.CalculateScatterInformation(loc, map, out prefabSpawn);
			}
			return result;
		}

		// Token: 0x06001F23 RID: 7971 RVA: 0x000A3858 File Offset: 0x000A1A58
		[CompilerGenerated]
		private bool <CalculateScatterInformation>g__CanSpawn|14_0(PrefabDef prefab, IntVec3 cell, Rot4 rot, ref GenStep_ScatterGroupPrefabs.<>c__DisplayClass14_0 A_4)
		{
			CellRect other = GenAdj.OccupiedRect(cell, rot, prefab.size);
			if (!other.InBounds(A_4.map))
			{
				return false;
			}
			foreach (GenStep_ScatterGroupPrefabs.PrefabSpawn prefabSpawn in GenStep_ScatterGroupPrefabs.tmpSpawns)
			{
				CellRect occupiedRect = prefabSpawn.occupiedRect;
				if (occupiedRect.Overlaps(other))
				{
					return false;
				}
			}
			foreach (CellRect cellRect in A_4.usedRects)
			{
				if (cellRect.Overlaps(other))
				{
					return false;
				}
			}
			foreach (IntVec3 c in other)
			{
				if (!c.GetAffordances(A_4.map).Contains(TerrainAffordanceDefOf.Heavy))
				{
					return false;
				}
				if (this.useFallback)
				{
					if (this.fallbackValidators != null)
					{
						for (int i = 0; i < this.fallbackValidators.Count; i++)
						{
							if (!this.fallbackValidators[i].Allows(A_4.loc, A_4.map))
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
						if (!this.validators[j].Allows(A_4.loc, A_4.map))
						{
							return false;
						}
					}
				}
				foreach (Thing thing in c.GetThingList(A_4.map))
				{
					if (thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Pawn || thing.def.IsEdifice() || !thing.def.destroyable)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001F24 RID: 7972 RVA: 0x000A3AEC File Offset: 0x000A1CEC
		[CompilerGenerated]
		internal static void <CalculateScatterInformation>g__WritePossibleRotations|14_1(PrefabDef prefab)
		{
			GenStep_ScatterGroupPrefabs.tmpPossibleRotations.Clear();
			foreach (Enum @enum in prefab.rotations.GetBitFlags())
			{
				RotEnum rot = (RotEnum)@enum;
				GenStep_ScatterGroupPrefabs.tmpPossibleRotations.Add(rot);
			}
			GenStep_ScatterGroupPrefabs.tmpPossibleRotations.Shuffle<Rot4>();
		}

		// Token: 0x04001AC1 RID: 6849
		public List<GenStep_ScatterGroupPrefabs.ScatterGroup> groups;

		// Token: 0x04001AC2 RID: 6850
		[Unsaved(false)]
		private bool dontChooseIndoor;

		// Token: 0x04001AC3 RID: 6851
		private static readonly List<GenStep_ScatterGroupPrefabs.PrefabSpawn> tmpSpawns = new List<GenStep_ScatterGroupPrefabs.PrefabSpawn>();

		// Token: 0x04001AC4 RID: 6852
		private static readonly List<GenStep_ScatterGroupPrefabs.ScatterGroup> tmpScatterGroups = new List<GenStep_ScatterGroupPrefabs.ScatterGroup>();

		// Token: 0x04001AC5 RID: 6853
		private static readonly Dictionary<GenStep_ScatterGroupPrefabs.ScatterGroup, float> tmpWeightedPrefabsRandom = new Dictionary<GenStep_ScatterGroupPrefabs.ScatterGroup, float>();

		// Token: 0x04001AC6 RID: 6854
		private static readonly List<IntVec3> tmpCellsRandomOrderWorkingList = new List<IntVec3>();

		// Token: 0x04001AC7 RID: 6855
		private static readonly List<Rot4> tmpPossibleRotations = new List<Rot4>();

		// Token: 0x020004A6 RID: 1190
		private struct PrefabSpawn
		{
			// Token: 0x04001AC8 RID: 6856
			public IntVec3 pos;

			// Token: 0x04001AC9 RID: 6857
			public Rot4 rotation;

			// Token: 0x04001ACA RID: 6858
			public CellRect occupiedRect;

			// Token: 0x04001ACB RID: 6859
			public PrefabDef def;

			// Token: 0x04001ACC RID: 6860
			public GenStep_ScatterGroupPrefabs.ScatterGroup sourceGroup;
		}

		// Token: 0x020004A7 RID: 1191
		public class ScatterGroup
		{
			// Token: 0x06001F25 RID: 7973 RVA: 0x000A3B68 File Offset: 0x000A1D68
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				XmlHelper.ParseElements(this, xmlRoot, "prefab", "weight");
			}

			// Token: 0x04001ACD RID: 6861
			public PrefabDef prefab;

			// Token: 0x04001ACE RID: 6862
			public float weight = 1f;

			// Token: 0x04001ACF RID: 6863
			public int max = -1;

			// Token: 0x04001AD0 RID: 6864
			public bool indoorRuin;

			// Token: 0x04001AD1 RID: 6865
			public ThingDef filthDef;

			// Token: 0x04001AD2 RID: 6866
			public int filthExpandBy;

			// Token: 0x04001AD3 RID: 6867
			public float filthChance = 0.5f;
		}
	}
}
