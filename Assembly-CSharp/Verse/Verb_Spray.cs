using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D85 RID: 3461
	public abstract class Verb_Spray : Verb
	{
		// Token: 0x17000E00 RID: 3584
		// (get) Token: 0x06005856 RID: 22614 RVA: 0x001C692C File Offset: 0x001C4B2C
		protected override int ShotsPerBurst
		{
			get
			{
				return base.BurstShotCount;
			}
		}

		// Token: 0x17000E01 RID: 3585
		// (get) Token: 0x06005857 RID: 22615 RVA: 0x001C7C80 File Offset: 0x001C5E80
		public override float? AimAngleOverride
		{
			get
			{
				if (this.state == VerbState.Bursting && this.Available())
				{
					return new float?((this.path[this.ShotsPerBurst - this.burstShotsLeft].ToVector3Shifted() - this.caster.DrawPos).AngleFlat());
				}
				return null;
			}
		}

		// Token: 0x06005858 RID: 22616 RVA: 0x001C7CE4 File Offset: 0x001C5EE4
		protected override bool TryCastShot()
		{
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			ShootLine shootLine;
			bool flag = base.TryFindShootLineFromTo(this.caster.Position, this.currentTarget, out shootLine, false);
			if (this.verbProps.stopBurstWithoutLos && !flag)
			{
				return false;
			}
			if (base.EquipmentSource != null && this.burstShotsLeft <= 1)
			{
				CompChangeableProjectile comp = base.EquipmentSource.GetComp<CompChangeableProjectile>();
				if (comp != null)
				{
					comp.Notify_ProjectileLaunched();
				}
				CompApparelReloadable comp2 = base.EquipmentSource.GetComp<CompApparelReloadable>();
				if (comp2 != null)
				{
					comp2.UsedOnce();
				}
			}
			this.HitCell(this.path[this.ShotsPerBurst - this.burstShotsLeft]);
			this.lastShotTick = Find.TickManager.TicksGame;
			return true;
		}

		// Token: 0x06005859 RID: 22617 RVA: 0x001C7DB6 File Offset: 0x001C5FB6
		public override bool Available()
		{
			return this.ShotsPerBurst - this.burstShotsLeft >= 0;
		}

		// Token: 0x0600585A RID: 22618 RVA: 0x001C7DCB File Offset: 0x001C5FCB
		public override void WarmupComplete()
		{
			this.burstShotsLeft = this.ShotsPerBurst;
			this.state = VerbState.Bursting;
			this.initialTargetPosition = this.currentTarget.CenterVector3;
			this.PreparePath();
			base.TryCastNextBurstShot();
		}

		// Token: 0x0600585B RID: 22619
		protected abstract void PreparePath();

		// Token: 0x0600585C RID: 22620 RVA: 0x001C7DFD File Offset: 0x001C5FFD
		protected virtual void HitCell(IntVec3 cell)
		{
			EffecterDef sprayEffecterDef = this.verbProps.sprayEffecterDef;
			if (sprayEffecterDef == null)
			{
				return;
			}
			sprayEffecterDef.Spawn(this.caster.Position, cell, this.caster.Map, 1f);
		}

		// Token: 0x0600585D RID: 22621 RVA: 0x001C7E34 File Offset: 0x001C6034
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.path, "path", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<Vector3>(ref this.initialTargetPosition, "initialTargetPosition", default(Vector3), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.path == null)
			{
				this.path = new List<IntVec3>();
			}
		}

		// Token: 0x0400402E RID: 16430
		protected List<IntVec3> path = new List<IntVec3>();

		// Token: 0x0400402F RID: 16431
		protected Vector3 initialTargetPosition;
	}
}
