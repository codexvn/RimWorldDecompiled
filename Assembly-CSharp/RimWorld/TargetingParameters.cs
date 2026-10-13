using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003761 RID: 14177
	public class TargetingParameters
	{
		// Token: 0x06013A67 RID: 80487 RVA: 0x005D67E8 File Offset: 0x005D49E8
		public bool CanTarget(TargetInfo targ, ITargetingSource source = null)
		{
			if (this.validator != null && !this.validator(targ))
			{
				return false;
			}
			if (targ.Thing == null)
			{
				return this.canTargetLocations;
			}
			if (this.neverTargetDoors && targ.Thing.def.IsDoor)
			{
				return false;
			}
			if (this.onlyTargetDamagedThings && targ.Thing.HitPoints == targ.Thing.MaxHitPoints)
			{
				return false;
			}
			if (this.onlyTargetFlammables && !targ.Thing.FlammableNow)
			{
				return false;
			}
			if (this.mustBeSelectable && !ThingSelectionUtility.SelectableByMapClick(targ.Thing))
			{
				return false;
			}
			if (this.onlyTargetColonistsOrPrisoners && targ.Thing.def.category != ThingCategory.Pawn)
			{
				return false;
			}
			if (this.onlyTargetColonistsOrPrisonersOrSlaves && targ.Thing.def.category != ThingCategory.Pawn)
			{
				return false;
			}
			if (this.onlyTargetDoors && !targ.Thing.def.IsDoor)
			{
				return false;
			}
			Corpse corpse;
			if ((corpse = (targ.Thing as Corpse)) == null)
			{
				Pawn pawn = targ.Thing as Pawn;
				corpse = ((pawn != null) ? pawn.Corpse : null);
			}
			Corpse corpse2 = corpse;
			if (this.canTargetCorpses && corpse2 != null)
			{
				return (this.canTargetMechs || !corpse2.InnerPawn.RaceProps.IsMechanoid) && (this.canTargetAnimals || !corpse2.InnerPawn.RaceProps.Animal) && (this.canTargetHumans || !corpse2.InnerPawn.RaceProps.Humanlike) && (this.canTargetSubhumans || !corpse2.InnerPawn.IsSubhuman);
			}
			if (this.onlyTargetCorpses)
			{
				return false;
			}
			if (this.targetSpecificThing != null && targ.Thing == this.targetSpecificThing)
			{
				return true;
			}
			if (this.canTargetFires && targ.Thing.def == ThingDefOf.Fire)
			{
				return true;
			}
			if (this.canTargetPawns && targ.Thing.def.category == ThingCategory.Pawn)
			{
				Pawn pawn2 = (Pawn)targ.Thing;
				if (pawn2.Downed)
				{
					if (this.neverTargetIncapacitated)
					{
						return false;
					}
				}
				else if (this.onlyTargetIncapacitatedPawns)
				{
					return false;
				}
				if (this.onlyTargetFactions != null && !this.onlyTargetFactions.Contains(targ.Thing.Faction))
				{
					return false;
				}
				if (pawn2.NonHumanlikeOrWildMan())
				{
					if (pawn2.Faction != null && pawn2.RaceProps.IsMechanoid)
					{
						if (!this.canTargetMechs)
						{
							return false;
						}
						if (this.onlyRepairableMechs && !MechRepairUtility.CanRepair(pawn2))
						{
							return false;
						}
					}
					else if (!this.canTargetAnimals)
					{
						return false;
					}
				}
				if (!pawn2.NonHumanlikeOrWildMan() && !this.canTargetHumans)
				{
					return false;
				}
				if (!this.canTargetEntities && pawn2.IsEntity)
				{
					return false;
				}
				if (!this.canTargetSubhumans && pawn2.IsSubhuman)
				{
					return false;
				}
				if (this.onlyTargetControlledPawns && !pawn2.IsColonistPlayerControlled)
				{
					return false;
				}
				if (this.onlyTargetColonists && (!pawn2.IsColonist || pawn2.HostFaction != null))
				{
					return false;
				}
				if (this.onlyTargetPrisonersOfColony && !pawn2.IsPrisonerOfColony)
				{
					return false;
				}
				if (this.onlyTargetColonistsOrPrisoners && !pawn2.IsColonistPlayerControlled && !pawn2.IsPrisonerOfColony)
				{
					return false;
				}
				if (this.onlyTargetColonistsOrPrisonersOrSlaves && !pawn2.IsColonistPlayerControlled && !pawn2.IsPrisonerOfColony && !pawn2.IsSlaveOfColony)
				{
					return false;
				}
				if (this.onlyTargetColonistsOrPrisonersOrSlavesAllowMinorMentalBreaks)
				{
					if (!pawn2.IsPrisonerOfColony && !pawn2.IsSlaveOfColony && (!pawn2.IsColonist || (pawn2.HostFaction != null && !pawn2.IsSlave)))
					{
						return false;
					}
					MentalStateDef mentalStateDef = pawn2.MentalStateDef;
					if (mentalStateDef != null && mentalStateDef.IsAggro)
					{
						return false;
					}
				}
				if (this.onlyTargetPsychicSensitive && pawn2.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) <= 0f)
				{
					return false;
				}
				if (this.neverTargetHostileFaction && !pawn2.IsPrisonerOfColony && !pawn2.IsSlaveOfColony)
				{
					Faction homeFaction = pawn2.HomeFaction;
					if (homeFaction != null && homeFaction.HostileTo(Faction.OfPlayer))
					{
						return false;
					}
				}
				if (this.onlyTargetSameIdeo)
				{
					if (source == null)
					{
						Log.Error("Source passed in is null but targeting parameters have onlyTargetSameIdeo set.");
					}
					else
					{
						Verb verb = source as Verb;
						if (verb != null && verb.CasterPawn != null)
						{
							Pawn pawn3 = targ.Thing as Pawn;
							Ideo ideo = (pawn3 != null) ? pawn3.Ideo : null;
							if (verb.CasterPawn.Ideo != ideo)
							{
								return false;
							}
						}
						else
						{
							Log.Error("Source passed in is incompatible type but targeting parameters have onlyTargetSameIdeo set.");
						}
					}
				}
				return this.canTargetBloodfeeders || !ModsConfig.BiotechActive || !pawn2.IsBloodfeeder();
			}
			else
			{
				if (this.canTargetBuildings && targ.Thing.def.category == ThingCategory.Building)
				{
					return (!this.mapObjectTargetsMustBeAutoAttackable || targ.Thing.def.building.isTargetable) && (!this.onlyTargetThingsAffectingRegions || targ.Thing.def.AffectsRegions) && (this.onlyTargetFactions == null || this.onlyTargetFactions.Contains(targ.Thing.Faction));
				}
				if (this.canTargetPlants && targ.Thing.def.category == ThingCategory.Plant)
				{
					return !ModsConfig.RoyaltyActive || !this.onlyTargetAnimaTrees || targ.Thing.def == ThingDefOf.Plant_TreeAnima;
				}
				if (this.canTargetItems)
				{
					if (this.mapObjectTargetsMustBeAutoAttackable && !targ.Thing.def.isAutoAttackableMapObject)
					{
						return false;
					}
					if (this.thingCategory == ThingCategory.None || this.thingCategory == targ.Thing.def.category)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06013A68 RID: 80488 RVA: 0x005D6D4A File Offset: 0x005D4F4A
		public static TargetingParameters ForAttackAny()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = true,
				canTargetItems = true,
				mapObjectTargetsMustBeAutoAttackable = true
			};
		}

		// Token: 0x06013A69 RID: 80489 RVA: 0x005D6D70 File Offset: 0x005D4F70
		public static TargetingParameters ForForceWear(Pawn selectedPawnForJob)
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetAnimals = false,
				canTargetMechs = false,
				canTargetItems = false,
				canTargetBuildings = true,
				mapObjectTargetsMustBeAutoAttackable = false,
				validator = delegate(TargetInfo targ)
				{
					if (!targ.HasThing)
					{
						return false;
					}
					Pawn pawn = targ.Thing as Pawn;
					if (pawn == null)
					{
						return ModsConfig.OdysseyActive && targ.Thing is Building_OutfitStand;
					}
					return pawn != selectedPawnForJob && pawn.kindDef.canStrip && ((pawn.Downed && pawn.RaceProps.Humanlike) || (pawn.IsPrisonerOfColony && pawn.guest.PrisonerIsSecure) || (!pawn.IsQuestLodger() && pawn.IsColonist));
				}
			};
		}

		// Token: 0x06013A6A RID: 80490 RVA: 0x005D6DCC File Offset: 0x005D4FCC
		public static TargetingParameters ForDropPodsDestination()
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetLocations = true;
			targetingParameters.canTargetSelf = false;
			targetingParameters.canTargetPawns = false;
			targetingParameters.canTargetFires = false;
			targetingParameters.canTargetBuildings = false;
			targetingParameters.canTargetItems = false;
			targetingParameters.validator = ((TargetInfo x) => DropCellFinder.IsGoodDropSpot(x.Cell, x.Map, false, true, true));
			return targetingParameters;
		}

		// Token: 0x06013A6B RID: 80491 RVA: 0x005D6E30 File Offset: 0x005D5030
		public static TargetingParameters ForBuilding(ThingDef def = null)
		{
			return new TargetingParameters
			{
				canTargetPawns = false,
				canTargetItems = false,
				canTargetBuildings = true,
				validator = ((TargetInfo targ) => targ.HasThing && (def == null || targ.Thing.def == def))
			};
		}

		// Token: 0x06013A6C RID: 80492 RVA: 0x005D6E76 File Offset: 0x005D5076
		public static TargetingParameters ForColonist()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				onlyTargetColonists = true
			};
		}

		// Token: 0x06013A6D RID: 80493 RVA: 0x004E6212 File Offset: 0x004E4412
		public static TargetingParameters ForPawns()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false
			};
		}

		// Token: 0x06013A6E RID: 80494 RVA: 0x005D6E92 File Offset: 0x005D5092
		public static TargetingParameters ForThing()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = true,
				canTargetItems = true,
				canTargetPlants = true,
				canTargetFires = true,
				mapObjectTargetsMustBeAutoAttackable = false
			};
		}

		// Token: 0x06013A6F RID: 80495 RVA: 0x005D6EC3 File Offset: 0x005D50C3
		public static TargetingParameters ForCell()
		{
			return new TargetingParameters
			{
				canTargetLocations = true,
				canTargetBuildings = false,
				canTargetHumans = false,
				canTargetMechs = false,
				canTargetAnimals = false,
				onlyTargetColonists = false
			};
		}

		// Token: 0x0400D990 RID: 55696
		public bool canTargetLocations;

		// Token: 0x0400D991 RID: 55697
		public bool canTargetSelf;

		// Token: 0x0400D992 RID: 55698
		public bool canTargetPawns = true;

		// Token: 0x0400D993 RID: 55699
		public bool canTargetFires;

		// Token: 0x0400D994 RID: 55700
		public bool canTargetBuildings = true;

		// Token: 0x0400D995 RID: 55701
		public bool canTargetItems;

		// Token: 0x0400D996 RID: 55702
		public bool canTargetAnimals = true;

		// Token: 0x0400D997 RID: 55703
		public bool canTargetHumans = true;

		// Token: 0x0400D998 RID: 55704
		public bool canTargetMechs = true;

		// Token: 0x0400D999 RID: 55705
		public bool canTargetPlants;

		// Token: 0x0400D99A RID: 55706
		public bool canTargetSubhumans = true;

		// Token: 0x0400D99B RID: 55707
		public bool canTargetEntities = true;

		// Token: 0x0400D99C RID: 55708
		public List<Faction> onlyTargetFactions;

		// Token: 0x0400D99D RID: 55709
		public Predicate<TargetInfo> validator;

		// Token: 0x0400D99E RID: 55710
		public bool onlyTargetFlammables;

		// Token: 0x0400D99F RID: 55711
		public Thing targetSpecificThing;

		// Token: 0x0400D9A0 RID: 55712
		public bool mustBeSelectable;

		// Token: 0x0400D9A1 RID: 55713
		public bool neverTargetDoors;

		// Token: 0x0400D9A2 RID: 55714
		public bool neverTargetIncapacitated;

		// Token: 0x0400D9A3 RID: 55715
		public bool neverTargetHostileFaction;

		// Token: 0x0400D9A4 RID: 55716
		public bool onlyTargetSameIdeo;

		// Token: 0x0400D9A5 RID: 55717
		public bool onlyTargetThingsAffectingRegions;

		// Token: 0x0400D9A6 RID: 55718
		public bool onlyTargetDamagedThings;

		// Token: 0x0400D9A7 RID: 55719
		public bool mapObjectTargetsMustBeAutoAttackable = true;

		// Token: 0x0400D9A8 RID: 55720
		public bool onlyTargetIncapacitatedPawns;

		// Token: 0x0400D9A9 RID: 55721
		public bool onlyTargetColonistsOrPrisoners;

		// Token: 0x0400D9AA RID: 55722
		public bool onlyTargetColonistsOrPrisonersOrSlaves;

		// Token: 0x0400D9AB RID: 55723
		public bool onlyTargetColonistsOrPrisonersOrSlavesAllowMinorMentalBreaks;

		// Token: 0x0400D9AC RID: 55724
		public bool onlyTargetControlledPawns;

		// Token: 0x0400D9AD RID: 55725
		public bool onlyTargetColonists;

		// Token: 0x0400D9AE RID: 55726
		public bool onlyTargetPrisonersOfColony;

		// Token: 0x0400D9AF RID: 55727
		public bool onlyTargetPsychicSensitive;

		// Token: 0x0400D9B0 RID: 55728
		public bool onlyTargetAnimaTrees;

		// Token: 0x0400D9B1 RID: 55729
		public bool canTargetBloodfeeders = true;

		// Token: 0x0400D9B2 RID: 55730
		public bool onlyRepairableMechs;

		// Token: 0x0400D9B3 RID: 55731
		public ThingCategory thingCategory;

		// Token: 0x0400D9B4 RID: 55732
		public bool onlyTargetDoors;

		// Token: 0x0400D9B5 RID: 55733
		public bool canTargetCorpses;

		// Token: 0x0400D9B6 RID: 55734
		public bool onlyTargetCorpses;

		// Token: 0x0400D9B7 RID: 55735
		public int mapBoundsContractedBy;
	}
}
