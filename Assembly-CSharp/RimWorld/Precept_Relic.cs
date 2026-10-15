using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Precept_Relic : Precept_ThingStyle
{
	[CompilerGenerated]
	private sealed class _003CEditFloatMenuOptions_003Ed__27 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Precept_Relic _003C_003E4__this;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CEditFloatMenuOptions_003Ed__27(int _003C_003E1__state)
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
			Precept_Relic precept_Relic = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = precept_Relic.EditFloatMenuOption();
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CEditFloatMenuOptions_003Ed__27 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CEditFloatMenuOptions_003Ed__27(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetQuestsToDeactivateIfLost_003Ed__35 : IEnumerable<Quest>, IEnumerable, IEnumerator<Quest>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Quest _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Precept_Relic _003C_003E4__this;

		private List<Quest> _003CallQuests_003E5__2;

		private int _003Ci_003E5__3;

		Quest IEnumerator<Quest>.Current
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
		public _003CGetQuestsToDeactivateIfLost_003Ed__35(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CallQuests_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Precept_Relic precept_Relic = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_00c2;
			}
			_003C_003E1__state = -1;
			_003CallQuests_003E5__2 = Find.QuestManager.QuestsListForReading;
			_003Ci_003E5__3 = 0;
			goto IL_00d4;
			IL_00c2:
			_003Ci_003E5__3++;
			goto IL_00d4;
			IL_00d4:
			if (_003Ci_003E5__3 < _003CallQuests_003E5__2.Count)
			{
				if (!_003CallQuests_003E5__2[_003Ci_003E5__3].Historical)
				{
					List<QuestPart> partsListForReading = _003CallQuests_003E5__2[_003Ci_003E5__3].PartsListForReading;
					for (int i = 0; i < partsListForReading.Count; i++)
					{
						if (partsListForReading[i] is QuestPart_SubquestGenerator_RelicHunt questPart_SubquestGenerator_RelicHunt && questPart_SubquestGenerator_RelicHunt.relic == precept_Relic)
						{
							_003C_003E2__current = _003CallQuests_003E5__2[_003Ci_003E5__3];
							_003C_003E1__state = 1;
							return true;
						}
					}
				}
				goto IL_00c2;
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
		IEnumerator<Quest> IEnumerable<Quest>.GetEnumerator()
		{
			_003CGetQuestsToDeactivateIfLost_003Ed__35 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetQuestsToDeactivateIfLost_003Ed__35(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Quest>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__32 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Precept_Relic _003C_003E4__this;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CSpecialDisplayStats_003Ed__32(int _003C_003E1__state)
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
			Precept_Relic precept_Relic = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (precept_Relic.ideo.classicMode)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Stat_Thing_RelicStatus".Translate(), "Relic".Translate().CapitalizeFirst(), "Stat_Thing_RelicStatus_Desc".Translate(), 1109);
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Stat_Thing_RelicOf_Name".Translate(), precept_Relic.ideo.name, "Stat_Thing_RelicOf_Desc".Translate(), 1109, null, new Dialog_InfoCard.Hyperlink[1]
				{
					new Dialog_InfoCard.Hyperlink(precept_Relic.ideo)
				});
				_003C_003E1__state = 2;
				return true;
			case 1:
				_003C_003E1__state = -1;
				break;
			case 2:
				_003C_003E1__state = -1;
				break;
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__32 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__32(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	private static List<ThingDef> usedThingsTmp = new List<ThingDef>();

	public ThingDef stuff;

	private bool relicGenerated;

	private Thing generatedRelic;

	private string tipLabelCached;

	public override string TipLabel
	{
		get
		{
			if (tipLabelCached == null)
			{
				tipLabelCached = base.LabelCap + "\n" + base.ThingDef.LabelCap;
			}
			return tipLabelCached;
		}
	}

	public override bool CanRegenerate => false;

	public bool CanGenerateRelic => !relicGenerated;

	public Thing GeneratedRelic => generatedRelic;

	protected override string ThingLabelCap => Find.ActiveLanguageWorker.PostProcessThingLabelForRelic(base.ThingLabelCap);

	public override string DescriptionForTip => base.ThingDef?.description ?? base.Description;

	public bool RelicInPlayerPossession
	{
		get
		{
			Thing thing = GeneratedRelic;
			if (thing == null || thing.Destroyed || !thing.EverSeenByPlayer || (thing.MapHeld == null && !ThingOwnerUtility.AnyParentIs<Caravan>(thing) && !ThingOwnerUtility.AnyParentIs<TravellingTransporters>(thing)))
			{
				return false;
			}
			return true;
		}
	}

	protected override string NameRootSymbol => "r_ideoRelicName";

	public static ThingDef GenerateStuffFor(ThingDef thing, Ideo ideo = null)
	{
		IEnumerable<ThingDef> source = GenStuff.AllowedStuffsFor(thing);
		if (ideo != null)
		{
			IEnumerable<ThingDef> alreadyUsedStuffs = (from p in ideo.PreceptsListForReading
				where p is Precept_Relic precept_Relic && precept_Relic.ThingDef == thing
				select ((Precept_Relic)p).stuff into p
				where p != null
				select p).Distinct();
			source = source.Where((ThingDef stuff) => !alreadyUsedStuffs.Contains(stuff));
		}
		return source.RandomElementByWeight((ThingDef stuff) => stuff.BaseMarketValue);
	}

	public void SetRandomStuff()
	{
		if (base.ThingDef.MadeFromStuff)
		{
			stuff = GenerateStuffFor(base.ThingDef, ideo);
		}
		else
		{
			stuff = null;
		}
	}

	protected override void Notify_ThingDefSet()
	{
		base.Notify_ThingDefSet();
		SetRandomStuff();
	}

	public override string GenerateNameRaw()
	{
		CompProperties_GeneratedName compProperties = base.ThingDef.GetCompProperties<CompProperties_GeneratedName>();
		if (compProperties != null)
		{
			return CompGeneratedNames.GenerateName(compProperties);
		}
		return base.GenerateNameRaw();
	}

	public Thing GenerateRelic()
	{
		if (stuff == null && base.ThingDef.MadeFromStuff)
		{
			Log.Warning("Tried generating relic with stuff, but precept had no stuff set? If this is an old savegame from testing this warning is to be expected.");
			stuff = GenerateStuffFor(base.ThingDef, ideo);
		}
		Thing thing = ((base.ThingDef.CompDefFor<CompQuality>() == null) ? ThingMaker.MakeThing(base.ThingDef, stuff) : new ThingStuffPairWithQuality(base.ThingDef, stuff, QualityCategory.Legendary).MakeThing(forceQuality: true));
		thing.StyleSourcePrecept = this;
		relicGenerated = true;
		generatedRelic = thing;
		return thing;
	}

	public override void ClearTipCache()
	{
		base.ClearTipCache();
		tipLabelCached = null;
	}

	[IteratorStateMachine(typeof(_003CEditFloatMenuOptions_003Ed__27))]
	public override IEnumerable<FloatMenuOption> EditFloatMenuOptions()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CEditFloatMenuOptions_003Ed__27(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string GetTip()
	{
		return base.GetTip() + "\n\n" + (ideo.classicMode ? "RelicTipClassic" : "RelicTip").Translate().Colorize(Color.grey);
	}

	public override void DrawIcon(Rect rect)
	{
		Widgets.DefIcon(rect, base.ThingDef, stuff, 1f, ideo.GetStyleFor(base.ThingDef), drawPlaceholder: false, null, null, null);
	}

	public override void Notify_ThingLost(Thing thing, bool destroyed = false)
	{
		if (thing.def != base.ThingDef || QuestPart_NewColony.IsGeneratingNewColony)
		{
			return;
		}
		Find.HistoryEventsManager.RecordEvent(new HistoryEvent(destroyed ? HistoryEventDefOf.RelicDestroyed : HistoryEventDefOf.RelicLost, thing.Named("SUBJECT")));
		List<Faction> list = new List<Faction>();
		HashSet<Pawn> hashSet = new HashSet<Pawn>();
		if (thing.EverSeenByPlayer)
		{
			list.Add(Faction.OfPlayer);
		}
		if (thing.MapHeld != null)
		{
			foreach (Pawn item in thing.MapHeld.mapPawns.AllPawnsSpawned)
			{
				if (!item.Dead && item.Faction != null && !list.Contains(item.Faction))
				{
					list.Add(item.Faction);
				}
			}
		}
		foreach (Pawn item2 in PawnsFinder.AllMapsAndWorld_Alive)
		{
			if (item2.needs != null && item2.needs.mood != null && item2.needs.mood.thoughts != null && item2.needs.mood.thoughts.memories != null && !hashSet.Contains(item2) && list.Contains(item2.Faction))
			{
				item2.needs.mood.thoughts.memories.TryGainMemory(destroyed ? ThoughtDefOf.RelicDestroyed : ThoughtDefOf.RelicLost);
				if (item2.Faction == Faction.OfPlayer)
				{
					hashSet.Add(item2);
				}
			}
		}
		if (list.Contains(Faction.OfPlayer) && (destroyed || thing.MapHeld == null || !thing.MapHeld.IsPlayerHome))
		{
			TaggedString label = (destroyed ? "LetterLabelRelicDestroyed" : "LetterLabelRelicLost").Translate() + ": " + base.LabelCap;
			TaggedString text = (destroyed ? "LetterTextRelicDestroyed" : "LetterTextRelicLost").Translate(base.LabelCap, hashSet.Select((Pawn p) => p.LabelNoCountColored.Resolve()).ToList().ToLineList("- "));
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NegativeEvent, new LookTargets(hashSet.Where((Pawn p) => p.Faction == Faction.OfPlayer)));
		}
	}

	public void Notify_NewColonyStarted()
	{
		if (relicGenerated && !RelicInPlayerPossession)
		{
			relicGenerated = false;
		}
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__32))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats(Thing thing)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__32(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string TransformThingLabel(string label)
	{
		return name + ", " + "Relic".Translate();
	}

	public override string InspectStringExtra(Thing thing)
	{
		if (ideo.classicMode)
		{
			return "Relic".Translate().CapitalizeFirst();
		}
		return "RelicOf".Translate(ideo.Named("IDEO")).Resolve();
	}

	[IteratorStateMachine(typeof(_003CGetQuestsToDeactivateIfLost_003Ed__35))]
	private IEnumerable<Quest> GetQuestsToDeactivateIfLost()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetQuestsToDeactivateIfLost_003Ed__35(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override bool TryGetLostByReformingWarning(out string warning)
	{
		warning = null;
		IEnumerable<Quest> questsToDeactivateIfLost = GetQuestsToDeactivateIfLost();
		if (questsToDeactivateIfLost.Any())
		{
			foreach (Quest item in questsToDeactivateIfLost)
			{
				if (!warning.NullOrEmpty())
				{
					warning += "\n\n";
				}
				warning += "ReformIdeoRelicHuntQuestEnd".Translate(base.LabelCap, item.name);
			}
			return true;
		}
		return false;
	}

	public override void Notify_RemovedByReforming()
	{
		foreach (Quest item in GetQuestsToDeactivateIfLost())
		{
			item.hidden = true;
			item.End(QuestEndOutcome.Unknown, sendLetter: false);
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Defs.Look(ref stuff, "stuff");
		if (!GameDataSaveLoader.IsSavingOrLoadingExternalIdeo)
		{
			if (Scribe.mode == LoadSaveMode.Saving && relicGenerated && generatedRelic != null && (generatedRelic.Destroyed || generatedRelic.Discarded))
			{
				relicGenerated = false;
				generatedRelic = null;
			}
			Scribe_Values.Look(ref relicGenerated, "everGenerated", defaultValue: false);
			Scribe_References.Look(ref generatedRelic, "generatedRelic");
		}
	}

	public override void CopyTo(Precept other)
	{
		base.CopyTo(other);
		Precept_Relic obj = (Precept_Relic)other;
		obj.stuff = stuff;
		obj.relicGenerated = relicGenerated;
		obj.generatedRelic = generatedRelic;
	}

	public override string ToString()
	{
		return "Relic - " + base.LabelCap;
	}
}
