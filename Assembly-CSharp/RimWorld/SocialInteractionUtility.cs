using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001784 RID: 6020
	public static class SocialInteractionUtility
	{
		// Token: 0x06008D40 RID: 36160 RVA: 0x002A0238 File Offset: 0x0029E438
		public static bool CanInitiateInteraction(Pawn pawn, InteractionDef interactionDef = null)
		{
			return pawn.interactions != null && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && pawn.Awake() && !pawn.IsBurning() && (!pawn.IsMutant || !pawn.mutant.Def.incapableOfSocialInteractions) && !pawn.IsInteractionBlocked(interactionDef, true, false);
		}

		// Token: 0x06008D41 RID: 36161 RVA: 0x002A02A8 File Offset: 0x0029E4A8
		public static IntVec3 GetAdjacentInteractionCell(Pawn pawn, Thing entityThing, bool forced)
		{
			IntVec3 result;
			SocialInteractionUtility.TryGetAdjacentInteractionCell(pawn, entityThing, forced, out result);
			return result;
		}

		// Token: 0x06008D42 RID: 36162 RVA: 0x002A02C4 File Offset: 0x0029E4C4
		public static bool TryGetAdjacentInteractionCell(Pawn pawn, Thing entityThing, bool forced, out IntVec3 cell)
		{
			if (entityThing is Pawn)
			{
				IntVec3 intVec;
				if (CellFinder.TryFindRandomCellNear(entityThing.Position, pawn.Map, 2, (IntVec3 x) => base.<TryGetAdjacentInteractionCell>g__CellIsValid|0(x, true), out intVec, -1) || CellFinder.TryFindRandomCellNear(entityThing.Position, pawn.Map, 2, (IntVec3 x) => base.<TryGetAdjacentInteractionCell>g__CellIsValid|0(x, false), out intVec, -1))
				{
					cell = intVec;
					return true;
				}
				cell = pawn.Position;
				return true;
			}
			else
			{
				IntVec3 intVec2;
				if ((from x in GenAdj.CellsAdjacentCardinal(entityThing)
				where base.<TryGetAdjacentInteractionCell>g__CellIsValid|0(x, true)
				select x).TryRandomElement(out intVec2) || (from x in GenAdj.CellsAdjacentCardinal(entityThing)
				where base.<TryGetAdjacentInteractionCell>g__CellIsValid|0(x, false)
				select x).TryRandomElement(out intVec2))
				{
					cell = intVec2;
					return true;
				}
				cell = pawn.Position;
				return false;
			}
		}

		// Token: 0x06008D43 RID: 36163 RVA: 0x002A03B4 File Offset: 0x0029E5B4
		public static bool CanReceiveInteraction(Pawn pawn, InteractionDef interactionDef = null)
		{
			return pawn.Awake() && !pawn.IsBurning() && (!pawn.IsMutant || !pawn.mutant.Def.incapableOfSocialInteractions) && !pawn.IsInteractionBlocked(interactionDef, false, false);
		}

		// Token: 0x06008D44 RID: 36164 RVA: 0x002A03F4 File Offset: 0x0029E5F4
		public static bool CanInitiateRandomInteraction(Pawn p)
		{
			return SocialInteractionUtility.CanInitiateInteraction(p, null) && p.RaceProps.Humanlike && !p.Downed && !p.InAggroMentalState && !p.IsInteractionBlocked(null, true, true) && p.Faction != null && p.ageTracker.CurLifeStage.canInitiateSocialInteraction && !p.Inhumanized();
		}

		// Token: 0x06008D45 RID: 36165 RVA: 0x002A045F File Offset: 0x0029E65F
		public static bool CanReceiveRandomInteraction(Pawn p)
		{
			return SocialInteractionUtility.CanReceiveInteraction(p, null) && p.RaceProps.Humanlike && !p.Downed && !p.InAggroMentalState;
		}

		// Token: 0x06008D46 RID: 36166 RVA: 0x002A048C File Offset: 0x0029E68C
		public static bool IsGoodPositionForInteraction(Pawn p, Pawn recipient)
		{
			return SocialInteractionUtility.IsGoodPositionForInteraction(p.Position, recipient.Position, p.Map);
		}

		// Token: 0x06008D47 RID: 36167 RVA: 0x002A04A5 File Offset: 0x0029E6A5
		public static bool IsGoodPositionForInteraction(IntVec3 cell, IntVec3 recipientCell, Map map)
		{
			return cell.InHorDistOf(recipientCell, 6f) && GenSight.LineOfSight(cell, recipientCell, map, true, null, 0, 0);
		}

		// Token: 0x06008D48 RID: 36168 RVA: 0x002A04C4 File Offset: 0x0029E6C4
		public static IntVec3 BestInteractableCell(Pawn actor, Pawn targetPawn)
		{
			IntVec3 intVec = IntVec3.Invalid;
			int num = 0;
			while (num < 9 && (num != 8 || !intVec.IsValid))
			{
				IntVec3 intVec2 = targetPawn.Position + GenAdj.AdjacentCellsAndInside[num];
				if (intVec2.InBounds(actor.Map) && intVec2.Walkable(actor.Map) && intVec2 != actor.Position && SocialInteractionUtility.IsGoodPositionForInteraction(intVec2, targetPawn.Position, actor.Map) && actor.CanReach(intVec2, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn) && (!intVec.IsValid || actor.Position.DistanceToSquared(intVec2) < actor.Position.DistanceToSquared(intVec)))
				{
					intVec = intVec2;
				}
				num++;
			}
			return intVec;
		}

		// Token: 0x06008D49 RID: 36169 RVA: 0x002A0588 File Offset: 0x0029E788
		public static bool HasAnyVerbForSocialFight(Pawn p)
		{
			if (p.Dead)
			{
				return false;
			}
			List<Verb> allVerbs = p.verbTracker.AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				if (allVerbs[i].IsMeleeAttack && allVerbs[i].IsStillUsableBy(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008D4A RID: 36170 RVA: 0x002A05DC File Offset: 0x0029E7DC
		public static bool TryGetRandomVerbForSocialFight(Pawn p, out Verb verb)
		{
			if (p.Dead)
			{
				verb = null;
				return false;
			}
			return (from x in p.verbTracker.AllVerbs
			where x.IsMeleeAttack && x.IsStillUsableBy(p)
			select x).TryRandomElementByWeight(delegate(Verb x)
			{
				float num = x.verbProps.AdjustedMeleeDamageAmount(x, p);
				Tool tool = x.tool;
				return num * ((tool != null) ? tool.chanceFactor : 1f);
			}, out verb);
		}

		// Token: 0x06008D4B RID: 36171 RVA: 0x002A063C File Offset: 0x0029E83C
		public static void ImitateSocialInteractionWithManyPawns(Pawn initiator, List<Pawn> targets, InteractionDef intDef)
		{
			List<Pawn> list = targets.Except(initiator).ToList<Pawn>();
			if (targets.NullOrEmpty<Pawn>())
			{
				Log.Error(((initiator != null) ? initiator.ToString() : null) + " tried to do interaction " + ((intDef != null) ? intDef.ToString() : null) + " with no targets. ");
				return;
			}
			if (intDef.initiatorXpGainSkill != null)
			{
				initiator.skills.Learn(intDef.initiatorXpGainSkill, (float)intDef.initiatorXpGainAmount, false, false);
			}
			foreach (Pawn pawn in list)
			{
				if (initiator != pawn && initiator.interactions.CanInteractNowWith(pawn, intDef))
				{
					if (intDef.recipientThought != null && pawn.needs.mood != null)
					{
						Pawn_InteractionsTracker.AddInteractionThought(pawn, initiator, intDef.recipientThought);
					}
					if (intDef.recipientXpGainSkill != null && pawn.RaceProps.Humanlike)
					{
						pawn.skills.Learn(intDef.recipientXpGainSkill, (float)intDef.recipientXpGainAmount, false, false);
					}
				}
			}
			LogEntry entry;
			if (list.Count > 0)
			{
				entry = new PlayLogEntry_InteractionWithMany(intDef, initiator, list, null);
				MoteMaker.MakeInteractionBubble(initiator, list.RandomElement<Pawn>(), intDef.interactionMote, intDef.GetSymbol(initiator.Faction, initiator.Ideo), intDef.GetSymbolColor(initiator.Faction));
			}
			else
			{
				entry = new PlayLogEntry_InteractionSinglePawn(intDef, initiator, null);
				MoteMaker.MakeInteractionBubble(initiator, null, intDef.interactionMote, intDef.GetSymbol(null, null), intDef.GetSymbolColor(null));
			}
			Find.PlayLog.Add(entry);
		}

		// Token: 0x06008D4C RID: 36172 RVA: 0x002A07C4 File Offset: 0x0029E9C4
		public static void ImitateInteractionWithNoPawn(Pawn initiator, InteractionDef intDef)
		{
			MoteMaker.MakeInteractionBubble(initiator, null, intDef.interactionMote, intDef.GetSymbol(null, null), intDef.GetSymbolColor(null));
			Find.PlayLog.Add(new PlayLogEntry_InteractionSinglePawn(intDef, initiator, null));
		}

		// Token: 0x04005BC7 RID: 23495
		public const float MaxInteractRange = 6f;
	}
}
