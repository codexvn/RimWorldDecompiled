using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008EE RID: 2286
	public class UnfinishedThing : ThingWithComps
	{
		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x06003AEE RID: 15086 RVA: 0x00138557 File Offset: 0x00136757
		// (set) Token: 0x06003AEF RID: 15087 RVA: 0x0013855F File Offset: 0x0013675F
		public Pawn Creator
		{
			get
			{
				return this.creatorInt;
			}
			set
			{
				if (value == null)
				{
					Log.Error("Cannot set creator to null.");
					return;
				}
				this.creatorInt = value;
				this.creatorName = value.LabelShort;
			}
		}

		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x06003AF0 RID: 15088 RVA: 0x00138582 File Offset: 0x00136782
		public RecipeDef Recipe
		{
			get
			{
				return this.recipeInt;
			}
		}

		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x06003AF1 RID: 15089 RVA: 0x0013858A File Offset: 0x0013678A
		// (set) Token: 0x06003AF2 RID: 15090 RVA: 0x001385BC File Offset: 0x001367BC
		public Bill_ProductionWithUft BoundBill
		{
			get
			{
				if (this.boundBillInt != null && (this.boundBillInt.DeletedOrDereferenced || this.boundBillInt.BoundUft != this))
				{
					this.boundBillInt = null;
				}
				return this.boundBillInt;
			}
			set
			{
				if (value == this.boundBillInt)
				{
					return;
				}
				Bill_ProductionWithUft bill_ProductionWithUft = this.boundBillInt;
				this.boundBillInt = value;
				if (bill_ProductionWithUft != null && bill_ProductionWithUft.BoundUft == this)
				{
					bill_ProductionWithUft.SetBoundUft(null, false);
				}
				if (value != null)
				{
					this.recipeInt = value.recipe;
					if (value.BoundUft != this)
					{
						value.SetBoundUft(this, false);
					}
				}
			}
		}

		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x06003AF3 RID: 15091 RVA: 0x00138618 File Offset: 0x00136818
		public Thing BoundWorkTable
		{
			get
			{
				if (this.BoundBill == null)
				{
					return null;
				}
				Thing thing = this.BoundBill.billStack.billGiver as Thing;
				if (thing.Destroyed)
				{
					return null;
				}
				return thing;
			}
		}

		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x06003AF4 RID: 15092 RVA: 0x00138650 File Offset: 0x00136850
		public override string LabelNoCount
		{
			get
			{
				if (this.Recipe == null)
				{
					return base.LabelNoCount;
				}
				if (base.Stuff == null)
				{
					return "UnfinishedItem".Translate(this.Recipe.products[0].thingDef.label);
				}
				return "UnfinishedItemWithStuff".Translate(base.Stuff.LabelAsStuff, this.Recipe.products[0].thingDef.label);
			}
		}

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x06003AF5 RID: 15093 RVA: 0x001386E3 File Offset: 0x001368E3
		public override string DescriptionDetailed
		{
			get
			{
				if (this.Recipe == null)
				{
					return base.LabelNoCount;
				}
				return this.Recipe.ProducedThingDef.DescriptionDetailed;
			}
		}

		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x06003AF6 RID: 15094 RVA: 0x00138704 File Offset: 0x00136904
		public override string DescriptionFlavor
		{
			get
			{
				if (this.Recipe == null)
				{
					return base.LabelNoCount;
				}
				return this.Recipe.ProducedThingDef.description;
			}
		}

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x06003AF7 RID: 15095 RVA: 0x00138725 File Offset: 0x00136925
		public bool Initialized
		{
			get
			{
				return this.workLeft > -5000f;
			}
		}

		// Token: 0x06003AF8 RID: 15096 RVA: 0x00138734 File Offset: 0x00136934
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving && this.boundBillInt != null && this.boundBillInt.DeletedOrDereferenced)
			{
				this.boundBillInt = null;
			}
			Scribe_References.Look<Pawn>(ref this.creatorInt, "creator", false);
			Scribe_Values.Look<string>(ref this.creatorName, "creatorName", null, false);
			Scribe_References.Look<Bill_ProductionWithUft>(ref this.boundBillInt, "bill", false);
			Scribe_Defs.Look<RecipeDef>(ref this.recipeInt, "recipe");
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
			Scribe_Collections.Look<Thing>(ref this.ingredients, "ingredients", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x06003AF9 RID: 15097 RVA: 0x001387DC File Offset: 0x001369DC
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode == DestroyMode.Cancel)
			{
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					int num = GenMath.RoundRandom((float)this.ingredients[i].stackCount * 0.75f);
					if (num > 0)
					{
						this.ingredients[i].stackCount = num;
						GenPlace.TryPlaceThing(this.ingredients[i], base.Position, base.Map, ThingPlaceMode.Near, null, null, null, 1);
					}
				}
				this.ingredients.Clear();
			}
			base.Destroy(mode);
			this.BoundBill = null;
		}

		// Token: 0x06003AFA RID: 15098 RVA: 0x0013887E File Offset: 0x00136A7E
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield return new Command_Action
			{
				defaultLabel = "CommandCancelConstructionLabel".Translate(),
				defaultDesc = "CommandCancelConstructionDesc".Translate(),
				icon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true),
				hotKey = KeyBindingDefOf.Designator_Cancel,
				action = delegate()
				{
					this.Destroy(DestroyMode.Cancel);
				}
			};
			if (this.Initialized && DebugSettings.ShowDevGizmos && !this.debugCompleted)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Complete",
					action = delegate()
					{
						this.debugCompleted = true;
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x06003AFB RID: 15099 RVA: 0x00138890 File Offset: 0x00136A90
		public Bill_ProductionWithUft BillOnTableForMe(Thing workTable)
		{
			if (this.Recipe.AllRecipeUsers.Contains(workTable.def))
			{
				IBillGiver billGiver = (IBillGiver)workTable;
				for (int i = 0; i < billGiver.BillStack.Count; i++)
				{
					Bill_ProductionWithUft bill_ProductionWithUft = billGiver.BillStack[i] as Bill_ProductionWithUft;
					if (bill_ProductionWithUft != null && bill_ProductionWithUft.ShouldDoNow() && bill_ProductionWithUft != null && bill_ProductionWithUft.recipe == this.Recipe)
					{
						return bill_ProductionWithUft;
					}
				}
			}
			return null;
		}

		// Token: 0x06003AFC RID: 15100 RVA: 0x00138903 File Offset: 0x00136B03
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.BoundWorkTable != null)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), this.BoundWorkTable.TrueCenter());
			}
		}

		// Token: 0x06003AFD RID: 15101 RVA: 0x0013892C File Offset: 0x00136B2C
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			text += "Author".Translate() + ": " + this.creatorName;
			text += "\n" + "WorkLeft".Translate() + ": " + this.workLeft.ToStringWorkAmount();
			Bill_ProductionWithUft boundBill = this.BoundBill;
			bool flag;
			if (boundBill == null)
			{
				flag = (null != null);
			}
			else
			{
				ThingStyleDef style = boundBill.style;
				flag = (((style != null) ? style.Category : null) != null);
			}
			if (flag && base.StyleSourcePrecept == null)
			{
				text += "\n" + "Style".Translate() + ": " + this.BoundBill.style.Category.LabelCap;
			}
			return text;
		}

		// Token: 0x06003AFE RID: 15102 RVA: 0x00138A26 File Offset: 0x00136C26
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats())
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			Bill_ProductionWithUft boundBill = this.BoundBill;
			bool flag;
			if (boundBill == null)
			{
				flag = (null != null);
			}
			else
			{
				ThingStyleDef style = boundBill.style;
				flag = (((style != null) ? style.Category : null) != null);
			}
			if (flag && base.StyleSourcePrecept == null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "Stat_Thing_StyleLabel".Translate(), this.BoundBill.style.Category.LabelCap, "Stat_Thing_StyleDesc".Translate(), 1108, null, null, false, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x04002AC2 RID: 10946
		private Pawn creatorInt;

		// Token: 0x04002AC3 RID: 10947
		private string creatorName = "ErrorCreatorName";

		// Token: 0x04002AC4 RID: 10948
		private RecipeDef recipeInt;

		// Token: 0x04002AC5 RID: 10949
		public List<Thing> ingredients = new List<Thing>();

		// Token: 0x04002AC6 RID: 10950
		private Bill_ProductionWithUft boundBillInt;

		// Token: 0x04002AC7 RID: 10951
		public float workLeft = -10000f;

		// Token: 0x04002AC8 RID: 10952
		public bool debugCompleted;

		// Token: 0x04002AC9 RID: 10953
		private const float CancelIngredientRecoveryFraction = 0.75f;
	}
}
