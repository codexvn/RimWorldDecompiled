using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FFB RID: 12283
	public class Alert_ColonistLeftUnburied : Alert
	{
		// Token: 0x060114B2 RID: 70834 RVA: 0x004F5034 File Offset: 0x004F3234
		public static bool IsCorpseOfColonist(Corpse corpse)
		{
			return corpse.InnerPawn.Faction == Faction.OfPlayer && corpse.InnerPawn.def.race.Humanlike && !corpse.InnerPawn.IsQuestLodger() && !corpse.InnerPawn.IsSlave && !corpse.InnerPawn.IsSubhuman && !corpse.InnerPawn.IsShambler && corpse.Spawned;
		}

		// Token: 0x17002CCE RID: 11470
		// (get) Token: 0x060114B3 RID: 70835 RVA: 0x004F50A8 File Offset: 0x004F32A8
		private List<Thing> UnburiedColonistCorpses
		{
			get
			{
				this.unburiedColonistCorpsesResult.Clear();
				foreach (Map map in Find.Maps)
				{
					if (map.mapPawns.AnyFreeColonistSpawned)
					{
						List<Thing> list = map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.Corpse));
						for (int i = 0; i < list.Count; i++)
						{
							Corpse corpse = (Corpse)list[i];
							if (Alert_ColonistLeftUnburied.IsCorpseOfColonist(corpse))
							{
								this.unburiedColonistCorpsesResult.Add(corpse);
							}
						}
					}
				}
				return this.unburiedColonistCorpsesResult;
			}
		}

		// Token: 0x060114B4 RID: 70836 RVA: 0x004F5158 File Offset: 0x004F3358
		public Alert_ColonistLeftUnburied()
		{
			this.defaultLabel = "AlertColonistLeftUnburied".Translate();
			this.defaultExplanation = "AlertColonistLeftUnburiedDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x060114B5 RID: 70837 RVA: 0x004F51A7 File Offset: 0x004F33A7
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.UnburiedColonistCorpses);
		}

		// Token: 0x0400B349 RID: 45897
		private List<Thing> unburiedColonistCorpsesResult = new List<Thing>();
	}
}
