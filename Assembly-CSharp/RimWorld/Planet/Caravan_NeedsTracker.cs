using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D94 RID: 15764
	public class Caravan_NeedsTracker : IExposable
	{
		// Token: 0x06015798 RID: 87960 RVA: 0x0066F203 File Offset: 0x0066D403
		public Caravan_NeedsTracker()
		{
		}

		// Token: 0x06015799 RID: 87961 RVA: 0x0066F216 File Offset: 0x0066D416
		public Caravan_NeedsTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x0601579A RID: 87962 RVA: 0x0066F230 File Offset: 0x0066D430
		public void ExposeData()
		{
			Scribe_Collections.Look<Pawn, Pawn>(ref this.breastfeedingBabyToFeeder, "breastfeedingBabyToFeeder", LookMode.Reference, LookMode.Reference);
		}

		// Token: 0x0601579B RID: 87963 RVA: 0x0066F244 File Offset: 0x0066D444
		public void NeedsTrackerTickInterval(int delta)
		{
			this.TrySatisfyPawnsNeeds(delta);
		}

		// Token: 0x17003568 RID: 13672
		// (get) Token: 0x0601579C RID: 87964 RVA: 0x0066F250 File Offset: 0x0066D450
		public bool AnyPawnsNeedRest
		{
			get
			{
				List<Pawn> pawnsListForReading = this.caravan.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					Pawn_NeedsTracker needs = pawnsListForReading[i].needs;
					if (((needs != null) ? needs.rest : null) != null)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x0601579D RID: 87965 RVA: 0x0066F298 File Offset: 0x0066D498
		public void TrySatisfyPawnsNeeds(int delta)
		{
			this.tmpPawns.Clear();
			this.tmpPawns.AddRange(this.caravan.PawnsListForReading.ToList<Pawn>());
			this.tmpPawns.Sort((Pawn p1, Pawn p2) => p1.RaceProps.Humanlike.CompareTo(p2.RaceProps.Humanlike));
			for (int i = this.tmpPawns.Count - 1; i >= 0; i--)
			{
				this.TrySatisfyPawnNeeds(this.tmpPawns[i], delta);
			}
		}

		// Token: 0x0601579E RID: 87966 RVA: 0x0066F320 File Offset: 0x0066D520
		private void TrySatisfyPawnNeeds(Pawn pawn, int delta)
		{
			if (pawn.Dead)
			{
				return;
			}
			List<Need> allNeeds = pawn.needs.AllNeeds;
			for (int i = 0; i < allNeeds.Count; i++)
			{
				Need need = allNeeds[i];
				Need_Rest need_Rest = need as Need_Rest;
				if (need_Rest != null)
				{
					this.TrySatisfyRestNeed(pawn, need_Rest, delta);
				}
				else
				{
					Need_Food need_Food = need as Need_Food;
					if (need_Food != null)
					{
						this.TrySatisfyFoodNeed(pawn, need_Food, delta);
					}
					else
					{
						Need_Chemical need_Chemical = need as Need_Chemical;
						if (need_Chemical != null)
						{
							this.TrySatisfyChemicalNeed(pawn, need_Chemical, delta);
						}
						else
						{
							Need_Joy need_Joy = need as Need_Joy;
							if (need_Joy != null)
							{
								this.TrySatisfyJoyNeed(pawn, need_Joy, delta);
							}
						}
					}
				}
			}
			if (ModsConfig.BiotechActive && pawn.genes != null)
			{
				Gene_Hemogen firstGeneOfType = pawn.genes.GetFirstGeneOfType<Gene_Hemogen>();
				if (firstGeneOfType != null)
				{
					this.TrySatisfyHemogenNeed(pawn, firstGeneOfType, delta);
				}
			}
			Pawn_PsychicEntropyTracker psychicEntropy = pawn.psychicEntropy;
			if (((psychicEntropy != null) ? psychicEntropy.Psylink : null) != null)
			{
				this.TryGainPsyfocus(psychicEntropy, delta);
			}
			this.TrySatisfyChemicalDependencies(pawn, delta);
		}

		// Token: 0x0601579F RID: 87967 RVA: 0x0066F408 File Offset: 0x0066D608
		private void TrySatisfyRestNeed(Pawn pawn, Need_Rest rest, int delta)
		{
			if (!this.caravan.pather.MovingNow || pawn.InCaravanBed() || pawn.CarriedByCaravan())
			{
				Building_Bed building_Bed = pawn.CurrentCaravanBed();
				float restEffectiveness = (building_Bed != null) ? building_Bed.GetStatValue(StatDefOf.BedRestEffectiveness, true, -1) : StatDefOf.BedRestEffectiveness.valueIfMissing;
				rest.TickResting(restEffectiveness);
			}
		}

		// Token: 0x060157A0 RID: 87968 RVA: 0x0066F464 File Offset: 0x0066D664
		private void TrySatisfyFoodNeed(Pawn pawn, Need_Food food, int delta)
		{
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (!ChildcareUtility.CanSuckle(pawn, out breastfeedFailReason))
			{
				Dictionary<Pawn, Pawn> dictionary = this.breastfeedingBabyToFeeder;
				if (dictionary != null)
				{
					dictionary.Remove(pawn);
				}
			}
			else
			{
				this.breastfeedingBabyToFeeder = (this.breastfeedingBabyToFeeder ?? new Dictionary<Pawn, Pawn>(8));
				Pawn pawn2;
				if (this.breastfeedingBabyToFeeder.TryGetValue(pawn, out pawn2))
				{
					if (!this.caravan.PawnsListForReading.Contains(pawn2))
					{
						this.breastfeedingBabyToFeeder.Remove(pawn);
					}
					else if (!ChildcareUtility.CanBreastfeed(pawn2, out breastfeedFailReason))
					{
						this.breastfeedingBabyToFeeder.Remove(pawn);
					}
					else
					{
						if (ChildcareUtility.SuckleFromLactatingPawn(pawn, pawn2, delta))
						{
							return;
						}
						this.breastfeedingBabyToFeeder.Remove(pawn);
					}
				}
				if (!ChildcareUtility.WantsSuckle(pawn, out breastfeedFailReason))
				{
					return;
				}
				foreach (Pawn pawn3 in this.caravan.pawns)
				{
					if (!this.breastfeedingBabyToFeeder.ContainsValue(pawn3) && ChildcareUtility.CanMomBreastfeedBabyNow(pawn3, pawn, out breastfeedFailReason) && ChildcareUtility.CanAutoBreastfeed(pawn3, pawn, false, out breastfeedFailReason))
					{
						this.breastfeedingBabyToFeeder[pawn] = pawn3;
						if (ChildcareUtility.SuckleFromLactatingPawn(pawn, pawn3, delta))
						{
							return;
						}
					}
				}
			}
			if (!food.Starving)
			{
				food.lastNonStarvingTick = Find.TickManager.TicksGame;
			}
			if (food.CurCategory < HungerCategory.Hungry)
			{
				return;
			}
			if (VirtualPlantsUtility.CanEatVirtualPlantsNow(pawn))
			{
				VirtualPlantsUtility.EatVirtualPlants(pawn);
				return;
			}
			Thing thing;
			Pawn pawn4;
			if (CaravanInventoryUtility.TryGetBestFood(this.caravan, pawn, out thing, out pawn4))
			{
				food.CurLevel += thing.Ingested(pawn, food.NutritionWanted);
				if (thing.Destroyed)
				{
					if (pawn4 != null)
					{
						pawn4.inventory.innerContainer.Remove(thing);
						this.caravan.RecacheInventory();
					}
					if (!this.caravan.notifiedOutOfFood && !CaravanInventoryUtility.TryGetBestFood(this.caravan, pawn, out thing, out pawn4))
					{
						Messages.Message("MessageCaravanRanOutOfFood".Translate(this.caravan.LabelCap), this.caravan, MessageTypeDefOf.ThreatBig, true);
						this.caravan.notifiedOutOfFood = true;
					}
				}
			}
		}

		// Token: 0x060157A1 RID: 87969 RVA: 0x0066F694 File Offset: 0x0066D894
		private void TrySatisfyHemogenNeed(Pawn pawn, Gene_Hemogen hemogenGene, int delta)
		{
			if (hemogenGene.ShouldConsumeHemogenNow())
			{
				Thing thing = CaravanInventoryUtility.AllInventoryItems(this.caravan).FirstOrFallback((Thing t) => t.def == ThingDefOf.HemogenPack, null);
				if (thing != null)
				{
					Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(this.caravan, thing);
					float num = thing.Ingested(pawn, thing.GetStatValue(StatDefOf.Nutrition, true, -1));
					Pawn_NeedsTracker needs = pawn.needs;
					if (((needs != null) ? needs.food : null) != null)
					{
						pawn.needs.food.CurLevel += num;
					}
					if (thing.Destroyed && ownerOf != null)
					{
						ownerOf.inventory.innerContainer.Remove(thing);
						this.caravan.RecacheInventory();
					}
				}
			}
		}

		// Token: 0x060157A2 RID: 87970 RVA: 0x0066F758 File Offset: 0x0066D958
		private void TrySatisfyChemicalNeed(Pawn pawn, Need_Chemical chemical, int delta)
		{
			if (chemical.CurCategory >= DrugDesireCategory.Satisfied)
			{
				return;
			}
			Thing drug;
			Pawn drugOwner;
			if (CaravanInventoryUtility.TryGetDrugToSatisfyChemicalNeed(this.caravan, pawn, chemical.AddictionHediff, out drug, out drugOwner))
			{
				this.IngestDrug(pawn, drug, drugOwner);
			}
		}

		// Token: 0x060157A3 RID: 87971 RVA: 0x0066F790 File Offset: 0x0066D990
		private void TrySatisfyChemicalDependencies(Pawn pawn, int delta)
		{
			Caravan_NeedsTracker.tmpHediffs.Clear();
			Caravan_NeedsTracker.tmpHediffs.AddRange(pawn.health.hediffSet.hediffs);
			foreach (Hediff hediff in Caravan_NeedsTracker.tmpHediffs)
			{
				Hediff_ChemicalDependency hediff_ChemicalDependency = hediff as Hediff_ChemicalDependency;
				Thing drug;
				Pawn drugOwner;
				if (hediff_ChemicalDependency != null && hediff_ChemicalDependency.ShouldSatify && CaravanInventoryUtility.TryGetDrugToSatisfyChemicalNeed(this.caravan, pawn, hediff_ChemicalDependency, out drug, out drugOwner))
				{
					this.IngestDrug(pawn, drug, drugOwner);
				}
			}
		}

		// Token: 0x060157A4 RID: 87972 RVA: 0x0066F82C File Offset: 0x0066DA2C
		public void IngestDrug(Pawn pawn, Thing drug, Pawn drugOwner)
		{
			float num = drug.Ingested(pawn, 0f);
			Need_Food food = pawn.needs.food;
			if (food != null)
			{
				food.CurLevel += num;
			}
			if (drug.Destroyed && drugOwner != null)
			{
				drugOwner.inventory.innerContainer.Remove(drug);
				this.caravan.RecacheInventory();
			}
		}

		// Token: 0x060157A5 RID: 87973 RVA: 0x0066F88C File Offset: 0x0066DA8C
		private void TrySatisfyJoyNeed(Pawn pawn, Need_Joy joy, int delta)
		{
			if (pawn.IsHashIntervalTick(1250, delta))
			{
				float num = this.GetCurrentJoyGainPerTick(pawn);
				if (num <= 0f)
				{
					return;
				}
				num *= 1250f;
				Caravan_NeedsTracker.tmpAvailableJoyKinds.Clear();
				this.GetAvailableJoyKindsFor(pawn, Caravan_NeedsTracker.tmpAvailableJoyKinds);
				JoyKindDef joyKind;
				if (!Caravan_NeedsTracker.tmpAvailableJoyKinds.TryRandomElementByWeight((JoyKindDef x) => 1f - Mathf.Clamp01(pawn.needs.joy.tolerances[x]), out joyKind))
				{
					return;
				}
				joy.GainJoy(num, joyKind);
				Caravan_NeedsTracker.tmpAvailableJoyKinds.Clear();
			}
		}

		// Token: 0x060157A6 RID: 87974 RVA: 0x0066F91F File Offset: 0x0066DB1F
		public float GetCurrentJoyGainPerTick(Pawn pawn)
		{
			if (this.caravan.pather.MovingNow)
			{
				return 0f;
			}
			return 4E-05f;
		}

		// Token: 0x060157A7 RID: 87975 RVA: 0x0066F93E File Offset: 0x0066DB3E
		public void TryGainPsyfocus(Pawn_PsychicEntropyTracker tracker, int delta)
		{
			if (!this.caravan.pather.MovingNow && !this.caravan.NightResting)
			{
				tracker.GainPsyfocus_NewTemp(delta, null);
			}
		}

		// Token: 0x060157A8 RID: 87976 RVA: 0x0066F968 File Offset: 0x0066DB68
		public bool AnyPawnOutOfFood(out string malnutritionHediff)
		{
			Caravan_NeedsTracker.tmpInvFood.Clear();
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(this.caravan);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.IsNutritionGivingIngestible)
				{
					Caravan_NeedsTracker.tmpInvFood.Add(list[i]);
				}
			}
			List<Pawn> pawnsListForReading = this.caravan.PawnsListForReading;
			for (int j = 0; j < pawnsListForReading.Count; j++)
			{
				Pawn pawn = pawnsListForReading[j];
				if (pawn.RaceProps.EatsFood)
				{
					Pawn_NeedsTracker needs = pawn.needs;
					if (((needs != null) ? needs.food : null) != null && !VirtualPlantsUtility.CanEatVirtualPlantsNow(pawn))
					{
						bool flag = false;
						for (int k = 0; k < Caravan_NeedsTracker.tmpInvFood.Count; k++)
						{
							if (CaravanPawnsNeedsUtility.CanEatForNutritionEver(Caravan_NeedsTracker.tmpInvFood[k].def, pawn))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							int num = -1;
							string text = null;
							for (int l = 0; l < pawnsListForReading.Count; l++)
							{
								Hediff firstHediffOfDef = pawnsListForReading[l].health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Malnutrition, false);
								if (firstHediffOfDef != null && (text == null || firstHediffOfDef.CurStageIndex > num))
								{
									num = firstHediffOfDef.CurStageIndex;
									text = firstHediffOfDef.LabelCap;
								}
							}
							malnutritionHediff = text;
							Caravan_NeedsTracker.tmpInvFood.Clear();
							return true;
						}
					}
				}
			}
			malnutritionHediff = null;
			Caravan_NeedsTracker.tmpInvFood.Clear();
			return false;
		}

		// Token: 0x060157A9 RID: 87977 RVA: 0x0066FAE0 File Offset: 0x0066DCE0
		private void GetAvailableJoyKindsFor(Pawn p, List<JoyKindDef> outJoyKinds)
		{
			outJoyKinds.Clear();
			if (!p.needs.joy.tolerances.BoredOf(JoyKindDefOf.Meditative))
			{
				outJoyKinds.Add(JoyKindDefOf.Meditative);
			}
			if (!p.needs.joy.tolerances.BoredOf(JoyKindDefOf.Social))
			{
				int num = 0;
				for (int i = 0; i < this.caravan.pawns.Count; i++)
				{
					if (this.caravan.pawns[i].RaceProps.Humanlike && !this.caravan.pawns[i].Downed && !this.caravan.pawns[i].InMentalState)
					{
						num++;
					}
				}
				if (num >= 2)
				{
					outJoyKinds.Add(JoyKindDefOf.Social);
				}
			}
		}

		// Token: 0x0400EF00 RID: 61184
		public Caravan caravan;

		// Token: 0x0400EF01 RID: 61185
		public Dictionary<Pawn, Pawn> breastfeedingBabyToFeeder;

		// Token: 0x0400EF02 RID: 61186
		private readonly List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x0400EF03 RID: 61187
		private static List<Hediff> tmpHediffs = new List<Hediff>();

		// Token: 0x0400EF04 RID: 61188
		private static List<JoyKindDef> tmpAvailableJoyKinds = new List<JoyKindDef>();

		// Token: 0x0400EF05 RID: 61189
		private static List<Thing> tmpInvFood = new List<Thing>();
	}
}
