using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C31 RID: 11313
	public class CompProperties_CableConnection : CompProperties
	{
		// Token: 0x0600FDAC RID: 64940 RVA: 0x004992A7 File Offset: 0x004974A7
		public CompProperties_CableConnection()
		{
			this.compClass = typeof(CompCableConnection);
		}

		// Token: 0x0400A631 RID: 42545
		public Color color = Color.white;

		// Token: 0x0400A632 RID: 42546
		public List<List<Vector3>> offsets = new List<List<Vector3>>();

		// Token: 0x0400A633 RID: 42547
		public bool drawMote;

		// Token: 0x0400A634 RID: 42548
		public ThingDef moteDef;
	}
}
