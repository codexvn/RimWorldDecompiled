using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200027C RID: 636
	public class GameConditionDef : Def
	{
		// Token: 0x0600100D RID: 4109 RVA: 0x0004BC79 File Offset: 0x00049E79
		public bool CanCoexistWith(GameConditionDef other)
		{
			return this != other && (!ModsConfig.AnomalyActive || this == GameConditionDefOf.UnnaturalDarkness || other != GameConditionDefOf.UnnaturalDarkness) && (this.exclusiveConditions == null || !this.exclusiveConditions.Contains(other));
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x0004BCB3 File Offset: 0x00049EB3
		public static GameConditionDef Named(string defName)
		{
			return DefDatabase<GameConditionDef>.GetNamed(defName, true);
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x0004BCBC File Offset: 0x00049EBC
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.conditionClass == null)
			{
				yield return "conditionClass is null";
			}
			if (!this.layerBlacklist.NullOrEmpty<PlanetLayerDef>() && !this.layerWhitelist.NullOrEmpty<PlanetLayerDef>())
			{
				yield return "Both layerBlacklist and layerWhitelist are used, this will lead to unexpected behaviour. Use one or the other.";
			}
			yield break;
			yield break;
		}

		// Token: 0x04000C8D RID: 3213
		public Type conditionClass = typeof(GameCondition);

		// Token: 0x04000C8E RID: 3214
		private List<GameConditionDef> exclusiveConditions;

		// Token: 0x04000C8F RID: 3215
		[MustTranslate]
		public string startMessage;

		// Token: 0x04000C90 RID: 3216
		[MustTranslate]
		public string endMessage;

		// Token: 0x04000C91 RID: 3217
		[MustTranslate]
		public string letterText;

		// Token: 0x04000C92 RID: 3218
		public List<ThingDef> letterHyperlinks;

		// Token: 0x04000C93 RID: 3219
		public LetterDef letterDef;

		// Token: 0x04000C94 RID: 3220
		public bool canBePermanent;

		// Token: 0x04000C95 RID: 3221
		public bool showPermanentInTooltip = true;

		// Token: 0x04000C96 RID: 3222
		public bool allowUnderground = true;

		// Token: 0x04000C97 RID: 3223
		public bool requireFish;

		// Token: 0x04000C98 RID: 3224
		[MustTranslate]
		public string descriptionFuture;

		// Token: 0x04000C99 RID: 3225
		[NoTranslate]
		public string jumpToSourceKey = "ClickToJumpToSource";

		// Token: 0x04000C9A RID: 3226
		public List<GameConditionDef> silencedByConditions;

		// Token: 0x04000C9B RID: 3227
		public bool natural = true;

		// Token: 0x04000C9C RID: 3228
		public bool preventNeutralVisitors;

		// Token: 0x04000C9D RID: 3229
		public bool preventShuttleLaunch;

		// Token: 0x04000C9E RID: 3230
		public bool causesTraderCaravanExit;

		// Token: 0x04000C9F RID: 3231
		public bool preventIncidents;

		// Token: 0x04000CA0 RID: 3232
		public bool displayOnUI = true;

		// Token: 0x04000CA1 RID: 3233
		public bool pennedAnimalsSeekShelter;

		// Token: 0x04000CA2 RID: 3234
		public ThingDef spreadsFilth;

		// Token: 0x04000CA3 RID: 3235
		public bool canAffectAllPlanetLayers;

		// Token: 0x04000CA4 RID: 3236
		public List<PlanetLayerDef> layerWhitelist;

		// Token: 0x04000CA5 RID: 3237
		public List<PlanetLayerDef> layerBlacklist;

		// Token: 0x04000CA6 RID: 3238
		public PsychicDroneLevel defaultDroneLevel = PsychicDroneLevel.BadMedium;

		// Token: 0x04000CA7 RID: 3239
		public bool preventRain;

		// Token: 0x04000CA8 RID: 3240
		public WeatherDef weatherDef;

		// Token: 0x04000CA9 RID: 3241
		public float temperatureOffset = -10f;

		// Token: 0x04000CAA RID: 3242
		public float minNearbyPollution;

		// Token: 0x04000CAB RID: 3243
		public SimpleCurve mtbOverNearbyPollutionCurve;

		// Token: 0x04000CAC RID: 3244
		public float fishPopulationOffsetPerDay;
	}
}
