using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02003387 RID: 13191
	public abstract class ITab : InspectTabBase
	{
		// Token: 0x17003070 RID: 12400
		// (get) Token: 0x060128B7 RID: 75959 RVA: 0x00578FB0 File Offset: 0x005771B0
		protected object SelObject
		{
			get
			{
				return Find.Selector.SingleSelectedObject;
			}
		}

		// Token: 0x17003071 RID: 12401
		// (get) Token: 0x060128B8 RID: 75960 RVA: 0x00578FBC File Offset: 0x005771BC
		protected List<object> AllSelObjects
		{
			get
			{
				return Find.Selector.SelectedObjects;
			}
		}

		// Token: 0x17003072 RID: 12402
		// (get) Token: 0x060128B9 RID: 75961 RVA: 0x00578FC8 File Offset: 0x005771C8
		protected Thing SelThing
		{
			get
			{
				return Find.Selector.SingleSelectedThing;
			}
		}

		// Token: 0x17003073 RID: 12403
		// (get) Token: 0x060128BA RID: 75962 RVA: 0x00578FD4 File Offset: 0x005771D4
		protected virtual Pawn SelPawn
		{
			get
			{
				return this.SelThing as Pawn;
			}
		}

		// Token: 0x17003074 RID: 12404
		// (get) Token: 0x060128BB RID: 75963 RVA: 0x00578FE1 File Offset: 0x005771E1
		private MainTabWindow_Inspect InspectPane
		{
			get
			{
				return (MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow;
			}
		}

		// Token: 0x17003075 RID: 12405
		// (get) Token: 0x060128BC RID: 75964 RVA: 0x00578FF4 File Offset: 0x005771F4
		protected override bool StillValid
		{
			get
			{
				if (Find.MainTabsRoot.OpenTab != MainButtonDefOf.Inspect)
				{
					return false;
				}
				MainTabWindow_Inspect mainTabWindow_Inspect = (MainTabWindow_Inspect)Find.MainTabsRoot.OpenTab.TabWindow;
				return mainTabWindow_Inspect.CurTabs != null && mainTabWindow_Inspect.CurTabs.Contains(this);
			}
		}

		// Token: 0x17003076 RID: 12406
		// (get) Token: 0x060128BD RID: 75965 RVA: 0x0057903F File Offset: 0x0057723F
		protected override float PaneTopY
		{
			get
			{
				return this.InspectPane.PaneTopY;
			}
		}

		// Token: 0x060128BE RID: 75966 RVA: 0x0057904C File Offset: 0x0057724C
		protected override void CloseTab()
		{
			this.InspectPane.CloseOpenTab();
			SoundDefOf.TabClose.PlayOneShotOnCamera(null);
		}
	}
}
