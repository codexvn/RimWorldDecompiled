using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002488 RID: 9352
	public class Need_Mood : Need_Seeker
	{
		// Token: 0x17001EDB RID: 7899
		// (get) Token: 0x0600CADE RID: 51934 RVA: 0x003B6B58 File Offset: 0x003B4D58
		public override float CurInstantLevel
		{
			get
			{
				if (this.lastInstantMoodCheckTick != Find.TickManager.TicksGame)
				{
					this.lastInstantMood = this.thoughts.TotalMoodOffset();
					if (this.pawn.IsColonist || this.pawn.IsPrisonerOfColony)
					{
						this.lastInstantMood += Find.Storyteller.difficulty.colonistMoodOffset;
					}
					this.lastInstantMood = Mathf.Clamp01((this.pawn.health.hediffSet.OverrideMoodBase ?? this.def.baseLevel) + this.lastInstantMood / 100f);
					this.lastInstantMoodCheckTick = Find.TickManager.TicksGame;
				}
				return this.lastInstantMood;
			}
		}

		// Token: 0x17001EDC RID: 7900
		// (get) Token: 0x0600CADF RID: 51935 RVA: 0x003B6C24 File Offset: 0x003B4E24
		public string MoodString
		{
			get
			{
				if (this.pawn.MentalStateDef != null)
				{
					return "Mood_MentalState".Translate();
				}
				float breakThresholdExtreme = this.pawn.mindState.mentalBreaker.BreakThresholdExtreme;
				if (this.CurLevel < breakThresholdExtreme)
				{
					return "Mood_AboutToBreak".Translate();
				}
				if (this.CurLevel < breakThresholdExtreme + 0.05f)
				{
					return "Mood_OnEdge".Translate();
				}
				if (this.CurLevel < this.pawn.mindState.mentalBreaker.BreakThresholdMinor)
				{
					return "Mood_Stressed".Translate();
				}
				if (this.CurLevel < 0.65f)
				{
					return "Mood_Neutral".Translate();
				}
				if (this.CurLevel < 0.9f)
				{
					return "Mood_Content".Translate();
				}
				return "Mood_Happy".Translate();
			}
		}

		// Token: 0x0600CAE0 RID: 51936 RVA: 0x003B6D14 File Offset: 0x003B4F14
		public Need_Mood(Pawn pawn) : base(pawn)
		{
			this.thoughts = new ThoughtHandler(pawn);
			this.observer = new PawnObserver(pawn);
			this.recentMemory = new PawnRecentMemory(pawn);
		}

		// Token: 0x0600CAE1 RID: 51937 RVA: 0x003B6D64 File Offset: 0x003B4F64
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThoughtHandler>(ref this.thoughts, "thoughts", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<PawnRecentMemory>(ref this.recentMemory, "recentMemory", new object[]
			{
				this.pawn
			});
		}

		// Token: 0x0600CAE2 RID: 51938 RVA: 0x003B6DB5 File Offset: 0x003B4FB5
		public override void NeedInterval()
		{
			base.NeedInterval();
			this.recentMemory.RecentMemoryInterval();
			this.thoughts.ThoughtInterval();
			this.observer.ObserverInterval();
		}

		// Token: 0x0600CAE3 RID: 51939 RVA: 0x003B6DE0 File Offset: 0x003B4FE0
		public override string GetTipString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			LifeStageDef curLifeStage = this.pawn.ageTracker.CurLifeStage;
			string text = (curLifeStage != null) ? curLifeStage.customMoodTipString : null;
			if (text != null)
			{
				stringBuilder.AppendLine((base.LabelCap + ": " + base.CurLevelPercentage.ToStringPercent()).Colorize(ColoredText.TipSectionTitleColor));
				stringBuilder.AppendLine(text);
			}
			else
			{
				stringBuilder.AppendLine(base.GetTipString());
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("MentalBreakThresholdExtreme".Translate() + ": " + this.pawn.mindState.mentalBreaker.BreakThresholdExtreme.ToStringPercent());
				stringBuilder.AppendLine("MentalBreakThresholdMajor".Translate() + ": " + this.pawn.mindState.mentalBreaker.BreakThresholdMajor.ToStringPercent());
				stringBuilder.AppendLine("MentalBreakThresholdMinor".Translate() + ": " + this.pawn.mindState.mentalBreaker.BreakThresholdMinor.ToStringPercent());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600CAE4 RID: 51940 RVA: 0x003B6F24 File Offset: 0x003B5124
		public override void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true, Rect? rectForTooltip = null, bool drawLabel = true)
		{
			if (this.threshPercents == null)
			{
				this.threshPercents = new List<float>();
			}
			this.threshPercents.Clear();
			if (this.pawn.mindState.mentalBreaker.CanDoRandomMentalBreaks)
			{
				this.threshPercents.Add(this.pawn.mindState.mentalBreaker.BreakThresholdExtreme);
				this.threshPercents.Add(this.pawn.mindState.mentalBreaker.BreakThresholdMajor);
				this.threshPercents.Add(this.pawn.mindState.mentalBreaker.BreakThresholdMinor);
			}
			base.DrawOnGUI(rect, maxThresholdMarkers, customMargin, drawArrows, doTooltip, rectForTooltip, drawLabel);
		}

		// Token: 0x04008A46 RID: 35398
		public ThoughtHandler thoughts;

		// Token: 0x04008A47 RID: 35399
		public PawnObserver observer;

		// Token: 0x04008A48 RID: 35400
		public PawnRecentMemory recentMemory;

		// Token: 0x04008A49 RID: 35401
		private int lastInstantMoodCheckTick = -9999;

		// Token: 0x04008A4A RID: 35402
		private float lastInstantMood = -1f;
	}
}
