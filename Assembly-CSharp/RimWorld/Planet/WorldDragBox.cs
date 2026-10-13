using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E74 RID: 15988
	public class WorldDragBox
	{
		// Token: 0x170036A4 RID: 13988
		// (get) Token: 0x06015DF5 RID: 89589 RVA: 0x0068AF56 File Offset: 0x00689156
		public float LeftX
		{
			get
			{
				return Math.Min(this.start.x, UI.MousePositionOnUIInverted.x);
			}
		}

		// Token: 0x170036A5 RID: 13989
		// (get) Token: 0x06015DF6 RID: 89590 RVA: 0x0068AF72 File Offset: 0x00689172
		public float RightX
		{
			get
			{
				return Math.Max(this.start.x, UI.MousePositionOnUIInverted.x);
			}
		}

		// Token: 0x170036A6 RID: 13990
		// (get) Token: 0x06015DF7 RID: 89591 RVA: 0x0068AF8E File Offset: 0x0068918E
		public float BotZ
		{
			get
			{
				return Math.Min(this.start.y, UI.MousePositionOnUIInverted.y);
			}
		}

		// Token: 0x170036A7 RID: 13991
		// (get) Token: 0x06015DF8 RID: 89592 RVA: 0x0068AFAA File Offset: 0x006891AA
		public float TopZ
		{
			get
			{
				return Math.Max(this.start.y, UI.MousePositionOnUIInverted.y);
			}
		}

		// Token: 0x170036A8 RID: 13992
		// (get) Token: 0x06015DF9 RID: 89593 RVA: 0x0068AFC6 File Offset: 0x006891C6
		public Rect ScreenRect
		{
			get
			{
				return new Rect(this.LeftX, this.BotZ, this.RightX - this.LeftX, this.TopZ - this.BotZ);
			}
		}

		// Token: 0x170036A9 RID: 13993
		// (get) Token: 0x06015DFA RID: 89594 RVA: 0x0068AFF4 File Offset: 0x006891F4
		public float Diagonal
		{
			get
			{
				return (this.start - new Vector2(UI.MousePositionOnUIInverted.x, UI.MousePositionOnUIInverted.y)).magnitude;
			}
		}

		// Token: 0x170036AA RID: 13994
		// (get) Token: 0x06015DFB RID: 89595 RVA: 0x0068B02D File Offset: 0x0068922D
		public bool IsValid
		{
			get
			{
				return this.Diagonal > 7f;
			}
		}

		// Token: 0x170036AB RID: 13995
		// (get) Token: 0x06015DFC RID: 89596 RVA: 0x0068B03C File Offset: 0x0068923C
		public bool IsValidAndActive
		{
			get
			{
				return this.active && this.IsValid;
			}
		}

		// Token: 0x06015DFD RID: 89597 RVA: 0x0068B04E File Offset: 0x0068924E
		public void DragBoxOnGUI()
		{
			if (this.IsValidAndActive)
			{
				Widgets.DrawBox(this.ScreenRect, 2, null);
			}
		}

		// Token: 0x06015DFE RID: 89598 RVA: 0x0068B065 File Offset: 0x00689265
		public bool Contains(WorldObject o)
		{
			return this.Contains(o.ScreenPos());
		}

		// Token: 0x06015DFF RID: 89599 RVA: 0x0068B074 File Offset: 0x00689274
		public bool Contains(Vector2 screenPoint)
		{
			return screenPoint.x + 0.5f > this.LeftX && screenPoint.x - 0.5f < this.RightX && screenPoint.y + 0.5f > this.BotZ && screenPoint.y - 0.5f < this.TopZ;
		}

		// Token: 0x0400F2AC RID: 62124
		public bool active;

		// Token: 0x0400F2AD RID: 62125
		public Vector2 start;

		// Token: 0x0400F2AE RID: 62126
		private const float DragBoxMinDiagonal = 7f;
	}
}
