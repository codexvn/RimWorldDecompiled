using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020022E8 RID: 8936
	public class Bill_Medical : Bill
	{
		// Token: 0x17001DBA RID: 7610
		// (get) Token: 0x0600C23D RID: 49725 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CheckIngredientsIfSociallyProper
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001DBB RID: 7611
		// (get) Token: 0x0600C23E RID: 49726 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool CanCopy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001DBC RID: 7612
		// (get) Token: 0x0600C23F RID: 49727 RVA: 0x00385284 File Offset: 0x00383484
		public override bool CompletableEver
		{
			get
			{
				if (this.recipe.targetsBodyPart && !this.recipe.Worker.GetPartsToApplyOn(this.GiverPawn, this.recipe).Contains(this.part))
				{
					return false;
				}
				Recipe_Surgery recipe_Surgery = this.recipe.Worker as Recipe_Surgery;
				if (recipe_Surgery != null && !recipe_Surgery.CompletableEver(this.GiverPawn))
				{
					return false;
				}
				if (!this.uniqueRequiredIngredients.NullOrEmpty<Thing>() && !this.iterationCompleted)
				{
					using (List<Thing>.Enumerator enumerator = this.uniqueRequiredIngredients.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.DestroyedOrNull())
							{
								return false;
							}
						}
					}
					return true;
				}
				return true;
			}
		}

		// Token: 0x17001DBD RID: 7613
		// (get) Token: 0x0600C240 RID: 49728 RVA: 0x00385350 File Offset: 0x00383550
		// (set) Token: 0x0600C241 RID: 49729 RVA: 0x00385358 File Offset: 0x00383558
		public BodyPartRecord Part
		{
			get
			{
				return this.part;
			}
			set
			{
				if (this.billStack == null && this.part != null)
				{
					Log.Error("Can only set Bill_Medical.Part after the bill has been added to a pawn's bill stack.");
					return;
				}
				this.part = value;
			}
		}

		// Token: 0x17001DBE RID: 7614
		// (get) Token: 0x0600C242 RID: 49730 RVA: 0x0038537C File Offset: 0x0038357C
		public Pawn GiverPawn
		{
			get
			{
				Pawn pawn = this.billStack.billGiver as Pawn;
				Corpse corpse = this.billStack.billGiver as Corpse;
				if (corpse != null)
				{
					pawn = corpse.InnerPawn;
				}
				if (pawn == null)
				{
					throw new InvalidOperationException("Medical bill on non-pawn.");
				}
				return pawn;
			}
		}

		// Token: 0x17001DBF RID: 7615
		// (get) Token: 0x0600C243 RID: 49731 RVA: 0x003853C4 File Offset: 0x003835C4
		public override string Label
		{
			get
			{
				string text = this.recipe.Worker.LabelFromUniqueIngredients(this);
				if (!text.NullOrEmpty())
				{
					return text;
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(this.recipe.Worker.GetLabelWhenUsedOn(this.GiverPawn, this.part));
				if (this.Part != null && !this.recipe.hideBodyPartNames)
				{
					stringBuilder.Append(" (" + this.Part.Label + ")");
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0600C244 RID: 49732 RVA: 0x00385452 File Offset: 0x00383652
		public Bill_Medical()
		{
		}

		// Token: 0x0600C245 RID: 49733 RVA: 0x00385465 File Offset: 0x00383665
		public Bill_Medical(RecipeDef recipe, List<Thing> uniqueIngredients) : base(recipe, null)
		{
			this.uniqueRequiredIngredients = uniqueIngredients;
		}

		// Token: 0x0600C246 RID: 49734 RVA: 0x00385481 File Offset: 0x00383681
		public override bool ShouldDoNow()
		{
			return !this.suspended;
		}

		// Token: 0x0600C247 RID: 49735 RVA: 0x00385490 File Offset: 0x00383690
		public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
			base.Notify_IterationCompleted(billDoer, ingredients);
			this.iterationCompleted = true;
			if (this.CompletableEver)
			{
				Pawn giverPawn = this.GiverPawn;
				this.recipe.Worker.ApplyOnPawn(giverPawn, this.Part, billDoer, ingredients, this);
				if (giverPawn.RaceProps.IsFlesh)
				{
					giverPawn.records.Increment(RecordDefOf.OperationsReceived);
					billDoer.records.Increment(RecordDefOf.OperationsPerformed);
					if (ModsConfig.AnomalyActive && Rand.Chance(0.5f) && MetalhorrorUtility.IsInfected(billDoer))
					{
						MetalhorrorUtility.Infect(giverPawn, billDoer, "SurgeryImplant", null);
					}
				}
			}
			this.billStack.Delete(this);
		}

		// Token: 0x0600C248 RID: 49736 RVA: 0x00385538 File Offset: 0x00383738
		public override void Notify_BillWorkStarted(Pawn billDoer)
		{
			base.Notify_BillWorkStarted(billDoer);
			this.consumedMedicine.Clear();
			if (!this.GiverPawn.Dead && this.recipe.anesthetize && HealthUtility.TryAnesthetize(this.GiverPawn))
			{
				List<ThingCountClass> placedThings = billDoer.CurJob.placedThings;
				if (placedThings != null)
				{
					int i = 0;
					while (i < placedThings.Count)
					{
						if (placedThings[i].thing is Medicine)
						{
							this.recipe.Worker.ConsumeIngredient(placedThings[i].thing.SplitOff(1), this.recipe, billDoer.MapHeld);
							ThingCountClass thingCountClass = placedThings[i];
							int count = thingCountClass.Count;
							thingCountClass.Count = count - 1;
							this.consumedMedicine.Add(placedThings[i].thing.def, 1);
							if (placedThings[i].thing.Destroyed || placedThings[i].Count <= 0)
							{
								placedThings.RemoveAt(i);
								return;
							}
							break;
						}
						else
						{
							i++;
						}
					}
				}
			}
		}

		// Token: 0x0600C249 RID: 49737 RVA: 0x00385654 File Offset: 0x00383854
		public override bool PawnAllowedToStartAnew(Pawn pawn)
		{
			if (!base.PawnAllowedToStartAnew(pawn))
			{
				return false;
			}
			if (this.recipe.Worker is Recipe_AdministerIngestible)
			{
				ThingDef singleDef = this.recipe.ingredients[0].filter.BestThingRequest.singleDef;
				if (singleDef.IsDrug && !new HistoryEvent(HistoryEventDefOf.AdministeredDrug, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
				{
					return false;
				}
				if (singleDef.IsDrug && singleDef.ingestible.drugCategory == DrugCategory.Hard && !new HistoryEvent(HistoryEventDefOf.AdministeredHardDrug, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
				{
					return false;
				}
				if (singleDef.IsNonMedicalDrug && !new HistoryEvent(HistoryEventDefOf.AdministeredRecreationalDrug, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600C24A RID: 49738 RVA: 0x00385724 File Offset: 0x00383924
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_BodyParts.Look(ref this.part, "part", null);
			Scribe_Collections.Look<ThingDef, int>(ref this.consumedMedicine, "consumedMedicine", LookMode.Undefined, LookMode.Undefined);
			Scribe_Collections.Look<Thing>(ref this.uniqueRequiredIngredients, "uniqueRequiredIngredients", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.iterationCompleted, "iterationCompleted", false, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				ThingDef thingDef = null;
				Scribe_Defs.Look<ThingDef>(ref thingDef, "consumedInitialMedicineDef");
				if (thingDef != null)
				{
					if (this.consumedMedicine == null)
					{
						this.consumedMedicine = new Dictionary<ThingDef, int>();
					}
					this.consumedMedicine.Add(thingDef, 1);
				}
			}
			else if (Scribe.mode == LoadSaveMode.PostLoadInit && this.consumedMedicine == null)
			{
				this.consumedMedicine = new Dictionary<ThingDef, int>();
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600C24B RID: 49739 RVA: 0x003857DE File Offset: 0x003839DE
		public override Bill Clone()
		{
			Bill_Medical bill_Medical = (Bill_Medical)base.Clone();
			bill_Medical.part = this.part;
			bill_Medical.consumedMedicine = new Dictionary<ThingDef, int>(this.consumedMedicine);
			return bill_Medical;
		}

		// Token: 0x040084A6 RID: 33958
		private BodyPartRecord part;

		// Token: 0x040084A7 RID: 33959
		public Dictionary<ThingDef, int> consumedMedicine = new Dictionary<ThingDef, int>();

		// Token: 0x040084A8 RID: 33960
		public List<Thing> uniqueRequiredIngredients;

		// Token: 0x040084A9 RID: 33961
		public int temp_partIndexToSetLater;

		// Token: 0x040084AA RID: 33962
		public bool iterationCompleted;

		// Token: 0x040084AB RID: 33963
		private const float MetalhorrorInfectionChance = 0.5f;
	}
}
