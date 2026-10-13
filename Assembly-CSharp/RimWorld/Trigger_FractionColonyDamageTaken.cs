using System;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001760 RID: 5984
	public class Trigger_FractionColonyDamageTaken : Trigger
	{
		// Token: 0x17001596 RID: 5526
		// (get) Token: 0x06008C79 RID: 35961 RVA: 0x0029C79B File Offset: 0x0029A99B
		private TriggerData_FractionColonyDamageTaken Data
		{
			get
			{
				return (TriggerData_FractionColonyDamageTaken)this.data;
			}
		}

		// Token: 0x06008C7A RID: 35962 RVA: 0x0029C7A8 File Offset: 0x0029A9A8
		public Trigger_FractionColonyDamageTaken(float desiredColonyDamageFraction, float minDamage = 3.4028235E+38f)
		{
			this.data = new TriggerData_FractionColonyDamageTaken();
			this.desiredColonyDamageFraction = desiredColonyDamageFraction;
			this.minDamage = minDamage;
		}

		// Token: 0x06008C7B RID: 35963 RVA: 0x0029C7C9 File Offset: 0x0029A9C9
		public override void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			if (!transition.sources.Contains(previousToil))
			{
				this.Data.startColonyDamage = transition.Map.damageWatcher.DamageTakenEver;
			}
		}

		// Token: 0x06008C7C RID: 35964 RVA: 0x0029C7F4 File Offset: 0x0029A9F4
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				if (this.data == null || !(this.data is TriggerData_FractionColonyDamageTaken))
				{
					BackCompatibility.TriggerDataFractionColonyDamageTakenNull(this, lord.Map);
				}
				float num = Mathf.Max((float)lord.initialColonyHealthTotal * this.desiredColonyDamageFraction, this.minDamage);
				return lord.Map.damageWatcher.DamageTakenEver > this.Data.startColonyDamage + num;
			}
			return false;
		}

		// Token: 0x04005B4C RID: 23372
		private float desiredColonyDamageFraction;

		// Token: 0x04005B4D RID: 23373
		private float minDamage;
	}
}
