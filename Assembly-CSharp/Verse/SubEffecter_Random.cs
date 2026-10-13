using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CBA RID: 3258
	public class SubEffecter_Random : SubEffecter
	{
		// Token: 0x06005367 RID: 21351 RVA: 0x001ADC24 File Offset: 0x001ABE24
		public SubEffecter_Random(SubEffecterDef subDef, Effecter parent) : base(subDef, parent)
		{
			if (this.def.children != null)
			{
				List<float> list = new List<float>();
				foreach (SubEffecterDef subEffecterDef in subDef.children)
				{
					list.Add(subEffecterDef.randomWeight);
				}
				this.child = subDef.children.RandomElementByWeight((SubEffecterDef p) => p.randomWeight).Spawn(parent);
			}
		}

		// Token: 0x06005368 RID: 21352 RVA: 0x001ADCD0 File Offset: 0x001ABED0
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			SubEffecter subEffecter = this.child;
			if (subEffecter == null)
			{
				return;
			}
			subEffecter.SubEffectTick(A, B);
		}

		// Token: 0x06005369 RID: 21353 RVA: 0x001ADCE4 File Offset: 0x001ABEE4
		public override void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1, bool force = false)
		{
			SubEffecter subEffecter = this.child;
			if (subEffecter == null)
			{
				return;
			}
			subEffecter.SubTrigger(A, B, overrideSpawnTick, force);
		}

		// Token: 0x04003C38 RID: 15416
		public SubEffecter child;
	}
}
