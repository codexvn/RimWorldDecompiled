using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000696 RID: 1686
	public class HediffComp_ChangeImplantLevel : HediffComp
	{
		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x06002A7A RID: 10874 RVA: 0x000DF957 File Offset: 0x000DDB57
		public HediffCompProperties_ChangeImplantLevel Props
		{
			get
			{
				return (HediffCompProperties_ChangeImplantLevel)this.props;
			}
		}

		// Token: 0x06002A7B RID: 10875 RVA: 0x000DF964 File Offset: 0x000DDB64
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			float mtbDays = this.Props.probabilityPerStage[this.parent.CurStageIndex].mtbDays;
			if (mtbDays <= 0f || !base.Pawn.IsHashIntervalTick(60, delta))
			{
				return;
			}
			ChangeImplantLevel_Probability changeImplantLevel_Probability = this.Props.probabilityPerStage[this.parent.CurStageIndex];
			if ((this.lastChangeLevelTick < 0 || (float)(Find.TickManager.TicksGame - this.lastChangeLevelTick) >= changeImplantLevel_Probability.minIntervalDays * 60000f) && Rand.MTBEventOccurs(mtbDays, 60000f, 60f))
			{
				Hediff_Level hediff_Level = this.parent.pawn.health.hediffSet.GetFirstHediffOfDef(this.Props.implant, false) as Hediff_Level;
				if (hediff_Level != null)
				{
					hediff_Level.ChangeLevel(this.Props.levelOffset);
					this.lastChangeLevelTick = Find.TickManager.TicksGame;
					Messages.Message("MessageLostImplantLevelFromHediff".Translate(this.parent.pawn.Named("PAWN"), hediff_Level.LabelBase, this.parent.Label), this.parent.pawn, MessageTypeDefOf.NegativeEvent, true);
				}
			}
		}

		// Token: 0x06002A7C RID: 10876 RVA: 0x000DFAB2 File Offset: 0x000DDCB2
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<int>(ref this.lastChangeLevelTick, "lastChangeLevelTick", 0, false);
		}

		// Token: 0x0400226F RID: 8815
		public int lastChangeLevelTick = -1;
	}
}
