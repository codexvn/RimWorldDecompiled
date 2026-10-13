using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C48 RID: 7240
	public class QuestPart_GuardianShipDelay : QuestPartActivable
	{
		// Token: 0x170018C4 RID: 6340
		// (get) Token: 0x0600A128 RID: 41256 RVA: 0x002E5CA6 File Offset: 0x002E3EA6
		public int TicksLeft
		{
			get
			{
				if (base.State != QuestPartState.Enabled)
				{
					return 0;
				}
				return this.delayTicks - this.ticksPassed;
			}
		}

		// Token: 0x170018C5 RID: 6341
		// (get) Token: 0x0600A129 RID: 41257 RVA: 0x002E5CC0 File Offset: 0x002E3EC0
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.pawn != null)
				{
					yield return this.pawn;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A12A RID: 41258 RVA: 0x002E5CD0 File Offset: 0x002E3ED0
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.ticksPassed = 0;
		}

		// Token: 0x0600A12B RID: 41259 RVA: 0x002E5CE0 File Offset: 0x002E3EE0
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (!this.pawn.DestroyedOrNull() && !this.pawn.Suspended)
			{
				this.ticksPassed++;
				if (this.ticksPassed >= this.delayTicks)
				{
					base.Complete();
				}
			}
		}

		// Token: 0x0600A12C RID: 41260 RVA: 0x002E5D2F File Offset: 0x002E3F2F
		public override IEnumerable<Gizmo> ExtraGizmos(ISelectable target)
		{
			if (target == this.pawn)
			{
				if (this.gizmo == null)
				{
					this.gizmo = new GuardianShipGizmo(this);
				}
				yield return this.gizmo;
			}
			yield break;
		}

		// Token: 0x0600A12D RID: 41261 RVA: 0x002E5D46 File Offset: 0x002E3F46
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Values.Look<int>(ref this.delayTicks, "delayTicks", 0, false);
			Scribe_Values.Look<int>(ref this.ticksPassed, "ticksPassed", 0, false);
		}

		// Token: 0x0600A12E RID: 41262 RVA: 0x002E5D83 File Offset: 0x002E3F83
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.pawn = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists.RandomElement<Pawn>();
			this.delayTicks = Rand.RangeInclusive(833, 2500);
		}

		// Token: 0x04006F66 RID: 28518
		public Pawn pawn;

		// Token: 0x04006F67 RID: 28519
		public int delayTicks;

		// Token: 0x04006F68 RID: 28520
		public int ticksPassed;

		// Token: 0x04006F69 RID: 28521
		private Gizmo gizmo;
	}
}
