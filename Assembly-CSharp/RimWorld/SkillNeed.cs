using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020019C0 RID: 6592
	public class SkillNeed
	{
		// Token: 0x0600950B RID: 38155 RVA: 0x00066B11 File Offset: 0x00064D11
		public virtual float ValueFor(Pawn pawn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600950C RID: 38156 RVA: 0x00035AE5 File Offset: 0x00033CE5
		public virtual IEnumerable<string> ConfigErrors()
		{
			return Enumerable.Empty<string>();
		}

		// Token: 0x04006114 RID: 24852
		public SkillDef skill;

		// Token: 0x04006115 RID: 24853
		public bool required = true;
	}
}
