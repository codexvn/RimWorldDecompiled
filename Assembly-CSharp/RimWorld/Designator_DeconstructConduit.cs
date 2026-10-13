using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003091 RID: 12433
	public class Designator_DeconstructConduit : Designator_Deconstruct
	{
		// Token: 0x06011728 RID: 71464 RVA: 0x0050348C File Offset: 0x0050168C
		public Designator_DeconstructConduit()
		{
			this.defaultLabel = "DesignatorDeconstructConduit".Translate();
			this.defaultDesc = "DesignatorDeconstructConduitDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/DeconstructConduit", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Deconstruct;
			this.hotKey = KeyBindingDefOf.Designator_Deconstruct;
		}

		// Token: 0x06011729 RID: 71465 RVA: 0x0050350D File Offset: 0x0050170D
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (t.def.building == null || !t.def.building.isPowerConduit)
			{
				return false;
			}
			return base.CanDesignateThing(t);
		}

		// Token: 0x0601172A RID: 71466 RVA: 0x0050353C File Offset: 0x0050173C
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			OverlayDrawHandler.DrawPowerGridOverlayThisFrame();
		}
	}
}
