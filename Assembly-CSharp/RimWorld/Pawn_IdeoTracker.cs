using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020024C1 RID: 9409
	public class Pawn_IdeoTracker : IExposable
	{
		// Token: 0x17001F45 RID: 8005
		// (get) Token: 0x0600CCC6 RID: 52422 RVA: 0x003C0BFF File Offset: 0x003BEDFF
		public Ideo Ideo
		{
			get
			{
				return this.ideo;
			}
		}

		// Token: 0x17001F46 RID: 8006
		// (get) Token: 0x0600CCC7 RID: 52423 RVA: 0x003C0C07 File Offset: 0x003BEE07
		public List<Ideo> PreviousIdeos
		{
			get
			{
				return this.previousIdeos;
			}
		}

		// Token: 0x17001F47 RID: 8007
		// (get) Token: 0x0600CCC8 RID: 52424 RVA: 0x003C0C0F File Offset: 0x003BEE0F
		// (set) Token: 0x0600CCC9 RID: 52425 RVA: 0x003C0C34 File Offset: 0x003BEE34
		public float Certainty
		{
			get
			{
				if (this.pawn.DevelopmentalStage.Baby())
				{
					this.certaintyInt = 0f;
				}
				return this.certaintyInt;
			}
			private set
			{
				if (this.pawn.DevelopmentalStage.Baby())
				{
					this.certaintyInt = 0f;
				}
				else
				{
					this.certaintyInt = value;
				}
				this.certaintyInt = Mathf.Clamp01(this.certaintyInt);
			}
		}

		// Token: 0x17001F48 RID: 8008
		// (get) Token: 0x0600CCCA RID: 52426 RVA: 0x003C0C70 File Offset: 0x003BEE70
		private float CertaintyChangeFactor
		{
			get
			{
				if (!ModsConfig.BiotechActive)
				{
					return 1f;
				}
				if (Pawn_IdeoTracker.pawnAgeCertaintyCurve == null)
				{
					Pawn_IdeoTracker.pawnAgeCertaintyCurve = new SimpleCurve
					{
						{
							new CurvePoint(this.pawn.ageTracker.LifeStageMinAge(LifeStageDefOf.HumanlikeChild), 2f),
							true
						},
						{
							new CurvePoint(this.pawn.ageTracker.LifeStageMinAge(LifeStageDefOf.HumanlikeAdult), 1f),
							true
						}
					};
				}
				return Pawn_IdeoTracker.pawnAgeCertaintyCurve.Evaluate(this.pawn.ageTracker.AgeBiologicalYearsFloat);
			}
		}

		// Token: 0x17001F49 RID: 8009
		// (get) Token: 0x0600CCCB RID: 52427 RVA: 0x003C0D01 File Offset: 0x003BEF01
		public float CertaintyChangePerDay
		{
			get
			{
				if (this.pawn.needs.mood != null)
				{
					return ConversionTuning.CertaintyPerDayByMoodCurve.Evaluate(this.pawn.needs.mood.CurLevelPercentage);
				}
				return 0f;
			}
		}

		// Token: 0x17001F4A RID: 8010
		// (get) Token: 0x0600CCCC RID: 52428 RVA: 0x003C0D3A File Offset: 0x003BEF3A
		public List<Pawn_IdeoTracker.IdeoExposureWeight> BabyIdeoExposureSorted
		{
			get
			{
				if (!this.IsBabyIdeoExposureSorted())
				{
					this.babyIdeoExposure.Sort();
					this.isIdeoExposureSortDirty = false;
				}
				return this.babyIdeoExposure;
			}
		}

		// Token: 0x17001F4B RID: 8011
		// (get) Token: 0x0600CCCD RID: 52429 RVA: 0x003C0D5C File Offset: 0x003BEF5C
		public float BabyIdeoExposureTotal
		{
			get
			{
				if (this.babyIdeoExposure.NullOrEmpty<Pawn_IdeoTracker.IdeoExposureWeight>())
				{
					return 0f;
				}
				float num = 0f;
				foreach (Pawn_IdeoTracker.IdeoExposureWeight ideoExposureWeight in this.babyIdeoExposure)
				{
					num += ideoExposureWeight.exposure;
				}
				return num;
			}
		}

		// Token: 0x0600CCCE RID: 52430 RVA: 0x003C0DCC File Offset: 0x003BEFCC
		public Pawn_IdeoTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.previousIdeos = new List<Ideo>();
		}

		// Token: 0x0600CCCF RID: 52431 RVA: 0x003C0DED File Offset: 0x003BEFED
		public Pawn_IdeoTracker()
		{
		}

		// Token: 0x0600CCD0 RID: 52432 RVA: 0x003C0DFC File Offset: 0x003BEFFC
		private float ApplyCertaintyChangeFactor(float certaintyOffset)
		{
			if (certaintyOffset <= 0f)
			{
				return certaintyOffset * this.CertaintyChangeFactor;
			}
			return certaintyOffset / this.CertaintyChangeFactor;
		}

		// Token: 0x0600CCD1 RID: 52433 RVA: 0x003C0E18 File Offset: 0x003BF018
		public void IdeoTrackerTickInterval(int delta)
		{
			if (!this.pawn.Destroyed && !this.pawn.InMentalState && this.ideo != null && !Find.IdeoManager.classicMode && !this.pawn.Deathresting)
			{
				this.Certainty += this.ApplyCertaintyChangeFactor(this.CertaintyChangePerDay / 60000f) * (float)delta;
			}
			if (this.ideo != null && this.ideo.currentCacheId != this.lastCacheId)
			{
				this.lastCacheId = this.ideo.currentCacheId;
				this.RecacheIdeoComponents();
			}
		}

		// Token: 0x0600CCD2 RID: 52434 RVA: 0x003C0EB4 File Offset: 0x003BF0B4
		public void SetIdeo(Ideo ideo)
		{
			if (this.ideo == ideo || this.pawn.DevelopmentalStage.Baby())
			{
				return;
			}
			if (this.ideo != null)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.ChangedIdeo, this.pawn.Named(HistoryEventArgsNames.Doer)), true);
			}
			if (this.previousIdeos.Contains(ideo))
			{
				this.previousIdeos.Remove(ideo);
			}
			if (this.ideo != null)
			{
				this.previousIdeos.Add(this.ideo);
			}
			Ideo ideo2 = this.ideo;
			if (this.pawn.Faction != null && this.pawn.Faction.IsPlayer)
			{
				Ideo ideo3 = this.ideo;
				if (ideo3 != null)
				{
					ideo3.Notify_MemberLost(this.pawn, this.pawn.Map);
				}
				Ideo ideo4 = this.ideo;
				if (ideo4 != null)
				{
					ideo4.RecacheColonistBelieverCount();
				}
			}
			this.ideo = ideo;
			this.Certainty = Mathf.Clamp01(ConversionTuning.InitialCertaintyRange.RandomInRange);
			if (this.pawn.Faction != null && this.pawn.Faction.IsPlayer)
			{
				this.pawn.Faction.ideos.Notify_ColonistChangedIdeo();
				if (ideo2 != null)
				{
					ideo2.RecacheColonistBelieverCount();
				}
				Ideo ideo5 = this.ideo;
				if (ideo5 != null)
				{
					ideo5.RecacheColonistBelieverCount();
				}
			}
			Ideo ideo6 = this.ideo;
			if (ideo6 != null)
			{
				ideo6.Notify_MemberGained(this.pawn);
			}
			if (this.pawn.ownership.OwnedBed != null && this.pawn.ownership.OwnedBed.CompAssignableToPawn.IdeoligionForbids(this.pawn))
			{
				this.pawn.ownership.UnclaimBed();
			}
			SpouseRelationUtility.RemoveSpousesAsForbiddenByIdeo(this.pawn);
			if (ideo != null && !ideo.MemberWillingToDo(new HistoryEvent(HistoryEventDefOf.Bonded)))
			{
				List<Pawn> list = new List<Pawn>();
				List<DirectPawnRelation> directRelations = this.pawn.relations.DirectRelations;
				for (int i = directRelations.Count - 1; i >= 0; i--)
				{
					DirectPawnRelation directPawnRelation = directRelations[i];
					if (directPawnRelation.def == PawnRelationDefOf.Bond)
					{
						list.Add(directPawnRelation.otherPawn);
						this.pawn.relations.RemoveDirectRelation(directPawnRelation);
					}
				}
				if (list.Count > 0)
				{
					Find.LetterStack.ReceiveLetter("LetterBondRemoved".Translate(), "LetterBondRemovedDesc".Translate(ideo.Named("IDEO"), this.pawn.Named("PAWN"), (from b in list
					select b.LabelCap).ToLineList(null, false).Named("BONDS")), LetterDefOf.NeutralEvent, new LookTargets(list.Concat(new Pawn[]
					{
						this.pawn
					})), null, null, null, null, 0, true);
				}
			}
			this.joinTick = Find.TickManager.TicksGame;
			this.RecacheIdeoComponents();
			Pawn_AgeTracker ageTracker = this.pawn.ageTracker;
			if (ageTracker == null)
			{
				return;
			}
			ageTracker.Notify_IdeoChanged();
		}

		// Token: 0x0600CCD3 RID: 52435 RVA: 0x003C11BC File Offset: 0x003BF3BC
		private void RecacheIdeoComponents()
		{
			Pawn_NeedsTracker needs = this.pawn.needs;
			if (needs != null)
			{
				Need_Mood mood = needs.mood;
				if (mood != null)
				{
					mood.thoughts.situational.Notify_SituationalThoughtsDirty();
				}
			}
			Pawn_NeedsTracker needs2 = this.pawn.needs;
			if (needs2 != null)
			{
				needs2.AddOrRemoveNeedsAsAppropriate();
			}
			Pawn_ApparelTracker apparel = this.pawn.apparel;
			if (apparel != null)
			{
				apparel.Notify_IdeoChanged();
			}
			Pawn_AbilityTracker abilities = this.pawn.abilities;
			if (abilities == null)
			{
				return;
			}
			abilities.Notify_TemporaryAbilitiesChanged();
		}

		// Token: 0x0600CCD4 RID: 52436 RVA: 0x003C1238 File Offset: 0x003BF438
		public bool IdeoConversionAttempt(float certaintyReduction, Ideo initiatorIdeo, bool applyCertaintyFactor = true)
		{
			if (!ModLister.CheckIdeology("Ideoligion conversion") || this.pawn.DevelopmentalStage.Baby())
			{
				return false;
			}
			if (Find.IdeoManager.classicMode)
			{
				return false;
			}
			float num = Mathf.Clamp01(this.Certainty + (applyCertaintyFactor ? this.ApplyCertaintyChangeFactor(-certaintyReduction) : (-certaintyReduction)));
			if (this.pawn.Spawned)
			{
				string text = "Certainty".Translate() + "\n" + this.Certainty.ToStringPercent() + " -> " + num.ToStringPercent();
				MoteMaker.ThrowText(this.pawn.DrawPos, this.pawn.Map, text, 8f);
			}
			this.Certainty = num;
			if (this.Certainty <= 0f)
			{
				bool flag = this.PreviousIdeos.Contains(initiatorIdeo);
				this.SetIdeo(initiatorIdeo);
				this.Certainty = 0.5f;
				initiatorIdeo.Notify_MemberGainedByConversion();
				if (!flag)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.ConvertedNewMember, this.pawn.Named(HistoryEventArgsNames.Doer), initiatorIdeo.Named(HistoryEventArgsNames.Ideo)), true);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600CCD5 RID: 52437 RVA: 0x003C1369 File Offset: 0x003BF569
		public void IncreaseIdeoExposureIfBabyTick(Ideo ideo, int ticks = 1)
		{
			this.IncreaseIdeoExposureIfBaby(ideo, (float)ticks * 0.0004f);
		}

		// Token: 0x0600CCD6 RID: 52438 RVA: 0x003C137C File Offset: 0x003BF57C
		public void IncreaseIdeoExposureIfBaby(Ideo ideo, float exposure)
		{
			if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive || ideo == null || Find.IdeoManager.classicMode || !this.pawn.DevelopmentalStage.Baby())
			{
				return;
			}
			if (ModsConfig.AnomalyActive && ideo == Find.IdeoManager.Horaxian)
			{
				return;
			}
			if (this.babyIdeoExposure == null)
			{
				this.babyIdeoExposure = new List<Pawn_IdeoTracker.IdeoExposureWeight>();
			}
			this.isIdeoExposureSortDirty = true;
			foreach (Pawn_IdeoTracker.IdeoExposureWeight ideoExposureWeight in this.babyIdeoExposure)
			{
				if (ideoExposureWeight.ideo == ideo)
				{
					ideoExposureWeight.exposure += exposure;
					return;
				}
			}
			this.babyIdeoExposure.Add(new Pawn_IdeoTracker.IdeoExposureWeight(ideo, exposure));
		}

		// Token: 0x0600CCD7 RID: 52439 RVA: 0x003C1454 File Offset: 0x003BF654
		private bool IsBabyIdeoExposureSorted()
		{
			if (this.babyIdeoExposure.NullOrEmpty<Pawn_IdeoTracker.IdeoExposureWeight>() || !this.isIdeoExposureSortDirty)
			{
				return true;
			}
			float exposure = this.babyIdeoExposure[0].exposure;
			foreach (Pawn_IdeoTracker.IdeoExposureWeight ideoExposureWeight in this.babyIdeoExposure)
			{
				if (exposure < ideoExposureWeight.exposure)
				{
					return false;
				}
				exposure = ideoExposureWeight.exposure;
			}
			this.isIdeoExposureSortDirty = false;
			return true;
		}

		// Token: 0x0600CCD8 RID: 52440 RVA: 0x003C14E8 File Offset: 0x003BF6E8
		public bool TryJoinIdeoFromExposures()
		{
			if (this.Ideo != null)
			{
				return false;
			}
			Ideo ideo;
			if (!this.babyIdeoExposure.NullOrEmpty<Pawn_IdeoTracker.IdeoExposureWeight>() && !Find.IdeoManager.classicMode)
			{
				Pawn_IdeoTracker.IdeoExposureWeight ideoExposureWeight;
				this.babyIdeoExposure.TryRandomElementByWeight((Pawn_IdeoTracker.IdeoExposureWeight i) => i.exposure, out ideoExposureWeight);
				ideo = ideoExposureWeight.ideo;
			}
			else
			{
				ideo = this.FallbackIdeo(null);
			}
			this.SetIdeo(ideo);
			if (this.Ideo != null)
			{
				this.babyIdeoExposure = null;
				return true;
			}
			return false;
		}

		// Token: 0x0600CCD9 RID: 52441 RVA: 0x003C156F File Offset: 0x003BF76F
		public void Reassure(float certaintyGain)
		{
			this.OffsetCertainty(certaintyGain);
		}

		// Token: 0x0600CCDA RID: 52442 RVA: 0x003C1578 File Offset: 0x003BF778
		public void OffsetCertainty(float offset)
		{
			if (!ModLister.CheckIdeology("Ideoligion certainty"))
			{
				return;
			}
			if (Find.IdeoManager.classicMode)
			{
				return;
			}
			float num = Mathf.Clamp01(this.Certainty + this.ApplyCertaintyChangeFactor(offset));
			if (this.pawn.Spawned)
			{
				string text = "Certainty".Translate() + "\n" + this.Certainty.ToStringPercent() + " -> " + num.ToStringPercent();
				MoteMaker.ThrowText(this.pawn.DrawPos, this.pawn.Map, text, 8f);
			}
			this.Certainty = num;
		}

		// Token: 0x0600CCDB RID: 52443 RVA: 0x003C1627 File Offset: 0x003BF827
		public void Notify_IdeoRemoved(Ideo removedIdeo)
		{
			if (this.Ideo == removedIdeo)
			{
				this.SetIdeo(this.FallbackIdeo(removedIdeo));
			}
			if (this.previousIdeos.Contains(removedIdeo))
			{
				this.previousIdeos.Remove(removedIdeo);
			}
		}

		// Token: 0x0600CCDC RID: 52444 RVA: 0x003C165C File Offset: 0x003BF85C
		private Ideo FallbackIdeo(Ideo excludeIdeo = null)
		{
			Faction faction = this.pawn.Faction;
			if (((faction != null) ? faction.ideos : null) != null && this.pawn.Faction.ideos.PrimaryIdeo != excludeIdeo && this.pawn.Faction.ideos.PrimaryIdeo != null)
			{
				return this.pawn.Faction.ideos.PrimaryIdeo;
			}
			return Find.IdeoManager.IdeosListForReading.RandomElement<Ideo>();
		}

		// Token: 0x0600CCDD RID: 52445 RVA: 0x003C16D6 File Offset: 0x003BF8D6
		public void Debug_ReduceCertainty(float amt)
		{
			this.Certainty -= amt;
		}

		// Token: 0x0600CCDE RID: 52446 RVA: 0x003C16E8 File Offset: 0x003BF8E8
		public void ExposeData()
		{
			Scribe_References.Look<Ideo>(ref this.ideo, "ideo", false);
			Scribe_Collections.Look<Ideo>(ref this.previousIdeos, "previousIdeos", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<float>(ref this.certaintyInt, "certainty", 0f, false);
			Scribe_Values.Look<int>(ref this.joinTick, "joinTick", 0, false);
			Scribe_Collections.Look<Pawn_IdeoTracker.IdeoExposureWeight>(ref this.babyIdeoExposure, "babyIdeoExposure", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.previousIdeos.RemoveAll((Ideo x) => x == null) != 0)
				{
					Log.Error("Removed null ideos");
				}
				if (this.Certainty <= 0f)
				{
					this.Certainty = Mathf.Clamp01(ConversionTuning.InitialCertaintyRange.RandomInRange);
				}
				if (this.Ideo == null && this.pawn.ShouldHaveIdeo)
				{
					Ideo arg = this.FallbackIdeo(null);
					Log.Warning(string.Format("{0} did not have an ideo set; assigning fallback ideo {1}.", this.pawn.ToStringSafe<Pawn>(), arg));
					this.SetIdeo(arg);
				}
			}
		}

		// Token: 0x04008B7E RID: 35710
		private Pawn pawn;

		// Token: 0x04008B7F RID: 35711
		public int joinTick;

		// Token: 0x04008B80 RID: 35712
		private Ideo ideo;

		// Token: 0x04008B81 RID: 35713
		private List<Ideo> previousIdeos;

		// Token: 0x04008B82 RID: 35714
		private float certaintyInt;

		// Token: 0x04008B83 RID: 35715
		private List<Pawn_IdeoTracker.IdeoExposureWeight> babyIdeoExposure;

		// Token: 0x04008B84 RID: 35716
		private bool isIdeoExposureSortDirty = true;

		// Token: 0x04008B85 RID: 35717
		private int lastCacheId;

		// Token: 0x04008B86 RID: 35718
		private const float ExposurePointsPerHour = 1f;

		// Token: 0x04008B87 RID: 35719
		public const float ExposurePointsPerTick = 0.0004f;

		// Token: 0x04008B88 RID: 35720
		private static SimpleCurve pawnAgeCertaintyCurve;

		// Token: 0x020024C2 RID: 9410
		public class IdeoExposureWeight : IExposable, IComparable<Pawn_IdeoTracker.IdeoExposureWeight>
		{
			// Token: 0x0600CCDF RID: 52447 RVA: 0x000024DF File Offset: 0x000006DF
			public IdeoExposureWeight()
			{
			}

			// Token: 0x0600CCE0 RID: 52448 RVA: 0x003C17FF File Offset: 0x003BF9FF
			public IdeoExposureWeight(Ideo ideo, float exposure)
			{
				this.ideo = ideo;
				this.exposure = exposure;
			}

			// Token: 0x0600CCE1 RID: 52449 RVA: 0x003C1815 File Offset: 0x003BFA15
			public int CompareTo(Pawn_IdeoTracker.IdeoExposureWeight other)
			{
				return other.exposure.CompareTo(this.exposure);
			}

			// Token: 0x0600CCE2 RID: 52450 RVA: 0x003C1828 File Offset: 0x003BFA28
			public void ExposeData()
			{
				Scribe_References.Look<Ideo>(ref this.ideo, "ideo", false);
				Scribe_Values.Look<float>(ref this.exposure, "exposure", 0f, false);
			}

			// Token: 0x04008B89 RID: 35721
			public Ideo ideo;

			// Token: 0x04008B8A RID: 35722
			public float exposure;
		}
	}
}
