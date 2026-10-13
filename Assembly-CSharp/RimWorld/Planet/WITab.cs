using System;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E5E RID: 15966
	public abstract class WITab : InspectTabBase
	{
		// Token: 0x17003690 RID: 13968
		// (get) Token: 0x06015D61 RID: 89441 RVA: 0x0068706F File Offset: 0x0068526F
		protected WorldObject SelObject
		{
			get
			{
				return Find.WorldSelector.SingleSelectedObject;
			}
		}

		// Token: 0x17003691 RID: 13969
		// (get) Token: 0x06015D62 RID: 89442 RVA: 0x0068707B File Offset: 0x0068527B
		protected PlanetTile SelPlanetTile
		{
			get
			{
				WorldObject selObject = this.SelObject;
				if (selObject == null)
				{
					return Find.WorldSelector.SelectedTile;
				}
				return selObject.Tile;
			}
		}

		// Token: 0x17003692 RID: 13970
		// (get) Token: 0x06015D63 RID: 89443 RVA: 0x00687097 File Offset: 0x00685297
		protected Tile SelTile
		{
			get
			{
				return Find.WorldGrid[this.SelPlanetTile];
			}
		}

		// Token: 0x17003693 RID: 13971
		// (get) Token: 0x06015D64 RID: 89444 RVA: 0x006870A9 File Offset: 0x006852A9
		protected Caravan SelCaravan
		{
			get
			{
				return this.SelObject as Caravan;
			}
		}

		// Token: 0x17003694 RID: 13972
		// (get) Token: 0x06015D65 RID: 89445 RVA: 0x006870B6 File Offset: 0x006852B6
		private WorldInspectPane InspectPane
		{
			get
			{
				return Find.World.UI.inspectPane;
			}
		}

		// Token: 0x17003695 RID: 13973
		// (get) Token: 0x06015D66 RID: 89446 RVA: 0x006870C7 File Offset: 0x006852C7
		protected override bool StillValid
		{
			get
			{
				return WorldRendererUtility.WorldSelected && Find.WindowStack.IsOpen<WorldInspectPane>() && this.InspectPane.CurTabs != null && this.InspectPane.CurTabs.Contains(this);
			}
		}

		// Token: 0x17003696 RID: 13974
		// (get) Token: 0x06015D67 RID: 89447 RVA: 0x00687100 File Offset: 0x00685300
		protected override float PaneTopY
		{
			get
			{
				return this.InspectPane.PaneTopY;
			}
		}

		// Token: 0x06015D68 RID: 89448 RVA: 0x0068710D File Offset: 0x0068530D
		protected override void CloseTab()
		{
			this.InspectPane.CloseOpenTab();
		}
	}
}
