using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E81 RID: 11905
	public class CompTemperatureRuinable : ThingComp
	{
		// Token: 0x17002B06 RID: 11014
		// (get) Token: 0x06010C29 RID: 68649 RVA: 0x004D4591 File Offset: 0x004D2791
		public CompProperties_TemperatureRuinable Props
		{
			get
			{
				return (CompProperties_TemperatureRuinable)this.props;
			}
		}

		// Token: 0x17002B07 RID: 11015
		// (get) Token: 0x06010C2A RID: 68650 RVA: 0x004D459E File Offset: 0x004D279E
		public bool Ruined
		{
			get
			{
				return this.ruinedPercent >= 1f;
			}
		}

		// Token: 0x06010C2B RID: 68651 RVA: 0x004D45B0 File Offset: 0x004D27B0
		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.ruinedPercent, "ruinedPercent", 0f, false);
		}

		// Token: 0x06010C2C RID: 68652 RVA: 0x004D45C8 File Offset: 0x004D27C8
		public void Reset()
		{
			this.ruinedPercent = 0f;
		}

		// Token: 0x06010C2D RID: 68653 RVA: 0x004A1C8D File Offset: 0x0049FE8D
		public override void CompTickRare()
		{
			this.CompTick();
		}

		// Token: 0x06010C2E RID: 68654 RVA: 0x004D45D8 File Offset: 0x004D27D8
		public override void CompTick()
		{
			if (this.Ruined)
			{
				return;
			}
			float ambientTemperature = this.parent.AmbientTemperature;
			if (ambientTemperature > this.Props.maxSafeTemperature)
			{
				this.ruinedPercent += (ambientTemperature - this.Props.maxSafeTemperature) * this.Props.progressPerDegreePerTick;
			}
			else if (ambientTemperature < this.Props.minSafeTemperature)
			{
				this.ruinedPercent -= (ambientTemperature - this.Props.minSafeTemperature) * this.Props.progressPerDegreePerTick;
			}
			if (this.ruinedPercent >= 1f)
			{
				this.ruinedPercent = 1f;
				this.parent.BroadcastCompSignal("RuinedByTemperature");
				return;
			}
			if (this.ruinedPercent < 0f)
			{
				this.ruinedPercent = 0f;
			}
		}

		// Token: 0x06010C2F RID: 68655 RVA: 0x004D46A8 File Offset: 0x004D28A8
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			float t = (float)count / (float)(this.parent.stackCount + count);
			CompTemperatureRuinable comp = ((ThingWithComps)otherStack).GetComp<CompTemperatureRuinable>();
			this.ruinedPercent = Mathf.Lerp(this.ruinedPercent, comp.ruinedPercent, t);
		}

		// Token: 0x06010C30 RID: 68656 RVA: 0x004D46EC File Offset: 0x004D28EC
		public override bool AllowStackWith(Thing other)
		{
			CompTemperatureRuinable comp = ((ThingWithComps)other).GetComp<CompTemperatureRuinable>();
			return this.Ruined == comp.Ruined;
		}

		// Token: 0x06010C31 RID: 68657 RVA: 0x004D4713 File Offset: 0x004D2913
		public override void PostSplitOff(Thing piece)
		{
			((ThingWithComps)piece).GetComp<CompTemperatureRuinable>().ruinedPercent = this.ruinedPercent;
		}

		// Token: 0x06010C32 RID: 68658 RVA: 0x004D472C File Offset: 0x004D292C
		public override string CompInspectStringExtra()
		{
			if (this.Ruined)
			{
				return "RuinedByTemperature".Translate();
			}
			if (this.ruinedPercent > 0f)
			{
				float ambientTemperature = this.parent.AmbientTemperature;
				string str;
				if (ambientTemperature > this.Props.maxSafeTemperature)
				{
					str = "Overheating".Translate();
				}
				else
				{
					if (ambientTemperature >= this.Props.minSafeTemperature)
					{
						return null;
					}
					str = "Freezing".Translate();
				}
				return str + ": " + this.ruinedPercent.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0400AEA0 RID: 44704
		protected float ruinedPercent;

		// Token: 0x0400AEA1 RID: 44705
		public const string RuinedSignal = "RuinedByTemperature";
	}
}
