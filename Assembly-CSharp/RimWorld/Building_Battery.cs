using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020029D4 RID: 10708
	[StaticConstructorOnStartup]
	public class Building_Battery : Building
	{
		// Token: 0x0600EC04 RID: 60420 RVA: 0x004486A3 File Offset: 0x004468A3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToExplode, "ticksToExplode", 0, false);
		}

		// Token: 0x0600EC05 RID: 60421 RVA: 0x004486C0 File Offset: 0x004468C0
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			CompPowerBattery comp = base.GetComp<CompPowerBattery>();
			GenDraw.FillableBarRequest r = new GenDraw.FillableBarRequest
			{
				center = drawLoc + Vector3.up * 0.1f,
				size = Building_Battery.BarSize,
				fillPercent = comp.StoredEnergy / comp.Props.storedEnergyMax,
				filledMat = Building_Battery.BatteryBarFilledMat,
				unfilledMat = Building_Battery.BatteryBarUnfilledMat,
				margin = 0.15f
			};
			Rot4 rotation = base.Rotation;
			rotation.Rotate(RotationDirection.Clockwise);
			r.rotation = rotation;
			GenDraw.DrawFillableBar(r);
			if (this.ticksToExplode > 0 && base.Spawned)
			{
				base.Map.overlayDrawer.DrawOverlay(this, OverlayTypes.BurningWick);
			}
		}

		// Token: 0x0600EC06 RID: 60422 RVA: 0x0044878C File Offset: 0x0044698C
		protected override void Tick()
		{
			base.Tick();
			if (this.ticksToExplode <= 0)
			{
				return;
			}
			if (this.wickSustainer == null)
			{
				this.StartWickSustainer();
			}
			else
			{
				this.wickSustainer.Maintain();
			}
			this.ticksToExplode--;
			if (this.ticksToExplode == 0)
			{
				IntVec3 center = base.Spawned ? this.OccupiedRect().RandomCell : base.PositionHeld;
				float radius = Rand.Range(0.5f, 1f) * 3f;
				GenExplosion.DoExplosion(center, base.MapHeld, radius, DamageDefOf.Flame, null, -1, -1f, null, null, null, null, null, 0f, 1, null, null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
				base.GetComp<CompPowerBattery>().DrawPower(400f);
			}
		}

		// Token: 0x0600EC07 RID: 60423 RVA: 0x00448894 File Offset: 0x00446A94
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (!base.Destroyed && this.ticksToExplode == 0 && dinfo.Def == DamageDefOf.Flame && Rand.Value < 0.05f && base.GetComp<CompPowerBattery>().StoredEnergy > 500f)
			{
				this.ticksToExplode = Rand.Range(70, 150);
				this.StartWickSustainer();
			}
		}

		// Token: 0x0600EC08 RID: 60424 RVA: 0x00448900 File Offset: 0x00446B00
		private void StartWickSustainer()
		{
			SoundInfo info = SoundInfo.InMap(base.SpawnedParentOrMe, MaintenanceType.PerTick);
			this.wickSustainer = SoundDefOf.HissSmall.TrySpawnSustainer(info);
		}

		// Token: 0x04009C91 RID: 40081
		private int ticksToExplode;

		// Token: 0x04009C92 RID: 40082
		private Sustainer wickSustainer;

		// Token: 0x04009C93 RID: 40083
		private static readonly Vector2 BarSize = new Vector2(1.3f, 0.4f);

		// Token: 0x04009C94 RID: 40084
		private const float MinEnergyToExplode = 500f;

		// Token: 0x04009C95 RID: 40085
		private const float EnergyToLoseWhenExplode = 400f;

		// Token: 0x04009C96 RID: 40086
		private const float ExplodeChancePerDamage = 0.05f;

		// Token: 0x04009C97 RID: 40087
		private static readonly Material BatteryBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.9f, 0.85f, 0.2f), false);

		// Token: 0x04009C98 RID: 40088
		private static readonly Material BatteryBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f), false);
	}
}
