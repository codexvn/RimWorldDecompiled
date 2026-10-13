using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A8B RID: 10891
	public class Blight : Thing
	{
		// Token: 0x170025E0 RID: 9696
		// (get) Token: 0x0600F26C RID: 62060 RVA: 0x004665E1 File Offset: 0x004647E1
		// (set) Token: 0x0600F26D RID: 62061 RVA: 0x004665E9 File Offset: 0x004647E9
		public float Severity
		{
			get
			{
				return this.severity;
			}
			set
			{
				this.severity = Mathf.Clamp01(value);
			}
		}

		// Token: 0x170025E1 RID: 9697
		// (get) Token: 0x0600F26E RID: 62062 RVA: 0x004665F7 File Offset: 0x004647F7
		public Plant Plant
		{
			get
			{
				if (!base.Spawned)
				{
					return null;
				}
				return BlightUtility.GetFirstBlightableEverPlant(base.Position, base.Map);
			}
		}

		// Token: 0x170025E2 RID: 9698
		// (get) Token: 0x0600F26F RID: 62063 RVA: 0x00466614 File Offset: 0x00464814
		protected float ReproduceMTBHours
		{
			get
			{
				if (this.severity < 0.28f)
				{
					return -1f;
				}
				return GenMath.LerpDouble(0.28f, 1f, 16.8f, 2.1f, this.severity);
			}
		}

		// Token: 0x0600F270 RID: 62064 RVA: 0x00466648 File Offset: 0x00464848
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.severity, "severity", 0f, false);
			Scribe_Values.Look<int>(ref this.lastPlantHarmTick, "lastPlantHarmTick", 0, false);
		}

		// Token: 0x0600F271 RID: 62065 RVA: 0x00466678 File Offset: 0x00464878
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad && !base.BeingTransportedOnGravship)
			{
				this.lastPlantHarmTick = Find.TickManager.TicksGame;
			}
			this.lastMapMeshUpdateSeverity = this.Severity;
		}

		// Token: 0x0600F272 RID: 62066 RVA: 0x004666AC File Offset: 0x004648AC
		public override void TickLong()
		{
			this.CheckHarmPlant();
			if (this.DestroyIfNoPlantHere())
			{
				return;
			}
			this.Severity += 0.033333335f;
			float reproduceMTBHours = this.ReproduceMTBHours;
			if (reproduceMTBHours > 0f && Rand.MTBEventOccurs(reproduceMTBHours, 2500f, 2000f))
			{
				this.TryReproduceNow();
			}
			if (Mathf.Abs(this.Severity - this.lastMapMeshUpdateSeverity) >= 0.05f)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Things);
				this.lastMapMeshUpdateSeverity = this.Severity;
			}
		}

		// Token: 0x0600F273 RID: 62067 RVA: 0x00466746 File Offset: 0x00464946
		public void Notify_PlantDeSpawned()
		{
			this.DestroyIfNoPlantHere();
		}

		// Token: 0x0600F274 RID: 62068 RVA: 0x0046674F File Offset: 0x0046494F
		private bool DestroyIfNoPlantHere()
		{
			if (base.Destroyed)
			{
				return true;
			}
			if (this.Plant == null)
			{
				this.Destroy(DestroyMode.Vanish);
				return true;
			}
			return false;
		}

		// Token: 0x0600F275 RID: 62069 RVA: 0x00466770 File Offset: 0x00464970
		private void CheckHarmPlant()
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (ticksGame - this.lastPlantHarmTick >= 60000)
			{
				List<Thing> thingList = base.Position.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Plant plant = thingList[i] as Plant;
					if (plant != null)
					{
						this.HarmPlant(plant);
					}
				}
				this.lastPlantHarmTick = ticksGame;
			}
		}

		// Token: 0x0600F276 RID: 62070 RVA: 0x004667D8 File Offset: 0x004649D8
		private void HarmPlant(Plant plant)
		{
			bool isCrop = plant.IsCrop;
			IntVec3 position = base.Position;
			Map map = base.Map;
			plant.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 5f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			if (plant.Destroyed && isCrop && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfBlight-" + plant.def.defName, 240f))
			{
				Messages.Message("MessagePlantDiedOfBlight".Translate(plant.Label, plant).CapitalizeFirst(), new TargetInfo(position, map, false), MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x0600F277 RID: 62071 RVA: 0x0046688F File Offset: 0x00464A8F
		public void TryReproduceNow()
		{
			GenRadial.ProcessEquidistantCells(base.Position, 4f, delegate(List<IntVec3> cells)
			{
				IntVec3 c;
				if ((from x in cells
				where BlightUtility.GetFirstBlightableNowPlant(x, base.Map) != null
				select x).TryRandomElement(out c))
				{
					BlightUtility.GetFirstBlightableNowPlant(c, base.Map).CropBlighted();
					return true;
				}
				return false;
			}, base.Map);
		}

		// Token: 0x0600F278 RID: 62072 RVA: 0x004668B4 File Offset: 0x00464AB4
		public override void Print(SectionLayer layer)
		{
			Plant plant = this.Plant;
			if (plant != null)
			{
				PlantUtility.SetWindExposureColors(Blight.workingColors, plant);
			}
			else
			{
				Blight.workingColors[0].a = (Blight.workingColors[1].a = (Blight.workingColors[2].a = (Blight.workingColors[3].a = 0)));
			}
			float num = Blight.SizeRange.LerpThroughRange(this.severity);
			if (plant != null)
			{
				float a = plant.Graphic.drawSize.x * plant.def.plant.visualSizeRange.LerpThroughRange(plant.Growth);
				num *= Mathf.Min(a, 1f);
			}
			num = Mathf.Clamp(num, 0.5f, 0.9f);
			Printer_Plane.PrintPlane(layer, this.TrueCenter(), this.def.graphic.drawSize * num, this.Graphic.MatAt(base.Rotation, this), 0f, false, null, Blight.workingColors, 0.1f, 0f);
		}

		// Token: 0x04009FB6 RID: 40886
		private float severity = 0.2f;

		// Token: 0x04009FB7 RID: 40887
		private int lastPlantHarmTick;

		// Token: 0x04009FB8 RID: 40888
		private float lastMapMeshUpdateSeverity;

		// Token: 0x04009FB9 RID: 40889
		private const float InitialSeverity = 0.2f;

		// Token: 0x04009FBA RID: 40890
		private const float SeverityPerDay = 1f;

		// Token: 0x04009FBB RID: 40891
		private const int DamagePerDay = 5;

		// Token: 0x04009FBC RID: 40892
		private const float MinSeverityToReproduce = 0.28f;

		// Token: 0x04009FBD RID: 40893
		private const float ReproduceMTBHoursAtMinSeverity = 16.8f;

		// Token: 0x04009FBE RID: 40894
		private const float ReproduceMTBHoursAtMaxSeverity = 2.1f;

		// Token: 0x04009FBF RID: 40895
		private const float ReproductionRadius = 4f;

		// Token: 0x04009FC0 RID: 40896
		private static FloatRange SizeRange = new FloatRange(0.6f, 1f);

		// Token: 0x04009FC1 RID: 40897
		private static Color32[] workingColors = new Color32[4];
	}
}
