using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020026A2 RID: 9890
	public class StageEndTrigger_RolesUnarmed : StageEndTrigger
	{
		// Token: 0x0600D912 RID: 55570 RVA: 0x00404D74 File Offset: 0x00402F74
		public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
		{
			return new Trigger_Custom(delegate(TriggerSignal signal)
			{
				foreach (string roleId in this.roleIds)
				{
					using (List<ThingWithComps>.Enumerator enumerator2 = ritual.PawnWithRole(roleId).equipment.AllEquipmentListForReading.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.def.IsWeapon)
							{
								return false;
							}
						}
					}
				}
				return true;
			});
		}

		// Token: 0x0600D913 RID: 55571 RVA: 0x00404D99 File Offset: 0x00402F99
		public override void ExposeData()
		{
			Scribe_Collections.Look<string>(ref this.roleIds, "roleIds", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x0400931B RID: 37659
		[NoTranslate]
		public List<string> roleIds;
	}
}
