using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld;

public class StatWorker_MeleeDPS : StatWorker
{
	[CompilerGenerated]
	private sealed class _003CGetInfoCardHyperlinks_003Ed__8 : IEnumerable<Dialog_InfoCard.Hyperlink>, IEnumerable, IEnumerator<Dialog_InfoCard.Hyperlink>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Dialog_InfoCard.Hyperlink _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private StatRequest statRequest;

		public StatRequest _003C_003E3__statRequest;

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
				if (statRequest.Thing is Pawn { equipment: not null } pawn && pawn.equipment.Primary != null)
				{
					_003C_003E2__current = new Dialog_InfoCard.Hyperlink(pawn.equipment.Primary);
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
			_003CGetInfoCardHyperlinks_003Ed__8 _003CGetInfoCardHyperlinks_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetInfoCardHyperlinks_003Ed__ = this;
			}
			else
			{
				_003CGetInfoCardHyperlinks_003Ed__ = new _003CGetInfoCardHyperlinks_003Ed__8(0);
			}
			_003CGetInfoCardHyperlinks_003Ed__.statRequest = _003C_003E3__statRequest;
			return _003CGetInfoCardHyperlinks_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Dialog_InfoCard.Hyperlink>)this).GetEnumerator();
		}
	}

	public override bool IsDisabledFor(Thing thing)
	{
		if (!base.IsDisabledFor(thing))
		{
			return StatDefOf.MeleeHitChance.Worker.IsDisabledFor(thing);
		}
		return true;
	}

	public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
	{
		if (req.Thing == null)
		{
			Log.Error("Getting MeleeDPS stat for " + req.Def?.ToString() + " without concrete pawn. This always returns 0.");
		}
		return GetMeleeDamage(req, applyPostProcess) * GetMeleeHitChance(req, applyPostProcess) / GetMeleeCooldown(req, applyPostProcess);
	}

	public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("StatsReport_MeleeDPSExplanation".Translate());
		stringBuilder.AppendLine("StatsReport_MeleeDamage".Translate() + " (" + "AverageOfAllAttacks".Translate() + ")");
		stringBuilder.AppendLine("  " + GetMeleeDamage(req).ToString("0.##"));
		stringBuilder.AppendLine("StatsReport_Cooldown".Translate() + " (" + "AverageOfAllAttacks".Translate() + ")");
		stringBuilder.AppendLine("  " + "StatsReport_CooldownFormat".Translate(GetMeleeCooldown(req).ToString("0.##")));
		stringBuilder.AppendLine("StatsReport_MeleeHitChance".Translate());
		stringBuilder.AppendLine(StatDefOf.MeleeHitChance.Worker.GetExplanationUnfinalized(req, StatDefOf.MeleeHitChance.toStringNumberSense).TrimEndNewlines().Indented());
		stringBuilder.Append(StatDefOf.MeleeHitChance.Worker.GetExplanationFinalizePart(req, StatDefOf.MeleeHitChance.toStringNumberSense, GetMeleeHitChance(req)).Indented());
		return stringBuilder.ToString();
	}

	public override string GetStatDrawEntryLabel(StatDef stat, float value, ToStringNumberSense numberSense, StatRequest optionalReq, bool finalized = true)
	{
		return string.Format("{0} ( {1} x {2} / {3} )", value.ToStringByStyle(stat.toStringStyle, numberSense), GetMeleeDamage(optionalReq).ToString("0.##"), StatDefOf.MeleeHitChance.ValueToString(GetMeleeHitChance(optionalReq)), GetMeleeCooldown(optionalReq).ToString("0.##"));
	}

	private float GetMeleeDamage(StatRequest req, bool applyPostProcess = true)
	{
		if (!(req.Thing is Pawn pawn))
		{
			return 0f;
		}
		List<VerbEntry> updatedAvailableVerbsList = pawn.meleeVerbs.GetUpdatedAvailableVerbsList(terrainTools: false);
		if (updatedAvailableVerbsList.Count == 0)
		{
			return 0f;
		}
		float num = 0f;
		for (int i = 0; i < updatedAvailableVerbsList.Count; i++)
		{
			if (updatedAvailableVerbsList[i].IsMeleeAttack)
			{
				num += updatedAvailableVerbsList[i].GetSelectionWeight(null);
			}
		}
		if (num == 0f)
		{
			return 0f;
		}
		float num2 = 0f;
		for (int j = 0; j < updatedAvailableVerbsList.Count; j++)
		{
			if (updatedAvailableVerbsList[j].IsMeleeAttack)
			{
				num2 += updatedAvailableVerbsList[j].GetSelectionWeight(null) / num * updatedAvailableVerbsList[j].verb.verbProps.AdjustedMeleeDamageAmount(updatedAvailableVerbsList[j].verb, pawn);
			}
		}
		return num2;
	}

	private float GetMeleeHitChance(StatRequest req, bool applyPostProcess = true)
	{
		if (req.HasThing)
		{
			return req.Thing.GetStatValue(StatDefOf.MeleeHitChance, applyPostProcess);
		}
		return req.BuildableDef.GetStatValueAbstract(StatDefOf.MeleeHitChance);
	}

	private float GetMeleeCooldown(StatRequest req, bool applyPostProcess = true)
	{
		if (!(req.Thing is Pawn pawn))
		{
			return 1f;
		}
		List<VerbEntry> updatedAvailableVerbsList = pawn.meleeVerbs.GetUpdatedAvailableVerbsList(terrainTools: false);
		if (updatedAvailableVerbsList.Count == 0)
		{
			return 1f;
		}
		float num = 0f;
		for (int i = 0; i < updatedAvailableVerbsList.Count; i++)
		{
			if (updatedAvailableVerbsList[i].IsMeleeAttack)
			{
				num += updatedAvailableVerbsList[i].GetSelectionWeight(null);
			}
		}
		if (num == 0f)
		{
			return 1f;
		}
		float num2 = 0f;
		for (int j = 0; j < updatedAvailableVerbsList.Count; j++)
		{
			if (updatedAvailableVerbsList[j].IsMeleeAttack)
			{
				num2 += updatedAvailableVerbsList[j].GetSelectionWeight(null) / num * (float)updatedAvailableVerbsList[j].verb.verbProps.AdjustedCooldownTicks(updatedAvailableVerbsList[j].verb, pawn);
			}
		}
		return num2 / 60f;
	}

	public override bool ShouldShowFor(StatRequest req)
	{
		if (base.ShouldShowFor(req))
		{
			return req.Thing is Pawn;
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CGetInfoCardHyperlinks_003Ed__8))]
	public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest statRequest)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetInfoCardHyperlinks_003Ed__8(-2)
		{
			_003C_003E3__statRequest = statRequest
		};
	}
}
