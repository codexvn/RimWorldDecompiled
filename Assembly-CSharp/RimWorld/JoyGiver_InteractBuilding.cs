using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014D7 RID: 5335
	public abstract class JoyGiver_InteractBuilding : JoyGiver
	{
		// Token: 0x170013A8 RID: 5032
		// (get) Token: 0x06007F7E RID: 32638 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual bool CanDoDuringGathering
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06007F7F RID: 32639 RVA: 0x00265FC4 File Offset: 0x002641C4
		public override Job TryGiveJob(Pawn pawn)
		{
			Thing thing = this.FindBestGame(pawn, false, IntVec3.Invalid);
			if (thing != null)
			{
				return this.TryGivePlayJob(pawn, thing);
			}
			return null;
		}

		// Token: 0x06007F80 RID: 32640 RVA: 0x00265FEC File Offset: 0x002641EC
		public override Job TryGiveJobWhileInBed(Pawn pawn)
		{
			Thing thing = this.FindBestGame(pawn, true, IntVec3.Invalid);
			if (thing != null)
			{
				return this.TryGivePlayJobWhileInBed(pawn, thing);
			}
			return null;
		}

		// Token: 0x06007F81 RID: 32641 RVA: 0x00266014 File Offset: 0x00264214
		public override Job TryGiveJobInGatheringArea(Pawn pawn, IntVec3 gatheringSpot, float maxRadius = -1f)
		{
			if (!this.CanDoDuringGathering)
			{
				return null;
			}
			Thing thing = this.FindBestGame(pawn, false, gatheringSpot);
			if (thing != null)
			{
				return this.TryGivePlayJob(pawn, thing);
			}
			return null;
		}

		// Token: 0x06007F82 RID: 32642 RVA: 0x00266044 File Offset: 0x00264244
		private Thing FindBestGame(Pawn pawn, bool inBed, IntVec3 gatheringSpot)
		{
			JoyGiver_InteractBuilding.tmpCandidates.Clear();
			this.GetSearchSet(pawn, JoyGiver_InteractBuilding.tmpCandidates);
			if (JoyGiver_InteractBuilding.tmpCandidates.Count == 0)
			{
				return null;
			}
			Predicate<Thing> predicate = (Thing t) => this.CanInteractWith(pawn, t, inBed);
			if (gatheringSpot.IsValid)
			{
				Predicate<Thing> oldValidator = predicate;
				predicate = ((Thing x) => GatheringsUtility.InGatheringArea(x.Position, gatheringSpot, pawn.Map) && oldValidator(x));
			}
			Thing result = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, JoyGiver_InteractBuilding.tmpCandidates, PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, predicate, null, false);
			JoyGiver_InteractBuilding.tmpCandidates.Clear();
			return result;
		}

		// Token: 0x06007F83 RID: 32643 RVA: 0x0026611C File Offset: 0x0026431C
		protected virtual bool CanInteractWith(Pawn pawn, Thing t, bool inBed)
		{
			if (!pawn.CanReserve(t, this.def.jobDef.joyMaxParticipants, -1, null, false))
			{
				return false;
			}
			if (t.IsForbidden(pawn))
			{
				return false;
			}
			if (t.Fogged())
			{
				return false;
			}
			if (!t.IsSociallyProper(pawn))
			{
				return false;
			}
			if (!t.IsPoliticallyProper(pawn))
			{
				return false;
			}
			if (t.VacuumConcernTo(pawn))
			{
				return false;
			}
			CompPowerTrader compPowerTrader = t.TryGetComp<CompPowerTrader>();
			return (compPowerTrader == null || compPowerTrader.PowerOn) && (!this.def.unroofedOnly || !t.Position.Roofed(t.Map));
		}

		// Token: 0x06007F84 RID: 32644
		protected abstract Job TryGivePlayJob(Pawn pawn, Thing bestGame);

		// Token: 0x06007F85 RID: 32645 RVA: 0x002661BC File Offset: 0x002643BC
		protected virtual Job TryGivePlayJobWhileInBed(Pawn pawn, Thing bestGame)
		{
			Building_Bed t = pawn.CurrentBed();
			return JobMaker.MakeJob(this.def.jobDef, bestGame, pawn.Position, t);
		}

		// Token: 0x04005551 RID: 21841
		private static List<Thing> tmpCandidates = new List<Thing>();
	}
}
