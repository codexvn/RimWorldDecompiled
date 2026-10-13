using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020031BF RID: 12735
	public class WorldSearchElement
	{
		// Token: 0x17002E8B RID: 11915
		// (get) Token: 0x06011ECC RID: 73420 RVA: 0x00537B70 File Offset: 0x00535D70
		public string DisplayLabel
		{
			get
			{
				if (this.worldObject != null)
				{
					return this.worldObject.LabelCap;
				}
				if (ModsConfig.OdysseyActive)
				{
					if (this.landmark != null)
					{
						return this.landmark.name.CapitalizeFirst();
					}
					if (!this.mutators.NullOrEmpty<TileMutatorDef>())
					{
						return this.mutators[0].label.CapitalizeFirst();
					}
				}
				return null;
			}
		}

		// Token: 0x0400B991 RID: 47505
		public PlanetTile tile = PlanetTile.Invalid;

		// Token: 0x0400B992 RID: 47506
		public WorldObject worldObject;

		// Token: 0x0400B993 RID: 47507
		public Landmark landmark;

		// Token: 0x0400B994 RID: 47508
		public List<TileMutatorDef> mutators;
	}
}
