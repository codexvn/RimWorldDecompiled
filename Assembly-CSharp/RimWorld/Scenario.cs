using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Steamworks;
using Verse;
using Verse.Steam;

namespace RimWorld;

public class Scenario : IExposable, WorkshopUploadable
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__31 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Scenario _003C_003E4__this;

		private IEnumerator<ScenPart> _003C_003E7__wrap1;

		private IEnumerator<string> _003C_003E7__wrap2;

		string IEnumerator<string>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CConfigErrors_003Ed__31(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || (uint)(num - 6) <= 1u)
			{
				try
				{
					if (num == -4 || num == 7)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Scenario scenario = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (scenario.name.NullOrEmpty())
					{
						_003C_003E2__current = "no title";
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_006f;
				case 1:
					_003C_003E1__state = -1;
					goto IL_006f;
				case 2:
					_003C_003E1__state = -1;
					goto IL_009c;
				case 3:
					_003C_003E1__state = -1;
					goto IL_00c4;
				case 4:
					_003C_003E1__state = -1;
					goto IL_0144;
				case 5:
					_003C_003E1__state = -1;
					goto IL_0144;
				case 6:
					_003C_003E1__state = -3;
					goto IL_01f1;
				case 7:
					{
						_003C_003E1__state = -4;
						goto IL_01d7;
					}
					IL_01d7:
					if (_003C_003E7__wrap2.MoveNext())
					{
						string current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 7;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					goto IL_01f1;
					IL_006f:
					if (scenario.parts.NullOrEmpty())
					{
						_003C_003E2__current = "no parts";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_009c;
					IL_009c:
					if (scenario.playerFaction == null)
					{
						_003C_003E2__current = "no playerFaction";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_00c4;
					IL_0144:
					_003C_003E7__wrap1 = scenario.AllParts.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_01f1;
					IL_00c4:
					if (scenario.surfaceLayer == null)
					{
						_003C_003E2__current = "no surfaceLayer";
						_003C_003E1__state = 4;
						return true;
					}
					if (!scenario.surfaceLayer.layer.layerType.SameOrSubclassOf<SurfaceLayer>())
					{
						_003C_003E2__current = "surfaceLayer layer type " + scenario.surfaceLayer.layer.layerType.Name + " is not the same or a subclass of SurfaceLayer";
						_003C_003E1__state = 5;
						return true;
					}
					goto IL_0144;
					IL_01f1:
					if (_003C_003E7__wrap1.MoveNext())
					{
						ScenPart current2 = _003C_003E7__wrap1.Current;
						if (current2 == null)
						{
							_003C_003E2__current = "scenario has null part";
							_003C_003E1__state = 6;
							return true;
						}
						_003C_003E7__wrap2 = current2.ConfigErrors().GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_01d7;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					return false;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__31 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__31(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_AllParts_003Ed__23 : IEnumerable<ScenPart>, IEnumerable, IEnumerator<ScenPart>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ScenPart _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Scenario _003C_003E4__this;

		private int _003Ci_003E5__2;

		ScenPart IEnumerator<ScenPart>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003Cget_AllParts_003Ed__23(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Scenario scenario = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = scenario.playerFaction;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = scenario.surfaceLayer;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003Ci_003E5__2 = 0;
				break;
			case 3:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				break;
			}
			if (_003Ci_003E5__2 < scenario.parts.Count)
			{
				_003C_003E2__current = scenario.parts[_003Ci_003E5__2];
				_003C_003E1__state = 3;
				return true;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<ScenPart> IEnumerable<ScenPart>.GetEnumerator()
		{
			_003Cget_AllParts_003Ed__23 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_AllParts_003Ed__23(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ScenPart>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_SupportedVersions_003Ed__16 : IEnumerable<System.Version>, IEnumerable, IEnumerator<System.Version>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private System.Version _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		System.Version IEnumerator<System.Version>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003Cget_SupportedVersions_003Ed__16(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = new System.Version(VersionControl.CurrentMajor, VersionControl.CurrentMinor);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				return false;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<System.Version> IEnumerable<System.Version>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003Cget_SupportedVersions_003Ed__16(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<System.Version>)this).GetEnumerator();
		}
	}

	[MustTranslate]
	public string name;

	[MustTranslate]
	public string summary;

	[MustTranslate]
	public string description;

	internal ScenPart_PlayerFaction playerFaction;

	internal ScenPart_PlanetLayer surfaceLayer;

	internal List<ScenPart> parts = new List<ScenPart>();

	private PublishedFileId_t publishedFileIdInt = PublishedFileId_t.Invalid;

	private ScenarioCategory categoryInt;

	[NoTranslate]
	public string fileName;

	private WorkshopItemHook workshopHookInt;

	[NoTranslate]
	private string tempUploadDir;

	public bool enabled = true;

	public bool showInUI = true;

	public bool valid = true;

	public bool standardAnomalyPlaystyleOnly;

	public const int NameMaxLength = 55;

	public const int SummaryMaxLength = 300;

	public const int DescriptionMaxLength = 1000;

	public IEnumerable<System.Version> SupportedVersions
	{
		[IteratorStateMachine(typeof(_003Cget_SupportedVersions_003Ed__16))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_SupportedVersions_003Ed__16(-2);
		}
	}

	public FileInfo File => new FileInfo(GenFilePaths.AbsPathForScenario(fileName));

	public IEnumerable<ScenPart> AllParts
	{
		[IteratorStateMachine(typeof(_003Cget_AllParts_003Ed__23))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_AllParts_003Ed__23(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public ScenarioCategory Category
	{
		get
		{
			if (categoryInt == ScenarioCategory.Undefined)
			{
				Log.Error("Category is Undefined on Scenario " + this);
			}
			return categoryInt;
		}
		set
		{
			categoryInt = value;
		}
	}

	public bool OverrideDangerMusic
	{
		get
		{
			foreach (ScenPart allPart in AllParts)
			{
				if (allPart.OverrideDangerMusic)
				{
					return true;
				}
			}
			return false;
		}
	}

	public void ExposeData()
	{
		Scribe_Values.Look(ref name, "name");
		Scribe_Values.Look(ref summary, "summary");
		Scribe_Values.Look(ref description, "description");
		Scribe_Values.Look(ref publishedFileIdInt, "publishedFileId", PublishedFileId_t.Invalid);
		Scribe_Deep.Look(ref playerFaction, "playerFaction");
		Scribe_Deep.Look(ref surfaceLayer, "surfaceLayer");
		Scribe_Collections.Look(ref parts, "parts", LookMode.Deep);
		if (Scribe.mode == LoadSaveMode.LoadingVars)
		{
			if (surfaceLayer == null)
			{
				surfaceLayer = new ScenPart_PlanetLayer
				{
					def = ScenPartDefOf.PlanetLayerFixed,
					layer = PlanetLayerDefOf.Surface,
					settingsDef = PlanetLayerSettingsDefOf.Surface,
					hide = true,
					tag = "Surface"
				};
			}
			if (ModsConfig.OdysseyActive && !parts.Any((ScenPart p) => p is ScenPart_PlanetLayer scenPart_PlanetLayer && scenPart_PlanetLayer.tag == "Orbit"))
			{
				ScenPart_PlanetLayer scenPart_PlanetLayer2 = new ScenPart_PlanetLayer
				{
					def = ScenPartDefOf.PlanetLayerFixed,
					layer = PlanetLayerDefOf.Orbit,
					settingsDef = PlanetLayerSettingsDefOf.Orbit,
					hide = true,
					tag = "Orbit"
				};
				parts.Add(scenPart_PlanetLayer2);
				surfaceLayer.connections.Add(new LayerConnection
				{
					tag = scenPart_PlanetLayer2.tag,
					zoomMode = LayerConnection.ZoomMode.ZoomOut
				});
				scenPart_PlanetLayer2.connections.Add(new LayerConnection
				{
					tag = surfaceLayer.tag,
					zoomMode = LayerConnection.ZoomMode.ZoomIn
				});
			}
		}
		if (Scribe.mode == LoadSaveMode.PostLoadInit && parts.RemoveAll((ScenPart p) => p == null || !p.Valid()) != 0)
		{
			Log.Warning("Some scenario parts were null or invalid after loading");
		}
		BackCompatibility.PostExposeData(this);
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__31))]
	public IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__31(-2)
		{
			_003C_003E4__this = this
		};
	}

	public string GetFullInformationText()
	{
		try
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(description);
			stringBuilder.AppendLine();
			foreach (ScenPart allPart in AllParts)
			{
				allPart.summarized = false;
			}
			foreach (ScenPart item in from p in AllParts
				orderby p.def.summaryPriority descending, p.def.defName
				where p.visible
				select p)
			{
				string text = item.Summary(this);
				if (!text.NullOrEmpty())
				{
					stringBuilder.AppendLine(text);
				}
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}
		catch (Exception ex)
		{
			Log.ErrorOnce("Exception in Scenario.GetFullInformationText():\n" + ex.ToString(), 10395878);
			return "Cannot read data.";
		}
	}

	public string GetSummary()
	{
		return summary;
	}

	public Scenario CopyForEditing()
	{
		Scenario scenario = new Scenario();
		scenario.name = name;
		scenario.summary = summary;
		scenario.description = description;
		scenario.playerFaction = (ScenPart_PlayerFaction)playerFaction.CopyForEditing();
		scenario.surfaceLayer = (ScenPart_PlanetLayer)surfaceLayer.CopyForEditing();
		scenario.parts.AddRange(parts.Select((ScenPart p) => p.CopyForEditing()));
		scenario.categoryInt = ScenarioCategory.CustomLocal;
		return scenario;
	}

	public void PreConfigure()
	{
		foreach (ScenPart allPart in AllParts)
		{
			allPart.PreConfigure();
		}
	}

	public virtual Page GetFirstConfigPage()
	{
		List<Page> list = new List<Page>();
		list.Add(new Page_SelectStoryteller());
		list.Add(new Page_CreateWorldParams());
		if (!parts.Any((ScenPart p) => p is ScenPart_ForcedMap))
		{
			list.Add(new Page_SelectStartingSite());
		}
		if (ModsConfig.IdeologyActive)
		{
			list.Add(new Page_ChooseIdeoPreset());
		}
		foreach (Page item in parts.SelectMany((ScenPart p) => p.GetConfigPages()))
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
			page2.nextAct = PageUtility.InitGameStart;
		}
		return page;
	}

	public bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
	{
		foreach (ScenPart allPart in AllParts)
		{
			if (!allPart.AllowPlayerStartingPawn(pawn, tryingToRedress, req))
			{
				return false;
			}
		}
		return true;
	}

	public void Notify_NewPawnGenerating(Pawn pawn, PawnGenerationContext context)
	{
		foreach (ScenPart allPart in AllParts)
		{
			allPart.Notify_NewPawnGenerating(pawn, context);
		}
	}

	public void Notify_PawnGenerated(Pawn pawn, PawnGenerationContext context, bool redressed)
	{
		foreach (ScenPart allPart in AllParts)
		{
			allPart.Notify_PawnGenerated(pawn, context, redressed);
		}
	}

	public void Notify_PawnDied(Corpse corpse)
	{
		for (int i = 0; i < parts.Count; i++)
		{
			parts[i].Notify_PawnDied(corpse);
		}
	}

	public void PostWorldGenerate()
	{
		foreach (ScenPart allPart in AllParts)
		{
			allPart.PostWorldGenerate();
		}
	}

	public void PostIdeoChosen()
	{
		foreach (ScenPart allPart in AllParts)
		{
			allPart.PostIdeoChosen();
		}
	}

	public void PreMapGenerate()
	{
		foreach (ScenPart allPart in AllParts)
		{
			allPart.PreMapGenerate();
		}
	}

	public void GenerateIntoMap(Map map)
	{
		foreach (ScenPart allPart in AllParts)
		{
			allPart.GenerateIntoMap(map);
		}
	}

	public void PostMapGenerate(Map map)
	{
		foreach (ScenPart allPart in AllParts)
		{
			allPart.PostMapGenerate(map);
		}
	}

	public void PostGravshipLanded(Map map)
	{
		foreach (ScenPart allPart in AllParts)
		{
			allPart.PostGravshipLanded(map);
		}
	}

	public void MapRemoved(Map map)
	{
		foreach (ScenPart allPart in AllParts)
		{
			allPart.MapRemoved(map);
		}
	}

	public void PostGameStart()
	{
		foreach (ScenPart allPart in AllParts)
		{
			allPart.PostGameStart();
		}
	}

	public float GetStatFactor(StatDef stat)
	{
		float num = 1f;
		for (int i = 0; i < parts.Count; i++)
		{
			if (parts[i] is ScenPart_StatFactor scenPart_StatFactor)
			{
				num *= scenPart_StatFactor.GetStatFactor(stat);
			}
		}
		return num;
	}

	public void TickScenario()
	{
		for (int i = 0; i < parts.Count; i++)
		{
			parts[i].Tick();
		}
	}

	public void RemovePart(ScenPart part)
	{
		if (!parts.Contains(part))
		{
			Log.Error("Cannot remove: " + part);
		}
		parts.Remove(part);
	}

	public bool CanReorder(ScenPart part, ReorderDirection dir)
	{
		if (!part.def.PlayerAddRemovable)
		{
			return false;
		}
		int num = parts.IndexOf(part);
		switch (dir)
		{
		case ReorderDirection.Up:
			if (num == 0)
			{
				return false;
			}
			if (num > 0 && !parts[num - 1].def.PlayerAddRemovable)
			{
				return false;
			}
			return true;
		case ReorderDirection.Down:
			return num != parts.Count - 1;
		default:
			throw new NotImplementedException();
		}
	}

	public void Reorder(ScenPart part, ReorderDirection dir)
	{
		int num = parts.IndexOf(part);
		parts.RemoveAt(num);
		if (dir == ReorderDirection.Up)
		{
			parts.Insert(num - 1, part);
		}
		if (dir == ReorderDirection.Down)
		{
			parts.Insert(num + 1, part);
		}
	}

	public bool CanToUploadToWorkshop()
	{
		if (Category == ScenarioCategory.FromDef)
		{
			return false;
		}
		if (!TryUploadReport().Accepted)
		{
			return false;
		}
		if (GetWorkshopItemHook().MayHaveAuthorNotCurrentUser)
		{
			return false;
		}
		return true;
	}

	public void PrepareForWorkshopUpload()
	{
		string path = name + Rand.RangeInclusive(100, 999);
		tempUploadDir = Path.Combine(GenFilePaths.TempFolderPath, path);
		DirectoryInfo directoryInfo = new DirectoryInfo(tempUploadDir);
		if (directoryInfo.Exists)
		{
			directoryInfo.Delete();
		}
		directoryInfo.Create();
		string text = Path.Combine(tempUploadDir, name);
		text += ".rsc";
		GameDataSaveLoader.SaveScenario(this, text);
	}

	public AcceptanceReport TryUploadReport()
	{
		if (name == null || name.Length < 3 || summary == null || summary.Length < 3 || description == null || description.Length < 3)
		{
			return "TextFieldsMustBeFilled".TranslateSimple();
		}
		return AcceptanceReport.WasAccepted;
	}

	public PublishedFileId_t GetPublishedFileId()
	{
		return publishedFileIdInt;
	}

	public void SetPublishedFileId(PublishedFileId_t newPfid)
	{
		publishedFileIdInt = newPfid;
		if (Category == ScenarioCategory.CustomLocal && !fileName.NullOrEmpty())
		{
			GameDataSaveLoader.SaveScenario(this, GenFilePaths.AbsPathForScenario(fileName));
		}
	}

	public string GetWorkshopName()
	{
		return name;
	}

	public string GetWorkshopDescription()
	{
		return GetFullInformationText();
	}

	public string GetWorkshopPreviewImagePath()
	{
		return GenFilePaths.ScenarioPreviewImagePath;
	}

	public IList<string> GetWorkshopTags()
	{
		return new List<string> { "Scenario" };
	}

	public DirectoryInfo GetWorkshopUploadDirectory()
	{
		return new DirectoryInfo(tempUploadDir);
	}

	public WorkshopItemHook GetWorkshopItemHook()
	{
		if (workshopHookInt == null)
		{
			workshopHookInt = new WorkshopItemHook(this);
		}
		return workshopHookInt;
	}

	public override string ToString()
	{
		if (name.NullOrEmpty())
		{
			return "LabellessScenario";
		}
		return name;
	}

	public override int GetHashCode()
	{
		int num = 6126121;
		if (name != null)
		{
			num ^= name.GetHashCode();
		}
		if (summary != null)
		{
			num ^= summary.GetHashCode();
		}
		if (description != null)
		{
			num ^= description.GetHashCode();
		}
		num ^= publishedFileIdInt.GetHashCode();
		for (int i = 0; i < parts.Count; i++)
		{
			if (parts[i] != null)
			{
				num ^= parts[i].GetHashCode() + i;
			}
		}
		return num;
	}
}
