using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020021A7 RID: 8615
	public sealed class WeatherManager : IExposable
	{
		// Token: 0x17001C9E RID: 7326
		// (get) Token: 0x0600BC3F RID: 48191 RVA: 0x00368D40 File Offset: 0x00366F40
		public float TransitionLerpFactor
		{
			get
			{
				float num = (float)this.curWeatherAge / 4000f;
				if (num > 1f)
				{
					num = 1f;
				}
				return num;
			}
		}

		// Token: 0x17001C9F RID: 7327
		// (get) Token: 0x0600BC40 RID: 48192 RVA: 0x00368D6A File Offset: 0x00366F6A
		public float RainRate
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.rainRate, this.curWeather.rainRate, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17001CA0 RID: 7328
		// (get) Token: 0x0600BC41 RID: 48193 RVA: 0x00368D8D File Offset: 0x00366F8D
		public float SnowRate
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.snowRate, this.curWeather.snowRate, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17001CA1 RID: 7329
		// (get) Token: 0x0600BC42 RID: 48194 RVA: 0x00368DB0 File Offset: 0x00366FB0
		public float SandRate
		{
			get
			{
				if (!ModsConfig.OdysseyActive)
				{
					return 0f;
				}
				return Mathf.Lerp(this.lastWeather.sandRate, this.curWeather.sandRate, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17001CA2 RID: 7330
		// (get) Token: 0x0600BC43 RID: 48195 RVA: 0x00368DE0 File Offset: 0x00366FE0
		public float CurWindSpeedFactor
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.windSpeedFactor, this.curWeather.windSpeedFactor, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17001CA3 RID: 7331
		// (get) Token: 0x0600BC44 RID: 48196 RVA: 0x00368E03 File Offset: 0x00367003
		public float CurWindSpeedOffset
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.windSpeedOffset, this.curWeather.windSpeedOffset, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17001CA4 RID: 7332
		// (get) Token: 0x0600BC45 RID: 48197 RVA: 0x00368E26 File Offset: 0x00367026
		public float CurMoveSpeedMultiplier
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.moveSpeedMultiplier, this.curWeather.moveSpeedMultiplier, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17001CA5 RID: 7333
		// (get) Token: 0x0600BC46 RID: 48198 RVA: 0x00368E49 File Offset: 0x00367049
		public float CurWeatherAccuracyMultiplier
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.accuracyMultiplier, this.curWeather.accuracyMultiplier, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17001CA6 RID: 7334
		// (get) Token: 0x0600BC47 RID: 48199 RVA: 0x00368E6C File Offset: 0x0036706C
		public float CurWeatherMaxRangeCap
		{
			get
			{
				return this.curWeather.maxRangeCap;
			}
		}

		// Token: 0x17001CA7 RID: 7335
		// (get) Token: 0x0600BC48 RID: 48200 RVA: 0x00368E7C File Offset: 0x0036707C
		public WeatherDef CurWeatherPerceived
		{
			get
			{
				if (this.curWeather == null)
				{
					return this.lastWeather;
				}
				if (this.lastWeather == null)
				{
					return this.curWeather;
				}
				float num;
				if (this.curWeather.perceivePriority > this.lastWeather.perceivePriority)
				{
					num = 0.18f;
				}
				else if (this.lastWeather.perceivePriority > this.curWeather.perceivePriority)
				{
					num = 0.82f;
				}
				else
				{
					num = 0.5f;
				}
				if (this.TransitionLerpFactor >= num)
				{
					return this.curWeather;
				}
				return this.lastWeather;
			}
		}

		// Token: 0x17001CA8 RID: 7336
		// (get) Token: 0x0600BC49 RID: 48201 RVA: 0x00368F03 File Offset: 0x00367103
		public WeatherDef CurWeatherLerped
		{
			get
			{
				if (this.curWeather == null)
				{
					return this.lastWeather;
				}
				if (this.lastWeather == null)
				{
					return this.curWeather;
				}
				if (this.TransitionLerpFactor >= 0.5f)
				{
					return this.curWeather;
				}
				return this.lastWeather;
			}
		}

		// Token: 0x0600BC4A RID: 48202 RVA: 0x00368F40 File Offset: 0x00367140
		public WeatherManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600BC4B RID: 48203 RVA: 0x00368F9C File Offset: 0x0036719C
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.curWeatherAge, "curWeatherAge", 0, true);
			Scribe_Values.Look<float>(ref this.prevSkyTargetLerp, "prevSkyTargetLerp", -1f, false);
			Scribe_Values.Look<float>(ref this.currSkyTargetLerp, "currSkyTargetLerp", -1f, false);
			Scribe_Defs.Look<WeatherDef>(ref this.curWeather, "curWeather");
			Scribe_Defs.Look<WeatherDef>(ref this.lastWeather, "lastWeather");
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.ambienceSustainers.Clear();
			}
		}

		// Token: 0x0600BC4C RID: 48204 RVA: 0x0036901C File Offset: 0x0036721C
		public void TransitionTo(WeatherDef newWeather)
		{
			this.lastWeather = this.curWeather;
			this.curWeather = newWeather;
			this.lastWeather.Worker.OnWeatherEnd(this.map);
			this.ResetSkyTargetLerpCache();
			this.curWeatherAge = 0;
			if (this.lastWeather != newWeather)
			{
				newWeather.Worker.OnWeatherStart(this.map);
			}
		}

		// Token: 0x0600BC4D RID: 48205 RVA: 0x0036907C File Offset: 0x0036727C
		public void DoWeatherGUI(Rect rect)
		{
			WeatherDef curWeatherPerceived = this.CurWeatherPerceived;
			Text.Anchor = TextAnchor.MiddleRight;
			Rect rect2 = new Rect(rect);
			rect2.width -= 15f;
			Text.Font = GameFont.Small;
			Widgets.Label(rect2, curWeatherPerceived.LabelCap);
			if (!curWeatherPerceived.description.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, curWeatherPerceived.description);
			}
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600BC4E RID: 48206 RVA: 0x003690E8 File Offset: 0x003672E8
		public void WeatherManagerTick()
		{
			this.eventHandler.WeatherEventHandlerTick();
			this.curWeatherAge++;
			this.curWeather.Worker.WeatherTick(this.map, this.TransitionLerpFactor);
			if (this.TransitionLerpFactor < 1f || !this.tickedLastWeather)
			{
				this.lastWeather.Worker.WeatherTick(this.map, 1f - this.TransitionLerpFactor);
				this.tickedLastWeather = true;
			}
			this.AmbientSoundsTick();
		}

		// Token: 0x0600BC4F RID: 48207 RVA: 0x00369170 File Offset: 0x00367370
		private void AmbientSoundsTick()
		{
			foreach (SoundDef soundDef in this.curWeather.ambientSounds)
			{
				if (!this.IsAmbienceSoundPlaying(soundDef) && this.VolumeOfAmbientSound(soundDef) > 0.0001f)
				{
					SoundInfo info = SoundInfo.OnCamera(MaintenanceType.None);
					Sustainer sustainer = soundDef.TrySpawnSustainer(info);
					if (sustainer != null)
					{
						this.ambienceSustainers[sustainer.def] = sustainer;
					}
				}
			}
		}

		// Token: 0x0600BC50 RID: 48208 RVA: 0x003691FC File Offset: 0x003673FC
		private bool IsAmbienceSoundPlaying(SoundDef sound)
		{
			return this.ambienceSustainers.ContainsKey(sound);
		}

		// Token: 0x0600BC51 RID: 48209 RVA: 0x0036920A File Offset: 0x0036740A
		public void WeatherManagerUpdate()
		{
			this.SetAmbienceSustainersVolume();
		}

		// Token: 0x0600BC52 RID: 48210 RVA: 0x00369214 File Offset: 0x00367414
		public void EndAllSustainers()
		{
			foreach (KeyValuePair<SoundDef, Sustainer> keyValuePair in this.ambienceSustainers)
			{
				SoundDef soundDef;
				Sustainer sustainer;
				keyValuePair.Deconstruct(out soundDef, out sustainer);
				sustainer.End();
			}
			this.ambienceSustainers.Clear();
		}

		// Token: 0x0600BC53 RID: 48211 RVA: 0x0036927C File Offset: 0x0036747C
		public void ResetSkyTargetLerpCache()
		{
			this.prevSkyTargetLerp = -1f;
			this.currSkyTargetLerp = -1f;
		}

		// Token: 0x0600BC54 RID: 48212 RVA: 0x00369294 File Offset: 0x00367494
		private void SetAmbienceSustainersVolume()
		{
			WeatherManager.TmpToRemove.Clear();
			foreach (KeyValuePair<SoundDef, Sustainer> keyValuePair in this.ambienceSustainers)
			{
				SoundDef soundDef;
				Sustainer sustainer;
				keyValuePair.Deconstruct(out soundDef, out sustainer);
				SoundDef soundDef2 = soundDef;
				Sustainer sustainer2 = sustainer;
				float num = this.VolumeOfAmbientSound(soundDef2);
				if (num > 0.0001f)
				{
					sustainer2.externalParams["LerpFactor"] = num;
				}
				else
				{
					sustainer2.End();
					WeatherManager.TmpToRemove.Add(soundDef2);
				}
			}
			foreach (SoundDef key in WeatherManager.TmpToRemove)
			{
				this.ambienceSustainers.Remove(key);
			}
			WeatherManager.TmpToRemove.Clear();
		}

		// Token: 0x0600BC55 RID: 48213 RVA: 0x00369388 File Offset: 0x00367588
		private float VolumeOfAmbientSound(SoundDef soundDef)
		{
			if (this.map != Find.CurrentMap)
			{
				return 0f;
			}
			for (int i = 0; i < Find.WindowStack.Count; i++)
			{
				if (Find.WindowStack[i].silenceAmbientSound)
				{
					return 0f;
				}
			}
			float num = 0f;
			using (List<SoundDef>.Enumerator enumerator = this.lastWeather.ambientSounds.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == soundDef)
					{
						num += 1f - this.TransitionLerpFactor;
					}
				}
			}
			using (List<SoundDef>.Enumerator enumerator = this.curWeather.ambientSounds.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == soundDef)
					{
						num += this.TransitionLerpFactor;
					}
				}
			}
			return num;
		}

		// Token: 0x0600BC56 RID: 48214 RVA: 0x0036947C File Offset: 0x0036767C
		public void DrawAllWeather()
		{
			this.eventHandler.WeatherEventsDraw();
			this.lastWeather.Worker.DrawWeather(this.map);
			this.curWeather.Worker.DrawWeather(this.map);
		}

		// Token: 0x040081A0 RID: 33184
		public Map map;

		// Token: 0x040081A1 RID: 33185
		public readonly WeatherEventHandler eventHandler = new WeatherEventHandler();

		// Token: 0x040081A2 RID: 33186
		public WeatherDef curWeather = WeatherDefOf.Clear;

		// Token: 0x040081A3 RID: 33187
		public WeatherDef lastWeather = WeatherDefOf.Clear;

		// Token: 0x040081A4 RID: 33188
		public float prevSkyTargetLerp = -1f;

		// Token: 0x040081A5 RID: 33189
		public float currSkyTargetLerp = -1f;

		// Token: 0x040081A6 RID: 33190
		public int curWeatherAge;

		// Token: 0x040081A7 RID: 33191
		private readonly Dictionary<SoundDef, Sustainer> ambienceSustainers = new Dictionary<SoundDef, Sustainer>();

		// Token: 0x040081A8 RID: 33192
		public const float TransitionTicks = 4000f;

		// Token: 0x040081A9 RID: 33193
		private bool tickedLastWeather;

		// Token: 0x040081AA RID: 33194
		private static readonly HashSet<SoundDef> TmpToRemove = new HashSet<SoundDef>();
	}
}
