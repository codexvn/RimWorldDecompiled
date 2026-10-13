using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200177B RID: 6011
	public class ShipJob_Unload : ShipJob
	{
		// Token: 0x170015AA RID: 5546
		// (get) Token: 0x06008D0B RID: 36107 RVA: 0x0029F4CC File Offset: 0x0029D6CC
		protected override bool ShouldEnd
		{
			get
			{
				return this.dropMode == TransportShipDropMode.None;
			}
		}

		// Token: 0x170015AB RID: 5547
		// (get) Token: 0x06008D0C RID: 36108 RVA: 0x00002501 File Offset: 0x00000701
		public override bool ShowGizmos
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170015AC RID: 5548
		// (get) Token: 0x06008D0D RID: 36109 RVA: 0x0029F4D7 File Offset: 0x0029D6D7
		private Thing ShipThing
		{
			get
			{
				return this.transportShip.shipThing;
			}
		}

		// Token: 0x06008D0E RID: 36110 RVA: 0x0029F4E4 File Offset: 0x0029D6E4
		public override bool TryStart()
		{
			return this.transportShip.ShipExistsAndIsSpawned && base.TryStart();
		}

		// Token: 0x06008D0F RID: 36111 RVA: 0x0029F4FB File Offset: 0x0029D6FB
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (!this.transportShip.ShipExistsAndIsSpawned)
			{
				return;
			}
			if (this.ShipThing.IsHashIntervalTick(60, delta))
			{
				this.Drop();
			}
		}

		// Token: 0x06008D10 RID: 36112 RVA: 0x0029F528 File Offset: 0x0029D728
		private void Drop()
		{
			Thing thing = null;
			float num = 0f;
			Map map = this.ShipThing.Map;
			for (int i = 0; i < this.transportShip.TransporterComp.innerContainer.Count; i++)
			{
				Thing thing2 = this.transportShip.TransporterComp.innerContainer[i];
				float dropPriority = this.GetDropPriority(thing2);
				if (dropPriority > num)
				{
					thing = thing2;
					num = dropPriority;
				}
			}
			if (thing != null && (thing is Pawn || this.dropMode != TransportShipDropMode.PawnsOnly))
			{
				ShipJob_Unload.UnloadThingFromShuttle(this.transportShip, thing, this.droppedThings, this.unforbidAll);
				return;
			}
			this.transportShip.TransporterComp.TryRemoveLord(map);
			this.End();
		}

		// Token: 0x06008D11 RID: 36113 RVA: 0x0029F5DC File Offset: 0x0029D7DC
		public static void UnloadThingFromShuttle(TransportShip ship, Thing thingToDrop, List<Thing> droppedThings = null, bool unforbidAll = false)
		{
			Thing shipThing = ship.shipThing;
			if (shipThing == null || !shipThing.Spawned)
			{
				Log.Error(string.Format("Tried to unload {0} from unspawned shuttle", thingToDrop));
				return;
			}
			Map map = ship.shipThing.Map;
			IntVec3 interactionCell = ship.shipThing.InteractionCell;
			Thing thing;
			if (ship.TransporterComp.innerContainer.TryDrop(thingToDrop, interactionCell, map, ThingPlaceMode.Near, out thing, null, delegate(IntVec3 c)
			{
				if (c.Fogged(map))
				{
					return false;
				}
				Pawn pawn2 = thingToDrop as Pawn;
				return pawn2 == null || !pawn2.Downed || c.GetFirstPawn(map) == null;
			}, !(thingToDrop is Pawn)))
			{
				ship.TransporterComp.Notify_ThingRemoved(thingToDrop);
				if (droppedThings != null)
				{
					droppedThings.Add(thingToDrop);
				}
				if (unforbidAll)
				{
					thingToDrop.SetForbidden(false, false);
				}
				Pawn pawn = thingToDrop as Pawn;
				if (pawn != null)
				{
					if (pawn.IsColonist && pawn.Spawned && !map.IsPlayerHome)
					{
						pawn.drafter.Drafted = true;
					}
					if (pawn.guest != null && pawn.guest.IsPrisoner)
					{
						pawn.guest.WaitInsteadOfEscapingForDefaultTicks();
					}
					if (pawn.IsColonist && map.IsPlayerHome)
					{
						pawn.inventory.UnloadEverything = true;
					}
				}
			}
		}

		// Token: 0x06008D12 RID: 36114 RVA: 0x0029F72C File Offset: 0x0029D92C
		private float GetDropPriority(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn == null)
			{
				return 0.25f;
			}
			if (this.droppedThings.Contains(t))
			{
				return 0f;
			}
			if (this.dropMode == TransportShipDropMode.NonRequired && this.transportShip.ShuttleComp.IsRequired(t))
			{
				return 0f;
			}
			Lord lord = pawn.GetLord();
			if (((lord != null) ? lord.CurLordToil : null) != null)
			{
				LordToil_EnterShuttleOrLeave lordToil_EnterShuttleOrLeave = lord.CurLordToil as LordToil_EnterShuttleOrLeave;
				if (lordToil_EnterShuttleOrLeave != null && lordToil_EnterShuttleOrLeave.shuttle == this.ShipThing)
				{
					return 0f;
				}
			}
			if (!pawn.AnimalOrWildMan())
			{
				return 1f;
			}
			return 0.5f;
		}

		// Token: 0x06008D13 RID: 36115 RVA: 0x0029F7D0 File Offset: 0x0029D9D0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.droppedThings, "droppedThings", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<TransportShipDropMode>(ref this.dropMode, "dropMode", TransportShipDropMode.None, false);
			Scribe_Values.Look<bool>(ref this.unforbidAll, "unforbidAll", true, false);
		}

		// Token: 0x04005BAC RID: 23468
		public TransportShipDropMode dropMode = TransportShipDropMode.All;

		// Token: 0x04005BAD RID: 23469
		private List<Thing> droppedThings = new List<Thing>();

		// Token: 0x04005BAE RID: 23470
		public bool unforbidAll = true;

		// Token: 0x04005BAF RID: 23471
		private const int DropInterval = 60;
	}
}
