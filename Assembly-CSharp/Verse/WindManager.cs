using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse.Noise;

namespace Verse
{
	// Token: 0x02000543 RID: 1347
	public class WindManager
	{
		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x060022F2 RID: 8946 RVA: 0x000B6344 File Offset: 0x000B4544
		public float WindSpeed
		{
			get
			{
				return this.cachedWindSpeed;
			}
		}

		// Token: 0x060022F3 RID: 8947 RVA: 0x000B634C File Offset: 0x000B454C
		public WindManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x060022F4 RID: 8948 RVA: 0x000B6368 File Offset: 0x000B4568
		public void WindManagerTick()
		{
			this.cachedWindSpeed = this.BaseWindSpeedAt(Find.TickManager.TicksAbs) * this.map.weatherManager.CurWindSpeedFactor;
			float num = this.map.weatherManager.CurWindSpeedOffset;
			this.tempAllGameConditionsAffectingMap.Clear();
			this.map.gameConditionManager.GetAllGameConditionsAffectingMap(this.map, this.tempAllGameConditionsAffectingMap);
			for (int i = 0; i < this.tempAllGameConditionsAffectingMap.Count; i++)
			{
				float num2 = this.tempAllGameConditionsAffectingMap[i].MinWindSpeed();
				if (num2 > num)
				{
					num = num2;
				}
			}
			if (num > 0f)
			{
				FloatRange floatRange = WindManager.WindSpeedRange * this.map.weatherManager.CurWindSpeedFactor;
				float num3 = (this.cachedWindSpeed - floatRange.min) / (floatRange.max - floatRange.min) * (floatRange.max - num);
				this.cachedWindSpeed = num + num3;
			}
			List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.WindSource);
			for (int j = 0; j < list.Count; j++)
			{
				CompWindSource compWindSource = list[j].TryGetComp<CompWindSource>();
				this.cachedWindSpeed = Mathf.Max(this.cachedWindSpeed, compWindSource.wind);
			}
			if (Prefs.PlantWindSway)
			{
				this.plantSwayHead += Mathf.Min(this.WindSpeed, 1f);
			}
			else
			{
				this.plantSwayHead = 0f;
			}
			if (Find.CurrentMap == this.map)
			{
				for (int k = 0; k < WindManager.plantMaterials.Count; k++)
				{
					WindManager.plantMaterials[k].SetFloat(ShaderPropertyIDs.SwayHead, this.plantSwayHead);
				}
			}
		}

		// Token: 0x060022F5 RID: 8949 RVA: 0x000B651C File Offset: 0x000B471C
		public static void Notify_PlantMaterialCreated(Material newMat)
		{
			WindManager.plantMaterials.Add(newMat);
		}

		// Token: 0x060022F6 RID: 8950 RVA: 0x000B652C File Offset: 0x000B472C
		private float BaseWindSpeedAt(int ticksAbs)
		{
			if (this.windNoise == null)
			{
				int seed = Gen.HashCombineInt(this.map.GetHashCode(), 122049541) ^ Find.World.info.Seed;
				this.windNoise = new Perlin(3.9999998989515007E-05, 2.0, 0.5, 4, seed, QualityMode.Medium);
				this.windNoise = new ScaleBias(1.5, 0.5, this.windNoise);
				this.windNoise = new Clamp((double)WindManager.WindSpeedRange.min, (double)WindManager.WindSpeedRange.max, this.windNoise);
			}
			return (float)this.windNoise.GetValue((double)ticksAbs, 0.0, 0.0);
		}

		// Token: 0x060022F7 RID: 8951 RVA: 0x000B6600 File Offset: 0x000B4800
		public string DebugString()
		{
			return "WindSpeed: " + this.WindSpeed.ToString() + "\nplantSwayHead: " + this.plantSwayHead.ToString();
		}

		// Token: 0x060022F8 RID: 8952 RVA: 0x000B6638 File Offset: 0x000B4838
		public void LogWindSpeeds()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Upcoming wind speeds:");
			for (int i = 0; i < 72; i++)
			{
				stringBuilder.AppendLine("Hour " + i.ToString() + " - " + this.BaseWindSpeedAt(Find.TickManager.TicksAbs + 2500 * i).ToString("F2"));
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04001D3C RID: 7484
		private static readonly FloatRange WindSpeedRange = new FloatRange(0.04f, 2f);

		// Token: 0x04001D3D RID: 7485
		private Map map;

		// Token: 0x04001D3E RID: 7486
		private static List<Material> plantMaterials = new List<Material>();

		// Token: 0x04001D3F RID: 7487
		private float cachedWindSpeed;

		// Token: 0x04001D40 RID: 7488
		private ModuleBase windNoise;

		// Token: 0x04001D41 RID: 7489
		private float plantSwayHead;

		// Token: 0x04001D42 RID: 7490
		private List<GameCondition> tempAllGameConditionsAffectingMap = new List<GameCondition>();
	}
}
