using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000311 RID: 785
	public class ToolCapacityDef : Def
	{
		// Token: 0x170003CC RID: 972
		// (get) Token: 0x060013A9 RID: 5033 RVA: 0x0005D4C3 File Offset: 0x0005B6C3
		public IEnumerable<ManeuverDef> Maneuvers
		{
			get
			{
				return from x in DefDatabase<ManeuverDef>.AllDefsListForReading
				where x.requiredCapacity == this
				select x;
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x060013AA RID: 5034 RVA: 0x0005D4DB File Offset: 0x0005B6DB
		public IEnumerable<VerbProperties> VerbsProperties
		{
			get
			{
				return from x in this.Maneuvers
				select x.verb;
			}
		}
	}
}
