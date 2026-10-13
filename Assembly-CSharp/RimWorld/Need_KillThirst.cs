using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002482 RID: 9346
	public class Need_KillThirst : Need
	{
		// Token: 0x17001ED0 RID: 7888
		// (get) Token: 0x0600CAC2 RID: 51906 RVA: 0x003B6356 File Offset: 0x003B4556
		protected override bool IsFrozen
		{
			get
			{
				return (float)this.pawn.ageTracker.AgeBiologicalYears < 13f || base.IsFrozen;
			}
		}

		// Token: 0x17001ED1 RID: 7889
		// (get) Token: 0x0600CAC3 RID: 51907 RVA: 0x003B6378 File Offset: 0x003B4578
		public override bool ShowOnNeedList
		{
			get
			{
				return (float)this.pawn.ageTracker.AgeBiologicalYears >= 13f && base.ShowOnNeedList;
			}
		}

		// Token: 0x0600CAC4 RID: 51908 RVA: 0x003B639A File Offset: 0x003B459A
		public Need_KillThirst(Pawn newPawn) : base(newPawn)
		{
			this.threshPercents = new List<float>
			{
				0.3f
			};
		}

		// Token: 0x0600CAC5 RID: 51909 RVA: 0x003B63B9 File Offset: 0x003B45B9
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				this.CurLevel -= 8.333333E-05f;
			}
		}

		// Token: 0x0600CAC6 RID: 51910 RVA: 0x003B63D8 File Offset: 0x003B45D8
		public void Notify_KilledPawn(DamageInfo? dinfo)
		{
			if (dinfo == null)
			{
				return;
			}
			if (((dinfo != null) ? dinfo.GetValueOrDefault().WeaponBodyPartGroup : null) != null || ((dinfo != null) ? dinfo.GetValueOrDefault().WeaponLinkedHediff : null) != null || (dinfo.Value.Weapon != null && dinfo.Value.Weapon.IsMeleeWeapon))
			{
				this.CurLevel = 1f;
			}
		}

		// Token: 0x04008A2A RID: 35370
		public const float FallPerDay = 0.033333335f;

		// Token: 0x04008A2B RID: 35371
		private const float MinAgeForNeed = 13f;
	}
}
