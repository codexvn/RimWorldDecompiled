using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020031B2 RID: 12722
	public class Dialog_ReformIdeo : Window
	{
		// Token: 0x17002E78 RID: 11896
		// (get) Token: 0x06011E50 RID: 73296 RVA: 0x005336BC File Offset: 0x005318BC
		public override Vector2 InitialSize
		{
			get
			{
				if (this.stage == IdeoReformStage.MemesAndStyles)
				{
					return new Vector2(750f, 700f);
				}
				return new Vector2(750f, Mathf.Min(1000f, (float)UI.screenHeight));
			}
		}

		// Token: 0x17002E79 RID: 11897
		// (get) Token: 0x06011E51 RID: 73297 RVA: 0x005336F0 File Offset: 0x005318F0
		public bool StructureMemeChanged
		{
			get
			{
				return this.newIdeo.StructureMeme != this.ideo.StructureMeme;
			}
		}

		// Token: 0x17002E7A RID: 11898
		// (get) Token: 0x06011E52 RID: 73298 RVA: 0x00533710 File Offset: 0x00531910
		public bool NormalMemesChanged
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.ideo.memes.Count; i++)
				{
					if (this.ideo.memes[i].category == MemeCategory.Normal)
					{
						if (!this.newIdeo.memes.Contains(this.ideo.memes[i]))
						{
							return true;
						}
						num++;
					}
				}
				int num2 = 0;
				for (int j = 0; j < this.newIdeo.memes.Count; j++)
				{
					if (this.newIdeo.memes[j].category == MemeCategory.Normal)
					{
						num2++;
					}
				}
				return num != num2;
			}
		}

		// Token: 0x17002E7B RID: 11899
		// (get) Token: 0x06011E53 RID: 73299 RVA: 0x005337BB File Offset: 0x005319BB
		public bool StylesChanged
		{
			get
			{
				return !this.newIdeo.thingStyleCategories.SetsEqual(this.ideo.thingStyleCategories) && !this.StructureMemeChanged && !this.NormalMemesChanged;
			}
		}

		// Token: 0x17002E7C RID: 11900
		// (get) Token: 0x06011E54 RID: 73300 RVA: 0x005337ED File Offset: 0x005319ED
		public bool AnyChooseOneChanges
		{
			get
			{
				return this.StructureMemeChanged || this.NormalMemesChanged || this.StylesChanged;
			}
		}

		// Token: 0x06011E55 RID: 73301 RVA: 0x00533808 File Offset: 0x00531A08
		public Dialog_ReformIdeo(Ideo ideo) : base(null)
		{
			if (!ModLister.CheckIdeology("Reform ideo dialog"))
			{
				return;
			}
			this.forcePause = true;
			this.doCloseX = false;
			this.doCloseButton = false;
			this.absorbInputAroundWindow = true;
			this.forceCatchAcceptAndCancelEventEvenIfUnfocused = true;
			this.ideo = ideo;
			this.newIdeo = IdeoGenerator.MakeIdeo(ideo.foundation.def);
			ideo.CopyTo(this.newIdeo);
		}

		// Token: 0x06011E56 RID: 73302 RVA: 0x0053388C File Offset: 0x00531A8C
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(inRect.x, inRect.y, inRect.width, 40f), "ReformIdeoligion".Translate());
			Text.Font = GameFont.Small;
			Widgets.Label(new Rect(inRect.x, inRect.y + 40f, inRect.width, 50f), "ReformIdeoligionDesc".Translate());
			Rect outRect = new Rect(inRect.x, inRect.y + 40f + 50f, inRect.width, inRect.height - 55f - 40f - 50f);
			Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, this.scrollViewHeight);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			float num = 0f;
			if (this.stage == IdeoReformStage.MemesAndStyles)
			{
				Rect rect = new Rect(0f, num, viewRect.width - 16f, Text.LineHeight);
				Widgets.Label(rect, "ReformIdeoChooseOneChange".Translate());
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
				Widgets.DrawLineHorizontal(0f, rect.y + Text.LineHeight, viewRect.width - 16f);
				GUI.color = Color.white;
				num += 2f * Text.LineHeight;
				Widgets.Label(new Rect(viewRect.x, num, viewRect.width, Text.LineHeight), "ReformIdeoChangeStructure".Translate());
				num += Text.LineHeight + 10f;
				bool flag = this.AnyChooseOneChanges && !this.StructureMemeChanged;
				Rect rect2 = new Rect(viewRect.x + viewRect.width / 2f - Dialog_ReformIdeo.MemeBoxSize.x / 2f, num, Dialog_ReformIdeo.MemeBoxSize.x, Dialog_ReformIdeo.MemeBoxSize.y);
				IdeoUIUtility.DoMeme(rect2, this.newIdeo.StructureMeme, this.newIdeo, flag ? IdeoEditMode.None : IdeoEditMode.Reform, true, null);
				if (flag)
				{
					Widgets.DrawRectFast(rect2, Dialog_ReformIdeo.DisabledColor, null);
					if (Widgets.ButtonInvisible(rect2, true))
					{
						Messages.Message("MessageFluidIdeoOneChangeAllowed".Translate(), MessageTypeDefOf.RejectInput, false);
					}
				}
				num += Dialog_ReformIdeo.MemeBoxSize.y + 17f;
				this.tmpNormalMemes.Clear();
				for (int i = 0; i < this.newIdeo.memes.Count; i++)
				{
					if (this.newIdeo.memes[i].category == MemeCategory.Normal)
					{
						this.tmpNormalMemes.Add(this.newIdeo.memes[i]);
					}
				}
				Widgets.Label(new Rect(viewRect.x, num, viewRect.width, Text.LineHeight), (this.ideo.memes.Count((MemeDef m) => m.category == MemeCategory.Normal) <= 1) ? "ReformIdeoAddMeme".Translate() : "ReformIdeoAddOrRemoveMeme".Translate());
				num += Text.LineHeight + 10f;
				int num2 = Mathf.CeilToInt((float)this.tmpNormalMemes.Count / 5f);
				bool flag2 = this.AnyChooseOneChanges && !this.NormalMemesChanged;
				for (int j = 0; j < num2; j++)
				{
					num += (float)j * Dialog_ReformIdeo.MemeBoxSize.y + (float)((j > 0) ? 10 : 0);
					int num3 = j * 5;
					int num4 = Mathf.Min(5, this.tmpNormalMemes.Count - j * 5);
					float num5 = (float)num4 * (Dialog_ReformIdeo.MemeBoxSize.x + 10f);
					float num6 = (viewRect.width - num5) / 2f;
					for (int k = num3; k < num3 + num4; k++)
					{
						Rect rect3 = new Rect(num6, num, Dialog_ReformIdeo.MemeBoxSize.x, Dialog_ReformIdeo.MemeBoxSize.y);
						IdeoUIUtility.DoMeme(rect3, this.tmpNormalMemes[k], this.newIdeo, flag2 ? IdeoEditMode.None : IdeoEditMode.Reform, true, delegate
						{
							this.tmpPreSelectedMemes.Clear();
							this.tmpPreSelectedMemes.AddRange(from m in this.newIdeo.memes
							where !this.ideo.memes.Contains(m)
							select m);
							this.ideo.CopyTo(this.newIdeo);
							Find.WindowStack.Add(new Dialog_ChooseMemes(this.newIdeo, MemeCategory.Normal, false, null, this.tmpPreSelectedMemes, true));
						});
						if (flag2)
						{
							Widgets.DrawRectFast(rect3, Dialog_ReformIdeo.DisabledColor, null);
							if (Widgets.ButtonInvisible(rect3, true))
							{
								Messages.Message("MessageFluidIdeoOneChangeAllowed".Translate(), MessageTypeDefOf.RejectInput, false);
							}
						}
						num6 += Dialog_ReformIdeo.MemeBoxSize.x + 10f;
					}
				}
				this.tmpNormalMemes.Clear();
				num += Dialog_ReformIdeo.MemeBoxSize.y + 17f;
				Widgets.Label(new Rect(viewRect.x, num, viewRect.width, Text.LineHeight), "ReformIdeoChangeStyles".Translate());
				num += Text.LineHeight + 10f;
				float x = viewRect.x;
				Rect position = new Rect(x, num, 0f, 0f);
				bool flag3 = this.AnyChooseOneChanges && !this.StylesChanged;
				IdeoUIUtility.DoStyles(ref num, ref x, viewRect.width, this.newIdeo, flag3 ? IdeoEditMode.None : IdeoEditMode.Reform, 50);
				if (flag3)
				{
					position.width = x - position.x;
					position.height = num - position.y;
					Widgets.DrawRectFast(position, Dialog_ReformIdeo.DisabledColor, null);
				}
				num += 67f;
			}
			else
			{
				Rect rect4 = new Rect(0f, num, viewRect.width - 16f, Text.LineHeight);
				Widgets.Label(rect4, "ReformIdeoChangeAny".Translate());
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
				Widgets.DrawLineHorizontal(0f, rect4.y + Text.LineHeight, viewRect.width - 16f);
				GUI.color = Color.white;
				num += 2f * Text.LineHeight;
				float width = viewRect.width - 16f;
				IdeoUIUtility.DoNameAndSymbol(ref num, width, this.newIdeo, IdeoEditMode.Reform);
				num += 10f;
				IdeoUIUtility.DoDescription(ref num, width, this.newIdeo, IdeoEditMode.Reform);
				num += 10f;
				if (this.newIdeo.foundation != null)
				{
					IdeoUIUtility.DoFoundationInfo(ref num, width, this.newIdeo, IdeoEditMode.Reform);
					num += 10f;
				}
				IdeoUIUtility.DoPrecepts(ref num, width, this.newIdeo, IdeoEditMode.Reform);
				num += 10f;
				IdeoUIUtility.DoAppearanceItems(this.newIdeo, IdeoEditMode.Reform, ref num, width);
			}
			if (Event.current.type == EventType.Layout)
			{
				this.scrollViewHeight = num;
			}
			Widgets.EndScrollView();
			Rect rect5 = new Rect(inRect.xMax - Window.CloseButSize.x, inRect.height - Window.CloseButSize.y, Window.CloseButSize.x, Window.CloseButSize.y);
			if (this.stage == IdeoReformStage.MemesAndStyles)
			{
				Rect rect6 = rect5;
				rect6.x = inRect.x;
				if (Widgets.ButtonText(rect6, "Cancel".Translate(), true, true, true, null))
				{
					this.Close(true);
				}
				if (this.AnyChooseOneChanges)
				{
					if (Widgets.ButtonText(new Rect(inRect.x + (inRect.width - Window.CloseButSize.x) / 2f, inRect.height - Window.CloseButSize.y, Window.CloseButSize.x, Window.CloseButSize.y), "ReformIdeoResetChanges".Translate(), true, true, true, null))
					{
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
						this.ResetAllChooseOneChanges();
					}
					num += 47f;
				}
				if (Widgets.ButtonText(rect5, "Next".Translate(), true, true, true, null))
				{
					this.stage = IdeoReformStage.PreceptsNarrativeAndDeities;
					return;
				}
			}
			else
			{
				Rect rect7 = rect5;
				rect7.x = inRect.x;
				Rect rect8 = inRect;
				rect8.xMin = rect8.xMax - rect5.width * 3.1f;
				rect8.width = rect5.width * 2f;
				rect8.yMin = rect8.yMax - rect5.height;
				Pair<Precept, Precept> lhs = this.newIdeo.FirstIncompatiblePreceptPair();
				if (lhs != default(Pair<Precept, Precept>))
				{
					GUI.color = Color.red;
					Text.Font = GameFont.Tiny;
					Text.Anchor = TextAnchor.UpperRight;
					string text = lhs.First.TipLabel;
					string text2 = lhs.Second.TipLabel;
					if (text == text2)
					{
						text = lhs.First.UIInfoSecondLine;
						text2 = lhs.Second.UIInfoSecondLine;
					}
					Widgets.Label(rect8, "MessageIdeoIncompatiblePrecepts".Translate(text.Named("PRECEPT1"), text2.Named("PRECEPT2")).CapitalizeFirst());
					Text.Font = GameFont.Small;
					Text.Anchor = TextAnchor.UpperLeft;
					GUI.color = Color.white;
				}
				if (Widgets.ButtonText(rect7, "Back".Translate(), true, true, true, null))
				{
					this.stage = IdeoReformStage.MemesAndStyles;
				}
				Rect rect9 = rect5;
				rect9.x = inRect.x + (inRect.width - rect9.width) / 2f - ((lhs != default(Pair<Precept, Precept>)) ? 65f : 0f);
				if (Widgets.ButtonText(rect9, "Randomize".Translate(), true, true, true, null))
				{
					this.RandomizeNewIdeo();
				}
				if (Widgets.ButtonText(rect5, "DoneButton".Translate(), true, true, true, null))
				{
					IdeoDevelopmentUtility.ConfirmChangesToIdeo(this.ideo, this.newIdeo, delegate
					{
						IdeoDevelopmentUtility.ApplyChangesToIdeo(this.ideo, this.newIdeo);
						this.Close(true);
					});
				}
			}
		}

		// Token: 0x06011E57 RID: 73303 RVA: 0x00534298 File Offset: 0x00532498
		private void RandomizeNewIdeo()
		{
			IdeoGenerationParms parms = new IdeoGenerationParms(Faction.OfPlayer.def, false, null, null, null, false, false, false, false, "", null, null, false, "", false);
			this.newIdeo.foundation.RandomizeCulture(parms);
			this.newIdeo.foundation.RandomizePlace();
			IdeoFoundation_Deity ideoFoundation_Deity = this.newIdeo.foundation as IdeoFoundation_Deity;
			if (ideoFoundation_Deity != null)
			{
				ideoFoundation_Deity.GenerateDeities();
			}
			this.newIdeo.foundation.GenerateTextSymbols();
			this.newIdeo.foundation.GenerateLeaderTitle();
			this.newIdeo.foundation.RandomizeIcon();
			this.newIdeo.foundation.RandomizePrecepts(true, parms);
			this.newIdeo.RegenerateDescription(true);
		}

		// Token: 0x06011E58 RID: 73304 RVA: 0x00534358 File Offset: 0x00532558
		private void ResetAllChooseOneChanges()
		{
			this.newIdeo.memes.Clear();
			this.newIdeo.memes.AddRange(this.ideo.memes);
			this.newIdeo.thingStyleCategories.Clear();
			this.newIdeo.thingStyleCategories.AddRange(this.ideo.thingStyleCategories);
		}

		// Token: 0x0400B91B RID: 47387
		public const float HeaderHeight = 40f;

		// Token: 0x0400B91C RID: 47388
		public const float DescriptionHeight = 50f;

		// Token: 0x0400B91D RID: 47389
		private static readonly Vector2 MemeBoxSize = IdeoUIUtility.MemeBoxSize;

		// Token: 0x0400B91E RID: 47390
		private const int StyleBoxSize = 50;

		// Token: 0x0400B91F RID: 47391
		private const int MemeBoxGap = 10;

		// Token: 0x0400B920 RID: 47392
		private const int ClearChangesBtnHeight = 30;

		// Token: 0x0400B921 RID: 47393
		private const int Width = 750;

		// Token: 0x0400B922 RID: 47394
		private static readonly Color DisabledColor = new Color32(55, 55, 55, 200);

		// Token: 0x0400B923 RID: 47395
		private const int MaxMemesPerRow = 5;

		// Token: 0x0400B924 RID: 47396
		private Ideo newIdeo;

		// Token: 0x0400B925 RID: 47397
		private Ideo ideo;

		// Token: 0x0400B926 RID: 47398
		private Vector2 scrollPosition;

		// Token: 0x0400B927 RID: 47399
		private float scrollViewHeight;

		// Token: 0x0400B928 RID: 47400
		private IdeoReformStage stage;

		// Token: 0x0400B929 RID: 47401
		private List<MemeDef> tmpNormalMemes = new List<MemeDef>();

		// Token: 0x0400B92A RID: 47402
		private List<MemeDef> tmpPreSelectedMemes = new List<MemeDef>();
	}
}
