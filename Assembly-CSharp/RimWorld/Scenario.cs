using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Steamworks;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02002863 RID: 10339
	public class Scenario : IExposable, WorkshopUploadable
	{
		// Token: 0x17002274 RID: 8820
		// (get) Token: 0x0600E209 RID: 57865 RVA: 0x004237B7 File Offset: 0x004219B7
		public IEnumerable<System.Version> SupportedVersions
		{
			get
			{
				yield return new System.Version(VersionControl.CurrentMajor, VersionControl.CurrentMinor);
				yield break;
			}
		}

		// Token: 0x17002275 RID: 8821
		// (get) Token: 0x0600E20A RID: 57866 RVA: 0x004237C0 File Offset: 0x004219C0
		public FileInfo File
		{
			get
			{
				return new FileInfo(GenFilePaths.AbsPathForScenario(this.fileName));
			}
		}

		// Token: 0x17002276 RID: 8822
		// (get) Token: 0x0600E20B RID: 57867 RVA: 0x004237D2 File Offset: 0x004219D2
		public IEnumerable<ScenPart> AllParts
		{
			get
			{
				yield return this.playerFaction;
				yield return this.surfaceLayer;
				int num;
				for (int i = 0; i < this.parts.Count; i = num + 1)
				{
					yield return this.parts[i];
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17002277 RID: 8823
		// (get) Token: 0x0600E20C RID: 57868 RVA: 0x004237E2 File Offset: 0x004219E2
		// (set) Token: 0x0600E20D RID: 57869 RVA: 0x0042380E File Offset: 0x00421A0E
		public ScenarioCategory Category
		{
			get
			{
				if (this.categoryInt == ScenarioCategory.Undefined)
				{
					Log.Error("Category is Undefined on Scenario " + ((this != null) ? this.ToString() : null));
				}
				return this.categoryInt;
			}
			set
			{
				this.categoryInt = value;
			}
		}

		// Token: 0x17002278 RID: 8824
		// (get) Token: 0x0600E20E RID: 57870 RVA: 0x00423818 File Offset: 0x00421A18
		public bool OverrideDangerMusic
		{
			get
			{
				using (IEnumerator<ScenPart> enumerator = this.AllParts.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.OverrideDangerMusic)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x0600E210 RID: 57872 RVA: 0x004238A0 File Offset: 0x00421AA0
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<string>(ref this.summary, "summary", null, false);
			Scribe_Values.Look<string>(ref this.description, "description", null, false);
			Scribe_Values.Look<PublishedFileId_t>(ref this.publishedFileIdInt, "publishedFileId", PublishedFileId_t.Invalid, false);
			Scribe_Deep.Look<ScenPart_PlayerFaction>(ref this.playerFaction, "playerFaction", Array.Empty<object>());
			Scribe_Deep.Look<ScenPart_PlanetLayer>(ref this.surfaceLayer, "surfaceLayer", Array.Empty<object>());
			Scribe_Collections.Look<ScenPart>(ref this.parts, "parts", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.surfaceLayer == null)
				{
					this.surfaceLayer = new ScenPart_PlanetLayer
					{
						def = ScenPartDefOf.PlanetLayerFixed,
						layer = PlanetLayerDefOf.Surface,
						settingsDef = PlanetLayerSettingsDefOf.Surface,
						hide = true,
						tag = "Surface"
					};
				}
				if (ModsConfig.OdysseyActive)
				{
					if (!this.parts.Any(delegate(ScenPart p)
					{
						ScenPart_PlanetLayer scenPart_PlanetLayer2 = p as ScenPart_PlanetLayer;
						return scenPart_PlanetLayer2 != null && scenPart_PlanetLayer2.tag == "Orbit";
					}))
					{
						ScenPart_PlanetLayer scenPart_PlanetLayer = new ScenPart_PlanetLayer
						{
							def = ScenPartDefOf.PlanetLayerFixed,
							layer = PlanetLayerDefOf.Orbit,
							settingsDef = PlanetLayerSettingsDefOf.Orbit,
							hide = true,
							tag = "Orbit"
						};
						this.parts.Add(scenPart_PlanetLayer);
						this.surfaceLayer.connections.Add(new LayerConnection
						{
							tag = scenPart_PlanetLayer.tag,
							zoomMode = LayerConnection.ZoomMode.ZoomOut
						});
						scenPart_PlanetLayer.connections.Add(new LayerConnection
						{
							tag = this.surfaceLayer.tag,
							zoomMode = LayerConnection.ZoomMode.ZoomIn
						});
					}
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = this.parts.Count - 1; i >= 0; i--)
				{
					if (!this.parts[i].Valid())
					{
						this.parts.RemoveAt(i);
					}
				}
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600E211 RID: 57873 RVA: 0x00423A9D File Offset: 0x00421C9D
		public IEnumerable<string> ConfigErrors()
		{
			if (this.name.NullOrEmpty())
			{
				yield return "no title";
			}
			if (this.parts.NullOrEmpty<ScenPart>())
			{
				yield return "no parts";
			}
			if (this.playerFaction == null)
			{
				yield return "no playerFaction";
			}
			if (this.surfaceLayer == null)
			{
				yield return "no surfaceLayer";
			}
			else if (!this.surfaceLayer.layer.layerType.SameOrSubclassOf<SurfaceLayer>())
			{
				yield return "surfaceLayer layer type " + this.surfaceLayer.layer.layerType.Name + " is not the same or a subclass of SurfaceLayer";
			}
			foreach (ScenPart scenPart in this.AllParts)
			{
				if (scenPart == null)
				{
					yield return "scenario has null part";
				}
				else
				{
					foreach (string text in scenPart.ConfigErrors())
					{
						yield return text;
					}
					IEnumerator<string> enumerator2 = null;
				}
			}
			IEnumerator<ScenPart> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600E212 RID: 57874 RVA: 0x00423AB0 File Offset: 0x00421CB0
		public string GetFullInformationText()
		{
			string result;
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(this.description);
				stringBuilder.AppendLine();
				foreach (ScenPart scenPart in this.AllParts)
				{
					scenPart.summarized = false;
				}
				foreach (ScenPart scenPart2 in from p in this.AllParts
				orderby p.def.summaryPriority descending, p.def.defName
				where p.visible
				select p)
				{
					string text = scenPart2.Summary(this);
					if (!text.NullOrEmpty())
					{
						stringBuilder.AppendLine(text);
					}
				}
				result = stringBuilder.ToString().TrimEndNewlines();
			}
			catch (Exception ex)
			{
				Log.ErrorOnce("Exception in Scenario.GetFullInformationText():\n" + ex.ToString(), 10395878);
				result = "Cannot read data.";
			}
			return result;
		}

		// Token: 0x0600E213 RID: 57875 RVA: 0x00423C30 File Offset: 0x00421E30
		public string GetSummary()
		{
			return this.summary;
		}

		// Token: 0x0600E214 RID: 57876 RVA: 0x00423C38 File Offset: 0x00421E38
		public Scenario CopyForEditing()
		{
			Scenario scenario = new Scenario();
			scenario.name = this.name;
			scenario.summary = this.summary;
			scenario.description = this.description;
			scenario.playerFaction = (ScenPart_PlayerFaction)this.playerFaction.CopyForEditing();
			scenario.surfaceLayer = (ScenPart_PlanetLayer)this.surfaceLayer.CopyForEditing();
			scenario.parts.AddRange(from p in this.parts
			select p.CopyForEditing());
			scenario.categoryInt = ScenarioCategory.CustomLocal;
			return scenario;
		}

		// Token: 0x0600E215 RID: 57877 RVA: 0x00423CD8 File Offset: 0x00421ED8
		public void PreConfigure()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PreConfigure();
			}
		}

		// Token: 0x0600E216 RID: 57878 RVA: 0x00423D24 File Offset: 0x00421F24
		public virtual Page GetFirstConfigPage()
		{
			List<Page> list = new List<Page>();
			list.Add(new Page_SelectStoryteller());
			list.Add(new Page_CreateWorldParams());
			if (!this.parts.Any((ScenPart p) => p is ScenPart_ForcedMap))
			{
				list.Add(new Page_SelectStartingSite());
			}
			if (ModsConfig.IdeologyActive)
			{
				list.Add(new Page_ChooseIdeoPreset());
			}
			foreach (Page item in this.parts.SelectMany((ScenPart p) => p.GetConfigPages()))
			{
				list.Add(item);
			}
			Page page = PageUtility.StitchedPages(list);
			if (page != null)
			{
				Page page2 = page;
				while (page2.next != null)
				{
					page2 = page2.next;
				}
				page2.nextAct = new Action(PageUtility.InitGameStart);
			}
			return page;
		}

		// Token: 0x0600E217 RID: 57879 RVA: 0x00423E30 File Offset: 0x00422030
		public bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			using (IEnumerator<ScenPart> enumerator = this.AllParts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.AllowPlayerStartingPawn(pawn, tryingToRedress, req))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600E218 RID: 57880 RVA: 0x00423E88 File Offset: 0x00422088
		public void Notify_NewPawnGenerating(Pawn pawn, PawnGenerationContext context)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.Notify_NewPawnGenerating(pawn, context);
			}
		}

		// Token: 0x0600E219 RID: 57881 RVA: 0x00423ED4 File Offset: 0x004220D4
		public void Notify_PawnGenerated(Pawn pawn, PawnGenerationContext context, bool redressed)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.Notify_PawnGenerated(pawn, context, redressed);
			}
		}

		// Token: 0x0600E21A RID: 57882 RVA: 0x00423F24 File Offset: 0x00422124
		public void Notify_PawnDied(Corpse corpse)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_PawnDied(corpse);
			}
		}

		// Token: 0x0600E21B RID: 57883 RVA: 0x00423F5C File Offset: 0x0042215C
		public void PostWorldGenerate()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PostWorldGenerate();
			}
		}

		// Token: 0x0600E21C RID: 57884 RVA: 0x00423FA8 File Offset: 0x004221A8
		public void PostIdeoChosen()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PostIdeoChosen();
			}
		}

		// Token: 0x0600E21D RID: 57885 RVA: 0x00423FF4 File Offset: 0x004221F4
		public void PreMapGenerate()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PreMapGenerate();
			}
		}

		// Token: 0x0600E21E RID: 57886 RVA: 0x00424040 File Offset: 0x00422240
		public void GenerateIntoMap(Map map)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.GenerateIntoMap(map);
			}
		}

		// Token: 0x0600E21F RID: 57887 RVA: 0x0042408C File Offset: 0x0042228C
		public void PostMapGenerate(Map map)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PostMapGenerate(map);
			}
		}

		// Token: 0x0600E220 RID: 57888 RVA: 0x004240D8 File Offset: 0x004222D8
		public void MapRemoved(Map map)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.MapRemoved(map);
			}
		}

		// Token: 0x0600E221 RID: 57889 RVA: 0x00424124 File Offset: 0x00422324
		public void PostGameStart()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PostGameStart();
			}
		}

		// Token: 0x0600E222 RID: 57890 RVA: 0x00424170 File Offset: 0x00422370
		public float GetStatFactor(StatDef stat)
		{
			float num = 1f;
			for (int i = 0; i < this.parts.Count; i++)
			{
				ScenPart_StatFactor scenPart_StatFactor = this.parts[i] as ScenPart_StatFactor;
				if (scenPart_StatFactor != null)
				{
					num *= scenPart_StatFactor.GetStatFactor(stat);
				}
			}
			return num;
		}

		// Token: 0x0600E223 RID: 57891 RVA: 0x004241BC File Offset: 0x004223BC
		public void TickScenario()
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Tick();
			}
		}

		// Token: 0x0600E224 RID: 57892 RVA: 0x004241F0 File Offset: 0x004223F0
		public void RemovePart(ScenPart part)
		{
			if (!this.parts.Contains(part))
			{
				Log.Error("Cannot remove: " + ((part != null) ? part.ToString() : null));
			}
			this.parts.Remove(part);
		}

		// Token: 0x0600E225 RID: 57893 RVA: 0x0042422C File Offset: 0x0042242C
		public bool CanReorder(ScenPart part, ReorderDirection dir)
		{
			if (!part.def.PlayerAddRemovable)
			{
				return false;
			}
			int num = this.parts.IndexOf(part);
			if (dir == ReorderDirection.Up)
			{
				return num != 0 && (num <= 0 || this.parts[num - 1].def.PlayerAddRemovable);
			}
			if (dir == ReorderDirection.Down)
			{
				return num != this.parts.Count - 1;
			}
			throw new NotImplementedException();
		}

		// Token: 0x0600E226 RID: 57894 RVA: 0x0042429C File Offset: 0x0042249C
		public void Reorder(ScenPart part, ReorderDirection dir)
		{
			int num = this.parts.IndexOf(part);
			this.parts.RemoveAt(num);
			if (dir == ReorderDirection.Up)
			{
				this.parts.Insert(num - 1, part);
			}
			if (dir == ReorderDirection.Down)
			{
				this.parts.Insert(num + 1, part);
			}
		}

		// Token: 0x0600E227 RID: 57895 RVA: 0x004242E8 File Offset: 0x004224E8
		public bool CanToUploadToWorkshop()
		{
			return this.Category != ScenarioCategory.FromDef && this.TryUploadReport().Accepted && !this.GetWorkshopItemHook().MayHaveAuthorNotCurrentUser;
		}

		// Token: 0x0600E228 RID: 57896 RVA: 0x00424324 File Offset: 0x00422524
		public void PrepareForWorkshopUpload()
		{
			string path = this.name + Rand.RangeInclusive(100, 999).ToString();
			this.tempUploadDir = Path.Combine(GenFilePaths.TempFolderPath, path);
			DirectoryInfo directoryInfo = new DirectoryInfo(this.tempUploadDir);
			if (directoryInfo.Exists)
			{
				directoryInfo.Delete();
			}
			directoryInfo.Create();
			string text = Path.Combine(this.tempUploadDir, this.name);
			text += ".rsc";
			GameDataSaveLoader.SaveScenario(this, text);
		}

		// Token: 0x0600E229 RID: 57897 RVA: 0x004243A8 File Offset: 0x004225A8
		public AcceptanceReport TryUploadReport()
		{
			if (this.name == null || this.name.Length < 3 || this.summary == null || this.summary.Length < 3 || this.description == null || this.description.Length < 3)
			{
				return "TextFieldsMustBeFilled".TranslateSimple();
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x0600E22A RID: 57898 RVA: 0x0042440C File Offset: 0x0042260C
		public PublishedFileId_t GetPublishedFileId()
		{
			return this.publishedFileIdInt;
		}

		// Token: 0x0600E22B RID: 57899 RVA: 0x00424414 File Offset: 0x00422614
		public void SetPublishedFileId(PublishedFileId_t newPfid)
		{
			this.publishedFileIdInt = newPfid;
			if (this.Category == ScenarioCategory.CustomLocal && !this.fileName.NullOrEmpty())
			{
				GameDataSaveLoader.SaveScenario(this, GenFilePaths.AbsPathForScenario(this.fileName));
			}
		}

		// Token: 0x0600E22C RID: 57900 RVA: 0x00424444 File Offset: 0x00422644
		public string GetWorkshopName()
		{
			return this.name;
		}

		// Token: 0x0600E22D RID: 57901 RVA: 0x0042444C File Offset: 0x0042264C
		public string GetWorkshopDescription()
		{
			return this.GetFullInformationText();
		}

		// Token: 0x0600E22E RID: 57902 RVA: 0x00424454 File Offset: 0x00422654
		public string GetWorkshopPreviewImagePath()
		{
			return GenFilePaths.ScenarioPreviewImagePath;
		}

		// Token: 0x0600E22F RID: 57903 RVA: 0x0042445B File Offset: 0x0042265B
		public IList<string> GetWorkshopTags()
		{
			return new List<string>
			{
				"Scenario"
			};
		}

		// Token: 0x0600E230 RID: 57904 RVA: 0x0042446D File Offset: 0x0042266D
		public DirectoryInfo GetWorkshopUploadDirectory()
		{
			return new DirectoryInfo(this.tempUploadDir);
		}

		// Token: 0x0600E231 RID: 57905 RVA: 0x0042447A File Offset: 0x0042267A
		public WorkshopItemHook GetWorkshopItemHook()
		{
			if (this.workshopHookInt == null)
			{
				this.workshopHookInt = new WorkshopItemHook(this);
			}
			return this.workshopHookInt;
		}

		// Token: 0x0600E232 RID: 57906 RVA: 0x00424496 File Offset: 0x00422696
		public override string ToString()
		{
			if (this.name.NullOrEmpty())
			{
				return "LabellessScenario";
			}
			return this.name;
		}

		// Token: 0x0600E233 RID: 57907 RVA: 0x004244B4 File Offset: 0x004226B4
		public override int GetHashCode()
		{
			int num = 6126121;
			if (this.name != null)
			{
				num ^= this.name.GetHashCode();
			}
			if (this.summary != null)
			{
				num ^= this.summary.GetHashCode();
			}
			if (this.description != null)
			{
				num ^= this.description.GetHashCode();
			}
			num ^= this.publishedFileIdInt.GetHashCode();
			for (int i = 0; i < this.parts.Count; i++)
			{
				if (this.parts[i] != null)
				{
					num ^= this.parts[i].GetHashCode() + i;
				}
			}
			return num;
		}

		// Token: 0x040097BA RID: 38842
		[MustTranslate]
		public string name;

		// Token: 0x040097BB RID: 38843
		[MustTranslate]
		public string summary;

		// Token: 0x040097BC RID: 38844
		[MustTranslate]
		public string description;

		// Token: 0x040097BD RID: 38845
		internal ScenPart_PlayerFaction playerFaction;

		// Token: 0x040097BE RID: 38846
		internal ScenPart_PlanetLayer surfaceLayer;

		// Token: 0x040097BF RID: 38847
		internal List<ScenPart> parts = new List<ScenPart>();

		// Token: 0x040097C0 RID: 38848
		private PublishedFileId_t publishedFileIdInt = PublishedFileId_t.Invalid;

		// Token: 0x040097C1 RID: 38849
		private ScenarioCategory categoryInt;

		// Token: 0x040097C2 RID: 38850
		[NoTranslate]
		public string fileName;

		// Token: 0x040097C3 RID: 38851
		private WorkshopItemHook workshopHookInt;

		// Token: 0x040097C4 RID: 38852
		[NoTranslate]
		private string tempUploadDir;

		// Token: 0x040097C5 RID: 38853
		public bool enabled = true;

		// Token: 0x040097C6 RID: 38854
		public bool showInUI = true;

		// Token: 0x040097C7 RID: 38855
		public bool valid = true;

		// Token: 0x040097C8 RID: 38856
		public bool standardAnomalyPlaystyleOnly;

		// Token: 0x040097C9 RID: 38857
		public const int NameMaxLength = 55;

		// Token: 0x040097CA RID: 38858
		public const int SummaryMaxLength = 300;

		// Token: 0x040097CB RID: 38859
		public const int DescriptionMaxLength = 1000;
	}
}
