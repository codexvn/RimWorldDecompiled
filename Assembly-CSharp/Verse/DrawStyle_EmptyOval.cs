using System;

namespace Verse
{
	// Token: 0x020003E7 RID: 999
	public class DrawStyle_EmptyOval : DrawStyle_FilledOval
	{
		// Token: 0x0600199B RID: 6555 RVA: 0x000814B4 File Offset: 0x0007F6B4
		protected override bool Filled(float x, float z, float radius, float ratio)
		{
			return base.Inside(x, z, radius, ratio) && (!base.Inside(x + 1f, z, radius, ratio) || !base.Inside(x - 1f, z, radius, ratio) || !base.Inside(x, z + 1f, radius, ratio) || !base.Inside(x, z - 1f, radius, ratio) || !base.Inside(x + 1f, z + 1f, radius, ratio) || !base.Inside(x + 1f, z - 1f, radius, ratio) || !base.Inside(x - 1f, z - 1f, radius, ratio) || !base.Inside(x - 1f, z + 1f, radius, ratio));
		}
	}
}
