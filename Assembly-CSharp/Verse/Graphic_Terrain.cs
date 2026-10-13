using System;

namespace Verse
{
	// Token: 0x020008B3 RID: 2227
	public class Graphic_Terrain : Graphic_Single
	{
		// Token: 0x06003861 RID: 14433 RVA: 0x0012C9DC File Offset: 0x0012ABDC
		public override string ToString()
		{
			return string.Format("Terrain(path={0}, shader={1}, color={2})", this.path, base.Shader, this.color);
		}
	}
}
