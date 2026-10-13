using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CE0 RID: 11488
	public class CompHatcher : ThingComp
	{
		// Token: 0x170028DA RID: 10458
		// (get) Token: 0x0601022B RID: 66091 RVA: 0x004AB1A2 File Offset: 0x004A93A2
		public CompProperties_Hatcher Props
		{
			get
			{
				return (CompProperties_Hatcher)this.props;
			}
		}

		// Token: 0x170028DB RID: 10459
		// (get) Token: 0x0601022C RID: 66092 RVA: 0x004AB1AF File Offset: 0x004A93AF
		private CompTemperatureRuinable FreezerComp
		{
			get
			{
				return this.parent.GetComp<CompTemperatureRuinable>();
			}
		}

		// Token: 0x170028DC RID: 10460
		// (get) Token: 0x0601022D RID: 66093 RVA: 0x004AB1BC File Offset: 0x004A93BC
		public bool TemperatureDamaged
		{
			get
			{
				return this.FreezerComp != null && this.FreezerComp.Ruined;
			}
		}

		// Token: 0x0601022E RID: 66094 RVA: 0x004AB1D4 File Offset: 0x004A93D4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.gestateProgress, "gestateProgress", 0f, false);
			Scribe_References.Look<Pawn>(ref this.hatcheeParent, "hatcheeParent", false);
			Scribe_References.Look<Pawn>(ref this.otherParent, "otherParent", false);
			Scribe_References.Look<Faction>(ref this.hatcheeFaction, "hatcheeFaction", false);
		}

		// Token: 0x0601022F RID: 66095 RVA: 0x004AB230 File Offset: 0x004A9430
		public override void CompTick()
		{
			if (!this.TemperatureDamaged)
			{
				float num = 1f / (this.Props.hatcherDaystoHatch * 60000f);
				this.gestateProgress += num;
				if (this.gestateProgress > 1f)
				{
					this.Hatch();
					return;
				}
			}
			else
			{
				PawnKindDef hatcherPawn = this.Props.hatcherPawn;
				CompProperties_EggLayer compProperties_EggLayer;
				if (hatcherPawn == null)
				{
					compProperties_EggLayer = null;
				}
				else
				{
					ThingDef race = hatcherPawn.race;
					compProperties_EggLayer = ((race != null) ? race.GetCompProperties<CompProperties_EggLayer>() : null);
				}
				CompProperties_EggLayer compProperties_EggLayer2 = compProperties_EggLayer;
				if (compProperties_EggLayer2 != null)
				{
					Pawn_CarryTracker pawn_CarryTracker = this.parent.ParentHolder as Pawn_CarryTracker;
					if (pawn_CarryTracker != null)
					{
						Thing thing;
						pawn_CarryTracker.TryDropCarriedThing(this.parent.Position, ThingPlaceMode.Near, out thing, null);
					}
					CompEggContainer compEggContainer = this.parent.ParentHolder as CompEggContainer;
					if (compEggContainer != null)
					{
						int stackCount = this.parent.stackCount;
						compEggContainer.innerContainer.Remove(this.parent);
						this.parent.Destroy(DestroyMode.Vanish);
						Thing thing2 = ThingMaker.MakeThing(compProperties_EggLayer2.eggUnfertilizedDef, null);
						thing2.stackCount = stackCount;
						compEggContainer.innerContainer.TryAdd(thing2, true);
						return;
					}
					Map mapHeld = this.parent.MapHeld;
					IntVec3 positionHeld = this.parent.PositionHeld;
					int stackCount2 = this.parent.stackCount;
					this.parent.Destroy(DestroyMode.Vanish);
					GenSpawn.Spawn(compProperties_EggLayer2.eggUnfertilizedDef, positionHeld, mapHeld, WipeMode.Vanish).stackCount = stackCount2;
				}
			}
		}

		// Token: 0x06010230 RID: 66096 RVA: 0x004AB388 File Offset: 0x004A9588
		public void Hatch()
		{
			try
			{
				PawnGenerationRequest request = new PawnGenerationRequest(this.Props.hatcherPawn, this.hatcheeFaction, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Newborn, null, null, null, false, false, false, -1, 0, false);
				Pawn_CarryTracker pawn_CarryTracker = this.parent.ParentHolder as Pawn_CarryTracker;
				if (pawn_CarryTracker != null)
				{
					Thing thing;
					pawn_CarryTracker.TryDropCarriedThing(this.parent.PositionHeld, ThingPlaceMode.Near, out thing, null);
				}
				for (int i = 0; i < this.parent.stackCount; i++)
				{
					Pawn pawn = PawnGenerator.GeneratePawn(request);
					if (PawnUtility.TrySpawnHatchedOrBornPawn(pawn, this.parent, null))
					{
						if (pawn != null)
						{
							if (this.hatcheeParent != null)
							{
								if (pawn.playerSettings != null && this.hatcheeParent.playerSettings != null && this.hatcheeParent.Faction == this.hatcheeFaction)
								{
									pawn.playerSettings.AreaRestrictionInPawnCurrentMap = this.hatcheeParent.playerSettings.AreaRestrictionInPawnCurrentMap;
								}
								if (pawn.RaceProps.IsFlesh)
								{
									pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, this.hatcheeParent);
								}
							}
							if (this.otherParent != null && (this.hatcheeParent == null || this.hatcheeParent.gender != this.otherParent.gender) && pawn.RaceProps.IsFlesh)
							{
								pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, this.otherParent);
							}
						}
						if (this.parent.Spawned)
						{
							FilthMaker.TryMakeFilth(this.parent.Position, this.parent.Map, ThingDefOf.Filth_AmnioticFluid, 1, FilthSourceFlags.None, true);
						}
					}
					else
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
					}
				}
			}
			finally
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06010231 RID: 66097 RVA: 0x004AB5D0 File Offset: 0x004A97D0
		public override bool AllowStackWith(Thing other)
		{
			CompHatcher comp = ((ThingWithComps)other).GetComp<CompHatcher>();
			return this.TemperatureDamaged == comp.TemperatureDamaged && base.AllowStackWith(other);
		}

		// Token: 0x06010232 RID: 66098 RVA: 0x004AB600 File Offset: 0x004A9800
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			float t = (float)count / (float)(this.parent.stackCount + count);
			float b = ((ThingWithComps)otherStack).GetComp<CompHatcher>().gestateProgress;
			this.gestateProgress = Mathf.Lerp(this.gestateProgress, b, t);
		}

		// Token: 0x06010233 RID: 66099 RVA: 0x004AB643 File Offset: 0x004A9843
		public override void PostSplitOff(Thing piece)
		{
			CompHatcher comp = ((ThingWithComps)piece).GetComp<CompHatcher>();
			comp.gestateProgress = this.gestateProgress;
			comp.hatcheeParent = this.hatcheeParent;
			comp.otherParent = this.otherParent;
			comp.hatcheeFaction = this.hatcheeFaction;
		}

		// Token: 0x06010234 RID: 66100 RVA: 0x004AB67F File Offset: 0x004A987F
		public override void PrePreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
			base.PrePreTraded(action, playerNegotiator, trader);
			if (action == TradeAction.PlayerBuys)
			{
				this.hatcheeFaction = Faction.OfPlayer;
				return;
			}
			if (action == TradeAction.PlayerSells)
			{
				this.hatcheeFaction = trader.Faction;
			}
		}

		// Token: 0x06010235 RID: 66101 RVA: 0x004AB6AA File Offset: 0x004A98AA
		public override void PostPostGeneratedForTrader(TraderKindDef trader, PlanetTile forTile, Faction forFaction)
		{
			base.PostPostGeneratedForTrader(trader, forTile, forFaction);
			this.hatcheeFaction = forFaction;
		}

		// Token: 0x06010236 RID: 66102 RVA: 0x004AB6BC File Offset: 0x004A98BC
		public override string CompInspectStringExtra()
		{
			if (!this.TemperatureDamaged)
			{
				return "EggProgress".Translate() + ": " + this.gestateProgress.ToStringPercent() + "\n" + "HatchesIn".Translate() + ": " + "PeriodDays".Translate((this.Props.hatcherDaystoHatch * (1f - this.gestateProgress)).ToString("F1"));
			}
			return null;
		}

		// Token: 0x0400A891 RID: 43153
		private float gestateProgress;

		// Token: 0x0400A892 RID: 43154
		public Pawn hatcheeParent;

		// Token: 0x0400A893 RID: 43155
		public Pawn otherParent;

		// Token: 0x0400A894 RID: 43156
		public Faction hatcheeFaction;
	}
}
