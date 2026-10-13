using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E25 RID: 15909
	public class DownedRefugeeComp : ImportantPawnComp, IThingHolder
	{
		// Token: 0x1700363B RID: 13883
		// (get) Token: 0x06015BAA RID: 89002 RVA: 0x0067FBD6 File Offset: 0x0067DDD6
		protected override string PawnSaveKey
		{
			get
			{
				return "refugee";
			}
		}

		// Token: 0x06015BAB RID: 89003 RVA: 0x0067FBE0 File Offset: 0x0067DDE0
		protected override void RemovePawnOnWorldObjectRemoved()
		{
			if (this.pawn.Any)
			{
				if (!this.pawn[0].Dead)
				{
					if (this.pawn[0].relations != null)
					{
						this.pawn[0].relations.Notify_FailedRescueQuest();
					}
					HealthUtility.HealNonPermanentInjuriesAndRestoreLegs(this.pawn[0]);
				}
				this.pawn.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
			}
		}

		// Token: 0x06015BAC RID: 89004 RVA: 0x0067FC53 File Offset: 0x0067DE53
		public override string CompInspectStringExtra()
		{
			if (this.pawn.Any)
			{
				return "Refugee".Translate() + ": " + this.pawn[0].LabelCap;
			}
			return null;
		}
	}
}
