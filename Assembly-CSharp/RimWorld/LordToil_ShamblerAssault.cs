using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200171A RID: 5914
	public class LordToil_ShamblerAssault : LordToil
	{
		// Token: 0x1700156A RID: 5482
		// (get) Token: 0x06008B3F RID: 35647 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AssignsDuties
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700156B RID: 5483
		// (get) Token: 0x06008B40 RID: 35648 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool AllowAggressiveTargetingOfRoamers
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008B41 RID: 35649 RVA: 0x00297B40 File Offset: 0x00295D40
		public override void UpdateAllDuties()
		{
			IEnumerable<Pawn> source = from pt in this.lord.Map.mapPawns.AllHumanlikeSpawned.Concat(this.lord.Map.mapPawns.SpawnedColonyAnimals).Concat(this.lord.Map.mapPawns.SpawnedColonyMechs)
			where pt.Faction != Faction.OfEntities && !pt.ThreatDisabled(null)
			select pt;
			if (!source.Any<Pawn>())
			{
				base.Map.lordManager.RemoveLord(this.lord);
				return;
			}
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn.mindState.enemyTarget == null)
				{
					Hediff_Shambler hediff_Shambler = pawn.mutant.Hediff as Hediff_Shambler;
					if (hediff_Shambler != null)
					{
						hediff_Shambler.Notify_DelayedAlert(source.RandomElement<Pawn>());
					}
					pawn.mindState.Notify_EngagedTarget();
				}
			}
		}

		// Token: 0x06008B42 RID: 35650 RVA: 0x00293E2F File Offset: 0x0029202F
		public override void LordToilTick()
		{
			if (this.lord.ticksInToil % 300 == 0)
			{
				this.UpdateAllDuties();
			}
		}

		// Token: 0x06008B43 RID: 35651 RVA: 0x00297C40 File Offset: 0x00295E40
		public override bool CanAddPawn(Pawn p)
		{
			return p.IsShambler;
		}

		// Token: 0x04005A7C RID: 23164
		private const int UpdateIntervalTicks = 300;
	}
}
