using System;

namespace Verse
{
	// Token: 0x020006EC RID: 1772
	public class HediffComp_Pollution : HediffComp
	{
		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x06002BB0 RID: 11184 RVA: 0x000E3E22 File Offset: 0x000E2022
		public HediffCompProperties_Pollution Props
		{
			get
			{
				return (HediffCompProperties_Pollution)this.props;
			}
		}

		// Token: 0x06002BB1 RID: 11185 RVA: 0x000E3E30 File Offset: 0x000E2030
		public override void CompPostTick(ref float severityAdjustment)
		{
			Pawn pawn = this.parent.pawn;
			if (!pawn.IsHashIntervalTick(this.Props.interval))
			{
				return;
			}
			if (pawn.Spawned && pawn.Position.IsPolluted(pawn.Map))
			{
				severityAdjustment += this.Props.pollutedSeverity;
				return;
			}
			severityAdjustment += this.Props.unpollutedSeverity;
		}
	}
}
