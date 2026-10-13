using System;

namespace RimWorld
{
	// Token: 0x020024A6 RID: 9382
	public struct ApparelRequirementWithSource
	{
		// Token: 0x17001F0F RID: 7951
		// (get) Token: 0x0600CBB3 RID: 52147 RVA: 0x003BB01F File Offset: 0x003B921F
		public ApparelRequirementSource Source
		{
			get
			{
				if (this.sourceRole != null)
				{
					return ApparelRequirementSource.Role;
				}
				if (this.sourceTitle != null)
				{
					return ApparelRequirementSource.Title;
				}
				return ApparelRequirementSource.Invalid;
			}
		}

		// Token: 0x17001F10 RID: 7952
		// (get) Token: 0x0600CBB4 RID: 52148 RVA: 0x003BB036 File Offset: 0x003B9236
		public string SourceLabelCap
		{
			get
			{
				if (this.sourceTitle != null)
				{
					return this.sourceTitle.def.GetLabelCapFor(this.sourceTitle.pawn);
				}
				return this.sourceRole.LabelCap;
			}
		}

		// Token: 0x0600CBB5 RID: 52149 RVA: 0x003BB067 File Offset: 0x003B9267
		public ApparelRequirementWithSource(ApparelRequirement requirement, Precept_Role role)
		{
			this.requirement = requirement;
			this.sourceRole = role;
			this.sourceTitle = null;
		}

		// Token: 0x0600CBB6 RID: 52150 RVA: 0x003BB07E File Offset: 0x003B927E
		public ApparelRequirementWithSource(ApparelRequirement requirement, RoyalTitle title)
		{
			this.requirement = requirement;
			this.sourceTitle = title;
			this.sourceRole = null;
		}

		// Token: 0x04008ADE RID: 35550
		public ApparelRequirement requirement;

		// Token: 0x04008ADF RID: 35551
		public Precept_Role sourceRole;

		// Token: 0x04008AE0 RID: 35552
		public RoyalTitle sourceTitle;
	}
}
