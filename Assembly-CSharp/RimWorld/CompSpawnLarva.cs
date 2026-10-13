using System;

namespace RimWorld
{
	// Token: 0x02002E40 RID: 11840
	public class CompSpawnLarva : CompSpawnPawnOnDestroyed
	{
		// Token: 0x17002AB7 RID: 10935
		// (get) Token: 0x06010AB8 RID: 68280 RVA: 0x004CE46F File Offset: 0x004CC66F
		protected override bool JoinLord
		{
			get
			{
				return this.parent.Faction != Faction.OfPlayer;
			}
		}
	}
}
