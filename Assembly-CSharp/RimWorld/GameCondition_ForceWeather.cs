using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001D2C RID: 7468
	public class GameCondition_ForceWeather : GameCondition
	{
		// Token: 0x0600A74D RID: 42829 RVA: 0x002FF71E File Offset: 0x002FD91E
		public override void Init()
		{
			base.Init();
			if (this.weather == null)
			{
				this.weather = this.def.weatherDef;
			}
		}

		// Token: 0x0600A74E RID: 42830 RVA: 0x002FF73F File Offset: 0x002FD93F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<WeatherDef>(ref this.weather, "weather");
		}

		// Token: 0x0600A74F RID: 42831 RVA: 0x002FF757 File Offset: 0x002FD957
		public override WeatherDef ForcedWeather()
		{
			return this.weather;
		}

		// Token: 0x0600A750 RID: 42832 RVA: 0x002FF760 File Offset: 0x002FD960
		public override void RandomizeSettings(float points, Map map, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.RandomizeSettings(points, map, outExtraDescriptionRules, outExtraDescriptionConstants);
			this.weather = (from def in DefDatabase<WeatherDef>.AllDefsListForReading
			where def.isBad && def.canOccurAsRandomForcedEvent
			select def).RandomElement<WeatherDef>();
			outExtraDescriptionRules.AddRange(GrammarUtility.RulesForDef("forcedWeather", this.weather));
		}

		// Token: 0x040073E3 RID: 29667
		public WeatherDef weather;
	}
}
