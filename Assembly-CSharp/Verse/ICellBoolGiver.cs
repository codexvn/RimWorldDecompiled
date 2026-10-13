using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003C9 RID: 969
	public interface ICellBoolGiver
	{
		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x060018E6 RID: 6374
		Color Color { get; }

		// Token: 0x060018E7 RID: 6375
		bool GetCellBool(int index);

		// Token: 0x060018E8 RID: 6376
		Color GetCellExtraColor(int index);
	}
}
