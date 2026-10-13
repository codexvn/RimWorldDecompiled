using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001277 RID: 4727
	public class JobDriver_Fish : JobDriver
	{
		// Token: 0x060073A4 RID: 29604 RVA: 0x0023825C File Offset: 0x0023645C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, ReservationLayerDefOf.Floor, errorOnFailed, false) && this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, -1, ReservationLayerDefOf.Floor, errorOnFailed, false);
		}

		// Token: 0x060073A5 RID: 29605 RVA: 0x002382B9 File Offset: 0x002364B9
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModsConfig.OdysseyActive)
			{
				yield break;
			}
			this.FailOn(delegate()
			{
				Zone_Fishing zone_Fishing = this.job.GetTarget(TargetIndex.A).Cell.GetZone(this.pawn.Map) as Zone_Fishing;
				return zone_Fishing == null || !zone_Fishing.Allowed;
			});
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			int ticks = Mathf.RoundToInt(7500f / this.pawn.GetStatValue(StatDefOf.FishingSpeed, true, -1));
			Toil toil = Toils_General.WaitWith(TargetIndex.A, ticks, false, true, false, TargetIndex.A, PathEndMode.Touch);
			Toil toil2 = toil;
			toil2.tickAction = (Action)Delegate.Combine(toil2.tickAction, new Action(delegate()
			{
				Pawn_SkillTracker skills = this.pawn.skills;
				if (skills == null)
				{
					return;
				}
				skills.Learn(SkillDefOf.Animals, 0.025f, false, false);
			}));
			toil.WithEffect(EffecterDefOf.Fishing, () => this.job.GetTarget(TargetIndex.A), null);
			toil.WithProgressBarToilDelay(TargetIndex.B, false, -0.5f);
			yield return toil;
			yield return this.CompleteFishingToil();
			yield break;
		}

		// Token: 0x060073A6 RID: 29606 RVA: 0x002382C9 File Offset: 0x002364C9
		private Toil CompleteFishingToil()
		{
			Toil toil = ToilMaker.MakeToil("CompleteFishingToil");
			toil.initAction = delegate()
			{
				IntVec3 cell = this.job.GetTarget(TargetIndex.A).Cell;
				List<NegativeFishingOutcomeDef> negativeFishingOutcomes = FishingUtility.GetNegativeFishingOutcomes(this.pawn, cell);
				bool flag;
				List<Thing> catchesFor = FishingUtility.GetCatchesFor(this.pawn, cell, false, out flag);
				if (negativeFishingOutcomes.Any<NegativeFishingOutcomeDef>())
				{
					NegativeFishingOutcomeDef negativeFishingOutcomeDef = negativeFishingOutcomes.RandomElement<NegativeFishingOutcomeDef>();
					this.pawn.Map.waterBodyTracker.lastNegativeCatchTick = Find.TickManager.TicksGame;
					if (negativeFishingOutcomeDef.damageDef != null)
					{
						DamageInfo dinfo = new DamageInfo(negativeFishingOutcomeDef.damageDef, (float)negativeFishingOutcomeDef.damageAmountRange.RandomInRange, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
						dinfo.SetBodyRegion(BodyPartHeight.Undefined, BodyPartDepth.Outside);
						this.pawn.TakeDamage(dinfo);
					}
					if (negativeFishingOutcomeDef.addsHediff != null)
					{
						Hediff hediff = this.pawn.health.AddHediff(negativeFishingOutcomeDef.addsHediff, null, null, null);
						if (negativeFishingOutcomeDef.hediffSeverity > 0f)
						{
							hediff.Severity = negativeFishingOutcomeDef.hediffSeverity;
						}
					}
					Find.LetterStack.ReceiveLetter(negativeFishingOutcomeDef.letterLabel, negativeFishingOutcomeDef.letterText.Formatted(this.pawn.Named("PAWN")), negativeFishingOutcomeDef.letterDef, this.pawn, null, null, null, null, 0, true);
				}
				else if (catchesFor.Any<Thing>())
				{
					bool flag2 = false;
					int num = catchesFor.Sum((Thing x) => x.stackCount);
					foreach (Thing thing in catchesFor)
					{
						flag2 |= GenPlace.TryPlaceThing(thing, this.pawn.Position, this.pawn.Map, ThingPlaceMode.Near, null, null, null, 1);
					}
					if (flag2)
					{
						if (flag)
						{
							this.pawn.Map.waterBodyTracker.lastRareCatchTick = Find.TickManager.TicksGame;
							Find.LetterStack.ReceiveLetter("LetterLabelRareCatch".Translate(), "LetterTextRareCatch".Translate(this.pawn.Named("PAWN")) + ":\n" + (from x in catchesFor
							select x.LabelCap).ToLineList("  - ", false), LetterDefOf.PositiveEvent, catchesFor, null, null, null, null, 0, true);
						}
						else
						{
							this.pawn.Map.waterBodyTracker.Notify_Fished(cell, (float)num);
							Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.SlaughteredFish, this.pawn.Named(HistoryEventArgsNames.Doer)), true);
						}
					}
				}
				Zone_Fishing zone_Fishing = cell.GetZone(this.pawn.Map) as Zone_Fishing;
				if (zone_Fishing == null)
				{
					return;
				}
				zone_Fishing.Notify_Fished();
			};
			toil.PlaySoundAtStart(SoundDefOf.Interact_CatchFish);
			return toil;
		}

		// Token: 0x04004FAB RID: 20395
		private const TargetIndex SpotInd = TargetIndex.A;

		// Token: 0x04004FAC RID: 20396
		private const TargetIndex StandInd = TargetIndex.B;
	}
}
