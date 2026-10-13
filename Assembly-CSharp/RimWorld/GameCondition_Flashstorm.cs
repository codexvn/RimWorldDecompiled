using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001D3C RID: 7484
	public class GameCondition_Flashstorm : GameCondition
	{
		// Token: 0x170019E5 RID: 6629
		// (get) Token: 0x0600A7AA RID: 42922 RVA: 0x0030085F File Offset: 0x002FEA5F
		public int AreaRadius
		{
			get
			{
				return this.areaRadius;
			}
		}

		// Token: 0x0600A7AB RID: 42923 RVA: 0x00300868 File Offset: 0x002FEA68
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec2>(ref this.centerLocation, "centerLocation", default(IntVec2), false);
			Scribe_Values.Look<int>(ref this.areaRadius, "areaRadius", 0, false);
			Scribe_Values.Look<IntRange>(ref this.areaRadiusOverride, "areaRadiusOverride", default(IntRange), false);
			Scribe_Values.Look<int>(ref this.nextLightningTicks, "nextLightningTicks", 0, false);
			Scribe_Values.Look<IntRange>(ref this.initialStrikeDelay, "initialStrikeDelay", default(IntRange), false);
			Scribe_Values.Look<bool>(ref this.ambientSound, "ambientSound", false, false);
			Scribe_Values.Look<bool>(ref this.avoidConditionCauser, "avoidConditionCauser", false, false);
		}

		// Token: 0x0600A7AC RID: 42924 RVA: 0x00300914 File Offset: 0x002FEB14
		public override void Init()
		{
			base.Init();
			this.areaRadius = ((this.areaRadiusOverride == IntRange.Zero) ? GameCondition_Flashstorm.AreaRadiusRange.RandomInRange : this.areaRadiusOverride.RandomInRange);
			this.nextLightningTicks = Find.TickManager.TicksGame + this.initialStrikeDelay.RandomInRange;
			if (this.centerLocation.IsInvalid)
			{
				this.FindGoodCenterLocation();
			}
		}

		// Token: 0x0600A7AD RID: 42925 RVA: 0x00300988 File Offset: 0x002FEB88
		public override void GameConditionTick()
		{
			if (Find.TickManager.TicksGame > this.nextLightningTicks)
			{
				Vector2 vector = Rand.UnitVector2 * Rand.Range(0f, (float)this.areaRadius);
				IntVec3 intVec = new IntVec3((int)Math.Round((double)vector.x) + this.centerLocation.x, 0, (int)Math.Round((double)vector.y) + this.centerLocation.z);
				if (this.IsGoodLocationForStrike(intVec))
				{
					base.SingleMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(base.SingleMap, intVec));
					this.nextLightningTicks = Find.TickManager.TicksGame + GameCondition_Flashstorm.TicksBetweenStrikes.RandomInRange;
				}
			}
			if (this.ambientSound)
			{
				if (this.soundSustainer == null || this.soundSustainer.Ended)
				{
					this.soundSustainer = SoundDefOf.FlashstormAmbience.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(this.centerLocation.ToIntVec3, base.SingleMap, false), MaintenanceType.PerTick));
					return;
				}
				this.soundSustainer.Maintain();
			}
		}

		// Token: 0x0600A7AE RID: 42926 RVA: 0x00300A9E File Offset: 0x002FEC9E
		public override void End()
		{
			base.SingleMap.weatherDecider.DisableRainFor(30000);
			base.End();
		}

		// Token: 0x0600A7AF RID: 42927 RVA: 0x00300ABC File Offset: 0x002FECBC
		private void FindGoodCenterLocation()
		{
			if (base.SingleMap.Size.x <= 16 || base.SingleMap.Size.z <= 16)
			{
				throw new Exception("Map too small for flashstorm.");
			}
			for (int i = 0; i < 10; i++)
			{
				this.centerLocation = new IntVec2(Rand.Range(8, base.SingleMap.Size.x - 8), Rand.Range(8, base.SingleMap.Size.z - 8));
				if (this.IsGoodCenterLocation(this.centerLocation))
				{
					break;
				}
			}
		}

		// Token: 0x0600A7B0 RID: 42928 RVA: 0x00300B54 File Offset: 0x002FED54
		private bool IsGoodLocationForStrike(IntVec3 loc)
		{
			return loc.InBounds(base.SingleMap) && !loc.Roofed(base.SingleMap) && loc.Standable(base.SingleMap) && (!this.avoidConditionCauser || this.conditionCauser == null || !this.conditionCauser.OccupiedRect().ExpandedBy(2).Contains(loc));
		}

		// Token: 0x0600A7B1 RID: 42929 RVA: 0x00300BC0 File Offset: 0x002FEDC0
		private bool IsGoodCenterLocation(IntVec2 loc)
		{
			int num = 0;
			int num2 = (int)(3.1415927f * (float)this.areaRadius * (float)this.areaRadius / 2f);
			foreach (IntVec3 loc2 in this.GetPotentiallyAffectedCells(loc))
			{
				if (this.IsGoodLocationForStrike(loc2))
				{
					num++;
				}
				if (num >= num2)
				{
					break;
				}
			}
			return num >= num2;
		}

		// Token: 0x0600A7B2 RID: 42930 RVA: 0x00300C40 File Offset: 0x002FEE40
		private IEnumerable<IntVec3> GetPotentiallyAffectedCells(IntVec2 center)
		{
			int num;
			for (int x = center.x - this.areaRadius; x <= center.x + this.areaRadius; x = num)
			{
				for (int z = center.z - this.areaRadius; z <= center.z + this.areaRadius; z = num)
				{
					if ((center.x - x) * (center.x - x) + (center.z - z) * (center.z - z) <= this.areaRadius * this.areaRadius)
					{
						yield return new IntVec3(x, 0, z);
					}
					num = z + 1;
				}
				num = x + 1;
			}
			yield break;
		}

		// Token: 0x0400740F RID: 29711
		private static readonly IntRange AreaRadiusRange = new IntRange(45, 60);

		// Token: 0x04007410 RID: 29712
		private static readonly IntRange TicksBetweenStrikes = new IntRange(320, 800);

		// Token: 0x04007411 RID: 29713
		private const int RainDisableTicksAfterConditionEnds = 30000;

		// Token: 0x04007412 RID: 29714
		private const int AvoidConditionCauserExpandRect = 2;

		// Token: 0x04007413 RID: 29715
		public IntVec2 centerLocation = IntVec2.Invalid;

		// Token: 0x04007414 RID: 29716
		public IntRange areaRadiusOverride = IntRange.Zero;

		// Token: 0x04007415 RID: 29717
		public IntRange initialStrikeDelay = IntRange.Zero;

		// Token: 0x04007416 RID: 29718
		public bool ambientSound;

		// Token: 0x04007417 RID: 29719
		private int areaRadius;

		// Token: 0x04007418 RID: 29720
		private int nextLightningTicks;

		// Token: 0x04007419 RID: 29721
		private Sustainer soundSustainer;

		// Token: 0x0400741A RID: 29722
		public bool avoidConditionCauser;
	}
}
