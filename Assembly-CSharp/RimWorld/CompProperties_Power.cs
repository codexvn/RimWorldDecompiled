using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001978 RID: 6520
	public class CompProperties_Power : CompProperties
	{
		// Token: 0x17001675 RID: 5749
		// (get) Token: 0x060093F7 RID: 37879 RVA: 0x002B7F94 File Offset: 0x002B6194
		public float PowerConsumption
		{
			get
			{
				float num = this.basePowerConsumption;
				if (Current.ProgramState == ProgramState.Entry)
				{
					return num;
				}
				int num2 = 0;
				for (;;)
				{
					int num3 = num2;
					List<CompProperties_Power.PowerUpgrade> list = this.powerUpgrades;
					int? num4 = (list != null) ? new int?(list.Count) : null;
					if (!(num3 < num4.GetValueOrDefault() & num4 != null))
					{
						break;
					}
					ResearchProjectDef researchProject = this.powerUpgrades[num2].researchProject;
					if (researchProject != null && researchProject.IsFinished)
					{
						num *= this.powerUpgrades[num2].factor;
					}
					num2++;
				}
				return num;
			}
		}

		// Token: 0x060093F8 RID: 37880 RVA: 0x002B8021 File Offset: 0x002B6221
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.basePowerConsumption > 0f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "PowerConsumption".Translate(), this.PowerConsumption.ToString("F0") + " W", "Stat_Thing_PowerConsumption_Desc".Translate(), 5000, null, null, false, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x04005F80 RID: 24448
		public bool transmitsPower;

		// Token: 0x04005F81 RID: 24449
		private float basePowerConsumption;

		// Token: 0x04005F82 RID: 24450
		public float idlePowerDraw = -1f;

		// Token: 0x04005F83 RID: 24451
		public bool shortCircuitInRain;

		// Token: 0x04005F84 RID: 24452
		public bool showPowerNeededIfOff = true;

		// Token: 0x04005F85 RID: 24453
		public SoundDef soundPowerOn;

		// Token: 0x04005F86 RID: 24454
		public SoundDef soundPowerOff;

		// Token: 0x04005F87 RID: 24455
		public SoundDef soundAmbientPowered;

		// Token: 0x04005F88 RID: 24456
		public SoundDef soundAmbientProducingPower;

		// Token: 0x04005F89 RID: 24457
		public List<CompProperties_Power.PowerUpgrade> powerUpgrades;

		// Token: 0x04005F8A RID: 24458
		public bool alwaysDisplayAsUsingPower;

		// Token: 0x02001979 RID: 6521
		public class PowerUpgrade
		{
			// Token: 0x04005F8B RID: 24459
			public ResearchProjectDef researchProject;

			// Token: 0x04005F8C RID: 24460
			public float factor;
		}
	}
}
