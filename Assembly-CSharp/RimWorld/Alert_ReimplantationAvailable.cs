using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002FE6 RID: 12262
	public class Alert_ReimplantationAvailable : Alert
	{
		// Token: 0x17002CCB RID: 11467
		// (get) Token: 0x0601145E RID: 70750 RVA: 0x004F2BFC File Offset: 0x004F0DFC
		private Pawn WaitingPawn
		{
			get
			{
				foreach (Map map in Find.Maps)
				{
					foreach (Lord lord in map.lordManager.lords)
					{
						LordToil_ReimplantXenogerm lordToil_ReimplantXenogerm = lord.CurLordToil as LordToil_ReimplantXenogerm;
						if (lordToil_ReimplantXenogerm != null)
						{
							this.waitingPawn = lordToil_ReimplantXenogerm.Data.target;
							return this.waitingPawn;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x0601145F RID: 70751 RVA: 0x004F2CB0 File Offset: 0x004F0EB0
		public Alert_ReimplantationAvailable()
		{
			this.defaultPriority = AlertPriority.High;
			this.defaultLabel = "AlertReimplantationAvailable".Translate();
			this.requireBiotech = true;
		}

		// Token: 0x06011460 RID: 70752 RVA: 0x004F2CDB File Offset: 0x004F0EDB
		public override TaggedString GetExplanation()
		{
			return "AlertReimplantationAvailableDesc".Translate(this.waitingPawn);
		}

		// Token: 0x06011461 RID: 70753 RVA: 0x004F2CF2 File Offset: 0x004F0EF2
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritIs(this.WaitingPawn);
		}

		// Token: 0x0400B31E RID: 45854
		private Pawn waitingPawn;
	}
}
