using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D11 RID: 15633
	public class WorldInfo : IExposable
	{
		// Token: 0x17003508 RID: 13576
		// (get) Token: 0x06015479 RID: 87161 RVA: 0x0065DA73 File Offset: 0x0065BC73
		public string FileNameNoExtension
		{
			get
			{
				return GenText.CapitalizedNoSpaces(this.name);
			}
		}

		// Token: 0x17003509 RID: 13577
		// (get) Token: 0x0601547A RID: 87162 RVA: 0x0065DA80 File Offset: 0x0065BC80
		public int Seed
		{
			get
			{
				return GenText.StableStringHash(this.seedString);
			}
		}

		// Token: 0x0601547B RID: 87163 RVA: 0x0065DA90 File Offset: 0x0065BC90
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<float>(ref this.planetCoverage, "planetCoverage", 0f, false);
			Scribe_Values.Look<string>(ref this.seedString, "seedString", null, false);
			Scribe_Values.Look<int>(ref this.persistentRandomValue, "persistentRandomValue", 0, false);
			Scribe_Values.Look<OverallRainfall>(ref this.overallRainfall, "overallRainfall", OverallRainfall.AlmostNone, false);
			Scribe_Values.Look<OverallTemperature>(ref this.overallTemperature, "overallTemperature", OverallTemperature.VeryCold, false);
			Scribe_Values.Look<IntVec3>(ref this.initialMapSize, "initialMapSize", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.pollution, "pollution", 0f, false);
			Scribe_Collections.Look<FactionDef>(ref this.factions, "factions", LookMode.Def, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Dictionary<FactionDef, int> dictionary = null;
				Scribe_Collections.Look<FactionDef, int>(ref dictionary, "factionCounts", LookMode.Def, LookMode.Value);
				if (dictionary != null)
				{
					this.factions = new List<FactionDef>();
					foreach (KeyValuePair<FactionDef, int> keyValuePair in dictionary)
					{
						if (keyValuePair.Value > 0)
						{
							for (int i = 0; i < keyValuePair.Value; i++)
							{
								this.factions.Add(keyValuePair.Key);
							}
						}
					}
				}
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0400ED34 RID: 60724
		public string name = "DefaultWorldName";

		// Token: 0x0400ED35 RID: 60725
		public float planetCoverage;

		// Token: 0x0400ED36 RID: 60726
		public string seedString = "SeedError";

		// Token: 0x0400ED37 RID: 60727
		public int persistentRandomValue = Rand.Int;

		// Token: 0x0400ED38 RID: 60728
		public OverallRainfall overallRainfall = OverallRainfall.Normal;

		// Token: 0x0400ED39 RID: 60729
		public OverallTemperature overallTemperature = OverallTemperature.Normal;

		// Token: 0x0400ED3A RID: 60730
		public OverallPopulation overallPopulation = OverallPopulation.Normal;

		// Token: 0x0400ED3B RID: 60731
		public LandmarkDensity landmarkDensity = LandmarkDensity.Normal;

		// Token: 0x0400ED3C RID: 60732
		public IntVec3 initialMapSize = new IntVec3(250, 1, 250);

		// Token: 0x0400ED3D RID: 60733
		public List<FactionDef> factions;

		// Token: 0x0400ED3E RID: 60734
		public float pollution;
	}
}
