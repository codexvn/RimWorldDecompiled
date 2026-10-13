using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A32 RID: 6706
	public class RenderSkipFlagDef : Def
	{
		// Token: 0x06009726 RID: 38694 RVA: 0x002C2B4C File Offset: 0x002C0D4C
		public override void PostSetIndices()
		{
			FlagDefUtility.SetMaskFromIndex(this, ref this.mask);
		}

		// Token: 0x06009727 RID: 38695 RVA: 0x002C2B5A File Offset: 0x002C0D5A
		public static implicit operator ulong(RenderSkipFlagDef def)
		{
			if (def == null)
			{
				return 0UL;
			}
			return def.mask;
		}

		// Token: 0x06009728 RID: 38696 RVA: 0x002C2B68 File Offset: 0x002C0D68
		public override string ToString()
		{
			return base.ToString() + string.Format(" ({0})", this.mask);
		}

		// Token: 0x04006415 RID: 25621
		private ulong mask;
	}
}
