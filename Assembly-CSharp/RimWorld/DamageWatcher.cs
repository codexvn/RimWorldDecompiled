using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E94 RID: 7828
	public class DamageWatcher : IExposable
	{
		// Token: 0x17001A9E RID: 6814
		// (get) Token: 0x0600ADD0 RID: 44496 RVA: 0x003192E5 File Offset: 0x003174E5
		public float DamageTakenEver
		{
			get
			{
				return this.everDamage;
			}
		}

		// Token: 0x0600ADD1 RID: 44497 RVA: 0x003192ED File Offset: 0x003174ED
		public void Notify_DamageTaken(Thing damagee, float amount)
		{
			if (damagee.Faction != Faction.OfPlayer)
			{
				return;
			}
			this.everDamage += amount;
		}

		// Token: 0x0600ADD2 RID: 44498 RVA: 0x0031930B File Offset: 0x0031750B
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.everDamage, "everDamage", 0f, false);
		}

		// Token: 0x04007783 RID: 30595
		private float everDamage;
	}
}
