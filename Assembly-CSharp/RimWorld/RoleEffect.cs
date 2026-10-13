using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002819 RID: 10265
	public abstract class RoleEffect
	{
		// Token: 0x17002247 RID: 8775
		// (get) Token: 0x0600E099 RID: 57497 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool IsBad
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600E09A RID: 57498 RVA: 0x0041F2FC File Offset: 0x0041D4FC
		public virtual string Label(Pawn pawn, Precept_Role role)
		{
			if (!this.labelKey.NullOrEmpty())
			{
				return this.labelKey.Translate(pawn.Named("PAWN"), role.LabelCap);
			}
			return this.label.Formatted(pawn.Named("PAWN"), role.LabelCap);
		}

		// Token: 0x0600E09B RID: 57499 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanEquip(Pawn pawn, Thing thing)
		{
			return true;
		}

		// Token: 0x0600E09C RID: 57500 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_Tended(Pawn doctor, Pawn target)
		{
		}

		// Token: 0x040096FB RID: 38651
		[MustTranslate]
		public string label;

		// Token: 0x040096FC RID: 38652
		[NoTranslate]
		public string labelKey;
	}
}
