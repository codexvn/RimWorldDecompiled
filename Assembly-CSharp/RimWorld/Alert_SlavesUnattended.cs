using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003004 RID: 12292
	public class Alert_SlavesUnattended : Alert
	{
		// Token: 0x17002CD7 RID: 11479
		// (get) Token: 0x060114D6 RID: 70870 RVA: 0x004F5D84 File Offset: 0x004F3F84
		public List<Pawn> Targets
		{
			get
			{
				this.targetsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (SlaveRebellionUtility.IsUnattendedByColonists(maps[i]))
					{
						this.targetsResult.AddRange(maps[i].mapPawns.SlavesOfColonySpawned);
					}
				}
				return this.targetsResult;
			}
		}

		// Token: 0x060114D7 RID: 70871 RVA: 0x004F5DE4 File Offset: 0x004F3FE4
		public Alert_SlavesUnattended()
		{
			this.defaultLabel = "SlaveUnattendedLabel".Translate();
			this.labelMultiple = "SlaveUnattendedMultipleLabel".Translate();
			this.defaultPriority = AlertPriority.High;
			this.requireIdeology = true;
		}

		// Token: 0x060114D8 RID: 70872 RVA: 0x004F5E3A File Offset: 0x004F403A
		public override string GetLabel()
		{
			if (this.targetsResult.Count != 1)
			{
				return this.labelMultiple;
			}
			return this.defaultLabel;
		}

		// Token: 0x060114D9 RID: 70873 RVA: 0x004F5E57 File Offset: 0x004F4057
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x060114DA RID: 70874 RVA: 0x004F5E64 File Offset: 0x004F4064
		public override TaggedString GetExplanation()
		{
			return "SlavesUnattendedDesc".Translate(this.targetsResult[0]);
		}

		// Token: 0x0400B357 RID: 45911
		private List<Pawn> targetsResult = new List<Pawn>();

		// Token: 0x0400B358 RID: 45912
		private string labelMultiple;
	}
}
