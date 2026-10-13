using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003E9 RID: 1001
	public class DrawStyle_FilledOval : DrawStyle
	{
		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x0600199F RID: 6559 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanHaveDuplicates
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x000815B8 File Offset: 0x0007F7B8
		public override void Update(IntVec3 origin, IntVec3 target, List<IntVec3> buffer)
		{
			CellRect cellRect = CellRect.FromLimits(origin, target);
			float radius = (float)cellRect.Width / 2f;
			float ratio = (float)cellRect.Width / (float)cellRect.Height;
			Vector3 b = cellRect.CenterCell.ToVector3();
			if (cellRect.Width % 2 == 0)
			{
				b.x -= 0.5f;
			}
			if (cellRect.Height % 2 == 0)
			{
				b.z -= 0.5f;
			}
			foreach (IntVec3 item in cellRect.Cells)
			{
				Vector3 vector = item.ToVector3() - b;
				if (this.Filled(vector.x, vector.z, radius, ratio))
				{
					buffer.Add(item);
				}
			}
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x000816A4 File Offset: 0x0007F8A4
		protected float DistanceSqr(float x, float z, float ratio)
		{
			return Mathf.Pow(z * ratio, 2f) + Mathf.Pow(x, 2f);
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x000816BF File Offset: 0x0007F8BF
		protected bool Inside(float x, float z, float radius, float ratio)
		{
			return this.DistanceSqr(x, z, ratio) <= (radius + 0.4f) * (radius + 0.4f);
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x000816DF File Offset: 0x0007F8DF
		protected virtual bool Filled(float x, float z, float radius, float ratio)
		{
			return this.Inside(x, z, radius, ratio);
		}

		// Token: 0x04001683 RID: 5763
		private const float RadiusOffset = 0.4f;
	}
}
