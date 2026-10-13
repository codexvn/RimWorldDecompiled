using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200309C RID: 12444
	[StaticConstructorOnStartup]
	public class Designator_Eyedropper : Designator
	{
		// Token: 0x06011772 RID: 71538 RVA: 0x005044C4 File Offset: 0x005026C4
		public Designator_Eyedropper(Action<ColorDef> selectAction, string rejectMessage, string desc)
		{
			this.selectAction = selectAction;
			this.rejectMessage = rejectMessage;
			this.defaultLabel = "DesignatorEyedropper".Translate();
			this.defaultDesc = desc;
			this.icon = Designator_Eyedropper.EyeDropperTex;
			this.useMouseIcon = true;
		}

		// Token: 0x06011773 RID: 71539 RVA: 0x00504513 File Offset: 0x00502713
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (this.ColorDefAt(c) != null)
			{
				return AcceptanceReport.WasAccepted;
			}
			if (!this.rejectMessage.NullOrEmpty())
			{
				return this.rejectMessage;
			}
			return false;
		}

		// Token: 0x06011774 RID: 71540 RVA: 0x00504544 File Offset: 0x00502744
		public override void DesignateSingleCell(IntVec3 cell)
		{
			ColorDef colorDef = this.ColorDefAt(cell);
			if (colorDef != null)
			{
				Action<ColorDef> action = this.selectAction;
				if (action != null)
				{
					action(colorDef);
				}
				Messages.Message("GrabbedColor".Translate() + ": " + colorDef.LabelCap, null, MessageTypeDefOf.NeutralEvent, false);
				return;
			}
			if (!this.rejectMessage.NullOrEmpty())
			{
				Messages.Message(this.rejectMessage, null, MessageTypeDefOf.RejectInput, false);
			}
		}

		// Token: 0x06011775 RID: 71541 RVA: 0x005045C0 File Offset: 0x005027C0
		protected virtual ColorDef ColorDefAt(IntVec3 cell)
		{
			if (!cell.InBounds(base.Map) || cell.Fogged(base.Map))
			{
				return null;
			}
			List<Thing> thingList = cell.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Building building = thingList[i] as Building;
				if (building != null && building.def.building.paintable && building.PaintColorDef != null)
				{
					return building.PaintColorDef;
				}
			}
			return base.Map.terrainGrid.ColorAt(cell) ?? cell.GetTerrain(base.Map).colorDef;
		}

		// Token: 0x06011776 RID: 71542 RVA: 0x00504660 File Offset: 0x00502860
		public override void DrawMouseAttachments()
		{
			if (this.useMouseIcon)
			{
				string text = string.Empty;
				ColorDef colorDef = this.ColorDefAt(UI.MouseCell());
				if (colorDef != null)
				{
					text = "Grab".Translate() + ": " + colorDef.LabelCap;
				}
				else if (!this.rejectMessage.NullOrEmpty())
				{
					text = this.rejectMessage;
				}
				GenUI.DrawMouseAttachment(this.icon, text, this.iconAngle, this.iconOffset, null, null, false, default(Color), null, null);
			}
		}

		// Token: 0x0400B4AD RID: 46253
		private Action<ColorDef> selectAction;

		// Token: 0x0400B4AE RID: 46254
		private string rejectMessage;

		// Token: 0x0400B4AF RID: 46255
		public static readonly Texture2D EyeDropperTex = ContentFinder<Texture2D>.Get("UI/Icons/Eyedropper", true);
	}
}
