using System;

namespace Verse
{
	// Token: 0x0200031D RID: 797
	public class WorldGenStepDef : Def
	{
		// Token: 0x060013D8 RID: 5080 RVA: 0x0005DE0F File Offset: 0x0005C00F
		public override void PostLoad()
		{
			base.PostLoad();
			this.worldGenStep.def = this;
		}

		// Token: 0x040012E4 RID: 4836
		public float order;

		// Token: 0x040012E5 RID: 4837
		public WorldGenStep worldGenStep;
	}
}
