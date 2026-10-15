using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld;

public class StatPart_Stuff : StatPart
{
	[CompilerGenerated]
	private sealed class _003CGetInfoCardHyperlinks_003Ed__5 : IEnumerable<Dialog_InfoCard.Hyperlink>, IEnumerable, IEnumerator<Dialog_InfoCard.Hyperlink>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Dialog_InfoCard.Hyperlink _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private StatRequest req;

		public StatRequest _003C_003E3__req;

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
		public _003CGetInfoCardHyperlinks_003Ed__5(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (req.StuffDef != null)
				{
					_003C_003E2__current = new Dialog_InfoCard.Hyperlink(req.StuffDef);
					_003C_003E1__state = 1;
					return true;
				}
				break;
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
		IEnumerator<Dialog_InfoCard.Hyperlink> IEnumerable<Dialog_InfoCard.Hyperlink>.GetEnumerator()
		{
			_003CGetInfoCardHyperlinks_003Ed__5 _003CGetInfoCardHyperlinks_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetInfoCardHyperlinks_003Ed__ = this;
			}
			else
			{
				_003CGetInfoCardHyperlinks_003Ed__ = new _003CGetInfoCardHyperlinks_003Ed__5(0);
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

	public StatDef stuffPowerStat;

	public StatDef multiplierStat;

	public override string ExplanationPart(StatRequest req)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (req.BuildableDef.MadeFromStuff)
		{
			string text = ((req.StuffDef != null) ? req.StuffDef.label : "None".TranslateSimple());
			string text2 = ((req.StuffDef != null) ? req.StuffDef.GetStatValueAbstract(stuffPowerStat).ToStringByStyle(parentStat.ToStringStyleUnfinalized) : "0");
			stringBuilder.AppendLine("StatsReport_Material".Translate() + " (" + text + "): " + text2);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("StatsReport_StuffEffectMultiplier".Translate() + ": " + GetMultiplier(req).ToStringPercent("F0"));
			stringBuilder.AppendLine();
		}
		return stringBuilder.ToString().TrimEndNewlines();
	}

	public override void TransformValue(StatRequest req, ref float value)
	{
		float num = ((req.StuffDef != null) ? req.StuffDef.GetStatValueAbstract(stuffPowerStat) : 0f);
		value += GetMultiplier(req) * num;
	}

	private float GetMultiplier(StatRequest req)
	{
		if (req.HasThing)
		{
			return req.Thing.GetStatValue(multiplierStat);
		}
		return req.BuildableDef.GetStatValueAbstract(multiplierStat);
	}

	[IteratorStateMachine(typeof(_003CGetInfoCardHyperlinks_003Ed__5))]
	public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest req)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetInfoCardHyperlinks_003Ed__5(-2)
		{
			_003C_003E3__req = req
		};
	}
}
