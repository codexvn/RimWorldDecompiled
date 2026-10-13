using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013FE RID: 5118
	public abstract class JobGiver_AIDefendPawn : JobGiver_AIFightEnemy
	{
		// Token: 0x06007CC2 RID: 31938 RVA: 0x00259404 File Offset: 0x00257604
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AIDefendPawn jobGiver_AIDefendPawn = (JobGiver_AIDefendPawn)base.DeepCopy(resolve);
			jobGiver_AIDefendPawn.attackMeleeThreatEvenIfNotHostile = this.attackMeleeThreatEvenIfNotHostile;
			return jobGiver_AIDefendPawn;
		}

		// Token: 0x06007CC3 RID: 31939
		protected abstract Pawn GetDefendee(Pawn pawn);

		// Token: 0x06007CC4 RID: 31940 RVA: 0x00259420 File Offset: 0x00257620
		protected override IntVec3 GetFlagPosition(Pawn pawn)
		{
			Pawn defendee = this.GetDefendee(pawn);
			if (defendee.Spawned || defendee.CarriedBy != null)
			{
				return defendee.PositionHeld;
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06007CC5 RID: 31941 RVA: 0x00259454 File Offset: 0x00257654
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn defendee = this.GetDefendee(pawn);
			if (defendee == null)
			{
				Type type = base.GetType();
				Log.Error(((type != null) ? type.ToString() : null) + " has null defendee. pawn=" + pawn.ToStringSafe<Pawn>());
				return null;
			}
			Pawn carriedBy = defendee.CarriedBy;
			if (carriedBy != null)
			{
				if (!pawn.CanReach(carriedBy, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return null;
				}
			}
			else if (!defendee.Spawned || !pawn.CanReach(defendee, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x06007CC6 RID: 31942 RVA: 0x002594D8 File Offset: 0x002576D8
		protected override Thing FindAttackTarget(Pawn pawn)
		{
			if (this.attackMeleeThreatEvenIfNotHostile)
			{
				Pawn defendee = this.GetDefendee(pawn);
				if (defendee.Spawned && !defendee.InMentalState && defendee.mindState.meleeThreat != null && defendee.mindState.meleeThreat != pawn && defendee.mindState.MeleeThreatStillThreat && pawn.CanReach(defendee.mindState.meleeThreat, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return defendee.mindState.meleeThreat;
				}
			}
			return base.FindAttackTarget(pawn);
		}

		// Token: 0x06007CC7 RID: 31943 RVA: 0x00259560 File Offset: 0x00257760
		protected override bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest, Verb verbToUse = null)
		{
			Verb verb = verbToUse ?? pawn.TryGetAttackVerb(null, !pawn.IsColonist, false);
			if (verb == null)
			{
				dest = IntVec3.Invalid;
				return false;
			}
			return CastPositionFinder.TryFindCastPosition(new CastPositionRequest
			{
				caster = pawn,
				target = pawn.mindState.enemyTarget,
				verb = verb,
				maxRangeFromTarget = 9999f,
				locus = this.GetDefendee(pawn).PositionHeld,
				maxRangeFromLocus = this.GetFlagRadius(pawn),
				wantCoverFromTarget = (verb.EffectiveRange > 7f),
				maxRegions = 50
			}, out dest);
		}

		// Token: 0x04005411 RID: 21521
		private bool attackMeleeThreatEvenIfNotHostile;
	}
}
