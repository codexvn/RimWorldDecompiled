using System;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x0200329B RID: 12955
	public class Page_ScenarioEditor : Page
	{
		// Token: 0x17002F04 RID: 12036
		// (get) Token: 0x0601239F RID: 74655 RVA: 0x0055B9D9 File Offset: 0x00559BD9
		public bool ScenarioEdited
		{
			get
			{
				return this.editMode && this.curScen.GetHashCode() != this.initialHash;
			}
		}

		// Token: 0x17002F05 RID: 12037
		// (get) Token: 0x060123A0 RID: 74656 RVA: 0x0055B9FB File Offset: 0x00559BFB
		public override string PageTitle
		{
			get
			{
				return "ScenarioEditor".Translate() + (this.ScenarioEdited ? "*" : "");
			}
		}

		// Token: 0x17002F06 RID: 12038
		// (get) Token: 0x060123A1 RID: 74657 RVA: 0x0055BA25 File Offset: 0x00559C25
		public Scenario EditingScenario
		{
			get
			{
				return this.curScen;
			}
		}

		// Token: 0x060123A2 RID: 74658 RVA: 0x0055BA30 File Offset: 0x00559C30
		public Page_ScenarioEditor(Scenario scen)
		{
			if (scen != null)
			{
				this.curScen = scen;
				this.seedIsValid = false;
			}
			else
			{
				this.RandomizeSeedAndScenario();
			}
			this.initialHash = this.curScen.GetHashCode();
		}

		// Token: 0x060123A3 RID: 74659 RVA: 0x0055BA86 File Offset: 0x00559C86
		public override void PreOpen()
		{
			base.PreOpen();
			this.infoScrollPosition = Vector2.zero;
			this.needsSave = true;
		}

		// Token: 0x060123A4 RID: 74660 RVA: 0x0055BAA0 File Offset: 0x00559CA0
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			Rect mainRect = base.GetMainRect(rect, 0f, false);
			Widgets.BeginGroup(mainRect);
			Rect rect2 = new Rect(0f, 0f, mainRect.width * 0.35f, mainRect.height).Rounded();
			this.DoConfigControls(rect2);
			Rect rect3 = new Rect(rect2.xMax + 17f, 0f, mainRect.width - rect2.width - 17f, mainRect.height).Rounded();
			if (!this.editMode)
			{
				ScenarioUI.DrawScenarioInfo(rect3, this.curScen, ref this.infoScrollPosition);
			}
			else
			{
				ScenarioUI.DrawScenarioEditInterface(rect3, this.curScen, ref this.infoScrollPosition);
			}
			Widgets.EndGroup();
			base.DoBottomButtons(rect, null, null, null, true, true);
		}

		// Token: 0x060123A5 RID: 74661 RVA: 0x0055BB70 File Offset: 0x00559D70
		private void RandomizeSeedAndScenario()
		{
			this.seed = GenText.RandomSeedString();
			this.curScen = ScenarioMaker.GenerateNewRandomScenario(this.seed);
		}

		// Token: 0x060123A6 RID: 74662 RVA: 0x0055BB90 File Offset: 0x00559D90
		private void DoConfigControls(Rect rect)
		{
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = 200f;
			listing_Standard.Begin(rect);
			if (listing_Standard.ButtonText("Load".Translate(), null, 1f))
			{
				Find.WindowStack.Add(new Dialog_ScenarioList_Load(delegate(Scenario loadedScen)
				{
					this.curScen = loadedScen;
					this.seedIsValid = false;
				}));
			}
			if (listing_Standard.ButtonText("Save".Translate(), null, 1f) && Page_ScenarioEditor.CheckAllPartsCompatible(this.curScen))
			{
				Find.WindowStack.Add(new Dialog_ScenarioList_Save(this.curScen, null));
			}
			if (listing_Standard.ButtonText("RandomizeSeed".Translate(), null, 1f))
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				this.RandomizeSeedAndScenario();
				this.seedIsValid = true;
			}
			if (this.seedIsValid)
			{
				listing_Standard.Label("Seed".Translate().CapitalizeFirst(), -1f, null);
				string a = listing_Standard.TextEntry(this.seed, 1);
				if (a != this.seed)
				{
					this.seed = a;
					this.curScen = ScenarioMaker.GenerateNewRandomScenario(this.seed);
				}
			}
			else
			{
				listing_Standard.Gap(Text.LineHeight + Text.LineHeight + 2f);
			}
			listing_Standard.CheckboxLabeled("EditMode".Translate().CapitalizeFirst(), ref this.editMode, null, 0f, 1f);
			if (this.editMode)
			{
				this.seedIsValid = false;
				if (listing_Standard.ButtonText("AddPart".Translate(), null, 1f))
				{
					this.OpenAddScenPartMenu();
				}
				if (SteamManager.Initialized && (this.curScen.Category == ScenarioCategory.CustomLocal || this.curScen.Category == ScenarioCategory.SteamWorkshop) && listing_Standard.ButtonText(Workshop.UploadButtonLabel(this.curScen.GetPublishedFileId()), null, 1f) && Page_ScenarioEditor.CheckAllPartsCompatible(this.curScen))
				{
					AcceptanceReport acceptanceReport = this.curScen.TryUploadReport();
					if (!acceptanceReport.Accepted)
					{
						Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
					}
					else
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmSteamWorkshopUpload".Translate(), delegate()
						{
							SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmContentAuthor".Translate(), delegate()
							{
								SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
								Workshop.Upload(this.curScen);
							}, true, null, WindowLayer.Dialog));
						}, true, null, WindowLayer.Dialog));
					}
				}
			}
			listing_Standard.End();
		}

		// Token: 0x060123A7 RID: 74663 RVA: 0x0055BDF0 File Offset: 0x00559FF0
		private static bool CheckAllPartsCompatible(Scenario scen)
		{
			foreach (ScenPart scenPart in scen.AllParts)
			{
				int num = 0;
				foreach (ScenPart scenPart2 in scen.AllParts)
				{
					if (scenPart2.def == scenPart.def)
					{
						num++;
					}
					if (num > scenPart.def.maxUses)
					{
						Messages.Message("TooMany".Translate(scenPart.def.maxUses) + ": " + scenPart.def.label, MessageTypeDefOf.RejectInput, false);
						return false;
					}
					if (scenPart != scenPart2 && !scenPart.CanCoexistWith(scenPart2))
					{
						Messages.Message("Incompatible".Translate() + ": " + scenPart.def.label + ", " + scenPart2.def.label, MessageTypeDefOf.RejectInput, false);
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060123A8 RID: 74664 RVA: 0x0055BF68 File Offset: 0x0055A168
		private void OpenAddScenPartMenu()
		{
			FloatMenuUtility.MakeMenu<ScenPartDef>(from p in ScenarioMaker.AddableParts(this.curScen)
			where p.PlayerAddRemovable
			orderby p.label
			select p, (ScenPartDef p) => p.LabelCap, (ScenPartDef p) => delegate()
			{
				this.AddScenPart(p);
			});
		}

		// Token: 0x060123A9 RID: 74665 RVA: 0x0055BFF8 File Offset: 0x0055A1F8
		private void AddScenPart(ScenPartDef def)
		{
			ScenPart scenPart = ScenarioMaker.MakeScenPart(def);
			scenPart.Randomize();
			this.curScen.parts.Add(scenPart);
		}

		// Token: 0x060123AA RID: 74666 RVA: 0x0055C024 File Offset: 0x0055A224
		protected override bool CanDoNext()
		{
			if (!base.CanDoNext())
			{
				return false;
			}
			if (this.curScen == null)
			{
				return false;
			}
			if (!Page_ScenarioEditor.CheckAllPartsCompatible(this.curScen))
			{
				return false;
			}
			if (!this.ScenarioEdited || !this.needsSave)
			{
				Page_SelectScenario.BeginScenarioConfiguration(this.curScen, this);
				return true;
			}
			Find.WindowStack.Add(new Dialog_MessageBox("ScenarioChangedSavePrompt".Translate(), "Yes".Translate(), delegate()
			{
				Find.WindowStack.Add(new Dialog_ScenarioList_Save(this.curScen, delegate()
				{
					Page_SelectScenario.BeginScenarioConfiguration(this.curScen, this);
					this.needsSave = false;
					this.DoNext();
				}));
			}, "No".Translate(), delegate()
			{
				Page_SelectScenario.BeginScenarioConfiguration(this.curScen, this);
				this.needsSave = false;
				this.DoNext();
			}, null, false, null, delegate()
			{
			}, WindowLayer.Dialog));
			return false;
		}

		// Token: 0x0400BDB3 RID: 48563
		private Scenario curScen;

		// Token: 0x0400BDB4 RID: 48564
		private Vector2 infoScrollPosition = Vector2.zero;

		// Token: 0x0400BDB5 RID: 48565
		private string seed;

		// Token: 0x0400BDB6 RID: 48566
		private bool seedIsValid = true;

		// Token: 0x0400BDB7 RID: 48567
		private bool editMode;

		// Token: 0x0400BDB8 RID: 48568
		private bool needsSave = true;

		// Token: 0x0400BDB9 RID: 48569
		private int initialHash;
	}
}
