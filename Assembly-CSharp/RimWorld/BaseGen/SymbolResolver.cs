using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020039B0 RID: 14768
	public abstract class SymbolResolver
	{
		// Token: 0x06014422 RID: 82978 RVA: 0x0060C7DA File Offset: 0x0060A9DA
		public virtual bool CanResolve(ResolveParams rp)
		{
			return rp.rect.Width >= this.minRectSize.x && rp.rect.Height >= this.minRectSize.z;
		}

		// Token: 0x06014423 RID: 82979
		public abstract void Resolve(ResolveParams rp);

		// Token: 0x0400DFD3 RID: 57299
		public IntVec2 minRectSize = IntVec2.One;

		// Token: 0x0400DFD4 RID: 57300
		public float selectionWeight = 1f;
	}
}
