using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200140C RID: 5132
	public class JobGiver_AIJumpEscapeEnemies : ThinkNode_JobGiver
	{
		// Token: 0x06007D08 RID: 32008 RVA: 0x0025A3A8 File Offset: 0x002585A8
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn_AbilityTracker abilities = pawn.abilities;
			Ability ability = (abilities != null) ? abilities.GetAbility(this.ability, false) : null;
			if (ability == null || !ability.CanCast)
			{
				return null;
			}
			IntVec3 c;
			if (this.TryFindRelocatePosition(pawn, out c, ability.verb.EffectiveRange))
			{
				return ability.GetJob(c, c);
			}
			return null;
		}

		// Token: 0x06007D09 RID: 32009 RVA: 0x0025A40C File Offset: 0x0025860C
		private bool TryFindRelocatePosition(Pawn pawn, out IntVec3 relocatePosition, float maxDistance)
		{
			JobGiver_AIJumpEscapeEnemies.<>c__DisplayClass3_0 CS$<>8__locals1 = new JobGiver_AIJumpEscapeEnemies.<>c__DisplayClass3_0();
			CS$<>8__locals1.pawn = pawn;
			JobGiver_AIJumpEscapeEnemies.tmpHostileSpots.Clear();
			JobGiver_AIJumpEscapeEnemies.tmpHostileSpots.AddRange(from a in CS$<>8__locals1.pawn.Map.attackTargetsCache.GetPotentialTargetsFor(CS$<>8__locals1.pawn)
			where !a.ThreatDisabled(CS$<>8__locals1.pawn)
			select a.Thing);
			JobGiver_AIJumpEscapeEnemies.<>c__DisplayClass3_0 CS$<>8__locals2 = CS$<>8__locals1;
			Pawn_AbilityTracker abilities = CS$<>8__locals1.pawn.abilities;
			CS$<>8__locals2.jump = ((abilities != null) ? abilities.GetAbility(this.ability, false) : null);
			relocatePosition = CellFinderLoose.GetFallbackDest(CS$<>8__locals1.pawn, JobGiver_AIJumpEscapeEnemies.tmpHostileSpots, maxDistance, 5f, 5f, 20, (IntVec3 c) => CS$<>8__locals1.jump.verb.ValidateTarget(c, false));
			JobGiver_AIJumpEscapeEnemies.tmpHostileSpots.Clear();
			return relocatePosition.IsValid;
		}

		// Token: 0x06007D0A RID: 32010 RVA: 0x0025A4ED File Offset: 0x002586ED
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AIJumpEscapeEnemies jobGiver_AIJumpEscapeEnemies = (JobGiver_AIJumpEscapeEnemies)base.DeepCopy(resolve);
			jobGiver_AIJumpEscapeEnemies.ability = this.ability;
			return jobGiver_AIJumpEscapeEnemies;
		}

		// Token: 0x04005426 RID: 21542
		private AbilityDef ability;

		// Token: 0x04005427 RID: 21543
		private static List<Thing> tmpHostileSpots = new List<Thing>();
	}
}
