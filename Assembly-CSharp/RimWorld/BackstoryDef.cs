using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld;

public class BackstoryDef : Def
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass45_0
	{
		public BackstoryTrait forcedTrait;

		internal bool _003CConfigErrors_003Eb__0(TraitDegreeData d)
		{
			return d.degree == forcedTrait.degree;
		}
	}

	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__45 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public BackstoryDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		private List<BackstoryTrait>.Enumerator _003C_003E7__wrap2;

		private List<SkillGain>.Enumerator _003C_003E7__wrap3;

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
		public _003CConfigErrors_003Ed__45(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 6:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -5:
			case 7:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = default(List<BackstoryTrait>.Enumerator);
			_003C_003E7__wrap3 = default(List<SkillGain>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				BackstoryDef backstoryDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)backstoryDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0086;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0086;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00d8;
				case 3:
					_003C_003E1__state = -1;
					goto IL_0110;
				case 4:
					_003C_003E1__state = -1;
					goto IL_014c;
				case 5:
					_003C_003E1__state = -1;
					goto IL_0181;
				case 6:
					_003C_003E1__state = -4;
					goto IL_0258;
				case 7:
					{
						_003C_003E1__state = -5;
						goto IL_0301;
					}
					IL_0086:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (backstoryDef.title.NullOrEmpty())
					{
						_003C_003E2__current = "null title, description is " + backstoryDef.description;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00d8;
					IL_027a:
					if (!Prefs.DevMode)
					{
						break;
					}
					_003C_003E7__wrap3 = backstoryDef.skillGains.GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_0301;
					IL_0181:
					if (backstoryDef.forcedTraits != null)
					{
						_003C_003E7__wrap2 = backstoryDef.forcedTraits.GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_0258;
					}
					goto IL_027a;
					IL_00d8:
					if (backstoryDef.titleShort.NullOrEmpty())
					{
						_003C_003E2__current = "null titleShort, description is " + backstoryDef.description;
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0110;
					IL_0301:
					while (_003C_003E7__wrap3.MoveNext())
					{
						SkillGain current2 = _003C_003E7__wrap3.Current;
						if (current2.skill.IsDisabled(backstoryDef.workDisables, backstoryDef.DisabledWorkTypes))
						{
							_003C_003E2__current = "modifies skill " + current2.skill?.ToString() + " but also disables this skill";
							_003C_003E1__state = 7;
							return true;
						}
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap3 = default(List<SkillGain>.Enumerator);
					break;
					IL_0110:
					if ((backstoryDef.workDisables & WorkTags.Violent) != 0 && backstoryDef.spawnCategories.Contains("Pirate"))
					{
						_003C_003E2__current = "cannot do Violent work but can spawn as a pirate";
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_014c;
					IL_0258:
					while (_003C_003E7__wrap2.MoveNext())
					{
						_003C_003Ec__DisplayClass45_0 CS_0024_003C_003E8__locals4 = new _003C_003Ec__DisplayClass45_0
						{
							forcedTrait = _003C_003E7__wrap2.Current
						};
						if (!CS_0024_003C_003E8__locals4.forcedTrait.def.degreeDatas.Any((TraitDegreeData d) => d.degree == CS_0024_003C_003E8__locals4.forcedTrait.degree))
						{
							_003C_003E2__current = "Backstory " + backstoryDef.title + " has invalid trait " + CS_0024_003C_003E8__locals4.forcedTrait.def.defName + " degree=" + CS_0024_003C_003E8__locals4.forcedTrait.degree;
							_003C_003E1__state = 6;
							return true;
						}
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<BackstoryTrait>.Enumerator);
					goto IL_027a;
					IL_014c:
					if (backstoryDef.shuffleable && backstoryDef.spawnCategories.Count == 0)
					{
						_003C_003E2__current = "no spawn categories";
						_003C_003E1__state = 5;
						return true;
					}
					goto IL_0181;
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__45 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__45(0)
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
	private sealed class _003Cget_DisabledWorkGivers_003Ed__32 : IEnumerable<WorkGiverDef>, IEnumerable, IEnumerator<WorkGiverDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private WorkGiverDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public BackstoryDef _003C_003E4__this;

		private List<WorkGiverDef> _003Clist_003E5__2;

		private int _003Ci_003E5__3;

		WorkGiverDef IEnumerator<WorkGiverDef>.Current
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
		public _003Cget_DisabledWorkGivers_003Ed__32(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Clist_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			BackstoryDef backstoryDef = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0072;
			}
			_003C_003E1__state = -1;
			_003Clist_003E5__2 = DefDatabase<WorkGiverDef>.AllDefsListForReading;
			_003Ci_003E5__3 = 0;
			goto IL_0082;
			IL_0072:
			_003Ci_003E5__3++;
			goto IL_0082;
			IL_0082:
			if (_003Ci_003E5__3 < _003Clist_003E5__2.Count)
			{
				if (!backstoryDef.AllowsWorkGiver(_003Clist_003E5__2[_003Ci_003E5__3]))
				{
					_003C_003E2__current = _003Clist_003E5__2[_003Ci_003E5__3];
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0072;
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
		IEnumerator<WorkGiverDef> IEnumerable<WorkGiverDef>.GetEnumerator()
		{
			_003Cget_DisabledWorkGivers_003Ed__32 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_DisabledWorkGivers_003Ed__32(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<WorkGiverDef>)this).GetEnumerator();
		}
	}

	public const string PlayerColonistCategoryChild = "Child";

	[MayTranslate]
	public string baseDesc;

	[NoTranslate]
	public string identifier;

	public BackstorySlot slot;

	[MustTranslate]
	public string title;

	[MustTranslate]
	public string titleFemale;

	[MustTranslate]
	public string titleShort;

	[MustTranslate]
	public string titleShortFemale;

	public List<SkillGain> skillGains = new List<SkillGain>();

	public WorkTags workDisables;

	public WorkTags requiredWorkTags;

	[NoTranslate]
	public List<string> spawnCategories = new List<string>();

	public BodyTypeDef bodyTypeGlobal;

	public BodyTypeDef bodyTypeFemale;

	public BodyTypeDef bodyTypeMale;

	public List<BackstoryTrait> forcedTraits;

	public List<BackstoryTrait> disallowedTraits;

	public RulePackDef nameMaker;

	public List<PossessionThingDefCountClass> possessions = new List<PossessionThingDefCountClass>();

	public bool shuffleable = true;

	public bool requiresSpawnCategory;

	[Unsaved(false)]
	public string untranslatedTitle;

	[Unsaved(false)]
	public string untranslatedTitleFemale;

	[Unsaved(false)]
	public string untranslatedTitleShort;

	[Unsaved(false)]
	public string untranslatedTitleShortFemale;

	[Unsaved(false)]
	public string untranslatedDesc;

	private List<WorkTypeDef> cachedDisabledWorkTypes;

	private List<string> unlockedMeditationTypesTemp = new List<string>();

	public bool IsPlayerColonyChildBackstory => spawnCategories?.Contains("Child") ?? false;

	public List<WorkTypeDef> DisabledWorkTypes
	{
		get
		{
			if (cachedDisabledWorkTypes == null)
			{
				cachedDisabledWorkTypes = new List<WorkTypeDef>();
				List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (!AllowsWorkType(allDefsListForReading[i]))
					{
						cachedDisabledWorkTypes.Add(allDefsListForReading[i]);
					}
				}
			}
			return cachedDisabledWorkTypes;
		}
	}

	public IEnumerable<WorkGiverDef> DisabledWorkGivers
	{
		[IteratorStateMachine(typeof(_003Cget_DisabledWorkGivers_003Ed__32))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_DisabledWorkGivers_003Ed__32(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override void PostLoad()
	{
		base.PostLoad();
		untranslatedTitle = title;
		untranslatedTitleFemale = titleFemale;
		untranslatedTitleShort = titleShort;
		untranslatedTitleShortFemale = titleShortFemale;
		untranslatedDesc = description;
		if (description.NullOrEmpty() && !baseDesc.NullOrEmpty())
		{
			description = baseDesc;
		}
		description = description.TrimEnd();
		description = description.Replace("\r", "");
	}

	public string TitleFor(Gender g)
	{
		if (g != Gender.Female || titleFemale.NullOrEmpty())
		{
			return title;
		}
		return titleFemale;
	}

	public string TitleCapFor(Gender g)
	{
		return TitleFor(g).CapitalizeFirst();
	}

	public string TitleShortFor(Gender g)
	{
		if (g == Gender.Female && !titleShortFemale.NullOrEmpty())
		{
			return titleShortFemale;
		}
		if (!titleShort.NullOrEmpty())
		{
			return titleShort;
		}
		return TitleFor(g);
	}

	public BodyTypeDef BodyTypeFor(Gender g)
	{
		if (bodyTypeGlobal == null)
		{
			switch (g)
			{
			case Gender.None:
				break;
			case Gender.Female:
				return bodyTypeFemale;
			default:
				return bodyTypeMale;
			}
		}
		return bodyTypeGlobal;
	}

	public TaggedString FullDescriptionFor(Pawn p)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(description.Formatted(p.Named("PAWN")).AdjustedFor(p).Resolve());
		stringBuilder.AppendLine();
		stringBuilder.AppendLine();
		List<SkillDef> allDefsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;
		for (int i = 0; i < allDefsListForReading.Count; i++)
		{
			SkillDef skillDef = allDefsListForReading[i];
			foreach (SkillGain skillGain in skillGains)
			{
				if (skillGain.skill == skillDef)
				{
					stringBuilder.AppendLine(skillDef.skillLabel.CapitalizeFirst() + ":   " + skillGain.amount.ToString("+##;-##"));
					break;
				}
			}
		}
		if (DisabledWorkTypes.Any() || DisabledWorkGivers.Any())
		{
			stringBuilder.AppendLine();
		}
		foreach (WorkTypeDef disabledWorkType in DisabledWorkTypes)
		{
			stringBuilder.AppendLine(disabledWorkType.gerundLabel.CapitalizeFirst() + " " + "DisabledLower".Translate());
		}
		foreach (WorkGiverDef disabledWorkGiver in DisabledWorkGivers)
		{
			stringBuilder.AppendLine(disabledWorkGiver.workType.gerundLabel.CapitalizeFirst() + ": " + disabledWorkGiver.LabelCap + " " + "DisabledLower".Translate());
		}
		if (ModsConfig.RoyaltyActive)
		{
			unlockedMeditationTypesTemp.Clear();
			foreach (MeditationFocusDef allDef in DefDatabase<MeditationFocusDef>.AllDefs)
			{
				for (int j = 0; j < allDef.requiredBackstoriesAny.Count; j++)
				{
					BackstoryCategoryAndSlot backstoryCategoryAndSlot = allDef.requiredBackstoriesAny[j];
					if (spawnCategories.Contains(backstoryCategoryAndSlot.categoryName) && backstoryCategoryAndSlot.slot == slot)
					{
						unlockedMeditationTypesTemp.Add(allDef.LabelCap);
						break;
					}
				}
			}
			if (unlockedMeditationTypesTemp.Count > 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("MeditationFocusesUnlocked".Translate() + ": ");
				stringBuilder.AppendLine(unlockedMeditationTypesTemp.ToLineList("  - "));
			}
		}
		if (!modContentPack.IsOfficialMod)
		{
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Source".Translate() + ": " + modContentPack.Name);
		}
		string str = stringBuilder.ToString().TrimEndNewlines();
		return Find.ActiveLanguageWorker.PostProcessed(str);
	}

	public void SetTitle(string newTitle, string newTitleFemale)
	{
		title = newTitle;
		titleFemale = newTitleFemale;
	}

	public void SetTitleShort(string newTitleShort, string newTitleShortFemale)
	{
		titleShort = newTitleShort;
		titleShortFemale = newTitleShortFemale;
	}

	public bool DisallowsTrait(TraitDef def, int degree)
	{
		if (disallowedTraits == null)
		{
			return false;
		}
		for (int i = 0; i < disallowedTraits.Count; i++)
		{
			if (disallowedTraits[i].def == def && disallowedTraits[i].degree == degree)
			{
				return true;
			}
		}
		return false;
	}

	private bool AllowsWorkType(WorkTypeDef workType)
	{
		return (workDisables & workType.workTags) == 0;
	}

	private bool AllowsWorkGiver(WorkGiverDef workGiver)
	{
		return (workDisables & workGiver.workTags) == 0;
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__45))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__45(-2)
		{
			_003C_003E4__this = this
		};
	}
}
