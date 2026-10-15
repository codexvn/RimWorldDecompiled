using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld;

public class CompProducesBioferrite : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__2 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompProducesBioferrite _003C_003E4__this;

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
		public _003CSpecialDisplayStats_003Ed__2(int _003C_003E1__state)
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
			CompProducesBioferrite compProducesBioferrite = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				StatDrawEntry statDrawEntry = BioferriteStatDrawEntry(compProducesBioferrite.parent as Pawn);
				statDrawEntry.overridesHideStats = true;
				_003C_003E2__current = statDrawEntry;
				_003C_003E1__state = 1;
				return true;
			}
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__2 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__2(0)
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

	public CompProperties_ProducesBioferrite Props => (CompProperties_ProducesBioferrite)props;

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__2))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__2(-2)
		{
			_003C_003E4__this = this
		};
	}

	public static float BioferritePerDay(Pawn pawn)
	{
		CompProducesBioferrite compProducesBioferrite = pawn.TryGetComp<CompProducesBioferrite>();
		if (compProducesBioferrite == null && (!pawn.IsMutant || !pawn.mutant.Def.producesBioferrite))
		{
			return 0f;
		}
		if (pawn.health.hediffSet.HasHediff(HediffDefOf.BioferriteExtracted))
		{
			return 0f;
		}
		float num = compProducesBioferrite?.Props.bioferriteDensity ?? 1f;
		return pawn.BodySize * num;
	}

	public static StatDrawEntry BioferriteStatDrawEntry(Pawn pawn)
	{
		CompProducesBioferrite compProducesBioferrite = pawn.TryGetComp<CompProducesBioferrite>();
		StringBuilder stringBuilder = new StringBuilder("StatsReport_BioferriteGeneration_Desc".Translate());
		stringBuilder.AppendLine();
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("StatsReport_BaseValue".Translate() + ": 1");
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("StatsReport_BodySize".Translate(pawn.BodySize.ToString("F2")) + ": x" + pawn.BodySize.ToStringPercent());
		if (compProducesBioferrite != null)
		{
			stringBuilder.AppendLine("StatsReport_BioferriteDensityMultiplier".Translate() + ": x" + compProducesBioferrite.Props.bioferriteDensity.ToStringPercent());
		}
		if (pawn.health.hediffSet.HasHediff(HediffDefOf.BioferriteExtracted))
		{
			stringBuilder.AppendLine("StatsReport_BioferriteExtractedMultiplier".Translate() + ": x" + 0f.ToStringPercent());
		}
		stringBuilder.AppendLine();
		stringBuilder.Append("StatsReport_FinalValue".Translate() + ": " + BioferritePerDay(pawn).ToString("F1"));
		return new StatDrawEntry(StatCategoryDefOf.Containment, "StatsReport_BioferriteGeneration".Translate(), BioferritePerDay(pawn).ToString(), stringBuilder.ToString(), 100);
	}
}
