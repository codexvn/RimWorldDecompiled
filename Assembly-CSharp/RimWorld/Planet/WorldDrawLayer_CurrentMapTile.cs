using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C7C RID: 15484
	public class WorldDrawLayer_CurrentMapTile : WorldDrawLayer_SingleTile
	{
		// Token: 0x1700344E RID: 13390
		// (get) Token: 0x06015091 RID: 86161 RVA: 0x0064BD9F File Offset: 0x00649F9F
		protected override PlanetTile Tile
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing)
				{
					return PlanetTile.Invalid;
				}
				if (Find.CurrentMap == null)
				{
					return PlanetTile.Invalid;
				}
				return Find.CurrentMap.Tile;
			}
		}

		// Token: 0x1700344F RID: 13391
		// (get) Token: 0x06015092 RID: 86162 RVA: 0x0064BDC6 File Offset: 0x00649FC6
		protected override Material Material
		{
			get
			{
				return WorldMaterials.CurrentMapTile;
			}
		}

		// Token: 0x17003450 RID: 13392
		// (get) Token: 0x06015093 RID: 86163 RVA: 0x00002501 File Offset: 0x00000701
		public override bool VisibleWhenLayerNotSelected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17003451 RID: 13393
		// (get) Token: 0x06015094 RID: 86164 RVA: 0x00002501 File Offset: 0x00000701
		public override bool VisibleInBackground
		{
			get
			{
				return false;
			}
		}
	}
}
