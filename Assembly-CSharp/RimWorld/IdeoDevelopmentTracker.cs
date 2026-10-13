using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020025E8 RID: 9704
	public class IdeoDevelopmentTracker : IExposable
	{
		// Token: 0x170020AE RID: 8366
		// (get) Token: 0x0600D4D8 RID: 54488 RVA: 0x003ED882 File Offset: 0x003EBA82
		public int Points
		{
			get
			{
				return this.points;
			}
		}

		// Token: 0x170020AF RID: 8367
		// (get) Token: 0x0600D4D9 RID: 54489 RVA: 0x003ED88A File Offset: 0x003EBA8A
		public bool CanReformNow
		{
			get
			{
				return this.Points >= this.NextReformationDevelopmentPoints;
			}
		}

		// Token: 0x170020B0 RID: 8368
		// (get) Token: 0x0600D4DA RID: 54490 RVA: 0x003ED89D File Offset: 0x003EBA9D
		public int NextReformationDevelopmentPoints
		{
			get
			{
				return Mathf.FloorToInt(IdeoDevelopmentTracker.DevelopmentPointsOverReformCountCurve.Evaluate((float)this.reformCount));
			}
		}

		// Token: 0x170020B1 RID: 8369
		// (get) Token: 0x0600D4DB RID: 54491 RVA: 0x003ED8B5 File Offset: 0x003EBAB5
		public bool CanBeDevelopedNow
		{
			get
			{
				return this.ideo.ColonistBelieverCountCached > 0;
			}
		}

		// Token: 0x0600D4DC RID: 54492 RVA: 0x003ED8C5 File Offset: 0x003EBAC5
		public IdeoDevelopmentTracker()
		{
		}

		// Token: 0x0600D4DD RID: 54493 RVA: 0x003ED8D8 File Offset: 0x003EBAD8
		public IdeoDevelopmentTracker(Ideo ideo)
		{
			this.ideo = ideo;
		}

		// Token: 0x0600D4DE RID: 54494 RVA: 0x003ED8F2 File Offset: 0x003EBAF2
		public void Notify_MemberGainedByConversion()
		{
			this.TryAddDevelopmentPoints(1);
		}

		// Token: 0x0600D4DF RID: 54495 RVA: 0x003ED8FC File Offset: 0x003EBAFC
		public void Notify_Reformed()
		{
			this.ResetDevelopmentPoints();
			this.reformCount++;
			foreach (Precept precept in this.ideo.PreceptsListForReading)
			{
				precept.Notify_IdeoReformed();
			}
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoCryptosleep)
			{
				if (pawn.Ideo == this.ideo && !pawn.IsSlave && !pawn.IsQuestLodger())
				{
					pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
				}
			}
		}

		// Token: 0x0600D4E0 RID: 54496 RVA: 0x003ED9D8 File Offset: 0x003EBBD8
		public void Notify_PreReform(Ideo newIdeo)
		{
			if (!IdeoDevelopmentTracker.<Notify_PreReform>g__ApprovesOfRaiding|18_0(this.ideo) && IdeoDevelopmentTracker.<Notify_PreReform>g__ApprovesOfRaiding|18_0(newIdeo))
			{
				this.lastTickRaidingApproved = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x0600D4E1 RID: 54497 RVA: 0x003EDA00 File Offset: 0x003EBC00
		public bool TryGainDevelopmentPointsForRitualOutcome(Precept_Ritual ritual, int outcomeIndex, out int developmentPoints)
		{
			developmentPoints = 0;
			if (!this.CanBeDevelopedNow)
			{
				return false;
			}
			SimpleCurve developmentPointsOverOutcomeIndexCurveForRitual = IdeoDevelopmentUtility.GetDevelopmentPointsOverOutcomeIndexCurveForRitual(this.ideo, ritual);
			if (developmentPointsOverOutcomeIndexCurveForRitual != null)
			{
				developmentPoints = Mathf.FloorToInt(developmentPointsOverOutcomeIndexCurveForRitual.Evaluate((float)outcomeIndex));
				int value = this.Points;
				if (developmentPoints > 0 && this.TryAddDevelopmentPoints(developmentPoints))
				{
					Messages.Message("MessageDevelopmentPointsEarned".Translate(value, this.Points, ritual.Label), MessageTypeDefOf.PositiveEvent, true);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600D4E2 RID: 54498 RVA: 0x003EDA88 File Offset: 0x003EBC88
		public bool TryAddDevelopmentPoints(int pointsToAdd)
		{
			if (pointsToAdd <= 0 || this.CanReformNow)
			{
				return false;
			}
			bool canReformNow = this.CanReformNow;
			this.points = Mathf.Min(this.points + pointsToAdd, this.NextReformationDevelopmentPoints);
			if (!canReformNow && this.CanReformNow)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelReformIdeo".Translate(), "LetterTextReformIdeo".Translate(this.ideo), LetterDefOf.PositiveEvent, null, 0, true);
			}
			return true;
		}

		// Token: 0x0600D4E3 RID: 54499 RVA: 0x003EDAFE File Offset: 0x003EBCFE
		public void ResetDevelopmentPoints()
		{
			this.points = 0;
		}

		// Token: 0x0600D4E4 RID: 54500 RVA: 0x003EDB07 File Offset: 0x003EBD07
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.points, "points", 0, false);
			Scribe_Values.Look<int>(ref this.reformCount, "reformCount", 0, false);
			Scribe_Values.Look<int>(ref this.lastTickRaidingApproved, "lastTickRaidingApproved", -9999999, false);
		}

		// Token: 0x0600D4E5 RID: 54501 RVA: 0x003EDB43 File Offset: 0x003EBD43
		public void CopyTo(IdeoDevelopmentTracker other)
		{
			other.points = this.points;
			other.reformCount = this.reformCount;
			other.lastTickRaidingApproved = this.lastTickRaidingApproved;
		}

		// Token: 0x0600D4E7 RID: 54503 RVA: 0x003EDC08 File Offset: 0x003EBE08
		[CompilerGenerated]
		internal static bool <Notify_PreReform>g__ApprovesOfRaiding|18_0(Ideo ideo)
		{
			List<Precept> preceptsListForReading = ideo.PreceptsListForReading;
			for (int i = 0; i < preceptsListForReading.Count; i++)
			{
				if (preceptsListForReading[i].def.approvesOfRaiding)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04009094 RID: 37012
		private const int DevelopmentPointsPerConversion = 1;

		// Token: 0x04009095 RID: 37013
		private static readonly SimpleCurve DevelopmentPointsOverReformCountCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 10f),
				true
			},
			{
				new CurvePoint(1f, 12f),
				true
			},
			{
				new CurvePoint(2f, 14f),
				true
			},
			{
				new CurvePoint(3f, 16f),
				true
			},
			{
				new CurvePoint(4f, 18f),
				true
			},
			{
				new CurvePoint(5f, 20f),
				true
			}
		};

		// Token: 0x04009096 RID: 37014
		public Ideo ideo;

		// Token: 0x04009097 RID: 37015
		public int points;

		// Token: 0x04009098 RID: 37016
		public int reformCount;

		// Token: 0x04009099 RID: 37017
		public int lastTickRaidingApproved = -9999999;
	}
}
