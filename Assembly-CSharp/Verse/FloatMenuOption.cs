using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Sound;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000B6D RID: 2925
	[StaticConstructorOnStartup]
	public class FloatMenuOption
	{
		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x06004AFD RID: 19197 RVA: 0x0017D4C1 File Offset: 0x0017B6C1
		private static float NormalVerticalMargin
		{
			get
			{
				return (float)(SteamDeck.IsSteamDeck ? 10 : 4);
			}
		}

		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x06004AFE RID: 19198 RVA: 0x0017D4D0 File Offset: 0x0017B6D0
		// (set) Token: 0x06004AFF RID: 19199 RVA: 0x0017D4D8 File Offset: 0x0017B6D8
		public string Label
		{
			get
			{
				return this.labelInt;
			}
			set
			{
				if (value.NullOrEmpty())
				{
					value = "(missing label)";
				}
				this.labelInt = value.TrimEnd();
				this.SetSizeMode(this.sizeMode);
			}
		}

		// Token: 0x17000B65 RID: 2917
		// (get) Token: 0x06004B00 RID: 19200 RVA: 0x0017D501 File Offset: 0x0017B701
		private float VerticalMargin
		{
			get
			{
				if (this.sizeMode != FloatMenuSizeMode.Normal)
				{
					return 1f;
				}
				return FloatMenuOption.NormalVerticalMargin;
			}
		}

		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x06004B01 RID: 19201 RVA: 0x0017D517 File Offset: 0x0017B717
		private float HorizontalMargin
		{
			get
			{
				if (this.sizeMode != FloatMenuSizeMode.Normal)
				{
					return 3f;
				}
				return 6f;
			}
		}

		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x06004B02 RID: 19202 RVA: 0x0017D52D File Offset: 0x0017B72D
		private float IconOffset
		{
			get
			{
				if (this.shownItem == null && !this.drawPlaceHolderIcon && !(this.iconTex != null) && this.iconThing == null)
				{
					return 0f;
				}
				return this.CurIconSize;
			}
		}

		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x06004B03 RID: 19203 RVA: 0x0017D561 File Offset: 0x0017B761
		private GameFont CurrentFont
		{
			get
			{
				if (this.sizeMode != FloatMenuSizeMode.Normal)
				{
					return GameFont.Tiny;
				}
				return GameFont.Small;
			}
		}

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x06004B04 RID: 19204 RVA: 0x0017D56F File Offset: 0x0017B76F
		private float CurIconSize
		{
			get
			{
				if (this.sizeMode != FloatMenuSizeMode.Tiny)
				{
					return 27f;
				}
				return 16f;
			}
		}

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x06004B05 RID: 19205 RVA: 0x0017D585 File Offset: 0x0017B785
		// (set) Token: 0x06004B06 RID: 19206 RVA: 0x0017D590 File Offset: 0x0017B790
		public bool Disabled
		{
			get
			{
				return this.action == null;
			}
			set
			{
				if (value)
				{
					this.action = null;
				}
			}
		}

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x06004B07 RID: 19207 RVA: 0x0017D59C File Offset: 0x0017B79C
		public float RequiredHeight
		{
			get
			{
				return this.cachedRequiredHeight;
			}
		}

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x06004B08 RID: 19208 RVA: 0x0017D5A4 File Offset: 0x0017B7A4
		public float RequiredWidth
		{
			get
			{
				return this.cachedRequiredWidth;
			}
		}

		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x06004B09 RID: 19209 RVA: 0x0017D5AC File Offset: 0x0017B7AC
		// (set) Token: 0x06004B0A RID: 19210 RVA: 0x0017D5BE File Offset: 0x0017B7BE
		public MenuOptionPriority Priority
		{
			get
			{
				if (this.Disabled)
				{
					return MenuOptionPriority.DisabledOption;
				}
				return this.priorityInt;
			}
			set
			{
				if (this.Disabled)
				{
					Log.Error("Setting priority on disabled FloatMenuOption: " + this.Label);
				}
				this.priorityInt = value;
			}
		}

		// Token: 0x06004B0B RID: 19211 RVA: 0x0017D5E4 File Offset: 0x0017B7E4
		public FloatMenuOption(string label, Action action, MenuOptionPriority priority = MenuOptionPriority.Default, Action<Rect> mouseoverGuiAction = null, Thing revalidateClickTarget = null, float extraPartWidth = 0f, Func<Rect, bool> extraPartOnGUI = null, WorldObject revalidateWorldClickTarget = null, bool playSelectionSound = true, int orderInPriority = 0)
		{
			this.Label = label;
			this.action = action;
			this.priorityInt = priority;
			this.revalidateClickTarget = revalidateClickTarget;
			this.mouseoverGuiAction = mouseoverGuiAction;
			this.extraPartWidth = extraPartWidth;
			this.extraPartOnGUI = extraPartOnGUI;
			this.revalidateWorldClickTarget = revalidateWorldClickTarget;
			this.playSelectionSound = playSelectionSound;
			this.orderInPriority = orderInPriority;
		}

		// Token: 0x06004B0C RID: 19212 RVA: 0x0017D67C File Offset: 0x0017B87C
		public FloatMenuOption(string label, Action action, ThingDef shownItemForIcon, ThingStyleDef thingStyle = null, bool forceBasicStyle = false, MenuOptionPriority priority = MenuOptionPriority.Default, Action<Rect> mouseoverGuiAction = null, Thing revalidateClickTarget = null, float extraPartWidth = 0f, Func<Rect, bool> extraPartOnGUI = null, WorldObject revalidateWorldClickTarget = null, bool playSelectionSound = true, int orderInPriority = 0, int? graphicIndexOverride = null) : this(label, action, priority, mouseoverGuiAction, revalidateClickTarget, extraPartWidth, extraPartOnGUI, revalidateWorldClickTarget, playSelectionSound, orderInPriority)
		{
			this.shownItem = shownItemForIcon;
			this.thingStyle = thingStyle;
			this.forceBasicStyle = forceBasicStyle;
			this.graphicIndexOverride = graphicIndexOverride;
			if (shownItemForIcon == null)
			{
				this.drawPlaceHolderIcon = true;
			}
		}

		// Token: 0x06004B0D RID: 19213 RVA: 0x0017D6CC File Offset: 0x0017B8CC
		public FloatMenuOption(string label, Action action, ThingDef shownItemForIcon, Texture2D iconTex, ThingStyleDef thingStyle = null, bool forceBasicStyle = false, MenuOptionPriority priority = MenuOptionPriority.Default, Action<Rect> mouseoverGuiAction = null, Thing revalidateClickTarget = null, float extraPartWidth = 0f, Func<Rect, bool> extraPartOnGUI = null, WorldObject revalidateWorldClickTarget = null, bool playSelectionSound = true, int orderInPriority = 0, int? graphicIndexOverride = null) : this(label, action, priority, mouseoverGuiAction, revalidateClickTarget, extraPartWidth, extraPartOnGUI, revalidateWorldClickTarget, playSelectionSound, orderInPriority)
		{
			this.iconTex = iconTex;
			this.shownItem = shownItemForIcon;
			this.thingStyle = thingStyle;
			this.forceBasicStyle = forceBasicStyle;
			this.graphicIndexOverride = graphicIndexOverride;
			if (shownItemForIcon == null && iconTex == null)
			{
				this.drawPlaceHolderIcon = true;
			}
		}

		// Token: 0x06004B0E RID: 19214 RVA: 0x0017D72C File Offset: 0x0017B92C
		public FloatMenuOption(string label, Action action, Texture2D iconTex, Color iconColor, MenuOptionPriority priority = MenuOptionPriority.Default, Action<Rect> mouseoverGuiAction = null, Thing revalidateClickTarget = null, float extraPartWidth = 0f, Func<Rect, bool> extraPartOnGUI = null, WorldObject revalidateWorldClickTarget = null, bool playSelectionSound = true, int orderInPriority = 0, HorizontalJustification iconJustification = HorizontalJustification.Left, bool extraPartRightJustified = false) : this(label, action, priority, mouseoverGuiAction, revalidateClickTarget, extraPartWidth, extraPartOnGUI, revalidateWorldClickTarget, playSelectionSound, orderInPriority)
		{
			this.iconTex = iconTex;
			this.iconColor = iconColor;
			this.iconJustification = iconJustification;
			this.extraPartRightJustified = extraPartRightJustified;
		}

		// Token: 0x06004B0F RID: 19215 RVA: 0x0017D770 File Offset: 0x0017B970
		public FloatMenuOption(string label, Action action, Thing iconThing, Color iconColor, MenuOptionPriority priority = MenuOptionPriority.Default, Action<Rect> mouseoverGuiAction = null, Thing revalidateClickTarget = null, float extraPartWidth = 0f, Func<Rect, bool> extraPartOnGUI = null, WorldObject revalidateWorldClickTarget = null, bool playSelectionSound = true, int orderInPriority = 0) : this(label, action, priority, mouseoverGuiAction, revalidateClickTarget, extraPartWidth, extraPartOnGUI, revalidateWorldClickTarget, playSelectionSound, orderInPriority)
		{
			this.iconThing = iconThing;
			this.iconColor = iconColor;
		}

		// Token: 0x06004B10 RID: 19216 RVA: 0x0017D7A4 File Offset: 0x0017B9A4
		public static FloatMenuOption CheckboxLabeled(string label, Action checkboxStateChanged, bool currentState)
		{
			return new FloatMenuOption(label, checkboxStateChanged, Widgets.GetCheckboxTexture(currentState), Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Right, false);
		}

		// Token: 0x06004B11 RID: 19217 RVA: 0x0017D7D4 File Offset: 0x0017B9D4
		public void SetSizeMode(FloatMenuSizeMode newSizeMode)
		{
			this.sizeMode = newSizeMode;
			GameFont font = Text.Font;
			Text.Font = this.CurrentFont;
			float width = 300f - (2f * this.HorizontalMargin + 4f + this.extraPartWidth + this.IconOffset);
			this.cachedRequiredHeight = 2f * this.VerticalMargin + Text.CalcHeight(this.Label, width);
			this.cachedRequiredWidth = this.HorizontalMargin + 4f + Text.CalcSize(this.Label).x + this.extraPartWidth + this.HorizontalMargin + this.IconOffset + 4f;
			Text.Font = font;
		}

		// Token: 0x06004B12 RID: 19218 RVA: 0x0017D884 File Offset: 0x0017BA84
		public void Chosen(bool colonistOrdering, FloatMenu floatMenu)
		{
			if (floatMenu != null)
			{
				floatMenu.PreOptionChosen(this);
			}
			if (!this.Disabled)
			{
				if (this.action != null)
				{
					if (colonistOrdering && this.playSelectionSound)
					{
						SoundDefOf.ColonistOrdered.PlayOneShotOnCamera(null);
					}
					this.action();
					return;
				}
			}
			else if (this.playSelectionSound)
			{
				SoundDefOf.ClickReject.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06004B13 RID: 19219 RVA: 0x0017D8E0 File Offset: 0x0017BAE0
		public virtual bool DoGUI(Rect rect, bool colonistOrdering, FloatMenu floatMenu)
		{
			Rect rect2 = rect;
			float height = rect2.height;
			rect2.height = height - 1f;
			bool flag = !this.Disabled && Mouse.IsOver(rect2);
			bool flag2 = false;
			Text.Font = this.CurrentFont;
			if (this.tooltip != null)
			{
				TooltipHandler.TipRegion(rect, this.tooltip.Value);
			}
			Rect rect3 = rect;
			if (this.iconJustification == HorizontalJustification.Left)
			{
				rect3.xMin += 4f;
				rect3.xMax = rect.x + this.CurIconSize;
				rect3.yMin += 4f;
				rect3.yMax = rect.y + this.CurIconSize;
				if (flag)
				{
					rect3.x += 4f;
				}
			}
			Rect rect4 = rect;
			rect4.xMin += this.HorizontalMargin;
			rect4.xMax -= this.HorizontalMargin;
			rect4.xMax -= 4f;
			rect4.xMax -= this.extraPartWidth + this.IconOffset;
			if (this.iconJustification == HorizontalJustification.Left)
			{
				rect4.x += this.IconOffset;
			}
			if (flag)
			{
				rect4.x += 4f;
			}
			float num = Mathf.Min(Text.CalcSize(this.Label).x, rect4.width - 4f);
			float num2 = rect4.xMin + num;
			if (this.iconJustification == HorizontalJustification.Right)
			{
				rect3.x = num2 + 4f;
				rect3.width = this.CurIconSize;
				rect3.yMin += 4f;
				rect3.yMax = rect.y + this.CurIconSize;
				num2 += this.CurIconSize;
			}
			Rect rect5 = default(Rect);
			if (this.extraPartWidth != 0f)
			{
				if (this.extraPartRightJustified)
				{
					num2 = rect.xMax - this.extraPartWidth;
				}
				rect5 = new Rect(num2, rect4.yMin, this.extraPartWidth, 30f);
				flag2 = Mouse.IsOver(rect5);
			}
			if (!this.Disabled)
			{
				MouseoverSounds.DoRegion(rect2);
			}
			Color color = GUI.color;
			if (this.Disabled)
			{
				GUI.color = FloatMenuOption.ColorBGDisabled * color;
			}
			else if (flag && !flag2)
			{
				GUI.color = FloatMenuOption.ColorBGActiveMouseover * color;
			}
			else
			{
				GUI.color = FloatMenuOption.ColorBGActive * color;
			}
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = ((!this.Disabled) ? FloatMenuOption.ColorTextActive : FloatMenuOption.ColorTextDisabled) * color;
			if (this.sizeMode == FloatMenuSizeMode.Tiny)
			{
				rect4.y += 1f;
			}
			Widgets.DrawAtlas(rect, TexUI.FloatMenuOptionBG);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect4, this.Label);
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = new Color(this.iconColor.r, this.iconColor.g, this.iconColor.b, this.iconColor.a * GUI.color.a);
			if (this.shownItem != null || this.drawPlaceHolderIcon)
			{
				ThingStyleDef thingStyleDef;
				if ((thingStyleDef = this.thingStyle) == null)
				{
					if (this.shownItem == null || Find.World == null)
					{
						thingStyleDef = null;
					}
					else
					{
						FactionIdeosTracker ideos = Faction.OfPlayer.ideos;
						if (ideos == null)
						{
							thingStyleDef = null;
						}
						else
						{
							Ideo primaryIdeo = ideos.PrimaryIdeo;
							thingStyleDef = ((primaryIdeo != null) ? primaryIdeo.GetStyleFor(this.shownItem) : null);
						}
					}
				}
				ThingStyleDef thingStyleDef2 = thingStyleDef;
				if (this.forceBasicStyle)
				{
					thingStyleDef2 = null;
				}
				Color value;
				if (this.forceThingColor != null)
				{
					value = this.forceThingColor.Value;
				}
				else if (this.shownItem != null)
				{
					value = (this.shownItem.MadeFromStuff ? this.shownItem.GetColorForStuff(GenStuff.DefaultStuffFor(this.shownItem)) : this.shownItem.uiIconColor);
				}
				else
				{
					value = Color.white;
				}
				value.a *= color.a;
				Widgets.DefIcon(rect3, this.shownItem, null, 1f, thingStyleDef2, this.drawPlaceHolderIcon, new Color?(value), null, this.graphicIndexOverride, 1f);
			}
			else if (this.iconTex)
			{
				Widgets.DrawTextureFitted(rect3, this.iconTex, 1f, new Vector2(1f, 1f), this.iconTexCoords, 0f, null, 1f);
			}
			else if (this.iconThing != null)
			{
				Widgets.ThingIcon(rect3, this.iconThing, color.a, null, false, 1f, false);
			}
			GUI.color = color;
			if (this.extraPartOnGUI != null)
			{
				bool flag3 = this.extraPartOnGUI(rect5);
				GUI.color = color;
				if (flag3)
				{
					return true;
				}
			}
			if (flag && this.mouseoverGuiAction != null)
			{
				this.mouseoverGuiAction(rect);
			}
			if (this.tutorTag != null)
			{
				UIHighlighter.HighlightOpportunity(rect, this.tutorTag);
			}
			if (!Widgets.ButtonInvisible(rect2, true))
			{
				return false;
			}
			if (this.tutorTag != null && !TutorSystem.AllowAction(this.tutorTag))
			{
				return false;
			}
			this.Chosen(colonistOrdering, floatMenu);
			if (this.tutorTag != null)
			{
				TutorSystem.Notify_Event(this.tutorTag);
			}
			return true;
		}

		// Token: 0x06004B14 RID: 19220 RVA: 0x0017DE28 File Offset: 0x0017C028
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"FloatMenuOption(",
				this.Label,
				", ",
				this.Disabled ? "disabled" : "enabled",
				")"
			});
		}

		// Token: 0x04003547 RID: 13639
		private string labelInt;

		// Token: 0x04003548 RID: 13640
		public Action action;

		// Token: 0x04003549 RID: 13641
		private MenuOptionPriority priorityInt = MenuOptionPriority.Default;

		// Token: 0x0400354A RID: 13642
		public int orderInPriority;

		// Token: 0x0400354B RID: 13643
		public bool autoTakeable;

		// Token: 0x0400354C RID: 13644
		public float autoTakeablePriority;

		// Token: 0x0400354D RID: 13645
		public Action<Rect> mouseoverGuiAction;

		// Token: 0x0400354E RID: 13646
		public Thing revalidateClickTarget;

		// Token: 0x0400354F RID: 13647
		public bool targetsDespawned;

		// Token: 0x04003550 RID: 13648
		public WorldObject revalidateWorldClickTarget;

		// Token: 0x04003551 RID: 13649
		public float extraPartWidth;

		// Token: 0x04003552 RID: 13650
		public Func<Rect, bool> extraPartOnGUI;

		// Token: 0x04003553 RID: 13651
		public string tutorTag;

		// Token: 0x04003554 RID: 13652
		public ThingStyleDef thingStyle;

		// Token: 0x04003555 RID: 13653
		public bool forceBasicStyle;

		// Token: 0x04003556 RID: 13654
		public TipSignal? tooltip;

		// Token: 0x04003557 RID: 13655
		public bool extraPartRightJustified;

		// Token: 0x04003558 RID: 13656
		public int? graphicIndexOverride;

		// Token: 0x04003559 RID: 13657
		public bool isGoto;

		// Token: 0x0400355A RID: 13658
		private FloatMenuSizeMode sizeMode;

		// Token: 0x0400355B RID: 13659
		private float cachedRequiredHeight;

		// Token: 0x0400355C RID: 13660
		private float cachedRequiredWidth;

		// Token: 0x0400355D RID: 13661
		private bool drawPlaceHolderIcon;

		// Token: 0x0400355E RID: 13662
		private bool playSelectionSound = true;

		// Token: 0x0400355F RID: 13663
		private ThingDef shownItem;

		// Token: 0x04003560 RID: 13664
		public Thing iconThing;

		// Token: 0x04003561 RID: 13665
		private Texture2D iconTex;

		// Token: 0x04003562 RID: 13666
		public Rect iconTexCoords = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x04003563 RID: 13667
		private HorizontalJustification iconJustification;

		// Token: 0x04003564 RID: 13668
		public Color iconColor = Color.white;

		// Token: 0x04003565 RID: 13669
		public Color? forceThingColor;

		// Token: 0x04003566 RID: 13670
		public const float MaxWidth = 300f;

		// Token: 0x04003567 RID: 13671
		private const float TinyVerticalMargin = 1f;

		// Token: 0x04003568 RID: 13672
		private const float NormalHorizontalMargin = 6f;

		// Token: 0x04003569 RID: 13673
		private const float TinyHorizontalMargin = 3f;

		// Token: 0x0400356A RID: 13674
		private const float MouseOverLabelShift = 4f;

		// Token: 0x0400356B RID: 13675
		public static readonly Color ColorBGActive = new ColorInt(21, 25, 29).ToColor;

		// Token: 0x0400356C RID: 13676
		public static readonly Color ColorBGActiveMouseover = new ColorInt(29, 45, 50).ToColor;

		// Token: 0x0400356D RID: 13677
		public static readonly Color ColorBGDisabled = new ColorInt(40, 40, 40).ToColor;

		// Token: 0x0400356E RID: 13678
		public static readonly Color ColorTextActive = Color.white;

		// Token: 0x0400356F RID: 13679
		public static readonly Color ColorTextDisabled = new Color(0.9f, 0.9f, 0.9f);

		// Token: 0x04003570 RID: 13680
		public const float ExtraPartHeight = 30f;

		// Token: 0x04003571 RID: 13681
		private const float ItemIconSize = 27f;

		// Token: 0x04003572 RID: 13682
		private const float ItemIconSizeTiny = 16f;

		// Token: 0x04003573 RID: 13683
		private const float ItemIconMargin = 4f;
	}
}
