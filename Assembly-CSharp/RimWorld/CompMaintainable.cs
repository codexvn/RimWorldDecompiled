using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D25 RID: 11557
	public class CompMaintainable : ThingComp
	{
		// Token: 0x1700293E RID: 10558
		// (get) Token: 0x060103C0 RID: 66496 RVA: 0x004B1A83 File Offset: 0x004AFC83
		public CompProperties_Maintainable Props
		{
			get
			{
				return (CompProperties_Maintainable)this.props;
			}
		}

		// Token: 0x1700293F RID: 10559
		// (get) Token: 0x060103C1 RID: 66497 RVA: 0x004B1A90 File Offset: 0x004AFC90
		public MaintainableStage CurStage
		{
			get
			{
				if (this.ticksSinceMaintain < this.Props.ticksHealthy)
				{
					return MaintainableStage.Healthy;
				}
				if (this.ticksSinceMaintain < this.Props.ticksHealthy + this.Props.ticksNeedsMaintenance)
				{
					return MaintainableStage.NeedsMaintenance;
				}
				return MaintainableStage.Damaging;
			}
		}

		// Token: 0x17002940 RID: 10560
		// (get) Token: 0x060103C2 RID: 66498 RVA: 0x004B1ACC File Offset: 0x004AFCCC
		private bool Active
		{
			get
			{
				Hive hive = this.parent as Hive;
				return hive == null || hive.CompDormant.Awake;
			}
		}

		// Token: 0x060103C3 RID: 66499 RVA: 0x004B1AF5 File Offset: 0x004AFCF5
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksSinceMaintain, "ticksSinceMaintain", 0, false);
		}

		// Token: 0x060103C4 RID: 66500 RVA: 0x004B1B09 File Offset: 0x004AFD09
		public override void CompTick()
		{
			base.CompTick();
			if (!this.Active)
			{
				return;
			}
			this.ticksSinceMaintain++;
			if (this.parent.IsHashIntervalTick(250))
			{
				this.CheckTakeDamage();
			}
		}

		// Token: 0x060103C5 RID: 66501 RVA: 0x004B1B40 File Offset: 0x004AFD40
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (!this.Active)
			{
				return;
			}
			this.ticksSinceMaintain += 250;
			this.CheckTakeDamage();
		}

		// Token: 0x060103C6 RID: 66502 RVA: 0x004B1B6C File Offset: 0x004AFD6C
		private void CheckTakeDamage()
		{
			if (this.CurStage == MaintainableStage.Damaging)
			{
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)this.Props.damagePerTickRare, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
		}

		// Token: 0x060103C7 RID: 66503 RVA: 0x004B1BB8 File Offset: 0x004AFDB8
		public void Maintained()
		{
			this.ticksSinceMaintain = 0;
		}

		// Token: 0x060103C8 RID: 66504 RVA: 0x004B1BC4 File Offset: 0x004AFDC4
		public override string CompInspectStringExtra()
		{
			MaintainableStage curStage = this.CurStage;
			if (curStage == MaintainableStage.NeedsMaintenance)
			{
				return "DueForMaintenance".Translate();
			}
			if (curStage != MaintainableStage.Damaging)
			{
				return null;
			}
			return "DeterioratingDueToLackOfMaintenance".Translate();
		}

		// Token: 0x0400A993 RID: 43411
		public int ticksSinceMaintain;
	}
}
