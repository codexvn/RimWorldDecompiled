using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x020019DE RID: 6622
	public struct WornGraphicBodyTypeData
	{
		// Token: 0x170016C3 RID: 5827
		// (get) Token: 0x06009591 RID: 38289 RVA: 0x002BCA2C File Offset: 0x002BAC2C
		public Vector2 Scale
		{
			get
			{
				Vector2? vector = this.scale;
				if (vector == null)
				{
					return Vector2.one;
				}
				return vector.GetValueOrDefault();
			}
		}

		// Token: 0x04006162 RID: 24930
		public Vector2 offset;

		// Token: 0x04006163 RID: 24931
		public Vector2? scale;
	}
}
