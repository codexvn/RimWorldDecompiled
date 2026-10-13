using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011F2 RID: 4594
	public class JobDriver_ViewArt : JobDriver_VisitJoyThing
	{
		// Token: 0x1700116A RID: 4458
		// (get) Token: 0x06007042 RID: 28738 RVA: 0x0022DA44 File Offset: 0x0022BC44
		private Thing ArtThing
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06007043 RID: 28739 RVA: 0x0022DA68 File Offset: 0x0022BC68
		protected override void WaitTickAction(int delta)
		{
			float num = this.ArtThing.GetStatValue(StatDefOf.Beauty, true, -1) / this.ArtThing.def.GetStatValueAbstract(StatDefOf.Beauty, null);
			float extraJoyGainFactor = (num > 0f) ? num : 0f;
			this.pawn.GainComfortFromCellIfPossible(delta, false);
			JoyUtility.JoyTickCheckEnd(this.pawn, delta, JoyTickFullJoyAction.EndJob, extraJoyGainFactor, (Building)this.ArtThing);
		}
	}
}
