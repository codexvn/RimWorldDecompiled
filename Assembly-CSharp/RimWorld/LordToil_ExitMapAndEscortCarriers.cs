using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016F9 RID: 5881
	public class LordToil_ExitMapAndEscortCarriers : LordToil
	{
		// Token: 0x17001548 RID: 5448
		// (get) Token: 0x06008AA9 RID: 35497 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001549 RID: 5449
		// (get) Token: 0x06008AAA RID: 35498 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008AAB RID: 35499 RVA: 0x00295938 File Offset: 0x00293B38
		public override void UpdateAllDuties()
		{
			Pawn trader;
			this.UpdateTraderDuty(out trader);
			this.UpdateCarriersDuties(trader);
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				TraderCaravanRole traderCaravanRole = pawn.GetTraderCaravanRole();
				if (traderCaravanRole != TraderCaravanRole.Carrier && traderCaravanRole != TraderCaravanRole.Trader && pawn.Spawned)
				{
					this.UpdateDutyForChattelOrGuard(pawn, trader);
				}
			}
		}

		// Token: 0x06008AAC RID: 35500 RVA: 0x002959A0 File Offset: 0x00293BA0
		private void UpdateTraderDuty(out Pawn trader)
		{
			trader = TraderCaravanUtility.FindTrader(this.lord);
			if (trader != null)
			{
				trader.mindState.duty = new PawnDuty(DutyDefOf.ExitMapBestAndDefendSelf);
				trader.mindState.duty.radius = 18f;
				trader.mindState.duty.locomotion = LocomotionUrgency.Jog;
			}
		}

		// Token: 0x06008AAD RID: 35501 RVA: 0x002959FC File Offset: 0x00293BFC
		private void UpdateCarriersDuties(Pawn trader)
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn.GetTraderCaravanRole() == TraderCaravanRole.Carrier)
				{
					if (trader != null)
					{
						pawn.mindState.duty = new PawnDuty(DutyDefOf.Follow, trader, 5f);
					}
					else
					{
						pawn.mindState.duty = new PawnDuty(DutyDefOf.ExitMapBest);
						pawn.mindState.duty.locomotion = LocomotionUrgency.Jog;
					}
				}
			}
		}

		// Token: 0x06008AAE RID: 35502 RVA: 0x00295A8C File Offset: 0x00293C8C
		private void UpdateDutyForChattelOrGuard(Pawn p, Pawn trader)
		{
			if (p.GetTraderCaravanRole() == TraderCaravanRole.Chattel)
			{
				if (trader != null)
				{
					p.mindState.duty = new PawnDuty(DutyDefOf.Escort, trader, 14f);
					return;
				}
				if (!this.TryToDefendClosestCarrier(p, 14f))
				{
					p.mindState.duty = new PawnDuty(DutyDefOf.ExitMapBestAndDefendSelf);
					p.mindState.duty.radius = 10f;
					p.mindState.duty.locomotion = LocomotionUrgency.Jog;
					return;
				}
			}
			else if (!this.TryToDefendClosestCarrier(p, 26f))
			{
				if (trader != null)
				{
					p.mindState.duty = new PawnDuty(DutyDefOf.Escort, trader, 26f);
					return;
				}
				p.mindState.duty = new PawnDuty(DutyDefOf.ExitMapBestAndDefendSelf);
				p.mindState.duty.radius = 18f;
				p.mindState.duty.locomotion = LocomotionUrgency.Jog;
			}
		}

		// Token: 0x06008AAF RID: 35503 RVA: 0x00295B80 File Offset: 0x00293D80
		private bool TryToDefendClosestCarrier(Pawn p, float escortRadius)
		{
			if (!p.Spawned)
			{
				return false;
			}
			Pawn closestCarrier = this.GetClosestCarrier(p);
			Thing thing = GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForGroup(ThingRequestGroup.Corpse), PathEndMode.ClosestTouch, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 20f, delegate(Thing x)
			{
				Pawn innerPawn = ((Corpse)x).InnerPawn;
				return innerPawn.Faction == p.Faction && innerPawn.RaceProps.packAnimal;
			}, null, 0, 15, false, RegionType.Set_Passable, false, false);
			Thing thing2 = GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.ClosestTouch, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 20f, delegate(Thing x)
			{
				Pawn pawn = (Pawn)x;
				return pawn.Downed && pawn.Faction == p.Faction && pawn.GetTraderCaravanRole() == TraderCaravanRole.Carrier;
			}, null, 0, 15, false, RegionType.Set_Passable, false, false);
			Thing thing3 = null;
			if (closestCarrier != null)
			{
				thing3 = closestCarrier;
			}
			if (thing != null && (thing3 == null || thing.Position.DistanceToSquared(p.Position) < thing3.Position.DistanceToSquared(p.Position)))
			{
				thing3 = thing;
			}
			if (thing2 != null && (thing3 == null || thing2.Position.DistanceToSquared(p.Position) < thing3.Position.DistanceToSquared(p.Position)))
			{
				thing3 = thing2;
			}
			if (thing3 == null)
			{
				return false;
			}
			if (thing3 is Pawn && !((Pawn)thing3).Downed)
			{
				p.mindState.duty = new PawnDuty(DutyDefOf.Escort, thing3, escortRadius);
				return true;
			}
			if (!GenHostility.AnyHostileActiveThreatTo(base.Map, this.lord.faction, true, false))
			{
				return false;
			}
			p.mindState.duty = new PawnDuty(DutyDefOf.Defend, thing3.Position, 16f);
			return true;
		}

		// Token: 0x06008AB0 RID: 35504 RVA: 0x00295D57 File Offset: 0x00293F57
		public static bool IsDefendingPosition(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.def == DutyDefOf.Defend;
		}

		// Token: 0x06008AB1 RID: 35505 RVA: 0x00295D80 File Offset: 0x00293F80
		public static bool IsAnyDefendingPosition(List<Pawn> pawns)
		{
			for (int i = 0; i < pawns.Count; i++)
			{
				if (LordToil_ExitMapAndEscortCarriers.IsDefendingPosition(pawns[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008AB2 RID: 35506 RVA: 0x00295DB0 File Offset: 0x00293FB0
		private Pawn GetClosestCarrier(Pawn closestTo)
		{
			Pawn pawn = null;
			float num = 0f;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn2 = this.lord.ownedPawns[i];
				if (pawn2.GetTraderCaravanRole() == TraderCaravanRole.Carrier)
				{
					float num2 = (float)pawn2.Position.DistanceToSquared(closestTo.Position);
					if (pawn == null || num2 < num)
					{
						pawn = pawn2;
						num = num2;
					}
				}
			}
			return pawn;
		}
	}
}
