using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F64 RID: 12132
	public class CompUseEffect_InstallImplantMechlink : CompUseEffect_InstallImplant
	{
		// Token: 0x06011189 RID: 70025 RVA: 0x004E8C4F File Offset: 0x004E6E4F
		public override AcceptanceReport CanBeUsedBy(Pawn p)
		{
			if (!ModLister.CheckBiotech("install implant mechlink"))
			{
				return false;
			}
			return base.CanBeUsedBy(p);
		}

		// Token: 0x0601118A RID: 70026 RVA: 0x004E8C6B File Offset: 0x004E6E6B
		public override TaggedString ConfirmMessage(Pawn p)
		{
			if (p.WorkTypeIsDisabled(WorkTypeDefOf.Smithing))
			{
				return "ConfirmInstallMechlink_Smithing".Translate();
			}
			if (p.WorkTagIsDisabled(WorkTags.Intellectual))
			{
				return "ConfirmInstallMechlink_Intellectual".Translate();
			}
			return null;
		}
	}
}
