using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02003195 RID: 12693
	public class Dialog_ModMismatch : Window
	{
		// Token: 0x17002E6D RID: 11885
		// (get) Token: 0x06011DBC RID: 73148 RVA: 0x0052F88E File Offset: 0x0052DA8E
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(900f, 750f);
			}
		}

		// Token: 0x06011DBD RID: 73149 RVA: 0x0052F8A0 File Offset: 0x0052DAA0
		public Dialog_ModMismatch(Action loadAction, List<string> loadedModIdsList, List<string> loadedModNamesList) : base(null)
		{
			this.loadAction = loadAction;
			this.loadedModIdsList = (from id in loadedModIdsList
			select id.ToLower()).ToList<string>();
			this.loadedModNamesList = loadedModNamesList;
		}

		// Token: 0x06011DBE RID: 73150 RVA: 0x0052F914 File Offset: 0x0052DB14
		public override void PreOpen()
		{
			base.PreOpen();
			List<string> source = (from mod in LoadedModManager.RunningMods
			select mod.PackageId).ToList<string>();
			this.runningModIdsList = (from id in source
			select ModLister.GetModWithIdentifier(id, false).PackageId.ToLower()).ToList<string>();
			this.runningModNamesList = (from id in source
			select ModLister.GetModWithIdentifier(id, false).Name).ToList<string>();
			this.addedModsList = (from modId in this.runningModIdsList
			where !this.loadedModIdsList.Contains(modId)
			select this.runningModNamesList[this.runningModIdsList.IndexOf(modId)]).ToList<string>();
			this.missingModsList = (from modId in this.loadedModIdsList
			where !this.runningModIdsList.Contains(modId)
			select this.loadedModNamesList[this.loadedModIdsList.IndexOf(modId)]).ToList<string>();
			this.sharedModsList = (from modId in this.runningModIdsList
			where this.loadedModIdsList.Contains(modId)
			select this.runningModNamesList[this.runningModIdsList.IndexOf(modId)]).ToList<string>();
		}

		// Token: 0x06011DBF RID: 73151 RVA: 0x0052FA50 File Offset: 0x0052DC50
		public override void DoWindowContents(Rect inRect)
		{
			float num = (inRect.width - 20f) / 3f;
			float num2 = 0f;
			float x = 0f;
			float x2 = num + 10f;
			float x3 = (num + 10f) * 2f;
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(0f, num2, inRect.width, Text.LineHeight), "ModsMismatchWarningTitle".Translate());
			num2 += Text.LineHeight + 10f;
			Text.Font = GameFont.Small;
			float height = Text.CalcHeight("ModsMismatchWarningText".Translate(), inRect.width);
			Rect rect = new Rect(0f, num2, inRect.width, height);
			Widgets.Label(rect, "ModsMismatchWarningText".Translate());
			num2 += rect.height + 17f;
			if (!this.addedModsList.Any<string>() && !this.missingModsList.Any<string>())
			{
				float height2 = Text.CalcHeight("ModsMismatchOrderChanged".Translate(), inRect.width);
				Widgets.Label(new Rect(0f, num2, inRect.width, height2), "ModsMismatchOrderChanged".Translate());
			}
			else
			{
				float num3 = num2;
				Widgets.Label(new Rect(x, num2, num, Text.LineHeight), "AddedModsList".Translate());
				num2 += Text.LineHeight + 10f;
				float height3 = inRect.height - num2 - Dialog_ModMismatch.ButtonHeight - 10f;
				this.DoModList(new Rect(x, num2, num, height3), this.addedModsList, ref this.addedModListScrollPosition, new Color?(new Color(0.27f, 0.4f, 0.1f)));
				num2 = num3;
				Widgets.Label(new Rect(x2, num2, num, Text.LineHeight), "MissingModsList".Translate());
				num2 += Text.LineHeight + 10f;
				this.DoModList(new Rect(x2, num2, num, height3), this.missingModsList, ref this.missingModListScrollPosition, new Color?(new Color(0.38f, 0.07f, 0.09f)));
				num2 = num3;
				Widgets.Label(new Rect(x3, num2, num, Text.LineHeight), "SharedModsList".Translate());
				num2 += Text.LineHeight + 10f;
				this.DoModList(new Rect(x3, num2, num, height3), this.sharedModsList, ref this.sharedModListScrollPosition, null);
			}
			float y = inRect.height - Dialog_ModMismatch.ButtonHeight;
			Rect rect2 = new Rect(0f, y, Dialog_ModMismatch.ButtonWidth, Dialog_ModMismatch.ButtonHeight);
			Rect rect3 = new Rect(inRect.width / 2f - Dialog_ModMismatch.ButtonWidth - 4f, y, Dialog_ModMismatch.ButtonWidth, Dialog_ModMismatch.ButtonHeight);
			Rect rect4 = new Rect(inRect.width / 2f + 4f, y, Dialog_ModMismatch.ButtonWidth, Dialog_ModMismatch.ButtonHeight);
			Rect rect5 = new Rect(inRect.width - Dialog_ModMismatch.ButtonWidth, y, Dialog_ModMismatch.ButtonWidth, Dialog_ModMismatch.ButtonHeight);
			if (Widgets.ButtonText(rect2, "GoBack".Translate(), true, true, true, null))
			{
				this.HandleGoBackClicked();
			}
			if (Widgets.ButtonText(rect3, "SaveModList".Translate(), true, true, true, null))
			{
				this.HandleSaveCurrentModList();
			}
			if (Widgets.ButtonText(rect4, "ChangeLoadedMods".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmLoadSaveList".Translate(), delegate()
				{
					this.HandleChangeLoadedModClicked();
				}, true, null, WindowLayer.Dialog));
			}
			if (Widgets.ButtonText(rect5, "LoadAnyway".Translate(), true, true, true, null))
			{
				this.HandleLoadAnywayClicked();
			}
		}

		// Token: 0x06011DC0 RID: 73152 RVA: 0x0052FE18 File Offset: 0x0052E018
		private void DoModList(Rect r, List<string> modList, ref Vector2 scrollPos, Color? rowColor = null)
		{
			int num = 0;
			Rect viewRect = new Rect(0f, 0f, r.width - 16f, (float)modList.Count * Dialog_ModMismatch.ModRowHeight);
			Widgets.BeginScrollView(r, ref scrollPos, viewRect, true);
			foreach (string modName in modList)
			{
				Rect rect = new Rect(0f, (float)num * Dialog_ModMismatch.ModRowHeight, r.width, Dialog_ModMismatch.ModRowHeight);
				if (rowColor != null)
				{
					Widgets.DrawBoxSolid(rect, rowColor.Value);
				}
				this.DoModRow(rect, modName, num);
				num++;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x06011DC1 RID: 73153 RVA: 0x0052FEE0 File Offset: 0x0052E0E0
		private void DoModRow(Rect r, string modName, int index)
		{
			if (index % 2 == 0)
			{
				Widgets.DrawLightHighlight(r);
			}
			r.xMin += 4f;
			r.xMax -= 4f;
			Widgets.Label(r, modName);
		}

		// Token: 0x06011DC2 RID: 73154 RVA: 0x0052FF1A File Offset: 0x0052E11A
		private void HandleGoBackClicked()
		{
			SoundDefOf.Click.PlayOneShotOnCamera(null);
			this.Close(true);
		}

		// Token: 0x06011DC3 RID: 73155 RVA: 0x0052FF30 File Offset: 0x0052E130
		private void HandleChangeLoadedModClicked()
		{
			SoundDefOf.Click.PlayOneShotOnCamera(null);
			if (Current.ProgramState == ProgramState.Entry)
			{
				ModsConfig.SetActiveToList(this.loadedModIdsList);
			}
			ModsConfig.SaveFromList(this.loadedModIdsList);
			IEnumerable<string> enumerable = from id in Enumerable.Range(0, this.loadedModIdsList.Count)
			where ModLister.GetModWithIdentifier(this.loadedModIdsList[id], false) == null
			select this.loadedModNamesList[id];
			if (enumerable.Any<string>())
			{
				Messages.Message(string.Format("{0}: {1}", "MissingMods".Translate(), enumerable.ToCommaList(false, false)), MessageTypeDefOf.RejectInput, false);
			}
			ModsConfig.RestartFromChangedMods();
		}

		// Token: 0x06011DC4 RID: 73156 RVA: 0x0052FFD4 File Offset: 0x0052E1D4
		private void HandleSaveCurrentModList()
		{
			SoundDefOf.Click.PlayOneShotOnCamera(null);
			ModList modList = new ModList();
			modList.ids = this.runningModIdsList;
			modList.names = this.runningModNamesList;
			Find.WindowStack.Add(new Dialog_ModList_Save(modList, null));
		}

		// Token: 0x06011DC5 RID: 73157 RVA: 0x0053001B File Offset: 0x0052E21B
		private void HandleLoadAnywayClicked()
		{
			SoundDefOf.Click.PlayOneShotOnCamera(null);
			this.loadAction();
			this.Close(true);
		}

		// Token: 0x06011DC6 RID: 73158 RVA: 0x0053003A File Offset: 0x0052E23A
		public override void OnAcceptKeyPressed()
		{
			this.HandleLoadAnywayClicked();
		}

		// Token: 0x0400B8B5 RID: 47285
		private Action loadAction;

		// Token: 0x0400B8B6 RID: 47286
		private List<string> loadedModIdsList;

		// Token: 0x0400B8B7 RID: 47287
		public List<string> loadedModNamesList;

		// Token: 0x0400B8B8 RID: 47288
		private Vector2 addedModListScrollPosition = Vector2.zero;

		// Token: 0x0400B8B9 RID: 47289
		private Vector2 missingModListScrollPosition = Vector2.zero;

		// Token: 0x0400B8BA RID: 47290
		private Vector2 sharedModListScrollPosition = Vector2.zero;

		// Token: 0x0400B8BB RID: 47291
		private List<string> runningModIdsList;

		// Token: 0x0400B8BC RID: 47292
		private List<string> runningModNamesList;

		// Token: 0x0400B8BD RID: 47293
		private List<string> addedModsList;

		// Token: 0x0400B8BE RID: 47294
		private List<string> missingModsList;

		// Token: 0x0400B8BF RID: 47295
		private List<string> sharedModsList;

		// Token: 0x0400B8C0 RID: 47296
		private static float ButtonWidth = 200f;

		// Token: 0x0400B8C1 RID: 47297
		private static float ButtonHeight = 30f;

		// Token: 0x0400B8C2 RID: 47298
		private static float ModRowHeight = 24f;
	}
}
