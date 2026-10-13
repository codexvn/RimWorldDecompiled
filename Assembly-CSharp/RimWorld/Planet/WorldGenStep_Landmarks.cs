using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003CF5 RID: 15605
	public class WorldGenStep_Landmarks : WorldGenStep
	{
		// Token: 0x170034ED RID: 13549
		// (get) Token: 0x060153BE RID: 86974 RVA: 0x0065A02F File Offset: 0x0065822F
		public override int SeedPart
		{
			get
			{
				return 471293987;
			}
		}

		// Token: 0x060153BF RID: 86975 RVA: 0x0065A038 File Offset: 0x00658238
		public override void GenerateFresh(string seed, PlanetLayer layer)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			WorldGenStep_Landmarks.landmarkCandidates.Clear();
			WorldGenStep_Landmarks.usedTiles.Clear();
			using (ProfilerBlock.Scope("Generate Landmarks"))
			{
				float num = 0.0022222223f * Find.World.info.landmarkDensity.GetScaleFactor();
				int num2 = Mathf.RoundToInt((float)layer.TilesCount * num);
				int num3 = 0;
				List<LandmarkDef> list = DefDatabase<LandmarkDef>.AllDefsListForReading.ToList<LandmarkDef>();
				List<int> list2 = Enumerable.Range(0, layer.TilesCount - 1).ToList<int>();
				list2.Shuffle<int>();
				int num4 = 0;
				foreach (LandmarkDef landmarkDef in list)
				{
					if (landmarkDef.EverValid())
					{
						WorldGenStep_Landmarks.landmarkCandidates[landmarkDef] = new HashSet<int>();
						for (int i = 0; i < list2.Count; i++)
						{
							num4 = (num4 + 1) % list2.Count;
							int num5 = list2[num4];
							if (!Find.WorldObjects.AnyMapParentAt(num5))
							{
								if (landmarkDef.IsValidTile(new PlanetTile(num5, layer), layer, true))
								{
									WorldGenStep_Landmarks.landmarkCandidates[landmarkDef].Add(num5);
								}
								if (WorldGenStep_Landmarks.landmarkCandidates[landmarkDef].Count >= num2)
								{
									break;
								}
							}
						}
					}
				}
				list.RemoveAll((LandmarkDef ld) => !WorldGenStep_Landmarks.landmarkCandidates.ContainsKey(ld) || WorldGenStep_Landmarks.landmarkCandidates[ld].Count == 0);
				LandmarkDef.ClearCache();
				int num6 = 999999;
				while (num3 < num2 && num6-- > 0)
				{
					LandmarkDef landmarkDef2 = list.RandomElementByWeight((LandmarkDef landmark) => landmark.commonality);
					int num7 = WorldGenStep_Landmarks.landmarkCandidates[landmarkDef2].FirstOrFallback(-1);
					if (num7 != -1)
					{
						WorldGenStep_Landmarks.landmarkCandidates[landmarkDef2].Remove(num7);
						if (!WorldGenStep_Landmarks.usedTiles.Contains(num7))
						{
							num3++;
							Find.World.landmarks.AddLandmark(landmarkDef2, new PlanetTile(num7, layer), layer, false);
							WorldGenStep_Landmarks.usedTiles.Add(num7);
						}
					}
				}
			}
		}

		// Token: 0x0400ECBC RID: 60604
		private const float LandmarkDensity = 0.0022222223f;

		// Token: 0x0400ECBD RID: 60605
		private static readonly Dictionary<LandmarkDef, HashSet<int>> landmarkCandidates = new Dictionary<LandmarkDef, HashSet<int>>();

		// Token: 0x0400ECBE RID: 60606
		private static readonly HashSet<int> usedTiles = new HashSet<int>();
	}
}
