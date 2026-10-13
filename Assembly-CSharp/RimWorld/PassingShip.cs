using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F72 RID: 12146
	public class PassingShip : IExposable, ICommunicable, ILoadReferenceable
	{
		// Token: 0x17002C47 RID: 11335
		// (get) Token: 0x060111BC RID: 70076 RVA: 0x004E9185 File Offset: 0x004E7385
		public virtual string FullTitle
		{
			get
			{
				return "ErrorFullTitle";
			}
		}

		// Token: 0x17002C48 RID: 11336
		// (get) Token: 0x060111BD RID: 70077 RVA: 0x004E918C File Offset: 0x004E738C
		public bool Departed
		{
			get
			{
				return this.ticksUntilDeparture <= 0;
			}
		}

		// Token: 0x17002C49 RID: 11337
		// (get) Token: 0x060111BE RID: 70078 RVA: 0x004E919A File Offset: 0x004E739A
		public Map Map
		{
			get
			{
				PassingShipManager passingShipManager = this.passingShipManager;
				if (passingShipManager == null)
				{
					return null;
				}
				return passingShipManager.map;
			}
		}

		// Token: 0x17002C4A RID: 11338
		// (get) Token: 0x060111BF RID: 70079 RVA: 0x004E91AD File Offset: 0x004E73AD
		public Faction Faction
		{
			get
			{
				return this.faction;
			}
		}

		// Token: 0x060111C0 RID: 70080 RVA: 0x004E91B5 File Offset: 0x004E73B5
		public PassingShip()
		{
		}

		// Token: 0x060111C1 RID: 70081 RVA: 0x004E91DA File Offset: 0x004E73DA
		public PassingShip(Faction faction)
		{
			this.faction = faction;
		}

		// Token: 0x060111C2 RID: 70082 RVA: 0x004E9208 File Offset: 0x004E7408
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<int>(ref this.ticksUntilDeparture, "ticksUntilDeparture", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x060111C3 RID: 70083 RVA: 0x004E925C File Offset: 0x004E745C
		public virtual void PassingShipTick()
		{
			this.ticksUntilDeparture--;
			if (this.Departed)
			{
				this.Depart();
			}
		}

		// Token: 0x060111C4 RID: 70084 RVA: 0x004E927C File Offset: 0x004E747C
		public virtual void Depart()
		{
			if (this.Map.listerBuildings.ColonistsHaveBuilding((Thing b) => b.def.IsCommsConsole))
			{
				Messages.Message("MessageShipHasLeftCommsRange".Translate(this.FullTitle), MessageTypeDefOf.SituationResolved, true);
			}
			this.passingShipManager.RemoveShip(this);
		}

		// Token: 0x060111C5 RID: 70085 RVA: 0x00066B11 File Offset: 0x00064D11
		public virtual void TryOpenComms(Pawn negotiator)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060111C6 RID: 70086 RVA: 0x004E92EB File Offset: 0x004E74EB
		public virtual string GetCallLabel()
		{
			return this.name;
		}

		// Token: 0x060111C7 RID: 70087 RVA: 0x004E92F3 File Offset: 0x004E74F3
		public string GetInfoText()
		{
			return this.FullTitle;
		}

		// Token: 0x060111C8 RID: 70088 RVA: 0x00002C42 File Offset: 0x00000E42
		Faction ICommunicable.GetFaction()
		{
			return null;
		}

		// Token: 0x060111C9 RID: 70089 RVA: 0x000A5EF4 File Offset: 0x000A40F4
		protected virtual AcceptanceReport CanCommunicateWith(Pawn negotiator)
		{
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x060111CA RID: 70090 RVA: 0x004E92FC File Offset: 0x004E74FC
		public FloatMenuOption CommFloatMenuOption(Building_CommsConsole console, Pawn negotiator)
		{
			string label = "CallOnRadio".Translate(this.GetCallLabel());
			Action action = null;
			AcceptanceReport canCommunicate = this.CanCommunicateWith(negotiator);
			if (!canCommunicate.Accepted)
			{
				if (!canCommunicate.Reason.NullOrEmpty())
				{
					action = delegate()
					{
						Messages.Message(canCommunicate.Reason, console, MessageTypeDefOf.RejectInput, false);
					};
				}
			}
			else
			{
				action = delegate()
				{
					if (!Building_OrbitalTradeBeacon.AllPowered(this.Map).Any<Building_OrbitalTradeBeacon>())
					{
						Messages.Message("MessageNeedBeaconToTradeWithShip".Translate(), console, MessageTypeDefOf.RejectInput, false);
						return;
					}
					console.GiveUseCommsJob(negotiator, this);
				};
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action, MenuOptionPriority.InitiateSocial, null, null, 0f, null, null, true, 0), negotiator, console, "ReservedBy", null);
		}

		// Token: 0x060111CB RID: 70091 RVA: 0x004E93BB File Offset: 0x004E75BB
		public string GetUniqueLoadID()
		{
			return "PassingShip_" + this.loadID.ToString();
		}

		// Token: 0x0400B187 RID: 45447
		public PassingShipManager passingShipManager;

		// Token: 0x0400B188 RID: 45448
		private Faction faction;

		// Token: 0x0400B189 RID: 45449
		public string name = "Nameless";

		// Token: 0x0400B18A RID: 45450
		protected int loadID = -1;

		// Token: 0x0400B18B RID: 45451
		public int ticksUntilDeparture = 40000;
	}
}
