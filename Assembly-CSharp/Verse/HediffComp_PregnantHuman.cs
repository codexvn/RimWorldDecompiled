using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020006EE RID: 1774
	public class HediffComp_PregnantHuman : HediffComp
	{
		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x06002BB3 RID: 11187 RVA: 0x000E3E99 File Offset: 0x000E2099
		public PregnancyAttitude? Attitude
		{
			get
			{
				return this.pregnancyAttitude;
			}
		}

		// Token: 0x06002BB4 RID: 11188 RVA: 0x000E3EA1 File Offset: 0x000E20A1
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.SetupPregnancyAttitude();
		}

		// Token: 0x06002BB5 RID: 11189 RVA: 0x000E3EB0 File Offset: 0x000E20B0
		private void SetupPregnancyAttitude()
		{
			this.pregnancyAttitude = new PregnancyAttitude?((Rand.Value < 0.5f) ? PregnancyAttitude.Positive : PregnancyAttitude.Negative);
		}

		// Token: 0x06002BB6 RID: 11190 RVA: 0x000E3ED0 File Offset: 0x000E20D0
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (!ModLister.CheckBiotech("Human pregnancy"))
			{
				return;
			}
			if (base.Pawn.IsHashIntervalTick(60, delta))
			{
				if ((this.lastGivenMorningSicknessTick == -1 || GenTicks.TicksGame - this.lastGivenMorningSicknessTick >= 48000) && Rand.MTBEventOccurs(HediffComp_PregnantHuman.MorningSicknessMTBDaysPerStage[this.parent.CurStageIndex], 60000f, 60f))
				{
					base.Pawn.health.AddHediff(HediffDefOf.MorningSickness, null, null, null);
					this.lastGivenMorningSicknessTick = GenTicks.TicksGame;
				}
				if ((this.lastGivenPregnancyMoodTick == -1 || GenTicks.TicksGame - this.lastGivenPregnancyMoodTick >= 48000) && Rand.MTBEventOccurs(HediffComp_PregnantHuman.PregnancyMoodMTBDaysPerStage[this.parent.CurStageIndex], 60000f, 60f))
				{
					base.Pawn.health.AddHediff(HediffDefOf.PregnancyMood, base.Pawn.health.hediffSet.GetBrain(), null, null);
					this.lastGivenPregnancyMoodTick = GenTicks.TicksGame;
				}
			}
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x06002BB7 RID: 11191 RVA: 0x000E3FE8 File Offset: 0x000E21E8
		public override string CompTipStringExtra
		{
			get
			{
				Hediff_Pregnant hediff_Pregnant = (Hediff_Pregnant)this.parent;
				TaggedString t = "\n" + "FatherTip".Translate() + ": " + ((hediff_Pregnant.Father != null) ? hediff_Pregnant.Father.LabelShort.Colorize(ColoredText.NameColor) : "Unknown".Translate().ToString()).CapitalizeFirst();
				if (hediff_Pregnant.Mother != null && hediff_Pregnant.Mother != this.parent.pawn)
				{
					t += "\n" + "MotherTip".Translate() + ": " + hediff_Pregnant.Mother.LabelShort.CapitalizeFirst().Colorize(ColoredText.NameColor);
				}
				return t.Resolve();
			}
		}

		// Token: 0x06002BB8 RID: 11192 RVA: 0x000E40C8 File Offset: 0x000E22C8
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<int>(ref this.lastGivenMorningSicknessTick, "lastGivenMorningSicknessTick", -1, false);
			Scribe_Values.Look<int>(ref this.lastGivenPregnancyMoodTick, "lastGivenPregnancyMoodTick", -1, false);
			Scribe_Values.Look<PregnancyAttitude?>(ref this.pregnancyAttitude, "pregnancyAttitude", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.pregnancyAttitude == null)
			{
				this.SetupPregnancyAttitude();
			}
		}

		// Token: 0x0400232F RID: 9007
		private int lastGivenMorningSicknessTick = -1;

		// Token: 0x04002330 RID: 9008
		private int lastGivenPregnancyMoodTick = -1;

		// Token: 0x04002331 RID: 9009
		private PregnancyAttitude? pregnancyAttitude;

		// Token: 0x04002332 RID: 9010
		private const int MorningSicknessMinIntervalTicks = 48000;

		// Token: 0x04002333 RID: 9011
		private static readonly float[] MorningSicknessMTBDaysPerStage = new float[]
		{
			4f,
			8f,
			8f
		};

		// Token: 0x04002334 RID: 9012
		private const int PregnancyMoodMinIntervalTicks = 48000;

		// Token: 0x04002335 RID: 9013
		private static readonly float[] PregnancyMoodMTBDaysPerStage = new float[]
		{
			4f,
			8f,
			8f
		};
	}
}
