using System;

namespace Verse
{
	// Token: 0x02000747 RID: 1863
	public class Hediff_Implant : HediffWithComps
	{
		// Token: 0x06002D77 RID: 11639 RVA: 0x000EA52E File Offset: 0x000E872E
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (base.Part == null)
			{
				Log.Error(this.def.defName + " has null Part. It should be set before PostAdd.");
				return;
			}
		}

		// Token: 0x06002D78 RID: 11640 RVA: 0x000EA55C File Offset: 0x000E875C
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error(base.GetType().Name + " has null part after loading.");
				this.pawn.health.hediffSet.hediffs.Remove(this);
				return;
			}
		}
	}
}
