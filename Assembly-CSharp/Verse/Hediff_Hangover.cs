using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000736 RID: 1846
	public class Hediff_Hangover : HediffWithComps
	{
		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06002CFB RID: 11515 RVA: 0x000E85AD File Offset: 0x000E67AD
		public override bool Visible
		{
			get
			{
				return !this.pawn.health.hediffSet.HasHediff(HediffDefOf.AlcoholHigh, false) && base.Visible;
			}
		}
	}
}
