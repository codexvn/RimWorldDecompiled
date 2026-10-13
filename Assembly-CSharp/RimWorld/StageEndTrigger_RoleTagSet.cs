using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002699 RID: 9881
	public class StageEndTrigger_RoleTagSet : StageEndTrigger
	{
		// Token: 0x0600D8FD RID: 55549 RVA: 0x004048FC File Offset: 0x00402AFC
		public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
		{
			return new Trigger_TickCondition(delegate()
			{
				foreach (string r in this.roleIds)
				{
					if (!this.ArrivedCheck(r, ritual))
					{
						return false;
					}
				}
				if (this.clearTag)
				{
					foreach (string b in this.roleIds)
					{
						foreach (KeyValuePair<Pawn, PawnTags> keyValuePair in ritual.perPawnTags)
						{
							RitualRole ritualRole = ritual.RoleFor(keyValuePair.Key, true);
							if (ritualRole != null && ritualRole.id == b && keyValuePair.Value.Contains(this.tag))
							{
								keyValuePair.Value.tags.Remove(this.tag);
							}
						}
					}
				}
				return true;
			}, 1);
		}

		// Token: 0x0600D8FE RID: 55550 RVA: 0x00404924 File Offset: 0x00402B24
		protected virtual bool ArrivedCheck(string r, LordJob_Ritual ritual)
		{
			if (!ritual.RoleFilled(r))
			{
				return true;
			}
			foreach (KeyValuePair<Pawn, PawnTags> keyValuePair in ritual.perPawnTags)
			{
				RitualRole ritualRole = ritual.RoleFor(keyValuePair.Key, true);
				if (ritualRole != null && ritualRole.id == r && keyValuePair.Value.Contains(this.tag))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D8FF RID: 55551 RVA: 0x004049B8 File Offset: 0x00402BB8
		public override void ExposeData()
		{
			Scribe_Collections.Look<string>(ref this.roleIds, "roleIds", LookMode.Undefined, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.clearTag, "clearTag", false, false);
			Scribe_Values.Look<string>(ref this.tag, "tag", null, false);
		}

		// Token: 0x04009310 RID: 37648
		public List<string> roleIds;

		// Token: 0x04009311 RID: 37649
		public bool clearTag;

		// Token: 0x04009312 RID: 37650
		[NoTranslate]
		public string tag;
	}
}
