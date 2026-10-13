using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002825 RID: 10277
	public class RoleRequirement_SupremeGender : RoleRequirement
	{
		// Token: 0x0600E0BB RID: 57531 RVA: 0x0041F5C6 File Offset: 0x0041D7C6
		private bool ActiveFor(Precept_Role role)
		{
			return role.restrictToSupremeGender && role.ideo.SupremeGender > Gender.None;
		}

		// Token: 0x0600E0BC RID: 57532 RVA: 0x0041F5E0 File Offset: 0x0041D7E0
		public override string GetLabel(Precept_Role role)
		{
			if (!this.ActiveFor(role))
			{
				return string.Empty;
			}
			return this.labelKey.Translate(role.ideo.SupremeGender.GetLabel(false));
		}

		// Token: 0x0600E0BD RID: 57533 RVA: 0x0041F617 File Offset: 0x0041D817
		public override bool Met(Pawn pawn, Precept_Role role)
		{
			return !this.ActiveFor(role) || pawn.gender == role.ideo.SupremeGender;
		}
	}
}
