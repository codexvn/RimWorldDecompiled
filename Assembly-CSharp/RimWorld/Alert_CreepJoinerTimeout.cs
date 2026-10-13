using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200300A RID: 12298
	public class Alert_CreepJoinerTimeout : Alert
	{
		// Token: 0x17002CDB RID: 11483
		// (get) Token: 0x060114EC RID: 70892 RVA: 0x004F6288 File Offset: 0x004F4488
		private List<Pawn> Creepjoiners
		{
			get
			{
				this.creepjoiners.Clear();
				foreach (Map map in Find.Maps)
				{
					foreach (Pawn pawn in map.mapPawns.AllHumanlikeSpawned)
					{
						if (pawn.IsCreepJoiner && pawn.creepjoiner.IsOnEntryLord && GenTicks.TicksAbs >= pawn.creepjoiner.timeoutAt - 30000)
						{
							this.creepjoiners.Add(pawn);
						}
					}
				}
				return this.creepjoiners;
			}
		}

		// Token: 0x060114ED RID: 70893 RVA: 0x004F635C File Offset: 0x004F455C
		public Alert_CreepJoinerTimeout()
		{
			this.defaultPriority = AlertPriority.High;
			this.requireAnomaly = true;
		}

		// Token: 0x060114EE RID: 70894 RVA: 0x004F6388 File Offset: 0x004F4588
		public override string GetLabel()
		{
			if (this.creepjoiners.NullOrEmpty<Pawn>())
			{
				return string.Empty;
			}
			return "CreepJoinerTimeout".Translate();
		}

		// Token: 0x060114EF RID: 70895 RVA: 0x004F63AC File Offset: 0x004F45AC
		public override TaggedString GetExplanation()
		{
			if (this.creepjoiners.NullOrEmpty<Pawn>())
			{
				return string.Empty;
			}
			this.sb.Length = 0;
			foreach (Pawn pawn in this.creepjoiners)
			{
				int numTicks = Mathf.Max(pawn.creepjoiner.timeoutAt - GenTicks.TicksAbs, 0);
				this.sb.AppendLineTagged(string.Concat(new string[]
				{
					"  - ",
					pawn.LabelShortCap.Colorize(ColoredText.NameColor),
					", ",
					pawn.creepjoiner.form.label.Colorize(Color.gray),
					": ",
					numTicks.ToStringTicksToPeriodVerbose(true, true)
				}));
			}
			return string.Format("{0}:\n\n{1}\n{2}", "CreepJoinerTimeoutDesc".Translate(), this.sb, "CreepJoinerTimeoutDescAppended".Translate());
		}

		// Token: 0x060114F0 RID: 70896 RVA: 0x004F64DC File Offset: 0x004F46DC
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Creepjoiners);
		}

		// Token: 0x0400B362 RID: 45922
		private const int WarningTicks = 30000;

		// Token: 0x0400B363 RID: 45923
		private readonly List<Pawn> creepjoiners = new List<Pawn>();

		// Token: 0x0400B364 RID: 45924
		private StringBuilder sb = new StringBuilder();
	}
}
