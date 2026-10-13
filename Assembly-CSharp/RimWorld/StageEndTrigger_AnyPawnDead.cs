using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020026A4 RID: 9892
	public class StageEndTrigger_AnyPawnDead : StageEndTrigger
	{
		// Token: 0x0600D917 RID: 55575 RVA: 0x00404E64 File Offset: 0x00403064
		protected virtual bool Trigger(LordJob_Ritual ritual)
		{
			foreach (string roleId in this.roleIds)
			{
				using (IEnumerator<Pawn> enumerator2 = ritual.assignments.AssignedPawns(roleId).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.Dead)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600D918 RID: 55576 RVA: 0x00404EF8 File Offset: 0x004030F8
		public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
		{
			return new Trigger_Custom((TriggerSignal signal) => this.Trigger(ritual));
		}

		// Token: 0x0600D919 RID: 55577 RVA: 0x00404F1D File Offset: 0x0040311D
		public override void ExposeData()
		{
			Scribe_Collections.Look<string>(ref this.roleIds, "roleIds", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x0400931E RID: 37662
		[NoTranslate]
		public List<string> roleIds;
	}
}
