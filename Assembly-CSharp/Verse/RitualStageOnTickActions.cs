using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000171 RID: 369
	public class RitualStageOnTickActions : IExposable
	{
		// Token: 0x06000AAA RID: 2730 RVA: 0x000323F4 File Offset: 0x000305F4
		public void ExposeData()
		{
			Scribe_Collections.Look<ActionOnTick>(ref this.actions, "actions", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x040005A0 RID: 1440
		public List<ActionOnTick> actions = new List<ActionOnTick>();
	}
}
