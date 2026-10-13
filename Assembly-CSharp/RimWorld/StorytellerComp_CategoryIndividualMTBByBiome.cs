using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E2C RID: 7724
	public class StorytellerComp_CategoryIndividualMTBByBiome : StorytellerComp
	{
		// Token: 0x17001A32 RID: 6706
		// (get) Token: 0x0600AC01 RID: 44033 RVA: 0x003132F0 File Offset: 0x003114F0
		protected StorytellerCompProperties_CategoryIndividualMTBByBiome Props
		{
			get
			{
				return (StorytellerCompProperties_CategoryIndividualMTBByBiome)this.props;
			}
		}

		// Token: 0x0600AC02 RID: 44034 RVA: 0x003132FD File Offset: 0x003114FD
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (target is World)
			{
				yield break;
			}
			List<IncidentDef> allIncidents = DefDatabase<IncidentDef>.AllDefsListForReading;
			int num2;
			for (int i = 0; i < allIncidents.Count; i = num2 + 1)
			{
				IncidentDef incidentDef = allIncidents[i];
				if (incidentDef.category == this.Props.category)
				{
					BiomeDef biome = Find.WorldGrid[target.Tile].PrimaryBiome;
					if (incidentDef.mtbDaysByBiome != null)
					{
						MTBByBiome mtbbyBiome = incidentDef.mtbDaysByBiome.Find((MTBByBiome x) => x.biome == biome);
						if (mtbbyBiome != null)
						{
							float num = mtbbyBiome.mtbDays;
							if (this.Props.applyCaravanVisibility)
							{
								Caravan caravan = target as Caravan;
								if (caravan != null)
								{
									num /= caravan.Visibility;
								}
								else
								{
									Map map = target as Map;
									if (map != null && map.Parent.def.isTempIncidentMapOwner)
									{
										IEnumerable<Pawn> pawns = map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer).Concat(map.mapPawns.PrisonersOfColonySpawned);
										num /= CaravanVisibilityCalculator.Visibility(pawns, false, null);
									}
								}
							}
							if (Rand.MTBEventOccurs(num, 60000f, 1000f))
							{
								IncidentParms parms = this.GenerateParms(incidentDef.category, target);
								if (incidentDef.Worker.CanFireNow(parms))
								{
									yield return new FiringIncident(incidentDef, this, parms);
								}
							}
						}
					}
				}
				num2 = i;
			}
			yield break;
		}

		// Token: 0x0600AC03 RID: 44035 RVA: 0x00313314 File Offset: 0x00311514
		public override string ToString()
		{
			string str = base.ToString();
			string str2 = " ";
			IncidentCategoryDef category = this.Props.category;
			return str + str2 + ((category != null) ? category.ToString() : null);
		}
	}
}
