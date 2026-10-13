using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200275D RID: 10077
	public class RitualOutcomeComp_GravshipFacilities : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x170021D0 RID: 8656
		// (get) Token: 0x0600DD02 RID: 56578 RVA: 0x0040D938 File Offset: 0x0040BB38
		protected override string LabelForDesc
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x0600DD03 RID: 56579 RVA: 0x00410778 File Offset: 0x0040E978
		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			QualityFactor qualityFactor = this.GetQualityFactor(ritual.Ritual, ritual.selectedTarget, ritual.obligation, ritual.assignments, data);
			if (qualityFactor == null)
			{
				return 0f;
			}
			return qualityFactor.quality;
		}

		// Token: 0x0600DD04 RID: 56580 RVA: 0x004107A8 File Offset: 0x0040E9A8
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			Thing thing = ritualTarget.Thing;
			Building_GravEngine building_GravEngine;
			if (thing == null)
			{
				building_GravEngine = null;
			}
			else
			{
				CompPilotConsole compPilotConsole = thing.TryGetComp<CompPilotConsole>();
				building_GravEngine = ((compPilotConsole != null) ? compPilotConsole.engine : null);
			}
			Building_GravEngine building_GravEngine2 = building_GravEngine;
			if (building_GravEngine2 == null)
			{
				return null;
			}
			float num = 0f;
			int num2 = 0;
			foreach (CompGravshipFacility compGravshipFacility in building_GravEngine2.GravshipComponents)
			{
				CompPowerTrader compPowerTrader = compGravshipFacility.parent.TryGetComp<CompPowerTrader>();
				if (compPowerTrader == null || compPowerTrader.PowerOn)
				{
					CompGravshipThruster compGravshipThruster = compGravshipFacility.parent.TryGetComp<CompGravshipThruster>();
					if (compGravshipThruster == null || compGravshipThruster.CanBeActive)
					{
						ThingDef def = compGravshipFacility.parent.def;
						float num3;
						if (this.facilityQualityOffsets.TryGetValue(def, out num3))
						{
							num += num3;
							num2++;
							this.tmpFacilityCount.TryAdd(def, 0);
							Dictionary<ThingDef, int> dictionary = this.tmpFacilityCount;
							ThingDef thingDef = def;
							int num4 = dictionary[thingDef];
							dictionary[thingDef] = num4 + 1;
						}
					}
				}
			}
			if (num2 == 0)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<ThingDef, int> keyValuePair in this.tmpFacilityCount)
			{
				ThingDef thingDef;
				int num4;
				keyValuePair.Deconstruct(out thingDef, out num4);
				ThingDef thingDef2 = thingDef;
				int num5 = num4;
				float num6;
				if (this.facilityQualityOffsets.TryGetValue(thingDef2, out num6))
				{
					stringBuilder.AppendLine(string.Format(" - {0} x{1}: +{2}", thingDef2.LabelCap, num5, (num6 * (float)num5).ToStringPercent()));
				}
			}
			this.tmpFacilityCount.Clear();
			int num7 = 0;
			foreach (KeyValuePair<ThingDef, float> keyValuePair2 in this.facilityQualityOffsets)
			{
				ThingDef thingDef;
				float num8;
				keyValuePair2.Deconstruct(out thingDef, out num8);
				ThingDef thingDef3 = thingDef;
				num7 += thingDef3.GetCompProperties<CompProperties_GravshipFacility>().maxSimultaneous;
			}
			string text = stringBuilder.ToString();
			return new QualityFactor
			{
				label = this.LabelForDesc.CapitalizeFirst(),
				qualityChange = "OutcomeBonusDesc_QualitySingleOffset".Translate(num.ToStringWithSign("0.#%")).Resolve(),
				count = num2.ToString() + " / " + num7.ToString(),
				quality = num,
				positive = true,
				priority = 4f,
				toolTip = (text.NullOrEmpty() ? null : text)
			};
		}

		// Token: 0x04009503 RID: 38147
		public Dictionary<ThingDef, float> facilityQualityOffsets = new Dictionary<ThingDef, float>();

		// Token: 0x04009504 RID: 38148
		private Dictionary<ThingDef, int> tmpFacilityCount = new Dictionary<ThingDef, int>();
	}
}
