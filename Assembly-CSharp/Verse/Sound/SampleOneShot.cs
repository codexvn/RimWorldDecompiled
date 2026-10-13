using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000E31 RID: 3633
	public class SampleOneShot : Sample
	{
		// Token: 0x17000EB4 RID: 3764
		// (get) Token: 0x06005BAE RID: 23470 RVA: 0x001D51C6 File Offset: 0x001D33C6
		public override float ParentStartRealTime
		{
			get
			{
				return this.startRealTime;
			}
		}

		// Token: 0x17000EB5 RID: 3765
		// (get) Token: 0x06005BAF RID: 23471 RVA: 0x001D51CE File Offset: 0x001D33CE
		public override float ParentStartTick
		{
			get
			{
				return (float)this.startTick;
			}
		}

		// Token: 0x17000EB6 RID: 3766
		// (get) Token: 0x06005BB0 RID: 23472 RVA: 0x001D51D7 File Offset: 0x001D33D7
		public override float ParentHashCode
		{
			get
			{
				return (float)this.GetHashCode();
			}
		}

		// Token: 0x17000EB7 RID: 3767
		// (get) Token: 0x06005BB1 RID: 23473 RVA: 0x001D51E0 File Offset: 0x001D33E0
		public override SoundParams ExternalParams
		{
			get
			{
				return this.externalParams;
			}
		}

		// Token: 0x17000EB8 RID: 3768
		// (get) Token: 0x06005BB2 RID: 23474 RVA: 0x001D51E8 File Offset: 0x001D33E8
		public override SoundInfo Info
		{
			get
			{
				return this.info;
			}
		}

		// Token: 0x06005BB3 RID: 23475 RVA: 0x001D51F0 File Offset: 0x001D33F0
		private SampleOneShot(SubSoundDef def) : base(def)
		{
		}

		// Token: 0x06005BB4 RID: 23476 RVA: 0x001D5204 File Offset: 0x001D3404
		public static SampleOneShot TryMakeAndPlay(SubSoundDef def, AudioClip clip, SoundInfo info)
		{
			if ((double)info.pitchFactor <= 0.0001)
			{
				Log.ErrorOnce(string.Format("Played sound with pitchFactor {0}: {1}, {2}", info.pitchFactor, def, info), 632321);
				return null;
			}
			SampleOneShot sampleOneShot = new SampleOneShot(def)
			{
				info = info,
				source = Find.SoundRoot.sourcePool.GetSource(def.onCamera)
			};
			if (sampleOneShot.source == null)
			{
				return null;
			}
			sampleOneShot.source.clip = clip;
			sampleOneShot.source.volume = sampleOneShot.SanitizedVolume;
			sampleOneShot.source.pitch = sampleOneShot.SanitizedPitch;
			sampleOneShot.source.minDistance = sampleOneShot.subDef.distRange.TrueMin;
			sampleOneShot.source.maxDistance = sampleOneShot.subDef.distRange.TrueMax;
			if (!def.onCamera)
			{
				sampleOneShot.source.gameObject.transform.position = info.Maker.Cell.ToVector3ShiftedWithAltitude(0f);
				sampleOneShot.source.minDistance = def.distRange.TrueMin;
				sampleOneShot.source.maxDistance = def.distRange.TrueMax;
				sampleOneShot.source.spatialBlend = 1f;
				if (def.canVacuumDampen)
				{
					Map map = info.Maker.Map;
					bool flag;
					if (map == null)
					{
						flag = false;
					}
					else
					{
						BiomeDef biome = map.Biome;
						bool? flag2 = (biome != null) ? new bool?(biome.inVacuum) : null;
						bool flag3 = true;
						flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
					}
					if (flag)
					{
						float num = EasingFunctions.EaseOutCubic(info.Maker.Cell.GetVacuum(info.Maker.Map));
						sampleOneShot.source.volume *= Mathf.Lerp(1f, 0.05f, num);
						SampleOneShot.VacuumHighPassFilter.cutoffFrequency = SampleOneShot.VacuumFrequencyCutoffCurve.Evaluate(num);
						SampleOneShot.VacuumHighPassFilter.SetupOn(sampleOneShot.source);
					}
				}
			}
			else
			{
				sampleOneShot.source.spatialBlend = 0f;
			}
			for (int i = 0; i < def.filters.Count; i++)
			{
				def.filters[i].SetupOn(sampleOneShot.source);
			}
			foreach (KeyValuePair<string, float> keyValuePair in info.DefinedParameters)
			{
				sampleOneShot.externalParams[keyValuePair.Key] = keyValuePair.Value;
			}
			sampleOneShot.Update();
			sampleOneShot.source.Play();
			Find.SoundRoot.oneShotManager.TryAddPlayingOneShot(sampleOneShot);
			return sampleOneShot;
		}

		// Token: 0x0400427C RID: 17020
		public SoundInfo info;

		// Token: 0x0400427D RID: 17021
		private const float MaxVacuumVolumePercent = 0.05f;

		// Token: 0x0400427E RID: 17022
		private SoundParams externalParams = new SoundParams();

		// Token: 0x0400427F RID: 17023
		private static readonly SimpleCurve VacuumFrequencyCutoffCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 22000f),
				true
			},
			{
				new CurvePoint(0.25f, 4000f),
				true
			},
			{
				new CurvePoint(1f, 400f),
				true
			}
		};

		// Token: 0x04004280 RID: 17024
		private static readonly SoundFilterLowPass VacuumHighPassFilter = new SoundFilterLowPass
		{
			lowpassResonaceQ = 0f
		};
	}
}
