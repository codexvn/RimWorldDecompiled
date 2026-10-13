using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200305C RID: 12380
	public abstract class Alert_Analyzable : Alert
	{
		// Token: 0x17002CF9 RID: 11513
		// (get) Token: 0x0601162C RID: 71212
		protected abstract ThingDef Def { get; }

		// Token: 0x0601162D RID: 71213 RVA: 0x004FCDB0 File Offset: 0x004FAFB0
		public override AlertReport GetReport()
		{
			if (this.Def == null)
			{
				return AlertReport.Inactive;
			}
			this.targets.Clear();
			foreach (Map map in Find.Maps)
			{
				this.targets.AddRange(map.listerThings.ThingsOfDef(this.Def));
			}
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0400B3F5 RID: 46069
		protected List<Thing> targets = new List<Thing>();
	}
}
