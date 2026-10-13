using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E87 RID: 7815
	public class StoryWatcher_Adaptation : IExposable
	{
		// Token: 0x17001A83 RID: 6787
		// (get) Token: 0x0600AD82 RID: 44418 RVA: 0x003186F7 File Offset: 0x003168F7
		public float TotalThreatPointsFactor
		{
			get
			{
				return this.StorytellerDef.pointsFactorFromAdaptDays.Evaluate(this.adaptDays);
			}
		}

		// Token: 0x17001A84 RID: 6788
		// (get) Token: 0x0600AD83 RID: 44419 RVA: 0x0031870F File Offset: 0x0031690F
		public float AdaptDays
		{
			get
			{
				return this.adaptDays;
			}
		}

		// Token: 0x17001A85 RID: 6789
		// (get) Token: 0x0600AD84 RID: 44420 RVA: 0x00318717 File Offset: 0x00316917
		private int Population
		{
			get
			{
				return PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists.Count<Pawn>();
			}
		}

		// Token: 0x17001A86 RID: 6790
		// (get) Token: 0x0600AD85 RID: 44421 RVA: 0x00317C10 File Offset: 0x00315E10
		private StorytellerDef StorytellerDef
		{
			get
			{
				return Find.Storyteller.def;
			}
		}

		// Token: 0x0600AD86 RID: 44422 RVA: 0x00318724 File Offset: 0x00316924
		public void Notify_PawnEvent(Pawn p, AdaptationEvent ev, DamageInfo? dinfo = null)
		{
			if (!p.RaceProps.Humanlike || !p.IsColonist || p.IsPrisoner)
			{
				return;
			}
			if (ev != AdaptationEvent.Downed)
			{
				this.ResolvePawnEvent(p, ev);
				return;
			}
			if (dinfo == null || !dinfo.Value.Def.ExternalViolenceFor(p))
			{
				return;
			}
			this.pawnsJustDownedThisTick.Add(p);
		}

		// Token: 0x0600AD87 RID: 44423 RVA: 0x00318788 File Offset: 0x00316988
		private void ResolvePawnEvent(Pawn p, AdaptationEvent ev)
		{
			float num;
			if (ev == AdaptationEvent.Downed)
			{
				num = this.StorytellerDef.adaptDaysLossFromColonistViolentlyDownedByPopulation.Evaluate((float)this.Population);
			}
			else
			{
				if (this.pawnsJustDownedThisTick.Contains(p))
				{
					this.pawnsJustDownedThisTick.Remove(p);
				}
				int num2 = this.Population - 1;
				num = this.StorytellerDef.adaptDaysLossFromColonistLostByPostPopulation.Evaluate((float)num2);
			}
			if (DebugViewSettings.writeStoryteller)
			{
				Log.Message(string.Concat(new string[]
				{
					"Adaptation event: ",
					(p != null) ? p.ToString() : null,
					" ",
					ev.ToString(),
					". Loss: ",
					num.ToString("F1"),
					" from ",
					this.adaptDays.ToString("F1")
				}));
			}
			this.adaptDays = Mathf.Max(this.StorytellerDef.adaptDaysMin, this.adaptDays - num);
		}

		// Token: 0x0600AD88 RID: 44424 RVA: 0x00318880 File Offset: 0x00316A80
		public void AdaptationWatcherTick()
		{
			for (int i = 0; i < this.pawnsJustDownedThisTick.Count; i++)
			{
				this.ResolvePawnEvent(this.pawnsJustDownedThisTick[i], AdaptationEvent.Downed);
			}
			this.pawnsJustDownedThisTick.Clear();
			if (Find.TickManager.TicksGame % 30000 == 0)
			{
				if (this.adaptDays >= 0f && (float)GenDate.DaysPassedSinceSettle < this.StorytellerDef.adaptDaysGameStartGraceDays)
				{
					return;
				}
				float num = 0.5f * this.StorytellerDef.adaptDaysGrowthRateCurve.Evaluate(this.adaptDays);
				if (this.adaptDays > 0f)
				{
					num *= Find.Storyteller.difficulty.adaptationGrowthRateFactorOverZero;
				}
				this.adaptDays += num;
				this.adaptDays = Mathf.Min(this.adaptDays, this.StorytellerDef.adaptDaysMax);
			}
		}

		// Token: 0x0600AD89 RID: 44425 RVA: 0x0031895D File Offset: 0x00316B5D
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.adaptDays, "adaptDays", 0f, false);
		}

		// Token: 0x0600AD8A RID: 44426 RVA: 0x00318975 File Offset: 0x00316B75
		public void Debug_OffsetAdaptDays(float days)
		{
			this.adaptDays += days;
		}

		// Token: 0x0600AD8B RID: 44427 RVA: 0x00318985 File Offset: 0x00316B85
		public void ResetAdaptDays()
		{
			this.adaptDays = 0f;
		}

		// Token: 0x0400776A RID: 30570
		private float adaptDays;

		// Token: 0x0400776B RID: 30571
		private List<Pawn> pawnsJustDownedThisTick = new List<Pawn>();

		// Token: 0x0400776C RID: 30572
		private const int UpdateInterval = 30000;
	}
}
