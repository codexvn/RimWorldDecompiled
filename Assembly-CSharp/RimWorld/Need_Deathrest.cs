using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002479 RID: 9337
	public class Need_Deathrest : Need
	{
		// Token: 0x17001EB6 RID: 7862
		// (get) Token: 0x0600CA8C RID: 51852 RVA: 0x003B53F4 File Offset: 0x003B35F4
		public bool Deathresting
		{
			get
			{
				return Find.TickManager.TicksGame <= this.lastDeathrestTick + this.pawn.UpdateRateTicks;
			}
		}

		// Token: 0x17001EB7 RID: 7863
		// (get) Token: 0x0600CA8D RID: 51853 RVA: 0x003B5417 File Offset: 0x003B3617
		private Gene_Deathrest DeathrestGene
		{
			get
			{
				if (this.cachedDeathrestGene == null)
				{
					Pawn_GeneTracker genes = this.pawn.genes;
					this.cachedDeathrestGene = ((genes != null) ? genes.GetFirstGeneOfType<Gene_Deathrest>() : null);
				}
				return this.cachedDeathrestGene;
			}
		}

		// Token: 0x17001EB8 RID: 7864
		// (get) Token: 0x0600CA8E RID: 51854 RVA: 0x003B5444 File Offset: 0x003B3644
		public override int GUIChangeArrow
		{
			get
			{
				if (this.IsFrozen)
				{
					return 0;
				}
				if (!this.Deathresting)
				{
					return -1;
				}
				return 1;
			}
		}

		// Token: 0x0600CA8F RID: 51855 RVA: 0x003B545B File Offset: 0x003B365B
		public Need_Deathrest(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>
			{
				0.1f
			};
		}

		// Token: 0x0600CA90 RID: 51856 RVA: 0x003B5485 File Offset: 0x003B3685
		public override void SetInitialLevel()
		{
			this.CurLevel = 1f;
		}

		// Token: 0x0600CA91 RID: 51857 RVA: 0x003B5494 File Offset: 0x003B3694
		public override void NeedInterval()
		{
			if (this.IsFrozen)
			{
				return;
			}
			float curLevel = this.CurLevel;
			float num;
			if (!this.Deathresting)
			{
				num = -0.033333335f;
			}
			else
			{
				float num2 = 0.2f;
				Gene_Deathrest deathrestGene = this.DeathrestGene;
				num = num2 * ((deathrestGene != null) ? deathrestGene.DeathrestEfficiency : 1f);
			}
			this.CurLevel = curLevel + num / 400f;
			this.CheckForStateChange();
		}

		// Token: 0x0600CA92 RID: 51858 RVA: 0x003B54F0 File Offset: 0x003B36F0
		private void CheckForStateChange()
		{
			Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DeathrestExhaustion, false);
			if (firstHediffOfDef != null && this.CurLevel > 0f)
			{
				firstHediffOfDef.Severity = 0f;
				return;
			}
			if (this.CurLevel == 0f)
			{
				Gene_Deathrest deathrestGene = this.DeathrestGene;
				if (deathrestGene != null)
				{
					deathrestGene.RemoveOldDeathrestBonuses();
				}
				this.pawn.health.AddHediff(HediffDefOf.DeathrestExhaustion, null, null, null);
			}
		}

		// Token: 0x0600CA93 RID: 51859 RVA: 0x003B5574 File Offset: 0x003B3774
		public override string GetTipString()
		{
			string str = (base.LabelCap + ": " + base.CurLevelPercentage.ToStringPercent()).Colorize(ColoredText.TipSectionTitleColor) + "\n";
			if (!this.Deathresting)
			{
				if (base.CurLevelPercentage > 0.1f)
				{
					float num = (base.CurLevelPercentage - 0.1f) / 0.033333335f;
					str += "NextDeathrestNeed".Translate(this.pawn.Named("PAWN"), "PeriodDays".Translate(num.ToString("F1")).Named("DURATION")).Resolve().CapitalizeFirst();
				}
				else
				{
					str += "PawnShouldDeathrestNow".Translate(this.pawn.Named("PAWN")).CapitalizeFirst().Colorize(ColorLibrary.RedReadable);
				}
				str += "\n\n";
			}
			return str + this.def.description;
		}

		// Token: 0x0600CA94 RID: 51860 RVA: 0x003B5686 File Offset: 0x003B3886
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastDeathrestTick, "lastDeathrestTick", -999, false);
		}

		// Token: 0x040089F3 RID: 35315
		public int lastDeathrestTick = -999;

		// Token: 0x040089F4 RID: 35316
		[Unsaved(false)]
		private Gene_Deathrest cachedDeathrestGene;

		// Token: 0x040089F5 RID: 35317
		public const float LevelForAlert = 0.1f;

		// Token: 0x040089F6 RID: 35318
		public const float FallPerDay = 0.033333335f;

		// Token: 0x040089F7 RID: 35319
		public const float GainPerDayDeathresting = 0.2f;

		// Token: 0x040089F8 RID: 35320
		private const float Interval = 400f;

		// Token: 0x040089F9 RID: 35321
		public const float HemogenGainPerDayDeathrest = 0.08f;
	}
}
