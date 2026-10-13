using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BB7 RID: 11191
	public class CompCauseGameCondition_ForceWeather : CompCauseGameCondition
	{
		// Token: 0x0600FA40 RID: 64064 RVA: 0x0048BF5B File Offset: 0x0048A15B
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.weather = base.Props.conditionDef.weatherDef;
		}

		// Token: 0x0600FA41 RID: 64065 RVA: 0x0048BF7A File Offset: 0x0048A17A
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<WeatherDef>(ref this.weather, "weather");
		}

		// Token: 0x0600FA42 RID: 64066 RVA: 0x0048BF92 File Offset: 0x0048A192
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode || !DebugSettings.godMode)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = this.weather.LabelCap,
				action = delegate()
				{
					List<WeatherDef> allDefsListForReading = DefDatabase<WeatherDef>.AllDefsListForReading;
					int num = allDefsListForReading.FindIndex((WeatherDef w) => w == this.weather);
					num++;
					if (num >= allDefsListForReading.Count)
					{
						num = 0;
					}
					this.weather = allDefsListForReading[num];
					base.ReSetupAllConditions();
				},
				hotKey = KeyBindingDefOf.Misc1
			};
			yield break;
		}

		// Token: 0x0600FA43 RID: 64067 RVA: 0x0048BFA2 File Offset: 0x0048A1A2
		protected override void SetupCondition(GameCondition condition, Map map)
		{
			base.SetupCondition(condition, map);
			((GameCondition_ForceWeather)condition).weather = this.weather;
		}

		// Token: 0x0600FA44 RID: 64068 RVA: 0x0048BFC0 File Offset: 0x0048A1C0
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + "Weather".Translate() + ": " + this.weather.LabelCap;
		}

		// Token: 0x0600FA45 RID: 64069 RVA: 0x0048C017 File Offset: 0x0048A217
		public override void RandomizeSettings(Site site)
		{
			this.weather = (from x in DefDatabase<WeatherDef>.AllDefsListForReading
			where x.isBad && x.canOccurAsRandomForcedEvent
			select x).RandomElement<WeatherDef>();
		}

		// Token: 0x0400A473 RID: 42099
		public WeatherDef weather;
	}
}
