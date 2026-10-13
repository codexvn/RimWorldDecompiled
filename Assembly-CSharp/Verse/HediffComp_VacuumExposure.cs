using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000719 RID: 1817
	public class HediffComp_VacuumExposure : HediffComp
	{
		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x06002C57 RID: 11351 RVA: 0x000E618D File Offset: 0x000E438D
		public HediffCompProperties_VacuumExposure Props
		{
			get
			{
				return (HediffCompProperties_VacuumExposure)this.props;
			}
		}

		// Token: 0x06002C58 RID: 11352 RVA: 0x000E619C File Offset: 0x000E439C
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			if (base.Pawn.IsHashIntervalTick(60, delta))
			{
				float num = this.Props.severityPerSecondUnexposed;
				if (base.Pawn.Spawned && base.Pawn.Position.GetVacuum(base.Pawn.Map) >= 0.5f && base.Pawn.GetStatValue(StatDefOf.VacuumResistance, true, -1) < 1f)
				{
					num = 0f;
				}
				severityAdjustment += num;
			}
		}

		// Token: 0x0400238E RID: 9102
		private const int SeverityUpdateInterval = 60;
	}
}
