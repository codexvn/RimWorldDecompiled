using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200259B RID: 9627
	public class GoodwillSituationWorker_AttackingSettlement : GoodwillSituationWorker
	{
		// Token: 0x0600D306 RID: 54022 RVA: 0x003E28DC File Offset: 0x003E0ADC
		public override int GetMaxGoodwill(Faction other)
		{
			if (!this.IsAttackingSettlement(other))
			{
				return 100;
			}
			return -80;
		}

		// Token: 0x0600D307 RID: 54023 RVA: 0x003E28EC File Offset: 0x003E0AEC
		private bool IsAttackingSettlement(Faction other)
		{
			return Current.ProgramState != ProgramState.Entry && SettlementUtility.IsPlayerAttackingAnySettlementOf(other);
		}

		// Token: 0x04008F3C RID: 36668
		private const int MaxGoodwill = -80;
	}
}
