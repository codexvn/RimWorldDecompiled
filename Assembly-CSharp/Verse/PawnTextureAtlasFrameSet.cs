using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007FA RID: 2042
	public class PawnTextureAtlasFrameSet
	{
		// Token: 0x0600341E RID: 13342 RVA: 0x00111BD8 File Offset: 0x0010FDD8
		public int GetIndex(Rot4 rotation, PawnDrawMode drawMode)
		{
			if (drawMode == PawnDrawMode.BodyAndHead)
			{
				return rotation.AsInt;
			}
			return 4 + rotation.AsInt;
		}

		// Token: 0x04002752 RID: 10066
		public RenderTexture atlas;

		// Token: 0x04002753 RID: 10067
		public Rect[] uvRects = new Rect[8];

		// Token: 0x04002754 RID: 10068
		public Mesh[] meshes = new Mesh[8];

		// Token: 0x04002755 RID: 10069
		public bool[] isDirty = new bool[]
		{
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true
		};
	}
}
