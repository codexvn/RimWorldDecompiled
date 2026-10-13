using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200213A RID: 8506
	[StaticConstructorOnStartup]
	public class CompPowerPlantSolar : CompPowerPlant
	{
		// Token: 0x17001C3A RID: 7226
		// (get) Token: 0x0600B9A9 RID: 47529 RVA: 0x0035904E File Offset: 0x0035724E
		protected override float DesiredPowerOutput
		{
			get
			{
				return Mathf.Lerp(0f, -base.Props.PowerConsumption, this.parent.Map.skyManager.CurSkyGlow) * this.RoofedPowerOutputFactor;
			}
		}

		// Token: 0x17001C3B RID: 7227
		// (get) Token: 0x0600B9AA RID: 47530 RVA: 0x00359084 File Offset: 0x00357284
		private float RoofedPowerOutputFactor
		{
			get
			{
				int num = 0;
				int num2 = 0;
				foreach (IntVec3 c in this.parent.OccupiedRect())
				{
					num++;
					if (this.parent.Map.roofGrid.Roofed(c))
					{
						num2++;
					}
				}
				return (float)(num - num2) / (float)num;
			}
		}

		// Token: 0x0600B9AB RID: 47531 RVA: 0x00359108 File Offset: 0x00357308
		public override void PostDraw()
		{
			base.PostDraw();
			GenDraw.FillableBarRequest r = default(GenDraw.FillableBarRequest);
			r.center = this.parent.DrawPos + Vector3.up * 0.1f;
			r.size = CompPowerPlantSolar.BarSize;
			r.fillPercent = base.PowerOutput / -base.Props.PowerConsumption;
			r.filledMat = CompPowerPlantSolar.PowerPlantSolarBarFilledMat;
			r.unfilledMat = CompPowerPlantSolar.PowerPlantSolarBarUnfilledMat;
			r.margin = 0.15f;
			Rot4 rotation = this.parent.Rotation;
			rotation.Rotate(RotationDirection.Clockwise);
			r.rotation = rotation;
			GenDraw.DrawFillableBar(r);
		}

		// Token: 0x04007FF5 RID: 32757
		private const float NightPower = 0f;

		// Token: 0x04007FF6 RID: 32758
		private static readonly Vector2 BarSize = new Vector2(2.3f, 0.14f);

		// Token: 0x04007FF7 RID: 32759
		private static readonly Material PowerPlantSolarBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.5f, 0.475f, 0.1f), false);

		// Token: 0x04007FF8 RID: 32760
		private static readonly Material PowerPlantSolarBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.15f, 0.15f, 0.15f), false);
	}
}
