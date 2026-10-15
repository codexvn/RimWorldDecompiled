using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld;

public class QuestScriptDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__61 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestScriptDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

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
		public _003CConfigErrors_003Ed__61(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				QuestScriptDef questScriptDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)questScriptDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007a;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007a;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00d8;
				case 3:
					_003C_003E1__state = -1;
					goto IL_010f;
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_007a:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (questScriptDef.rootSelectionWeight > 0f && !questScriptDef.autoAccept && questScriptDef.expireDaysRange.TrueMax <= 0f)
					{
						_003C_003E2__current = "rootSelectionWeight > 0 but expireDaysRange not set";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00d8;
					IL_010f:
					if (questScriptDef.defaultChallengeRating > 0 && !questScriptDef.IsRootAny)
					{
						_003C_003E2__current = "non-root quest has defaultChallengeRating";
						_003C_003E1__state = 4;
						return true;
					}
					break;
					IL_00d8:
					if (questScriptDef.autoAccept && questScriptDef.expireDaysRange.TrueMax > 0f)
					{
						_003C_003E2__current = "autoAccept but there is an expireDaysRange set";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_010f;
				}
				return false;
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__61 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__61(0)
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

	public QuestNode root;

	public float rootSelectionWeight;

	public SimpleCurve rootSelectionWeightFactorFromPointsCurve;

	public bool randomlySelectable = true;

	public float rootMinPoints;

	public float rootMinProgressScore;

	public int rootEarliestDay;

	public bool rootIncreasesPopulation;

	public float minRefireDays;

	public float decreeSelectionWeight;

	public List<string> decreeTags;

	public RulePack questDescriptionRules;

	public RulePack questNameRules;

	public RulePack questDescriptionAndNameRules;

	public RulePack questContentRules;

	public RulePack questSubjectRules;

	public bool autoAccept;

	public bool hideOnCleanup;

	public FloatRange expireDaysRange = new FloatRange(-1f, -1f);

	public bool nameMustBeUnique;

	public int defaultChallengeRating = -1;

	public bool defaultHidden;

	public bool isRootSpecial;

	public bool canGiveRoyalFavor;

	public string questAvailableLetterLabel;

	public LetterDef questAvailableLetterDef;

	public bool questAvailableLetterTextIsDescription;

	public bool hideInvolvedFactionsInfo;

	public bool affectedByPopulation;

	public bool affectedByPoints = true;

	public bool defaultCharity;

	public HistoryEventDef successHistoryEvent;

	public HistoryEventDef failedOrExpiredHistoryEvent;

	public bool sendAvailableLetter = true;

	public bool epic;

	public QuestScriptDef epicParent;

	public bool endOnColonyMove = true;

	public bool everAcceptableInSpace;

	public bool neverPossibleInSpace;

	public List<QuestGiverTag> givenBy = new List<QuestGiverTag>();

	public List<PlanetLayerDef> layerWhitelist;

	public List<PlanetLayerDef> layerBlacklist;

	public bool canOccurOnAllPlanetLayers;

	[Unsaved(false)]
	private int lastCheckCanRunTick;

	[Unsaved(false)]
	private int lastCheckCanRunPoints;

	[Unsaved(false)]
	private bool lastCanRunResult;

	public bool IsRootRandomSelected
	{
		get
		{
			if (rootSelectionWeight != 0f)
			{
				return randomlySelectable;
			}
			return false;
		}
	}

	public bool IsRootDecree => decreeSelectionWeight != 0f;

	public bool IsRootAny
	{
		get
		{
			if (!IsRootRandomSelected && !IsRootDecree)
			{
				return isRootSpecial;
			}
			return true;
		}
	}

	public bool IsEpic => epic;

	public void Run()
	{
		InitializeRules();
		root.Run();
	}

	public void InitializeRules()
	{
		if (questDescriptionRules != null)
		{
			RimWorld.QuestGen.QuestGen.AddQuestDescriptionRules(questDescriptionRules);
		}
		if (questNameRules != null)
		{
			RimWorld.QuestGen.QuestGen.AddQuestNameRules(questNameRules);
		}
		if (questDescriptionAndNameRules != null)
		{
			RimWorld.QuestGen.QuestGen.AddQuestDescriptionRules(questDescriptionAndNameRules);
			RimWorld.QuestGen.QuestGen.AddQuestNameRules(questDescriptionAndNameRules);
		}
		if (questContentRules != null)
		{
			RimWorld.QuestGen.QuestGen.AddQuestContentRules(questContentRules);
		}
	}

	public bool CanRun(Slate slate, IIncidentTarget target)
	{
		using (new ProfilerBlock("CanRun()"))
		{
			if (!slate.TryGet<int>("points", out var var))
			{
				var = 0;
			}
			if (lastCheckCanRunTick == Find.TickManager.TicksGame && lastCheckCanRunPoints == var)
			{
				return lastCanRunResult;
			}
			lastCheckCanRunTick = Find.TickManager.TicksGame;
			lastCheckCanRunPoints = var;
			try
			{
				lastCanRunResult = target != null && CanQuestOccurOnTile(target.Tile) && root.TestRun(slate.DeepCopy());
				Scenario scenario = Find.Scenario;
				for (int i = 0; i < scenario.parts.Count; i++)
				{
					if (!lastCanRunResult)
					{
						break;
					}
					if (scenario.parts[i] is ScenPart_DisableQuest scenPart_DisableQuest && scenPart_DisableQuest.questDef == this)
					{
						lastCanRunResult = false;
					}
				}
			}
			catch (Exception value)
			{
				lastCanRunResult = false;
				Log.Error($"Error while checking if can generate quest: {value}");
				throw;
			}
			return lastCanRunResult;
		}
	}

	private bool CanQuestOccurOnTile(PlanetTile tile)
	{
		if (!tile.Valid)
		{
			return true;
		}
		PlanetLayerDef layerDef = tile.LayerDef;
		if (!layerWhitelist.NullOrEmpty() && !layerWhitelist.Contains(layerDef))
		{
			return false;
		}
		if (!layerBlacklist.NullOrEmpty() && layerBlacklist.Contains(layerDef))
		{
			return false;
		}
		if (!autoAccept && !everAcceptableInSpace && layerDef.isSpace)
		{
			return false;
		}
		if (neverPossibleInSpace && layerDef.isSpace)
		{
			return false;
		}
		return true;
	}

	public MapParent TryFindNewSuitableMapParentForRetarget()
	{
		foreach (Map playerHomeMap in Current.Game.PlayerHomeMaps)
		{
			if (CanQuestOccurOnTile(playerHomeMap.Tile))
			{
				return playerHomeMap.Parent;
			}
		}
		foreach (Map map in Current.Game.Maps)
		{
			if (CanQuestOccurOnTile(map.Tile) && GravshipUtility.GetPlayerGravEngine_NewTemp(map) != null)
			{
				return map.Parent;
			}
		}
		return null;
	}

	public bool IsParentSuitableForQuest(MapParent mapParent)
	{
		return CanQuestOccurOnTile(mapParent.Tile);
	}

	public bool CanRun(float points, IIncidentTarget target)
	{
		Slate slate = new Slate();
		slate.Set("points", points);
		return CanRun(slate, target);
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__61))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__61(-2)
		{
			_003C_003E4__this = this
		};
	}
}
