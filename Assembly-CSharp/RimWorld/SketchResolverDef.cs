using System;
using RimWorld.SketchGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AE7 RID: 6887
	public class SketchResolverDef : Def
	{
		// Token: 0x060099ED RID: 39405 RVA: 0x002CC91F File Offset: 0x002CAB1F
		public void Resolve(SketchResolveParams parms)
		{
			this.resolver.Resolve(parms);
		}

		// Token: 0x060099EE RID: 39406 RVA: 0x002CC92D File Offset: 0x002CAB2D
		public bool CanResolve(SketchResolveParams parms)
		{
			return this.resolver.CanResolve(parms);
		}

		// Token: 0x040069A6 RID: 27046
		public SketchResolver resolver;

		// Token: 0x040069A7 RID: 27047
		public bool isRoot;
	}
}
