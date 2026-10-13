using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C96 RID: 15510
	public class WorldDrawLayer_SelectedTile : WorldDrawLayer_SingleTile
	{
		// Token: 0x1700347B RID: 13435
		// (get) Token: 0x06015138 RID: 86328 RVA: 0x0064EDDB File Offset: 0x0064CFDB
		protected override PlanetTile Tile
		{
			get
			{
				return Find.WorldSelector.SelectedTile;
			}
		}

		// Token: 0x1700347C RID: 13436
		// (get) Token: 0x06015139 RID: 86329 RVA: 0x0064EDE7 File Offset: 0x0064CFE7
		protected override Material Material
		{
			get
			{
				return WorldMaterials.SelectedTile;
			}
		}

		// Token: 0x1700347D RID: 13437
		// (get) Token: 0x0601513A RID: 86330 RVA: 0x00002501 File Offset: 0x00000701
		public override bool VisibleWhenLayerNotSelected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700347E RID: 13438
		// (get) Token: 0x0601513B RID: 86331 RVA: 0x00002501 File Offset: 0x00000701
		public override bool VisibleInBackground
		{
			get
			{
				return false;
			}
		}
	}
}
