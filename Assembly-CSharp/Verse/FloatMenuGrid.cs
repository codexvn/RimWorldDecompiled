using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000B68 RID: 2920
	public class FloatMenuGrid : Window
	{
		// Token: 0x17000B5D RID: 2909
		// (get) Token: 0x06004AE4 RID: 19172 RVA: 0x0017CC7A File Offset: 0x0017AE7A
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(this.TotalWidth, this.TotalHeight);
			}
		}

		// Token: 0x17000B5E RID: 2910
		// (get) Token: 0x06004AE5 RID: 19173 RVA: 0x0017CC8D File Offset: 0x0017AE8D
		public float TotalWidth
		{
			get
			{
				return (float)this.calculatedSquareSize * (FloatMenuGrid.OptionSize.x - 1f);
			}
		}

		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x06004AE6 RID: 19174 RVA: 0x0017CCA7 File Offset: 0x0017AEA7
		public float TotalHeight
		{
			get
			{
				return (float)this.calculatedSquareSize * (FloatMenuGrid.OptionSize.y - 1f);
			}
		}

		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x06004AE7 RID: 19175 RVA: 0x00006AAC File Offset: 0x00004CAC
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06004AE8 RID: 19176 RVA: 0x0017CCC4 File Offset: 0x0017AEC4
		public FloatMenuGrid(List<FloatMenuGridOption> options) : base(null)
		{
			this.options = (from op in options
			orderby op.Priority descending
			select op).ToList<FloatMenuGridOption>();
			this.layer = WindowLayer.Super;
			this.closeOnClickedOutside = true;
			this.doWindowBackground = false;
			this.drawShadow = false;
			this.preventCameraMotion = false;
			this.calculatedSquareSize = Mathf.RoundToInt(Mathf.Sqrt(Mathf.Pow(Mathf.Round(Mathf.Sqrt((float)options.Count)), 2f)));
			SoundDefOf.FloatMenu_Open.PlayOneShotOnCamera(null);
		}

		// Token: 0x06004AE9 RID: 19177 RVA: 0x0017CD6C File Offset: 0x0017AF6C
		protected override void SetInitialSizeAndPosition()
		{
			Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
			if (mousePositionOnUIInverted.x + this.InitialSize.x > (float)UI.screenWidth)
			{
				mousePositionOnUIInverted.x = (float)UI.screenWidth - this.InitialSize.x;
			}
			if (mousePositionOnUIInverted.y > (float)UI.screenHeight)
			{
				mousePositionOnUIInverted.y = (float)UI.screenHeight;
			}
			this.windowRect = new Rect(mousePositionOnUIInverted.x, mousePositionOnUIInverted.y - this.InitialSize.y, this.InitialSize.x, this.InitialSize.y);
		}

		// Token: 0x06004AEA RID: 19178 RVA: 0x0017CE08 File Offset: 0x0017B008
		public override void DoWindowContents(Rect rect)
		{
			this.UpdateBaseColor();
			GUI.color = this.baseColor;
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < this.options.Count; i++)
			{
				FloatMenuGridOption floatMenuGridOption = this.options[i];
				float num3 = (float)num * FloatMenuGrid.OptionSize.x;
				float num4 = (float)num2 * FloatMenuGrid.OptionSize.y;
				if (num3 > 0f)
				{
					num3 -= (float)num;
				}
				if (num4 > 0f)
				{
					num4 -= (float)num2;
				}
				if (floatMenuGridOption.OnGUI(new Rect(num3, num4, FloatMenuGrid.OptionSize.x, FloatMenuGrid.OptionSize.y)))
				{
					Find.WindowStack.TryRemove(this, true);
					break;
				}
				num++;
				if (num >= this.calculatedSquareSize)
				{
					num = 0;
					num2++;
				}
			}
			GUI.color = Color.white;
		}

		// Token: 0x06004AEB RID: 19179 RVA: 0x0017CEDC File Offset: 0x0017B0DC
		private void UpdateBaseColor()
		{
			this.baseColor = Color.white;
			Rect r = new Rect(0f, 0f, this.TotalWidth, this.TotalHeight).ExpandedBy(5f);
			if (!r.Contains(Event.current.mousePosition))
			{
				float num = GenUI.DistFromRect(r, Event.current.mousePosition);
				this.baseColor = new Color(1f, 1f, 1f, 1f - num / 95f);
				if (num > 95f)
				{
					this.Close(false);
					SoundDefOf.FloatMenu_Cancel.PlayOneShotOnCamera(null);
					Find.WindowStack.TryRemove(this, true);
					return;
				}
			}
		}

		// Token: 0x06004AEC RID: 19180 RVA: 0x0017CF8D File Offset: 0x0017B18D
		public override void PostClose()
		{
			base.PostClose();
			Action action = this.onCloseCallback;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x0400352A RID: 13610
		private List<FloatMenuGridOption> options;

		// Token: 0x0400352B RID: 13611
		private int calculatedSquareSize;

		// Token: 0x0400352C RID: 13612
		private Color baseColor = Color.white;

		// Token: 0x0400352D RID: 13613
		public Action onCloseCallback;

		// Token: 0x0400352E RID: 13614
		private static readonly Vector2 OptionSize = new Vector2(34f, 34f);
	}
}
