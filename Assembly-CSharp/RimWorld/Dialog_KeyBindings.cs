using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02003185 RID: 12677
	public class Dialog_KeyBindings : Window
	{
		// Token: 0x17002E55 RID: 11861
		// (get) Token: 0x06011D50 RID: 73040 RVA: 0x0052C17D File Offset: 0x0052A37D
		public override Vector2 InitialSize
		{
			get
			{
				return this.WindowSize;
			}
		}

		// Token: 0x06011D51 RID: 73041 RVA: 0x0052C188 File Offset: 0x0052A388
		public Dialog_KeyBindings() : base(null)
		{
			this.forcePause = true;
			this.onlyOneOfTypeAllowed = true;
			this.absorbInputAroundWindow = true;
			this.scrollPosition = new Vector2(0f, 0f);
			this.keyPrefsData = KeyPrefs.KeyPrefsData.Clone();
			this.contentHeight = 0f;
			KeyBindingCategoryDef keyBindingCategoryDef = null;
			foreach (KeyBindingDef keyBindingDef in DefDatabase<KeyBindingDef>.AllDefs)
			{
				if (keyBindingCategoryDef != keyBindingDef.category)
				{
					keyBindingCategoryDef = keyBindingDef.category;
					this.contentHeight += 44f;
				}
				this.contentHeight += 34f;
			}
		}

		// Token: 0x06011D52 RID: 73042 RVA: 0x0052C264 File Offset: 0x0052A464
		public override void DoWindowContents(Rect inRect)
		{
			Vector2 vector = new Vector2(120f, 40f);
			float y = vector.y;
			Rect rect = new Rect(inRect.x, inRect.y, inRect.width, inRect.height - (y + 10f)).ContractedBy(10f);
			Rect rect2 = new Rect(rect.x, rect.y + rect.height + 10f, rect.width, y);
			Widgets.BeginGroup(rect);
			Rect rect3 = new Rect(0f, 0f, rect.width, 40f);
			Text.Font = GameFont.Medium;
			GenUI.SetLabelAlign(TextAnchor.MiddleCenter);
			Widgets.Label(rect3, "KeyboardConfig".Translate());
			GenUI.ResetLabelAlign();
			Text.Font = GameFont.Small;
			Rect outRect = new Rect(0f, rect3.height, rect.width, rect.height - rect3.height);
			Rect rect4 = new Rect(0f, 0f, outRect.width - 16f, this.contentHeight);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect4, true);
			float num = 0f;
			KeyBindingCategoryDef keyBindingCategoryDef = null;
			Dialog_KeyBindings.keyBindingsWorkingList.Clear();
			Dialog_KeyBindings.keyBindingsWorkingList.AddRange(DefDatabase<KeyBindingDef>.AllDefs);
			Dialog_KeyBindings.keyBindingsWorkingList.SortBy((KeyBindingDef x) => x.category.index, (KeyBindingDef x) => x.index);
			for (int i = 0; i < Dialog_KeyBindings.keyBindingsWorkingList.Count; i++)
			{
				KeyBindingDef keyBindingDef = Dialog_KeyBindings.keyBindingsWorkingList[i];
				if (keyBindingCategoryDef != keyBindingDef.category)
				{
					bool skipDrawing = num - this.scrollPosition.y + 40f < 0f || num - this.scrollPosition.y > outRect.height;
					keyBindingCategoryDef = keyBindingDef.category;
					this.DrawCategoryEntry(keyBindingCategoryDef, rect4.width, ref num, skipDrawing);
				}
				bool skipDrawing2 = num - this.scrollPosition.y + 34f < 0f || num - this.scrollPosition.y > outRect.height;
				this.DrawKeyEntry(keyBindingDef, rect4, ref num, skipDrawing2);
			}
			Widgets.EndScrollView();
			Widgets.EndGroup();
			Widgets.BeginGroup(rect2);
			Rect rect5 = new Rect(0f, 0f, vector.x, vector.y);
			Rect rect6 = new Rect((rect2.width - vector.x) / 2f, 0f, vector.x, vector.y);
			Rect rect7 = new Rect(rect2.width - vector.x, 0f, vector.x, vector.y);
			if (Widgets.ButtonText(rect6, "ResetButton".Translate(), true, true, true, null))
			{
				this.keyPrefsData.ResetToDefaults();
				this.keyPrefsData.ErrorCheck();
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				Event.current.Use();
			}
			if (Widgets.ButtonText(rect5, "CancelButton".Translate(), true, true, true, null))
			{
				this.Close(true);
				Event.current.Use();
			}
			if (Widgets.ButtonText(rect7, "OK".Translate(), true, true, true, null))
			{
				KeyPrefs.KeyPrefsData = this.keyPrefsData;
				KeyPrefs.Save();
				this.Close(true);
				this.keyPrefsData.ErrorCheck();
				Event.current.Use();
			}
			Widgets.EndGroup();
		}

		// Token: 0x06011D53 RID: 73043 RVA: 0x0052C620 File Offset: 0x0052A820
		private void DrawCategoryEntry(KeyBindingCategoryDef category, float width, ref float curY, bool skipDrawing)
		{
			if (!skipDrawing)
			{
				Rect rect = new Rect(0f, curY, width, 40f).ContractedBy(4f);
				Text.Font = GameFont.Medium;
				Widgets.Label(rect, category.LabelCap);
				Text.Font = GameFont.Small;
				if (Mouse.IsOver(rect) && !category.description.NullOrEmpty())
				{
					TooltipHandler.TipRegion(rect, new TipSignal(category.description));
				}
			}
			curY += 40f;
			if (!skipDrawing)
			{
				Color color = GUI.color;
				GUI.color = new Color(0.3f, 0.3f, 0.3f);
				Widgets.DrawLineHorizontal(0f, curY, width);
				GUI.color = color;
			}
			curY += 4f;
		}

		// Token: 0x06011D54 RID: 73044 RVA: 0x0052C6D4 File Offset: 0x0052A8D4
		private void DrawKeyEntry(KeyBindingDef keyDef, Rect parentRect, ref float curY, bool skipDrawing)
		{
			if (!skipDrawing)
			{
				Rect rect = new Rect(parentRect.x, parentRect.y + curY, parentRect.width, 34f).ContractedBy(3f);
				GenUI.SetLabelAlign(TextAnchor.MiddleLeft);
				Widgets.Label(rect, keyDef.LabelCap);
				GenUI.ResetLabelAlign();
				float num = 4f;
				Vector2 vector = new Vector2(140f, 28f);
				Rect rect2 = new Rect(rect.x + rect.width - vector.x * 2f - num, rect.y, vector.x, vector.y);
				Rect rect3 = new Rect(rect.x + rect.width - vector.x, rect.y, vector.x, vector.y);
				string key = SteamDeck.IsSteamDeckInNonKeyboardMode ? "BindingButtonToolTipController" : "BindingButtonToolTip";
				TooltipHandler.TipRegionByKey(rect2, key);
				TooltipHandler.TipRegionByKey(rect3, key);
				if (Widgets.ButtonText(rect2, this.keyPrefsData.GetBoundKeyCode(keyDef, KeyPrefs.BindingSlot.A).ToStringReadable(), true, true, true, null))
				{
					this.SettingButtonClicked(keyDef, KeyPrefs.BindingSlot.A);
				}
				if (Widgets.ButtonText(rect3, this.keyPrefsData.GetBoundKeyCode(keyDef, KeyPrefs.BindingSlot.B).ToStringReadable(), true, true, true, null))
				{
					this.SettingButtonClicked(keyDef, KeyPrefs.BindingSlot.B);
				}
			}
			curY += 34f;
		}

		// Token: 0x06011D55 RID: 73045 RVA: 0x0052C838 File Offset: 0x0052AA38
		private void SettingButtonClicked(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot)
		{
			if (Event.current.button == 0)
			{
				Find.WindowStack.Add(new Dialog_DefineBinding(this.keyPrefsData, keyDef, slot));
				Event.current.Use();
				return;
			}
			if (Event.current.button == 1)
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				list.Add(new FloatMenuOption("ResetBinding".Translate(), delegate()
				{
					KeyCode keyCode = (slot == KeyPrefs.BindingSlot.A) ? keyDef.defaultKeyCodeA : keyDef.defaultKeyCodeB;
					this.keyPrefsData.SetBinding(keyDef, slot, keyCode);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				list.Add(new FloatMenuOption("ClearBinding".Translate(), delegate()
				{
					this.keyPrefsData.SetBinding(keyDef, slot, KeyCode.None);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x0400B86B RID: 47211
		protected Vector2 scrollPosition;

		// Token: 0x0400B86C RID: 47212
		protected float contentHeight;

		// Token: 0x0400B86D RID: 47213
		protected KeyPrefsData keyPrefsData;

		// Token: 0x0400B86E RID: 47214
		protected Vector2 WindowSize = new Vector2(700f, 760f);

		// Token: 0x0400B86F RID: 47215
		protected const float EntryHeight = 34f;

		// Token: 0x0400B870 RID: 47216
		protected const float CategoryHeadingHeight = 40f;

		// Token: 0x0400B871 RID: 47217
		private static List<KeyBindingDef> keyBindingsWorkingList = new List<KeyBindingDef>();
	}
}
