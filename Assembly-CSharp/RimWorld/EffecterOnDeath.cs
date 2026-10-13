using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001965 RID: 6501
	public class EffecterOnDeath : ThingComp
	{
		// Token: 0x17001667 RID: 5735
		// (get) Token: 0x060093A7 RID: 37799 RVA: 0x002B6EDC File Offset: 0x002B50DC
		public CompProperties_EffecterOnDeath Props
		{
			get
			{
				return (CompProperties_EffecterOnDeath)this.props;
			}
		}

		// Token: 0x060093A8 RID: 37800 RVA: 0x002B6EE9 File Offset: 0x002B50E9
		public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
		{
			this.Props.effecterDef.SpawnMaintained(this.parent.PositionHeld, prevMap, 1f);
			base.Notify_Killed(prevMap, dinfo);
		}

		// Token: 0x04005F01 RID: 24321
		private Effecter effecter;
	}
}
