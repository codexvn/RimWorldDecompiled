using System;

namespace Verse
{
	// Token: 0x020006FA RID: 1786
	public class HediffComp_RemoveIfOtherHediff : HediffComp_MessageBase
	{
		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x06002BD8 RID: 11224 RVA: 0x000E456F File Offset: 0x000E276F
		protected HediffCompProperties_RemoveIfOtherHediff Props
		{
			get
			{
				return (HediffCompProperties_RemoveIfOtherHediff)this.props;
			}
		}

		// Token: 0x06002BD9 RID: 11225 RVA: 0x000E457C File Offset: 0x000E277C
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (this.ShouldRemove(delta))
			{
				this.Message();
				this.parent.pawn.health.RemoveHediff(this.parent);
			}
		}

		// Token: 0x06002BDA RID: 11226 RVA: 0x000E45A8 File Offset: 0x000E27A8
		private bool ShouldRemove(int delta)
		{
			if (base.CompShouldRemove)
			{
				return true;
			}
			foreach (HediffDef def in this.Props.hediffs)
			{
				Hediff firstHediffOfDef = base.Pawn.health.hediffSet.GetFirstHediffOfDef(def, false);
				if (firstHediffOfDef != null && (this.Props.stages == null || this.Props.stages.Value.Includes(firstHediffOfDef.CurStageIndex)) && (this.Props.mtbHours <= 0 || (base.Pawn.IsHashIntervalTick(1000, delta) && Rand.MTBEventOccurs((float)this.Props.mtbHours, 2500f, 1000f))))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04002342 RID: 9026
		private const int MtbRemovalCheckInterval = 1000;
	}
}
