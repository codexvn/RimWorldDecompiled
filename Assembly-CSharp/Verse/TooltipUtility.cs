using System;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000BDF RID: 3039
	public static class TooltipUtility
	{
		// Token: 0x06004DD0 RID: 19920 RVA: 0x0018B81C File Offset: 0x00189A1C
		public static string ShotCalculationTipString(Thing target)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (Find.Selector.SingleSelectedThing != null)
			{
				Thing singleSelectedThing = Find.Selector.SingleSelectedThing;
				Verb verb = null;
				Pawn pawn = singleSelectedThing as Pawn;
				if (pawn != null && pawn != target)
				{
					Pawn_EquipmentTracker equipment = pawn.equipment;
					if (((equipment != null) ? equipment.Primary : null) != null && pawn.equipment.PrimaryEq.PrimaryVerb is Verb_LaunchProjectile && (!pawn.IsPlayerControlled || pawn.Drafted))
					{
						verb = pawn.equipment.PrimaryEq.PrimaryVerb;
						goto IL_97;
					}
				}
				Building_TurretGun building_TurretGun = singleSelectedThing as Building_TurretGun;
				if (building_TurretGun != null && building_TurretGun != target)
				{
					verb = building_TurretGun.AttackVerb;
				}
				IL_97:
				if (verb != null)
				{
					stringBuilder.Append("ShotBy".Translate(Find.Selector.SingleSelectedThing.LabelShort, Find.Selector.SingleSelectedThing) + ": ");
					if (verb.CanHitTarget(target))
					{
						stringBuilder.Append(ShotReport.HitReportFor(verb.caster, verb, target).GetTextReadout());
					}
					else
					{
						stringBuilder.AppendLine("CannotHit".Translate());
					}
					Pawn pawn2 = target as Pawn;
					if (pawn2 != null && pawn2.Faction == null && !pawn2.InAggroMentalState && pawn2.AnimalOrWildMan())
					{
						float manhunterOnDamageChance;
						if (verb.IsMeleeAttack)
						{
							manhunterOnDamageChance = PawnUtility.GetManhunterOnDamageChance(pawn2, singleSelectedThing, 0f);
						}
						else
						{
							manhunterOnDamageChance = PawnUtility.GetManhunterOnDamageChance(pawn2, singleSelectedThing, -1f);
						}
						if (manhunterOnDamageChance > 0f)
						{
							stringBuilder.AppendLine();
							stringBuilder.AppendLine(string.Format("{0}: {1}", "ManhunterPerHit".Translate(), manhunterOnDamageChance.ToStringPercent()));
						}
					}
				}
			}
			return stringBuilder.ToString();
		}
	}
}
