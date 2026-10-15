using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld;

public class QuestPart_Delay : QuestPartActivable
{
	[CompilerGenerated]
	private sealed class _003Cget_QuestLookTargets_003Ed__26 : IEnumerable<GlobalTargetInfo>, IEnumerable, IEnumerator<GlobalTargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GlobalTargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_Delay _003C_003E4__this;

		private IEnumerator<GlobalTargetInfo> _003C_003E7__wrap1;

		private int _003Ci_003E5__3;

		GlobalTargetInfo IEnumerator<GlobalTargetInfo>.Current
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
		public _003Cget_QuestLookTargets_003Ed__26(int _003C_003E1__state)
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
				QuestPart_Delay questPart_Delay = _003C_003E4__this;
				ISelectable selectable;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart)questPart_Delay).QuestLookTargets.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0076;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -1;
					goto IL_0116;
				case 3:
					{
						_003C_003E1__state = -1;
						goto IL_0116;
					}
					IL_0076:
					if (_003C_003E7__wrap1.MoveNext())
					{
						GlobalTargetInfo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (questPart_Delay.inspectStringTargets == null)
					{
						break;
					}
					_003Ci_003E5__3 = 0;
					goto IL_0128;
					IL_0116:
					_003Ci_003E5__3++;
					goto IL_0128;
					IL_0128:
					if (_003Ci_003E5__3 >= questPart_Delay.inspectStringTargets.Count)
					{
						break;
					}
					selectable = questPart_Delay.inspectStringTargets[_003Ci_003E5__3];
					if (selectable is Thing)
					{
						_003C_003E2__current = (Thing)selectable;
						_003C_003E1__state = 2;
						return true;
					}
					if (selectable is WorldObject)
					{
						_003C_003E2__current = (WorldObject)selectable;
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0116;
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
		IEnumerator<GlobalTargetInfo> IEnumerable<GlobalTargetInfo>.GetEnumerator()
		{
			_003Cget_QuestLookTargets_003Ed__26 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_QuestLookTargets_003Ed__26(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<GlobalTargetInfo>)this).GetEnumerator();
		}
	}

	public int delayTicks;

	public string expiryInfoPart;

	public string expiryInfoPartTip;

	public string inspectString;

	public List<ISelectable> inspectStringTargets;

	public bool isBad;

	public bool waitUntilPlayerHasHomeMap;

	public string alertLabel;

	public string alertExplanation;

	public List<GlobalTargetInfo> alertCulprits = new List<GlobalTargetInfo>();

	public int ticksLeftAlertCritical;

	public int TicksLeft
	{
		get
		{
			if (base.State != QuestPartState.Enabled)
			{
				return 0;
			}
			return enableTick + delayTicks - Find.TickManager.TicksGame;
		}
	}

	public override string ExpiryInfoPart
	{
		get
		{
			if (quest.Historical)
			{
				return null;
			}
			return expiryInfoPart.Formatted(TicksLeft.ToStringTicksToPeriod());
		}
	}

	public override string ExpiryInfoPartTip => expiryInfoPartTip.Formatted(GenDate.DateFullStringWithHourAt(GenDate.TickGameToAbs(enableTick + delayTicks), QuestUtility.GetLocForDates()));

	public override string AlertLabel
	{
		get
		{
			TaggedString? taggedString = alertLabel?.Formatted(TicksLeft.ToStringTicksToPeriodVerbose());
			if (!taggedString.HasValue)
			{
				return null;
			}
			return taggedString.GetValueOrDefault();
		}
	}

	public override string AlertExplanation
	{
		get
		{
			TaggedString? taggedString = alertExplanation?.Formatted(TicksLeft.ToStringTicksToPeriodVerbose());
			if (!taggedString.HasValue)
			{
				return null;
			}
			return taggedString.GetValueOrDefault();
		}
	}

	public override AlertReport AlertReport
	{
		get
		{
			if (alertCulprits.Count <= 0)
			{
				return AlertReport.Inactive;
			}
			return AlertReport.CulpritsAre(alertCulprits);
		}
	}

	public override bool AlertCritical => TicksLeft < ticksLeftAlertCritical;

	public override IEnumerable<GlobalTargetInfo> QuestLookTargets
	{
		[IteratorStateMachine(typeof(_003Cget_QuestLookTargets_003Ed__26))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_QuestLookTargets_003Ed__26(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override void QuestPartTick()
	{
		base.QuestPartTick();
		if ((!waitUntilPlayerHasHomeMap || Find.AnyPlayerHomeMap != null) && Find.TickManager.TicksGame >= enableTick + delayTicks)
		{
			DelayFinished();
		}
	}

	protected virtual void DelayFinished()
	{
		Complete();
	}

	public override string ExtraInspectString(ISelectable target)
	{
		if (inspectStringTargets != null && inspectStringTargets.Contains(target))
		{
			return inspectString.Formatted(TicksLeft.ToStringTicksToPeriod()).Resolve();
		}
		return null;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref delayTicks, "delayTicks", 0);
		Scribe_Values.Look(ref expiryInfoPart, "expiryInfoPart");
		Scribe_Values.Look(ref expiryInfoPartTip, "expiryInfoPartTip");
		Scribe_Values.Look(ref inspectString, "inspectString");
		Scribe_Collections.Look(ref inspectStringTargets, "inspectStringTargets", LookMode.Reference);
		Scribe_Values.Look(ref isBad, "isBad", defaultValue: false);
		Scribe_Values.Look(ref alertLabel, "alertLabel");
		Scribe_Values.Look(ref alertExplanation, "alertExplanation");
		Scribe_Values.Look(ref ticksLeftAlertCritical, "ticksLeftAlertCritical", 0);
		Scribe_Values.Look(ref waitUntilPlayerHasHomeMap, "waitUntilPlayerHasHomeMap", defaultValue: false);
		Scribe_Collections.Look(ref alertCulprits, "alertCulprits", LookMode.GlobalTargetInfo);
		if (alertCulprits == null)
		{
			alertCulprits = new List<GlobalTargetInfo>();
		}
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			alertCulprits.RemoveAll((GlobalTargetInfo x) => !x.IsValid);
		}
	}

	public override void DoDebugWindowContents(Rect innerRect, ref float curY)
	{
		if (base.State == QuestPartState.Enabled)
		{
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect, "End " + ToString(), drawBackground: true, doMouseoverSound: true, active: true, null))
			{
				DelayFinished();
			}
			curY += rect.height + 4f;
		}
	}

	public override void AssignDebugData()
	{
		base.AssignDebugData();
		delayTicks = Rand.RangeInclusive(833, 2500);
	}

	public void DebugForceEnd()
	{
		DelayFinished();
	}
}
