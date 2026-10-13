using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020026A0 RID: 9888
	public class StageEndTrigger_MutilatedRole : StageEndTrigger
	{
		// Token: 0x0600D90D RID: 55565 RVA: 0x00404C90 File Offset: 0x00402E90
		public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
		{
			LordJob_Ritual_Mutilation r = ritual as LordJob_Ritual_Mutilation;
			if (r != null)
			{
				return new Trigger_Custom(delegate(TriggerSignal signal)
				{
					foreach (Pawn p in r.mutilatedPawns)
					{
						RitualRole ritualRole = r.RoleFor(p, true);
						if (ritualRole != null && ritualRole.id == this.roleId)
						{
							return true;
						}
					}
					return false;
				});
			}
			Log.Error("Used StageEndTrigger_MutilatedRole on non LordJob_Ritual_Mutilation ritual job");
			return null;
		}

		// Token: 0x0600D90E RID: 55566 RVA: 0x00404CDB File Offset: 0x00402EDB
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.roleId, "roleId", null, false);
		}

		// Token: 0x04009318 RID: 37656
		[NoTranslate]
		public string roleId;
	}
}
