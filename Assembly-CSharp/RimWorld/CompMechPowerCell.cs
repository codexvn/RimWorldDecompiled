using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D3D RID: 11581
	public class CompMechPowerCell : ThingComp
	{
		// Token: 0x1700295D RID: 10589
		// (get) Token: 0x06010452 RID: 66642 RVA: 0x004B3F67 File Offset: 0x004B2167
		public CompProperties_MechPowerCell Props
		{
			get
			{
				return (CompProperties_MechPowerCell)this.props;
			}
		}

		// Token: 0x1700295E RID: 10590
		// (get) Token: 0x06010453 RID: 66643 RVA: 0x004B3F74 File Offset: 0x004B2174
		public float PercentFull
		{
			get
			{
				return (float)this.powerTicksLeft / (float)this.Props.totalPowerTicks;
			}
		}

		// Token: 0x1700295F RID: 10591
		// (get) Token: 0x06010454 RID: 66644 RVA: 0x004B3F8A File Offset: 0x004B218A
		public int PowerTicksLeft
		{
			get
			{
				return this.powerTicksLeft;
			}
		}

		// Token: 0x06010456 RID: 66646 RVA: 0x004B3F94 File Offset: 0x004B2194
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckBiotechOrAnomalyOrOdyssey("Mech power cell"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && !this.parent.BeingTransportedOnGravship)
			{
				this.powerTicksLeft = this.Props.totalPowerTicks;
			}
		}

		// Token: 0x06010457 RID: 66647 RVA: 0x004B3FE2 File Offset: 0x004B21E2
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.parent.Faction != Faction.OfPlayer && !this.Props.showGizmoOnNonPlayerControlled)
			{
				yield break;
			}
			if (Find.Selector.SingleSelectedThing == this.parent)
			{
				if (this.gizmo == null)
				{
					this.gizmo = new MechPowerCellGizmo(this)
					{
						Order = -100f
					};
				}
				yield return this.gizmo;
			}
			if (DebugSettings.ShowDevGizmos)
			{
				Command_Action command_Action = new Command_Action
				{
					defaultLabel = "DEV: Power left 0%",
					action = delegate()
					{
						this.powerTicksLeft = 0;
					}
				};
				yield return command_Action;
				Command_Action command_Action2 = new Command_Action
				{
					defaultLabel = "DEV: Power left 100%",
					action = delegate()
					{
						this.powerTicksLeft = this.Props.totalPowerTicks;
					}
				};
				yield return command_Action2;
			}
			yield break;
		}

		// Token: 0x06010458 RID: 66648 RVA: 0x004B3FF4 File Offset: 0x004B21F4
		public override void CompTick()
		{
			base.CompTick();
			if (this.depleted)
			{
				return;
			}
			this.powerTicksLeft--;
			if (this.powerTicksLeft <= 0)
			{
				if (this.Props.killWhenDepleted)
				{
					this.KillPowerProcessor();
					return;
				}
				this.powerTicksLeft = 0;
				this.depleted = true;
			}
		}

		// Token: 0x06010459 RID: 66649 RVA: 0x004B404C File Offset: 0x004B224C
		private void KillPowerProcessor()
		{
			Pawn pawn = (Pawn)this.parent;
			List<BodyPartRecord> allParts = pawn.def.race.body.AllParts;
			for (int i = 0; i < allParts.Count; i++)
			{
				BodyPartRecord bodyPartRecord = allParts[i];
				if (bodyPartRecord.def.tags.Contains(BodyPartTagDefOf.BloodPumpingSource))
				{
					pawn.health.AddHediff(HediffDefOf.MissingBodyPart, bodyPartRecord, null, null);
				}
			}
			if (!pawn.Dead)
			{
				pawn.Kill(null, null);
			}
		}

		// Token: 0x0601045A RID: 66650 RVA: 0x004B40E1 File Offset: 0x004B22E1
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.powerTicksLeft, "powerTicksLeft", 0, false);
			Scribe_Values.Look<bool>(ref this.depleted, "depleted", false, false);
		}

		// Token: 0x0400A9F5 RID: 43509
		private int powerTicksLeft;

		// Token: 0x0400A9F6 RID: 43510
		public bool depleted;

		// Token: 0x0400A9F7 RID: 43511
		private MechPowerCellGizmo gizmo;
	}
}
