using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000B65 RID: 2917
	public class FloatMenu : Window
	{
		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x06004AC7 RID: 19143 RVA: 0x00006AAC File Offset: 0x00004CAC
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x06004AC8 RID: 19144 RVA: 0x0017C457 File Offset: 0x0017A657
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(this.TotalWidth, this.TotalWindowHeight);
			}
		}

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x06004AC9 RID: 19145 RVA: 0x0017C46A File Offset: 0x0017A66A
		private float MaxWindowHeight
		{
			get
			{
				return (float)UI.screenHeight * 0.9f;
			}
		}

		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x06004ACA RID: 19146 RVA: 0x0017C478 File Offset: 0x0017A678
		private float TotalWindowHeight
		{
			get
			{
				return Mathf.Min(this.TotalViewHeight, this.MaxWindowHeight) + 1f;
			}
		}

		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x06004ACB RID: 19147 RVA: 0x0017C494 File Offset: 0x0017A694
		private float MaxViewHeight
		{
			get
			{
				if (this.UsingScrollbar)
				{
					float num = 0f;
					float num2 = 0f;
					for (int i = 0; i < this.options.Count; i++)
					{
						float requiredHeight = this.options[i].RequiredHeight;
						if (requiredHeight > num)
						{
							num = requiredHeight;
						}
						num2 += requiredHeight + -1f;
					}
					int columnCount = this.ColumnCount;
					num2 += (float)columnCount * num;
					return num2 / (float)columnCount;
				}
				return this.MaxWindowHeight;
			}
		}

		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x06004ACC RID: 19148 RVA: 0x0017C50C File Offset: 0x0017A70C
		private float TotalViewHeight
		{
			get
			{
				float num = 0f;
				float num2 = 0f;
				float maxViewHeight = this.MaxViewHeight;
				for (int i = 0; i < this.options.Count; i++)
				{
					float requiredHeight = this.options[i].RequiredHeight;
					if (num2 + requiredHeight + -1f > maxViewHeight)
					{
						if (num2 > num)
						{
							num = num2;
						}
						num2 = requiredHeight;
					}
					else
					{
						num2 += requiredHeight + -1f;
					}
				}
				return Mathf.Max(num, num2);
			}
		}

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x06004ACD RID: 19149 RVA: 0x0017C580 File Offset: 0x0017A780
		private float TotalWidth
		{
			get
			{
				float num = (float)this.ColumnCount * this.ColumnWidth;
				if (this.UsingScrollbar)
				{
					num += 16f;
				}
				return num;
			}
		}

		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x06004ACE RID: 19150 RVA: 0x0017C5B0 File Offset: 0x0017A7B0
		private float ColumnWidth
		{
			get
			{
				float num = 70f;
				for (int i = 0; i < this.options.Count; i++)
				{
					float requiredWidth = this.options[i].RequiredWidth;
					if (requiredWidth >= 300f)
					{
						return 300f;
					}
					if (requiredWidth > num)
					{
						num = requiredWidth;
					}
				}
				return Mathf.Round(num);
			}
		}

		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x06004ACF RID: 19151 RVA: 0x0017C605 File Offset: 0x0017A805
		private int MaxColumns
		{
			get
			{
				return Mathf.FloorToInt(((float)UI.screenWidth - 16f) / this.ColumnWidth);
			}
		}

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x06004AD0 RID: 19152 RVA: 0x0017C61F File Offset: 0x0017A81F
		private bool UsingScrollbar
		{
			get
			{
				return this.ColumnCountIfNoScrollbar > this.MaxColumns;
			}
		}

		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x06004AD1 RID: 19153 RVA: 0x0017C62F File Offset: 0x0017A82F
		private int ColumnCount
		{
			get
			{
				return Mathf.Min(this.ColumnCountIfNoScrollbar, this.MaxColumns);
			}
		}

		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x06004AD2 RID: 19154 RVA: 0x0017C644 File Offset: 0x0017A844
		private int ColumnCountIfNoScrollbar
		{
			get
			{
				if (this.options == null)
				{
					return 1;
				}
				Text.Font = GameFont.Small;
				int num = 1;
				float num2 = 0f;
				float maxWindowHeight = this.MaxWindowHeight;
				for (int i = 0; i < this.options.Count; i++)
				{
					float requiredHeight = this.options[i].RequiredHeight;
					if (num2 + requiredHeight + -1f > maxWindowHeight)
					{
						num2 = requiredHeight;
						num++;
					}
					else
					{
						num2 += requiredHeight + -1f;
					}
				}
				return num;
			}
		}

		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x06004AD3 RID: 19155 RVA: 0x0017C6BB File Offset: 0x0017A8BB
		public FloatMenuSizeMode SizeMode
		{
			get
			{
				if (this.options.Count > 60)
				{
					return FloatMenuSizeMode.Tiny;
				}
				return FloatMenuSizeMode.Normal;
			}
		}

		// Token: 0x06004AD4 RID: 19156 RVA: 0x0017C6D0 File Offset: 0x0017A8D0
		public FloatMenu(List<FloatMenuOption> options) : base(null)
		{
			if (options.NullOrEmpty<FloatMenuOption>())
			{
				Log.Error("Created FloatMenu with no options. Closing.");
				this.Close(true);
			}
			this.options = (from op in options
			orderby op.Priority descending, op.orderInPriority descending
			select op).ToList<FloatMenuOption>();
			for (int i = 0; i < options.Count; i++)
			{
				options[i].SetSizeMode(this.SizeMode);
			}
			this.layer = WindowLayer.Super;
			this.closeOnClickedOutside = true;
			this.doWindowBackground = false;
			this.drawShadow = false;
			this.preventCameraMotion = false;
			SoundDefOf.FloatMenu_Open.PlayOneShotOnCamera(null);
		}

		// Token: 0x06004AD5 RID: 19157 RVA: 0x0017C7B4 File Offset: 0x0017A9B4
		public FloatMenu(List<FloatMenuOption> options, string title, bool needSelection = false) : this(options)
		{
			this.title = title;
			this.needSelection = needSelection;
		}

		// Token: 0x06004AD6 RID: 19158 RVA: 0x0017C7CC File Offset: 0x0017A9CC
		protected override void SetInitialSizeAndPosition()
		{
			Vector2 vector = UI.MousePositionOnUIInverted + FloatMenu.InitialPositionShift;
			if (vector.x + this.InitialSize.x > (float)UI.screenWidth)
			{
				vector.x = (float)UI.screenWidth - this.InitialSize.x;
			}
			if (vector.y + this.InitialSize.y > (float)UI.screenHeight)
			{
				vector.y = (float)UI.screenHeight - this.InitialSize.y;
			}
			this.windowRect = new Rect(vector.x, vector.y, this.InitialSize.x, this.InitialSize.y);
		}

		// Token: 0x06004AD7 RID: 19159 RVA: 0x0017C87C File Offset: 0x0017AA7C
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			if (!this.title.NullOrEmpty())
			{
				Vector2 vector = new Vector2(this.windowRect.x, this.windowRect.y);
				Text.Font = GameFont.Small;
				float width = Mathf.Max(150f, 15f + Text.CalcSize(this.title).x);
				Rect titleRect = new Rect(vector.x + FloatMenu.TitleOffset.x, vector.y + FloatMenu.TitleOffset.y, width, 23f);
				Find.WindowStack.ImmediateWindow(6830963, titleRect, WindowLayer.Super, delegate
				{
					GUI.color = this.baseColor;
					Text.Font = GameFont.Small;
					Rect position = titleRect.AtZero();
					position.width = 150f;
					GUI.DrawTexture(position, TexUI.TextBGBlack);
					Rect rect = titleRect.AtZero();
					rect.x += 15f;
					Text.Anchor = TextAnchor.MiddleLeft;
					Widgets.Label(rect, this.title);
					Text.Anchor = TextAnchor.UpperLeft;
				}, false, false, 0f, null, false);
			}
		}

		// Token: 0x06004AD8 RID: 19160 RVA: 0x0017C950 File Offset: 0x0017AB50
		public override void DoWindowContents(Rect rect)
		{
			if (this.needSelection && Find.Selector.SingleSelectedThing == null)
			{
				Find.WindowStack.TryRemove(this, true);
				return;
			}
			this.UpdateBaseColor();
			bool usingScrollbar = this.UsingScrollbar;
			GUI.color = this.baseColor;
			Text.Font = GameFont.Small;
			Vector2 zero = Vector2.zero;
			float maxViewHeight = this.MaxViewHeight;
			float columnWidth = this.ColumnWidth;
			if (usingScrollbar)
			{
				rect.width -= 10f;
				Widgets.BeginScrollView(rect, ref this.scrollPosition, new Rect(0f, 0f, this.TotalWidth - 16f, this.TotalViewHeight), true);
			}
			for (int i = 0; i < this.options.Count; i++)
			{
				FloatMenuOption floatMenuOption = this.options[i];
				float requiredHeight = floatMenuOption.RequiredHeight;
				if (zero.y + requiredHeight + -1f > maxViewHeight)
				{
					zero.y = 0f;
					zero.x += columnWidth + -1f;
				}
				Rect rect2 = new Rect(zero.x, zero.y, columnWidth, requiredHeight);
				zero.y += requiredHeight + -1f;
				if (floatMenuOption.DoGUI(rect2, this.givesColonistOrders, this))
				{
					Find.WindowStack.TryRemove(this, true);
					break;
				}
			}
			if (usingScrollbar)
			{
				Widgets.EndScrollView();
			}
			if (Event.current.type == EventType.MouseDown)
			{
				Event.current.Use();
				this.Close(true);
			}
			GUI.color = Color.white;
		}

		// Token: 0x06004AD9 RID: 19161 RVA: 0x0017CAD1 File Offset: 0x0017ACD1
		public override void PostClose()
		{
			base.PostClose();
			if (this.onCloseCallback != null)
			{
				this.onCloseCallback();
			}
		}

		// Token: 0x06004ADA RID: 19162 RVA: 0x0017CAEC File Offset: 0x0017ACEC
		public void Cancel()
		{
			SoundDefOf.FloatMenu_Cancel.PlayOneShotOnCamera(null);
			Find.WindowStack.TryRemove(this, true);
		}

		// Token: 0x06004ADB RID: 19163 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PreOptionChosen(FloatMenuOption opt)
		{
		}

		// Token: 0x06004ADC RID: 19164 RVA: 0x0017CB08 File Offset: 0x0017AD08
		private void UpdateBaseColor()
		{
			this.baseColor = Color.white;
			if (this.vanishIfMouseDistant)
			{
				Rect r = new Rect(0f, 0f, this.TotalWidth, this.TotalWindowHeight).ContractedBy(-5f);
				if (!r.Contains(Event.current.mousePosition))
				{
					float num = GenUI.DistFromRect(r, Event.current.mousePosition);
					this.baseColor = new Color(1f, 1f, 1f, 1f - num / 95f);
					if (num > 95f)
					{
						this.Close(false);
						this.Cancel();
						return;
					}
				}
			}
		}

		// Token: 0x04003515 RID: 13589
		public bool givesColonistOrders;

		// Token: 0x04003516 RID: 13590
		public bool vanishIfMouseDistant = true;

		// Token: 0x04003517 RID: 13591
		public Action onCloseCallback;

		// Token: 0x04003518 RID: 13592
		protected List<FloatMenuOption> options;

		// Token: 0x04003519 RID: 13593
		private string title;

		// Token: 0x0400351A RID: 13594
		private bool needSelection;

		// Token: 0x0400351B RID: 13595
		private Color baseColor = Color.white;

		// Token: 0x0400351C RID: 13596
		private Vector2 scrollPosition;

		// Token: 0x0400351D RID: 13597
		private static readonly Vector2 TitleOffset = new Vector2(30f, -25f);

		// Token: 0x0400351E RID: 13598
		private const float OptionSpacing = -1f;

		// Token: 0x0400351F RID: 13599
		private const float MaxScreenHeightPercent = 0.9f;

		// Token: 0x04003520 RID: 13600
		private const float MinimumColumnWidth = 70f;

		// Token: 0x04003521 RID: 13601
		private static readonly Vector2 InitialPositionShift = new Vector2(4f, 0f);

		// Token: 0x04003522 RID: 13602
		public const float FadeStartMouseDist = 5f;

		// Token: 0x04003523 RID: 13603
		private const float FadeFinishMouseDist = 100f;

		// Token: 0x04003524 RID: 13604
		public const float FinishDistFromStartDist = 95f;
	}
}
