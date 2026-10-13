using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DA7 RID: 15783
	public class EscapeShip : MapParent
	{
		// Token: 0x17003579 RID: 13689
		// (get) Token: 0x0601580C RID: 88076 RVA: 0x006714DF File Offset: 0x0066F6DF
		public override Texture2D ExpandingIcon
		{
			get
			{
				if (!base.HasMap || base.Faction == null)
				{
					return base.ExpandingIcon;
				}
				return base.Faction.def.FactionIcon;
			}
		}

		// Token: 0x1700357A RID: 13690
		// (get) Token: 0x0601580D RID: 88077 RVA: 0x00671508 File Offset: 0x0066F708
		public override Color ExpandingIconColor
		{
			get
			{
				if (!base.HasMap || base.Faction == null)
				{
					return base.ExpandingIconColor;
				}
				return base.Faction.Color;
			}
		}

		// Token: 0x1700357B RID: 13691
		// (get) Token: 0x0601580E RID: 88078 RVA: 0x0067152C File Offset: 0x0066F72C
		public override Material Material
		{
			get
			{
				if (!base.HasMap || base.Faction == null)
				{
					return base.Material;
				}
				if (this.cachedPostGenerateMat == null)
				{
					this.cachedPostGenerateMat = MaterialPool.MatFrom(base.Faction.def.settlementTexturePath, ShaderDatabase.WorldOverlayTransparentLit, base.Faction.Color, 3550);
				}
				return this.cachedPostGenerateMat;
			}
		}

		// Token: 0x0601580F RID: 88079 RVA: 0x00671594 File Offset: 0x0066F794
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			Find.World.renderer.SetDirty<WorldDrawLayer_WorldObjects>(base.Tile.Layer);
		}

		// Token: 0x0400EF3C RID: 61244
		private Material cachedPostGenerateMat;
	}
}
