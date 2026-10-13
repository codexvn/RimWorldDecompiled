using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002822 RID: 10274
	public abstract class RoleRequirement
	{
		// Token: 0x0600E0B0 RID: 57520 RVA: 0x0041F532 File Offset: 0x0041D732
		public virtual string GetLabel(Precept_Role role)
		{
			return this.labelKey.Translate();
		}

		// Token: 0x0600E0B1 RID: 57521 RVA: 0x0041F544 File Offset: 0x0041D744
		public string GetLabelCap(Precept_Role role)
		{
			return this.GetLabel(role).CapitalizeFirst();
		}

		// Token: 0x0600E0B2 RID: 57522
		public abstract bool Met(Pawn p, Precept_Role role);

		// Token: 0x04009702 RID: 38658
		[NoTranslate]
		public string labelKey;
	}
}
