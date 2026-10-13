using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C97 RID: 15511
	public class WorldDrawLayer_SettleTile : WorldDrawLayer_SingleTile
	{
		// Token: 0x1700347F RID: 13439
		// (get) Token: 0x0601513D RID: 86333 RVA: 0x0064EDF0 File Offset: 0x0064CFF0
		protected override PlanetTile Tile
		{
			get
			{
				if (!(WorldGizmoUtility.LastMouseOverGizmo is Command_Settle))
				{
					return PlanetTile.Invalid;
				}
				Caravan caravan = Find.WorldSelector.SingleSelectedObject as Caravan;
				if (caravan == null)
				{
					return PlanetTile.Invalid;
				}
				return caravan.Tile;
			}
		}

		// Token: 0x17003480 RID: 13440
		// (get) Token: 0x0601513E RID: 86334 RVA: 0x0064BDC6 File Offset: 0x00649FC6
		protected override Material Material
		{
			get
			{
				return WorldMaterials.CurrentMapTile;
			}
		}

		// Token: 0x17003481 RID: 13441
		// (get) Token: 0x0601513F RID: 86335 RVA: 0x0064EE2E File Offset: 0x0064D02E
		protected override float Alpha
		{
			get
			{
				return Mathf.Abs(Time.time % 2f - 1f);
			}
		}
	}
}
