using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200337D RID: 13181
	public class DragBox
	{
		// Token: 0x17003062 RID: 12386
		// (get) Token: 0x0601287C RID: 75900 RVA: 0x00577B59 File Offset: 0x00575D59
		public float LeftX
		{
			get
			{
				return Math.Min(this.start.x, UI.MouseMapPosition().x);
			}
		}

		// Token: 0x17003063 RID: 12387
		// (get) Token: 0x0601287D RID: 75901 RVA: 0x00577B75 File Offset: 0x00575D75
		public float RightX
		{
			get
			{
				return Math.Max(this.start.x, UI.MouseMapPosition().x);
			}
		}

		// Token: 0x17003064 RID: 12388
		// (get) Token: 0x0601287E RID: 75902 RVA: 0x00577B91 File Offset: 0x00575D91
		public float BotZ
		{
			get
			{
				return Math.Min(this.start.z, UI.MouseMapPosition().z);
			}
		}

		// Token: 0x17003065 RID: 12389
		// (get) Token: 0x0601287F RID: 75903 RVA: 0x00577BAD File Offset: 0x00575DAD
		public float TopZ
		{
			get
			{
				return Math.Max(this.start.z, UI.MouseMapPosition().z);
			}
		}

		// Token: 0x17003066 RID: 12390
		// (get) Token: 0x06012880 RID: 75904 RVA: 0x00577BCC File Offset: 0x00575DCC
		public Rect ScreenRect
		{
			get
			{
				Vector2 vector = this.start.MapToUIPosition();
				Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
				if (mousePositionOnUIInverted.x < vector.x)
				{
					float x = mousePositionOnUIInverted.x;
					mousePositionOnUIInverted.x = vector.x;
					vector.x = x;
				}
				if (mousePositionOnUIInverted.y < vector.y)
				{
					float y = mousePositionOnUIInverted.y;
					mousePositionOnUIInverted.y = vector.y;
					vector.y = y;
				}
				return new Rect
				{
					xMin = vector.x,
					xMax = mousePositionOnUIInverted.x,
					yMin = vector.y,
					yMax = mousePositionOnUIInverted.y
				};
			}
		}

		// Token: 0x17003067 RID: 12391
		// (get) Token: 0x06012881 RID: 75905 RVA: 0x00577C80 File Offset: 0x00575E80
		public bool IsValid
		{
			get
			{
				return (this.start - UI.MouseMapPosition()).magnitude > 0.5f;
			}
		}

		// Token: 0x17003068 RID: 12392
		// (get) Token: 0x06012882 RID: 75906 RVA: 0x00577CAC File Offset: 0x00575EAC
		public bool IsValidAndActive
		{
			get
			{
				return this.active && this.IsValid;
			}
		}

		// Token: 0x06012883 RID: 75907 RVA: 0x00577CBE File Offset: 0x00575EBE
		public void DragBoxOnGUI()
		{
			if (this.IsValidAndActive)
			{
				Widgets.DrawBox(this.ScreenRect, 2, null);
			}
		}

		// Token: 0x06012884 RID: 75908 RVA: 0x00577CD8 File Offset: 0x00575ED8
		public bool Contains(Thing t)
		{
			if (t is Pawn)
			{
				return this.Contains((t as Pawn).Drawer.DrawPos);
			}
			foreach (IntVec3 intVec in t.OccupiedRect())
			{
				if (this.Contains(intVec.ToVector3Shifted()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06012885 RID: 75909 RVA: 0x00577D5C File Offset: 0x00575F5C
		public bool Contains(Vector3 v)
		{
			return v.x + 0.5f > this.LeftX && v.x - 0.5f < this.RightX && v.z + 0.5f > this.BotZ && v.z - 0.5f < this.TopZ;
		}

		// Token: 0x0400C083 RID: 49283
		public bool active;

		// Token: 0x0400C084 RID: 49284
		public Vector3 start;

		// Token: 0x0400C085 RID: 49285
		private const float DragBoxMinDiagonal = 0.5f;
	}
}
