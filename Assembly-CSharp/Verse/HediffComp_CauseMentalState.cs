using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000690 RID: 1680
	public class HediffComp_CauseMentalState : HediffComp
	{
		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06002A60 RID: 10848 RVA: 0x000DF13F File Offset: 0x000DD33F
		public HediffCompProperties_CauseMentalState Props
		{
			get
			{
				return (HediffCompProperties_CauseMentalState)this.props;
			}
		}

		// Token: 0x06002A61 RID: 10849 RVA: 0x000DF14C File Offset: 0x000DD34C
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (this.parent.Severity < this.Props.minSeverity)
			{
				return;
			}
			if (!base.Pawn.IsHashIntervalTick(60, delta))
			{
				return;
			}
			if (base.Pawn.RaceProps.Humanlike)
			{
				if (base.Pawn.mindState.mentalStateHandler.CurStateDef != this.Props.humanMentalState && Rand.MTBEventOccurs(this.Props.mtbDaysToCauseMentalState, 60000f, 60f) && base.Pawn.Awake() && base.Pawn.Spawned && base.Pawn.mindState.mentalStateHandler.TryStartMentalState(this.Props.humanMentalState, this.parent.def.LabelCap, this.Props.forced, false, false, null, true, false, false))
				{
					this.TriggeredMentalBreak(this.Props.humanMentalState);
					return;
				}
			}
			else if (base.Pawn.RaceProps.Animal && base.Pawn.mindState.mentalStateHandler.CurStateDef != this.Props.animalMentalState && (this.Props.animalMentalStateAlias == null || base.Pawn.mindState.mentalStateHandler.CurStateDef != this.Props.animalMentalStateAlias) && Rand.MTBEventOccurs(this.Props.mtbDaysToCauseMentalState, 60000f, 60f) && base.Pawn.Awake() && base.Pawn.Spawned && base.Pawn.mindState.mentalStateHandler.TryStartMentalState(this.Props.animalMentalState, this.parent.def.LabelCap, false, false, false, null, true, false, false))
			{
				this.TriggeredMentalBreak(this.Props.animalMentalState);
			}
		}

		// Token: 0x06002A62 RID: 10850 RVA: 0x000DF34C File Offset: 0x000DD54C
		public override void CompPostPostRemoved()
		{
			if (this.Props.endMentalStateOnCure && ((base.Pawn.RaceProps.Humanlike && base.Pawn.mindState.mentalStateHandler.CurStateDef == this.Props.humanMentalState) || (base.Pawn.RaceProps.Animal && (base.Pawn.mindState.mentalStateHandler.CurStateDef == this.Props.animalMentalState || base.Pawn.mindState.mentalStateHandler.CurStateDef == this.Props.animalMentalStateAlias))) && !base.Pawn.mindState.mentalStateHandler.CurState.causedByMood)
			{
				base.Pawn.mindState.mentalStateHandler.CurState.RecoverFromState();
			}
		}

		// Token: 0x06002A63 RID: 10851 RVA: 0x000DF429 File Offset: 0x000DD629
		private void TriggeredMentalBreak(MentalStateDef mentalStateDef)
		{
			this.SendLetter(mentalStateDef);
			if (this.Props.removeOnTriggered)
			{
				base.Pawn.health.RemoveHediff(this.parent);
			}
		}

		// Token: 0x06002A64 RID: 10852 RVA: 0x000DF458 File Offset: 0x000DD658
		private void SendLetter(MentalStateDef mentalStateDef)
		{
			string str;
			if (!string.IsNullOrEmpty(this.Props.overrideLetterLabel))
			{
				str = this.Props.overrideLetterLabel.Formatted(base.Pawn.Named("PAWN")).Resolve();
			}
			else
			{
				str = (mentalStateDef.beginLetterLabel ?? mentalStateDef.LabelCap).CapitalizeFirst() + ": " + base.Pawn.LabelShortCap;
			}
			string str2;
			if (!string.IsNullOrEmpty(this.Props.overrideLetterDesc))
			{
				str2 = this.Props.overrideLetterDesc.Formatted(base.Pawn.Named("PAWN")).Resolve();
			}
			else
			{
				str2 = string.Format("{0}\n\n{1}", base.Pawn.mindState.mentalStateHandler.CurState.GetBeginLetterText(), "CausedByHediff".Translate(this.parent.LabelCap));
			}
			Find.LetterStack.ReceiveLetter(str, str2, this.Props.letterDef ?? mentalStateDef.beginLetterDef, base.Pawn, null, null, null, null, 0, true);
		}
	}
}
