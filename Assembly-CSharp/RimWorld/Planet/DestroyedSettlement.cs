using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DA4 RID: 15780
	public class DestroyedSettlement : MapParent
	{
		// Token: 0x060157F4 RID: 88052 RVA: 0x0067111C File Offset: 0x0066F31C
		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			if (ModsConfig.OdysseyActive && base.Map.listerThings.AnyThingWithDef(ThingDefOf.GravAnchor))
			{
				return false;
			}
			if (base.Map.mapPawns.AnyPawnBlockingMapRemoval)
			{
				return false;
			}
			if (TransporterUtility.IncomingTransporterPreventingMapRemoval(base.Map))
			{
				return false;
			}
			alsoRemoveWorldObject = true;
			return true;
		}

		// Token: 0x060157F5 RID: 88053 RVA: 0x00671173 File Offset: 0x0066F373
		public override IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			foreach (IncidentTargetTagDef incidentTargetTagDef in base.IncidentTargetTags())
			{
				yield return incidentTargetTagDef;
			}
			IEnumerator<IncidentTargetTagDef> enumerator = null;
			yield return IncidentTargetTagDefOf.Map_PlayerHome;
			yield break;
			yield break;
		}

		// Token: 0x060157F6 RID: 88054 RVA: 0x00671183 File Offset: 0x0066F383
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (base.HasMap && Find.WorldSelector.SingleSelectedObject == this)
			{
				yield return SettleInExistingMapUtility.SettleCommand(base.Map, false);
			}
			yield break;
			yield break;
		}
	}
}
