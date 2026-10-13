using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D14 RID: 15636
	public class WorldLandmarks : IExposable
	{
		// Token: 0x1700350B RID: 13579
		public Landmark this[PlanetTile tile]
		{
			get
			{
				if (this.landmarks.ContainsKey(tile))
				{
					return this.landmarks[tile];
				}
				return null;
			}
			set
			{
				this.landmarks[tile] = value;
			}
		}

		// Token: 0x06015482 RID: 87170 RVA: 0x0065DD0D File Offset: 0x0065BF0D
		public void ExposeData()
		{
			Scribe_Collections.Look<PlanetTile, Landmark>(ref this.landmarks, "landmarks", LookMode.Value, LookMode.Deep);
		}

		// Token: 0x06015483 RID: 87171 RVA: 0x0065DD24 File Offset: 0x0065BF24
		public void AddLandmark(LandmarkDef landmarkDef, PlanetTile tile, PlanetLayer layer = null, bool forced = false)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			if (layer == null)
			{
				layer = tile.Layer;
			}
			Landmark landmark = (Landmark)Activator.CreateInstance(landmarkDef.workerClass, new object[]
			{
				landmarkDef
			});
			if (landmarkDef.nameMaker != null)
			{
				landmark.name = NameGenerator.GenerateName(landmarkDef.nameMaker, null, false, "r_name", null, null);
			}
			else
			{
				landmark.name = landmarkDef.LabelCap;
			}
			this[tile] = landmark;
			foreach (MutatorChance mutatorChance in landmarkDef.mutatorChances)
			{
				if (Rand.Chance(mutatorChance.chance) && ((mutatorChance.required && forced) || mutatorChance.mutator.IsValidTile(tile, layer)))
				{
					layer[tile].AddMutator(mutatorChance.mutator);
				}
			}
			foreach (MutatorChance mutatorChance2 in landmarkDef.comboLandmarkMutators)
			{
				if (Rand.Chance(mutatorChance2.chance) && ((mutatorChance2.required && forced) || mutatorChance2.mutator.IsValidTile(tile, layer)))
				{
					layer[tile].AddMutator(mutatorChance2.mutator);
					landmark.isComboLandmark = true;
					break;
				}
			}
		}

		// Token: 0x06015484 RID: 87172 RVA: 0x0065DE94 File Offset: 0x0065C094
		public void RemoveLandmark(PlanetTile tile)
		{
			if (this.landmarks.ContainsKey(tile))
			{
				this.landmarks.Remove(tile);
			}
		}

		// Token: 0x0400ED4F RID: 60751
		public Dictionary<PlanetTile, Landmark> landmarks = new Dictionary<PlanetTile, Landmark>();
	}
}
