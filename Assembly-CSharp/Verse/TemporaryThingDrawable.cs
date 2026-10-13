using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200053D RID: 1341
	public class TemporaryThingDrawable : IExposable
	{
		// Token: 0x060022D5 RID: 8917 RVA: 0x000B547C File Offset: 0x000B367C
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			Scribe_Values.Look<Vector3>(ref this.position, "position", default(Vector3), false);
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		// Token: 0x04001D28 RID: 7464
		public Thing thing;

		// Token: 0x04001D29 RID: 7465
		public Vector3 position;

		// Token: 0x04001D2A RID: 7466
		public int ticksLeft;
	}
}
