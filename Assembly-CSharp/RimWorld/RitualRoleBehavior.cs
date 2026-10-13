using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020027C0 RID: 10176
	public class RitualRoleBehavior : IExposable
	{
		// Token: 0x17002213 RID: 8723
		// (get) Token: 0x0600DEE6 RID: 57062 RVA: 0x00418A52 File Offset: 0x00416C52
		public List<RitualPosition> CustomPositionsForReading
		{
			get
			{
				return this.customPositions;
			}
		}

		// Token: 0x0600DEE7 RID: 57063 RVA: 0x00418A5C File Offset: 0x00416C5C
		public RitualPosition GetPosition(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			if (!this.customPositions.NullOrEmpty<RitualPosition>())
			{
				foreach (RitualPosition ritualPosition in this.customPositions)
				{
					if (ritualPosition.CanUse(spot, p, ritual))
					{
						return ritualPosition;
					}
				}
			}
			return null;
		}

		// Token: 0x0600DEE8 RID: 57064 RVA: 0x00418AC8 File Offset: 0x00416CC8
		public virtual string GetJobReportOverride(Pawn pawn)
		{
			string text = this.jobReportOverride;
			TaggedString? taggedString = (text != null) ? new TaggedString?(text.Formatted(pawn.Named("PAWN")).CapitalizeFirst()) : null;
			if (taggedString == null)
			{
				return null;
			}
			return taggedString.GetValueOrDefault();
		}

		// Token: 0x0600DEE9 RID: 57065 RVA: 0x00418B20 File Offset: 0x00416D20
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.roleId, "roleId", null, false);
			Scribe_Defs.Look<DutyDef>(ref this.dutyDef, "dutyDef");
			Scribe_Collections.Look<RitualPosition>(ref this.customPositions, "customPositions", LookMode.Deep, Array.Empty<object>());
			Scribe_Defs.Look<InteractionDef>(ref this.speakerInteraction, "speakerInteraction");
		}

		// Token: 0x0400961D RID: 38429
		[NoTranslate]
		public string roleId;

		// Token: 0x0400961E RID: 38430
		public DutyDef dutyDef;

		// Token: 0x0400961F RID: 38431
		[MustTranslate]
		public string jobReportOverride;

		// Token: 0x04009620 RID: 38432
		protected List<RitualPosition> customPositions;

		// Token: 0x04009621 RID: 38433
		public InteractionDef speakerInteraction;
	}
}
