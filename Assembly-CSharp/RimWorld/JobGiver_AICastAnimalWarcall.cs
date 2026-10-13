using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013FB RID: 5115
	public class JobGiver_AICastAnimalWarcall : JobGiver_AICastAbility
	{
		// Token: 0x06007CBA RID: 31930 RVA: 0x002591CC File Offset: 0x002573CC
		protected override LocalTargetInfo GetTarget(Pawn caster, Ability ability)
		{
			JobGiver_AICastAnimalWarcall.potentialTargets.Clear();
			IEnumerable<Thing> hostiles = from x in caster.Map.attackTargetsCache.GetPotentialTargetsFor(caster)
			select x.Thing;
			if (hostiles.EnumerableNullOrEmpty<Thing>())
			{
				return LocalTargetInfo.Invalid;
			}
			foreach (Pawn pawn in caster.Map.mapPawns.AllPawnsSpawned)
			{
				if ((!pawn.IsMutant || !pawn.mutant.Def.preventsMentalBreaks) && pawn.RaceProps.Animal && pawn.Faction == null && pawn.MentalStateDef != MentalStateDefOf.BerserkWarcall && pawn.Position.InHorDistOf(caster.Position, 30f) && ability.CanApplyOn(new LocalTargetInfo(pawn)))
				{
					JobGiver_AICastAnimalWarcall.potentialTargets.Add(pawn);
				}
			}
			Pawn thing;
			if (JobGiver_AICastAnimalWarcall.potentialTargets.TryRandomElementByWeight(delegate(Pawn x)
			{
				float num = 625f;
				foreach (Thing thing2 in hostiles)
				{
					if (thing2.Spawned)
					{
						float num2 = (float)thing2.Position.DistanceToSquared(x.Position);
						if (num2 < num)
						{
							num = num2;
						}
					}
				}
				return JobGiver_AICastAnimalWarcall.DistanceSquaredToTargetSelectionWeightCurve.Evaluate(num);
			}, out thing))
			{
				return new LocalTargetInfo(thing);
			}
			return LocalTargetInfo.Invalid;
		}

		// Token: 0x0400540A RID: 21514
		private const float MaxDistanceFromCaster = 30f;

		// Token: 0x0400540B RID: 21515
		private const float MaxSquareDistanceFromTarget = 625f;

		// Token: 0x0400540C RID: 21516
		private static readonly SimpleCurve DistanceSquaredToTargetSelectionWeightCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 1f),
				true
			},
			{
				new CurvePoint(400f, 0.1f),
				true
			},
			{
				new CurvePoint(625f, 0f),
				true
			}
		};

		// Token: 0x0400540D RID: 21517
		private static List<Pawn> potentialTargets = new List<Pawn>();
	}
}
