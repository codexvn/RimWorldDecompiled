using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012B4 RID: 4788
	public class JobDriver_Lovin : JobDriver
	{
		// Token: 0x17001266 RID: 4710
		// (get) Token: 0x06007551 RID: 30033 RVA: 0x0023D2E3 File Offset: 0x0023B4E3
		private Pawn Partner
		{
			get
			{
				return (Pawn)((Thing)this.job.GetTarget(this.PartnerInd));
			}
		}

		// Token: 0x17001267 RID: 4711
		// (get) Token: 0x06007552 RID: 30034 RVA: 0x0023D300 File Offset: 0x0023B500
		private Building_Bed Bed
		{
			get
			{
				return (Building_Bed)((Thing)this.job.GetTarget(this.BedInd));
			}
		}

		// Token: 0x06007553 RID: 30035 RVA: 0x0023D31D File Offset: 0x0023B51D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		// Token: 0x06007554 RID: 30036 RVA: 0x0023D338 File Offset: 0x0023B538
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Partner, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.Bed, this.job, this.Bed.SleepingSlotsCount, 0, null, errorOnFailed, false);
		}

		// Token: 0x06007555 RID: 30037 RVA: 0x0023D395 File Offset: 0x0023B595
		public override bool CanBeginNowWhileLyingDown()
		{
			return JobInBedUtility.InBedOrRestSpotNow(this.pawn, this.job.GetTarget(this.BedInd));
		}

		// Token: 0x06007556 RID: 30038 RVA: 0x0023D3B3 File Offset: 0x0023B5B3
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(this.BedInd);
			this.FailOnDespawnedOrNull(this.PartnerInd);
			this.FailOn(() => !this.Partner.health.capacities.CanBeAwake);
			this.KeepLyingDown(this.BedInd);
			yield return Toils_Bed.ClaimBedIfNonMedical(this.BedInd, TargetIndex.None);
			yield return Toils_Bed.GotoBed(this.BedInd);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				if (this.Partner.CurJob == null || this.Partner.CurJob.def != JobDefOf.Lovin)
				{
					Job newJob = JobMaker.MakeJob(JobDefOf.Lovin, this.pawn, this.Bed);
					this.Partner.jobs.StartJob(newJob, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
					this.ticksLeft = (int)(2500f * Mathf.Clamp(Rand.Range(0.1f, 1.1f), 0.1f, 2f));
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.InitiatedLovin, this.pawn.Named(HistoryEventArgsNames.Doer)), true);
					if (InteractionWorker_RomanceAttempt.CanCreatePsychicBondBetween(this.pawn, this.Partner) && InteractionWorker_RomanceAttempt.TryCreatePsychicBondBetween(this.pawn, this.Partner) && (PawnUtility.ShouldSendNotificationAbout(this.pawn) || PawnUtility.ShouldSendNotificationAbout(this.Partner)))
					{
						Find.LetterStack.ReceiveLetter("LetterPsychicBondCreatedLovinLabel".Translate(), "LetterPsychicBondCreatedLovinText".Translate(this.pawn.Named("BONDPAWN"), this.Partner.Named("OTHERPAWN")), LetterDefOf.PositiveEvent, new LookTargets(new TargetInfo[]
						{
							this.pawn,
							this.Partner
						}), null, null, null, null, 0, true);
						return;
					}
				}
				else
				{
					this.ticksLeft = 9999999;
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil;
			Toil toil2 = Toils_LayDown.LayDown(this.BedInd, true, false, false, false, PawnPosture.LayingOnGroundNormal, false);
			toil2.FailOn(() => this.Partner.CurJob == null || this.Partner.CurJob.def != JobDefOf.Lovin);
			toil2.AddPreTickIntervalAction(delegate(int delta)
			{
				this.ticksLeft -= delta;
				if (this.ticksLeft <= 0)
				{
					base.ReadyForNextToil();
					return;
				}
				if (this.pawn.IsHashIntervalTick(100, delta))
				{
					FleckMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, FleckDefOf.Heart, 0.42f);
				}
			});
			toil2.AddFinishAction(delegate
			{
				Thought_Memory thought_Memory = (Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.GotSomeLovin);
				if (this.pawn.health != null && this.pawn.health.hediffSet != null)
				{
					if (this.pawn.health.hediffSet.hediffs.Any((Hediff h) => h.def == HediffDefOf.LoveEnhancer))
					{
						goto IL_C4;
					}
				}
				if (this.Partner.health == null || this.Partner.health.hediffSet == null)
				{
					goto IL_CF;
				}
				if (!this.Partner.health.hediffSet.hediffs.Any((Hediff h) => h.def == HediffDefOf.LoveEnhancer))
				{
					goto IL_CF;
				}
				IL_C4:
				thought_Memory.moodPowerFactor = 1.5f;
				IL_CF:
				if (this.pawn.needs.mood != null)
				{
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, this.Partner);
				}
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.GotLovin, this.pawn.Named(HistoryEventArgsNames.Doer)), true);
				HistoryEventDef def = this.pawn.relations.DirectRelationExists(PawnRelationDefOf.Spouse, this.Partner) ? HistoryEventDefOf.GotLovin_Spouse : HistoryEventDefOf.GotLovin_NonSpouse;
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(def, this.pawn.Named(HistoryEventArgsNames.Doer)), true);
				this.pawn.mindState.canLovinTick = Find.TickManager.TicksGame + this.GenerateRandomMinTicksToNextLovin(this.pawn);
				if (ModsConfig.BiotechActive)
				{
					Pawn pawn = (this.pawn.gender == Gender.Male) ? this.pawn : ((this.Partner.gender == Gender.Male) ? this.Partner : null);
					Pawn pawn2 = (this.pawn.gender == Gender.Female) ? this.pawn : ((this.Partner.gender == Gender.Female) ? this.Partner : null);
					if (pawn != null && pawn2 != null && Rand.Chance(JobDriver_Lovin.PregnancyChance * PregnancyUtility.PregnancyChanceForPartners(pawn2, pawn)))
					{
						bool flag;
						GeneSet inheritedGeneSet = PregnancyUtility.GetInheritedGeneSet(pawn, pawn2, out flag);
						if (flag)
						{
							Hediff_Pregnant hediff_Pregnant = (Hediff_Pregnant)HediffMaker.MakeHediff(HediffDefOf.PregnantHuman, pawn2, null);
							hediff_Pregnant.SetParents(null, pawn, inheritedGeneSet);
							pawn2.health.AddHediff(hediff_Pregnant, null, null, null);
							return;
						}
						if (PawnUtility.ShouldSendNotificationAbout(pawn) || PawnUtility.ShouldSendNotificationAbout(pawn2))
						{
							Messages.Message("MessagePregnancyFailed".Translate(pawn.Named("FATHER"), pawn2.Named("MOTHER")) + ": " + "CombinedGenesExceedMetabolismLimits".Translate(), new LookTargets(new TargetInfo[]
							{
								pawn,
								pawn2
							}), MessageTypeDefOf.NegativeEvent, true);
						}
					}
				}
			});
			toil2.socialMode = RandomSocialMode.Off;
			yield return toil2;
			yield break;
		}

		// Token: 0x06007557 RID: 30039 RVA: 0x0023D3C4 File Offset: 0x0023B5C4
		private int GenerateRandomMinTicksToNextLovin(Pawn pawn)
		{
			if (DebugSettings.alwaysDoLovin)
			{
				return 100;
			}
			float num = JobDriver_Lovin.LovinIntervalHoursFromAgeCurve.Evaluate(pawn.ageTracker.AgeBiologicalYearsFloat);
			if (ModsConfig.BiotechActive && pawn.genes != null)
			{
				foreach (Gene gene in pawn.genes.GenesListForReading)
				{
					num *= gene.def.lovinMTBFactor;
				}
			}
			foreach (Hediff hd in pawn.health.hediffSet.hediffs)
			{
				HediffComp_GiveLovinMTBFactor hediffComp_GiveLovinMTBFactor = hd.TryGetComp<HediffComp_GiveLovinMTBFactor>();
				if (hediffComp_GiveLovinMTBFactor != null)
				{
					num *= hediffComp_GiveLovinMTBFactor.Props.lovinMTBFactor;
				}
			}
			num = Rand.Gaussian(num, 0.3f);
			if (num < 0.5f)
			{
				num = 0.5f;
			}
			return (int)(num * 2500f);
		}

		// Token: 0x04005058 RID: 20568
		private int ticksLeft;

		// Token: 0x04005059 RID: 20569
		private TargetIndex PartnerInd = TargetIndex.A;

		// Token: 0x0400505A RID: 20570
		private TargetIndex BedInd = TargetIndex.B;

		// Token: 0x0400505B RID: 20571
		private const int TicksBetweenHeartMotes = 100;

		// Token: 0x0400505C RID: 20572
		private static float PregnancyChance = 0.05f;

		// Token: 0x0400505D RID: 20573
		private static readonly SimpleCurve LovinIntervalHoursFromAgeCurve = new SimpleCurve
		{
			{
				new CurvePoint(16f, 1.5f),
				true
			},
			{
				new CurvePoint(22f, 1.5f),
				true
			},
			{
				new CurvePoint(30f, 4f),
				true
			},
			{
				new CurvePoint(50f, 12f),
				true
			},
			{
				new CurvePoint(75f, 36f),
				true
			}
		};
	}
}
