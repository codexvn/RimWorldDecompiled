using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D81 RID: 3457
	public class Verb_Shoot : Verb_LaunchProjectile
	{
		// Token: 0x17000DF9 RID: 3577
		// (get) Token: 0x06005831 RID: 22577 RVA: 0x001C692C File Offset: 0x001C4B2C
		protected override int ShotsPerBurst
		{
			get
			{
				return base.BurstShotCount;
			}
		}

		// Token: 0x06005832 RID: 22578 RVA: 0x001C6934 File Offset: 0x001C4B34
		public override void WarmupComplete()
		{
			base.WarmupComplete();
			Pawn pawn = this.currentTarget.Thing as Pawn;
			if (pawn != null && !pawn.Downed && !pawn.IsColonyMech && this.CasterIsPawn && this.CasterPawn.skills != null)
			{
				float num = pawn.HostileTo(this.caster) ? 170f : 20f;
				float num2 = this.verbProps.AdjustedFullCycleTime(this, this.CasterPawn);
				this.CasterPawn.skills.Learn(SkillDefOf.Shooting, num * num2, false, false);
			}
		}

		// Token: 0x06005833 RID: 22579 RVA: 0x001C69C8 File Offset: 0x001C4BC8
		protected override bool TryCastShot()
		{
			bool flag = base.TryCastShot();
			if (flag && this.CasterIsPawn)
			{
				this.CasterPawn.records.Increment(RecordDefOf.ShotsFired);
			}
			return flag;
		}
	}
}
