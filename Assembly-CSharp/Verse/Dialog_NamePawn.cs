using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000C2C RID: 3116
	public class Dialog_NamePawn : Window
	{
		// Token: 0x06004FED RID: 20461 RVA: 0x0019B2AC File Offset: 0x001994AC
		private Name BuildName()
		{
			if (this.pawn.Name is NameTriple)
			{
				string current = this.names[0].current;
				string first = (current != null) ? current.Trim() : null;
				string current2 = this.names[1].current;
				string nick = (current2 != null) ? current2.Trim() : null;
				string current3 = this.names[2].current;
				return new NameTriple(first, nick, (current3 != null) ? current3.Trim() : null);
			}
			if (this.pawn.Name is NameSingle)
			{
				string current4 = this.names[0].current;
				return new NameSingle((current4 != null) ? current4.Trim() : null, false);
			}
			throw new InvalidOperationException();
		}

		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x06004FEE RID: 20462 RVA: 0x0019B364 File Offset: 0x00199564
		// (set) Token: 0x06004FEF RID: 20463 RVA: 0x0019B3C0 File Offset: 0x001995C0
		private string CurPawnNick
		{
			get
			{
				if (this.pawn.Name is NameTriple)
				{
					return this.names[1].current;
				}
				if (this.pawn.Name is NameSingle)
				{
					return this.names[0].current;
				}
				throw new InvalidOperationException();
			}
			set
			{
				int index;
				if (this.pawn.Name is NameTriple)
				{
					index = 1;
				}
				else
				{
					if (!(this.pawn.Name is NameSingle))
					{
						throw new InvalidOperationException();
					}
					index = 0;
				}
				this.names[index].current = value;
			}
		}

		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x06004FF0 RID: 20464 RVA: 0x0019B413 File Offset: 0x00199613
		private string CurPawnTitle
		{
			get
			{
				if (!(this.names.Last<Dialog_NamePawn.NameContext>().textboxName == "BackstoryTitle"))
				{
					return null;
				}
				return this.names.Last<Dialog_NamePawn.NameContext>().current;
			}
		}

		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x06004FF1 RID: 20465 RVA: 0x0019B443 File Offset: 0x00199643
		public override Vector2 InitialSize
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x06004FF2 RID: 20466 RVA: 0x0019B44C File Offset: 0x0019964C
		public Dialog_NamePawn(Pawn pawn, NameFilter visibleNames, NameFilter editableNames, Dictionary<NameFilter, List<string>> suggestedNames, string initialFirstNameOverride = null, string initialNickNameOverride = null, string initialLastNameOverride = null, string initialTitleOverride = null) : base(null)
		{
			this.pawn = pawn;
			if (pawn.RaceProps.Humanlike)
			{
				this.descriptionText = "{0}: {1}\n{2}: {3}".Formatted("Mother".Translate().CapitalizeFirst(), this.DescribePawn(pawn.GetMother()), "Father".Translate().CapitalizeFirst(), this.DescribePawn(pawn.GetFather()));
				this.renameText = "RenamePerson".Translate().CapitalizeFirst();
				this.portraitDirection = Rot4.South;
				Pawn_StoryTracker story = pawn.story;
				if (((story != null) ? story.bodyType : null) == BodyTypeDefOf.Hulk)
				{
					this.cameraZoom = 0.9f;
					this.humanPortraitVerticalOffset -= 8f;
				}
			}
			else
			{
				this.descriptionText = pawn.KindLabelIndefinite().CapitalizeFirst();
				this.renameText = ((ModsConfig.BiotechActive && pawn.RaceProps.IsMechanoid) ? "RenameMech" : "RenameAnimal").Translate().CapitalizeFirst();
				this.portraitDirection = Rot4.East;
				Vector3 extents = pawn.Drawer.renderer.BodyGraphic.MeshAt(this.portraitDirection).bounds.extents;
				float num = Math.Max(extents.x, extents.z);
				this.cameraZoom = 1f / num;
				this.portraitSize = Mathf.Min(128f, Mathf.Ceil(128f * num));
			}
			NameTriple nameTriple = pawn.Name as NameTriple;
			if (nameTriple != null && (visibleNames & NameFilter.First) > NameFilter.None)
			{
				this.names.Add(new Dialog_NamePawn.NameContext("FirstName", 0, initialFirstNameOverride ?? nameTriple.First, 12, (editableNames & NameFilter.First) > NameFilter.None, (suggestedNames != null) ? suggestedNames.GetWithFallback(NameFilter.First, null) : null));
			}
			if ((visibleNames & NameFilter.Nick) > NameFilter.None)
			{
				string text;
				if (nameTriple != null && !nameTriple.NickSet && (editableNames & NameFilter.Nick) > NameFilter.None)
				{
					text = "";
				}
				else
				{
					text = pawn.Name.ToStringShort;
				}
				this.names.Add(new Dialog_NamePawn.NameContext("NickName", 1, initialNickNameOverride ?? text, 16, (editableNames & NameFilter.Nick) > NameFilter.None, (suggestedNames != null) ? suggestedNames.GetWithFallback(NameFilter.Nick, null) : null));
			}
			if (nameTriple != null && (visibleNames & NameFilter.Last) > NameFilter.None)
			{
				this.names.Add(new Dialog_NamePawn.NameContext("LastName", 2, initialLastNameOverride ?? nameTriple.Last, 12, (editableNames & NameFilter.Last) > NameFilter.None, (suggestedNames != null) ? suggestedNames.GetWithFallback(NameFilter.Last, null) : null));
			}
			if (pawn.story != null && (visibleNames & NameFilter.Title) > NameFilter.None)
			{
				this.names.Add(new Dialog_NamePawn.NameContext("BackstoryTitle", -1, initialTitleOverride ?? (pawn.story.title ?? ""), 16, (editableNames & NameFilter.Title) > NameFilter.None, (suggestedNames != null) ? suggestedNames.GetWithFallback(NameFilter.Title, null) : null));
			}
			float num2 = this.names.Max((Dialog_NamePawn.NameContext name) => name.labelWidth);
			float num3 = this.names.Max((Dialog_NamePawn.NameContext name) => name.textboxWidth);
			foreach (Dialog_NamePawn.NameContext nameContext in this.names)
			{
				nameContext.labelWidth = num2;
				nameContext.textboxWidth = num3;
			}
			this.randomizeText = "Randomize".Translate().CapitalizeFirst();
			this.suggestedText = "Suggested".Translate().CapitalizeFirst() + "...";
			this.randomizeButtonWidth = Dialog_NamePawn.ButtonWidth(this.randomizeText.GetWidthCached());
			this.genderText = string.Format("{0}: {1}", "Gender".Translate().CapitalizeFirst(), pawn.GetGenderLabel().CapitalizeFirst());
			float x = 2f * this.Margin + num2 + num3 + this.randomizeButtonWidth + 34f;
			this.size = new Vector2(x, this.size.y);
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.closeOnClickedOutside = true;
			this.closeOnAccept = false;
		}

		// Token: 0x06004FF3 RID: 20467 RVA: 0x0019B934 File Offset: 0x00199B34
		public override void DoWindowContents(Rect inRect)
		{
			bool flag = false;
			if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter))
			{
				flag = true;
				Event.current.Use();
			}
			bool flag2 = false;
			bool forward = true;
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Tab)
			{
				flag2 = true;
				forward = !Event.current.shift;
				Event.current.Use();
			}
			if (!this.firstCall && Event.current.type == EventType.Layout)
			{
				this.currentControl = GUI.GetNameOfFocusedControl();
			}
			RectAggregator rectAggregator = new RectAggregator(new Rect(inRect.x, inRect.y, inRect.width, 0f), 136098329, new Vector2?(new Vector2(17f, 4f)));
			if (this.renameHeight == null)
			{
				Text.Font = GameFont.Medium;
				this.renameHeight = new float?(Mathf.Ceil(this.renameText.RawText.GetHeightCached()));
				Text.Font = GameFont.Small;
			}
			this.descriptionHeight = new float?(this.descriptionHeight ?? Mathf.Ceil(Text.CalcHeight(this.descriptionText, rectAggregator.Rect.width - this.portraitSize - 17f)));
			float num = this.renameHeight.Value + 4f + this.descriptionHeight.Value;
			if (!this.pawn.RaceProps.Humanlike && this.portraitSize > num)
			{
				num = this.portraitSize;
			}
			RectDivider rectDivider = rectAggregator.NewRow(num, VerticalJustification.Bottom);
			Text.Font = GameFont.Medium;
			Pawn pawn = this.pawn;
			Vector2 vector = new Vector2(this.portraitSize, this.portraitSize);
			Rot4 rotation = this.portraitDirection;
			PawnHealthState? healthStateOverride = new PawnHealthState?(PawnHealthState.Mobile);
			float num2 = this.cameraZoom;
			RenderTexture image = PortraitsCache.Get(pawn, vector, rotation, default(Vector3), num2, true, true, true, true, null, null, false, healthStateOverride);
			Rect position = rectDivider.NewCol(this.portraitSize, HorizontalJustification.Left, null);
			if (this.pawn.RaceProps.Humanlike)
			{
				position.y += this.humanPortraitVerticalOffset;
			}
			position.height = this.portraitSize;
			GUI.DrawTexture(position, image);
			RectDivider rect = rectDivider.NewRow(this.renameHeight.Value, VerticalJustification.Top, null);
			Rect rect2 = rect.NewCol(this.renameHeight.Value, HorizontalJustification.Right, null);
			GUI.DrawTexture(rect2, this.pawn.gender.GetIcon());
			TooltipHandler.TipRegion(rect2, this.genderText);
			Widgets.Label(rect, this.renameText);
			Text.Font = GameFont.Small;
			Widgets.Label(rectDivider.NewRow(this.descriptionHeight.Value, VerticalJustification.Top, null), this.descriptionText);
			Text.Anchor = TextAnchor.MiddleLeft;
			foreach (Dialog_NamePawn.NameContext nameContext in this.names)
			{
				RectDivider rectDivider2 = rectAggregator.NewRow(30f, VerticalJustification.Bottom);
				nameContext.MakeRow(this.pawn, this.randomizeButtonWidth, this.randomizeText, this.suggestedText, ref rectDivider2, ref this.focusControlOverride);
			}
			Text.Anchor = TextAnchor.UpperLeft;
			rectAggregator.NewRow(17.5f, VerticalJustification.Bottom);
			RectDivider rectDivider3 = rectAggregator.NewRow(35f, VerticalJustification.Bottom);
			float width = Mathf.Floor((rectDivider3.Rect.width - 17f) / 2f);
			if (Widgets.ButtonText(rectDivider3.NewCol(width, HorizontalJustification.Left, null), this.cancelText, true, true, true, null))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(rectDivider3.NewCol(width, HorizontalJustification.Left, null), this.acceptText, true, true, true, null) || flag)
			{
				Name name = this.BuildName();
				if (!name.IsValid)
				{
					Messages.Message("NameInvalid".Translate(), this.pawn, MessageTypeDefOf.NeutralEvent, false);
				}
				else
				{
					this.pawn.Name = name;
					if (this.pawn.story != null)
					{
						this.pawn.story.Title = this.CurPawnTitle;
					}
					Find.WindowStack.TryRemove(this, true);
					string text;
					if (this.pawn.def.race.Animal)
					{
						text = "AnimalGainsName".Translate(this.CurPawnNick);
					}
					else if (this.pawn.def.race.IsMechanoid)
					{
						text = "MechGainsName".Translate(this.CurPawnNick);
					}
					else
					{
						NameTriple nameTriple = name as NameTriple;
						if (nameTriple != null)
						{
							text = "PawnGainsName".Translate(nameTriple.Nick, this.pawn.story.Title, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true);
						}
						else
						{
							text = "PawnGainsName".Translate(this.CurPawnNick, this.pawn.story.Title, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true);
						}
					}
					Messages.Message(text, this.pawn, MessageTypeDefOf.PositiveEvent, false);
					this.pawn.babyNamingDeadline = -1;
				}
			}
			this.size = new Vector2(this.size.x, Mathf.Ceil(this.size.y + (rectAggregator.Rect.height - inRect.height)));
			this.SetInitialSizeAndPosition();
			if (flag2 || this.firstCall)
			{
				this.FocusNextControl(this.currentControl, forward);
				this.firstCall = false;
			}
			if (Event.current.type == EventType.Layout && !string.IsNullOrEmpty(this.focusControlOverride))
			{
				GUI.FocusControl(this.focusControlOverride);
				this.focusControlOverride = null;
			}
		}

		// Token: 0x06004FF4 RID: 20468 RVA: 0x0019BFE4 File Offset: 0x0019A1E4
		private void FocusNextControl(string currentControl, bool forward)
		{
			int num = this.names.FindIndex((Dialog_NamePawn.NameContext name) => name.textboxName == currentControl);
			int num2 = -1;
			if (forward)
			{
				for (int i = 1; i <= this.names.Count; i++)
				{
					int num3 = (num + i) % this.names.Count;
					if (this.names[num3].editable)
					{
						num2 = num3;
						break;
					}
				}
			}
			else
			{
				for (int j = 1; j <= this.names.Count; j++)
				{
					int num4 = (this.names.Count + num - j) % this.names.Count;
					if (this.names[num4].editable)
					{
						num2 = num4;
						break;
					}
				}
			}
			if (num2 >= 0)
			{
				this.focusControlOverride = this.names[num2].textboxName;
			}
		}

		// Token: 0x06004FF5 RID: 20469 RVA: 0x0019C0CC File Offset: 0x0019A2CC
		private TaggedString DescribePawn(Pawn pawn)
		{
			if (pawn != null)
			{
				return pawn.FactionDesc(pawn.NameFullColored, false, pawn.NameFullColored, pawn.gender.GetLabel(pawn.RaceProps.Animal)).Resolve();
			}
			return "Unknown".Translate().Colorize(Color.gray);
		}

		// Token: 0x06004FF6 RID: 20470 RVA: 0x0019C131 File Offset: 0x0019A331
		private static float ButtonWidth(float textWidth)
		{
			return Math.Max(114f, textWidth + 35f);
		}

		// Token: 0x04003982 RID: 14722
		private Pawn pawn;

		// Token: 0x04003983 RID: 14723
		private List<Dialog_NamePawn.NameContext> names = new List<Dialog_NamePawn.NameContext>(4);

		// Token: 0x04003984 RID: 14724
		private bool firstCall = true;

		// Token: 0x04003985 RID: 14725
		private string focusControlOverride;

		// Token: 0x04003986 RID: 14726
		private string currentControl;

		// Token: 0x04003987 RID: 14727
		private TaggedString descriptionText;

		// Token: 0x04003988 RID: 14728
		private float? descriptionHeight;

		// Token: 0x04003989 RID: 14729
		private float randomizeButtonWidth;

		// Token: 0x0400398A RID: 14730
		private Vector2 size = new Vector2(800f, 800f);

		// Token: 0x0400398B RID: 14731
		private float? renameHeight;

		// Token: 0x0400398C RID: 14732
		private Rot4 portraitDirection;

		// Token: 0x0400398D RID: 14733
		private float cameraZoom = 1f;

		// Token: 0x0400398E RID: 14734
		private float portraitSize = 128f;

		// Token: 0x0400398F RID: 14735
		private float humanPortraitVerticalOffset = -18f;

		// Token: 0x04003990 RID: 14736
		private TaggedString cancelText = "Cancel".Translate().CapitalizeFirst();

		// Token: 0x04003991 RID: 14737
		private TaggedString acceptText = "Accept".Translate().CapitalizeFirst();

		// Token: 0x04003992 RID: 14738
		private TaggedString randomizeText;

		// Token: 0x04003993 RID: 14739
		private TaggedString suggestedText;

		// Token: 0x04003994 RID: 14740
		private TaggedString renameText;

		// Token: 0x04003995 RID: 14741
		private string genderText;

		// Token: 0x04003996 RID: 14742
		private const float ButtonHeight = 35f;

		// Token: 0x04003997 RID: 14743
		private const float NameFieldsHeight = 30f;

		// Token: 0x04003998 RID: 14744
		private const int MaximumNumberOfNames = 4;

		// Token: 0x04003999 RID: 14745
		private const float VerticalMargin = 4f;

		// Token: 0x0400399A RID: 14746
		private const float HorizontalMargin = 17f;

		// Token: 0x0400399B RID: 14747
		private const float PortraitSize = 128f;

		// Token: 0x0400399C RID: 14748
		private const int ContextHash = 136098329;

		// Token: 0x02000C2D RID: 3117
		private class NameContext
		{
			// Token: 0x06004FF7 RID: 20471 RVA: 0x0019C144 File Offset: 0x0019A344
			public NameContext(string label, int nameIndex, string currentName, int maximumNameLength, bool editable, List<string> suggestedNames)
			{
				this.current = currentName;
				this.nameIndex = nameIndex;
				this.label = label.Translate().CapitalizeFirst() + ":";
				this.labelWidth = Mathf.Ceil(this.label.GetWidthCached());
				this.maximumNameLength = maximumNameLength;
				this.textboxWidth = Mathf.Ceil(Text.CalcSize(new string('W', maximumNameLength + 2)).x);
				this.textboxName = label;
				this.editable = editable;
				this.suggestedNames = suggestedNames;
				if (suggestedNames != null)
				{
					this.suggestedOptions = new List<FloatMenuOption>(suggestedNames.Count);
					using (List<string>.Enumerator enumerator = suggestedNames.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string suggestedName = enumerator.Current;
							this.suggestedOptions.Add(new FloatMenuOption(suggestedName, delegate()
							{
								this.current = suggestedName;
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
					}
				}
			}

			// Token: 0x06004FF8 RID: 20472 RVA: 0x0019C26C File Offset: 0x0019A46C
			public void MakeRow(Pawn pawn, float randomizeButtonWidth, TaggedString randomizeText, TaggedString suggestedText, ref RectDivider divider, ref string focusControlOverride)
			{
				Widgets.Label(divider.NewCol(this.labelWidth, HorizontalJustification.Left, null), this.label);
				RectDivider rect = divider.NewCol(this.textboxWidth, HorizontalJustification.Left, null);
				if (this.editable)
				{
					GUI.SetNextControlName(this.textboxName);
					CharacterCardUtility.DoNameInputRect(rect, ref this.current, this.maximumNameLength);
				}
				else
				{
					Widgets.Label(rect, this.current);
				}
				if (this.editable && this.nameIndex >= 0)
				{
					Rect rect2 = divider.NewCol(randomizeButtonWidth, HorizontalJustification.Left, null);
					if (this.suggestedNames != null)
					{
						List<string> list = this.suggestedNames;
						if (list != null && list.Count > 0 && Widgets.ButtonText(rect2, suggestedText, true, true, true, null))
						{
							Find.WindowStack.Add(new FloatMenu(this.suggestedOptions));
							return;
						}
					}
					else if (Widgets.ButtonText(rect2, randomizeText, true, true, true, null))
					{
						SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
						NameStyle style = NameStyle.Full;
						string forcedLastName = null;
						bool forceNoNick = false;
						Pawn_GeneTracker genes = pawn.genes;
						Name name = PawnBioAndNameGenerator.GeneratePawnName(pawn, style, forcedLastName, forceNoNick, (genes != null) ? genes.Xenotype : null);
						NameTriple nameTriple = name as NameTriple;
						if (nameTriple != null)
						{
							this.current = nameTriple[this.nameIndex];
							return;
						}
						NameSingle nameSingle = name as NameSingle;
						if (nameSingle != null)
						{
							this.current = nameSingle.Name;
						}
					}
				}
			}

			// Token: 0x0400399D RID: 14749
			public string current;

			// Token: 0x0400399E RID: 14750
			public TaggedString label;

			// Token: 0x0400399F RID: 14751
			public float labelWidth;

			// Token: 0x040039A0 RID: 14752
			public int maximumNameLength;

			// Token: 0x040039A1 RID: 14753
			public float textboxWidth;

			// Token: 0x040039A2 RID: 14754
			public string textboxName;

			// Token: 0x040039A3 RID: 14755
			public bool editable;

			// Token: 0x040039A4 RID: 14756
			public int nameIndex;

			// Token: 0x040039A5 RID: 14757
			public List<string> suggestedNames;

			// Token: 0x040039A6 RID: 14758
			private List<FloatMenuOption> suggestedOptions;
		}
	}
}
