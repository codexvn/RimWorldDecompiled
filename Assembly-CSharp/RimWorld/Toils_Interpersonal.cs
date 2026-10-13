using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001377 RID: 4983
	public static class Toils_Interpersonal
	{
		// Token: 0x06007A3C RID: 31292 RVA: 0x0024D874 File Offset: 0x0024BA74
		public static Toil GotoInteractablePosition(TargetIndex target)
		{
			Toil toil = ToilMaker.MakeToil("GotoInteractablePosition");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)((Thing)actor.CurJob.GetTarget(target));
				if (SocialInteractionUtility.IsGoodPositionForInteraction(actor, pawn))
				{
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				actor.pather.StartPath(pawn, PathEndMode.Touch);
			};
			toil.tickIntervalAction = delegate(int delta)
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)((Thing)actor.CurJob.GetTarget(target));
				Map map = actor.Map;
				if (SocialInteractionUtility.IsGoodPositionForInteraction(actor, pawn) && actor.Position.InHorDistOf(pawn.Position, (float)Mathf.CeilToInt(3f)) && (!actor.pather.Moving || actor.pather.nextCell.GetDoor(map) == null))
				{
					actor.pather.StopDead();
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				if (!actor.pather.Moving)
				{
					IntVec3 c = SocialInteractionUtility.BestInteractableCell(actor, pawn);
					if (c.IsValid)
					{
						actor.pather.StartPath(c, PathEndMode.OnCell);
						return;
					}
					actor.jobs.curDriver.EndJobWith(JobCondition.Incompletable);
				}
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			return toil;
		}

		// Token: 0x06007A3D RID: 31293 RVA: 0x0024D8EC File Offset: 0x0024BAEC
		public static Toil GotoPrisoner(Pawn pawn, Pawn talkee, PrisonerInteractionModeDef mode)
		{
			Toil toil = ToilMaker.MakeToil("GotoPrisoner");
			toil.initAction = delegate()
			{
				pawn.pather.StartPath(talkee, PathEndMode.Touch);
			};
			toil.AddFailCondition(() => talkee.DestroyedOrNull() || (mode.mustBeAwake && !talkee.Awake()) || !talkee.IsPrisonerOfColony || (talkee.guest == null || talkee.guest.IsInteractionDisabled(mode)));
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x06007A3E RID: 31294 RVA: 0x0024D950 File Offset: 0x0024BB50
		public static Toil GotoGuiltyColonist(Pawn pawn, Pawn talkee)
		{
			Toil toil = ToilMaker.MakeToil("GotoGuiltyColonist");
			toil.initAction = delegate()
			{
				pawn.pather.StartPath(talkee, PathEndMode.Touch);
			};
			toil.AddFailCondition(() => talkee.DestroyedOrNull() || !talkee.guilt.IsGuilty);
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x06007A3F RID: 31295 RVA: 0x0024D9B0 File Offset: 0x0024BBB0
		public static Toil GotoSlave(Pawn pawn, Pawn talkee)
		{
			Toil toil = ToilMaker.MakeToil("GotoSlave");
			toil.initAction = delegate()
			{
				pawn.pather.StartPath(talkee, PathEndMode.Touch);
			};
			toil.AddFailCondition(() => talkee.DestroyedOrNull() || !talkee.IsSlaveOfColony);
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x06007A40 RID: 31296 RVA: 0x0024DA10 File Offset: 0x0024BC10
		public static Toil WaitToBeAbleToInteract(Pawn pawn)
		{
			Toil toil = ToilMaker.MakeToil("WaitToBeAbleToInteract");
			toil.initAction = delegate()
			{
				if (!pawn.interactions.InteractedTooRecentlyToInteract())
				{
					pawn.jobs.curDriver.ReadyForNextToil();
				}
			};
			toil.tickIntervalAction = delegate(int delta)
			{
				if (!pawn.interactions.InteractedTooRecentlyToInteract())
				{
					pawn.jobs.curDriver.ReadyForNextToil();
				}
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			return toil;
		}

		// Token: 0x06007A41 RID: 31297 RVA: 0x0024DA68 File Offset: 0x0024BC68
		public static Toil ConvinceRecruitee(Pawn pawn, Pawn talkee, InteractionDef interactionDef = null)
		{
			Toil toil = ToilMaker.MakeToil("ConvinceRecruitee");
			toil.initAction = delegate()
			{
				if (!pawn.interactions.TryInteractWith(talkee, interactionDef ?? InteractionDefOf.BuildRapport))
				{
					pawn.jobs.curDriver.ReadyForNextToil();
					return;
				}
				pawn.records.Increment(RecordDefOf.PrisonersChatted);
			};
			toil.FailOn(() => !talkee.guest.ScheduledForInteraction);
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 350;
			toil.activeSkill = (() => SkillDefOf.Social);
			return toil;
		}

		// Token: 0x06007A42 RID: 31298 RVA: 0x0024DB00 File Offset: 0x0024BD00
		public static Toil Interrogate(Pawn pawn, Pawn talkee)
		{
			Toil toil = ToilMaker.MakeToil("Interrogate");
			toil.initAction = delegate()
			{
				if (!pawn.interactions.TryInteractWith(talkee, InteractionDefOf.InterrogateIdentity))
				{
					pawn.jobs.curDriver.ReadyForNextToil();
					return;
				}
				pawn.records.Increment(RecordDefOf.PrisonersChatted);
			};
			toil.FailOn(() => !talkee.guest.ScheduledForInteraction);
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 350;
			toil.activeSkill = (() => SkillDefOf.Social);
			return toil;
		}

		// Token: 0x06007A43 RID: 31299 RVA: 0x0024DB90 File Offset: 0x0024BD90
		public static Toil ReduceWill(Pawn pawn, Pawn talkee)
		{
			Toil toil = ToilMaker.MakeToil("ReduceWill");
			toil.initAction = delegate()
			{
				if (!pawn.interactions.TryInteractWith(talkee, InteractionDefOf.ReduceWill))
				{
					pawn.jobs.curDriver.ReadyForNextToil();
					return;
				}
				pawn.records.Increment(RecordDefOf.PrisonersChatted);
			};
			toil.FailOn(() => !talkee.guest.ScheduledForInteraction);
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 350;
			toil.activeSkill = (() => SkillDefOf.Social);
			return toil;
		}

		// Token: 0x06007A44 RID: 31300 RVA: 0x0024DC20 File Offset: 0x0024BE20
		public static Toil SetLastInteractTime(TargetIndex targetInd)
		{
			Toil toil = ToilMaker.MakeToil("SetLastInteractTime");
			toil.initAction = delegate()
			{
				Pawn pawn = (Pawn)toil.actor.jobs.curJob.GetTarget(targetInd).Thing;
				pawn.mindState.lastAssignedInteractTime = Find.TickManager.TicksGame;
				pawn.mindState.interactionsToday++;
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x06007A45 RID: 31301 RVA: 0x0024DC74 File Offset: 0x0024BE74
		public static Toil TryRecruit(TargetIndex recruiteeInd)
		{
			Toil toil = ToilMaker.MakeToil("TryRecruit");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)actor.jobs.curJob.GetTarget(recruiteeInd).Thing;
				if (!pawn.Spawned || !pawn.Awake())
				{
					return;
				}
				InteractionDef intDef = pawn.AnimalOrWildMan() ? InteractionDefOf.TameAttempt : InteractionDefOf.RecruitAttempt;
				actor.interactions.TryInteractWith(pawn, intDef);
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 350;
			toil.activeSkill = delegate()
			{
				if (!((Pawn)toil.actor.jobs.curJob.GetTarget(recruiteeInd).Thing).RaceProps.Animal)
				{
					return SkillDefOf.Social;
				}
				return SkillDefOf.Animals;
			};
			return toil;
		}

		// Token: 0x06007A46 RID: 31302 RVA: 0x0024DCFC File Offset: 0x0024BEFC
		public static Toil TryTrain(TargetIndex traineeInd)
		{
			Toil toil = ToilMaker.MakeToil("TryTrain");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)actor.jobs.curJob.GetTarget(traineeInd).Thing;
				if (pawn.Spawned && pawn.Awake() && actor.interactions.TryInteractWith(pawn, InteractionDefOf.TrainAttempt))
				{
					float num = actor.GetStatValue(StatDefOf.TrainAnimalChance, true, -1);
					num *= GenMath.LerpDouble(0f, 1f, 1.5f, 0.5f, pawn.GetStatValue(StatDefOf.Wildness, true, -1));
					if (actor.relations.DirectRelationExists(PawnRelationDefOf.Bond, pawn))
					{
						num *= 5f;
					}
					num = Mathf.Clamp01(num);
					TrainableDef trainableDef = pawn.training.NextTrainableToTrain();
					if (trainableDef == null)
					{
						Log.ErrorOnce("Attempted to train untrainable animal", 7842936);
						return;
					}
					string text;
					if (Rand.Value < num)
					{
						pawn.training.Train(trainableDef, actor, false);
						if (pawn.caller != null)
						{
							pawn.caller.DoCall(false);
						}
						text = "TextMote_TrainSuccess".Translate(trainableDef.LabelCap, num.ToStringPercent());
						RelationsUtility.TryDevelopBondRelation(actor, pawn, 0.007f);
						TaleRecorder.RecordTale(TaleDefOf.TrainedAnimal, new object[]
						{
							actor,
							pawn,
							trainableDef
						});
					}
					else
					{
						text = "TextMote_TrainFail".Translate(trainableDef.LabelCap, num.ToStringPercent());
					}
					text = string.Concat(new string[]
					{
						text,
						"\n",
						pawn.training.GetSteps(trainableDef).ToString(),
						" / ",
						trainableDef.steps.ToString()
					});
					MoteMaker.ThrowText((actor.DrawPos + pawn.DrawPos) / 2f, actor.Map, text, 5f);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 100;
			toil.activeSkill = (() => SkillDefOf.Animals);
			return toil;
		}

		// Token: 0x06007A47 RID: 31303 RVA: 0x0024DD88 File Offset: 0x0024BF88
		public static Toil Interact(TargetIndex otherPawnInd, InteractionDef interaction)
		{
			Toil toil = ToilMaker.MakeToil("Interact");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)actor.jobs.curJob.GetTarget(otherPawnInd).Thing;
				if (!pawn.Spawned)
				{
					return;
				}
				actor.interactions.TryInteractWith(pawn, interaction);
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 60;
			return toil;
		}

		// Token: 0x06007A48 RID: 31304 RVA: 0x0024DDFC File Offset: 0x0024BFFC
		public static Toil TryEnslave(TargetIndex prisonerInd)
		{
			Toil toil = ToilMaker.MakeToil("TryEnslave");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)actor.jobs.curJob.GetTarget(prisonerInd).Thing;
				if (!pawn.Spawned || !pawn.Awake())
				{
					return;
				}
				actor.interactions.TryInteractWith(pawn, InteractionDefOf.EnslaveAttempt);
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 350;
			toil.activeSkill = (() => SkillDefOf.Social);
			return toil;
		}

		// Token: 0x06007A49 RID: 31305 RVA: 0x0024DE98 File Offset: 0x0024C098
		public static Toil TryConvert(TargetIndex prisonerInd)
		{
			Toil toil = ToilMaker.MakeToil("TryConvert");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn recipient = (Pawn)actor.jobs.curJob.GetTarget(prisonerInd).Thing;
				actor.interactions.TryInteractWith(recipient, InteractionDefOf.ConvertIdeoAttempt);
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 350;
			toil.activeSkill = (() => SkillDefOf.Social);
			return toil;
		}

		// Token: 0x06007A4A RID: 31306 RVA: 0x0024DF34 File Offset: 0x0024C134
		public static Toil ExtractHemogen(TargetIndex prisonerInd, float bloodLoss)
		{
			Toil toil = ToilMaker.MakeToil("ExtractHemogen");
			toil.initAction = delegate()
			{
				GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.HemogenPack, null), toil.actor.Position, toil.actor.Map, ThingPlaceMode.Near, null, null, null, 1);
				Pawn pawn = (Pawn)toil.actor.jobs.curJob.GetTarget(prisonerInd).Thing;
				Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.BloodLoss, pawn, null);
				hediff.Severity = bloodLoss;
				pawn.health.AddHediff(hediff, null, null, null);
			};
			return toil;
		}
	}
}
