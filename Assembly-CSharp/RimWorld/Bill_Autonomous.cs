using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A66 RID: 10854
	public class Bill_Autonomous : Bill_Production
	{
		// Token: 0x170025AB RID: 9643
		// (get) Token: 0x0600F154 RID: 61780 RVA: 0x00460204 File Offset: 0x0045E404
		public FormingState State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x170025AC RID: 9644
		// (get) Token: 0x0600F155 RID: 61781 RVA: 0x0046020C File Offset: 0x0045E40C
		private Building_WorkTableAutonomous WorkTable
		{
			get
			{
				return (Building_WorkTableAutonomous)this.billStack.billGiver;
			}
		}

		// Token: 0x170025AD RID: 9645
		// (get) Token: 0x0600F156 RID: 61782 RVA: 0x0046021E File Offset: 0x0045E41E
		public override bool CanFinishNow
		{
			get
			{
				return this.state == FormingState.Formed;
			}
		}

		// Token: 0x170025AE RID: 9646
		// (get) Token: 0x0600F157 RID: 61783 RVA: 0x00460229 File Offset: 0x0045E429
		private List<IngredientCount> CurrentBillIngredients
		{
			get
			{
				if (this.ingredients.Count == 0)
				{
					this.MakeIngredientsListInProcessingOrder(this.ingredients);
				}
				return this.ingredients;
			}
		}

		// Token: 0x0600F158 RID: 61784 RVA: 0x0046024A File Offset: 0x0045E44A
		public Bill_Autonomous()
		{
		}

		// Token: 0x0600F159 RID: 61785 RVA: 0x0046025D File Offset: 0x0045E45D
		public Bill_Autonomous(RecipeDef recipe, Precept_ThingStyle precept = null) : base(recipe, precept)
		{
		}

		// Token: 0x0600F15A RID: 61786 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual Thing CreateProducts()
		{
			return null;
		}

		// Token: 0x0600F15B RID: 61787 RVA: 0x00460272 File Offset: 0x0045E472
		public override bool ShouldDoNow()
		{
			return base.ShouldDoNow() && this.state != FormingState.Forming;
		}

		// Token: 0x0600F15C RID: 61788 RVA: 0x0046028C File Offset: 0x0045E48C
		public override bool PawnAllowedToStartAnew(Pawn p)
		{
			return base.PawnAllowedToStartAnew(p) && (this.WorkTable.ActiveBill == null || this.WorkTable.ActiveBill == this || (this.WorkTable.ActiveBill.suspended && this.WorkTable.ActiveBill.State == FormingState.Gathering && !this.WorkTable.innerContainer.Any<Thing>()));
		}

		// Token: 0x0600F15D RID: 61789 RVA: 0x004602F8 File Offset: 0x0045E4F8
		public override void Notify_DoBillStarted(Pawn billDoer)
		{
			base.Notify_DoBillStarted(billDoer);
			this.WorkTable.ActiveBill = this;
			this.startedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600F15E RID: 61790 RVA: 0x00460320 File Offset: 0x0045E520
		public override void Notify_BillWorkFinished(Pawn billDoer)
		{
			base.Notify_BillWorkFinished(billDoer);
			switch (this.state)
			{
			case FormingState.Gathering:
				this.state = FormingState.Forming;
				this.formingTicks = (float)this.recipe.formingTicks;
				this.WorkTable.Notify_StartForming(billDoer);
				return;
			case FormingState.Preparing:
				this.formingTicks = (float)this.recipe.formingTicks;
				this.state = FormingState.Forming;
				break;
			case FormingState.Forming:
			case FormingState.Formed:
				break;
			default:
				return;
			}
		}

		// Token: 0x0600F15F RID: 61791 RVA: 0x00460391 File Offset: 0x0045E591
		public virtual void Reset()
		{
			this.ingredients.Clear();
			this.state = FormingState.Gathering;
		}

		// Token: 0x0600F160 RID: 61792 RVA: 0x004603A5 File Offset: 0x0045E5A5
		public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
			base.Notify_IterationCompleted(billDoer, ingredients);
			this.Reset();
			if (this.WorkTable.ActiveBill == this)
			{
				this.WorkTable.ActiveBill = null;
			}
		}

		// Token: 0x0600F161 RID: 61793 RVA: 0x004603CF File Offset: 0x0045E5CF
		public override float GetWorkAmount(Thing thing = null)
		{
			if (this.state == FormingState.Formed)
			{
				return 300f;
			}
			return base.GetWorkAmount(thing);
		}

		// Token: 0x0600F162 RID: 61794 RVA: 0x004603E8 File Offset: 0x0045E5E8
		public virtual void BillTick()
		{
			if (!this.suspended && this.state == FormingState.Forming)
			{
				this.formingTicks -= 1f;
				if (this.formingTicks <= 0f)
				{
					this.state = FormingState.Formed;
					this.WorkTable.Notify_FormingCompleted();
				}
			}
		}

		// Token: 0x0600F163 RID: 61795 RVA: 0x00460438 File Offset: 0x0045E638
		public void AppendCurrentIngredientCount(StringBuilder sb)
		{
			foreach (IngredientCount ingredientCount in this.CurrentBillIngredients)
			{
				if (ingredientCount != null && ingredientCount.IsFixedIngredient)
				{
					TaggedString taggedString = ingredientCount.FixedIngredient.LabelCap;
					int num = this.WorkTable.innerContainer.TotalStackCountOfDef(ingredientCount.FixedIngredient);
					taggedString += string.Format(" {0} / {1}", num, ingredientCount.CountRequiredOfFor(ingredientCount.FixedIngredient, this.recipe, this));
					sb.AppendLine(taggedString);
				}
			}
		}

		// Token: 0x0600F164 RID: 61796 RVA: 0x004604F0 File Offset: 0x0045E6F0
		public virtual void AppendInspectionData(StringBuilder sb)
		{
			if (this.State == FormingState.Forming || this.State == FormingState.Preparing)
			{
				sb.AppendTagged("FinishesIn".Translate() + ": " + ((int)this.formingTicks).ToStringTicksToPeriod(true, false, true, true, false));
			}
			if (this.State == FormingState.Formed)
			{
				sb.AppendLine("Finished".Translate());
			}
		}

		// Token: 0x0600F165 RID: 61797 RVA: 0x00460560 File Offset: 0x0045E760
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.formingTicks, "formingTicks", 0f, false);
			Scribe_Values.Look<FormingState>(ref this.state, "state", FormingState.Gathering, false);
			Scribe_Values.Look<int>(ref this.startedTick, "startedTick", 0, false);
		}

		// Token: 0x04009F32 RID: 40754
		public const int CompleteBillTicks = 300;

		// Token: 0x04009F33 RID: 40755
		public float formingTicks;

		// Token: 0x04009F34 RID: 40756
		protected int startedTick;

		// Token: 0x04009F35 RID: 40757
		protected FormingState state;

		// Token: 0x04009F36 RID: 40758
		private List<IngredientCount> ingredients = new List<IngredientCount>();
	}
}
