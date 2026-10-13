using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020029FC RID: 10748
	[StaticConstructorOnStartup]
	public class Building_FermentingBarrel : Building
	{
		// Token: 0x170024BB RID: 9403
		// (get) Token: 0x0600ED81 RID: 60801 RVA: 0x0044E793 File Offset: 0x0044C993
		// (set) Token: 0x0600ED82 RID: 60802 RVA: 0x0044E79B File Offset: 0x0044C99B
		public float Progress
		{
			get
			{
				return this.progressInt;
			}
			set
			{
				if (value == this.progressInt)
				{
					return;
				}
				this.progressInt = value;
				this.barFilledCachedMat = null;
			}
		}

		// Token: 0x170024BC RID: 9404
		// (get) Token: 0x0600ED83 RID: 60803 RVA: 0x0044E7B5 File Offset: 0x0044C9B5
		private Material BarFilledMat
		{
			get
			{
				if (this.barFilledCachedMat == null)
				{
					this.barFilledCachedMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.Lerp(Building_FermentingBarrel.BarZeroProgressColor, Building_FermentingBarrel.BarFermentedColor, this.Progress), false);
				}
				return this.barFilledCachedMat;
			}
		}

		// Token: 0x170024BD RID: 9405
		// (get) Token: 0x0600ED84 RID: 60804 RVA: 0x0044E7EC File Offset: 0x0044C9EC
		public int SpaceLeftForWort
		{
			get
			{
				if (!this.Fermented)
				{
					return 25 - this.wortCount;
				}
				return 0;
			}
		}

		// Token: 0x170024BE RID: 9406
		// (get) Token: 0x0600ED85 RID: 60805 RVA: 0x0044E801 File Offset: 0x0044CA01
		private bool Empty
		{
			get
			{
				return this.wortCount <= 0;
			}
		}

		// Token: 0x170024BF RID: 9407
		// (get) Token: 0x0600ED86 RID: 60806 RVA: 0x0044E80F File Offset: 0x0044CA0F
		public bool Fermented
		{
			get
			{
				return !this.Empty && this.Progress >= 1f;
			}
		}

		// Token: 0x170024C0 RID: 9408
		// (get) Token: 0x0600ED87 RID: 60807 RVA: 0x0044E82C File Offset: 0x0044CA2C
		private float CurrentTempProgressSpeedFactor
		{
			get
			{
				CompProperties_TemperatureRuinable compProperties = this.def.GetCompProperties<CompProperties_TemperatureRuinable>();
				float ambientTemperature = base.AmbientTemperature;
				if (ambientTemperature < compProperties.minSafeTemperature)
				{
					return 0.1f;
				}
				if (ambientTemperature < 7f)
				{
					return GenMath.LerpDouble(compProperties.minSafeTemperature, 7f, 0.1f, 1f, ambientTemperature);
				}
				return 1f;
			}
		}

		// Token: 0x170024C1 RID: 9409
		// (get) Token: 0x0600ED88 RID: 60808 RVA: 0x0044E884 File Offset: 0x0044CA84
		private float ProgressPerTickAtCurrentTemp
		{
			get
			{
				return 2.7777778E-06f * this.CurrentTempProgressSpeedFactor;
			}
		}

		// Token: 0x170024C2 RID: 9410
		// (get) Token: 0x0600ED89 RID: 60809 RVA: 0x0044E892 File Offset: 0x0044CA92
		private int EstimatedTicksLeft
		{
			get
			{
				return Mathf.Max(Mathf.RoundToInt((1f - this.Progress) / this.ProgressPerTickAtCurrentTemp), 0);
			}
		}

		// Token: 0x0600ED8A RID: 60810 RVA: 0x0044E8B2 File Offset: 0x0044CAB2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.wortCount, "wortCount", 0, false);
			Scribe_Values.Look<float>(ref this.progressInt, "progress", 0f, false);
		}

		// Token: 0x0600ED8B RID: 60811 RVA: 0x0044E8E2 File Offset: 0x0044CAE2
		public override void TickRare()
		{
			base.TickRare();
			if (!this.Empty)
			{
				this.Progress = Mathf.Min(this.Progress + 250f * this.ProgressPerTickAtCurrentTemp, 1f);
			}
		}

		// Token: 0x0600ED8C RID: 60812 RVA: 0x0044E918 File Offset: 0x0044CB18
		public void AddWort(int count)
		{
			base.GetComp<CompTemperatureRuinable>().Reset();
			if (this.Fermented)
			{
				Log.Warning("Tried to add wort to a barrel full of beer. Colonists should take the beer first.");
				return;
			}
			int num = Mathf.Min(count, 25 - this.wortCount);
			if (num <= 0)
			{
				return;
			}
			this.Progress = GenMath.WeightedAverage(0f, (float)num, this.Progress, (float)this.wortCount);
			this.wortCount += num;
		}

		// Token: 0x0600ED8D RID: 60813 RVA: 0x0044E985 File Offset: 0x0044CB85
		protected override void ReceiveCompSignal(string signal)
		{
			if (signal == "RuinedByTemperature")
			{
				this.Reset();
			}
		}

		// Token: 0x0600ED8E RID: 60814 RVA: 0x0044E99A File Offset: 0x0044CB9A
		private void Reset()
		{
			this.wortCount = 0;
			this.Progress = 0f;
		}

		// Token: 0x0600ED8F RID: 60815 RVA: 0x0044E9B0 File Offset: 0x0044CBB0
		public void AddWort(Thing wort)
		{
			int num = Mathf.Min(wort.stackCount, 25 - this.wortCount);
			if (num > 0)
			{
				this.AddWort(num);
				wort.SplitOff(num).Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600ED90 RID: 60816 RVA: 0x0044E9EC File Offset: 0x0044CBEC
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (stringBuilder.Length != 0)
			{
				stringBuilder.AppendLine();
			}
			CompTemperatureRuinable comp = base.GetComp<CompTemperatureRuinable>();
			if (!this.Empty && !comp.Ruined)
			{
				if (this.Fermented)
				{
					stringBuilder.AppendLine("ContainsBeer".Translate(this.wortCount, 25));
				}
				else
				{
					stringBuilder.AppendLine("ContainsWort".Translate(this.wortCount, 25));
				}
			}
			if (!this.Empty)
			{
				if (this.Fermented)
				{
					stringBuilder.AppendLine("Fermented".Translate());
				}
				else
				{
					stringBuilder.AppendLine("FermentationProgress".Translate(this.Progress.ToStringPercent(), this.EstimatedTicksLeft.ToStringTicksToPeriod(true, false, true, true, false)));
					if (this.CurrentTempProgressSpeedFactor != 1f)
					{
						stringBuilder.AppendLine("FermentationBarrelOutOfIdealTemperature".Translate(this.CurrentTempProgressSpeedFactor.ToStringPercent()));
					}
				}
			}
			stringBuilder.AppendLine("Temperature".Translate() + ": " + base.AmbientTemperature.ToStringTemperature("F0"));
			stringBuilder.AppendLine("IdealFermentingTemperature".Translate() + ": " + 7f.ToStringTemperature("F0") + " ~ " + comp.Props.maxSafeTemperature.ToStringTemperature("F0"));
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600ED91 RID: 60817 RVA: 0x0044EBBD File Offset: 0x0044CDBD
		public Thing TakeOutBeer()
		{
			if (!this.Fermented)
			{
				Log.Warning("Tried to get beer but it's not yet fermented.");
				return null;
			}
			Thing thing = ThingMaker.MakeThing(ThingDefOf.Beer, null);
			thing.stackCount = this.wortCount;
			this.Reset();
			return thing;
		}

		// Token: 0x0600ED92 RID: 60818 RVA: 0x0044EBF0 File Offset: 0x0044CDF0
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			if (this.Empty)
			{
				return;
			}
			Vector3 center = drawLoc;
			center.y += 0.03658537f;
			center.z += 0.25f;
			GenDraw.DrawFillableBar(new GenDraw.FillableBarRequest
			{
				center = center,
				size = Building_FermentingBarrel.BarSize,
				fillPercent = (float)this.wortCount / 25f,
				filledMat = this.BarFilledMat,
				unfilledMat = Building_FermentingBarrel.BarUnfilledMat,
				margin = 0.1f,
				rotation = Rot4.North
			});
		}

		// Token: 0x0600ED93 RID: 60819 RVA: 0x0044EC97 File Offset: 0x0044CE97
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.ShowDevGizmos)
			{
				if (!this.Empty)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Set progress to 1",
						action = delegate()
						{
							this.Progress = 1f;
						}
					};
				}
				if (this.SpaceLeftForWort > 0)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Fill",
						action = delegate()
						{
							this.Progress = 1f;
							this.wortCount = 25;
						}
					};
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x04009D31 RID: 40241
		private int wortCount;

		// Token: 0x04009D32 RID: 40242
		private float progressInt;

		// Token: 0x04009D33 RID: 40243
		private Material barFilledCachedMat;

		// Token: 0x04009D34 RID: 40244
		public const int MaxCapacity = 25;

		// Token: 0x04009D35 RID: 40245
		private const int BaseFermentationDuration = 360000;

		// Token: 0x04009D36 RID: 40246
		public const float MinIdealTemperature = 7f;

		// Token: 0x04009D37 RID: 40247
		private static readonly Vector2 BarSize = new Vector2(0.55f, 0.1f);

		// Token: 0x04009D38 RID: 40248
		private static readonly Color BarZeroProgressColor = new Color(0.4f, 0.27f, 0.22f);

		// Token: 0x04009D39 RID: 40249
		private static readonly Color BarFermentedColor = new Color(0.9f, 0.85f, 0.2f);

		// Token: 0x04009D3A RID: 40250
		private static readonly Material BarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f), false);
	}
}
