using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld;

public class StatPart_GearStatOffset : StatPart
{
	[CompilerGenerated]
	private sealed class _003CGetInfoCardHyperlinks_003Ed__8 : IEnumerable<Dialog_InfoCard.Hyperlink>, IEnumerable, IEnumerator<Dialog_InfoCard.Hyperlink>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Dialog_InfoCard.Hyperlink _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private StatRequest req;

		public StatRequest _003C_003E3__req;

		public StatPart_GearStatOffset _003C_003E4__this;

		private Pawn _003Cpawn_003E5__2;

		private int _003Ci_003E5__3;

		Dialog_InfoCard.Hyperlink IEnumerator<Dialog_InfoCard.Hyperlink>.Current
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
		public _003CGetInfoCardHyperlinks_003Ed__8(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cpawn_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			StatPart_GearStatOffset statPart_GearStatOffset = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_00af;
			}
			_003C_003E1__state = -1;
			Thing thing = req.Thing;
			_003Cpawn_003E5__2 = thing as Pawn;
			if (_003Cpawn_003E5__2 == null)
			{
				return false;
			}
			if (_003Cpawn_003E5__2.apparel == null)
			{
				return false;
			}
			_003Ci_003E5__3 = 0;
			goto IL_00c1;
			IL_00c1:
			if (_003Ci_003E5__3 < _003Cpawn_003E5__2.apparel.WornApparel.Count)
			{
				Apparel thing2 = _003Cpawn_003E5__2.apparel.WornApparel[_003Ci_003E5__3];
				if (!Mathf.Approximately(thing2.GetStatValue(statPart_GearStatOffset.apparelStat), 0f))
				{
					_003C_003E2__current = new Dialog_InfoCard.Hyperlink(thing2);
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00af;
			}
			return false;
			IL_00af:
			_003Ci_003E5__3++;
			goto IL_00c1;
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
		IEnumerator<Dialog_InfoCard.Hyperlink> IEnumerable<Dialog_InfoCard.Hyperlink>.GetEnumerator()
		{
			_003CGetInfoCardHyperlinks_003Ed__8 _003CGetInfoCardHyperlinks_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetInfoCardHyperlinks_003Ed__ = this;
			}
			else
			{
				_003CGetInfoCardHyperlinks_003Ed__ = new _003CGetInfoCardHyperlinks_003Ed__8(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetInfoCardHyperlinks_003Ed__.req = _003C_003E3__req;
			return _003CGetInfoCardHyperlinks_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Dialog_InfoCard.Hyperlink>)this).GetEnumerator();
		}
	}

	private StatDef apparelStat;

	private bool subtract;

	private bool includeWeapon;

	public override void TransformValue(StatRequest req, ref float val)
	{
		if (!req.HasThing || req.Thing == null || !(req.Thing is Pawn pawn))
		{
			return;
		}
		if (pawn.apparel != null)
		{
			for (int i = 0; i < pawn.apparel.WornApparel.Count; i++)
			{
				float statValue = pawn.apparel.WornApparel[i].GetStatValue(apparelStat);
				statValue += StatWorker.StatOffsetFromGear(pawn.apparel.WornApparel[i], apparelStat);
				if (subtract)
				{
					val -= statValue;
				}
				else
				{
					val += statValue;
				}
			}
		}
		if (includeWeapon && pawn.equipment != null && pawn.equipment.Primary != null)
		{
			float statValue2 = pawn.equipment.Primary.GetStatValue(apparelStat);
			statValue2 += StatWorker.StatOffsetFromGear(pawn.equipment.Primary, apparelStat);
			if (subtract)
			{
				val -= statValue2;
			}
			else
			{
				val += statValue2;
			}
		}
	}

	public override string ExplanationPart(StatRequest req)
	{
		if (req.HasThing && req.Thing != null)
		{
			if (!(req.Thing is Pawn pawn) || !PawnWearingRelevantGear(pawn))
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("StatsReport_RelevantGear".Translate());
			if (pawn.apparel != null)
			{
				for (int i = 0; i < pawn.apparel.WornApparel.Count; i++)
				{
					Apparel gear = pawn.apparel.WornApparel[i];
					float num = GetStatValueForGear(gear);
					if (subtract)
					{
						num = 0f - num;
					}
					if (!Mathf.Approximately(num, 0f))
					{
						stringBuilder.AppendLine(InfoTextLineFrom(gear, num));
					}
				}
			}
			if (includeWeapon && pawn.equipment != null && pawn.equipment.Primary != null)
			{
				float num2 = GetStatValueForGear(pawn.equipment.Primary);
				if (subtract)
				{
					num2 = 0f - num2;
				}
				if (!Mathf.Approximately(num2, 0f))
				{
					stringBuilder.AppendLine(InfoTextLineFrom(pawn.equipment.Primary, num2));
				}
			}
			return stringBuilder.ToString();
		}
		return null;
	}

	private string InfoTextLineFrom(Thing gear, float value)
	{
		return "    " + gear.LabelCap + ": " + value.ToStringByStyle(parentStat.toStringStyle, ToStringNumberSense.Offset);
	}

	private float GetStatValueForGear(Thing gear)
	{
		return gear.GetStatValue(apparelStat) + StatWorker.StatOffsetFromGear(gear, apparelStat);
	}

	private bool PawnWearingRelevantGear(Pawn pawn)
	{
		if (pawn.apparel != null)
		{
			for (int i = 0; i < pawn.apparel.WornApparel.Count; i++)
			{
				Apparel apparel = pawn.apparel.WornApparel[i];
				if (apparel.GetStatValue(apparelStat) != 0f)
				{
					return true;
				}
				if (StatWorker.StatOffsetFromGear(apparel, apparelStat) != 0f)
				{
					return true;
				}
			}
		}
		if (includeWeapon && pawn.equipment != null && pawn.equipment.Primary != null && StatWorker.StatOffsetFromGear(pawn.equipment.Primary, apparelStat) != 0f)
		{
			return true;
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CGetInfoCardHyperlinks_003Ed__8))]
	public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest req)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetInfoCardHyperlinks_003Ed__8(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__req = req
		};
	}
}
