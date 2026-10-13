using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020033EE RID: 13294
	public class MainButtonWorker_ToggleMechTab : MainButtonWorker_ToggleTab
	{
		// Token: 0x170030F7 RID: 12535
		// (get) Token: 0x06012B25 RID: 76581 RVA: 0x005890EC File Offset: 0x005872EC
		public override bool Disabled
		{
			get
			{
				if (base.Disabled)
				{
					return true;
				}
				Map currentMap = Find.CurrentMap;
				int num = (currentMap != null) ? currentMap.uniqueID : int.MinValue;
				if ((long)GenTicks.TicksGame - this.lastDisabledCheckTick < 180L && this.lastDisabledCheckMapId == num)
				{
					return this.lastDisabled;
				}
				this.lastDisabledCheckMapId = num;
				this.lastDisabledCheckTick = (long)GenTicks.TicksGame;
				if (currentMap != null)
				{
					List<Pawn> list = currentMap.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].RaceProps.IsMechanoid)
						{
							this.lastDisabled = false;
							return false;
						}
					}
					List<Pawn> list2 = currentMap.mapPawns.PawnsInFaction(Faction.OfPlayer);
					for (int j = 0; j < list2.Count; j++)
					{
						if (list2[j].RaceProps.IsMechanoid)
						{
							this.lastDisabled = false;
							return false;
						}
					}
				}
				this.lastDisabled = true;
				return true;
			}
		}

		// Token: 0x170030F8 RID: 12536
		// (get) Token: 0x06012B26 RID: 76582 RVA: 0x005891E6 File Offset: 0x005873E6
		public override bool Visible
		{
			get
			{
				return !this.Disabled;
			}
		}

		// Token: 0x0400C228 RID: 49704
		private const long DisabledCheckInterval = 180L;

		// Token: 0x0400C229 RID: 49705
		private long lastDisabledCheckTick = -10000000L;

		// Token: 0x0400C22A RID: 49706
		private int lastDisabledCheckMapId = int.MinValue;

		// Token: 0x0400C22B RID: 49707
		private bool lastDisabled;
	}
}
