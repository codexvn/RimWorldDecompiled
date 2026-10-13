using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002ABC RID: 10940
	public class SignalAction_DormancyWakeUp : SignalAction_Delay
	{
		// Token: 0x1700261D RID: 9757
		// (get) Token: 0x0600F3C7 RID: 62407 RVA: 0x0046D338 File Offset: 0x0046B538
		public override Alert_ActionDelay Alert
		{
			get
			{
				if (this.cachedAlert == null && this.lord.faction != null && this.lord.faction.HostileTo(Faction.OfPlayer) && this.lord.ownedPawns.Count > 0)
				{
					this.cachedAlert = new Alert_DormanyWakeUpDelay(this);
				}
				return this.cachedAlert;
			}
		}

		// Token: 0x1700261E RID: 9758
		// (get) Token: 0x0600F3C8 RID: 62408 RVA: 0x0046D398 File Offset: 0x0046B598
		public override bool ShouldRemoveNow
		{
			get
			{
				if (this.lord == null || !base.Map.lordManager.lords.Contains(this.lord))
				{
					return true;
				}
				List<Pawn> ownedPawns = this.lord.ownedPawns;
				if (ownedPawns.NullOrEmpty<Pawn>())
				{
					return true;
				}
				for (int i = 0; i < ownedPawns.Count; i++)
				{
					if (!ownedPawns[i].Awake())
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x0600F3C9 RID: 62409 RVA: 0x0046D404 File Offset: 0x0046B604
		protected override void Complete()
		{
			base.Complete();
			if (this.lord != null)
			{
				this.lord.Notify_DormancyWakeup();
			}
		}

		// Token: 0x0600F3CA RID: 62410 RVA: 0x0046D41F File Offset: 0x0046B61F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Lord>(ref this.lord, "lord", false);
		}

		// Token: 0x0400A096 RID: 41110
		public Lord lord;

		// Token: 0x0400A097 RID: 41111
		private Alert_ActionDelay cachedAlert;
	}
}
