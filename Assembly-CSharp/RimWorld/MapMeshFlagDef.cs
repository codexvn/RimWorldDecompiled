using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A31 RID: 6705
	public class MapMeshFlagDef : Def
	{
		// Token: 0x06009722 RID: 38690 RVA: 0x002C2B0E File Offset: 0x002C0D0E
		public override void PostSetIndices()
		{
			FlagDefUtility.SetMaskFromIndex(this, ref this.mask);
		}

		// Token: 0x06009723 RID: 38691 RVA: 0x002C2B1C File Offset: 0x002C0D1C
		public static implicit operator ulong(MapMeshFlagDef def)
		{
			if (def == null)
			{
				return 0UL;
			}
			return def.mask;
		}

		// Token: 0x06009724 RID: 38692 RVA: 0x002C2B2A File Offset: 0x002C0D2A
		public override string ToString()
		{
			return base.ToString() + string.Format(" ({0})", this.mask);
		}

		// Token: 0x04006414 RID: 25620
		private ulong mask;
	}
}
