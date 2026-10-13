using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002824 RID: 10276
	public class RoleRequirement_SameIdeo : RoleRequirement
	{
		// Token: 0x0600E0B8 RID: 57528 RVA: 0x0041F587 File Offset: 0x0041D787
		public override string GetLabel(Precept_Role role)
		{
			return this.labelKey.Translate(Find.ActiveLanguageWorker.WithIndefiniteArticle(role.ideo.memberName, Gender.None, false, false));
		}

		// Token: 0x0600E0B9 RID: 57529 RVA: 0x0041F5B6 File Offset: 0x0041D7B6
		public override bool Met(Pawn p, Precept_Role role)
		{
			return p.Ideo == role.ideo;
		}
	}
}
