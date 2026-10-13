using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C93 RID: 7315
	public class QuestPart_RequirementsToAcceptPlanetLayer : QuestPart_RequirementsToAccept
	{
		// Token: 0x1700190E RID: 6414
		// (get) Token: 0x0600A2FC RID: 41724 RVA: 0x00002501 File Offset: 0x00000701
		public override bool ShowInRequirementBox
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700190F RID: 6415
		// (get) Token: 0x0600A2FD RID: 41725 RVA: 0x002ED2B7 File Offset: 0x002EB4B7
		public Map TargetMap
		{
			get
			{
				if (this.mapParent != null && this.mapParent.HasMap)
				{
					return this.mapParent.Map;
				}
				Pawn pawn = this.mapPawn;
				if (pawn == null)
				{
					return null;
				}
				return pawn.MapHeld;
			}
		}

		// Token: 0x0600A2FE RID: 41726 RVA: 0x002ED2EC File Offset: 0x002EB4EC
		public override AcceptanceReport CanAccept()
		{
			if (this.TargetMap == null || !this.TargetMap.Tile.Valid)
			{
				if (this.canBeSpace)
				{
					return true;
				}
				foreach (Map map in Current.Game.PlayerHomeMaps)
				{
					if (map.Tile.Valid && !map.Tile.LayerDef.isSpace)
					{
						return true;
					}
				}
				return "QuestNotSpace".Translate().CapitalizeFirst().Resolve();
			}
			else
			{
				PlanetLayerDef layerDef = this.TargetMap.Tile.LayerDef;
				if (!this.canBeSpace && layerDef.isSpace)
				{
					return QuestPart_RequirementsToAcceptPlanetLayer.GetReport(layerDef);
				}
				if (!this.layerWhitelist.NullOrEmpty<PlanetLayerDef>() && !this.layerWhitelist.Contains(layerDef))
				{
					return QuestPart_RequirementsToAcceptPlanetLayer.GetReport(layerDef);
				}
				if (!this.layerBlacklist.NullOrEmpty<PlanetLayerDef>() && this.layerBlacklist.Contains(layerDef))
				{
					return QuestPart_RequirementsToAcceptPlanetLayer.GetReport(layerDef);
				}
				return true;
			}
		}

		// Token: 0x0600A2FF RID: 41727 RVA: 0x002ED434 File Offset: 0x002EB634
		private static AcceptanceReport GetReport(PlanetLayerDef def)
		{
			return "QuestRequiredLayer".Translate(def.gerundLabel.Named("GERUND"), def.label.Named("LAYER")).CapitalizeFirst().Resolve();
		}

		// Token: 0x0600A300 RID: 41728 RVA: 0x002ED480 File Offset: 0x002EB680
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<Pawn>(ref this.mapPawn, "mapPawn", false);
			Scribe_Values.Look<bool>(ref this.canBeSpace, "canBeSpace", false, false);
			Scribe_Collections.Look<PlanetLayerDef>(ref this.layerWhitelist, "layerWhitelist", LookMode.Undefined, Array.Empty<object>());
			Scribe_Collections.Look<PlanetLayerDef>(ref this.layerBlacklist, "layerBlacklist", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x040070AE RID: 28846
		public MapParent mapParent;

		// Token: 0x040070AF RID: 28847
		public Pawn mapPawn;

		// Token: 0x040070B0 RID: 28848
		public bool canBeSpace;

		// Token: 0x040070B1 RID: 28849
		public List<PlanetLayerDef> layerWhitelist;

		// Token: 0x040070B2 RID: 28850
		public List<PlanetLayerDef> layerBlacklist;
	}
}
