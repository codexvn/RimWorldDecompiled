using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003E5 RID: 997
	public abstract class DrawStyle
	{
		// Token: 0x06001992 RID: 6546
		public abstract void Update(IntVec3 origin, IntVec3 target, List<IntVec3> buffer);

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06001993 RID: 6547 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanHaveDuplicates
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06001994 RID: 6548 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool SingleCell
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Draw()
		{
		}
	}
}
