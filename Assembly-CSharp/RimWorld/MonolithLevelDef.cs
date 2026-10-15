using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class MonolithLevelDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__41 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public MonolithLevelDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

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
		public _003CConfigErrors_003Ed__41(int _003C_003E1__state)
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
				MonolithLevelDef monolithLevelDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)monolithLevelDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_006f;
				case 1:
					_003C_003E1__state = -3;
					goto IL_006f;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_006f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (monolithLevelDef.entityCatagoryCompletionRequired != null && monolithLevelDef.entityCountCompletionRequired <= 0)
					{
						_003C_003E2__current = "entityCatagoryCompletionRequired is set but entityCountCompletionRequired is not";
						_003C_003E1__state = 2;
						return true;
					}
					break;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__41 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__41(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public int level;

	public int graphicIndex;

	public bool monolithGlows;

	public int anomalyThreatTier;

	public bool useInactiveAnomalyThreatFraction;

	public bool useActiveAnomalyThreatFraction;

	public float anomalyThreatFractionFactor = 1f;

	public float anomalyThreatFraction;

	public List<IncidentDef> incidentsOnReached;

	public bool triggersGrayPall;

	public List<GameConditionDef> unreachableDuringConditions;

	public bool advanceThroughActivation;

	public int desiredHarbingerTreeCount;

	public bool postEndgame;

	public int monolithGlowRadiusOverride = -1;

	public KnowledgeCategoryDef monolithStudyCategory;

	public EntityCategoryDef entityCatagoryCompletionRequired;

	public int entityCountCompletionRequired;

	public float anomalyMentalBreakChance;

	public SoundDef activateSound;

	public SoundDef activatedSound;

	public List<MonolithAttachment> attachments;

	public IntVec2? sizeIncludingAttachments;

	public string uiIconPath;

	[MustTranslate]
	public string monolithLabel;

	[MustTranslate]
	public string monolithDescription;

	[MustTranslate]
	public string levelInspectText;

	[MustTranslate]
	public string extraQuestDescription;

	[MustTranslate]
	public string activateGizmoText;

	[MustTranslate]
	public string activateFloatMenuText;

	[MustTranslate]
	public string activateGizmoDescription;

	[MustTranslate]
	public string pawnSentToActivateMessage;

	[MustTranslate]
	public string monolithCanBeActivatedText;

	[MustTranslate]
	public string activateQuestText;

	[MustTranslate]
	public string activatableLetterLabel;

	[MustTranslate]
	public string activatableLetterText;

	[MustTranslate]
	public string activatedLetterText;

	private Texture2D uiIcon;

	public Texture2D UIIcon => uiIcon;

	public override void PostLoad()
	{
		LongEventHandler.ExecuteWhenFinished(delegate
		{
			if (!uiIconPath.NullOrEmpty())
			{
				uiIcon = ContentFinder<Texture2D>.Get(uiIconPath);
			}
		});
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__41))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__41(-2)
		{
			_003C_003E4__this = this
		};
	}
}
