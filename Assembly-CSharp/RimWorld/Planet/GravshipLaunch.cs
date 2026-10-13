using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DA8 RID: 15784
	public class GravshipLaunch : WorldObject
	{
		// Token: 0x06015811 RID: 88081 RVA: 0x006715C4 File Offset: 0x0066F7C4
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
			text += "LaunchDate".Translate() + ": " + GenDate.DateShortStringAt((long)GenDate.TickGameToAbs(this.creationGameTicks), Find.WorldGrid.LongLatOf(base.Tile));
			text += " (";
			text += "TimeAgo".Translate((Find.TickManager.TicksGame - this.creationGameTicks).ToStringTicksToPeriodVague(true, true));
			return text + ")";
		}
	}
}
