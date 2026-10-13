using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003742 RID: 14146
	public class StatWorker_MaxPowerOutput : StatWorker
	{
		// Token: 0x060139D2 RID: 80338 RVA: 0x005D38A8 File Offset: 0x005D1AA8
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			ThingDef thingDef = req.Def as ThingDef;
			if (thingDef == null)
			{
				string str = "Tried to get max power output for non-thing ";
				Def def = req.Def;
				Log.ErrorOnce(str + ((def != null) ? def.ToString() : null), 64352724);
				return 0f;
			}
			CompProperties_Power compProperties_Power = (CompProperties_Power)thingDef.CompDefForAssignableFrom<CompPowerPlant>();
			if (compProperties_Power == null)
			{
				string str2 = "Tried to get power output of ";
				ThingDef thingDef2 = thingDef;
				Log.ErrorOnce(str2 + ((thingDef2 != null) ? thingDef2.ToString() : null) + " which has no CompPowerPlant.", 756784453);
				return 0f;
			}
			return -compProperties_Power.PowerConsumption;
		}
	}
}
