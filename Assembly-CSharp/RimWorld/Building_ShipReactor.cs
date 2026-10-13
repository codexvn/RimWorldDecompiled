using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200298B RID: 10635
	public class Building_ShipReactor : Building
	{
		// Token: 0x0600E9DA RID: 59866 RVA: 0x00440B8F File Offset: 0x0043ED8F
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.charlonsReactor)
			{
				QuestUtility.SendQuestTargetSignals(base.Map.Parent.questTags, "ReactorDestroyed");
			}
			base.Destroy(mode);
		}

		// Token: 0x0600E9DB RID: 59867 RVA: 0x00440BBA File Offset: 0x0043EDBA
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			foreach (Gizmo gizmo2 in ShipUtility.ShipStartupGizmos(this))
			{
				yield return gizmo2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600E9DC RID: 59868 RVA: 0x00440BCA File Offset: 0x0043EDCA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.charlonsReactor, "charlonsReactor", false, false);
		}

		// Token: 0x04009B99 RID: 39833
		public bool charlonsReactor;
	}
}
