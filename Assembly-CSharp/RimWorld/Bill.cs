using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020010F4 RID: 4340
	public abstract class Bill : IExposable, ILoadReferenceable
	{
		// Token: 0x1700104A RID: 4170
		// (get) Token: 0x06006985 RID: 27013 RVA: 0x00213C08 File Offset: 0x00211E08
		public Map Map
		{
			get
			{
				BillStack billStack = this.billStack;
				Map map;
				if (billStack == null)
				{
					map = null;
				}
				else
				{
					IBillGiver billGiver = billStack.billGiver;
					map = ((billGiver != null) ? billGiver.Map : null);
				}
				Map result;
				if ((result = map) == null)
				{
					BillStack billStack2 = this.billStack;
					Thing thing = ((billStack2 != null) ? billStack2.billGiver : null) as Thing;
					if (thing == null)
					{
						return null;
					}
					result = thing.MapHeld;
				}
				return result;
			}
		}

		// Token: 0x1700104B RID: 4171
		// (get) Token: 0x06006986 RID: 27014 RVA: 0x00213C59 File Offset: 0x00211E59
		public virtual string Label
		{
			get
			{
				if (this.precept == null)
				{
					return this.recipe.label;
				}
				return "RecipeMake".Translate(this.precept.LabelCap).UncapitalizeFirst();
			}
		}

		// Token: 0x1700104C RID: 4172
		// (get) Token: 0x06006987 RID: 27015 RVA: 0x00213C93 File Offset: 0x00211E93
		public virtual string LabelCap
		{
			get
			{
				if (this.precept == null)
				{
					return this.Label.CapitalizeFirst(this.recipe);
				}
				return "RecipeMake".Translate(this.precept.LabelCap).CapitalizeFirst();
			}
		}

		// Token: 0x1700104D RID: 4173
		// (get) Token: 0x06006988 RID: 27016 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CheckIngredientsIfSociallyProper
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700104E RID: 4174
		// (get) Token: 0x06006989 RID: 27017 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CompletableEver
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700104F RID: 4175
		// (get) Token: 0x0600698A RID: 27018 RVA: 0x00002C42 File Offset: 0x00000E42
		protected virtual string StatusString
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001050 RID: 4176
		// (get) Token: 0x0600698B RID: 27019 RVA: 0x00006AAC File Offset: 0x00004CAC
		protected virtual float StatusLineMinHeight
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17001051 RID: 4177
		// (get) Token: 0x0600698C RID: 27020 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool CanCopy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001052 RID: 4178
		// (get) Token: 0x0600698D RID: 27021 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanFinishNow
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001053 RID: 4179
		// (get) Token: 0x0600698E RID: 27022 RVA: 0x00213CD4 File Offset: 0x00211ED4
		public bool DeletedOrDereferenced
		{
			get
			{
				if (this.deleted)
				{
					return true;
				}
				Thing thing = this.billStack.billGiver as Thing;
				return thing != null && thing.Destroyed;
			}
		}

		// Token: 0x17001054 RID: 4180
		// (get) Token: 0x0600698F RID: 27023 RVA: 0x00213D0A File Offset: 0x00211F0A
		public Pawn PawnRestriction
		{
			get
			{
				return this.pawnRestriction;
			}
		}

		// Token: 0x17001055 RID: 4181
		// (get) Token: 0x06006990 RID: 27024 RVA: 0x00213D12 File Offset: 0x00211F12
		public bool SlavesOnly
		{
			get
			{
				return this.slavesOnly;
			}
		}

		// Token: 0x17001056 RID: 4182
		// (get) Token: 0x06006991 RID: 27025 RVA: 0x00213D1A File Offset: 0x00211F1A
		public bool MechsOnly
		{
			get
			{
				return this.mechsOnly;
			}
		}

		// Token: 0x17001057 RID: 4183
		// (get) Token: 0x06006992 RID: 27026 RVA: 0x00213D22 File Offset: 0x00211F22
		public bool NonMechsOnly
		{
			get
			{
				return this.nonMechsOnly;
			}
		}

		// Token: 0x17001058 RID: 4184
		// (get) Token: 0x06006993 RID: 27027 RVA: 0x00213D2A File Offset: 0x00211F2A
		protected virtual Color BaseColor
		{
			get
			{
				if (this.ShouldDoNow())
				{
					return Color.white;
				}
				return new Color(1f, 0.7f, 0.7f, 0.7f);
			}
		}

		// Token: 0x06006994 RID: 27028 RVA: 0x00213D53 File Offset: 0x00211F53
		public Bill()
		{
		}

		// Token: 0x06006995 RID: 27029 RVA: 0x00213D84 File Offset: 0x00211F84
		public Bill(RecipeDef recipe, Precept_ThingStyle precept = null)
		{
			this.recipe = recipe;
			this.precept = precept;
			this.ingredientFilter = new ThingFilter();
			this.ingredientFilter.CopyAllowancesFrom(recipe.defaultIngredientFilter);
			this.InitializeAfterClone();
		}

		// Token: 0x06006996 RID: 27030 RVA: 0x00213DEE File Offset: 0x00211FEE
		public void InitializeAfterClone()
		{
			this.loadID = Find.UniqueIDsManager.GetNextBillID();
		}

		// Token: 0x06006997 RID: 27031 RVA: 0x00213E00 File Offset: 0x00212000
		public void SetPawnRestriction(Pawn pawn)
		{
			this.pawnRestriction = pawn;
			this.slavesOnly = false;
			this.mechsOnly = false;
			this.nonMechsOnly = false;
		}

		// Token: 0x06006998 RID: 27032 RVA: 0x00213E1E File Offset: 0x0021201E
		public void SetAnySlaveRestriction()
		{
			this.pawnRestriction = null;
			this.slavesOnly = true;
			this.mechsOnly = false;
			this.nonMechsOnly = false;
		}

		// Token: 0x06006999 RID: 27033 RVA: 0x00213E3C File Offset: 0x0021203C
		public void SetAnyPawnRestriction()
		{
			this.slavesOnly = false;
			this.pawnRestriction = null;
			this.mechsOnly = false;
			this.nonMechsOnly = false;
		}

		// Token: 0x0600699A RID: 27034 RVA: 0x00213E5A File Offset: 0x0021205A
		public void SetAnyMechRestriction()
		{
			this.slavesOnly = false;
			this.pawnRestriction = null;
			this.mechsOnly = true;
			this.nonMechsOnly = false;
		}

		// Token: 0x0600699B RID: 27035 RVA: 0x00213E78 File Offset: 0x00212078
		public void SetAnyNonMechRestriction()
		{
			this.slavesOnly = false;
			this.pawnRestriction = null;
			this.mechsOnly = false;
			this.nonMechsOnly = true;
		}

		// Token: 0x0600699C RID: 27036 RVA: 0x00213E98 File Offset: 0x00212098
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Defs.Look<RecipeDef>(ref this.recipe, "recipe");
			Scribe_Values.Look<bool>(ref this.suspended, "suspended", false, false);
			Scribe_Values.Look<float>(ref this.ingredientSearchRadius, "ingredientSearchRadius", 999f, false);
			Scribe_Values.Look<IntRange>(ref this.allowedSkillRange, "allowedSkillRange", default(IntRange), false);
			Scribe_References.Look<Pawn>(ref this.pawnRestriction, "pawnRestriction", false);
			Scribe_References.Look<Precept_ThingStyle>(ref this.precept, "precept", false);
			Scribe_References.Look<Xenogerm>(ref this.xenogerm, "xenogerm", false);
			Scribe_Values.Look<bool>(ref this.slavesOnly, "slavesOnly", false, false);
			Scribe_Values.Look<bool>(ref this.mechsOnly, "mechsOnly", false, false);
			Scribe_Values.Look<bool>(ref this.nonMechsOnly, "nonMechsOnly", false, false);
			Scribe_Defs.Look<ThingStyleDef>(ref this.style, "style");
			Scribe_Values.Look<bool>(ref this.globalStyle, "globalStyle", true, false);
			Scribe_Values.Look<int?>(ref this.graphicIndexOverride, "graphicIndexOverride", null, false);
			if (Scribe.mode == LoadSaveMode.Saving && this.recipe.fixedIngredientFilter != null)
			{
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
				{
					if (!this.recipe.fixedIngredientFilter.Allows(thingDef))
					{
						this.ingredientFilter.SetAllow(thingDef, false);
					}
				}
			}
			Scribe_Deep.Look<ThingFilter>(ref this.ingredientFilter, "ingredientFilter", Array.Empty<object>());
		}

		// Token: 0x0600699D RID: 27037 RVA: 0x00214030 File Offset: 0x00212230
		public virtual bool PawnAllowedToStartAnew(Pawn p)
		{
			if (this.pawnRestriction != null)
			{
				return this.pawnRestriction == p;
			}
			if (this.slavesOnly && !p.IsSlave)
			{
				return false;
			}
			if (this.mechsOnly && !p.IsColonyMechPlayerControlled)
			{
				return false;
			}
			if (this.nonMechsOnly && p.IsColonyMechPlayerControlled)
			{
				return false;
			}
			if (this.recipe.workSkill != null && (p.skills != null || p.IsColonyMech))
			{
				int num = (p.skills != null) ? p.skills.GetSkill(this.recipe.workSkill).Level : p.RaceProps.mechFixedSkillLevel;
				if (num < this.allowedSkillRange.min)
				{
					JobFailReason.Is("UnderAllowedSkill".Translate(this.allowedSkillRange.min), this.Label);
					return false;
				}
				if (num > this.allowedSkillRange.max)
				{
					JobFailReason.Is("AboveAllowedSkill".Translate(this.allowedSkillRange.max), this.Label);
					return false;
				}
			}
			if (ModsConfig.BiotechActive && this.recipe.mechanitorOnlyRecipe && !MechanitorUtility.IsMechanitor(p))
			{
				JobFailReason.Is("NotAMechanitor".Translate(), null);
				return false;
			}
			return true;
		}

		// Token: 0x0600699E RID: 27038 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PawnDidWork(Pawn p)
		{
		}

		// Token: 0x0600699F RID: 27039 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
		}

		// Token: 0x060069A0 RID: 27040
		public abstract bool ShouldDoNow();

		// Token: 0x060069A1 RID: 27041 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_DoBillStarted(Pawn billDoer)
		{
		}

		// Token: 0x060069A2 RID: 27042 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_BillWorkStarted(Pawn billDoer)
		{
		}

		// Token: 0x060069A3 RID: 27043 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_BillWorkFinished(Pawn billDoer)
		{
		}

		// Token: 0x060069A4 RID: 27044 RVA: 0x00214184 File Offset: 0x00212384
		protected virtual void DoConfigInterface(Rect rect, Color baseColor)
		{
			rect.yMin += 29f;
			float y = rect.center.y;
			Widgets.InfoCardButton(rect.xMax - (rect.yMax - y) - 12f, y - 12f, this.recipe);
		}

		// Token: 0x060069A5 RID: 27045 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void DoStatusLineInterface(Rect rect)
		{
		}

		// Token: 0x060069A6 RID: 27046 RVA: 0x002141DC File Offset: 0x002123DC
		public Rect DoInterface(float x, float y, float width, int index)
		{
			Rect rect = new Rect(x, y, width, 53f);
			float num = 0f;
			if (!this.StatusString.NullOrEmpty())
			{
				num = Mathf.Max(Text.TinyFontSupported ? 17f : 21f, this.StatusLineMinHeight);
			}
			rect.height += num;
			Color baseColor = this.BaseColor;
			GUI.color = baseColor;
			Text.Font = GameFont.Small;
			if (index % 2 == 0)
			{
				Widgets.DrawAltRect(rect);
			}
			Widgets.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 0f, 24f, 24f);
			if (this.billStack.IndexOf(this) > 0)
			{
				if (Widgets.ButtonImage(rect2, TexButton.ReorderUp, baseColor, true, null))
				{
					this.billStack.Reorder(this, -1);
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				TooltipHandler.TipRegionByKey(rect2, "ReorderBillUpTip");
			}
			if (this.billStack.IndexOf(this) < this.billStack.Count - 1)
			{
				Rect rect3 = new Rect(0f, 24f, 24f, 24f);
				if (Widgets.ButtonImage(rect3, TexButton.ReorderDown, baseColor, true, null))
				{
					this.billStack.Reorder(this, 1);
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
				TooltipHandler.TipRegionByKey(rect3, "ReorderBillDownTip");
			}
			GUI.color = baseColor;
			Widgets.Label(new Rect(28f, 0f, rect.width - 48f - 20f, rect.height + 5f), this.LabelCap);
			this.DoConfigInterface(rect.AtZero(), baseColor);
			Rect rect4 = new Rect(rect.width - 24f, 0f, 24f, 24f);
			if (Widgets.ButtonImage(rect4, TexButton.Delete, baseColor, baseColor * GenUI.SubtleMouseoverColor, true, null))
			{
				this.billStack.Delete(this);
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			TooltipHandler.TipRegionByKey(rect4, "DeleteBillTip");
			Rect rect6;
			if (this.CanCopy)
			{
				Rect rect5 = new Rect(rect4);
				rect5.x -= rect5.width + 4f;
				if (Widgets.ButtonImageFitted(rect5, TexButton.Copy, baseColor))
				{
					BillUtility.Clipboard = this.Clone();
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				TooltipHandler.TipRegionByKey(rect5, "CopyBillTip");
				rect6 = new Rect(rect5);
			}
			else
			{
				rect6 = new Rect(rect4);
			}
			rect6.x -= rect6.width;
			if (Widgets.ButtonImage(rect6, TexButton.Suspend, baseColor, true, null))
			{
				this.suspended = !this.suspended;
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			TooltipHandler.TipRegionByKey(rect6, "SuspendBillTip");
			if (!this.StatusString.NullOrEmpty())
			{
				Text.Font = GameFont.Tiny;
				Rect rect7 = new Rect(24f, rect.height - num, rect.width - 24f, num);
				Widgets.Label(rect7, this.StatusString);
				this.DoStatusLineInterface(rect7);
			}
			Widgets.EndGroup();
			if (this.suspended)
			{
				Text.Font = GameFont.Medium;
				Text.Anchor = TextAnchor.MiddleCenter;
				Rect rect8 = new Rect(rect.x + rect.width / 2f - 70f, rect.y + rect.height / 2f - 20f, 140f, 40f);
				GUI.DrawTexture(rect8, TexUI.GrayTextBG);
				Widgets.Label(rect8, "SuspendedCaps".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				Text.Font = GameFont.Small;
			}
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			return rect;
		}

		// Token: 0x060069A7 RID: 27047 RVA: 0x00214570 File Offset: 0x00212770
		public virtual bool IsFixedOrAllowedIngredient(Thing thing)
		{
			for (int i = 0; i < this.recipe.ingredients.Count; i++)
			{
				IngredientCount ingredientCount = this.recipe.ingredients[i];
				if (ingredientCount.IsFixedIngredient && ingredientCount.filter.Allows(thing))
				{
					return true;
				}
			}
			return this.recipe.fixedIngredientFilter.Allows(thing) && this.ingredientFilter.Allows(thing);
		}

		// Token: 0x060069A8 RID: 27048 RVA: 0x002145E4 File Offset: 0x002127E4
		public bool IsFixedOrAllowedIngredient(ThingDef def)
		{
			for (int i = 0; i < this.recipe.ingredients.Count; i++)
			{
				IngredientCount ingredientCount = this.recipe.ingredients[i];
				if (ingredientCount.IsFixedIngredient && ingredientCount.filter.Allows(def))
				{
					return true;
				}
			}
			return this.recipe.fixedIngredientFilter.Allows(def) && this.ingredientFilter.Allows(def);
		}

		// Token: 0x060069A9 RID: 27049 RVA: 0x00214658 File Offset: 0x00212858
		public static void CreateNoPawnsWithSkillDialog(RecipeDef recipe)
		{
			string text = "RecipeRequiresSkills".Translate(recipe.LabelCap);
			text += "\n\n";
			text += recipe.MinSkillString;
			Find.WindowStack.Add(new Dialog_MessageBox(text, null, null, null, null, null, false, null, null, WindowLayer.Dialog));
		}

		// Token: 0x060069AA RID: 27050 RVA: 0x002146B7 File Offset: 0x002128B7
		public virtual BillStoreModeDef GetStoreMode()
		{
			return BillStoreModeDefOf.BestStockpile;
		}

		// Token: 0x060069AB RID: 27051 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual ISlotGroup GetSlotGroup()
		{
			return null;
		}

		// Token: 0x060069AC RID: 27052 RVA: 0x002146BE File Offset: 0x002128BE
		public virtual void SetStoreMode(BillStoreModeDef mode, ISlotGroup group = null)
		{
			Log.ErrorOnce("Tried to set store mode of a non-production bill", 27190980);
		}

		// Token: 0x060069AD RID: 27053 RVA: 0x002146CF File Offset: 0x002128CF
		public virtual float GetWorkAmount(Thing thing = null)
		{
			return this.recipe.WorkAmountTotal(thing);
		}

		// Token: 0x060069AE RID: 27054 RVA: 0x002146E0 File Offset: 0x002128E0
		public virtual Bill Clone()
		{
			Bill bill = (Bill)Activator.CreateInstance(base.GetType());
			bill.recipe = this.recipe;
			bill.precept = this.precept;
			bill.style = this.style;
			bill.globalStyle = this.globalStyle;
			bill.suspended = this.suspended;
			bill.ingredientFilter = new ThingFilter();
			bill.ingredientFilter.CopyAllowancesFrom(this.ingredientFilter);
			bill.ingredientSearchRadius = this.ingredientSearchRadius;
			bill.allowedSkillRange = this.allowedSkillRange;
			bill.pawnRestriction = this.pawnRestriction;
			bill.slavesOnly = this.slavesOnly;
			bill.xenogerm = this.xenogerm;
			bill.mechsOnly = this.mechsOnly;
			bill.nonMechsOnly = this.nonMechsOnly;
			return bill;
		}

		// Token: 0x060069AF RID: 27055 RVA: 0x002147AC File Offset: 0x002129AC
		public virtual void ValidateSettings()
		{
			if (this.pawnRestriction != null && (this.pawnRestriction.Dead || this.pawnRestriction.Faction != Faction.OfPlayer || this.pawnRestriction.IsKidnapped()))
			{
				if (this != BillUtility.Clipboard)
				{
					Messages.Message("MessageBillValidationPawnUnavailable".Translate(this.pawnRestriction.LabelShortCap, this.Label, this.billStack.billGiver.LabelShort), this.billStack.billGiver as Thing, MessageTypeDefOf.NegativeEvent, true);
				}
				this.pawnRestriction = null;
			}
		}

		// Token: 0x060069B0 RID: 27056 RVA: 0x0021485E File Offset: 0x00212A5E
		public string GetUniqueLoadID()
		{
			return "Bill_" + this.recipe.defName + "_" + this.loadID.ToString();
		}

		// Token: 0x060069B1 RID: 27057 RVA: 0x00214885 File Offset: 0x00212A85
		public override string ToString()
		{
			return this.GetUniqueLoadID();
		}

		// Token: 0x04004AAB RID: 19115
		[Unsaved(false)]
		public BillStack billStack;

		// Token: 0x04004AAC RID: 19116
		private int loadID = -1;

		// Token: 0x04004AAD RID: 19117
		public RecipeDef recipe;

		// Token: 0x04004AAE RID: 19118
		public Precept_ThingStyle precept;

		// Token: 0x04004AAF RID: 19119
		public ThingStyleDef style;

		// Token: 0x04004AB0 RID: 19120
		public bool globalStyle = true;

		// Token: 0x04004AB1 RID: 19121
		public int? graphicIndexOverride;

		// Token: 0x04004AB2 RID: 19122
		public Xenogerm xenogerm;

		// Token: 0x04004AB3 RID: 19123
		public bool suspended;

		// Token: 0x04004AB4 RID: 19124
		public ThingFilter ingredientFilter;

		// Token: 0x04004AB5 RID: 19125
		public float ingredientSearchRadius = 999f;

		// Token: 0x04004AB6 RID: 19126
		public IntRange allowedSkillRange = new IntRange(0, 20);

		// Token: 0x04004AB7 RID: 19127
		private Pawn pawnRestriction;

		// Token: 0x04004AB8 RID: 19128
		private bool slavesOnly;

		// Token: 0x04004AB9 RID: 19129
		private bool mechsOnly;

		// Token: 0x04004ABA RID: 19130
		private bool nonMechsOnly;

		// Token: 0x04004ABB RID: 19131
		public bool deleted;

		// Token: 0x04004ABC RID: 19132
		public int nextTickToSearchForIngredients;

		// Token: 0x04004ABD RID: 19133
		public const int MaxIngredientSearchRadius = 999;

		// Token: 0x04004ABE RID: 19134
		public const float ButSize = 24f;

		// Token: 0x04004ABF RID: 19135
		private const float InterfaceBaseHeight = 53f;

		// Token: 0x04004AC0 RID: 19136
		private const float InterfaceStatusLineHeight = 17f;
	}
}
