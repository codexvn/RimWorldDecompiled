using System;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DF2 RID: 15858
	public class SitePartWorker_InsectLair : SitePartWorker
	{
		// Token: 0x06015A2A RID: 88618 RVA: 0x00679F80 File Offset: 0x00678180
		public override void PostMapGenerate(Map map)
		{
			Thing thing = map.listerThings.ThingsOfDef(ThingDefOf.InsectLairEntrance).FirstOrDefault<Thing>();
			if (thing != null)
			{
				Find.LetterStack.ReceiveLetter("CaveEntranceLetter".Translate(), "CaveEntranceLetterText".Translate(), LetterDefOf.NeutralEvent, thing, null, null, null, null, 0, true);
			}
		}
	}
}
