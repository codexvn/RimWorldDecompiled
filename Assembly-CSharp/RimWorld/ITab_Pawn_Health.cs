using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020033B7 RID: 13239
	public class ITab_Pawn_Health : ITab
	{
		// Token: 0x170030AB RID: 12459
		// (get) Token: 0x060129BA RID: 76218 RVA: 0x005817E8 File Offset: 0x0057F9E8
		private Pawn PawnForHealth
		{
			get
			{
				if (this.SelPawn != null)
				{
					return this.SelPawn;
				}
				Corpse corpse = base.SelThing as Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
				return null;
			}
		}

		// Token: 0x060129BB RID: 76219 RVA: 0x0058181B File Offset: 0x0057FA1B
		public ITab_Pawn_Health()
		{
			this.size = new Vector2(630f, 430f);
			this.labelKey = "TabHealth";
			this.tutorTag = "Health";
		}

		// Token: 0x060129BC RID: 76220 RVA: 0x00581850 File Offset: 0x0057FA50
		protected override void FillTab()
		{
			Pawn pawnForHealth = this.PawnForHealth;
			if (pawnForHealth == null)
			{
				Log.Error("Health tab found no selected pawn to display.");
				return;
			}
			HealthCardUtility.DrawPawnHealthCard(new Rect(Vector2.zero, this.size), pawnForHealth, this.ShouldAllowOperations(), HealthCardUtility.ShowBloodLoss(base.SelThing), base.SelThing);
		}

		// Token: 0x060129BD RID: 76221 RVA: 0x005818A0 File Offset: 0x0057FAA0
		private bool ShouldAllowOperations()
		{
			Pawn pawn = this.PawnForHealth;
			return !pawn.Dead && base.SelThing.def.AllRecipes.Any((RecipeDef x) => x.AvailableNow && x.AvailableOnNow(pawn, null)) && (!pawn.IsMutant || pawn.mutant.Def.entitledToMedicalCare) && (pawn.Faction == Faction.OfPlayer || (pawn.IsPrisonerOfColony || (pawn.HostFaction == Faction.OfPlayer && !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving))) || ((!pawn.RaceProps.IsFlesh || pawn.Faction == null || !pawn.Faction.HostileTo(Faction.OfPlayer)) && (!pawn.RaceProps.Humanlike && pawn.Downed)));
		}

		// Token: 0x0400C140 RID: 49472
		public const float Width = 630f;
	}
}
