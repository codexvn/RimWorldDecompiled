using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BDF RID: 7135
	public class QuestPart_TradeRequestInactive : QuestPartActivable
	{
		// Token: 0x1700186E RID: 6254
		// (get) Token: 0x06009EEB RID: 40683 RVA: 0x002DED66 File Offset: 0x002DCF66
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.settlement != null)
				{
					yield return this.settlement;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x1700186F RID: 6255
		// (get) Token: 0x06009EEC RID: 40684 RVA: 0x002DED76 File Offset: 0x002DCF76
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in base.InvolvedFactions)
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.settlement.Faction != null)
				{
					yield return this.settlement.Faction;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06009EED RID: 40685 RVA: 0x002DED88 File Offset: 0x002DCF88
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.settlement == null || !this.settlement.Spawned)
			{
				base.Complete();
				return;
			}
			TradeRequestComp component = this.settlement.GetComponent<TradeRequestComp>();
			if (component == null || !component.ActiveRequest)
			{
				base.Complete(this.settlement.Named("SUBJECT"));
			}
		}

		// Token: 0x06009EEE RID: 40686 RVA: 0x002DEDE4 File Offset: 0x002DCFE4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06009EEF RID: 40687 RVA: 0x002DEDFD File Offset: 0x002DCFFD
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.settlement = Find.WorldObjects.Settlements.FirstOrDefault<Settlement>();
		}

		// Token: 0x04006DF2 RID: 28146
		public Settlement settlement;
	}
}
