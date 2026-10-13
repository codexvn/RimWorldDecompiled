using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DBB RID: 15803
	public class AbandonedSettlement : WorldObject
	{
		// Token: 0x060158AC RID: 88236 RVA: 0x00673B44 File Offset: 0x00671D44
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (base.Faction != Faction.OfPlayer)
			{
				return text;
			}
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			text += "Abandoned".Translate() + ": " + GenDate.DateShortStringAt((long)GenDate.TickGameToAbs(this.creationGameTicks), Find.WorldGrid.LongLatOf(base.Tile));
			text += " (";
			text += "TimeAgo".Translate((Find.TickManager.TicksGame - this.creationGameTicks).ToStringTicksToPeriodVague(true, true));
			return text + ")";
		}
	}
}
