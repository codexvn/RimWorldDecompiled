using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F75 RID: 12149
	public sealed class PassingShipManager : IExposable
	{
		// Token: 0x060111D2 RID: 70098 RVA: 0x004E9471 File Offset: 0x004E7671
		public PassingShipManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x060111D3 RID: 70099 RVA: 0x004E948C File Offset: 0x004E768C
		public void ExposeData()
		{
			Scribe_Collections.Look<PassingShip>(ref this.passingShips, "passingShips", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				for (int i = 0; i < this.passingShips.Count; i++)
				{
					this.passingShips[i].passingShipManager = this;
				}
			}
		}

		// Token: 0x060111D4 RID: 70100 RVA: 0x004E94DF File Offset: 0x004E76DF
		public void AddShip(PassingShip vis)
		{
			this.passingShips.Add(vis);
			vis.passingShipManager = this;
		}

		// Token: 0x060111D5 RID: 70101 RVA: 0x004E94F4 File Offset: 0x004E76F4
		public void RemoveShip(PassingShip vis)
		{
			this.passingShips.Remove(vis);
			vis.passingShipManager = null;
		}

		// Token: 0x060111D6 RID: 70102 RVA: 0x004E950C File Offset: 0x004E770C
		public void PassingShipManagerTick()
		{
			for (int i = this.passingShips.Count - 1; i >= 0; i--)
			{
				this.passingShips[i].PassingShipTick();
			}
		}

		// Token: 0x060111D7 RID: 70103 RVA: 0x004E9544 File Offset: 0x004E7744
		public void RemoveAllShipsOfFaction(Faction faction)
		{
			for (int i = this.passingShips.Count - 1; i >= 0; i--)
			{
				if (this.passingShips[i].Faction == faction)
				{
					this.passingShips[i].Depart();
				}
			}
		}

		// Token: 0x060111D8 RID: 70104 RVA: 0x004E9590 File Offset: 0x004E7790
		internal void DebugSendAllShipsAway()
		{
			PassingShipManager.tmpPassingShips.Clear();
			PassingShipManager.tmpPassingShips.AddRange(this.passingShips);
			for (int i = 0; i < PassingShipManager.tmpPassingShips.Count; i++)
			{
				PassingShipManager.tmpPassingShips[i].Depart();
			}
			Messages.Message("All passing ships sent away.", MessageTypeDefOf.TaskCompletion, false);
		}

		// Token: 0x0400B192 RID: 45458
		public Map map;

		// Token: 0x0400B193 RID: 45459
		public List<PassingShip> passingShips = new List<PassingShip>();

		// Token: 0x0400B194 RID: 45460
		private static List<PassingShip> tmpPassingShips = new List<PassingShip>();
	}
}
