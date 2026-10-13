using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C85 RID: 15493
	public class WorldDrawLayer_MouseTile : WorldDrawLayer_SingleTile
	{
		// Token: 0x17003460 RID: 13408
		// (get) Token: 0x060150D0 RID: 86224 RVA: 0x0064CC24 File Offset: 0x0064AE24
		protected override PlanetTile Tile
		{
			get
			{
				if (Find.World.UI.selector.dragBox.IsValidAndActive)
				{
					return PlanetTile.Invalid;
				}
				if (Find.WorldTargeter.IsTargeting)
				{
					return PlanetTile.Invalid;
				}
				if (Find.ScreenshotModeHandler.Active)
				{
					return PlanetTile.Invalid;
				}
				return GenWorld.MouseTile(false);
			}
		}

		// Token: 0x17003461 RID: 13409
		// (get) Token: 0x060150D1 RID: 86225 RVA: 0x0064CC7C File Offset: 0x0064AE7C
		protected override Material Material
		{
			get
			{
				return WorldMaterials.MouseTile;
			}
		}

		// Token: 0x17003462 RID: 13410
		// (get) Token: 0x060150D2 RID: 86226 RVA: 0x00002501 File Offset: 0x00000701
		public override bool VisibleWhenLayerNotSelected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17003463 RID: 13411
		// (get) Token: 0x060150D3 RID: 86227 RVA: 0x00002501 File Offset: 0x00000701
		public override bool VisibleInBackground
		{
			get
			{
				return false;
			}
		}
	}
}
