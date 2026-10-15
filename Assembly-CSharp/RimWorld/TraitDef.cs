using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class TraitDef : Def
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass17_0
	{
		public TraitDegreeData dd;

		internal bool _003CConfigErrors_003Eb__0(TraitDegreeData dd2)
		{
			return dd2.degree == dd.degree;
		}
	}

	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__17 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public TraitDef _003C_003E4__this;

		private _003C_003Ec__DisplayClass17_0 _003C_003E8__1;

		private IEnumerator<string> _003C_003E7__wrap1;

		private int _003Ci_003E5__3;

		private List<IngestibleModifiers>.Enumerator _003C_003E7__wrap3;

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
		public _003CConfigErrors_003Ed__17(int _003C_003E1__state)
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
			case 4:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap3 = default(List<IngestibleModifiers>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				TraitDef traitDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)traitDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007a;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007a;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00cc;
				case 3:
					_003C_003E1__state = -1;
					goto IL_0158;
				case 4:
					{
						_003C_003E1__state = -4;
						goto IL_01cb;
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
					if (!traitDef.degreeDatas.Any())
					{
						_003C_003E2__current = traitDef.defName + " has no degree datas.";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00cc;
					IL_01ea:
					_003C_003E8__1 = null;
					_003Ci_003E5__3++;
					goto IL_0203;
					IL_0158:
					if (!_003C_003E8__1.dd.ingestibleModifiers.NullOrEmpty())
					{
						_003C_003E7__wrap3 = _003C_003E8__1.dd.ingestibleModifiers.GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_01cb;
					}
					goto IL_01ea;
					IL_00cc:
					_003Ci_003E5__3 = 0;
					goto IL_0203;
					IL_0203:
					if (_003Ci_003E5__3 < traitDef.degreeDatas.Count)
					{
						_003C_003E8__1 = new _003C_003Ec__DisplayClass17_0();
						_003C_003E8__1.dd = traitDef.degreeDatas[_003Ci_003E5__3];
						if (traitDef.degreeDatas.Count((TraitDegreeData dd2) => dd2.degree == _003C_003E8__1.dd.degree) > 1)
						{
							_003C_003E2__current = ">1 datas for degree " + _003C_003E8__1.dd.degree;
							_003C_003E1__state = 3;
							return true;
						}
						goto IL_0158;
					}
					return false;
					IL_01cb:
					while (_003C_003E7__wrap3.MoveNext())
					{
						if (_003C_003E7__wrap3.Current?.ingestible == null)
						{
							_003C_003E2__current = "ingestible override has a null target ingestible";
							_003C_003E1__state = 4;
							return true;
						}
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap3 = default(List<IngestibleModifiers>.Enumerator);
					goto IL_01ea;
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
			_003CConfigErrors_003Ed__17 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__17(0)
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

	public List<TraitDegreeData> degreeDatas = new List<TraitDegreeData>();

	public List<TraitDef> conflictingTraits = new List<TraitDef>();

	public List<string> exclusionTags = new List<string>();

	public List<SkillDef> conflictingPassions = new List<SkillDef>();

	public List<SkillDef> forcedPassions = new List<SkillDef>();

	public List<WorkTypeDef> requiredWorkTypes = new List<WorkTypeDef>();

	public WorkTags requiredWorkTags;

	public List<WorkTypeDef> disabledWorkTypes = new List<WorkTypeDef>();

	public WorkTags disabledWorkTags;

	public AnimalType? disableHostilityFromAnimalType;

	public FactionDef disableHostilityFromFaction;

	public bool canBeSuppressed = true;

	private float commonality = 1f;

	private float commonalityFemale = -1f;

	public bool allowOnHostileSpawn = true;

	public static TraitDef Named(string defName)
	{
		return DefDatabase<TraitDef>.GetNamed(defName);
	}

	public TraitDegreeData DataAtDegree(int degree)
	{
		for (int i = 0; i < degreeDatas.Count; i++)
		{
			if (degreeDatas[i].degree == degree)
			{
				return degreeDatas[i];
			}
		}
		Log.Error(defName + " found no data at degree " + degree + ", returning first defined.");
		return degreeDatas[0];
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__17))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__17(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ResolveReferences()
	{
		base.ResolveReferences();
		foreach (TraitDegreeData degreeData in degreeDatas)
		{
			degreeData.ResolveReferences();
		}
	}

	public bool ConflictsWith(Trait other)
	{
		return ConflictsWith(other.def);
	}

	public bool ConflictsWith(TraitDef other)
	{
		if ((other.conflictingTraits != null && other.conflictingTraits.Contains(this)) || (conflictingTraits != null && conflictingTraits.Contains(other)))
		{
			return true;
		}
		if (exclusionTags != null && other.exclusionTags != null)
		{
			for (int i = 0; i < exclusionTags.Count; i++)
			{
				if (other.exclusionTags.Contains(exclusionTags[i]))
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool CanSuppress(Trait other)
	{
		if (!ConflictsWith(other))
		{
			return other.def == this;
		}
		return true;
	}

	public bool ConflictsWithPassion(SkillDef passion)
	{
		if (conflictingPassions != null)
		{
			return conflictingPassions.Contains(passion);
		}
		return false;
	}

	public bool RequiresPassion(SkillDef passion)
	{
		if (forcedPassions != null)
		{
			return forcedPassions.Contains(passion);
		}
		return false;
	}

	public float GetGenderSpecificCommonality(Gender gender)
	{
		if (gender == Gender.Female && commonalityFemale >= 0f)
		{
			return commonalityFemale;
		}
		return commonality;
	}
}
