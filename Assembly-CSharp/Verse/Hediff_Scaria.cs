using System;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000769 RID: 1897
	public class Hediff_Scaria : Hediff
	{
		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06002E7F RID: 11903 RVA: 0x00002501 File Offset: 0x00000701
		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06002E80 RID: 11904 RVA: 0x000EF0D8 File Offset: 0x000ED2D8
		public override string TipStringExtra
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (this.TicksLeft >= 0)
				{
					stringBuilder.Append("DeathIn".Translate(this.TicksLeft.ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor)).Resolve().CapitalizeFirst());
				}
				string tipStringExtra = base.TipStringExtra;
				if (!tipStringExtra.NullOrEmpty())
				{
					stringBuilder.AppendLineIfNotEmpty();
					stringBuilder.AppendLineIfNotEmpty();
					stringBuilder.Append(tipStringExtra);
				}
				stringBuilder.AppendLineIfNotEmpty();
				stringBuilder.AppendLineIfNotEmpty();
				stringBuilder.Append("  - " + "HarmedRevengeChance".Translate() + ": " + 0.5f.ToStringPercentSigned());
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06002E81 RID: 11905 RVA: 0x000EF1A3 File Offset: 0x000ED3A3
		private int TicksLeft
		{
			get
			{
				return 300000 - (Find.TickManager.TicksGame - this.manhunterTick);
			}
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06002E82 RID: 11906 RVA: 0x000EF1BC File Offset: 0x000ED3BC
		private bool IsBerserk
		{
			get
			{
				return (this.pawn.RaceProps.Humanlike && this.pawn.mindState.mentalStateHandler.CurStateDef == MentalStateDefOf.BerserkPermanent) || (this.pawn.RaceProps.Animal && this.pawn.mindState.mentalStateHandler.CurStateDef == MentalStateDefOf.ManhunterPermanent);
			}
		}

		// Token: 0x06002E83 RID: 11907 RVA: 0x000EF229 File Offset: 0x000ED429
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.manhunterTick, "manhunterTick", 0, false);
		}

		// Token: 0x06002E84 RID: 11908 RVA: 0x000EF244 File Offset: 0x000ED444
		public override void TickInterval(int delta)
		{
			if (this.pawn.RaceProps.Humanlike && !this.pawn.mindState.mentalStateHandler.InMentalState && !this.pawn.Downed)
			{
				this.StartMentalState();
			}
			if (this.pawn.IsAnimal && this.pawn.Faction != null)
			{
				this.StartMentalState();
			}
			if (this.manhunterTick < 0 && this.IsBerserk)
			{
				this.manhunterTick = Find.TickManager.TicksGame;
			}
			if (this.manhunterTick > 0 && !this.IsBerserk)
			{
				this.manhunterTick = -999999;
			}
			if (this.manhunterTick > 0 && Find.TickManager.TicksGame - this.manhunterTick > 300000)
			{
				this.pawn.Kill(null, this);
			}
		}

		// Token: 0x06002E85 RID: 11909 RVA: 0x000EF320 File Offset: 0x000ED520
		private void StartMentalState()
		{
			this.pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.BerserkPermanent, "CausedByHediff".Translate(this.def.LabelCap), true, true, false, null, true, false, false);
		}

		// Token: 0x06002E86 RID: 11910 RVA: 0x000EF36E File Offset: 0x000ED56E
		public override void PostRemoved()
		{
			if (this.IsBerserk && !this.pawn.mindState.mentalStateHandler.CurState.causedByMood)
			{
				this.pawn.mindState.mentalStateHandler.CurState.RecoverFromState();
			}
		}

		// Token: 0x06002E87 RID: 11911 RVA: 0x000EF3B0 File Offset: 0x000ED5B0
		public override void Notify_PawnDamagedThing(Thing thing, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			if (result.hediffs.NullOrEmpty<Hediff>())
			{
				return;
			}
			foreach (Hediff hd in result.hediffs)
			{
				if (dinfo.Def == DamageDefOf.Bite || dinfo.Def == DamageDefOf.Scratch || dinfo.Def == DamageDefOf.ScratchToxic)
				{
					HediffComp_Infecter hediffComp_Infecter = hd.TryGetComp<HediffComp_Infecter>();
					if (hediffComp_Infecter != null)
					{
						hediffComp_Infecter.fromScaria = true;
					}
				}
			}
		}

		// Token: 0x0400246A RID: 9322
		public const float ScariaManhunterChanceOffset = 0.5f;

		// Token: 0x0400246B RID: 9323
		private const int KillAfterDaysManhunter = 5;

		// Token: 0x0400246C RID: 9324
		private int manhunterTick = -999999;
	}
}
