using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DD4 RID: 7636
	public class IncidentWorker_ShortCircuit : IncidentWorker
	{
		// Token: 0x0600AA54 RID: 43604 RVA: 0x0030B896 File Offset: 0x00309A96
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return ShortCircuitUtility.GetShortCircuitablePowerConduits((Map)parms.target).Any<Building>();
		}

		// Token: 0x0600AA55 RID: 43605 RVA: 0x0030B8B0 File Offset: 0x00309AB0
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Building culprit;
			if (!ShortCircuitUtility.GetShortCircuitablePowerConduits((Map)parms.target).TryRandomElement(out culprit))
			{
				return false;
			}
			ShortCircuitUtility.DoShortCircuit(culprit);
			return true;
		}
	}
}
