using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x020019DF RID: 6623
	public struct WornGraphicDirectionData
	{
		// Token: 0x170016C4 RID: 5828
		// (get) Token: 0x06009592 RID: 38290 RVA: 0x002BCA58 File Offset: 0x002BAC58
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

		// Token: 0x04006164 RID: 24932
		public Vector2 offset;

		// Token: 0x04006165 RID: 24933
		public Vector2? scale;

		// Token: 0x04006166 RID: 24934
		public WornGraphicBodyTypeData male;

		// Token: 0x04006167 RID: 24935
		public WornGraphicBodyTypeData female;

		// Token: 0x04006168 RID: 24936
		public WornGraphicBodyTypeData thin;

		// Token: 0x04006169 RID: 24937
		public WornGraphicBodyTypeData hulk;

		// Token: 0x0400616A RID: 24938
		public WornGraphicBodyTypeData fat;
	}
}
