using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D17 RID: 15639
	public class Camp : MapParent
	{
		// Token: 0x1700350F RID: 13583
		// (get) Token: 0x06015496 RID: 87190 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool CanReformFoggedEnemies
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17003510 RID: 13584
		// (get) Token: 0x06015497 RID: 87191 RVA: 0x0065E2D0 File Offset: 0x0065C4D0
		public override Material Material
		{
			get
			{
				if (this.cachedMat != null)
				{
					return this.cachedMat;
				}
				Faction faction = base.Faction;
				Color color = (faction != null) ? faction.Color : Color.white;
				this.cachedMat = MaterialPool.MatFrom(this.def.texture, ShaderDatabase.WorldOverlayTransparentLit, color, 3550);
				return this.cachedMat;
			}
		}

		// Token: 0x06015498 RID: 87192 RVA: 0x0065E330 File Offset: 0x0065C530
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (Find.WorldSelector.SingleSelectedObject == this)
			{
				Command command = SettleInExistingMapUtility.SettleCommand(base.Map, true);
				if (!TileFinder.IsValidTileForNewSettlement(base.Tile, Camp.sb, false))
				{
					command.Disable(Camp.sb.ToString());
				}
				yield return command;
			}
			Camp.sb.Clear();
			yield break;
			yield break;
		}

		// Token: 0x06015499 RID: 87193 RVA: 0x0065E340 File Offset: 0x0065C540
		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			if (base.Map.mapPawns.AnyPawnBlockingMapRemoval)
			{
				return false;
			}
			if (TransporterUtility.IncomingTransporterPreventingMapRemoval(base.Map))
			{
				return false;
			}
			alsoRemoveWorldObject = true;
			return true;
		}

		// Token: 0x0601549A RID: 87194 RVA: 0x0065E36C File Offset: 0x0065C56C
		public override void Notify_MyMapRemoved(Map map)
		{
			base.Notify_MyMapRemoved(map);
			if (ModsConfig.OdysseyActive && map.TileInfo.Landmark != null)
			{
				WorldObject worldObject = WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.AbandonedLandmark);
				worldObject.Tile = base.Tile;
				worldObject.SetFaction(base.Faction);
				Find.WorldObjects.Add(worldObject);
				return;
			}
			WorldObject worldObject2 = WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.AbandonedCamp);
			worldObject2.Tile = base.Tile;
			worldObject2.SetFaction(base.Faction);
			worldObject2.GetComponent<TimeoutComp>().StartTimeout(1800000);
			Find.WorldObjects.Add(worldObject2);
		}

		// Token: 0x0400ED58 RID: 60760
		private Material cachedMat;

		// Token: 0x0400ED59 RID: 60761
		private static readonly StringBuilder sb = new StringBuilder();
	}
}
