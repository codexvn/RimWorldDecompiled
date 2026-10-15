using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld;

public class StatPart_GearStatFactor : StatPart
{
	[CompilerGenerated]
	private sealed class _003CGetInfoCardHyperlinks_003Ed__6 : IEnumerable<Dialog_InfoCard.Hyperlink>, IEnumerable, IEnumerator<Dialog_InfoCard.Hyperlink>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Dialog_InfoCard.Hyperlink _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private StatRequest req;

		public StatRequest _003C_003E3__req;

		public StatPart_GearStatFactor _003C_003E4__this;

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
		public _003CGetInfoCardHyperlinks_003Ed__6(int _003C_003E1__state)
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
			StatPart_GearStatFactor statPart_GearStatFactor = _003C_003E4__this;
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
				if (!Mathf.Approximately(thing2.GetStatValue(statPart_GearStatFactor.apparelStat), 1f))
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
			_003CGetInfoCardHyperlinks_003Ed__6 _003CGetInfoCardHyperlinks_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetInfoCardHyperlinks_003Ed__ = this;
			}
			else
			{
				_003CGetInfoCardHyperlinks_003Ed__ = new _003CGetInfoCardHyperlinks_003Ed__6(0)
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
				val *= pawn.apparel.WornApparel[i].GetStatValue(apparelStat);
			}
		}
		if (includeWeapon && pawn.equipment != null && pawn.equipment.Primary != null)
		{
			val *= pawn.equipment.Primary.GetStatValue(apparelStat);
		}
	}

	public override string ExplanationPart(StatRequest req)
	{
		if (!req.HasThing || req.Thing == null)
		{
			return null;
		}
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
				Apparel apparel = pawn.apparel.WornApparel[i];
				float statValue = apparel.GetStatValue(apparelStat);
				if (!Mathf.Approximately(statValue, 1f))
				{
					stringBuilder.AppendLine(InfoTextLineFrom(apparel, statValue));
				}
			}
		}
		if (includeWeapon && pawn.equipment != null && pawn.equipment.Primary != null)
		{
			float statValue2 = pawn.equipment.Primary.GetStatValue(apparelStat);
			if (!Mathf.Approximately(statValue2, 1f))
			{
				stringBuilder.AppendLine(InfoTextLineFrom(pawn.equipment.Primary, statValue2));
			}
		}
		return stringBuilder.ToString();
	}

	private string InfoTextLineFrom(Thing gear, float value)
	{
		return "    " + gear.LabelCap + ": " + value.ToStringByStyle(apparelStat.toStringStyle, ToStringNumberSense.Factor);
	}

	private bool PawnWearingRelevantGear(Pawn pawn)
	{
		if (pawn.apparel != null)
		{
			for (int i = 0; i < pawn.apparel.WornApparel.Count; i++)
			{
				if (!Mathf.Approximately(pawn.apparel.WornApparel[i].GetStatValue(apparelStat), 1f))
				{
					return true;
				}
			}
		}
		if (includeWeapon && pawn.equipment != null && pawn.equipment.Primary != null && !Mathf.Approximately(pawn.equipment.Primary.GetStatValue(apparelStat), 1f))
		{
			return true;
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CGetInfoCardHyperlinks_003Ed__6))]
	public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest req)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetInfoCardHyperlinks_003Ed__6(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__req = req
		};
	}
}
