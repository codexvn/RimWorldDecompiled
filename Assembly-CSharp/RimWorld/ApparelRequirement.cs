using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class ApparelRequirement : IExposable
{
	[CompilerGenerated]
	private sealed class _003CAllAllowedApparelForPawn_003Ed__12 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ApparelRequirement _003C_003E4__this;

		private Pawn p;

		public Pawn _003C_003E3__p;

		private bool ignoreGender;

		public bool _003C_003E3__ignoreGender;

		private bool includeWorn;

		public bool _003C_003E3__includeWorn;

		private List<ThingDef>.Enumerator _003C_003E7__wrap1;

		ThingDef IEnumerator<ThingDef>.Current
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
		public _003CAllAllowedApparelForPawn_003Ed__12(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<ThingDef>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				ApparelRequirement apparelRequirement = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = DefDatabase<ThingDef>.AllDefsListForReading.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					ThingDef current = _003C_003E7__wrap1.Current;
					if (apparelRequirement.AllowedForPawn(p, current, ignoreGender) && (includeWorn || !WearsAny(p, current)))
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<ThingDef>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
		{
			_003CAllAllowedApparelForPawn_003Ed__12 _003CAllAllowedApparelForPawn_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAllAllowedApparelForPawn_003Ed__ = this;
			}
			else
			{
				_003CAllAllowedApparelForPawn_003Ed__ = new _003CAllAllowedApparelForPawn_003Ed__12(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CAllAllowedApparelForPawn_003Ed__.p = _003C_003E3__p;
			_003CAllAllowedApparelForPawn_003Ed__.ignoreGender = _003C_003E3__ignoreGender;
			_003CAllAllowedApparelForPawn_003Ed__.includeWorn = _003C_003E3__includeWorn;
			return _003CAllAllowedApparelForPawn_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDef>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CAllRequiredApparel_003Ed__15 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ApparelRequirement _003C_003E4__this;

		private Gender gender;

		public Gender _003C_003E3__gender;

		private List<ThingDef>.Enumerator _003C_003E7__wrap1;

		ThingDef IEnumerator<ThingDef>.Current
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
		public _003CAllRequiredApparel_003Ed__15(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<ThingDef>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				ApparelRequirement apparelRequirement = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = DefDatabase<ThingDef>.AllDefsListForReading.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					ThingDef current = _003C_003E7__wrap1.Current;
					if (current.IsApparel && current.apparel.tags != null && ((apparelRequirement.requiredDefs != null && apparelRequirement.requiredDefs.Contains(current)) || apparelRequirement.HasRequiredTag(current)) && current.apparel.PawnCanWear(gender) && apparelRequirement.MatchesBodyPartGroups(current))
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<ThingDef>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
		{
			_003CAllRequiredApparel_003Ed__15 _003CAllRequiredApparel_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAllRequiredApparel_003Ed__ = this;
			}
			else
			{
				_003CAllRequiredApparel_003Ed__ = new _003CAllRequiredApparel_003Ed__15(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CAllRequiredApparel_003Ed__.gender = _003C_003E3__gender;
			return _003CAllRequiredApparel_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDef>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CAllRequiredApparelForPawn_003Ed__14 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ApparelRequirement _003C_003E4__this;

		private Pawn p;

		public Pawn _003C_003E3__p;

		private bool ignoreGender;

		public bool _003C_003E3__ignoreGender;

		private bool includeWorn;

		public bool _003C_003E3__includeWorn;

		private List<ThingDef>.Enumerator _003C_003E7__wrap1;

		ThingDef IEnumerator<ThingDef>.Current
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
		public _003CAllRequiredApparelForPawn_003Ed__14(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<ThingDef>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				ApparelRequirement apparelRequirement = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = DefDatabase<ThingDef>.AllDefsListForReading.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					ThingDef current = _003C_003E7__wrap1.Current;
					if (apparelRequirement.RequiredForPawn(p, current, ignoreGender) && (includeWorn || !WearsAny(p, current)))
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<ThingDef>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
		{
			_003CAllRequiredApparelForPawn_003Ed__14 _003CAllRequiredApparelForPawn_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAllRequiredApparelForPawn_003Ed__ = this;
			}
			else
			{
				_003CAllRequiredApparelForPawn_003Ed__ = new _003CAllRequiredApparelForPawn_003Ed__14(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CAllRequiredApparelForPawn_003Ed__.p = _003C_003E3__p;
			_003CAllRequiredApparelForPawn_003Ed__.ignoreGender = _003C_003E3__ignoreGender;
			_003CAllRequiredApparelForPawn_003Ed__.includeWorn = _003C_003E3__includeWorn;
			return _003CAllRequiredApparelForPawn_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDef>)this).GetEnumerator();
		}
	}

	public List<BodyPartGroupDef> bodyPartGroupsMatchAny;

	public List<string> requiredTags;

	public List<string> allowedTags;

	public List<ThingDef> requiredDefs;

	[MustTranslate]
	public string groupLabel;

	[Unsaved(false)]
	private bool? isValid;

	public bool IsValid
	{
		get
		{
			if (!isValid.HasValue)
			{
				isValid = AllRequiredApparel().Any();
			}
			return isValid.Value;
		}
	}

	private bool MatchesBodyPartGroups(ThingDef apparel)
	{
		foreach (BodyPartGroupDef item in bodyPartGroupsMatchAny)
		{
			if (apparel.apparel.bodyPartGroups.Contains(item))
			{
				return true;
			}
		}
		return false;
	}

	private bool HasRequiredTag(ThingDef apparel)
	{
		if (requiredTags != null)
		{
			foreach (string tag in apparel.apparel.tags)
			{
				if (requiredTags.Contains(tag))
				{
					return true;
				}
			}
		}
		return false;
	}

	private static bool WearsAny(Pawn p, ThingDef apparel)
	{
		foreach (Apparel item in p.apparel.WornApparel)
		{
			if (item.def == apparel)
			{
				return true;
			}
		}
		return false;
	}

	public bool AllowedForPawn(Pawn p, ThingDef apparel, bool ignoreGender = false)
	{
		if (!apparel.IsApparel)
		{
			return false;
		}
		if (!apparel.apparel.PawnCanWear(p, ignoreGender))
		{
			return false;
		}
		if (apparel.apparel.tags == null)
		{
			return false;
		}
		bool flag = requiredDefs != null && requiredDefs.Contains(apparel);
		if (!flag)
		{
			foreach (string tag in apparel.apparel.tags)
			{
				if ((requiredTags != null && requiredTags.Contains(tag)) || (allowedTags != null && allowedTags.Contains(tag)))
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			return MatchesBodyPartGroups(apparel);
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CAllAllowedApparelForPawn_003Ed__12))]
	public IEnumerable<ThingDef> AllAllowedApparelForPawn(Pawn p, bool ignoreGender = false, bool includeWorn = false)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllAllowedApparelForPawn_003Ed__12(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__p = p,
			_003C_003E3__ignoreGender = ignoreGender,
			_003C_003E3__includeWorn = includeWorn
		};
	}

	public bool RequiredForPawn(Pawn p, ThingDef apparel, bool ignoreGender = false)
	{
		if (!apparel.IsApparel)
		{
			return false;
		}
		if (!apparel.apparel.PawnCanWear(p, ignoreGender))
		{
			return false;
		}
		if (apparel.apparel.tags == null)
		{
			return false;
		}
		if ((requiredDefs != null && requiredDefs.Contains(apparel)) || HasRequiredTag(apparel))
		{
			return MatchesBodyPartGroups(apparel);
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CAllRequiredApparelForPawn_003Ed__14))]
	public IEnumerable<ThingDef> AllRequiredApparelForPawn(Pawn p, bool ignoreGender = false, bool includeWorn = false)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllRequiredApparelForPawn_003Ed__14(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__p = p,
			_003C_003E3__ignoreGender = ignoreGender,
			_003C_003E3__includeWorn = includeWorn
		};
	}

	[IteratorStateMachine(typeof(_003CAllRequiredApparel_003Ed__15))]
	public IEnumerable<ThingDef> AllRequiredApparel(Gender gender = Gender.None)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllRequiredApparel_003Ed__15(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__gender = gender
		};
	}

	public bool ApparelMeetsRequirement(ThingDef thingDef, bool allowUnmatched = true)
	{
		bool flag = false;
		for (int i = 0; i < bodyPartGroupsMatchAny.Count; i++)
		{
			if (thingDef.apparel.bodyPartGroups.Contains(bodyPartGroupsMatchAny[i]))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			if (requiredDefs != null && requiredDefs.Contains(thingDef))
			{
				return true;
			}
			if (requiredTags != null)
			{
				for (int j = 0; j < requiredTags.Count; j++)
				{
					if (thingDef.apparel.tags.Contains(requiredTags[j]))
					{
						return true;
					}
				}
			}
			if (allowedTags != null)
			{
				for (int k = 0; k < allowedTags.Count; k++)
				{
					if (thingDef.apparel.tags.Contains(allowedTags[k]))
					{
						return true;
					}
				}
			}
			return false;
		}
		return allowUnmatched;
	}

	public bool IsMet(Pawn p)
	{
		foreach (Apparel item in p.apparel.WornApparel)
		{
			bool flag = false;
			for (int i = 0; i < bodyPartGroupsMatchAny.Count; i++)
			{
				if (item.def.apparel.bodyPartGroups.Contains(bodyPartGroupsMatchAny[i]))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				continue;
			}
			if (requiredDefs != null && requiredDefs.Contains(item.def))
			{
				return true;
			}
			if (requiredTags != null)
			{
				for (int j = 0; j < requiredTags.Count; j++)
				{
					if (item.def.apparel.tags.Contains(requiredTags[j]))
					{
						return true;
					}
				}
			}
			if (allowedTags == null)
			{
				continue;
			}
			for (int k = 0; k < allowedTags.Count; k++)
			{
				if (item.def.apparel.tags.Contains(allowedTags[k]))
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool SameApparelAs(ApparelRequirement other)
	{
		if (requiredDefs.SetsEqual(other.requiredDefs) && requiredTags.SetsEqual(other.requiredTags))
		{
			return allowedTags.SetsEqual(other.allowedTags);
		}
		return false;
	}

	public bool IsActive(Pawn forPawn)
	{
		if (forPawn.apparel == null)
		{
			return false;
		}
		return forPawn.apparel.ActiveRequirementsForReading.Contains(this);
	}

	public ThingDef RandomRequiredApparelForPawnInGeneration(Pawn p, Func<ThingDef, bool> validator)
	{
		ThingDef result = null;
		if (!DefDatabase<ThingDef>.AllDefsListForReading.Where((ThingDef a) => a.IsApparel && a.apparel.tags != null && a.apparel.bodyPartGroups.Any((BodyPartGroupDef b) => bodyPartGroupsMatchAny.Contains(b)) && a.apparel.tags.Any((string t) => requiredTags.Contains(t)) && a.apparel.PawnCanWear(p) && (validator == null || validator(a))).TryRandomElementByWeight((ThingDef a) => a.generateCommonality, out result))
		{
			return null;
		}
		return result;
	}

	public override string ToString()
	{
		string text = "";
		if (requiredTags != null)
		{
			text += string.Join(",", requiredTags.ToArray());
		}
		if (allowedTags != null)
		{
			if (!text.NullOrEmpty())
			{
				text += "|";
			}
			text += string.Join(",", allowedTags.ToArray());
		}
		if (requiredDefs != null)
		{
			if (!text.NullOrEmpty())
			{
				text += "|";
			}
			text += string.Join(",", requiredDefs.Select((ThingDef d) => d.LabelCap).ToArray());
		}
		return "(" + string.Join(",", bodyPartGroupsMatchAny.Select((BodyPartGroupDef a) => a.defName).ToArray()) + ") -> " + text;
	}

	public void ExposeData()
	{
		Scribe_Collections.Look(ref bodyPartGroupsMatchAny, "bodyPartGroupsMatchAny", LookMode.Def);
		Scribe_Collections.Look(ref requiredDefs, "requiredDefs", LookMode.Def);
		Scribe_Collections.Look(ref requiredTags, "requiredTags", LookMode.Value);
		Scribe_Collections.Look(ref allowedTags, "allowedTags", LookMode.Value);
		Scribe_Values.Look(ref groupLabel, "groupLabel");
	}

	public virtual void CopyTo(ApparelRequirement other)
	{
		if (bodyPartGroupsMatchAny != null)
		{
			if (other.bodyPartGroupsMatchAny == null)
			{
				other.bodyPartGroupsMatchAny = new List<BodyPartGroupDef>();
			}
			other.bodyPartGroupsMatchAny.Clear();
			other.bodyPartGroupsMatchAny.AddRange(bodyPartGroupsMatchAny);
		}
		else
		{
			other.bodyPartGroupsMatchAny = null;
		}
		if (requiredTags != null)
		{
			if (other.requiredTags == null)
			{
				other.requiredTags = new List<string>();
			}
			other.requiredTags.Clear();
			other.requiredTags.AddRange(requiredTags);
		}
		else
		{
			other.requiredTags = null;
		}
		if (allowedTags != null)
		{
			if (other.allowedTags == null)
			{
				other.allowedTags = new List<string>();
			}
			other.allowedTags.Clear();
			other.allowedTags.AddRange(allowedTags);
		}
		else
		{
			other.allowedTags = null;
		}
		if (requiredDefs != null)
		{
			if (other.requiredDefs == null)
			{
				other.requiredDefs = new List<ThingDef>();
			}
			other.requiredDefs.Clear();
			other.requiredDefs.AddRange(requiredDefs);
		}
		else
		{
			other.requiredDefs = null;
		}
		other.groupLabel = groupLabel;
	}
}
