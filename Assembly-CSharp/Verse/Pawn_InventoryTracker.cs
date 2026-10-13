using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020007E4 RID: 2020
	[StaticConstructorOnStartup]
	public class Pawn_InventoryTracker : IThingHolder, IExposable
	{
		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x0600338B RID: 13195 RVA: 0x0010EA32 File Offset: 0x0010CC32
		// (set) Token: 0x0600338C RID: 13196 RVA: 0x0010EA44 File Offset: 0x0010CC44
		public bool UnloadEverything
		{
			get
			{
				return this.unloadEverything && this.HasAnyUnloadableThing;
			}
			set
			{
				if (value && this.HasAnyUnloadableThing)
				{
					this.unloadEverything = true;
					return;
				}
				this.unloadEverything = false;
			}
		}

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x0600338D RID: 13197 RVA: 0x0010EA60 File Offset: 0x0010CC60
		public bool HasAnyUnpackedCaravanItems
		{
			get
			{
				return this.unpackedCaravanItems.Count > 0;
			}
		}

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x0600338E RID: 13198 RVA: 0x0010EA70 File Offset: 0x0010CC70
		private bool HasAnyUnloadableThing
		{
			get
			{
				return this.FirstUnloadableThing != default(ThingCount);
			}
		}

		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x0600338F RID: 13199 RVA: 0x0010EA94 File Offset: 0x0010CC94
		public ThingCount FirstUnloadableThing
		{
			get
			{
				if (this.innerContainer.Count == 0)
				{
					return default(ThingCount);
				}
				Pawn_DrugPolicyTracker drugs = this.pawn.drugs;
				if (((drugs != null) ? drugs.CurrentPolicy : null) != null)
				{
					DrugPolicy currentPolicy = this.pawn.drugs.CurrentPolicy;
					Pawn_InventoryTracker.tmpItemsToKeep.Clear();
					for (int i = 0; i < currentPolicy.Count; i++)
					{
						if (currentPolicy[i].takeToInventory > 0)
						{
							Pawn_InventoryTracker.tmpItemsToKeep.Add(new ThingDefCount(currentPolicy[i].drug, currentPolicy[i].takeToInventory));
						}
					}
				}
				Pawn_InventoryStockTracker inventoryStock = this.pawn.inventoryStock;
				bool flag;
				if (inventoryStock == null)
				{
					flag = false;
				}
				else
				{
					Dictionary<InventoryStockGroupDef, InventoryStockEntry> stockEntries = inventoryStock.stockEntries;
					int? num = (stockEntries != null) ? new int?(stockEntries.Count) : null;
					int num2 = 0;
					flag = (num.GetValueOrDefault() > num2 & num != null);
				}
				if (flag)
				{
					foreach (InventoryStockEntry inventoryStockEntry in this.pawn.inventoryStock.stockEntries.Values)
					{
						Pawn_InventoryTracker.tmpItemsToKeep.Add(new ThingDefCount(inventoryStockEntry.thingDef, inventoryStockEntry.count));
					}
				}
				foreach (Thing thing in this.innerContainer)
				{
					int num3 = -1;
					for (int j = 0; j < Pawn_InventoryTracker.tmpItemsToKeep.Count; j++)
					{
						if (thing.def == Pawn_InventoryTracker.tmpItemsToKeep[j].ThingDef)
						{
							num3 = j;
							break;
						}
					}
					if (this.pawn.IsColonist && thing.def.IsNutritionGivingIngestible && !thing.def.IsDrug && JobGiver_PackFood.IsGoodPackableFoodFor(thing, this.pawn, false))
					{
						float inventoryPackableFoodNutrition = JobGiver_PackFood.GetInventoryPackableFoodNutrition(this.pawn);
						float maxLevel = this.pawn.needs.food.MaxLevel;
						if (inventoryPackableFoodNutrition - thing.GetStatValue(StatDefOf.Nutrition, true, -1) * (float)thing.stackCount <= maxLevel)
						{
							int num4 = 0;
							while (inventoryPackableFoodNutrition - thing.GetStatValue(StatDefOf.Nutrition, true, -1) * (float)num4 > maxLevel)
							{
								num4++;
							}
							if (thing.stackCount - num4 > 0)
							{
								Pawn_InventoryTracker.tmpItemsToKeep.Add(new ThingDefCount(thing.def, thing.stackCount - num4));
								num3 = Pawn_InventoryTracker.tmpItemsToKeep.Count - 1;
							}
						}
					}
					if (num3 < 0)
					{
						return new ThingCount(thing, thing.stackCount, false);
					}
					if (thing.stackCount > Pawn_InventoryTracker.tmpItemsToKeep[num3].Count)
					{
						return new ThingCount(thing, thing.stackCount - Pawn_InventoryTracker.tmpItemsToKeep[num3].Count, false);
					}
					Pawn_InventoryTracker.tmpItemsToKeep[num3] = new ThingDefCount(Pawn_InventoryTracker.tmpItemsToKeep[num3].ThingDef, Pawn_InventoryTracker.tmpItemsToKeep[num3].Count - thing.stackCount);
				}
				return default(ThingCount);
			}
		}

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x06003390 RID: 13200 RVA: 0x0010EE1C File Offset: 0x0010D01C
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x06003391 RID: 13201 RVA: 0x0010EE24 File Offset: 0x0010D024
		public Pawn_InventoryTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep, true);
		}

		// Token: 0x06003392 RID: 13202 RVA: 0x0010EE64 File Offset: 0x0010D064
		public void ExposeData()
		{
			Scribe_Collections.Look<Thing>(ref this.itemsNotForSale, "itemsNotForSale", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.unpackedCaravanItems, "unpackedCaravanItems", LookMode.Reference, Array.Empty<object>());
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.unloadEverything, "unloadEverything", false, false);
		}

		// Token: 0x06003393 RID: 13203 RVA: 0x0010EEC9 File Offset: 0x0010D0C9
		public void InventoryTrackerTick()
		{
			if (this.unloadEverything && !this.HasAnyUnloadableThing)
			{
				this.unloadEverything = false;
			}
		}

		// Token: 0x06003394 RID: 13204 RVA: 0x0010EEE2 File Offset: 0x0010D0E2
		public void DropAllNearPawn(IntVec3 pos, bool forbid = false, bool unforbid = false)
		{
			this.DropAllNearPawnHelper(pos, forbid, unforbid, false);
		}

		// Token: 0x06003395 RID: 13205 RVA: 0x0010EEF0 File Offset: 0x0010D0F0
		private void DropAllNearPawnHelper(IntVec3 pos, bool forbid = false, bool unforbid = false, bool caravanHaulOnly = false)
		{
			if (this.pawn.MapHeld == null)
			{
				string str = "Tried to drop all inventory near pawn but the pawn is unspawned. pawn=";
				Pawn pawn = this.pawn;
				Log.Error(str + ((pawn != null) ? pawn.ToString() : null));
				return;
			}
			Pawn_InventoryTracker.tmpThingList.Clear();
			if (caravanHaulOnly)
			{
				Pawn_InventoryTracker.tmpThingList.AddRange(this.unpackedCaravanItems);
			}
			else
			{
				Pawn_InventoryTracker.tmpThingList.AddRange(this.innerContainer);
			}
			int i;
			Action<Thing, int> <>9__0;
			int j;
			for (i = 0; i < Pawn_InventoryTracker.tmpThingList.Count; i = j + 1)
			{
				if (caravanHaulOnly && !this.innerContainer.Contains(Pawn_InventoryTracker.tmpThingList[i]))
				{
					this.unpackedCaravanItems.Remove(Pawn_InventoryTracker.tmpThingList[i]);
					Log.Warning("Could not drop unpacked caravan item " + Pawn_InventoryTracker.tmpThingList[i].Label + ", inventory no longer contains it");
				}
				else
				{
					ThingOwner<Thing> thingOwner = this.innerContainer;
					Thing thing = Pawn_InventoryTracker.tmpThingList[i];
					Map mapHeld = this.pawn.MapHeld;
					ThingPlaceMode mode = ThingPlaceMode.Near;
					Action<Thing, int> placedAction;
					if ((placedAction = <>9__0) == null)
					{
						placedAction = (<>9__0 = delegate(Thing t, int unused)
						{
							if (forbid)
							{
								t.SetForbiddenIfOutsideHomeArea();
							}
							if (unforbid)
							{
								t.SetForbidden(false, false);
							}
							if (t.def.IsPleasureDrug)
							{
								LessonAutoActivator.TeachOpportunity(ConceptDefOf.DrugBurning, OpportunityType.Important);
							}
							Lord formAndSendCaravanLord = CaravanFormingUtility.GetFormAndSendCaravanLord(this.pawn);
							LordJob_FormAndSendCaravan lordJob_FormAndSendCaravan = ((formAndSendCaravanLord != null) ? formAndSendCaravanLord.LordJob : null) as LordJob_FormAndSendCaravan;
							if (caravanHaulOnly && lordJob_FormAndSendCaravan != null && lordJob_FormAndSendCaravan.GatheringItemsNow)
							{
								CaravanFormingUtility.TryAddItemBackToTransferables(t, lordJob_FormAndSendCaravan.transferables, Pawn_InventoryTracker.tmpThingList[i].stackCount);
							}
							this.unpackedCaravanItems.Remove(Pawn_InventoryTracker.tmpThingList[i]);
						});
					}
					Thing thing2;
					thingOwner.TryDrop(thing, pos, mapHeld, mode, out thing2, placedAction, null);
				}
				j = i;
			}
		}

		// Token: 0x06003396 RID: 13206 RVA: 0x0010F06C File Offset: 0x0010D26C
		public void DropCount(ThingDef def, int count, bool forbid = false, bool unforbid = false)
		{
			if (this.pawn.MapHeld == null)
			{
				string str = "Tried to drop a thing near pawn but the pawn is unspawned. pawn=";
				Pawn pawn = this.pawn;
				Log.Error(str + ((pawn != null) ? pawn.ToString() : null));
				return;
			}
			Pawn_InventoryTracker.tmpThingList.Clear();
			Pawn_InventoryTracker.tmpThingList.AddRange(this.innerContainer);
			int num = 0;
			Action<Thing, int> <>9__0;
			for (int i = 0; i < Pawn_InventoryTracker.tmpThingList.Count; i++)
			{
				Thing thing = Pawn_InventoryTracker.tmpThingList[i];
				if (thing.def == def)
				{
					int num2 = Math.Min(thing.stackCount, count);
					ThingOwner<Thing> thingOwner = this.innerContainer;
					Thing thing2 = Pawn_InventoryTracker.tmpThingList[i];
					IntVec3 position = this.pawn.Position;
					Map mapHeld = this.pawn.MapHeld;
					ThingPlaceMode mode = ThingPlaceMode.Near;
					int count2 = num2;
					Action<Thing, int> placedAction;
					if ((placedAction = <>9__0) == null)
					{
						placedAction = (<>9__0 = delegate(Thing t, int unused)
						{
							if (forbid)
							{
								t.SetForbiddenIfOutsideHomeArea();
							}
							if (unforbid)
							{
								t.SetForbidden(false, false);
							}
							if (t.def.IsPleasureDrug)
							{
								LessonAutoActivator.TeachOpportunity(ConceptDefOf.DrugBurning, OpportunityType.Important);
							}
						});
					}
					Thing thing3;
					thingOwner.TryDrop(thing2, position, mapHeld, mode, count2, out thing3, placedAction, null);
					num += num2;
					if (num >= count)
					{
						break;
					}
				}
			}
		}

		// Token: 0x06003397 RID: 13207 RVA: 0x0010F174 File Offset: 0x0010D374
		public void RemoveCount(ThingDef def, int count, bool destroy = true)
		{
			Pawn_InventoryTracker.tmpThingList.Clear();
			Pawn_InventoryTracker.tmpThingList.AddRange(this.innerContainer);
			foreach (Thing thing in Pawn_InventoryTracker.tmpThingList)
			{
				if (thing.def == def)
				{
					if (thing.stackCount > count)
					{
						thing.stackCount -= count;
						break;
					}
					this.innerContainer.Remove(thing);
					if (destroy)
					{
						thing.Destroy(DestroyMode.Vanish);
						break;
					}
					break;
				}
			}
		}

		// Token: 0x06003398 RID: 13208 RVA: 0x0010F214 File Offset: 0x0010D414
		public void DestroyAll(DestroyMode mode = DestroyMode.Vanish)
		{
			this.innerContainer.ClearAndDestroyContents(mode);
		}

		// Token: 0x06003399 RID: 13209 RVA: 0x0010F222 File Offset: 0x0010D422
		public bool Contains(Thing item)
		{
			return this.innerContainer.Contains(item);
		}

		// Token: 0x0600339A RID: 13210 RVA: 0x0010F230 File Offset: 0x0010D430
		public int Count(ThingDef def)
		{
			int num = 0;
			foreach (Thing thing in this.innerContainer)
			{
				if (thing.def == def)
				{
					num += thing.stackCount;
				}
			}
			return num;
		}

		// Token: 0x0600339B RID: 13211 RVA: 0x0010F294 File Offset: 0x0010D494
		public int Count(Func<Thing, bool> validator)
		{
			int num = 0;
			foreach (Thing thing in this.innerContainer)
			{
				if (validator(thing))
				{
					num += thing.stackCount;
				}
			}
			return num;
		}

		// Token: 0x0600339C RID: 13212 RVA: 0x0010F2F8 File Offset: 0x0010D4F8
		public void AddHauledCaravanItem(Thing item)
		{
			Thing thing;
			if (this.pawn.carryTracker.innerContainer.TryTransferToContainer(item, this.innerContainer, item.stackCount, out thing, false) > 0)
			{
				this.unpackedCaravanItems.Add(thing);
			}
			CompForbiddable compForbiddable = (thing != null) ? thing.TryGetComp<CompForbiddable>() : null;
			if (compForbiddable != null)
			{
				compForbiddable.Forbidden = false;
			}
		}

		// Token: 0x0600339D RID: 13213 RVA: 0x0010F350 File Offset: 0x0010D550
		public void TryAddAndUnforbid(Thing item)
		{
			CompForbiddable compForbiddable = item.TryGetComp<CompForbiddable>();
			if (this.innerContainer.TryAdd(item, true) && compForbiddable != null)
			{
				compForbiddable.Forbidden = false;
			}
		}

		// Token: 0x0600339E RID: 13214 RVA: 0x0010F380 File Offset: 0x0010D580
		public void TransferCaravanItemsToCarrier(Pawn_InventoryTracker carrierInventory)
		{
			List<Thing> list = new List<Thing>();
			list.AddRange(this.pawn.inventory.unpackedCaravanItems);
			foreach (Thing thing in list)
			{
				if (MassUtility.IsOverEncumbered(carrierInventory.pawn))
				{
					break;
				}
				if (this.innerContainer.Contains(thing))
				{
					this.pawn.inventory.innerContainer.TryTransferToContainer(thing, carrierInventory.innerContainer, thing.stackCount, true);
				}
				this.unpackedCaravanItems.Remove(thing);
			}
		}

		// Token: 0x0600339F RID: 13215 RVA: 0x0010F430 File Offset: 0x0010D630
		public void DropAllPackingCaravanThings()
		{
			if (this.pawn.Spawned)
			{
				this.DropAllNearPawnHelper(this.pawn.Position, false, false, true);
				this.ClearHaulingCaravanCache();
			}
		}

		// Token: 0x060033A0 RID: 13216 RVA: 0x0010F459 File Offset: 0x0010D659
		public void ClearHaulingCaravanCache()
		{
			this.unpackedCaravanItems.Clear();
		}

		// Token: 0x060033A1 RID: 13217 RVA: 0x0010F466 File Offset: 0x0010D666
		public bool NotForSale(Thing item)
		{
			return this.itemsNotForSale.Contains(item);
		}

		// Token: 0x060033A2 RID: 13218 RVA: 0x0010F474 File Offset: 0x0010D674
		public void TryAddItemNotForSale(Thing item)
		{
			if (this.innerContainer.TryAdd(item, false))
			{
				this.itemsNotForSale.Add(item);
			}
		}

		// Token: 0x060033A3 RID: 13219 RVA: 0x0010F491 File Offset: 0x0010D691
		public void Notify_ItemRemoved(Thing item)
		{
			this.itemsNotForSale.Remove(item);
			this.unpackedCaravanItems.Remove(item);
			if (this.unloadEverything && !this.HasAnyUnloadableThing)
			{
				this.unloadEverything = false;
			}
		}

		// Token: 0x060033A4 RID: 13220 RVA: 0x0010F4C4 File Offset: 0x0010D6C4
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x060033A5 RID: 13221 RVA: 0x0010F4CC File Offset: 0x0010D6CC
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x060033A6 RID: 13222 RVA: 0x0010F4DA File Offset: 0x0010D6DA
		public IEnumerable<Thing> GetDrugs()
		{
			foreach (Thing thing in this.innerContainer)
			{
				if (thing.TryGetComp<CompDrug>() != null)
				{
					yield return thing;
				}
			}
			List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060033A7 RID: 13223 RVA: 0x0010F4EA File Offset: 0x0010D6EA
		public IEnumerable<Thing> GetCombatEnhancingDrugs()
		{
			foreach (Thing thing in this.innerContainer)
			{
				CompDrug compDrug = thing.TryGetComp<CompDrug>();
				if (compDrug != null && compDrug.Props.isCombatEnhancingDrug)
				{
					yield return thing;
				}
			}
			List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060033A8 RID: 13224 RVA: 0x0010F4FA File Offset: 0x0010D6FA
		public Thing FindCombatEnhancingDrug()
		{
			return this.GetCombatEnhancingDrugs().FirstOrDefault<Thing>();
		}

		// Token: 0x060033A9 RID: 13225 RVA: 0x0010F507 File Offset: 0x0010D707
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (this.pawn.IsColonistPlayerControlled && this.pawn.Drafted && Find.Selector.SingleSelectedThing == this.pawn)
			{
				this.usableDrugsTmp.Clear();
				foreach (Thing thing in this.GetDrugs())
				{
					if (FoodUtility.WillIngestFromInventoryNow(this.pawn, thing))
					{
						this.usableDrugsTmp.Add(thing);
					}
				}
				if (this.usableDrugsTmp.Count == 0)
				{
					yield break;
				}
				if (this.usableDrugsTmp.Count == 1)
				{
					Thing drug = this.usableDrugsTmp[0];
					string defaultLabel = drug.def.ingestible.ingestCommandString.NullOrEmpty() ? "ConsumeThing".Translate(drug.LabelNoCount, drug) : drug.def.ingestible.ingestCommandString.Formatted(drug.LabelShort);
					yield return new Command_Action
					{
						defaultLabel = defaultLabel,
						defaultDesc = drug.LabelCapNoCount + ": " + drug.def.description.CapitalizeFirst(),
						icon = drug.def.uiIcon,
						iconAngle = drug.def.uiIconAngle,
						iconOffset = drug.def.uiIconOffset,
						action = delegate()
						{
							FoodUtility.IngestFromInventoryNow(this.pawn, drug);
						}
					};
				}
				else
				{
					yield return new Command_Action
					{
						defaultLabel = "TakeDrug".Translate(),
						defaultDesc = "TakeDrugDesc".Translate(),
						icon = Pawn_InventoryTracker.DrugTex,
						action = delegate()
						{
							List<FloatMenuOption> list = new List<FloatMenuOption>();
							using (List<Thing>.Enumerator enumerator2 = this.usableDrugsTmp.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									Thing drug = enumerator2.Current;
									string label = drug.def.ingestible.ingestCommandString.NullOrEmpty() ? "ConsumeThing".Translate(drug.LabelNoCount, drug) : drug.def.ingestible.ingestCommandString.Formatted(drug.LabelShort);
									list.Add(new FloatMenuOption(label, delegate()
									{
										FoodUtility.IngestFromInventoryNow(this.pawn, drug);
									}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
								}
							}
							Find.WindowStack.Add(new FloatMenu(list));
						}
					};
				}
			}
			yield break;
		}

		// Token: 0x040026FD RID: 9981
		public Pawn pawn;

		// Token: 0x040026FE RID: 9982
		public ThingOwner<Thing> innerContainer;

		// Token: 0x040026FF RID: 9983
		private bool unloadEverything;

		// Token: 0x04002700 RID: 9984
		private List<Thing> itemsNotForSale = new List<Thing>();

		// Token: 0x04002701 RID: 9985
		private List<Thing> unpackedCaravanItems = new List<Thing>();

		// Token: 0x04002702 RID: 9986
		public static readonly Texture2D DrugTex = ContentFinder<Texture2D>.Get("UI/Commands/TakeDrug", true);

		// Token: 0x04002703 RID: 9987
		private static List<ThingDefCount> tmpItemsToKeep = new List<ThingDefCount>();

		// Token: 0x04002704 RID: 9988
		private static readonly List<Thing> tmpThingList = new List<Thing>();

		// Token: 0x04002705 RID: 9989
		private List<Thing> usableDrugsTmp = new List<Thing>();
	}
}
