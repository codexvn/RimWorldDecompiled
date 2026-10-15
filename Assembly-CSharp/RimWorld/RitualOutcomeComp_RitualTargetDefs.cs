using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class RitualOutcomeComp_RitualTargetDefs : RitualOutcomeComp_QualitySingleOffset
{
	[CompilerGenerated]
	private sealed class _003CBlockingIssues_003Ed__8 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private TargetInfo target;

		public TargetInfo _003C_003E3__target;

		public RitualOutcomeComp_RitualTargetDefs _003C_003E4__this;

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
		public _003CBlockingIssues_003Ed__8(int _003C_003E1__state)
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
			RitualOutcomeComp_RitualTargetDefs ritualOutcomeComp_RitualTargetDefs = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				Thing thing = target.Thing;
				if (thing != null && ritualOutcomeComp_RitualTargetDefs.autoPoweredNeedsToBeOn)
				{
					CompAutoPowered compAutoPowered = thing.TryGetComp<CompAutoPowered>();
					if (compAutoPowered != null && !compAutoPowered.AppearsPowered)
					{
						_003C_003E2__current = "RitualOutcomeBlockedNoPower".Translate(thing.LabelShort).CapitalizeFirst();
						_003C_003E1__state = 1;
						return true;
					}
				}
				break;
			}
			case 1:
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CBlockingIssues_003Ed__8 _003CBlockingIssues_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CBlockingIssues_003Ed__ = this;
			}
			else
			{
				_003CBlockingIssues_003Ed__ = new _003CBlockingIssues_003Ed__8(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CBlockingIssues_003Ed__.target = _003C_003E3__target;
			return _003CBlockingIssues_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[MustTranslate]
	public string expectedThingLabelTip;

	public List<ThingDef> defs;

	public bool allowAltars;

	public bool autoPoweredNeedsToBeOn;

	public bool autoApplyInClassicMode;

	public override bool Applies(LordJob_Ritual ritual)
	{
		if (autoApplyInClassicMode && Find.IdeoManager.classicMode)
		{
			return true;
		}
		Thing thing = ritual.selectedTarget.Thing;
		if (thing == null)
		{
			return false;
		}
		if (!CheckIdeoCorrectness(ritual?.Ritual, thing))
		{
			return false;
		}
		if (allowAltars && thing.def.isAltar)
		{
			return true;
		}
		if (!defs.NullOrEmpty())
		{
			return defs.Contains(ritual.selectedTarget.Thing?.def);
		}
		return false;
	}

	protected bool CheckIdeoCorrectness(Precept_Ritual ritual, Thing thing)
	{
		Ideo ideo = ritual?.ideo;
		if (thing.StyleSourcePrecept != null)
		{
			if (ideo != null)
			{
				return thing.StyleSourcePrecept.ideo == ideo;
			}
			return false;
		}
		return true;
	}

	public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
	{
		if (autoApplyInClassicMode && Find.IdeoManager.classicMode)
		{
			return new QualityFactor
			{
				quality = qualityOffset
			};
		}
		Thing thing = ritualTarget.Thing;
		if (thing == null)
		{
			return null;
		}
		bool flag = CheckIdeoCorrectness(ritual, thing) && ((allowAltars && thing.def.isAltar) || (!defs.NullOrEmpty() && defs.Contains(ritualTarget.Thing.def)));
		TaggedString taggedString = "RitualOutcomeCompTip_RitualTargetDefsPart1".Translate(ritual.LabelCap, expectedThingLabelTip);
		if (!flag)
		{
			taggedString += "\n\n" + "RitualOutcomeCompTip_RitualTargetDefsPart2".Translate(ritual.LabelCap, thing);
		}
		return new QualityFactor
		{
			label = label.CapitalizeFirst(),
			qualityChange = ExpectedOffsetDesc(flag, -1f),
			positive = flag,
			present = flag,
			quality = (flag ? qualityOffset : 0f),
			priority = 2f,
			toolTip = taggedString
		};
	}

	[IteratorStateMachine(typeof(_003CBlockingIssues_003Ed__8))]
	public override IEnumerable<string> BlockingIssues(Precept_Ritual ritual, TargetInfo target, RitualRoleAssignments assignments)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CBlockingIssues_003Ed__8(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__target = target
		};
	}

	protected override string ExpectedOffsetDesc(bool positive, float quality = -1f)
	{
		quality = ((quality == -1f) ? qualityOffset : quality);
		if (!positive)
		{
			return "";
		}
		return quality.ToStringWithSign("0.#%");
	}
}
