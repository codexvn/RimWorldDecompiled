using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000715 RID: 1813
	public class Hediff_VatLearning : HediffWithComps
	{
		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06002C4B RID: 11339 RVA: 0x000E6034 File Offset: 0x000E4234
		public override bool ShouldRemove
		{
			get
			{
				return this.pawn.Spawned || !(this.pawn.ParentHolder is Building_GrowthVat);
			}
		}

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06002C4C RID: 11340 RVA: 0x000E605B File Offset: 0x000E425B
		public override string LabelInBrackets
		{
			get
			{
				return this.Severity.ToStringPercent();
			}
		}

		// Token: 0x06002C4D RID: 11341 RVA: 0x000E6068 File Offset: 0x000E4268
		public override void PostTickInterval(int delta)
		{
			base.PostTickInterval(delta);
			if (this.Severity >= this.def.maxSeverity)
			{
				this.Learn();
			}
		}

		// Token: 0x06002C4E RID: 11342 RVA: 0x000E608C File Offset: 0x000E428C
		public void Learn()
		{
			if (this.pawn.skills != null)
			{
				SkillRecord skillRecord;
				if ((from x in this.pawn.skills.skills
				where !x.TotallyDisabled
				select x).TryRandomElement(out skillRecord))
				{
					skillRecord.Learn(8000f, true, false);
				}
			}
			this.Severity = this.def.initialSeverity;
		}

		// Token: 0x0400238A RID: 9098
		private const float XPToAward = 8000f;
	}
}
