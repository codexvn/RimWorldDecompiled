using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030AF RID: 12463
	public abstract class Designator_Paint : DesignatorWithEyedropper
	{
		// Token: 0x17002D53 RID: 11603
		// (get) Token: 0x060117FF RID: 71679
		protected abstract Texture2D IconTopTex { get; }

		// Token: 0x17002D54 RID: 11604
		// (get) Token: 0x06011800 RID: 71680 RVA: 0x00506A2F File Offset: 0x00504C2F
		public override Color IconDrawColor
		{
			get
			{
				return this.colorDef.color;
			}
		}

		// Token: 0x17002D55 RID: 11605
		// (get) Token: 0x06011801 RID: 71681 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002D56 RID: 11606
		// (get) Token: 0x06011802 RID: 71682 RVA: 0x00506A3C File Offset: 0x00504C3C
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Paint;
			}
		}

		// Token: 0x17002D57 RID: 11607
		// (get) Token: 0x06011803 RID: 71683 RVA: 0x00506A44 File Offset: 0x00504C44
		protected virtual IEnumerable<ColorDef> Colors
		{
			get
			{
				return from x in DefDatabase<ColorDef>.AllDefs
				where x.colorType == ColorType.Structure
				orderby x.displayOrder
				select x;
			}
		}

		// Token: 0x17002D58 RID: 11608
		// (get) Token: 0x06011804 RID: 71684 RVA: 0x00506AA0 File Offset: 0x00504CA0
		private string AttachmentString
		{
			get
			{
				if (this.cachedAttachmentString == null)
				{
					this.cachedAttachmentString = "Paint".Translate() + ": " + this.colorDef.LabelCap + "\n" + KeyBindingDefOf.ShowEyedropper.MainKeyLabel + ": " + "GrabExistingColor".Translate();
				}
				return this.cachedAttachmentString;
			}
		}

		// Token: 0x06011805 RID: 71685 RVA: 0x00506B1C File Offset: 0x00504D1C
		public Designator_Paint()
		{
			this.colorDef = this.Colors.FirstOrDefault<ColorDef>();
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Paint;
			this.hotKey = KeyBindingDefOf.Misc6;
			this.eyedropper = new Designator_Eyedropper(delegate(ColorDef newCol)
			{
				this.colorDef = newCol;
				this.cachedAttachmentString = null;
				if (!this.eyedropMode)
				{
					Find.DesignatorManager.Select(this);
				}
			}, "SelectAPaintedBuilding".Translate(), "DesignatorEyeDropperDesc_Paint".Translate());
		}

		// Token: 0x06011806 RID: 71686 RVA: 0x00506BA8 File Offset: 0x00504DA8
		public override void ProcessInput(Event ev)
		{
			if (!base.CheckCanInteract())
			{
				return;
			}
			List<FloatMenuGridOption> list = new List<FloatMenuGridOption>();
			List<FloatMenuGridOption> list2 = list;
			Texture2D eyeDropperTex = Designator_Eyedropper.EyeDropperTex;
			Action action = delegate()
			{
				this.<>n__0(ev);
				Find.DesignatorManager.Select(this.eyedropper);
			};
			TipSignal? tooltip = new TipSignal?("DesignatorEyeDropperDesc_Paint".Translate());
			list2.Add(new FloatMenuGridOption(eyeDropperTex, action, null, tooltip));
			foreach (ColorDef newCol2 in this.Colors)
			{
				ColorDef newCol = newCol2;
				list.Add(new FloatMenuGridOption(BaseContent.WhiteTex, delegate()
				{
					this.<>n__0(ev);
					Find.DesignatorManager.Select(this);
					this.colorDef = newCol;
					this.cachedAttachmentString = null;
				}, new Color?(newCol.color), new TipSignal?(newCol.LabelCap)));
			}
			Find.WindowStack.Add(new FloatMenuGrid(list));
			Find.DesignatorManager.Select(this);
		}

		// Token: 0x06011807 RID: 71687 RVA: 0x00506CCC File Offset: 0x00504ECC
		public override void DrawMouseAttachments()
		{
			this.eyedropMode = KeyBindingDefOf.ShowEyedropper.IsDown;
			if (this.eyedropMode)
			{
				this.eyedropper.DrawMouseAttachments();
				return;
			}
			if (this.useMouseIcon)
			{
				Texture icon = this.icon;
				string attachmentString = this.AttachmentString;
				float iconAngle = this.iconAngle;
				Vector2 iconOffset = this.iconOffset;
				Color? color = new Color?(this.colorDef.color);
				GenUI.DrawMouseAttachment(icon, attachmentString, iconAngle, iconOffset, null, null, false, default(Color), color, delegate(Rect r)
				{
					GUI.DrawTexture(r, this.IconTopTex);
				});
			}
			if (Find.DesignatorManager.Dragger.Dragging)
			{
				Vector2 vector = Event.current.mousePosition + Designator_Place.PlaceMouseAttachmentDrawOffset;
				if (this.useMouseIcon)
				{
					vector.y += 32f + Text.LineHeight * 2f;
				}
				Rect rect = new Rect(vector.x, vector.y, 27f, 27f);
				ThingDef dye = ThingDefOf.Dye;
				ThingDef stuffDef = null;
				ThingStyleDef thingStyleDef = null;
				float scale = 1f;
				Color? color = null;
				Widgets.ThingIcon(rect, dye, stuffDef, thingStyleDef, scale, color, null, 1f);
				int num = this.NumHighlightedCells();
				string text = num.ToStringCached();
				if (base.Map.resourceCounter.GetCount(ThingDefOf.Dye) < num)
				{
					GUI.color = Color.red;
					text += " (" + "NotEnoughStoredLower".Translate() + ")";
				}
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.MiddleLeft;
				Widgets.Label(new Rect(vector.x + 29f, vector.y, 999f, 29f), text);
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
			}
		}

		// Token: 0x06011808 RID: 71688 RVA: 0x00506E97 File Offset: 0x00505097
		public override void DrawIcon(Rect rect, Material buttonMat, GizmoRenderParms parms)
		{
			base.DrawIcon(rect, buttonMat, parms);
			Widgets.DrawTextureFitted(rect, this.IconTopTex, this.iconDrawScale * 0.85f, this.iconProportions, this.iconTexCoords, this.iconAngle, buttonMat, 1f);
		}

		// Token: 0x06011809 RID: 71689
		protected abstract int NumHighlightedCells();

		// Token: 0x0400B4DE RID: 46302
		protected ColorDef colorDef;

		// Token: 0x0400B4DF RID: 46303
		private string cachedAttachmentString;
	}
}
