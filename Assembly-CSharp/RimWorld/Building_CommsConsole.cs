using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020029E7 RID: 10727
	public class Building_CommsConsole : Building
	{
		// Token: 0x17002488 RID: 9352
		// (get) Token: 0x0600ECB9 RID: 60601 RVA: 0x0044B863 File Offset: 0x00449A63
		public bool CanUseCommsNow
		{
			get
			{
				return (!base.Spawned || !base.Map.gameConditionManager.ElectricityDisabled(base.Map)) && (this.powerComp == null || this.powerComp.PowerOn);
			}
		}

		// Token: 0x0600ECBA RID: 60602 RVA: 0x0044B89C File Offset: 0x00449A9C
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.powerComp = base.GetComp<CompPowerTrader>();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.BuildOrbitalTradeBeacon, OpportunityType.GoodToKnow);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.OpeningComms, OpportunityType.GoodToKnow);
			if (this.CanUseCommsNow)
			{
				LongEventHandler.ExecuteWhenFinished(new Action(this.AnnounceTradeShips));
			}
		}

		// Token: 0x0600ECBB RID: 60603 RVA: 0x0044B8EC File Offset: 0x00449AEC
		private void UseAct(Pawn myPawn, ICommunicable commTarget)
		{
			Job job = JobMaker.MakeJob(JobDefOf.UseCommsConsole, this);
			job.commTarget = commTarget;
			myPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.OpeningComms, KnowledgeAmount.Total);
		}

		// Token: 0x0600ECBC RID: 60604 RVA: 0x0044B930 File Offset: 0x00449B30
		private FloatMenuOption GetFailureReason(Pawn myPawn)
		{
			if (!myPawn.CanReach(this, PathEndMode.InteractionCell, Danger.Some, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotUseNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (base.Spawned && base.Map.gameConditionManager.ElectricityDisabled(base.Map))
			{
				return new FloatMenuOption("CannotUseSolarFlare".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (this.powerComp != null && !this.powerComp.PowerOn)
			{
				return new FloatMenuOption("CannotUseNoPower".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!myPawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking))
			{
				return new FloatMenuOption("CannotUseReason".Translate("IncapableOfCapacity".Translate(PawnCapacityDefOf.Talking.label, myPawn.Named("PAWN"))), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!this.GetCommTargets(myPawn).Any<ICommunicable>())
			{
				return new FloatMenuOption("CannotUseReason".Translate("NoCommsTarget".Translate()), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!this.CanUseCommsNow)
			{
				Log.Error(((myPawn != null) ? myPawn.ToString() : null) + " could not use comm console for unknown reason.");
				return new FloatMenuOption("Cannot use now", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return null;
		}

		// Token: 0x0600ECBD RID: 60605 RVA: 0x0044BACC File Offset: 0x00449CCC
		public IEnumerable<ICommunicable> GetCommTargets(Pawn myPawn)
		{
			return myPawn.Map.passingShipManager.passingShips.Cast<ICommunicable>().Concat((from f in Find.FactionManager.AllFactionsVisibleInViewOrder
			where !f.temporary && !f.IsPlayer
			select f).Cast<ICommunicable>());
		}

		// Token: 0x0600ECBE RID: 60606 RVA: 0x0044BB26 File Offset: 0x00449D26
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
		{
			FloatMenuOption failureReason = this.GetFailureReason(myPawn);
			if (failureReason != null)
			{
				yield return failureReason;
				yield break;
			}
			foreach (ICommunicable communicable in this.GetCommTargets(myPawn))
			{
				FloatMenuOption floatMenuOption = communicable.CommFloatMenuOption(this, myPawn);
				if (floatMenuOption != null)
				{
					yield return floatMenuOption;
				}
			}
			IEnumerator<ICommunicable> enumerator = null;
			foreach (FloatMenuOption floatMenuOption2 in base.GetFloatMenuOptions(myPawn))
			{
				yield return floatMenuOption2;
			}
			IEnumerator<FloatMenuOption> enumerator2 = null;
			yield break;
			yield break;
		}

		// Token: 0x0600ECBF RID: 60607 RVA: 0x0044BB40 File Offset: 0x00449D40
		public void GiveUseCommsJob(Pawn negotiator, ICommunicable target)
		{
			Job job = JobMaker.MakeJob(JobDefOf.UseCommsConsole, this);
			job.commTarget = target;
			negotiator.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.OpeningComms, KnowledgeAmount.Total);
		}

		// Token: 0x0600ECC0 RID: 60608 RVA: 0x0044BB84 File Offset: 0x00449D84
		private void AnnounceTradeShips()
		{
			foreach (TradeShip tradeShip in from s in base.Map.passingShipManager.passingShips.OfType<TradeShip>()
			where !s.WasAnnounced
			select s)
			{
				TaggedString baseLetterText = "TraderArrival".Translate(tradeShip.name, tradeShip.def.label, (tradeShip.Faction == null) ? "TraderArrivalNoFaction".Translate() : "TraderArrivalFromFaction".Translate(tradeShip.Faction.Named("FACTION")));
				IncidentParms incidentParms = new IncidentParms();
				incidentParms.target = base.Map;
				incidentParms.traderKind = tradeShip.TraderKind;
				IncidentWorker.SendIncidentLetter(tradeShip.def.LabelCap, baseLetterText, LetterDefOf.PositiveEvent, incidentParms, LookTargets.Invalid, null, Array.Empty<NamedArgument>());
				tradeShip.WasAnnounced = true;
			}
		}

		// Token: 0x04009CDC RID: 40156
		private CompPowerTrader powerComp;
	}
}
