using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BD2 RID: 11218
	public class CompAncientToxVent : CompAncientVent
	{
		// Token: 0x17002752 RID: 10066
		// (get) Token: 0x0600FAEE RID: 64238 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool AppliesEffectsToPawns
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600FAEF RID: 64239 RVA: 0x0048E878 File Offset: 0x0048CA78
		protected override void ToggleIndividualVent(bool on)
		{
			this.parent.GetComp<CompFleckEmitterLongTerm>().Enabled = on;
		}

		// Token: 0x0600FAF0 RID: 64240 RVA: 0x0048E893 File Offset: 0x0048CA93
		protected override void ApplyPawnEffect(Pawn pawn, int delta)
		{
			ToxicUtility.DoPawnToxicDamage(pawn, 0.0028977108f * (float)delta);
		}
	}
}
