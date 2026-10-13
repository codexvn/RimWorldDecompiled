using System;

namespace RimWorld.SketchGen
{
	// Token: 0x020038C9 RID: 14537
	public class SketchResolver_DamageBuildingsLight : SketchResolver_DamageBuildings
	{
		// Token: 0x17003321 RID: 13089
		// (get) Token: 0x0601410E RID: 82190 RVA: 0x0012950E File Offset: 0x0012770E
		protected override float MaxPctOfTotalDestroyed
		{
			get
			{
				return 0.15f;
			}
		}

		// Token: 0x17003322 RID: 13090
		// (get) Token: 0x0601410F RID: 82191 RVA: 0x00302CA4 File Offset: 0x00300EA4
		protected override float HpRandomFactor
		{
			get
			{
				return 1.5f;
			}
		}

		// Token: 0x17003323 RID: 13091
		// (get) Token: 0x06014110 RID: 82192 RVA: 0x005F71BD File Offset: 0x005F53BD
		protected override float DestroyChanceExp
		{
			get
			{
				return 2f;
			}
		}
	}
}
