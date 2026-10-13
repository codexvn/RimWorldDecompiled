using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200031E RID: 798
	public abstract class WorldGenStep
	{
		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x060013DA RID: 5082
		public abstract int SeedPart { get; }

		// Token: 0x060013DB RID: 5083
		public abstract void GenerateFresh(string seed, PlanetLayer layer);

		// Token: 0x060013DC RID: 5084 RVA: 0x0005DE23 File Offset: 0x0005C023
		public virtual void GenerateWithoutWorldData(string seed, PlanetLayer layer)
		{
			this.GenerateFresh(seed, layer);
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void GenerateFromScribe(string seed, PlanetLayer layer)
		{
		}

		// Token: 0x040012E6 RID: 4838
		public WorldGenStepDef def;
	}
}
