using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld;

public static class PawnRelationUtility
{
	[CompilerGenerated]
	private sealed class _003CGetRelations_003Ed__0 : IEnumerable<PawnRelationDef>, IEnumerable, IEnumerator<PawnRelationDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private PawnRelationDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn me;

		public Pawn _003C_003E3__me;

		private Pawn other;

		public Pawn _003C_003E3__other;

		private bool _003CanyNonKinFamilyByBloodRelation_003E5__2;

		private List<PawnRelationDef> _003Cdefs_003E5__3;

		private int _003Ci_003E5__4;

		private int _003Ccount_003E5__5;

		PawnRelationDef IEnumerator<PawnRelationDef>.Current
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
		public _003CGetRelations_003Ed__0(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || (uint)(num - 1) <= 1u)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003Cdefs_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (me == other)
					{
						return false;
					}
					if (!me.RaceProps.IsFlesh || !other.RaceProps.IsFlesh)
					{
						return false;
					}
					if (!me.relations.RelatedToAnyoneOrAnyoneRelatedToMe || !other.relations.RelatedToAnyoneOrAnyoneRelatedToMe)
					{
						return false;
					}
					_003C_003E1__state = -3;
					_003CanyNonKinFamilyByBloodRelation_003E5__2 = false;
					_003Cdefs_003E5__3 = DefDatabase<PawnRelationDef>.AllDefsListForReading;
					_003Ci_003E5__4 = 0;
					_003Ccount_003E5__5 = _003Cdefs_003E5__3.Count;
					goto IL_0135;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0125;
				case 2:
					{
						_003C_003E1__state = -3;
						break;
					}
					IL_0125:
					_003Ci_003E5__4++;
					goto IL_0135;
					IL_0135:
					if (_003Ci_003E5__4 < _003Ccount_003E5__5)
					{
						PawnRelationDef pawnRelationDef = _003Cdefs_003E5__3[_003Ci_003E5__4];
						if (pawnRelationDef != PawnRelationDefOf.Kin && pawnRelationDef.Worker.InRelation(me, other))
						{
							if (pawnRelationDef.familyByBloodRelation)
							{
								_003CanyNonKinFamilyByBloodRelation_003E5__2 = true;
							}
							_003C_003E2__current = pawnRelationDef;
							_003C_003E1__state = 1;
							return true;
						}
						goto IL_0125;
					}
					if (!_003CanyNonKinFamilyByBloodRelation_003E5__2 && PawnRelationDefOf.Kin.Worker.InRelation(me, other))
					{
						_003C_003E2__current = PawnRelationDefOf.Kin;
						_003C_003E1__state = 2;
						return true;
					}
					break;
				}
				_003Cdefs_003E5__3 = null;
				_003C_003Em__Finally1();
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
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<PawnRelationDef> IEnumerable<PawnRelationDef>.GetEnumerator()
		{
			_003CGetRelations_003Ed__0 _003CGetRelations_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetRelations_003Ed__ = this;
			}
			else
			{
				_003CGetRelations_003Ed__ = new _003CGetRelations_003Ed__0(0);
			}
			_003CGetRelations_003Ed__.me = _003C_003E3__me;
			_003CGetRelations_003Ed__.other = _003C_003E3__other;
			return _003CGetRelations_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<PawnRelationDef>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CGetRelations_003Ed__0))]
	public static IEnumerable<PawnRelationDef> GetRelations(this Pawn me, Pawn other)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetRelations_003Ed__0(-2)
		{
			_003C_003E3__me = me,
			_003C_003E3__other = other
		};
	}

	public static PawnRelationDef GetMostImportantRelation(this Pawn me, Pawn other)
	{
		PawnRelationDef pawnRelationDef = null;
		foreach (PawnRelationDef relation in me.GetRelations(other))
		{
			if (pawnRelationDef == null || relation.importance > pawnRelationDef.importance)
			{
				pawnRelationDef = relation;
			}
		}
		return pawnRelationDef;
	}

	public static void Notify_PawnsSeenByPlayer(IEnumerable<Pawn> seenPawns, out string pawnRelationsInfo, bool informEvenIfSeenBefore = false, bool writeSeenPawnsNames = true)
	{
		StringBuilder stringBuilder = new StringBuilder();
		List<Pawn> list = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners.Where((Pawn x) => x.relations.everSeenByPlayer).ToList();
		bool flag = false;
		foreach (Pawn seenPawn in seenPawns)
		{
			if (!seenPawn.RaceProps.IsFlesh || (!informEvenIfSeenBefore && seenPawn.relations.everSeenByPlayer) || (seenPawn.Spawned && seenPawn.Fogged()))
			{
				continue;
			}
			seenPawn.relations.everSeenByPlayer = true;
			bool flag2 = false;
			foreach (Pawn item in list)
			{
				if (seenPawn == item)
				{
					continue;
				}
				PawnRelationDef mostImportantRelation = item.GetMostImportantRelation(seenPawn);
				if (mostImportantRelation == null)
				{
					continue;
				}
				if (!flag2)
				{
					flag2 = true;
					if (flag)
					{
						stringBuilder.AppendLine();
					}
					if (writeSeenPawnsNames)
					{
						stringBuilder.AppendLine(seenPawn.KindLabel.CapitalizeFirst() + " " + seenPawn.Name.ToStringShort.Colorize(ColoredText.NameColor) + ":");
					}
				}
				flag = true;
				stringBuilder.AppendLine("  - " + "Relationship".Translate(mostImportantRelation.GetGenderSpecificLabelCap(seenPawn), item.KindLabel + " " + item.NameShortColored.Resolve(), item));
			}
		}
		if (flag)
		{
			pawnRelationsInfo = stringBuilder.ToString().TrimEndNewlines();
		}
		else
		{
			pawnRelationsInfo = null;
		}
	}

	public static void Notify_PawnsSeenByPlayer_Letter(IEnumerable<Pawn> seenPawns, ref TaggedString letterLabel, ref TaggedString letterText, string relationsInfoHeader, bool informEvenIfSeenBefore = false, bool writeSeenPawnsNames = true)
	{
		Notify_PawnsSeenByPlayer(seenPawns, out var pawnRelationsInfo, informEvenIfSeenBefore, writeSeenPawnsNames);
		if (!pawnRelationsInfo.NullOrEmpty())
		{
			if (letterLabel.NullOrEmpty())
			{
				letterLabel = "LetterLabelNoticedRelatedPawns".Translate();
			}
			else
			{
				letterLabel += ": " + "RelationshipAppendedLetterSuffix".Translate().CapitalizeFirst();
			}
			if (!letterText.NullOrEmpty())
			{
				letterText += "\n\n";
			}
			letterText += relationsInfoHeader + "\n\n" + pawnRelationsInfo;
		}
	}

	public static void Notify_PawnsSeenByPlayer_Letter_Send(IEnumerable<Pawn> seenPawns, string relationsInfoHeader, LetterDef letterDef, bool informEvenIfSeenBefore = false, bool writeSeenPawnsNames = true)
	{
		TaggedString letterLabel = "";
		TaggedString letterText = "";
		seenPawns = seenPawns.ToList();
		Notify_PawnsSeenByPlayer_Letter(seenPawns, ref letterLabel, ref letterText, relationsInfoHeader, informEvenIfSeenBefore, writeSeenPawnsNames);
		if (letterText.NullOrEmpty())
		{
			return;
		}
		Pawn pawn = null;
		foreach (Pawn seenPawn in seenPawns)
		{
			if (GetMostImportantColonyRelative(seenPawn) != null)
			{
				pawn = seenPawn;
				break;
			}
		}
		if (pawn == null)
		{
			pawn = seenPawns.FirstOrDefault();
		}
		Find.LetterStack.ReceiveLetter(letterLabel, letterText, letterDef, pawn);
	}

	public static bool TryAppendRelationsWithColonistsInfo(ref TaggedString text, Pawn pawn)
	{
		TaggedString title = null;
		return TryAppendRelationsWithColonistsInfo(ref text, ref title, pawn);
	}

	public static bool TryAppendRelationsWithColonistsInfo(ref TaggedString text, ref TaggedString title, Pawn pawn)
	{
		Pawn mostImportantColonyRelative = GetMostImportantColonyRelative(pawn);
		if (mostImportantColonyRelative == null)
		{
			return false;
		}
		if ((string)title != null)
		{
			title += " (" + "RelationshipAppendedLetterSuffix".Translate() + ")";
		}
		string text2 = mostImportantColonyRelative.GetMostImportantRelation(pawn)?.GetGenderSpecificLabel(pawn);
		if (text2 == null)
		{
			return false;
		}
		if (mostImportantColonyRelative.IsColonist)
		{
			text += "\n\n" + "RelationshipAppendedLetterTextColonist".Translate(mostImportantColonyRelative.LabelShort, text2, mostImportantColonyRelative.Named("RELATIVE"), pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", addRelationInfoSymbol: false);
		}
		else
		{
			text += "\n\n" + "RelationshipAppendedLetterTextPrisoner".Translate(mostImportantColonyRelative.LabelShort, text2, mostImportantColonyRelative.Named("RELATIVE"), pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", addRelationInfoSymbol: false);
		}
		return true;
	}

	public static Pawn GetMostImportantColonyRelative(Pawn pawn)
	{
		if (pawn.relations == null || !pawn.relations.RelatedToAnyoneOrAnyoneRelatedToMe)
		{
			return null;
		}
		IEnumerable<Pawn> enumerable = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners.Where((Pawn x) => x.relations.everSeenByPlayer);
		float num = 0f;
		Pawn pawn2 = null;
		foreach (Pawn item in enumerable)
		{
			PawnRelationDef mostImportantRelation = pawn.GetMostImportantRelation(item);
			if (mostImportantRelation != null && (pawn2 == null || mostImportantRelation.importance > num))
			{
				num = mostImportantRelation.importance;
				pawn2 = item;
			}
		}
		return pawn2;
	}

	public static float MaxPossibleBioAgeAt(float myBiologicalAge, float myChronologicalAge, float atChronologicalAge)
	{
		float num = Mathf.Min(myBiologicalAge, myChronologicalAge - atChronologicalAge);
		if (num < 0f)
		{
			return -1f;
		}
		return num;
	}

	public static float MinPossibleBioAgeAt(float myBiologicalAge, float atChronologicalAge)
	{
		return Mathf.Max(myBiologicalAge - atChronologicalAge, 0f);
	}
}
