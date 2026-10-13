using System;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x020010E8 RID: 4328
	public class Trigger_TicksPassed : Trigger
	{
		// Token: 0x1700103B RID: 4155
		// (get) Token: 0x06006952 RID: 26962 RVA: 0x002131BA File Offset: 0x002113BA
		protected TriggerData_TicksPassed Data
		{
			get
			{
				return (TriggerData_TicksPassed)this.data;
			}
		}

		// Token: 0x1700103C RID: 4156
		// (get) Token: 0x06006953 RID: 26963 RVA: 0x002131C7 File Offset: 0x002113C7
		public int TicksLeft
		{
			get
			{
				return Mathf.Max(this.duration - this.Data.ticksPassed, 0);
			}
		}

		// Token: 0x06006954 RID: 26964 RVA: 0x002131E1 File Offset: 0x002113E1
		public Trigger_TicksPassed(int tickLimit)
		{
			this.data = new TriggerData_TicksPassed();
			this.duration = tickLimit;
		}

		// Token: 0x06006955 RID: 26965 RVA: 0x00213204 File Offset: 0x00211404
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				if (this.data == null || !(this.data is TriggerData_TicksPassed))
				{
					BackCompatibility.TriggerDataTicksPassedNull(this);
				}
				TriggerData_TicksPassed data = this.Data;
				data.ticksPassed++;
				return data.ticksPassed > this.duration;
			}
			return false;
		}

		// Token: 0x06006956 RID: 26966 RVA: 0x00213258 File Offset: 0x00211458
		public override void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			if (!transition.sources.Contains(previousToil))
			{
				this.Data.ticksPassed = 0;
			}
		}

		// Token: 0x04004A82 RID: 19074
		private int duration = 100;
	}
}
