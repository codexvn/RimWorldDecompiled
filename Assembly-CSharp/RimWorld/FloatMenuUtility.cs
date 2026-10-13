using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02003350 RID: 13136
	public static class FloatMenuUtility
	{
		// Token: 0x06012767 RID: 75623 RVA: 0x005713B8 File Offset: 0x0056F5B8
		public static void MakeMenu<T>(IEnumerable<T> objects, Func<T, string> labelGetter, Func<T, Action> actionGetter)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (T arg in objects)
			{
				list.Add(new FloatMenuOption(labelGetter(arg), actionGetter(arg), MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06012768 RID: 75624 RVA: 0x00571434 File Offset: 0x0056F634
		public static Action GetRangedAttackAction(Pawn pawn, LocalTargetInfo target, out string failStr)
		{
			failStr = "";
			if (pawn.equipment.Primary == null)
			{
				return null;
			}
			Verb primaryVerb = pawn.equipment.PrimaryEq.PrimaryVerb;
			if (primaryVerb.verbProps.IsMeleeAttack)
			{
				return null;
			}
			if (!pawn.Drafted)
			{
				failStr = "IsNotDraftedLower".Translate(pawn.LabelShort, pawn);
			}
			else if (!pawn.IsColonistPlayerControlled && !pawn.IsColonyMech && !pawn.IsColonySubhumanPlayerControlled)
			{
				failStr = "CannotOrderNonControlledLower".Translate();
			}
			else if (pawn.IsColonyMechPlayerControlled && target.IsValid && !MechanitorUtility.InMechanitorCommandRange(pawn, target))
			{
				failStr = "OutOfCommandRange".Translate();
			}
			else if (target.IsValid && !pawn.equipment.PrimaryEq.PrimaryVerb.CanHitTarget(target))
			{
				if (!pawn.Position.InHorDistOf(target.Cell, primaryVerb.EffectiveRange))
				{
					failStr = "OutOfRange".Translate();
				}
				else
				{
					float num = primaryVerb.verbProps.EffectiveMinRange(target, pawn);
					if ((float)pawn.Position.DistanceToSquared(target.Cell) < num * num)
					{
						failStr = "TooClose".Translate();
					}
					else
					{
						failStr = "CannotHitTarget".Translate();
					}
				}
			}
			else if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				failStr = "IsIncapableOfViolenceLower".Translate(pawn.LabelShort, pawn);
			}
			else if (pawn == target.Thing)
			{
				failStr = "CannotAttackSelf".Translate();
			}
			else
			{
				Pawn pawn2 = target.Thing as Pawn;
				if (pawn2 != null && (pawn.InSameExtraFaction(pawn2, ExtraFactionType.HomeFaction, null) || pawn.InSameExtraFaction(pawn2, ExtraFactionType.MiniFaction, null)))
				{
					failStr = "CannotAttackSameFactionMember".Translate();
				}
				else
				{
					Pawn pawn3 = target.Thing as Pawn;
					if (pawn3 != null && HistoryEventUtility.IsKillingInnocentAnimal(pawn, pawn3) && !new HistoryEvent(HistoryEventDefOf.KilledInnocentAnimal, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo())
					{
						failStr = "IdeoligionForbids".Translate();
					}
					else
					{
						Pawn pawn4 = target.Thing as Pawn;
						if (pawn4 == null || pawn.Ideo == null || !pawn.Ideo.IsVeneratedAnimal(pawn4) || new HistoryEvent(HistoryEventDefOf.HuntedVeneratedAnimal, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo())
						{
							return delegate()
							{
								Job job = JobMaker.MakeJob(JobDefOf.AttackStatic, target);
								pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
							};
						}
						failStr = "IdeoligionForbids".Translate();
					}
				}
			}
			failStr = failStr.CapitalizeFirst();
			return null;
		}

		// Token: 0x06012769 RID: 75625 RVA: 0x005717BC File Offset: 0x0056F9BC
		public static Action GetMeleeAttackAction(Pawn pawn, LocalTargetInfo target, out string failStr, bool ignoreControlled = false)
		{
			failStr = "";
			if (!pawn.Drafted && !ignoreControlled)
			{
				failStr = "IsNotDraftedLower".Translate(pawn.LabelShort, pawn);
			}
			else if (!pawn.IsColonistPlayerControlled && !pawn.IsColonyMech && !pawn.IsColonySubhumanPlayerControlled && !ignoreControlled)
			{
				failStr = "CannotOrderNonControlledLower".Translate();
			}
			else if (pawn.IsColonyMechPlayerControlled && target.IsValid && !MechanitorUtility.InMechanitorCommandRange(pawn, target))
			{
				failStr = "OutOfCommandRange".Translate();
			}
			else if (target.IsValid && !pawn.CanReach(target, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				failStr = "NoPath".Translate();
			}
			else if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				failStr = "IsIncapableOfViolenceLower".Translate(pawn.LabelShort, pawn);
			}
			else if (pawn.meleeVerbs.TryGetMeleeVerb(target.Thing) == null)
			{
				failStr = "Incapable".Translate();
			}
			else if (pawn == target.Thing)
			{
				failStr = "CannotAttackSelf".Translate();
			}
			else
			{
				Pawn pawn2 = target.Thing as Pawn;
				if (pawn2 != null && (pawn.InSameExtraFaction(pawn2, ExtraFactionType.HomeFaction, null) || pawn.InSameExtraFaction(pawn2, ExtraFactionType.MiniFaction, null)))
				{
					failStr = "CannotAttackSameFactionMember".Translate();
				}
				else
				{
					Pawn pawn3 = target.Thing as Pawn;
					if (pawn3 == null || !pawn3.RaceProps.Animal || !HistoryEventUtility.IsKillingInnocentAnimal(pawn, pawn3) || new HistoryEvent(HistoryEventDefOf.KilledInnocentAnimal, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo())
					{
						return delegate()
						{
							Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, target);
							Pawn pawn4 = target.Thing as Pawn;
							if (pawn4 != null)
							{
								job.killIncappedTarget = pawn4.Downed;
							}
							pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
						};
					}
					failStr = "IdeoligionForbids".Translate();
				}
			}
			failStr = failStr.CapitalizeFirst();
			return null;
		}

		// Token: 0x0601276A RID: 75626 RVA: 0x00571A3F File Offset: 0x0056FC3F
		public static bool UseRangedAttack(Pawn pawn)
		{
			return pawn.equipment.Primary != null && !pawn.equipment.PrimaryEq.PrimaryVerb.verbProps.IsMeleeAttack;
		}

		// Token: 0x0601276B RID: 75627 RVA: 0x00571A6D File Offset: 0x0056FC6D
		public static Action GetAttackAction(Pawn pawn, LocalTargetInfo target, out string failStr)
		{
			if (FloatMenuUtility.UseRangedAttack(pawn))
			{
				return FloatMenuUtility.GetRangedAttackAction(pawn, target, out failStr);
			}
			return FloatMenuUtility.GetMeleeAttackAction(pawn, target, out failStr, false);
		}

		// Token: 0x0601276C RID: 75628 RVA: 0x00571A8C File Offset: 0x0056FC8C
		public static FloatMenuOption DecoratePrioritizedTask(FloatMenuOption option, Pawn pawn, LocalTargetInfo target, string reservedText = "ReservedBy", ReservationLayerDef layer = null)
		{
			if (option.action == null)
			{
				return option;
			}
			if (pawn != null && !pawn.CanReserve(target, 1, -1, layer, false) && pawn.CanReserve(target, 1, -1, layer, true))
			{
				Pawn pawn2 = pawn.Map.reservationManager.FirstRespectedReserver(target, pawn, layer);
				if (pawn2 == null)
				{
					pawn2 = pawn.Map.physicalInteractionReservationManager.FirstReserverOf(target);
				}
				if (pawn2 != null)
				{
					option.Label = option.Label + ": " + reservedText.Translate(pawn2.LabelShort, pawn2);
				}
			}
			if (option.revalidateClickTarget != null && option.revalidateClickTarget != target.Thing)
			{
				Log.ErrorOnce(string.Format("Click target mismatch; {0} vs {1} in {2}", option.revalidateClickTarget, target.Thing, option.Label), 52753118);
			}
			option.revalidateClickTarget = target.Thing;
			return option;
		}

		// Token: 0x0601276D RID: 75629 RVA: 0x00571B78 File Offset: 0x0056FD78
		public static void ValidateTakeToBedOption(Pawn pawn, Pawn target, FloatMenuOption option, string cannot, GuestStatus? guestStatus = null)
		{
			if (RestUtility.FindBedFor(target, pawn, false, false, guestStatus) == null)
			{
				Building_Bed building_Bed = RestUtility.FindBedFor(target, pawn, false, true, guestStatus);
				if (building_Bed != null)
				{
					Pawn pawn2;
					if (pawn.MapHeld.reservationManager.TryGetReserver(building_Bed, pawn.Faction, out pawn2))
					{
						option.Label = string.Concat(new string[]
						{
							option.Label,
							" (",
							building_Bed.def.label,
							" ",
							"ReservedBy".Translate(pawn2.LabelShort, pawn2).Resolve().StripTags(),
							")"
						});
						return;
					}
				}
				else
				{
					option.Disabled = true;
					option.Label = cannot;
				}
			}
		}
	}
}
