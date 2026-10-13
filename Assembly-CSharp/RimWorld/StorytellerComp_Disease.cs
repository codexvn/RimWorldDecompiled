using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E3D RID: 7741
	public class StorytellerComp_Disease : StorytellerComp
	{
		// Token: 0x17001A3F RID: 6719
		// (get) Token: 0x0600AC43 RID: 44099 RVA: 0x00313DE3 File Offset: 0x00311FE3
		protected StorytellerCompProperties_Disease Props
		{
			get
			{
				return (StorytellerCompProperties_Disease)this.props;
			}
		}

		// Token: 0x0600AC44 RID: 44100 RVA: 0x00313DF0 File Offset: 0x00311FF0
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_Disease.<>c__DisplayClass3_0 CS$<>8__locals1 = new StorytellerComp_Disease.<>c__DisplayClass3_0();
			if (!DebugSettings.enableRandomDiseases)
			{
				yield break;
			}
			if (target is World)
			{
				yield break;
			}
			if (Find.Storyteller.difficulty.diseaseIntervalFactor >= 100f)
			{
				yield break;
			}
			Map map = target as Map;
			if (map != null)
			{
				CS$<>8__locals1.biome = map.Biome;
			}
			else
			{
				StorytellerComp_Disease.<>c__DisplayClass3_0 CS$<>8__locals2 = CS$<>8__locals1;
				Tile tile = Find.WorldGrid[target.Tile];
				CS$<>8__locals2.biome = ((tile != null) ? tile.PrimaryBiome : null);
			}
			if (CS$<>8__locals1.biome == null)
			{
				yield break;
			}
			float num = CS$<>8__locals1.biome.diseaseMtbDays;
			num *= Find.Storyteller.difficulty.diseaseIntervalFactor;
			if (target is Caravan)
			{
				num *= this.CaravanDiseaseMTBFactor;
			}
			if (Rand.MTBEventOccurs(num, 60000f, 1000f))
			{
				IncidentParms parms = this.GenerateParms(this.Props.category, target);
				IncidentDef def;
				if (base.UsableIncidentsInCategory(this.Props.category, parms).TryRandomElementByWeight((IncidentDef d) => CS$<>8__locals1.biome.CommonalityOfDisease(d), out def))
				{
					yield return new FiringIncident(def, this, parms);
				}
			}
			yield break;
		}

		// Token: 0x0600AC45 RID: 44101 RVA: 0x00313E07 File Offset: 0x00312007
		public override string ToString()
		{
			string str = base.ToString();
			string str2 = " ";
			IncidentCategoryDef category = this.Props.category;
			return str + str2 + ((category != null) ? category.ToString() : null);
		}

		// Token: 0x04007672 RID: 30322
		private float CaravanDiseaseMTBFactor = 4f;
	}
}
