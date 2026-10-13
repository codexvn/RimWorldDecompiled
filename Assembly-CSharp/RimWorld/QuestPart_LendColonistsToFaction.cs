using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C5C RID: 7260
	public class QuestPart_LendColonistsToFaction : QuestPartActivable
	{
		// Token: 0x170018DC RID: 6364
		// (get) Token: 0x0600A1B3 RID: 41395 RVA: 0x002E78A8 File Offset: 0x002E5AA8
		public List<Thing> LentColonistsListForReading
		{
			get
			{
				return this.lentColonists;
			}
		}

		// Token: 0x170018DD RID: 6365
		// (get) Token: 0x0600A1B4 RID: 41396 RVA: 0x002E78B0 File Offset: 0x002E5AB0
		public int ReturnPawnsInDurationTicks
		{
			get
			{
				return Mathf.Max(this.returnColonistsOnTick - GenTicks.TicksGame, 0);
			}
		}

		// Token: 0x0600A1B5 RID: 41397 RVA: 0x002E78C4 File Offset: 0x002E5AC4
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			CompTransporter compTransporter = this.shuttle.TryGetComp<CompTransporter>();
			if (this.lendColonistsToFaction != null && compTransporter != null)
			{
				foreach (Thing thing in ((IEnumerable<Thing>)compTransporter.innerContainer))
				{
					Pawn pawn = thing as Pawn;
					if (pawn != null && pawn.IsFreeColonist)
					{
						this.lentColonists.Add(pawn);
					}
				}
				this.returnColonistsOnTick = GenTicks.TicksGame + this.returnLentColonistsInTicks;
			}
		}

		// Token: 0x170018DE RID: 6366
		// (get) Token: 0x0600A1B6 RID: 41398 RVA: 0x002E7958 File Offset: 0x002E5B58
		public override string DescriptionPart
		{
			get
			{
				if (base.State == QuestPartState.Disabled || this.lentColonists.Count == 0)
				{
					return null;
				}
				return "PawnsLent".Translate((from t in this.lentColonists
				select t.LabelShort).ToCommaList(true, false), this.ReturnPawnsInDurationTicks.ToStringTicksToDays("0.0"));
			}
		}

		// Token: 0x0600A1B7 RID: 41399 RVA: 0x002E79D8 File Offset: 0x002E5BD8
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (ModsConfig.BiotechActive)
			{
				if (this.lentColonists.Any(delegate(Thing baby)
				{
					ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
					return ChildcareUtility.CanSuckle(baby as Pawn, out breastfeedFailReason);
				}))
				{
					foreach (Thing thing in this.lentColonists)
					{
						Pawn pawn = thing as Pawn;
						if (pawn != null)
						{
							Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Lactating, false);
							if (firstHediffOfDef != null)
							{
								firstHediffOfDef.Severity = 1f;
							}
						}
					}
				}
			}
			if (Find.TickManager.TicksGame >= this.enableTick + this.returnLentColonistsInTicks)
			{
				base.Complete();
			}
		}

		// Token: 0x0600A1B8 RID: 41400 RVA: 0x002E7AAC File Offset: 0x002E5CAC
		protected override void Complete(SignalArgs signalArgs)
		{
			MapParent mapParent = this.returnMap;
			Map map = (mapParent == null || mapParent.Map == null) ? Find.AnyPlayerHomeMap : this.returnMap.Map;
			if (map == null)
			{
				return;
			}
			base.Complete(new SignalArgs(new LookTargets(this.lentColonists).Named("SUBJECT"), (from c in this.lentColonists
			select c.LabelShort).ToCommaList(true, false).Named("PAWNS")));
			if (this.lendColonistsToFaction != null && this.lendColonistsToFaction == Faction.OfEmpire)
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Shuttle, null);
				thing.SetFaction(Faction.OfEmpire, null);
				TransportShip transportShip = TransportShipMaker.MakeTransportShip(TransportShipDefOf.Ship_Shuttle, this.lentColonists, thing);
				transportShip.ArriveAt(DropCellFinder.GetBestShuttleLandingSpot(map, Faction.OfEmpire), map.Parent);
				transportShip.AddJobs(new ShipJobDef[]
				{
					ShipJobDefOf.Unload,
					ShipJobDefOf.FlyAway
				});
				return;
			}
			DropPodUtility.DropThingsNear(DropCellFinder.TradeDropSpot(map), map, this.lentColonists, 110, false, false, false, false, true, null);
		}

		// Token: 0x0600A1B9 RID: 41401 RVA: 0x002E7BC8 File Offset: 0x002E5DC8
		private void ReturnDead(Corpse corpse)
		{
			Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
			if (anyPlayerHomeMap != null)
			{
				DropPodUtility.DropThingsNear(DropCellFinder.TradeDropSpot(anyPlayerHomeMap), anyPlayerHomeMap, Gen.YieldSingle<Corpse>(corpse), 110, false, false, false, false, true, null);
			}
		}

		// Token: 0x0600A1BA RID: 41402 RVA: 0x002E7BF8 File Offset: 0x002E5DF8
		public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			if (this.lentColonists.Contains(pawn))
			{
				Building_Grave assignedGrave = null;
				if (pawn.ownership != null)
				{
					assignedGrave = pawn.ownership.AssignedGrave;
				}
				Corpse corpse = pawn.MakeCorpse(assignedGrave, null);
				this.lentColonists.Remove(pawn);
				this.ReturnDead(corpse);
				if (!this.outSignalColonistsDied.NullOrEmpty() && this.lentColonists.Count == 0)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalColonistsDied, false));
				}
			}
		}

		// Token: 0x0600A1BB RID: 41403 RVA: 0x002E7C78 File Offset: 0x002E5E78
		public override void Notify_PawnBorn(Thing baby, Thing birther, Pawn mother, Pawn father)
		{
			if (this.lentColonists.Contains(birther))
			{
				Corpse corpse = baby as Corpse;
				if (corpse != null)
				{
					this.ReturnDead(corpse);
					return;
				}
				baby.SetFaction(birther.Faction, null);
				this.lentColonists.Add(baby);
			}
		}

		// Token: 0x0600A1BC RID: 41404 RVA: 0x002E7CC0 File Offset: 0x002E5EC0
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			if (base.State != QuestPartState.Enabled)
			{
				return;
			}
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect, "End " + this.ToString(), true, true, true, null))
			{
				base.Complete();
			}
			curY += rect.height + 4f;
		}

		// Token: 0x0600A1BD RID: 41405 RVA: 0x002E7D30 File Offset: 0x002E5F30
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_References.Look<Faction>(ref this.lendColonistsToFaction, "lendColonistsToFaction", false);
			Scribe_Values.Look<int>(ref this.returnLentColonistsInTicks, "returnLentColonistsInTicks", 0, false);
			Scribe_Values.Look<int>(ref this.returnColonistsOnTick, "colonistsReturnOnTick", 0, false);
			Scribe_Collections.Look<Thing>(ref this.lentColonists, "lentPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<MapParent>(ref this.returnMap, "returnMap", false);
			Scribe_Values.Look<string>(ref this.outSignalColonistsDied, "outSignalColonistsDied", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.lentColonists.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x04006FB8 RID: 28600
		public Thing shuttle;

		// Token: 0x04006FB9 RID: 28601
		public Faction lendColonistsToFaction;

		// Token: 0x04006FBA RID: 28602
		public int returnLentColonistsInTicks = -1;

		// Token: 0x04006FBB RID: 28603
		public MapParent returnMap;

		// Token: 0x04006FBC RID: 28604
		public string outSignalColonistsDied;

		// Token: 0x04006FBD RID: 28605
		private int returnColonistsOnTick;

		// Token: 0x04006FBE RID: 28606
		private List<Thing> lentColonists = new List<Thing>();
	}
}
