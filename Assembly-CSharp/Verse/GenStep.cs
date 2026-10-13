using System;

namespace Verse
{
	// Token: 0x02000281 RID: 641
	public abstract class GenStep
	{
		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06001024 RID: 4132
		public abstract int SeedPart { get; }

		// Token: 0x06001025 RID: 4133
		public abstract void Generate(Map map, GenStepParams parms);

		// Token: 0x06001026 RID: 4134 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostMapInitialized(Map map, GenStepParams parms)
		{
		}

		// Token: 0x04000CBA RID: 3258
		public GenStepDef def;
	}
}
