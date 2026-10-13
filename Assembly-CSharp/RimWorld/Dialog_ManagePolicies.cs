using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020031EC RID: 12780
	public abstract class Dialog_ManagePolicies<T> : Window where T : Policy
	{
		// Token: 0x17002EAC RID: 11948
		// (get) Token: 0x06011F95 RID: 73621 RVA: 0x0053B987 File Offset: 0x00539B87
		// (set) Token: 0x06011F96 RID: 73622 RVA: 0x0053B98F File Offset: 0x00539B8F
		protected T SelectedPolicy
		{
			get
			{
				return this.policyInt;
			}
			set
			{
				this.ValidateName();
				this.policyInt = value;
			}
		}

		// Token: 0x17002EAD RID: 11949
		// (get) Token: 0x06011F97 RID: 73623
		protected abstract string TitleKey { get; }

		// Token: 0x17002EAE RID: 11950
		// (get) Token: 0x06011F98 RID: 73624
		protected abstract string TipKey { get; }

		// Token: 0x17002EAF RID: 11951
		// (get) Token: 0x06011F99 RID: 73625 RVA: 0x0053AD44 File Offset: 0x00538F44
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(700f, 700f);
			}
		}

		// Token: 0x17002EB0 RID: 11952
		// (get) Token: 0x06011F9A RID: 73626 RVA: 0x00006AAC File Offset: 0x00004CAC
		protected virtual float OffsetHeaderY
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06011F9B RID: 73627 RVA: 0x0053B9A0 File Offset: 0x00539BA0
		private void ValidateName()
		{
			if (this.SelectedPolicy != null && this.SelectedPolicy.label.NullOrEmpty())
			{
				this.SelectedPolicy.label = "UnnamedPolicy".Translate();
			}
		}

		// Token: 0x06011F9C RID: 73628 RVA: 0x0053B9F0 File Offset: 0x00539BF0
		protected Dialog_ManagePolicies(T policy) : base(null)
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
			this.scroll = Vector2.zero;
			this.quickSearch.Reset();
			this.SelectedPolicy = policy;
		}

		// Token: 0x06011F9D RID: 73629 RVA: 0x0053BA5A File Offset: 0x00539C5A
		public override void PreOpen()
		{
			base.PreOpen();
			this.thingFilterState.quickSearch.Reset();
		}

		// Token: 0x06011F9E RID: 73630 RVA: 0x0053BA74 File Offset: 0x00539C74
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = inRect;
			rect.height = 32f;
			Rect rect2 = rect;
			rect2.y = rect.yMax;
			Rect rect3 = rect2;
			rect3.y = rect2.yMax + 10f;
			rect3.height = 32f;
			rect3.width = 200f;
			Rect rect4 = rect3;
			rect4.x = rect3.xMax + 10f;
			rect4.xMax = inRect.xMax;
			Rect rect5 = rect4;
			rect5.xMax -= 100f;
			Rect rect6 = rect4;
			rect6.x = rect4.xMax - rect4.height;
			rect6.width = rect4.height;
			Rect rect7 = rect6;
			rect7.x = rect6.x - rect4.height - 10f;
			Rect rect8 = rect7;
			rect8.x = rect7.x - rect4.height - 10f;
			Rect rect9 = rect8;
			rect9.x = rect8.x - rect4.height - 10f;
			Rect leftRect = rect3;
			leftRect.y = rect3.yMax + this.OffsetHeaderY + 4f;
			leftRect.yMax = inRect.yMax - Window.CloseButSize.y - 10f;
			Rect rect10 = rect4;
			rect10.y = leftRect.y;
			rect10.yMax = leftRect.yMax;
			Rect rect11 = rect4;
			rect11.yMax = rect10.yMax;
			using (new TextBlock(GameFont.Medium))
			{
				Widgets.Label(rect, this.TitleKey.Translate());
			}
			Text.Font = GameFont.Small;
			Widgets.Label(rect2, this.TipKey.Translate());
			using (new TextBlock(GameFont.Medium))
			{
				Widgets.Label(rect3, "AvailablePolicies".Translate());
			}
			this.DoPolicyListing(leftRect);
			if (this.SelectedPolicy != null)
			{
				this.DoContentsRect(rect10);
				bool flag = this.GetDefaultPolicy() == this.SelectedPolicy;
				string text = this.SelectedPolicy.label;
				if (flag)
				{
					text += string.Format(" ({0})", "default".Translate()).Colorize(Color.gray);
				}
				using (new TextBlock(GameFont.Medium))
				{
					Widgets.LabelEllipses(rect5, text);
				}
				TooltipHandler.TipRegionByKey(rect6, "DeletePolicyTip");
				TooltipHandler.TipRegionByKey(rect7, "DuplicatePolicyTip");
				TooltipHandler.TipRegionByKey(rect8, "RenamePolicyTip");
				if (!flag)
				{
					TooltipHandler.TipRegionByKey(rect9, "DefaultPolicyTip");
				}
				if (Widgets.ButtonImage(rect6, TexUI.DismissTex, true, null))
				{
					if (Input.GetKey(KeyCode.LeftControl))
					{
						this.DeletePolicy();
					}
					else
					{
						TaggedString taggedString = "DeletePolicyConfirm".Translate(this.SelectedPolicy.label);
						TaggedString taggedString2 = "DeletePolicyConfirmButton".Translate();
						Find.WindowStack.Add(new Dialog_Confirm(taggedString, taggedString2, new Action(this.DeletePolicy)));
					}
				}
				if (Widgets.ButtonImage(rect7, TexUI.CopyTex, true, null))
				{
					T t = this.CreateNewPolicy();
					t.CopyFrom(this.SelectedPolicy);
					this.SelectedPolicy = t;
				}
				if (Widgets.ButtonImage(rect8, TexUI.RenameTex, true, null))
				{
					Find.WindowStack.Add(new Dialog_RenamePolicy(this.SelectedPolicy));
				}
				if (!flag && Widgets.ButtonImage(rect9, TexUI.MakeDefault, true, null))
				{
					this.SetDefaultPolicy(this.SelectedPolicy);
					return;
				}
			}
			else
			{
				using (new TextBlock(new GameFont?(GameFont.Medium), new TextAnchor?(TextAnchor.MiddleCenter), new Color?(Color.gray)))
				{
					Widgets.Label(rect11, "NoPolicySelected".Translate());
				}
			}
		}

		// Token: 0x06011F9F RID: 73631 RVA: 0x0053BEA4 File Offset: 0x0053A0A4
		private void DeletePolicy()
		{
			AcceptanceReport acceptanceReport = this.TryDeletePolicy(this.SelectedPolicy);
			if (!acceptanceReport.Accepted)
			{
				Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
				return;
			}
			this.SelectedPolicy = default(T);
		}

		// Token: 0x06011FA0 RID: 73632 RVA: 0x0053BEEC File Offset: 0x0053A0EC
		private void DoPolicyListing(Rect leftRect)
		{
			Rect rect = leftRect;
			rect.y = leftRect.yMax - 24f;
			rect.height = 24f;
			Rect rect2 = leftRect;
			rect2.yMax = rect.y - 10f;
			Rect rect3 = rect2;
			rect3.xMin += 10f;
			rect3.xMax -= 10f;
			rect3.y = rect2.yMax - Window.CloseButSize.y - 10f;
			rect3.height = Window.CloseButSize.y;
			Rect outRect = rect2;
			outRect.yMax = rect3.y - 10f;
			this.quickSearch.OnGUI(rect, null, null);
			Widgets.DrawMenuSection(rect2);
			if (Widgets.ButtonText(rect3, "NewPolicy".Translate(), true, true, true, null))
			{
				this.SelectedPolicy = this.CreateNewPolicy();
			}
			int num = 0;
			foreach (T t in this.GetPolicies())
			{
				if (this.quickSearch.filter.Matches(t.label))
				{
					num++;
				}
			}
			Rect viewRect = new Rect(0f, 0f, outRect.width, (float)num * 32f);
			Widgets.AdjustRectsForScrollView(rect2, ref outRect, ref viewRect);
			Widgets.BeginScrollView(outRect, ref this.scroll, viewRect, true);
			float num2 = 0f;
			int num3 = 0;
			T defaultPolicy = this.GetDefaultPolicy();
			IEnumerable<T> policies = this.GetPolicies();
			Func<T, bool> <>9__0;
			Func<T, bool> keySelector;
			if ((keySelector = <>9__0) == null)
			{
				keySelector = (<>9__0 = ((T x) => defaultPolicy != x));
			}
			foreach (T t2 in policies.OrderBy(keySelector).ThenBy((T x) => x.label))
			{
				if (this.quickSearch.filter.Matches(t2.label))
				{
					Rect rect4 = new Rect(0f, num2, outRect.width, 32f);
					Rect rect5 = rect4;
					rect5.x += 10f;
					num2 += 32f;
					if (this.SelectedPolicy == t2)
					{
						Widgets.DrawHighlightSelected(rect4);
					}
					else if (Mouse.IsOver(rect4))
					{
						Widgets.DrawHighlight(rect4);
					}
					else if (num3 % 2 == 1)
					{
						Widgets.DrawLightHighlight(rect4);
					}
					num3++;
					string text = t2.label;
					if (defaultPolicy == t2)
					{
						text += "*".Colorize(Color.gray);
					}
					using (new TextBlock(TextAnchor.MiddleLeft))
					{
						Widgets.Label(rect5, text);
					}
					if (Widgets.ButtonInvisible(rect4, true))
					{
						this.SelectedPolicy = t2;
					}
				}
			}
			Widgets.EndScrollView();
		}

		// Token: 0x06011FA1 RID: 73633
		protected abstract T CreateNewPolicy();

		// Token: 0x06011FA2 RID: 73634
		protected abstract T GetDefaultPolicy();

		// Token: 0x06011FA3 RID: 73635
		protected abstract void SetDefaultPolicy(T policy);

		// Token: 0x06011FA4 RID: 73636
		protected abstract AcceptanceReport TryDeletePolicy(T policy);

		// Token: 0x06011FA5 RID: 73637
		protected abstract List<T> GetPolicies();

		// Token: 0x06011FA6 RID: 73638
		protected abstract void DoContentsRect(Rect rect);

		// Token: 0x06011FA7 RID: 73639 RVA: 0x0053C270 File Offset: 0x0053A470
		public override void PreClose()
		{
			base.PreClose();
			this.ValidateName();
		}

		// Token: 0x0400BA27 RID: 47655
		private readonly ThingFilterUI.UIState thingFilterState = new ThingFilterUI.UIState();

		// Token: 0x0400BA28 RID: 47656
		private readonly QuickSearchWidget quickSearch = new QuickSearchWidget();

		// Token: 0x0400BA29 RID: 47657
		private Vector2 scroll;

		// Token: 0x0400BA2A RID: 47658
		private T policyInt;

		// Token: 0x0400BA2B RID: 47659
		private const float LeftPanelWidth = 200f;

		// Token: 0x0400BA2C RID: 47660
		private const float PolicyHeight = 32f;
	}
}
