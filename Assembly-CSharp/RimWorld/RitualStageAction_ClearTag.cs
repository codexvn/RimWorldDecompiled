using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020027C8 RID: 10184
	public class RitualStageAction_ClearTag : RitualStageAction
	{
		// Token: 0x0600DF00 RID: 57088 RVA: 0x00418E50 File Offset: 0x00417050
		public override void Apply(LordJob_Ritual ritual)
		{
			Pawn key = ritual.PawnWithRole(this.roleId);
			PawnTags pawnTags;
			if (ritual.perPawnTags.TryGetValue(key, out pawnTags))
			{
				List<string> tags = pawnTags.tags;
				if (tags == null)
				{
					return;
				}
				tags.Remove(this.tag);
			}
		}

		// Token: 0x0600DF01 RID: 57089 RVA: 0x00418E91 File Offset: 0x00417091
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.roleId, "roleId", null, false);
			Scribe_Values.Look<string>(ref this.tag, "tag", null, false);
		}

		// Token: 0x04009628 RID: 38440
		[NoTranslate]
		public string roleId;

		// Token: 0x04009629 RID: 38441
		[NoTranslate]
		public string tag;
	}
}
