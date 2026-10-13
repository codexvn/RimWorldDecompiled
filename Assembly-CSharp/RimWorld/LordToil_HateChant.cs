using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016FD RID: 5885
	public class LordToil_HateChant : LordToil
	{
		// Token: 0x1700154A RID: 5450
		// (get) Token: 0x06008ABB RID: 35515 RVA: 0x00295F60 File Offset: 0x00294160
		protected LordToilData_HateChant Data
		{
			get
			{
				return (LordToilData_HateChant)this.data;
			}
		}

		// Token: 0x06008ABC RID: 35516 RVA: 0x00295F6D File Offset: 0x0029416D
		public LordToil_HateChant()
		{
			this.data = new LordToilData_HateChant();
		}

		// Token: 0x06008ABD RID: 35517 RVA: 0x00295F8B File Offset: 0x0029418B
		public LordToil_HateChant(IEnumerable<PsychicRitualParticipant> participants)
		{
			this.data = new LordToilData_HateChant();
			this.SetParticipants(participants);
		}

		// Token: 0x06008ABE RID: 35518 RVA: 0x00295FB0 File Offset: 0x002941B0
		public override void Init()
		{
			base.Init();
			this.TryCreatingDrone();
			this.Data.lastDroneUpdate = Find.TickManager.TicksGame;
		}

		// Token: 0x06008ABF RID: 35519 RVA: 0x00295FD4 File Offset: 0x002941D4
		public override void UpdateAllDuties()
		{
			foreach (PsychicRitualParticipant psychicRitualParticipant in this.Data.chanters)
			{
				Pawn pawn;
				IntVec3 intVec;
				psychicRitualParticipant.Deconstruct(out pawn, out intVec);
				Pawn pawn2 = pawn;
				IntVec3 c = intVec;
				if (((pawn2 != null) ? pawn2.mindState : null) != null)
				{
					pawn2.mindState.duty = new PawnDuty(DutyDefOf.PerformHateChant, c, -1f);
				}
				if (pawn2 != null)
				{
					pawn2.health.AddHediff(HediffDefOf.PsychicTrance, null, null, null);
				}
			}
		}

		// Token: 0x06008AC0 RID: 35520 RVA: 0x00296080 File Offset: 0x00294280
		public override void LordToilTick()
		{
			base.LordToilTick();
			if (this.Data.condition != null && this.Data.condition.level < HateChantDroneLevel.Extreme && Find.TickManager.TicksGame > this.Data.lastDroneUpdate + this.DroneBoostInterval)
			{
				this.Data.lastDroneUpdate = Find.TickManager.TicksGame;
				GameCondition_HateChantDrone condition = this.Data.condition;
				condition.level += 1;
				Messages.Message("MessageHateChantIncreased".Translate(), MessageTypeDefOf.ThreatSmall, true);
			}
		}

		// Token: 0x06008AC1 RID: 35521 RVA: 0x00296119 File Offset: 0x00294319
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.Data.condition == null)
			{
				return;
			}
			this.Data.condition.End();
			this.Data.condition = null;
		}

		// Token: 0x06008AC2 RID: 35522 RVA: 0x0029614C File Offset: 0x0029434C
		public void SetParticipants(IEnumerable<PsychicRitualParticipant> participants)
		{
			PsychicRitualParticipant[] collection = (participants as PsychicRitualParticipant[]) ?? participants.ToArray<PsychicRitualParticipant>();
			this.Data.chanters = new List<PsychicRitualParticipant>(collection);
		}

		// Token: 0x06008AC3 RID: 35523 RVA: 0x0029617C File Offset: 0x0029437C
		private void TryCreatingDrone()
		{
			GameConditionManager gameConditionManager = this.lord.Map.gameConditionManager;
			if (gameConditionManager == null)
			{
				Log.ErrorOnce(string.Format("Couldn't find condition manager for incident target {0}", this.lord.Map), 70849667);
				return;
			}
			if (gameConditionManager.ConditionIsActive(GameConditionDefOf.HateChantDrone))
			{
				return;
			}
			List<GameCondition> activeConditions = gameConditionManager.ActiveConditions;
			for (int i = 0; i < activeConditions.Count; i++)
			{
				if (!GameConditionDefOf.HateChantDrone.CanCoexistWith(activeConditions[i].def))
				{
					return;
				}
			}
			GameCondition_HateChantDrone gameCondition_HateChantDrone = (GameCondition_HateChantDrone)GameConditionMaker.MakeCondition(GameConditionDefOf.HateChantDrone, -1);
			gameConditionManager.RegisterCondition(gameCondition_HateChantDrone);
			this.Data.condition = gameCondition_HateChantDrone;
		}

		// Token: 0x04005A4A RID: 23114
		private readonly int DroneBoostInterval = 15000;
	}
}
