using System;
using System.Collections.Generic;
using System.Text;
using LudeonTK;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002140 RID: 8512
	[StaticConstructorOnStartup]
	public class CompPowerPlantWind : CompPowerPlant
	{
		// Token: 0x17001C43 RID: 7235
		// (get) Token: 0x0600B9DE RID: 47582 RVA: 0x00359E73 File Offset: 0x00358073
		protected override float DesiredPowerOutput
		{
			get
			{
				return this.cachedPowerOutput;
			}
		}

		// Token: 0x17001C44 RID: 7236
		// (get) Token: 0x0600B9DF RID: 47583 RVA: 0x00359E7B File Offset: 0x0035807B
		private float PowerPercent
		{
			get
			{
				return base.PowerOutput / (-base.Props.PowerConsumption * 1.5f);
			}
		}

		// Token: 0x0600B9E0 RID: 47584 RVA: 0x00359E98 File Offset: 0x00358098
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			CompPowerPlantWind.BarSize = new Vector2((float)this.parent.def.size.z - 0.95f, 0.14f);
			this.RecalculateBlockages();
			this.spinPosition = Rand.Range(0f, 15f);
		}

		// Token: 0x0600B9E1 RID: 47585 RVA: 0x00359EF4 File Offset: 0x003580F4
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			this.windPathCells.Clear();
			Sustainer sustainer = this.sustainer;
			if (sustainer != null && !sustainer.Ended)
			{
				this.sustainer.End();
			}
		}

		// Token: 0x0600B9E2 RID: 47586 RVA: 0x00359F31 File Offset: 0x00358131
		public override void PostSwapMap()
		{
			this.RecalculateBlockages();
		}

		// Token: 0x0600B9E3 RID: 47587 RVA: 0x00359F39 File Offset: 0x00358139
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceWeatherUpdate, "updateCounter", 0, false);
			Scribe_Values.Look<float>(ref this.cachedPowerOutput, "cachedPowerOutput", 0f, false);
		}

		// Token: 0x0600B9E4 RID: 47588 RVA: 0x00359F6C File Offset: 0x0035816C
		public override void CompTick()
		{
			base.CompTick();
			if (!base.PowerOn)
			{
				this.cachedPowerOutput = 0f;
				return;
			}
			this.ticksSinceWeatherUpdate++;
			if (this.ticksSinceWeatherUpdate >= this.updateWeatherEveryXTicks)
			{
				float num = Mathf.Min(this.parent.Map.windManager.WindSpeed, 1.5f);
				this.ticksSinceWeatherUpdate = 0;
				this.cachedPowerOutput = -(base.Props.PowerConsumption * num);
				this.RecalculateBlockages();
				if (this.blockedDueToInOrbit)
				{
					this.cachedPowerOutput = 0f;
				}
				else if (this.windPathBlockedCells.Count > 0)
				{
					float num2 = 0f;
					for (int i = 0; i < this.windPathBlockedCells.Count; i++)
					{
						num2 += this.cachedPowerOutput * 0.2f;
					}
					this.cachedPowerOutput -= num2;
					if (this.cachedPowerOutput < 0f)
					{
						this.cachedPowerOutput = 0f;
					}
				}
			}
			if (this.cachedPowerOutput > 0.01f)
			{
				this.spinPosition += this.PowerPercent * CompPowerPlantWind.SpinRateFactor;
			}
			if (this.sustainer == null || this.sustainer.Ended)
			{
				this.sustainer = SoundDefOf.WindTurbine_Ambience.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.None));
			}
			this.sustainer.Maintain();
			this.sustainer.externalParams["PowerOutput"] = this.PowerPercent;
		}

		// Token: 0x0600B9E5 RID: 47589 RVA: 0x0035A0EC File Offset: 0x003582EC
		public override void PostDraw()
		{
			base.PostDraw();
			GenDraw.FillableBarRequest r = new GenDraw.FillableBarRequest
			{
				center = this.parent.DrawPos + Vector3.up * 0.1f,
				size = CompPowerPlantWind.BarSize,
				fillPercent = this.PowerPercent,
				filledMat = CompPowerPlantWind.WindTurbineBarFilledMat,
				unfilledMat = CompPowerPlantWind.WindTurbineBarUnfilledMat,
				margin = 0.15f
			};
			Rot4 rotation = this.parent.Rotation;
			rotation.Rotate(RotationDirection.Clockwise);
			r.rotation = rotation;
			GenDraw.DrawFillableBar(r);
			Vector3 vector = this.parent.TrueCenter();
			vector += this.parent.Rotation.FacingCell.ToVector3() * CompPowerPlantWind.VerticalBladeOffset;
			vector += this.parent.Rotation.RighthandCell.ToVector3() * CompPowerPlantWind.HorizontalBladeOffset;
			vector.y += 0.03658537f;
			float num = CompPowerPlantWind.BladeWidth * Mathf.Sin(this.spinPosition);
			if (num < 0f)
			{
				num *= -1f;
			}
			bool flag = this.spinPosition % 3.1415927f * 2f < 3.1415927f;
			Vector2 vector2 = new Vector2(num, 1f);
			Vector3 s = new Vector3(vector2.x, 1f, vector2.y);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(vector, this.parent.Rotation.AsQuat, s);
			Graphics.DrawMesh(flag ? MeshPool.plane10 : MeshPool.plane10Flip, matrix, CompPowerPlantWind.WindTurbineBladesMat, 0);
			vector.y -= 0.07317074f;
			matrix.SetTRS(vector, this.parent.Rotation.AsQuat, s);
			Graphics.DrawMesh(flag ? MeshPool.plane10Flip : MeshPool.plane10, matrix, CompPowerPlantWind.WindTurbineBladesMat, 0);
		}

		// Token: 0x0600B9E6 RID: 47590 RVA: 0x0035A2F4 File Offset: 0x003584F4
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.CompInspectStringExtra());
			if (this.windPathBlockedCells.Count > 0)
			{
				stringBuilder.AppendLine();
				Thing thing = null;
				if (this.windPathBlockedByThings != null)
				{
					thing = this.windPathBlockedByThings[0];
				}
				if (thing != null)
				{
					stringBuilder.Append("WindTurbine_WindPathIsBlockedBy".Translate() + " " + thing.Label);
				}
				else
				{
					stringBuilder.Append("WindTurbine_WindPathIsBlockedByRoof".Translate());
				}
			}
			if (this.blockedDueToInOrbit)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("CannotFunctionOnLayer".Translate(this.parent.Map.Tile.LayerDef.label).CapitalizeFirst().Colorize(ColoredText.WarningColor));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600B9E7 RID: 47591 RVA: 0x0035A3EC File Offset: 0x003585EC
		private void RecalculateBlockages()
		{
			if (this.windPathCells.Count == 0)
			{
				IEnumerable<IntVec3> collection = WindTurbineUtility.CalculateWindCells(this.parent.Position, this.parent.Rotation, this.parent.def.size);
				this.windPathCells.AddRange(collection);
			}
			this.windPathBlockedCells.Clear();
			this.windPathBlockedByThings.Clear();
			this.blockedDueToInOrbit = this.parent.Map.Biome.inVacuum;
			for (int i = 0; i < this.windPathCells.Count; i++)
			{
				IntVec3 intVec = this.windPathCells[i];
				if (this.parent.Map.roofGrid.Roofed(intVec))
				{
					this.windPathBlockedByThings.Add(null);
					this.windPathBlockedCells.Add(intVec);
				}
				else
				{
					List<Thing> list = this.parent.Map.thingGrid.ThingsListAt(intVec);
					for (int j = 0; j < list.Count; j++)
					{
						Thing thing = list[j];
						if (thing.def.blockWind)
						{
							this.windPathBlockedByThings.Add(thing);
							this.windPathBlockedCells.Add(intVec);
							break;
						}
					}
				}
			}
		}

		// Token: 0x0400801E RID: 32798
		public int updateWeatherEveryXTicks = 250;

		// Token: 0x0400801F RID: 32799
		private int ticksSinceWeatherUpdate;

		// Token: 0x04008020 RID: 32800
		private float cachedPowerOutput;

		// Token: 0x04008021 RID: 32801
		private List<IntVec3> windPathCells = new List<IntVec3>();

		// Token: 0x04008022 RID: 32802
		private List<Thing> windPathBlockedByThings = new List<Thing>();

		// Token: 0x04008023 RID: 32803
		private List<IntVec3> windPathBlockedCells = new List<IntVec3>();

		// Token: 0x04008024 RID: 32804
		private bool blockedDueToInOrbit;

		// Token: 0x04008025 RID: 32805
		private float spinPosition;

		// Token: 0x04008026 RID: 32806
		private Sustainer sustainer;

		// Token: 0x04008027 RID: 32807
		private const float MaxUsableWindIntensity = 1.5f;

		// Token: 0x04008028 RID: 32808
		[TweakValue("Graphics", 0f, 0.1f)]
		private static float SpinRateFactor = 0.035f;

		// Token: 0x04008029 RID: 32809
		[TweakValue("Graphics", -1f, 1f)]
		private static float HorizontalBladeOffset = -0.02f;

		// Token: 0x0400802A RID: 32810
		[TweakValue("Graphics", 0f, 1f)]
		private static float VerticalBladeOffset = 0.7f;

		// Token: 0x0400802B RID: 32811
		[TweakValue("Graphics", 4f, 8f)]
		private static float BladeWidth = 6.6f;

		// Token: 0x0400802C RID: 32812
		private const float PowerReductionPercentPerObstacle = 0.2f;

		// Token: 0x0400802D RID: 32813
		private const string TranslateWindPathIsBlockedBy = "WindTurbine_WindPathIsBlockedBy";

		// Token: 0x0400802E RID: 32814
		private const string TranslateWindPathIsBlockedByRoof = "WindTurbine_WindPathIsBlockedByRoof";

		// Token: 0x0400802F RID: 32815
		private static Vector2 BarSize;

		// Token: 0x04008030 RID: 32816
		private static readonly Material WindTurbineBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.5f, 0.475f, 0.1f), false);

		// Token: 0x04008031 RID: 32817
		private static readonly Material WindTurbineBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.15f, 0.15f, 0.15f), false);

		// Token: 0x04008032 RID: 32818
		private static readonly Material WindTurbineBladesMat = MaterialPool.MatFrom("Things/Building/Power/WindTurbine/WindTurbineBlades");
	}
}
