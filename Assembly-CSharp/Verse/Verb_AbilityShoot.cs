using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D7B RID: 3451
	public class Verb_AbilityShoot : Verb_Shoot, IAbilityVerb
	{
		// Token: 0x17000DF2 RID: 3570
		// (get) Token: 0x0600580C RID: 22540 RVA: 0x001C5B7B File Offset: 0x001C3D7B
		// (set) Token: 0x0600580D RID: 22541 RVA: 0x001C5B83 File Offset: 0x001C3D83
		public Ability Ability
		{
			get
			{
				return this.ability;
			}
			set
			{
				this.ability = value;
			}
		}

		// Token: 0x0600580E RID: 22542 RVA: 0x001C5B8C File Offset: 0x001C3D8C
		protected override bool TryCastShot()
		{
			bool flag = base.TryCastShot();
			if (flag)
			{
				this.ability.StartCooldown(this.ability.def.cooldownTicksRange.RandomInRange);
			}
			return flag;
		}

		// Token: 0x0600580F RID: 22543 RVA: 0x001C5BB7 File Offset: 0x001C3DB7
		public override void ExposeData()
		{
			Scribe_References.Look<Ability>(ref this.ability, "ability", false);
			base.ExposeData();
		}

		// Token: 0x04004007 RID: 16391
		private Ability ability;
	}
}
