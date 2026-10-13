using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EA1 RID: 7841
	public sealed class FertilityGrid
	{
		// Token: 0x17001AA1 RID: 6817
		// (get) Token: 0x0600ADFE RID: 44542 RVA: 0x00319D6C File Offset: 0x00317F6C
		public CellBoolDrawer Drawer
		{
			get
			{
				if (this.drawerInt == null)
				{
					this.drawerInt = new CellBoolDrawer(new Func<int, bool>(this.CellBoolDrawerGetBoolInt), new Func<Color>(this.CellBoolDrawerColorInt), new Func<int, Color>(this.CellBoolDrawerGetExtraColorInt), this.map.Size.x, this.map.Size.z, 3610, 0.33f);
				}
				return this.drawerInt;
			}
		}

		// Token: 0x0600ADFF RID: 44543 RVA: 0x00319DE0 File Offset: 0x00317FE0
		public FertilityGrid(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600AE00 RID: 44544 RVA: 0x00319DEF File Offset: 0x00317FEF
		public float FertilityAt(IntVec3 loc)
		{
			return this.CalculateFertilityAt(loc);
		}

		// Token: 0x0600AE01 RID: 44545 RVA: 0x00319DF8 File Offset: 0x00317FF8
		private float CalculateFertilityAt(IntVec3 loc)
		{
			Thing edifice = loc.GetEdifice(this.map);
			if (edifice != null && edifice.def.AffectsFertility)
			{
				return edifice.def.fertility;
			}
			float num = this.map.terrainGrid.TerrainAt(loc).fertility;
			if (ModsConfig.BiotechActive && this.map.pollutionGrid.IsPolluted(loc))
			{
				num = Mathf.Min(num, 0.5f);
			}
			return num;
		}

		// Token: 0x0600AE02 RID: 44546 RVA: 0x00319E6C File Offset: 0x0031806C
		public void FertilityGridUpdate()
		{
			if (Find.PlaySettings.showFertilityOverlay && !Find.ScreenshotModeHandler.Active)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x0600AE03 RID: 44547 RVA: 0x000440FD File Offset: 0x000422FD
		private Color CellBoolDrawerColorInt()
		{
			return Color.white;
		}

		// Token: 0x0600AE04 RID: 44548 RVA: 0x00319E9C File Offset: 0x0031809C
		private bool CellBoolDrawerGetBoolInt(int index)
		{
			IntVec3 intVec = CellIndicesUtility.IndexToCell(index, this.map.Size.x);
			if (intVec.Filled(this.map) || intVec.Fogged(this.map))
			{
				return false;
			}
			float num = this.FertilityAt(intVec);
			if (ModsConfig.BiotechActive)
			{
				return num >= 0.5f;
			}
			return num > 0.69f;
		}

		// Token: 0x0600AE05 RID: 44549 RVA: 0x00319F04 File Offset: 0x00318104
		private Color CellBoolDrawerGetExtraColorInt(int index)
		{
			float num = this.FertilityAt(CellIndicesUtility.IndexToCell(index, this.map.Size.x));
			if (ModsConfig.BiotechActive && num <= 0.5f)
			{
				return FertilityGrid.ExtraLowFertilityColor;
			}
			if (num <= 0.95f)
			{
				return FertilityGrid.LowFertilityColor;
			}
			if (num <= 1.1f)
			{
				return FertilityGrid.MediumFertilityColor;
			}
			if (num >= 1.1f)
			{
				return FertilityGrid.HighFertilityColor;
			}
			return Color.white;
		}

		// Token: 0x040077AC RID: 30636
		public const float MaxPollutedFertility = 0.5f;

		// Token: 0x040077AD RID: 30637
		private Map map;

		// Token: 0x040077AE RID: 30638
		private CellBoolDrawer drawerInt;

		// Token: 0x040077AF RID: 30639
		private static readonly Color ExtraLowFertilityColor = Color.red;

		// Token: 0x040077B0 RID: 30640
		private static readonly Color LowFertilityColor = Color.yellow;

		// Token: 0x040077B1 RID: 30641
		private static readonly Color MediumFertilityColor = new Color(0.59f, 0.98f, 0.59f, 1f);

		// Token: 0x040077B2 RID: 30642
		private static readonly Color HighFertilityColor = Color.green;
	}
}
