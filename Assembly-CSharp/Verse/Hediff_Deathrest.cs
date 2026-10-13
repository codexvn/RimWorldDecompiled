using System;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000742 RID: 1858
	public class Hediff_Deathrest : HediffWithComps
	{
		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06002D5E RID: 11614 RVA: 0x000EA100 File Offset: 0x000E8300
		private Gene_Deathrest DeathrestGene
		{
			get
			{
				Gene_Deathrest result;
				if ((result = this.cachedGene) == null)
				{
					Pawn_GeneTracker genes = this.pawn.genes;
					result = (this.cachedGene = ((genes != null) ? genes.GetFirstGeneOfType<Gene_Deathrest>() : null));
				}
				return result;
			}
		}

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x06002D5F RID: 11615 RVA: 0x000EA137 File Offset: 0x000E8337
		public bool Paused
		{
			get
			{
				if (this.lastPauseCheckTick < Find.TickManager.TicksGame + 120)
				{
					this.lastPauseCheckTick = Find.TickManager.TicksGame;
					this.cachedPaused = SanguophageUtility.ShouldBeDeathrestingOrInComaInsteadOfDead(this.pawn);
				}
				return this.cachedPaused;
			}
		}

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x06002D60 RID: 11616 RVA: 0x000EA175 File Offset: 0x000E8375
		public override string LabelInBrackets
		{
			get
			{
				if (this.Paused)
				{
					return base.LabelInBrackets;
				}
				return this.DeathrestGene.DeathrestPercent.ToStringPercent("F0");
			}
		}

		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x06002D61 RID: 11617 RVA: 0x000EA19C File Offset: 0x000E839C
		public override string TipStringExtra
		{
			get
			{
				string text = base.TipStringExtra;
				if (this.Paused)
				{
					if (!text.NullOrEmpty())
					{
						text += "\n";
					}
					text += "PawnWillKeepDeathrestingLethalInjuries".Translate(this.pawn.Named("PAWN")).Colorize(ColorLibrary.RedReadable);
				}
				return text;
			}
		}

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x06002D62 RID: 11618 RVA: 0x000EA1F8 File Offset: 0x000E83F8
		public override bool ShouldRemove
		{
			get
			{
				return this.DeathrestGene == null || base.ShouldRemove;
			}
		}

		// Token: 0x06002D63 RID: 11619 RVA: 0x000EA20A File Offset: 0x000E840A
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			Gene_Deathrest deathrestGene = this.DeathrestGene;
			if (deathrestGene == null)
			{
				return;
			}
			deathrestGene.Notify_DeathrestStarted();
		}

		// Token: 0x06002D64 RID: 11620 RVA: 0x000EA224 File Offset: 0x000E8424
		public override void PostRemoved()
		{
			base.PostRemoved();
			Gene_Deathrest deathrestGene = this.DeathrestGene;
			if (deathrestGene != null)
			{
				deathrestGene.Notify_DeathrestEnded();
			}
			if (this.pawn.Spawned && this.pawn.CurJobDef == JobDefOf.Deathrest)
			{
				this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x06002D65 RID: 11621 RVA: 0x000EA27C File Offset: 0x000E847C
		public override void PostTickInterval(int delta)
		{
			base.PostTickInterval(delta);
			bool paused = this.Paused;
			Gene_Deathrest deathrestGene = this.DeathrestGene;
			if (deathrestGene == null)
			{
				return;
			}
			deathrestGene.TickDeathresting(paused, delta);
		}

		// Token: 0x040023FD RID: 9213
		private Gene_Deathrest cachedGene;

		// Token: 0x040023FE RID: 9214
		private int lastPauseCheckTick = -1;

		// Token: 0x040023FF RID: 9215
		private bool cachedPaused;

		// Token: 0x04002400 RID: 9216
		private const int PauseCheckInterval = 120;
	}
}
