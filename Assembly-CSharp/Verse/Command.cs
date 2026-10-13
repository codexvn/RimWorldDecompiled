using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000B7F RID: 2943
	[StaticConstructorOnStartup]
	public abstract class Command : Gizmo
	{
		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x06004B5D RID: 19293 RVA: 0x0017FEC7 File Offset: 0x0017E0C7
		public virtual string Label
		{
			get
			{
				return this.defaultLabel;
			}
		}

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x06004B5E RID: 19294 RVA: 0x0017FECF File Offset: 0x0017E0CF
		public virtual string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x06004B5F RID: 19295 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string TopRightLabel
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x06004B60 RID: 19296 RVA: 0x0017FEDC File Offset: 0x0017E0DC
		public virtual string Desc
		{
			get
			{
				return this.defaultDesc;
			}
		}

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x06004B61 RID: 19297 RVA: 0x0017FEE4 File Offset: 0x0017E0E4
		public virtual string DescPostfix
		{
			get
			{
				return this.defaultDescPostfix;
			}
		}

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x06004B62 RID: 19298 RVA: 0x0017FEEC File Offset: 0x0017E0EC
		public virtual Color IconDrawColor
		{
			get
			{
				return this.defaultIconColor;
			}
		}

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x06004B63 RID: 19299 RVA: 0x0017FEF4 File Offset: 0x0017E0F4
		public virtual SoundDef CurActivateSound
		{
			get
			{
				return this.activateSound;
			}
		}

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x06004B64 RID: 19300 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool DoTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x06004B65 RID: 19301 RVA: 0x0017FEFC File Offset: 0x0017E0FC
		public virtual string HighlightTag
		{
			get
			{
				return this.tutorTag;
			}
		}

		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x06004B66 RID: 19302 RVA: 0x0017FEFC File Offset: 0x0017E0FC
		public virtual string TutorTagSelect
		{
			get
			{
				return this.tutorTag;
			}
		}

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x06004B67 RID: 19303 RVA: 0x0017FF04 File Offset: 0x0017E104
		public virtual Texture2D BGTexture
		{
			get
			{
				return Command.BGTex;
			}
		}

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x06004B68 RID: 19304 RVA: 0x0017FF0B File Offset: 0x0017E10B
		public virtual Texture2D BGTextureShrunk
		{
			get
			{
				return Command.BGTexShrunk;
			}
		}

		// Token: 0x06004B69 RID: 19305 RVA: 0x0017FF12 File Offset: 0x0017E112
		public override float GetWidth(float maxWidth)
		{
			return 75f;
		}

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x06004B6A RID: 19306 RVA: 0x0017FF19 File Offset: 0x0017E119
		public float GetShrunkSize
		{
			get
			{
				return 36f;
			}
		}

		// Token: 0x06004B6B RID: 19307 RVA: 0x0017FF20 File Offset: 0x0017E120
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			return this.GizmoOnGUIInt(new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f), parms);
		}

		// Token: 0x06004B6C RID: 19308 RVA: 0x0017FF46 File Offset: 0x0017E146
		public virtual GizmoResult GizmoOnGUIShrunk(Vector2 topLeft, float size, GizmoRenderParms parms)
		{
			parms.shrunk = true;
			return this.GizmoOnGUIInt(new Rect(topLeft.x, topLeft.y, size, size), parms);
		}

		// Token: 0x06004B6D RID: 19309 RVA: 0x0017FF6C File Offset: 0x0017E16C
		protected virtual GizmoResult GizmoOnGUIInt(Rect butRect, GizmoRenderParms parms)
		{
			Text.Font = GameFont.Tiny;
			Color color = Color.white;
			bool flag = false;
			if (Mouse.IsOver(butRect))
			{
				flag = true;
				if (!this.disabled)
				{
					color = GenUI.MouseoverColor;
				}
			}
			MouseoverSounds.DoRegion(butRect, SoundDefOf.Mouseover_Command);
			if (parms.highLight)
			{
				Widgets.DrawStrongHighlight(butRect.ExpandedBy(4f), null);
			}
			if (this.disabled)
			{
				parms.lowLight = true;
			}
			Material material = parms.lowLight ? TexUI.GrayscaleGUI : null;
			GUI.color = (parms.lowLight ? Command.LowLightBgColor : color);
			GenUI.DrawTextureWithMaterial(butRect, parms.shrunk ? this.BGTextureShrunk : this.BGTexture, material, default(Rect));
			GUI.color = color;
			this.DrawIcon(butRect, material, parms);
			bool flag2 = false;
			GUI.color = Color.white;
			if (parms.lowLight)
			{
				GUI.color = Command.LowLightLabelColor;
			}
			Vector2 vector = parms.shrunk ? new Vector2(3f, 0f) : new Vector2(5f, 3f);
			Rect rect = new Rect(butRect.x + vector.x, butRect.y + vector.y, butRect.width - 10f, Text.LineHeight);
			if (SteamDeck.IsSteamDeckInNonKeyboardMode)
			{
				if (parms.isFirst)
				{
					GUI.DrawTexture(new Rect(rect.x, rect.y, 21f, 21f), TexUI.SteamDeck_ButtonA);
					if (KeyBindingDefOf.Accept.KeyDownEvent)
					{
						flag2 = true;
						Event.current.Use();
					}
				}
			}
			else
			{
				KeyCode keyCode = (this.hotKey == null) ? KeyCode.None : this.hotKey.MainKey;
				if (keyCode != KeyCode.None && !GizmoGridDrawer.drawnHotKeys.Contains(keyCode))
				{
					Widgets.Label(rect, keyCode.ToStringReadable());
					GizmoGridDrawer.drawnHotKeys.Add(keyCode);
					if (this.hotKey.KeyDownEvent)
					{
						flag2 = true;
						Event.current.Use();
					}
				}
			}
			if (GizmoGridDrawer.customActivator != null && GizmoGridDrawer.customActivator(this))
			{
				flag2 = true;
			}
			if (Widgets.ButtonInvisible(butRect, true))
			{
				flag2 = true;
			}
			if (!parms.shrunk)
			{
				string topRightLabel = this.TopRightLabel;
				if (!topRightLabel.NullOrEmpty())
				{
					Vector2 vector2 = Text.CalcSize(topRightLabel);
					Rect position;
					Rect rect2 = position = new Rect(butRect.xMax - vector2.x - 2f, butRect.y + 3f, vector2.x, vector2.y);
					position.x -= 2f;
					position.width += 3f;
					Text.Anchor = TextAnchor.UpperRight;
					GUI.DrawTexture(position, TexUI.GrayTextBG);
					Widgets.Label(rect2, topRightLabel);
					Text.Anchor = TextAnchor.UpperLeft;
				}
				string labelCap = this.LabelCap;
				if (!labelCap.NullOrEmpty())
				{
					float num = Text.CalcHeight(labelCap, butRect.width + 0.1f);
					Rect rect3 = new Rect(butRect.x, butRect.yMax - num + 12f, butRect.width, num);
					GUI.DrawTexture(rect3, TexUI.GrayTextBG);
					Text.Anchor = TextAnchor.UpperCenter;
					Widgets.Label(rect3, labelCap);
					Text.Anchor = TextAnchor.UpperLeft;
				}
				GUI.color = Color.white;
			}
			if (Mouse.IsOver(butRect) && this.DoTooltip)
			{
				TipSignal tip = this.Desc;
				if (this.disabled && !this.disabledReason.NullOrEmpty())
				{
					tip.text += ("\n\n" + "DisabledCommand".Translate() + ": " + this.disabledReason).Colorize(ColorLibrary.RedReadable);
				}
				tip.text += this.DescPostfix;
				TooltipHandler.TipRegion(butRect, tip);
			}
			if (!this.HighlightTag.NullOrEmpty() && (Find.WindowStack.FloatMenu == null || !Find.WindowStack.FloatMenu.windowRect.Overlaps(butRect)))
			{
				UIHighlighter.HighlightOpportunity(butRect, this.HighlightTag);
			}
			Text.Font = GameFont.Small;
			if (flag2)
			{
				if (this.disabled)
				{
					if (!this.disabledReason.NullOrEmpty())
					{
						Messages.Message("DisabledCommand".Translate() + ": " + this.disabledReason, MessageTypeDefOf.RejectInput, false);
					}
					return new GizmoResult(GizmoState.Mouseover, null);
				}
				GizmoResult result;
				if (Event.current.button == 1)
				{
					result = new GizmoResult(GizmoState.OpenedFloatMenu, Event.current);
				}
				else
				{
					if (!TutorSystem.AllowAction(this.TutorTagSelect))
					{
						return new GizmoResult(GizmoState.Mouseover, null);
					}
					result = new GizmoResult(GizmoState.Interacted, Event.current);
					TutorSystem.Notify_Event(this.TutorTagSelect);
				}
				return result;
			}
			else
			{
				if (flag)
				{
					return new GizmoResult(GizmoState.Mouseover, null);
				}
				return new GizmoResult(GizmoState.Clear, null);
			}
		}

		// Token: 0x06004B6E RID: 19310 RVA: 0x00180438 File Offset: 0x0017E638
		public virtual void DrawIcon(Rect rect, Material buttonMat, GizmoRenderParms parms)
		{
			Texture badTex = this.icon;
			if (badTex == null)
			{
				badTex = BaseContent.BadTex;
			}
			rect.position += new Vector2(this.iconOffset.x * rect.size.x, this.iconOffset.y * rect.size.y);
			if (!this.disabled || parms.lowLight)
			{
				GUI.color = this.IconDrawColor;
			}
			else
			{
				GUI.color = this.IconDrawColor.SaturationChanged(0f);
			}
			if (parms.lowLight)
			{
				GUI.color = GUI.color.ToTransparent(0.6f);
			}
			Widgets.DrawTextureFitted(rect, badTex, this.iconDrawScale * 0.85f, this.iconProportions, this.iconTexCoords, this.iconAngle, this.overrideMaterial ?? buttonMat, 1f);
			GUI.color = Color.white;
		}

		// Token: 0x06004B6F RID: 19311 RVA: 0x00180530 File Offset: 0x0017E730
		public override bool GroupsWith(Gizmo other)
		{
			if (!this.groupable)
			{
				return false;
			}
			Command command = other as Command;
			return command != null && command.groupable && ((this.hotKey == command.hotKey && this.Label == command.Label && this.icon == command.icon && this.groupKey == command.groupKey) || (this.groupKeyIgnoreContent != -1 && command.groupKeyIgnoreContent != -1 && this.groupKeyIgnoreContent == command.groupKeyIgnoreContent));
		}

		// Token: 0x06004B70 RID: 19312 RVA: 0x001805C4 File Offset: 0x0017E7C4
		public override void ProcessInput(Event ev)
		{
			if (this.CurActivateSound != null)
			{
				this.CurActivateSound.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06004B71 RID: 19313 RVA: 0x001805DA File Offset: 0x0017E7DA
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Command(label=",
				this.defaultLabel,
				", defaultDesc=",
				this.defaultDesc,
				")"
			});
		}

		// Token: 0x040035AC RID: 13740
		public string defaultLabel;

		// Token: 0x040035AD RID: 13741
		public string defaultDesc;

		// Token: 0x040035AE RID: 13742
		public string defaultDescPostfix;

		// Token: 0x040035AF RID: 13743
		public string mouseText;

		// Token: 0x040035B0 RID: 13744
		public Texture icon;

		// Token: 0x040035B1 RID: 13745
		public float iconAngle;

		// Token: 0x040035B2 RID: 13746
		public Vector2 iconProportions = Vector2.one;

		// Token: 0x040035B3 RID: 13747
		public Rect iconTexCoords = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x040035B4 RID: 13748
		public float iconDrawScale = 1f;

		// Token: 0x040035B5 RID: 13749
		public Vector2 iconOffset;

		// Token: 0x040035B6 RID: 13750
		public Color defaultIconColor = Color.white;

		// Token: 0x040035B7 RID: 13751
		public KeyBindingDef hotKey;

		// Token: 0x040035B8 RID: 13752
		public SoundDef activateSound;

		// Token: 0x040035B9 RID: 13753
		public int groupKey = -1;

		// Token: 0x040035BA RID: 13754
		public int groupKeyIgnoreContent = -1;

		// Token: 0x040035BB RID: 13755
		public string tutorTag = "TutorTagNotSet";

		// Token: 0x040035BC RID: 13756
		public bool shrinkable;

		// Token: 0x040035BD RID: 13757
		public bool groupable = true;

		// Token: 0x040035BE RID: 13758
		public bool hideMouseIcon;

		// Token: 0x040035BF RID: 13759
		public Material overrideMaterial;

		// Token: 0x040035C0 RID: 13760
		public static readonly Texture2D BGTex = ContentFinder<Texture2D>.Get("UI/Widgets/DesButBG", true);

		// Token: 0x040035C1 RID: 13761
		public static readonly Texture2D BGTexShrunk = ContentFinder<Texture2D>.Get("UI/Widgets/DesButBG", true);

		// Token: 0x040035C2 RID: 13762
		public static readonly Color LowLightBgColor = new Color(0.8f, 0.8f, 0.7f, 0.5f);

		// Token: 0x040035C3 RID: 13763
		public static readonly Color LowLightIconColor = new Color(0.8f, 0.8f, 0.7f, 0.6f);

		// Token: 0x040035C4 RID: 13764
		public static readonly Color LowLightLabelColor = Color.white;

		// Token: 0x040035C5 RID: 13765
		public const float LowLightIconAlpha = 0.6f;

		// Token: 0x040035C6 RID: 13766
		protected const float InnerIconDrawScale = 0.85f;
	}
}
