using System;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020006F4 RID: 1780
	public class HediffComp_ReactOnDamage : HediffComp
	{
		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x06002BC9 RID: 11209 RVA: 0x000E4354 File Offset: 0x000E2554
		public HediffCompProperties_ReactOnDamage Props
		{
			get
			{
				return (HediffCompProperties_ReactOnDamage)this.props;
			}
		}

		// Token: 0x06002BCA RID: 11210 RVA: 0x000E4361 File Offset: 0x000E2561
		public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.Props.damageDefIncoming == dinfo.Def)
			{
				this.React();
			}
		}

		// Token: 0x06002BCB RID: 11211 RVA: 0x000E4380 File Offset: 0x000E2580
		private void React()
		{
			if (this.Props.createHediff != null)
			{
				BodyPartRecord part = this.parent.Part;
				if (this.Props.createHediffOn != null)
				{
					part = this.parent.pawn.RaceProps.body.AllParts.FirstOrFallback((BodyPartRecord p) => p.def == this.Props.createHediffOn, null);
				}
				this.parent.pawn.health.AddHediff(this.Props.createHediff, part, null, null);
			}
			if (this.Props.vomit)
			{
				this.parent.pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced, null, true, true, null, null, false, false, null, false, true, false);
			}
		}
	}
}
