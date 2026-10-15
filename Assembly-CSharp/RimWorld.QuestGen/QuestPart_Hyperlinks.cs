using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.QuestGen;

public class QuestPart_Hyperlinks : QuestPart
{
	[CompilerGenerated]
	private sealed class _003CGetHyperlinks_003Ed__7 : IEnumerable<Dialog_InfoCard.Hyperlink>, IEnumerable, IEnumerator<Dialog_InfoCard.Hyperlink>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Dialog_InfoCard.Hyperlink _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_Hyperlinks _003C_003E4__this;

		private int _003Ci_003E5__2;

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
		public _003CGetHyperlinks_003Ed__7(int _003C_003E1__state)
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
			QuestPart_Hyperlinks questPart_Hyperlinks = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (questPart_Hyperlinks.thingDefs != null)
				{
					_003Ci_003E5__2 = 0;
					goto IL_007f;
				}
				goto IL_0092;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				goto IL_007f;
			case 2:
				_003C_003E1__state = -1;
				goto IL_012a;
			case 3:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				goto IL_019f;
			case 4:
				{
					_003C_003E1__state = -1;
					_003Ci_003E5__2++;
					goto IL_01ff;
				}
				IL_01b2:
				if (questPart_Hyperlinks.researchProjects == null)
				{
					break;
				}
				_003Ci_003E5__2 = 0;
				goto IL_01ff;
				IL_007f:
				if (_003Ci_003E5__2 < questPart_Hyperlinks.thingDefs.Count)
				{
					_003C_003E2__current = new Dialog_InfoCard.Hyperlink(questPart_Hyperlinks.thingDefs[_003Ci_003E5__2]);
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0092;
				IL_012a:
				_003Ci_003E5__2++;
				goto IL_013a;
				IL_0092:
				if (questPart_Hyperlinks.pawns != null)
				{
					_003Ci_003E5__2 = 0;
					goto IL_013a;
				}
				goto IL_0150;
				IL_0150:
				if (questPart_Hyperlinks.factions != null)
				{
					_003Ci_003E5__2 = 0;
					goto IL_019f;
				}
				goto IL_01b2;
				IL_013a:
				if (_003Ci_003E5__2 < questPart_Hyperlinks.pawns.Count)
				{
					if (questPart_Hyperlinks.pawns[_003Ci_003E5__2].royalty != null && questPart_Hyperlinks.pawns[_003Ci_003E5__2].royalty.AllTitlesForReading.Any())
					{
						RoyalTitle mostSeniorTitle = questPart_Hyperlinks.pawns[_003Ci_003E5__2].royalty.MostSeniorTitle;
						if (mostSeniorTitle != null)
						{
							_003C_003E2__current = new Dialog_InfoCard.Hyperlink(mostSeniorTitle.def, mostSeniorTitle.faction);
							_003C_003E1__state = 2;
							return true;
						}
					}
					goto IL_012a;
				}
				goto IL_0150;
				IL_019f:
				if (_003Ci_003E5__2 < questPart_Hyperlinks.factions.Count)
				{
					_003C_003E2__current = new Dialog_InfoCard.Hyperlink(questPart_Hyperlinks.factions[_003Ci_003E5__2]);
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_01b2;
				IL_01ff:
				if (_003Ci_003E5__2 < questPart_Hyperlinks.researchProjects.Count)
				{
					_003C_003E2__current = new Dialog_InfoCard.Hyperlink(questPart_Hyperlinks.researchProjects[_003Ci_003E5__2]);
					_003C_003E1__state = 4;
					return true;
				}
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
			_003CGetHyperlinks_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetHyperlinks_003Ed__7(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Dialog_InfoCard.Hyperlink>)this).GetEnumerator();
		}
	}

	public List<ThingDef> thingDefs = new List<ThingDef>();

	public List<Pawn> pawns = new List<Pawn>();

	public List<Faction> factions = new List<Faction>();

	public List<ResearchProjectDef> researchProjects = new List<ResearchProjectDef>();

	private IEnumerable<Dialog_InfoCard.Hyperlink> cachedHyperlinks;

	public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
	{
		get
		{
			if (cachedHyperlinks == null)
			{
				cachedHyperlinks = GetHyperlinks();
			}
			return cachedHyperlinks;
		}
	}

	[IteratorStateMachine(typeof(_003CGetHyperlinks_003Ed__7))]
	private IEnumerable<Dialog_InfoCard.Hyperlink> GetHyperlinks()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetHyperlinks_003Ed__7(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Collections.Look(ref thingDefs, "thingDefs", LookMode.Undefined);
		Scribe_Collections.Look(ref pawns, "pawns", LookMode.Reference);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			if (thingDefs == null)
			{
				thingDefs = new List<ThingDef>();
			}
			thingDefs.RemoveAll((ThingDef x) => x == null);
			if (pawns == null)
			{
				pawns = new List<Pawn>();
			}
			pawns.RemoveAll((Pawn x) => x == null);
		}
	}

	public override void ReplacePawnReferences(Pawn replace, Pawn with)
	{
		pawns.Replace(replace, with);
	}
}
