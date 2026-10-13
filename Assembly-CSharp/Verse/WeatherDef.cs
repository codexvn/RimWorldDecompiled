using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000315 RID: 789
	public class WeatherDef : Def
	{
		// Token: 0x170003CE RID: 974
		// (get) Token: 0x060013B1 RID: 5041 RVA: 0x0005D526 File Offset: 0x0005B726
		public WeatherWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (WeatherWorker)Activator.CreateInstance(this.workerClass, new object[]
					{
						this
					});
				}
				return this.workerInt;
			}
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x0005D556 File Offset: 0x0005B756
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.skyColorsDay.saturation == 0f || this.skyColorsDusk.saturation == 0f || this.skyColorsNightMid.saturation == 0f || this.skyColorsNightEdge.saturation == 0f)
			{
				yield return "a sky color has saturation of 0";
			}
			yield break;
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x0005D566 File Offset: 0x0005B766
		public static WeatherDef Named(string defName)
		{
			return DefDatabase<WeatherDef>.GetNamed(defName, true);
		}

		// Token: 0x0400128E RID: 4750
		public IntRange durationRange = new IntRange(16000, 160000);

		// Token: 0x0400128F RID: 4751
		public bool repeatable;

		// Token: 0x04001290 RID: 4752
		public bool isBad;

		// Token: 0x04001291 RID: 4753
		public bool canOccurAsRandomForcedEvent = true;

		// Token: 0x04001292 RID: 4754
		public Favorability favorability = Favorability.Neutral;

		// Token: 0x04001293 RID: 4755
		public FloatRange temperatureRange = new FloatRange(-999f, 999f);

		// Token: 0x04001294 RID: 4756
		public SimpleCurve commonalityRainfallFactor;

		// Token: 0x04001295 RID: 4757
		public int transitionTicksOverride = int.MaxValue;

		// Token: 0x04001296 RID: 4758
		public int minMonolithLevel;

		// Token: 0x04001297 RID: 4759
		public bool canOccurInAmbientHorror;

		// Token: 0x04001298 RID: 4760
		[MustTranslate]
		public string letterText;

		// Token: 0x04001299 RID: 4761
		[MustTranslate]
		public string letterLabel;

		// Token: 0x0400129A RID: 4762
		public LetterDef letterDef;

		// Token: 0x0400129B RID: 4763
		public float rainRate;

		// Token: 0x0400129C RID: 4764
		public float snowRate;

		// Token: 0x0400129D RID: 4765
		public float sandRate;

		// Token: 0x0400129E RID: 4766
		public float windSpeedFactor = 1f;

		// Token: 0x0400129F RID: 4767
		public float windSpeedOffset;

		// Token: 0x040012A0 RID: 4768
		public float moveSpeedMultiplier = 1f;

		// Token: 0x040012A1 RID: 4769
		public float accuracyMultiplier = 1f;

		// Token: 0x040012A2 RID: 4770
		public float maxRangeCap = -1f;

		// Token: 0x040012A3 RID: 4771
		public float perceivePriority;

		// Token: 0x040012A4 RID: 4772
		public bool doToxicBuildup;

		// Token: 0x040012A5 RID: 4773
		public ThoughtDef weatherThought;

		// Token: 0x040012A6 RID: 4774
		public float maxGlow = 1f;

		// Token: 0x040012A7 RID: 4775
		public bool preventSkygaze;

		// Token: 0x040012A8 RID: 4776
		public bool preventsShuttleLaunch;

		// Token: 0x040012A9 RID: 4777
		public List<SoundDef> ambientSounds = new List<SoundDef>();

		// Token: 0x040012AA RID: 4778
		public List<WeatherEventMaker> eventMakers = new List<WeatherEventMaker>();

		// Token: 0x040012AB RID: 4779
		public List<Type> overlayClasses = new List<Type>();

		// Token: 0x040012AC RID: 4780
		public SkyColorSet skyColorsNightMid;

		// Token: 0x040012AD RID: 4781
		public SkyColorSet skyColorsNightEdge;

		// Token: 0x040012AE RID: 4782
		public SkyColorSet skyColorsDay;

		// Token: 0x040012AF RID: 4783
		public SkyColorSet skyColorsDusk;

		// Token: 0x040012B0 RID: 4784
		public Type workerClass = typeof(WeatherWorker);

		// Token: 0x040012B1 RID: 4785
		[Unsaved(false)]
		private WeatherWorker workerInt;
	}
}
