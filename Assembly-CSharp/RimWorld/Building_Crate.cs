using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020029EC RID: 10732
	public class Building_Crate : Building_Casket, IThingGlower
	{
		// Token: 0x1700248F RID: 9359
		// (get) Token: 0x0600ECDE RID: 60638 RVA: 0x003E28D8 File Offset: 0x003E0AD8
		public override int OpenTicks
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x0600ECDF RID: 60639 RVA: 0x0044B336 File Offset: 0x00449536
		public bool ShouldBeLitNow()
		{
			return base.HasAnyContents;
		}

		// Token: 0x17002490 RID: 9360
		// (get) Token: 0x0600ECE0 RID: 60640 RVA: 0x0044C0DE File Offset: 0x0044A2DE
		public override bool CanOpen
		{
			get
			{
				if (this.hackable == null)
				{
					this.hackable = base.GetComp<CompHackable>();
				}
				return (this.hackable == null || this.hackable.IsHacked) && base.CanOpen;
			}
		}

		// Token: 0x0600ECE1 RID: 60641 RVA: 0x0044C111 File Offset: 0x0044A311
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.def == ThingDefOf.AncientSecurityCrate && !ModLister.CheckIdeology("Ancient security crate"))
			{
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
		}

		// Token: 0x0600ECE2 RID: 60642 RVA: 0x0044C135 File Offset: 0x0044A335
		public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			if (base.TryAcceptThing(thing, allowSpecialEffects))
			{
				base.BroadcastCompSignal("CrateContentsChanged");
				return true;
			}
			return false;
		}

		// Token: 0x0600ECE3 RID: 60643 RVA: 0x0044C150 File Offset: 0x0044A350
		public override void EjectContents()
		{
			this.innerContainer.TryDropAll(base.Position, base.Map, ThingPlaceMode.Near, null, (IntVec3 c) => c.GetEdifice(base.Map) == null, true);
			this.contentsKnown = true;
			if (this.def.building.openingEffect != null)
			{
				Effecter effecter = this.def.building.openingEffect.Spawn();
				effecter.Trigger(new TargetInfo(base.Position, base.Map, false), null, -1);
				effecter.Cleanup();
			}
			base.BroadcastCompSignal("CrateContentsChanged");
		}

		// Token: 0x0600ECE4 RID: 60644 RVA: 0x0044C1E2 File Offset: 0x0044A3E2
		protected override void ReceiveCompSignal(string signal)
		{
			base.ReceiveCompSignal(signal);
			if (signal == "Hacked")
			{
				this.Open();
			}
		}

		// Token: 0x0600ECE5 RID: 60645 RVA: 0x0044C1FE File Offset: 0x0044A3FE
		public override void Open()
		{
			this.contentsKnown = true;
			if (!this.CanOpen)
			{
				return;
			}
			base.Open();
		}

		// Token: 0x0600ECE6 RID: 60646 RVA: 0x0044C216 File Offset: 0x0044A416
		public override IEnumerable<FloatMenuOption> GetMultiSelectFloatMenuOptions(IEnumerable<Pawn> selPawns)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetMultiSelectFloatMenuOptions(selPawns))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (!this.CanOpen)
			{
				yield break;
			}
			Building_Crate.tmpAllowedPawns.Clear();
			foreach (Pawn pawn in selPawns)
			{
				if (pawn.RaceProps.Humanlike && pawn.CanReach(this, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					Building_Crate.tmpAllowedPawns.Add(pawn);
				}
			}
			if (Building_Crate.tmpAllowedPawns.Count <= 0)
			{
				yield return new FloatMenuOption("CannotOpen".Translate(this) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			Building_Crate.tmpAllowedPawns.Clear();
			foreach (Pawn pawn2 in selPawns)
			{
				if (pawn2.RaceProps.Humanlike && this.IsCapableOfOpening(pawn2))
				{
					Building_Crate.tmpAllowedPawns.Add(pawn2);
				}
			}
			if (Building_Crate.tmpAllowedPawns.Count <= 0)
			{
				yield return new FloatMenuOption("CannotOpen".Translate(this.Label) + ": " + "Incapable".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			Building_Crate.tmpAllowedPawns.Clear();
			foreach (Pawn pawn3 in selPawns)
			{
				if (pawn3.RaceProps.Humanlike && this.IsCapableOfOpening(pawn3) && pawn3.CanReach(this, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					Building_Crate.tmpAllowedPawns.Add(pawn3);
				}
			}
			if (Building_Crate.tmpAllowedPawns.Count > 0)
			{
				yield return new FloatMenuOption("Open".Translate(this), delegate()
				{
					Building_Crate.tmpAllowedPawns[0].jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Open, this), new JobTag?(JobTag.Misc), false);
					for (int i = 1; i < Building_Crate.tmpAllowedPawns.Count; i++)
					{
						FloatMenuOptionProvider_DraftedMove.PawnGotoAction(base.Position, Building_Crate.tmpAllowedPawns[i], RCellFinder.BestOrderedGotoDestNear(base.Position, Building_Crate.tmpAllowedPawns[i], null, true));
					}
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			yield break;
			yield break;
		}

		// Token: 0x0600ECE7 RID: 60647 RVA: 0x0044C22D File Offset: 0x0044A42D
		private bool IsCapableOfOpening(Pawn pawn)
		{
			return !pawn.IsSubhuman && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation);
		}

		// Token: 0x04009CE9 RID: 40169
		public const string CrateContentsChanged = "CrateContentsChanged";

		// Token: 0x04009CEA RID: 40170
		private CompHackable hackable;

		// Token: 0x04009CEB RID: 40171
		private static List<Pawn> tmpAllowedPawns = new List<Pawn>();
	}
}
