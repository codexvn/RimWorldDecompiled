using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D6A RID: 15722
	public static class CaravanUtility
	{
		// Token: 0x060156A8 RID: 87720 RVA: 0x00668B07 File Offset: 0x00666D07
		public static bool IsOwner(Pawn pawn, Faction caravanFaction)
		{
			if (caravanFaction == null)
			{
				Log.Warning("Called IsOwner with null faction.");
				return false;
			}
			return !pawn.NonHumanlikeOrWildMan() && pawn.Faction == caravanFaction && pawn.HostFaction == null && !pawn.IsSlave;
		}

		// Token: 0x060156A9 RID: 87721 RVA: 0x00668B3C File Offset: 0x00666D3C
		public static bool IsCaravanMember(this Pawn pawn)
		{
			return pawn.GetCaravan() != null;
		}

		// Token: 0x060156AA RID: 87722 RVA: 0x00668B48 File Offset: 0x00666D48
		public static bool IsPlayerControlledCaravanMember(this Pawn pawn)
		{
			Caravan caravan = pawn.GetCaravan();
			return caravan != null && caravan.IsPlayerControlled;
		}

		// Token: 0x060156AB RID: 87723 RVA: 0x00668B68 File Offset: 0x00666D68
		public static PlanetTile BestGotoDestNear(PlanetTile tile, Caravan c)
		{
			CaravanUtility.<>c__DisplayClass3_0 CS$<>8__locals1 = new CaravanUtility.<>c__DisplayClass3_0();
			CS$<>8__locals1.c = c;
			if (CS$<>8__locals1.<BestGotoDestNear>g__IsGoodDest|0(tile))
			{
				return tile;
			}
			PlanetTile result;
			GenWorldClosest.TryFindClosestTile(tile, new Predicate<PlanetTile>(CS$<>8__locals1.<BestGotoDestNear>g__IsGoodDest|0), out result, 50, true);
			return result;
		}

		// Token: 0x060156AC RID: 87724 RVA: 0x00668BA8 File Offset: 0x00666DA8
		public static bool PlayerHasAnyCaravan()
		{
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = 0; i < caravans.Count; i++)
			{
				if (caravans[i].IsPlayerControlled)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060156AD RID: 87725 RVA: 0x00668BE4 File Offset: 0x00666DE4
		public static Pawn RandomOwner(this Caravan caravan)
		{
			return (from p in caravan.PawnsListForReading
			where caravan.IsOwner(p)
			select p).RandomElement<Pawn>();
		}

		// Token: 0x060156AE RID: 87726 RVA: 0x00668C1F File Offset: 0x00666E1F
		public static bool ShouldAutoCapture(Pawn p, Faction caravanFaction)
		{
			return p.RaceProps.Humanlike && !p.Dead && p.Faction != caravanFaction && (!p.IsPrisoner || p.HostFaction != caravanFaction);
		}

		// Token: 0x060156AF RID: 87727 RVA: 0x00668C58 File Offset: 0x00666E58
		public static PlanetTile GetTileCurrentlyOver(this Caravan caravan)
		{
			if (caravan.pather.Moving && caravan.pather.IsNextTilePassable() && 1f - caravan.pather.nextTileCostLeft / caravan.pather.nextTileCostTotal > 0.5f)
			{
				return caravan.pather.nextTile;
			}
			return caravan.Tile;
		}

		// Token: 0x060156B0 RID: 87728 RVA: 0x00668CB8 File Offset: 0x00666EB8
		public static Caravan GetCaravan(this Thing thing)
		{
			Caravan caravan = thing.ParentHolder as Caravan;
			if (caravan != null)
			{
				return caravan;
			}
			Pawn_InventoryTracker pawn_InventoryTracker = thing.ParentHolder as Pawn_InventoryTracker;
			if (pawn_InventoryTracker != null)
			{
				return pawn_InventoryTracker.pawn.GetCaravan();
			}
			Thing thing2 = thing.ParentHolder as Thing;
			if (thing2 != null)
			{
				return thing2.GetCaravan();
			}
			return null;
		}

		// Token: 0x060156B1 RID: 87729 RVA: 0x00668B3C File Offset: 0x00666D3C
		public static bool IsInCaravan(this Thing thing)
		{
			return thing.GetCaravan() != null;
		}
	}
}
