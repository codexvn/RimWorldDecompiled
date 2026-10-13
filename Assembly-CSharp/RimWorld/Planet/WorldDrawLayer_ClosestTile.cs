using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C79 RID: 15481
	public class WorldDrawLayer_ClosestTile : WorldDrawLayer_SingleTile
	{
		// Token: 0x17003448 RID: 13384
		// (get) Token: 0x0601507E RID: 86142 RVA: 0x0064B9E3 File Offset: 0x00649BE3
		protected override PlanetTile Tile
		{
			get
			{
				if (Find.WorldTargeter.IsTargeting)
				{
					return Find.WorldTargeter.ClosestLayerTile;
				}
				return Find.TilePicker.ClosestLayerTile;
			}
		}

		// Token: 0x17003449 RID: 13385
		// (get) Token: 0x0601507F RID: 86143 RVA: 0x0064BA06 File Offset: 0x00649C06
		protected override Material Material
		{
			get
			{
				return WorldMaterials.ClosestTile;
			}
		}

		// Token: 0x1700344A RID: 13386
		// (get) Token: 0x06015080 RID: 86144 RVA: 0x00002501 File Offset: 0x00000701
		public override bool VisibleWhenLayerNotSelected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700344B RID: 13387
		// (get) Token: 0x06015081 RID: 86145 RVA: 0x00002501 File Offset: 0x00000701
		public override bool VisibleInBackground
		{
			get
			{
				return false;
			}
		}
	}
}
