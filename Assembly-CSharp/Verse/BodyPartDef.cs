using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse;

public class BodyPartDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__34 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public BodyPartDef _003C_003E4__this;

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
		public _003CConfigErrors_003Ed__34(int _003C_003E1__state)
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
				BodyPartDef bodyPartDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)bodyPartDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0076;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00ca;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0076:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (bodyPartDef.frostbiteVulnerability > 10f)
					{
						_003C_003E2__current = "frostbitePriority > max 10: " + bodyPartDef.frostbiteVulnerability;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00ca;
					IL_00ca:
					if (bodyPartDef.solid && bodyPartDef.bleedRate > 0f)
					{
						_003C_003E2__current = "solid but bleedRate is not zero";
						_003C_003E1__state = 3;
						return true;
					}
					break;
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
			_003CConfigErrors_003Ed__34 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__34(0)
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

	[MustTranslate]
	public string labelShort;

	public List<BodyPartTagDef> tags = new List<BodyPartTagDef>();

	public int hitPoints = 10;

	public float permanentInjuryChanceFactor = 1f;

	public float bleedRate = 1f;

	public float frostbiteVulnerability;

	private bool skinCovered;

	private bool solid;

	public bool alive = true;

	public bool delicate;

	public bool canScarify;

	public bool beautyRelated;

	public bool conceptual;

	public bool socketed;

	public ThingDef spawnThingOnRemoved;

	public bool pawnGeneratorCanAmputate;

	public bool canSuggestAmputation = true;

	public bool forceAlwaysRemovable;

	public Dictionary<DamageDef, float> hitChanceFactors;

	public bool destroyableByDamage = true;

	public bool canBeVacuumBurnt = true;

	[MustTranslate]
	public string removeRecipeLabelOverride;

	public float executionPartPriority;

	[Unsaved(false)]
	private string cachedLabelShortCap;

	public bool IsSolidInDefinition_Debug => solid;

	public bool IsSkinCoveredInDefinition_Debug => skinCovered;

	public bool IsMirroredPart => tags.Contains(BodyPartTagDefOf.Mirrored);

	public string LabelShort
	{
		get
		{
			if (!labelShort.NullOrEmpty())
			{
				return labelShort;
			}
			return label;
		}
	}

	public string LabelShortCap
	{
		get
		{
			if (labelShort.NullOrEmpty())
			{
				return LabelCap;
			}
			if (cachedLabelShortCap == null)
			{
				cachedLabelShortCap = labelShort.CapitalizeFirst();
			}
			return cachedLabelShortCap;
		}
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__34))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__34(-2)
		{
			_003C_003E4__this = this
		};
	}

	public bool IsSolid(BodyPartRecord part, List<Hediff> hediffs)
	{
		for (BodyPartRecord bodyPartRecord = part; bodyPartRecord != null; bodyPartRecord = bodyPartRecord.parent)
		{
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].Part == bodyPartRecord && hediffs[i] is Hediff_AddedPart)
				{
					if (hediffs[i].def.addedPartProps.solid)
					{
						return !hediffs[i].def.organicAddedBodypart;
					}
					return false;
				}
			}
		}
		return solid;
	}

	public bool IsSkinCovered(BodyPartRecord part, HediffSet body)
	{
		if (body.PartOrAnyAncestorHasDirectlyAddedParts(part))
		{
			return false;
		}
		return skinCovered;
	}

	public float GetMaxHealth(Pawn pawn)
	{
		return Mathf.CeilToInt((float)hitPoints * pawn.HealthScale);
	}

	public float GetHitChanceFactorFor(DamageDef damage)
	{
		if (conceptual)
		{
			return 0f;
		}
		if (hitChanceFactors == null)
		{
			return 1f;
		}
		if (hitChanceFactors.TryGetValue(damage, out var value))
		{
			return value;
		}
		return 1f;
	}
}
