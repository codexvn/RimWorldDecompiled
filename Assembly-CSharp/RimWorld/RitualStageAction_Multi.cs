using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020027C5 RID: 10181
	public class RitualStageAction_Multi : RitualStageAction
	{
		// Token: 0x0600DEF5 RID: 57077 RVA: 0x00418BC0 File Offset: 0x00416DC0
		public override void Apply(LordJob_Ritual ritual)
		{
			if (this.subActions == null)
			{
				Log.Error("RitualStageAction_Multi without any sub actions on ritual " + ((ritual != null) ? ritual.ToString() : null));
				return;
			}
			foreach (RitualStageAction ritualStageAction in this.subActions)
			{
				ritualStageAction.Apply(ritual);
			}
		}

		// Token: 0x0600DEF6 RID: 57078 RVA: 0x00418C38 File Offset: 0x00416E38
		public override void ApplyToPawn(LordJob_Ritual ritual, Pawn pawn)
		{
			if (this.subActions == null)
			{
				Log.Error("RitualStageAction_Multi without any sub actions on ritual " + ((ritual != null) ? ritual.ToString() : null));
				return;
			}
			foreach (RitualStageAction ritualStageAction in this.subActions)
			{
				ritualStageAction.ApplyToPawn(ritual, pawn);
			}
		}

		// Token: 0x0600DEF7 RID: 57079 RVA: 0x00418CB0 File Offset: 0x00416EB0
		public override void ExposeData()
		{
			Scribe_Collections.Look<RitualStageAction>(ref this.subActions, "subActions", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x04009623 RID: 38435
		public List<RitualStageAction> subActions;
	}
}
