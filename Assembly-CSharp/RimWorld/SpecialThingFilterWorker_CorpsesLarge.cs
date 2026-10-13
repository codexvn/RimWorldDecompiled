using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036BE RID: 14014
	public class SpecialThingFilterWorker_CorpsesLarge : SpecialThingFilterWorker
	{
		// Token: 0x0601375F RID: 79711 RVA: 0x005C8B80 File Offset: 0x005C6D80
		public override bool Matches(Thing t)
		{
			Corpse corpse = t as Corpse;
			return corpse != null && corpse.InnerPawn.BodySize >= 0.75f;
		}

		// Token: 0x06013760 RID: 79712 RVA: 0x005C8BB0 File Offset: 0x005C6DB0
		public override bool CanEverMatch(ThingDef def)
		{
			if (def.IsCorpse)
			{
				IngestibleProperties ingestible = def.ingestible;
				bool flag;
				if (ingestible == null)
				{
					flag = (null != null);
				}
				else
				{
					ThingDef sourceDef = ingestible.sourceDef;
					flag = (((sourceDef != null) ? sourceDef.race : null) != null);
				}
				if (flag)
				{
					RaceProperties race = def.ingestible.sourceDef.race;
					for (int i = 0; i < race.lifeStageAges.Count; i++)
					{
						if (race.lifeStageAges[i].def.bodySizeFactor >= 0.75f)
						{
							return true;
						}
					}
					return false;
				}
			}
			return false;
		}

		// Token: 0x06013761 RID: 79713 RVA: 0x005C8C30 File Offset: 0x005C6E30
		public override bool AlwaysMatches(ThingDef def)
		{
			if (def.IsCorpse)
			{
				IngestibleProperties ingestible = def.ingestible;
				bool flag;
				if (ingestible == null)
				{
					flag = (null != null);
				}
				else
				{
					ThingDef sourceDef = ingestible.sourceDef;
					flag = (((sourceDef != null) ? sourceDef.race : null) != null);
				}
				if (flag)
				{
					RaceProperties race = def.ingestible.sourceDef.race;
					for (int i = 0; i < race.lifeStageAges.Count; i++)
					{
						if (race.lifeStageAges[i].def.bodySizeFactor < 0.75f)
						{
							return false;
						}
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400D7AB RID: 55211
		private const float MinBodySize = 0.75f;
	}
}
