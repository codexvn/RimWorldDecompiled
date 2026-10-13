using System;
using System.Collections.Generic;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02002110 RID: 8464
	public class RiverNode
	{
		// Token: 0x04007F4B RID: 32587
		public List<RiverNode> childNodes = new List<RiverNode>();

		// Token: 0x04007F4C RID: 32588
		public Vector3 start;

		// Token: 0x04007F4D RID: 32589
		public Vector3 end;

		// Token: 0x04007F4E RID: 32590
		public float width;
	}
}
