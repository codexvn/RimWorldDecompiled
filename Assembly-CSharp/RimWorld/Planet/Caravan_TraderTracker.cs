using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D97 RID: 15767
	public class Caravan_TraderTracker : IExposable
	{
		// Token: 0x17003569 RID: 13673
		// (get) Token: 0x060157B1 RID: 87985 RVA: 0x0066FC44 File Offset: 0x0066DE44
		public TraderKindDef TraderKind
		{
			get
			{
				List<Pawn> pawnsListForReading = this.caravan.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					Pawn pawn = pawnsListForReading[i];
					if (this.caravan.IsOwner(pawn) && pawn.TraderKind != null)
					{
						return pawn.TraderKind;
					}
				}
				return null;
			}
		}

		// Token: 0x1700356A RID: 13674
		// (get) Token: 0x060157B2 RID: 87986 RVA: 0x0066FC94 File Offset: 0x0066DE94
		public IEnumerable<Thing> Goods
		{
			get
			{
				List<Thing> inv = CaravanInventoryUtility.AllInventoryItems(this.caravan);
				int num;
				for (int i = 0; i < inv.Count; i = num + 1)
				{
					yield return inv[i];
					num = i;
				}
				List<Pawn> pawns = this.caravan.PawnsListForReading;
				for (int i = 0; i < pawns.Count; i = num + 1)
				{
					Pawn pawn = pawns[i];
					if (!this.caravan.IsOwner(pawn) && (!pawn.RaceProps.packAnimal || pawn.inventory == null || pawn.inventory.innerContainer.Count <= 0) && !this.soldPrisoners.Contains(pawn))
					{
						yield return pawn;
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x1700356B RID: 13675
		// (get) Token: 0x060157B3 RID: 87987 RVA: 0x0066FCA4 File Offset: 0x0066DEA4
		public int RandomPriceFactorSeed
		{
			get
			{
				return Gen.HashCombineInt(this.caravan.ID, 1048142365);
			}
		}

		// Token: 0x1700356C RID: 13676
		// (get) Token: 0x060157B4 RID: 87988 RVA: 0x0066FCBB File Offset: 0x0066DEBB
		public string TraderName
		{
			get
			{
				return this.caravan.LabelCap;
			}
		}

		// Token: 0x1700356D RID: 13677
		// (get) Token: 0x060157B5 RID: 87989 RVA: 0x0066FCC8 File Offset: 0x0066DEC8
		public bool CanTradeNow
		{
			get
			{
				return this.TraderKind != null && !this.caravan.AllOwnersDowned && this.caravan.Faction != Faction.OfPlayer && this.Goods.Any((Thing x) => this.TraderKind.WillTrade(x.def));
			}
		}

		// Token: 0x060157B6 RID: 87990 RVA: 0x0066FD15 File Offset: 0x0066DF15
		public Caravan_TraderTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x060157B7 RID: 87991 RVA: 0x0066FD30 File Offset: 0x0066DF30
		public void ExposeData()
		{
			Scribe_Collections.Look<Pawn>(ref this.soldPrisoners, "soldPrisoners", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.soldPrisoners.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x060157B8 RID: 87992 RVA: 0x0066FD86 File Offset: 0x0066DF86
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			Caravan playerCaravan = playerNegotiator.GetCaravan();
			foreach (Thing thing in CaravanInventoryUtility.AllInventoryItems(playerCaravan))
			{
				yield return thing;
			}
			List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			List<Pawn> pawns = playerCaravan.PawnsListForReading;
			int num;
			for (int i = 0; i < pawns.Count; i = num + 1)
			{
				if (!playerCaravan.IsOwner(pawns[i]))
				{
					yield return pawns[i];
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x060157B9 RID: 87993 RVA: 0x0066FD98 File Offset: 0x0066DF98
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			if (this.Goods.Contains(toGive))
			{
				Log.Error("Tried to add " + ((toGive != null) ? toGive.ToString() : null) + " to stock (pawn's trader tracker), but it's already here.");
				return;
			}
			Caravan caravan = playerNegotiator.GetCaravan();
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerSells, playerNegotiator, this.caravan);
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawn, caravan.PawnsListForReading, null);
				this.caravan.AddPawn(pawn, false);
				if (pawn.IsWorldPawn() && !this.caravan.Spawned)
				{
					Find.WorldPawns.RemovePawn(pawn);
				}
				if (pawn.RaceProps.Humanlike)
				{
					this.soldPrisoners.Add(pawn);
					return;
				}
			}
			else
			{
				Pawn pawn2 = CaravanInventoryUtility.FindPawnToMoveInventoryTo(thing, this.caravan.PawnsListForReading, null, null);
				if (pawn2 == null)
				{
					string str = "Could not find pawn to move sold thing to (sold by player). thing=";
					Thing thing2 = thing;
					Log.Error(str + ((thing2 != null) ? thing2.ToString() : null));
					thing.Destroy(DestroyMode.Vanish);
					return;
				}
				if (!pawn2.inventory.innerContainer.TryAdd(thing, true))
				{
					Log.Error("Could not add item to inventory.");
					thing.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x060157BA RID: 87994 RVA: 0x0066FEB0 File Offset: 0x0066E0B0
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			Caravan caravan = playerNegotiator.GetCaravan();
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this.caravan);
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawn, this.caravan.PawnsListForReading, null);
				caravan.AddPawn(pawn, true);
				if (!pawn.IsWorldPawn() && caravan.Spawned)
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
				}
				this.soldPrisoners.Remove(pawn);
				return;
			}
			Pawn pawn2 = CaravanInventoryUtility.FindPawnToMoveInventoryTo(thing, caravan.PawnsListForReading, null, null);
			if (pawn2 == null)
			{
				string str = "Could not find pawn to move bought thing to (bought by player). thing=";
				Thing thing2 = thing;
				Log.Error(str + ((thing2 != null) ? thing2.ToString() : null));
				thing.Destroy(DestroyMode.Vanish);
				return;
			}
			if (!pawn2.inventory.innerContainer.TryAdd(thing, true))
			{
				Log.Error("Could not add item to inventory.");
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0400EF0A RID: 61194
		private Caravan caravan;

		// Token: 0x0400EF0B RID: 61195
		private List<Pawn> soldPrisoners = new List<Pawn>();
	}
}
