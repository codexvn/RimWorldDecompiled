using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001410 RID: 5136
	public class JobGiver_AIResurrectTarget : ThinkNode_JobGiver
	{
		// Token: 0x06007D15 RID: 32021 RVA: 0x0025A57C File Offset: 0x0025877C
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			using (new ProfilerBlock("JobGiver_AIResurrectTarget.TryGiveJob"))
			{
				Ability ability = pawn.abilities.GetAbility(this.abilityDef, false);
				if (ability == null || !ability.CanCast)
				{
					result = null;
				}
				else if (!pawn.Spawned)
				{
					result = null;
				}
				else
				{
					this.UpdateResurrectTarget(pawn);
					if (pawn.mindState.resurrectTarget == null)
					{
						result = null;
					}
					else
					{
						Job job = ability.GetJob(pawn.mindState.resurrectTarget.corpse, pawn.mindState.resurrectTarget.castPosition);
						job.expiryInterval = this.expiryInterval;
						result = job;
					}
				}
			}
			return result;
		}

		// Token: 0x06007D16 RID: 32022 RVA: 0x0025A640 File Offset: 0x00258840
		private static HashSet<Corpse> ReservedCorpsesForResurrection(Map map, Faction faction)
		{
			HashSet<Corpse> result;
			using (new ProfilerBlock("ReservedCorpsesForResurrection"))
			{
				JobGiver_AIResurrectTarget.tmpReservedCorpses.Clear();
				List<Pawn> list = map.mapPawns.SpawnedPawnsInFaction(faction);
				for (int i = 0; i < list.Count; i++)
				{
					Pawn_MindState mindState = list[i].mindState;
					Corpse corpse;
					if (mindState == null)
					{
						corpse = null;
					}
					else
					{
						ResurrectCorpseData resurrectTarget = mindState.resurrectTarget;
						corpse = ((resurrectTarget != null) ? resurrectTarget.corpse : null);
					}
					Corpse corpse2 = corpse;
					if (corpse2 != null)
					{
						JobGiver_AIResurrectTarget.tmpReservedCorpses.Add(corpse2);
					}
				}
				result = JobGiver_AIResurrectTarget.tmpReservedCorpses;
			}
			return result;
		}

		// Token: 0x06007D17 RID: 32023 RVA: 0x0025A6E0 File Offset: 0x002588E0
		private void UpdateResurrectTarget(Pawn pawn)
		{
			pawn.mindState.resurrectTarget = null;
			Ability ability = pawn.abilities.GetAbility(this.abilityDef, false);
			List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse);
			list.SortBy((Thing c) => c.Position.DistanceToSquared(pawn.Position));
			HashSet<Corpse> hashSet = JobGiver_AIResurrectTarget.ReservedCorpsesForResurrection(pawn.Map, pawn.Faction);
			for (int i = 0; i < list.Count; i++)
			{
				Corpse corpse = (Corpse)list[i];
				if (!hashSet.Contains(corpse) && JobGiver_AIResurrectTarget.ShouldResurrectCorpse(corpse, pawn) && ability.CanApplyOn(new LocalTargetInfo(corpse)))
				{
					using (new ProfilerBlock("TryFindCastPosition"))
					{
						IntVec3 castPosition;
						if (CastPositionFinder.TryFindCastPosition(new CastPositionRequest
						{
							caster = pawn,
							target = corpse,
							verb = ability.verb,
							maxRangeFromTarget = ability.verb.EffectiveRange,
							wantCoverFromTarget = false,
							maxRegions = this.maxRegions
						}, out castPosition))
						{
							pawn.mindState.resurrectTarget = new ResurrectCorpseData(corpse, castPosition);
							break;
						}
					}
				}
			}
		}

		// Token: 0x06007D18 RID: 32024 RVA: 0x0025A868 File Offset: 0x00258A68
		public static bool ShouldResurrectCorpse(Corpse corpse, Pawn pawn)
		{
			return corpse != null && corpse.Spawned && corpse.Map == pawn.Map && corpse.InnerPawn.Faction == pawn.Faction && pawn.CanReserve(corpse, 1, -1, null, false) && !corpse.InnerPawn.kindDef.abilities.NotNullAndContains(AbilityDefOf.ResurrectionMech);
		}

		// Token: 0x06007D19 RID: 32025 RVA: 0x0025A8D2 File Offset: 0x00258AD2
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AIResurrectTarget jobGiver_AIResurrectTarget = (JobGiver_AIResurrectTarget)base.DeepCopy(resolve);
			jobGiver_AIResurrectTarget.abilityDef = this.abilityDef;
			jobGiver_AIResurrectTarget.expiryInterval = this.expiryInterval;
			jobGiver_AIResurrectTarget.maxRegions = this.maxRegions;
			return jobGiver_AIResurrectTarget;
		}

		// Token: 0x0400542C RID: 21548
		private AbilityDef abilityDef;

		// Token: 0x0400542D RID: 21549
		private int expiryInterval = 500;

		// Token: 0x0400542E RID: 21550
		private int maxRegions = 50;

		// Token: 0x0400542F RID: 21551
		private static readonly HashSet<Corpse> tmpReservedCorpses = new HashSet<Corpse>();
	}
}
