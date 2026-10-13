using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003774 RID: 14196
	public class ThreatsGeneratorParams : IExposable
	{
		// Token: 0x06013AC6 RID: 80582 RVA: 0x005D84A8 File Offset: 0x005D66A8
		public void ExposeData()
		{
			Scribe_Values.Look<AllowedThreatsGeneratorThreats>(ref this.allowedThreats, "allowedThreats", AllowedThreatsGeneratorThreats.None, false);
			Scribe_Values.Look<int>(ref this.randSeed, "randSeed", 0, false);
			Scribe_Values.Look<float>(ref this.onDays, "onDays", 0f, false);
			Scribe_Values.Look<float>(ref this.offDays, "offDays", 0f, false);
			Scribe_Values.Look<float>(ref this.minSpacingDays, "minSpacingDays", 0f, false);
			Scribe_Values.Look<FloatRange>(ref this.numIncidentsRange, "numIncidentsRange", default(FloatRange), false);
			Scribe_Values.Look<float?>(ref this.threatPoints, "threatPoints", null, false);
			Scribe_Values.Look<float?>(ref this.minThreatPoints, "minThreatPoints", null, false);
			Scribe_Values.Look<float>(ref this.currentThreatPointsFactor, "currentThreatPointsFactor", 1f, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x06013AC7 RID: 80583 RVA: 0x005D8590 File Offset: 0x005D6790
		public override string ToString()
		{
			string text = "(";
			text = text + "onDays=" + this.onDays.ToString("0.##");
			text = text + " offDays=" + this.offDays.ToString("0.##");
			text = text + " minSpacingDays=" + this.minSpacingDays.ToString("0.##");
			string str = text;
			string str2 = " numIncidentsRange=";
			FloatRange floatRange = this.numIncidentsRange;
			text = str + str2 + floatRange.ToString();
			if (this.threatPoints != null)
			{
				text = text + " threatPoints=" + this.threatPoints.Value.ToString();
			}
			if (this.minThreatPoints != null)
			{
				text = text + " minThreatPoints=" + this.minThreatPoints.Value.ToString();
			}
			if (this.faction != null)
			{
				string str3 = text;
				string str4 = " faction=";
				Faction faction = this.faction;
				text = str3 + str4 + ((faction != null) ? faction.ToString() : null);
			}
			return text + ")";
		}

		// Token: 0x0400D9F5 RID: 55797
		public AllowedThreatsGeneratorThreats allowedThreats;

		// Token: 0x0400D9F6 RID: 55798
		public int randSeed;

		// Token: 0x0400D9F7 RID: 55799
		public float onDays;

		// Token: 0x0400D9F8 RID: 55800
		public float offDays;

		// Token: 0x0400D9F9 RID: 55801
		public float minSpacingDays;

		// Token: 0x0400D9FA RID: 55802
		public FloatRange numIncidentsRange;

		// Token: 0x0400D9FB RID: 55803
		public float? threatPoints;

		// Token: 0x0400D9FC RID: 55804
		public float? minThreatPoints;

		// Token: 0x0400D9FD RID: 55805
		public float currentThreatPointsFactor = 1f;

		// Token: 0x0400D9FE RID: 55806
		public Faction faction;
	}
}
