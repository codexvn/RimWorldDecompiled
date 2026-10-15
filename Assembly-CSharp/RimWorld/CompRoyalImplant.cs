using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld;

public class CompRoyalImplant : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__2 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompRoyalImplant _003C_003E4__this;

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
			CompRoyalImplant compRoyalImplant = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				Pair<Faction, RoyalTitleDef> minTitleForImplantAllFactions = Faction.GetMinTitleForImplantAllFactions(compRoyalImplant.Props.implantHediff);
				if (minTitleForImplantAllFactions.First == null)
				{
					break;
				}
				Faction first = minTitleForImplantAllFactions.First;
				StringBuilder stringBuilder = new StringBuilder("Stat_Thing_MinimumRoyalTitle_Desc".Translate(first.Named("FACTION")));
				if (typeof(Hediff_Level).IsAssignableFrom(compRoyalImplant.Props.implantHediff.hediffClass))
				{
					stringBuilder.Append("\n\n" + "Stat_Thing_MinimumRoyalTitle_Level_Desc".Translate(first.Named("FACTION")) + ":\n\n");
					for (int i = 1; (float)i <= compRoyalImplant.Props.implantHediff.maxSeverity; i++)
					{
						stringBuilder.Append(" -  x" + i + ", " + first.GetMinTitleForImplant(compRoyalImplant.Props.implantHediff, i).GetLabelCapForBothGenders() + "\n");
					}
				}
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsNonPawnImportant, "Stat_Thing_MinimumRoyalTitle_Name".Translate(first.Named("FACTION")).Resolve(), minTitleForImplantAllFactions.Second.GetLabelCapForBothGenders(), stringBuilder.ToTaggedString().Resolve(), 2100);
				_003C_003E1__state = 1;
				return true;
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

	public CompProperties_RoyalImplant Props => (CompProperties_RoyalImplant)props;

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__2))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__2(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string CompInspectStringExtra()
	{
		StringBuilder stringBuilder = new StringBuilder();
		Pair<Faction, RoyalTitleDef> minTitleForImplantAllFactions = Faction.GetMinTitleForImplantAllFactions(Props.implantHediff);
		if (minTitleForImplantAllFactions.First != null)
		{
			stringBuilder.AppendLine("MinimumRoyalTitleInspectString".Translate(minTitleForImplantAllFactions.First.Named("FACTION"), minTitleForImplantAllFactions.Second.GetLabelCapForBothGenders().Named("TITLE")).Resolve());
		}
		if (stringBuilder.Length <= 0)
		{
			return null;
		}
		return stringBuilder.ToString().TrimEndNewlines();
	}

	public static TaggedString CheckForViolations(Pawn pawn, HediffDef hediff, int levelOffset)
	{
		if (levelOffset < 0)
		{
			return "";
		}
		if (pawn.Faction != Faction.OfPlayer || !hediff.HasComp(typeof(HediffComp_RoyalImplant)))
		{
			return "";
		}
		Hediff_Level hediff_Level = (Hediff_Level)pawn.health.hediffSet.hediffs.FirstOrDefault((Hediff h) => h.def == hediff);
		int num = ((levelOffset != 0 && hediff_Level != null) ? (hediff_Level.level + levelOffset) : 0);
		foreach (Faction item in Find.FactionManager.AllFactionsListForReading)
		{
			if (pawn.Faction != null && !item.Hidden && !item.HostileTo(Faction.OfPlayer) && ThingRequiringRoyalPermissionUtility.IsViolatingRulesOf(hediff, pawn, item, num))
			{
				RoyalTitleDef minTitleForImplant = item.GetMinTitleForImplant(hediff, num);
				HediffCompProperties_RoyalImplant hediffCompProperties_RoyalImplant = hediff.CompProps<HediffCompProperties_RoyalImplant>();
				string arg = hediff.label + ((num == 0) ? "" : (" (" + num + "x)"));
				TaggedString taggedString = hediffCompProperties_RoyalImplant.violationTriggerDescriptionKey.Translate(pawn.Named("PAWN"));
				TaggedString taggedString2 = "RoyalImplantIllegalUseWarning".Translate(pawn.Named("PAWN"), arg.Named("IMPLANT"), item.Named("FACTION"), minTitleForImplant.GetLabelCapFor(pawn).Named("TITLE"), taggedString.Named("VIOLATIONTRIGGER"));
				if (levelOffset != 0)
				{
					return taggedString2 + ("\n\n" + "RoyalImplantUpgradeConfirmation".Translate());
				}
				return taggedString2 + ("\n\n" + "RoyalImplantInstallConfirmation".Translate());
			}
		}
		return "";
	}
}
