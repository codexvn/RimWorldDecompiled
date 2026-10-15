using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld;

public static class QuestUtility
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass125_0
	{
		public Quest linkedQuest;

		internal void _003CGetQuestRelatedGizmos_003Eb__0()
		{
			Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests);
			((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(linkedQuest);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass132_0
	{
		public QuestGiverTag tag;

		public Func<QuestScriptDef, bool> _003C_003E9__0;

		internal bool _003CGetGiverQuests_003Eb__0(QuestScriptDef x)
		{
			return x.givenBy.Contains(tag);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetAllQuestPartsOfType_003Ed__120<T> : IEnumerable<T>, IEnumerable, IEnumerator<T>, IEnumerator, IDisposable where T : class
	{
		private int _003C_003E1__state;

		private T _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private bool ongoingOnly;

		public bool _003C_003E3__ongoingOnly;

		private List<Quest> _003Cquests_003E5__2;

		private int _003Ci_003E5__3;

		private List<QuestPart> _003CpartList_003E5__4;

		private int _003Cj_003E5__5;

		T IEnumerator<T>.Current
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
		public _003CGetAllQuestPartsOfType_003Ed__120(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cquests_003E5__2 = null;
			_003CpartList_003E5__4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_00ba;
			}
			_003C_003E1__state = -1;
			_003Cquests_003E5__2 = Find.QuestManager.QuestsListForReading;
			_003Ci_003E5__3 = 0;
			goto IL_00f4;
			IL_00e4:
			_003Ci_003E5__3++;
			goto IL_00f4;
			IL_00ca:
			if (_003Cj_003E5__5 < _003CpartList_003E5__4.Count)
			{
				if (_003CpartList_003E5__4[_003Cj_003E5__5] is T val)
				{
					_003C_003E2__current = val;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00ba;
			}
			_003CpartList_003E5__4 = null;
			goto IL_00e4;
			IL_00f4:
			if (_003Ci_003E5__3 < _003Cquests_003E5__2.Count)
			{
				if (!ongoingOnly || _003Cquests_003E5__2[_003Ci_003E5__3].State == QuestState.Ongoing)
				{
					_003CpartList_003E5__4 = _003Cquests_003E5__2[_003Ci_003E5__3].PartsListForReading;
					_003Cj_003E5__5 = 0;
					goto IL_00ca;
				}
				goto IL_00e4;
			}
			return false;
			IL_00ba:
			_003Cj_003E5__5++;
			goto IL_00ca;
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
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			_003CGetAllQuestPartsOfType_003Ed__120<T> _003CGetAllQuestPartsOfType_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetAllQuestPartsOfType_003Ed__ = this;
			}
			else
			{
				_003CGetAllQuestPartsOfType_003Ed__ = new _003CGetAllQuestPartsOfType_003Ed__120<T>(0);
			}
			_003CGetAllQuestPartsOfType_003Ed__.ongoingOnly = _003C_003E3__ongoingOnly;
			return _003CGetAllQuestPartsOfType_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGiverQuests_003Ed__132 : IEnumerable<QuestScriptDef>, IEnumerable, IEnumerator<QuestScriptDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private QuestScriptDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private QuestGiverTag tag;

		public QuestGiverTag _003C_003E3__tag;

		private IEnumerator<QuestScriptDef> _003C_003E7__wrap1;

		QuestScriptDef IEnumerator<QuestScriptDef>.Current
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
		public _003CGetGiverQuests_003Ed__132(int _003C_003E1__state)
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
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					_003C_003Ec__DisplayClass132_0 CS_0024_003C_003E8__locals1 = new _003C_003Ec__DisplayClass132_0
					{
						tag = tag
					};
					if (!ModsConfig.OdysseyActive)
					{
						return false;
					}
					_003C_003E7__wrap1 = DefDatabase<QuestScriptDef>.AllDefs.Where((QuestScriptDef x) => x.givenBy.Contains(CS_0024_003C_003E8__locals1.tag)).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					QuestScriptDef current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
		IEnumerator<QuestScriptDef> IEnumerable<QuestScriptDef>.GetEnumerator()
		{
			_003CGetGiverQuests_003Ed__132 _003CGetGiverQuests_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetGiverQuests_003Ed__ = this;
			}
			else
			{
				_003CGetGiverQuests_003Ed__ = new _003CGetGiverQuests_003Ed__132(0);
			}
			_003CGetGiverQuests_003Ed__.tag = _003C_003E3__tag;
			return _003CGetGiverQuests_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<QuestScriptDef>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetQuestRelatedGizmos_003Ed__125 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Thing thing;

		public Thing _003C_003E3__thing;

		private _003C_003Ec__DisplayClass125_0 _003C_003E8__1;

		private List<Quest> _003Cquests_003E5__2;

		private int _003Ci_003E5__3;

		private List<QuestPart> _003Cparts_003E5__4;

		private int _003Cj_003E5__5;

		private IEnumerator<Gizmo> _003C_003E7__wrap5;

		Gizmo IEnumerator<Gizmo>.Current
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
		public _003CGetQuestRelatedGizmos_003Ed__125(int _003C_003E1__state)
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
			_003C_003E8__1 = null;
			_003Cquests_003E5__2 = null;
			_003Cparts_003E5__4 = null;
			_003C_003E7__wrap5 = null;
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
					if (Find.Selector.SelectedObjects.Count != 1)
					{
						break;
					}
					_003C_003E8__1 = new _003C_003Ec__DisplayClass125_0();
					_003C_003E8__1.linkedQuest = null;
					_003Cquests_003E5__2 = Find.QuestManager.QuestsListForReading;
					_003Ci_003E5__3 = 0;
					goto IL_0223;
				case 1:
					_003C_003E1__state = -3;
					goto IL_01c8;
				case 2:
					{
						_003C_003E1__state = -1;
						goto IL_02cf;
					}
					IL_01f4:
					if (_003Cj_003E5__5 < _003Cparts_003E5__4.Count)
					{
						if (_003Cparts_003E5__4[_003Cj_003E5__5] is QuestPartActivable { State: QuestPartState.Enabled } questPartActivable)
						{
							IEnumerable<Gizmo> enumerable = questPartActivable.ExtraGizmos(thing);
							if (enumerable != null)
							{
								_003C_003E7__wrap5 = enumerable.GetEnumerator();
								_003C_003E1__state = -3;
								goto IL_01c8;
							}
						}
						goto IL_01e2;
					}
					_003Cparts_003E5__4 = null;
					goto IL_0211;
					IL_0223:
					if (_003Ci_003E5__3 < _003Cquests_003E5__2.Count)
					{
						if (!_003Cquests_003E5__2[_003Ci_003E5__3].hidden && !_003Cquests_003E5__2[_003Ci_003E5__3].Historical && !_003Cquests_003E5__2[_003Ci_003E5__3].dismissed)
						{
							if (_003Cquests_003E5__2[_003Ci_003E5__3].QuestLookTargets.Contains(thing) || _003Cquests_003E5__2[_003Ci_003E5__3].QuestSelectTargets.Contains(thing))
							{
								_003C_003E8__1.linkedQuest = _003Cquests_003E5__2[_003Ci_003E5__3];
							}
							_003Cparts_003E5__4 = _003Cquests_003E5__2[_003Ci_003E5__3].PartsListForReading;
							_003Cj_003E5__5 = 0;
							goto IL_01f4;
						}
						goto IL_0211;
					}
					if (_003C_003E8__1.linkedQuest != null)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "CommandOpenLinkedQuest".Translate(_003C_003E8__1.linkedQuest.name),
							defaultDesc = "CommandOpenLinkedQuestDesc".Translate(),
							icon = TexCommand.OpenLinkedQuestTex,
							action = delegate
							{
								Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests);
								((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(_003C_003E8__1.linkedQuest);
							}
						};
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_02cf;
					IL_01e2:
					_003Cj_003E5__5++;
					goto IL_01f4;
					IL_0211:
					_003Ci_003E5__3++;
					goto IL_0223;
					IL_02cf:
					_003C_003E8__1 = null;
					_003Cquests_003E5__2 = null;
					break;
					IL_01c8:
					if (_003C_003E7__wrap5.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap5.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap5 = null;
					goto IL_01e2;
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
			if (_003C_003E7__wrap5 != null)
			{
				_003C_003E7__wrap5.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetQuestRelatedGizmos_003Ed__125 _003CGetQuestRelatedGizmos_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetQuestRelatedGizmos_003Ed__ = this;
			}
			else
			{
				_003CGetQuestRelatedGizmos_003Ed__ = new _003CGetQuestRelatedGizmos_003Ed__125(0);
			}
			_003CGetQuestRelatedGizmos_003Ed__.thing = _003C_003E3__thing;
			return _003CGetQuestRelatedGizmos_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetSubquests_003Ed__128 : IEnumerable<Quest>, IEnumerable, IEnumerator<Quest>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Quest _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Quest quest;

		public Quest _003C_003E3__quest;

		private QuestState? state;

		public QuestState? _003C_003E3__state;

		private List<Quest> _003CallQuests_003E5__2;

		private int _003Ci_003E5__3;

		Quest IEnumerator<Quest>.Current
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
		public _003CGetSubquests_003Ed__128(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CallQuests_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_00b8;
			}
			_003C_003E1__state = -1;
			_003CallQuests_003E5__2 = Find.QuestManager.questsInDisplayOrder;
			_003Ci_003E5__3 = 0;
			goto IL_00c8;
			IL_00b8:
			_003Ci_003E5__3++;
			goto IL_00c8;
			IL_00c8:
			if (_003Ci_003E5__3 < _003CallQuests_003E5__2.Count)
			{
				if (_003CallQuests_003E5__2[_003Ci_003E5__3].parent == quest && (!state.HasValue || _003CallQuests_003E5__2[_003Ci_003E5__3].State == state))
				{
					_003C_003E2__current = _003CallQuests_003E5__2[_003Ci_003E5__3];
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00b8;
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
		IEnumerator<Quest> IEnumerable<Quest>.GetEnumerator()
		{
			_003CGetSubquests_003Ed__128 _003CGetSubquests_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetSubquests_003Ed__ = this;
			}
			else
			{
				_003CGetSubquests_003Ed__ = new _003CGetSubquests_003Ed__128(0);
			}
			_003CGetSubquests_003Ed__.quest = _003C_003E3__quest;
			_003CGetSubquests_003Ed__.state = _003C_003E3__state;
			return _003CGetSubquests_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Quest>)this).GetEnumerator();
		}
	}

	public const string QuestTargetSignalPart_MapGenerated = "MapGenerated";

	public const string QuestTargetSignalPart_MapRemoved = "MapRemoved";

	public const string QuestTargetSignalPart_MapSettled = "MapSettled";

	public const string QuestTargetSignalPart_Spawned = "Spawned";

	public const string QuestTargetSignalPart_Despawned = "Despawned";

	public const string QuestTargetSignalPart_Destroyed = "Destroyed";

	public const string QuestTargetSignalPart_Killed = "Killed";

	public const string QuestTargetSignalPart_TookDamage = "TookDamage";

	public const string QuestTargetSignalPart_TookDamageFromPlayer = "TookDamageFromPlayer";

	public const string QuestTargetSignalPart_ChangedFaction = "ChangedFaction";

	public const string QuestTargetSignalPart_ChangedFactionToPlayer = "ChangedFactionToPlayer";

	public const string QuestTargetSignalPart_ChangedFactionToNonPlayer = "ChangedFactionToNonPlayer";

	public const string QuestTargetSignalPart_Hacked = "Hacked";

	public const string QuestTargetSignalPart_HackingStarted = "HackingStarted";

	public const string QuestTargetSignalPart_LockedOut = "LockedOut";

	public const string QuestTargetSignalPart_StartedExtractingFromContainer = "StartedExtractingFromContainer";

	public const string QuestTargetSignalPart_Studied = "Researched";

	public const string QuestTargetSignalPart_KilledLeavingsLeft = "KilledLeavingsLeft";

	public const string QuestTargetSignalPart_Unfogged = "Unfogged";

	public const string QuestTargetSignalPart_Inspected = "Inspected";

	public const string QuestTargetSignalPart_SwappedMap = "SwappedMap";

	public const string QuestTargetSignalPart_LeftBehind = "LeftBehind";

	public const string QuestTargetSignalPart_LeftMap = "LeftMap";

	public const string QuestTargetSignalPart_SurgeryViolation = "SurgeryViolation";

	public const string QuestTargetSignalPart_Arrested = "Arrested";

	public const string QuestTargetSignalPart_Released = "Released";

	public const string QuestTargetSignalPart_Recruited = "Recruited";

	public const string QuestTargetSignalPart_Kidnapped = "Kidnapped";

	public const string QuestTargetSignalPart_ChangedHostFaction = "ChangedHostFaction";

	public const string QuestTargetSignalPart_NoLongerFactionLeader = "NoLongerFactionLeader";

	public const string QuestTargetSignalPart_TitleChanged = "TitleChanged";

	public const string QuestTargetSignalPart_TitleAwardedWhenUpdatingChanged = "TitleAwardedWhenUpdatingChanged";

	public const string QuestTargetSignalPart_Banished = "Banished";

	public const string QuestTargetSignalPart_Rescued = "Rescued";

	public const string QuestTargetSignalPart_RanWild = "RanWild";

	public const string QuestTargetSignalPart_Enslaved = "Enslaved";

	public const string QuestTargetSignalPart_PlayerTended = "PlayerTended";

	public const string QuestTargetSignalPart_XenogermAbsorbed = "XenogermAbsorbed";

	public const string QuestTargetSignalPart_XenogermReimplanted = "XenogermReimplanted";

	public const string QuestTargetSignalPart_BecameMutant = "BecameMutant";

	public const string QuestTargetSignalPart_PsychicRitualTarget = "PsychicRitualTarget";

	public const string QuestTargetSignalPart_ReceivedItems = "ReceivedItems";

	public const string QuestTargetSignalPart_ShuttleSentSatisfied = "SentSatisfied";

	public const string QuestTargetSignalPart_ShuttleSentUnsatisfied = "SentUnsatisfied";

	public const string QuestTargetSignalPart_ShuttleSentWithExtraColonists = "SentWithExtraColonists";

	public const string QuestTargetSignalPart_ShuttleUnloaded = "Unloaded";

	public const string QuestTargetSignalPart_AllEnemiesDefeated = "AllEnemiesDefeated";

	public const string QuestTargetSignalPart_TradeRequestFulfilled = "TradeRequestFulfilled";

	public const string QuestTargetSignalPart_PeaceTalksResolved = "Resolved";

	public const string QuestTargetSignalPart_LaunchedShip = "LaunchedShip";

	public const string QuestTargetSignalPart_ReactorDestroyed = "ReactorDestroyed";

	public const string QuestTargetSignalPart_MonumentCompleted = "MonumentCompleted";

	public const string QuestTargetSignalPart_MonumentDestroyed = "MonumentDestroyed";

	public const string QuestTargetSignalPart_MonumentCancelled = "MonumentCancelled";

	public const string QuestTargetSignalPart_AllHivesDestroyed = "AllHivesDestroyed";

	public const string QuestTargetSignalPart_ExitMentalState = "ExitMentalState";

	public const string QuestTargetSignalPart_FactionBecameHostileToPlayer = "BecameHostileToPlayer";

	public const string QuestTargetSignalPart_FactionBuiltBuilding = "BuiltBuilding";

	public const string QuestTargetSignalPart_FactionPlacedBlueprint = "PlacedBlueprint";

	public const string QuestTargetSignalPart_FactionMemberArrested = "FactionMemberArrested";

	public const string QuestTargetSignalPart_CeremonyExpired = "CeremonyExpired";

	public const string QuestTargetSignalPart_CeremonyFailed = "CeremonyFailed";

	public const string QuestTargetSignalPart_CeremonyDone = "CeremonyDone";

	public const string QuestTargetSignalPart_BeingAttacked = "BeingAttacked";

	public const string QuestTargetSignalPart_Fleeing = "Fleeing";

	public const string QuestTargetSignalPart_QuestEnded = "QuestEnded";

	public const string QuestTargetSignalPart_AllPawnsLost = "AllPawnsLost";

	public const string QuestTargetSignalPart_ShipDisposed = "Disposed";

	public const string QuestTargetSignalPart_ShipArrived = "Arrived";

	public const string QuestTargetSignalPart_ShipFlewAway = "FlewAway";

	public const string QuestTargetSignalPart_ShipThingAdded = "ThingAdded";

	public const string QuestTargetSignalPart_Activated = "Activated";

	public const string QuestTargetSignalPart_NodeClosed = "NodeClosed";

	public const string QuestTargetSignalPart_CoreDefeated = "CoreDefeated";

	public const string QuestTargetSignalPart_NoActiveThreats = "NoActiveThreats";

	private static readonly List<Pawn> tmpPawns = new List<Pawn>();

	private static List<QuestPart_WorkDisabled> tmpQuestWorkDisabled = new List<QuestPart_WorkDisabled>();

	private static List<ExtraFaction> tmpExtraFactions = new List<ExtraFaction>();

	private static List<QuestPart> tmpQuestParts = new List<QuestPart>();

	public static Quest GenerateQuestAndMakeAvailable(QuestScriptDef root, float points)
	{
		Slate slate = new Slate();
		slate.Set("points", points);
		return GenerateQuestAndMakeAvailable(root, slate);
	}

	public static Quest GenerateQuestAndMakeAvailable(QuestScriptDef root, Slate vars)
	{
		Quest quest = RimWorld.QuestGen.QuestGen.Generate(root, vars);
		Find.QuestManager.Add(quest);
		return quest;
	}

	public static void SendLetterQuestAvailable(Quest quest, string discoveryMethod = null)
	{
		ChoiceLetter choiceLetter = LetterMaker.MakeLetter(QuestAvailableLetterLabel(quest), QuestAvailableLetterText(quest, discoveryMethod), quest.root?.questAvailableLetterDef ?? IncidentDefOf.GiveQuest_Random.letterDef, LookTargets.Invalid, null, quest);
		choiceLetter.title = quest.name;
		Find.LetterStack.ReceiveLetter(choiceLetter);
	}

	private static TaggedString QuestAvailableLetterLabel(Quest quest)
	{
		if (quest.root != null && !quest.root.questAvailableLetterLabel.NullOrEmpty())
		{
			return quest.root.questAvailableLetterLabel;
		}
		if (quest.initiallyAccepted)
		{
			return "LetterLabelQuestAutomaticallyAcceptedTitle".Translate(quest.name);
		}
		return "LetterLabelQuestAvailableTitle".Translate(quest.name);
	}

	private static TaggedString QuestAvailableLetterText(Quest quest, string discoveryMethod = null)
	{
		TaggedString result;
		if (quest.root != null && quest.root.questAvailableLetterTextIsDescription)
		{
			result = quest.description;
		}
		else
		{
			result = (discoveryMethod.NullOrEmpty() ? "LetterNewQuestFromUnknown".Translate() : "LetterNewQuest".Translate(discoveryMethod.Named("DISCOVEREDFROM")));
			result += "\n\n" + "LetterQuestIsNamed".Translate(quest.name);
		}
		if (quest.initiallyAccepted)
		{
			int questTicksRemaining = GetQuestTicksRemaining(quest);
			if (questTicksRemaining > 0)
			{
				result += "\n\n" + "LetterQuestActiveNowTime".Translate(questTicksRemaining.ToStringTicksToPeriod(allowSeconds: false, shortForm: false, canUseDecimals: false));
			}
		}
		else if (quest.TicksUntilExpiry >= 0)
		{
			result += "\n\n" + "LetterQuestRequiresAcceptance".Translate(quest.TicksUntilExpiry.ToStringTicksToPeriod(allowSeconds: false, shortForm: false, canUseDecimals: false));
		}
		return result;
	}

	public static int GetQuestTicksRemaining(Quest quest)
	{
		foreach (QuestPart item in quest.PartsListForReading)
		{
			if (item is QuestPart_Delay { State: QuestPartState.Enabled, isBad: not false } questPart_Delay && !questPart_Delay.expiryInfoPart.NullOrEmpty())
			{
				return questPart_Delay.TicksLeft;
			}
		}
		return 0;
	}

	public static void GenerateBackCompatibilityNameFor(Quest quest)
	{
		quest.name = NameGenerator.GenerateName(RulePackDefOf.NamerQuestDefault, Find.QuestManager.QuestsListForReading.Select((Quest x) => x.name), appendNumberIfNameUsed: false, "defaultQuestName");
	}

	public static bool CanPawnAcceptQuest(Pawn p, Quest quest)
	{
		for (int i = 0; i < quest.PartsListForReading.Count; i++)
		{
			if (quest.PartsListForReading[i] is QuestPart_RequirementsToAccept questPart_RequirementsToAccept && !questPart_RequirementsToAccept.CanPawnAccept(p))
			{
				return false;
			}
		}
		if (!p.Destroyed && p.IsFreeColonist && !p.Downed && !p.Suspended)
		{
			return !p.IsQuestLodger();
		}
		return false;
	}

	public static AcceptanceReport CanAcceptQuest(Quest quest)
	{
		if (Find.AnyPlayerHomeMap == null)
		{
			return false;
		}
		if (!Current.Game.PlayerHasControl)
		{
			return false;
		}
		for (int i = 0; i < quest.PartsListForReading.Count; i++)
		{
			if (quest.PartsListForReading[i] is QuestPart_RequirementsToAccept questPart_RequirementsToAccept)
			{
				AcceptanceReport result = questPart_RequirementsToAccept.CanAccept();
				if (!result.Accepted)
				{
					return result;
				}
			}
		}
		return true;
	}

	public static Vector2 GetLocForDates()
	{
		if (Find.AnyPlayerHomeMap != null)
		{
			return Find.WorldGrid.LongLatOf(Find.AnyPlayerHomeMap.Tile);
		}
		return default(Vector2);
	}

	public static void SendQuestTargetSignals(List<string> questTags, string signalPart)
	{
		SendQuestTargetSignals(questTags, signalPart, default(SignalArgs));
	}

	public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1)
	{
		SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1));
	}

	public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1, NamedArgument arg2)
	{
		SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1, arg2));
	}

	public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
	{
		SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1, arg2, arg3));
	}

	public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
	{
		SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1, arg2, arg3, arg4));
	}

	public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument[] args)
	{
		SendQuestTargetSignals(questTags, signalPart, new SignalArgs(args));
	}

	public static void SendQuestTargetSignals(List<string> questTags, string signalPart, SignalArgs args)
	{
		if (questTags != null)
		{
			for (int i = 0; i < questTags.Count; i++)
			{
				Find.SignalManager.SendSignal(new Signal(questTags[i] + "." + signalPart, args));
			}
		}
	}

	public static void AddQuestTag(ref List<string> questTags, string questTagToAdd)
	{
		if (questTags == null)
		{
			questTags = new List<string>();
		}
		if (!questTags.Contains(questTagToAdd))
		{
			questTags.Add(questTagToAdd);
		}
	}

	public static void AddQuestTag(object obj, string questTagToAdd)
	{
		if (questTagToAdd.NullOrEmpty())
		{
			return;
		}
		if (obj is Thing thing)
		{
			AddQuestTag(ref thing.questTags, questTagToAdd);
		}
		else if (obj is WorldObject worldObject)
		{
			AddQuestTag(ref worldObject.questTags, questTagToAdd);
		}
		else if (obj is Map map)
		{
			AddQuestTag(ref map.Parent.questTags, questTagToAdd);
		}
		else if (obj is Lord lord)
		{
			AddQuestTag(ref lord.questTags, questTagToAdd);
		}
		else if (obj is Faction faction)
		{
			AddQuestTag(ref faction.questTags, questTagToAdd);
		}
		else if (obj is TransportShip transportShip)
		{
			AddQuestTag(ref transportShip.questTags, questTagToAdd);
		}
		else
		{
			if (!(obj is IEnumerable enumerable))
			{
				return;
			}
			foreach (object item in enumerable)
			{
				if (item is Thing thing2)
				{
					AddQuestTag(ref thing2.questTags, questTagToAdd);
				}
				else if (item is WorldObject worldObject2)
				{
					AddQuestTag(ref worldObject2.questTags, questTagToAdd);
				}
				else if (item is Map map2)
				{
					AddQuestTag(ref map2.Parent.questTags, questTagToAdd);
				}
				else if (item is Faction faction2)
				{
					AddQuestTag(ref faction2.questTags, questTagToAdd);
				}
				else if (enumerable is TransportShip transportShip2)
				{
					AddQuestTag(ref transportShip2.questTags, questTagToAdd);
				}
			}
		}
	}

	public static bool TryGetIdealColonist(out Pawn pawn, Map idealMap = null, Func<Pawn, bool> validator = null)
	{
		tmpPawns.Clear();
		if (idealMap != null)
		{
			for (int i = 0; i <= 3; i++)
			{
				CacheIdealColonists(idealMap.mapPawns.AllHumanlikeSpawned, i, tmpPawns, validator);
				if (!tmpPawns.Empty())
				{
					pawn = tmpPawns.RandomElement();
					tmpPawns.Clear();
					return true;
				}
			}
		}
		for (int j = 0; j <= 3; j++)
		{
			foreach (Map map in Find.Maps)
			{
				if (idealMap == null || idealMap != map)
				{
					CacheIdealColonists(map.mapPawns.AllHumanlikeSpawned, j, tmpPawns, validator);
				}
			}
			if (!tmpPawns.Empty())
			{
				pawn = tmpPawns.RandomElement();
				tmpPawns.Clear();
				return true;
			}
		}
		for (int k = 0; k <= 3; k++)
		{
			CacheIdealColonists(Find.WorldPawns.AllPawnsAlive, k, tmpPawns, validator);
			if (!tmpPawns.Empty())
			{
				pawn = tmpPawns.RandomElement();
				tmpPawns.Clear();
				return true;
			}
		}
		tmpPawns.Clear();
		pawn = null;
		return false;
	}

	private static void CacheIdealColonists(List<Pawn> pawnsToScan, int level, List<Pawn> pawns, Func<Pawn, bool> validator = null)
	{
		foreach (Pawn item in pawnsToScan)
		{
			if (!item.DevelopmentalStage.Baby() && (validator == null || validator(item)) && (item.IsColonistPlayerControlled || (item.IsColonist && item.IsCaravanMember())) && (level > 2 || item.IsSlaveOfColony || item.IsColonist) && (level > 1 || item.ageTracker.AgeBiologicalYears >= 13) && (level > 0 || item.health.capacities.CapableOf(PawnCapacityDefOf.Moving)))
			{
				pawns.Add(item);
			}
		}
	}

	public static bool AnyMatchingTags(List<string> first, List<string> second)
	{
		if (first.NullOrEmpty() || second.NullOrEmpty())
		{
			return false;
		}
		for (int i = 0; i < first.Count; i++)
		{
			for (int j = 0; j < second.Count; j++)
			{
				if (first[i] == second[j])
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool IsReservedByQuestOrQuestBeingGenerated(Pawn pawn)
	{
		if (!Find.QuestManager.IsReservedByAnyQuest(pawn) && (RimWorld.QuestGen.QuestGen.quest == null || !RimWorld.QuestGen.QuestGen.quest.QuestReserves(pawn)))
		{
			return RimWorld.QuestGen.QuestGen.WasGeneratedForQuestBeingGenerated(pawn);
		}
		return true;
	}

	public static List<QuestPart_WorkDisabled> GetWorkDisabledQuestPart(Pawn p)
	{
		tmpQuestWorkDisabled.Clear();
		List<Quest> activeQuestsListForReading = Find.QuestManager.ActiveQuestsListForReading;
		for (int i = 0; i < activeQuestsListForReading.Count; i++)
		{
			if (activeQuestsListForReading[i].State != QuestState.Ongoing)
			{
				continue;
			}
			List<QuestPart> partsListForReading = activeQuestsListForReading[i].PartsListForReading;
			for (int j = 0; j < partsListForReading.Count; j++)
			{
				if (partsListForReading[j] is QuestPart_WorkDisabled questPart_WorkDisabled && questPart_WorkDisabled.pawns.Contains(p))
				{
					tmpQuestWorkDisabled.Add(questPart_WorkDisabled);
				}
			}
		}
		return tmpQuestWorkDisabled;
	}

	public static bool IsQuestLodger(this Pawn p)
	{
		if (!p.HasExtraHomeFaction())
		{
			return p.HasExtraMiniFaction();
		}
		return true;
	}

	public static bool IsQuestHelper(this Pawn p)
	{
		if (!p.IsQuestLodger())
		{
			return false;
		}
		List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
		for (int i = 0; i < questsListForReading.Count; i++)
		{
			if (questsListForReading[i].State != QuestState.Ongoing)
			{
				continue;
			}
			List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
			for (int j = 0; j < partsListForReading.Count; j++)
			{
				if (partsListForReading[j] is QuestPart_ExtraFaction questPart_ExtraFaction && questPart_ExtraFaction.affectedPawns.Contains(p) && questPart_ExtraFaction.areHelpers)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool IsQuestReward(this Pawn pawn)
	{
		List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
		for (int i = 0; i < questsListForReading.Count; i++)
		{
			if ((questsListForReading[i].State == QuestState.NotYetAccepted || questsListForReading[i].State == QuestState.Ongoing) && pawn.IsQuestReward(questsListForReading[i]))
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsQuestReward(this Pawn pawn, Quest quest)
	{
		List<QuestPart> partsListForReading = quest.PartsListForReading;
		for (int i = 0; i < partsListForReading.Count; i++)
		{
			if (!(partsListForReading[i] is QuestPart_Choice questPart_Choice))
			{
				continue;
			}
			for (int j = 0; j < questPart_Choice.choices.Count; j++)
			{
				QuestPart_Choice.Choice choice = questPart_Choice.choices[j];
				for (int k = 0; k < choice.rewards.Count; k++)
				{
					if (choice.rewards[k] is Reward_Pawn reward_Pawn && reward_Pawn.pawn == pawn)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public static bool LodgerAllowedDecrees(this Pawn p)
	{
		List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
		for (int i = 0; i < questsListForReading.Count; i++)
		{
			if (questsListForReading[i].State != QuestState.Ongoing)
			{
				continue;
			}
			List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
			for (int j = 0; j < partsListForReading.Count; j++)
			{
				if (partsListForReading[j] is QuestPart_AllowDecreesForLodger questPart_AllowDecreesForLodger && questPart_AllowDecreesForLodger.lodger == p)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool HasExtraHomeFaction(this Pawn p, Quest forQuest = null)
	{
		return p.GetExtraHomeFaction(forQuest) != null;
	}

	public static bool HasExtraMiniFaction(this Pawn p, Quest forQuest = null)
	{
		return p.GetExtraMiniFaction(forQuest) != null;
	}

	public static bool HasExtraHomeFaction(this Pawn p, Faction faction)
	{
		tmpExtraFactions.Clear();
		GetExtraFactionsFromQuestParts(p, tmpExtraFactions);
		for (int i = 0; i < tmpExtraFactions.Count; i++)
		{
			if (tmpExtraFactions[i].factionType == ExtraFactionType.HomeFaction && tmpExtraFactions[i].faction == faction)
			{
				tmpExtraFactions.Clear();
				return true;
			}
		}
		tmpExtraFactions.Clear();
		return false;
	}

	public static bool HasExtraMiniFaction(this Pawn p, Faction faction)
	{
		tmpExtraFactions.Clear();
		GetExtraFactionsFromQuestParts(p, tmpExtraFactions);
		for (int i = 0; i < tmpExtraFactions.Count; i++)
		{
			if (tmpExtraFactions[i].factionType == ExtraFactionType.MiniFaction && tmpExtraFactions[i].faction == faction)
			{
				tmpExtraFactions.Clear();
				return true;
			}
		}
		tmpExtraFactions.Clear();
		return false;
	}

	public static Faction GetExtraHomeFaction(this Pawn p, Quest forQuest = null)
	{
		return p.GetExtraFaction(ExtraFactionType.HomeFaction, forQuest);
	}

	public static Faction GetExtraHostFaction(this Pawn p, Quest forQuest = null)
	{
		return p.GetExtraFaction(ExtraFactionType.HostFaction, forQuest);
	}

	public static Faction GetExtraMiniFaction(this Pawn p, Quest forQuest = null)
	{
		return p.GetExtraFaction(ExtraFactionType.MiniFaction, forQuest);
	}

	public static bool InSameExtraFaction(this Pawn p, Pawn target, ExtraFactionType type, Quest forQuest = null)
	{
		return p.GetSharedExtraFaction(target, type, forQuest) != null;
	}

	public static Faction GetSharedExtraFaction(this Pawn p, Pawn target, ExtraFactionType type, Quest forQuest = null)
	{
		Faction extraFaction = p.GetExtraFaction(type, forQuest);
		Faction extraFaction2 = target.GetExtraFaction(type, forQuest);
		if (extraFaction != null && extraFaction == extraFaction2)
		{
			return extraFaction;
		}
		return null;
	}

	public static Faction GetExtraFaction(this Pawn p, ExtraFactionType extraFactionType, Quest forQuest = null)
	{
		tmpExtraFactions.Clear();
		GetExtraFactionsFromQuestParts(p, tmpExtraFactions, forQuest);
		for (int i = 0; i < tmpExtraFactions.Count; i++)
		{
			if (tmpExtraFactions[i].factionType == extraFactionType)
			{
				Faction faction = tmpExtraFactions[i].faction;
				tmpExtraFactions.Clear();
				return faction;
			}
		}
		tmpExtraFactions.Clear();
		return null;
	}

	public static void GetExtraFactionsFromQuestParts(Pawn pawn, List<ExtraFaction> outExtraFactions, Quest forQuest = null)
	{
		outExtraFactions.Clear();
		List<QuestPart_ExtraFaction> extraFactionQuestParts = Find.QuestManager.ExtraFactionQuestParts;
		for (int i = 0; i < extraFactionQuestParts.Count; i++)
		{
			if (extraFactionQuestParts[i].extraFaction?.faction != null)
			{
				Quest quest = extraFactionQuestParts[i].quest;
				if ((quest.State == QuestState.Ongoing || quest == forQuest || (quest.root == QuestScriptDefOf.Hospitality_Refugee && !pawn.IsColonist)) && !AlreadyAdded(extraFactionQuestParts[i].extraFaction) && extraFactionQuestParts[i].affectedPawns.Contains(pawn))
				{
					outExtraFactions.Add(extraFactionQuestParts[i].extraFaction);
				}
			}
		}
		bool AlreadyAdded(ExtraFaction extraFaction)
		{
			for (int j = 0; j < outExtraFactions.Count; j++)
			{
				if (outExtraFactions[j].faction == extraFaction.faction && outExtraFactions[j].factionType == extraFaction.factionType)
				{
					return true;
				}
			}
			return false;
		}
	}

	public static bool IsBorrowedByAnyFaction(this Pawn pawn)
	{
		List<Quest> activeQuestsListForReading = Find.QuestManager.ActiveQuestsListForReading;
		int count = activeQuestsListForReading.Count;
		for (int i = 0; i < count; i++)
		{
			if (activeQuestsListForReading[i].State != QuestState.Ongoing)
			{
				continue;
			}
			List<QuestPart> partsListForReading = activeQuestsListForReading[i].PartsListForReading;
			int count2 = partsListForReading.Count;
			for (int j = 0; j < count2; j++)
			{
				if (partsListForReading[j] is QuestPart_LendColonistsToFaction questPart_LendColonistsToFaction && questPart_LendColonistsToFaction.LentColonistsListForReading.Contains(pawn))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static int TotalBorrowedColonistCount()
	{
		List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
		int num = 0;
		for (int i = 0; i < questsListForReading.Count; i++)
		{
			if (questsListForReading[i].State != QuestState.Ongoing)
			{
				continue;
			}
			List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
			for (int j = 0; j < partsListForReading.Count; j++)
			{
				if (partsListForReading[j] is QuestPart_LendColonistsToFaction questPart_LendColonistsToFaction)
				{
					num += questPart_LendColonistsToFaction.LentColonistsListForReading.Count;
				}
			}
		}
		return num;
	}

	[IteratorStateMachine(typeof(_003CGetAllQuestPartsOfType_003Ed__120<>))]
	public static IEnumerable<T> GetAllQuestPartsOfType<T>(bool ongoingOnly = true) where T : class
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetAllQuestPartsOfType_003Ed__120<T>(-2)
		{
			_003C_003E3__ongoingOnly = ongoingOnly
		};
	}

	public static void AppendInspectStringsFromQuestParts(StringBuilder sb, ISelectable target)
	{
		AppendInspectStringsFromQuestParts(sb, target, out var _);
	}

	public static void AppendInspectStringsFromQuestParts(StringBuilder sb, ISelectable target, out int count)
	{
		AppendInspectStringsFromQuestParts(delegate(string str, Quest quest)
		{
			if (sb.Length != 0)
			{
				sb.AppendLine();
			}
			sb.Append(str);
		}, target, out count);
	}

	public static void AppendInspectStringsFromQuestParts(Action<string, Quest> func, ISelectable target, out int count)
	{
		count = 0;
		List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
		for (int i = 0; i < questsListForReading.Count; i++)
		{
			if (questsListForReading[i].State != QuestState.Ongoing)
			{
				continue;
			}
			tmpQuestParts.Clear();
			tmpQuestParts.AddRange(questsListForReading[i].PartsListForReading);
			tmpQuestParts.SortBy((QuestPart x) => (x is QuestPartActivable questPartActivable) ? questPartActivable.EnableTick : 0);
			for (int j = 0; j < tmpQuestParts.Count; j++)
			{
				if (tmpQuestParts[j] is QuestPartActivable { State: QuestPartState.Enabled } questPartActivable2)
				{
					string str = questPartActivable2.ExtraInspectString(target);
					if (!str.NullOrEmpty())
					{
						func(str.Formatted(target.Named("TARGET")), questsListForReading[i]);
						count++;
					}
				}
			}
			tmpQuestParts.Clear();
		}
	}

	[IteratorStateMachine(typeof(_003CGetQuestRelatedGizmos_003Ed__125))]
	public static IEnumerable<Gizmo> GetQuestRelatedGizmos(Thing thing)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetQuestRelatedGizmos_003Ed__125(-2)
		{
			_003C_003E3__thing = thing
		};
	}

	public static Gizmo GetSelectMonumentMarkerGizmo(Thing thing)
	{
		if (!thing.Spawned || !ModsConfig.RoyaltyActive)
		{
			return null;
		}
		List<Thing> list = thing.Map.listerThings.ThingsOfDef(ThingDefOf.MonumentMarker);
		for (int i = 0; i < list.Count; i++)
		{
			MonumentMarker m = (MonumentMarker)list[i];
			if (m.IsPart(thing))
			{
				return new Command_Action
				{
					defaultLabel = "CommandSelectMonumentMarker".Translate(),
					defaultDesc = "CommandSelectMonumentMarkerDesc".Translate(),
					icon = ThingDefOf.MonumentMarker.uiIcon,
					iconAngle = ThingDefOf.MonumentMarker.uiIconAngle,
					iconOffset = ThingDefOf.MonumentMarker.uiIconOffset,
					action = delegate
					{
						CameraJumper.TrySelect(m);
					}
				};
			}
		}
		return null;
	}

	public static bool AnyQuestDisablesRandomMoodCausedMentalBreaksFor(Pawn p)
	{
		List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
		for (int i = 0; i < questsListForReading.Count; i++)
		{
			if (questsListForReading[i].State != QuestState.Ongoing)
			{
				continue;
			}
			List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
			for (int j = 0; j < partsListForReading.Count; j++)
			{
				if (partsListForReading[j] is QuestPart_DisableRandomMoodCausedMentalBreaks { State: QuestPartState.Enabled } questPart_DisableRandomMoodCausedMentalBreaks && questPart_DisableRandomMoodCausedMentalBreaks.pawns.Contains(p))
				{
					return true;
				}
			}
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CGetSubquests_003Ed__128))]
	public static IEnumerable<Quest> GetSubquests(this Quest quest, QuestState? state = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetSubquests_003Ed__128(-2)
		{
			_003C_003E3__quest = quest,
			_003C_003E3__state = state
		};
	}

	public static bool IsSubquestOf(this Quest quest, Quest parent)
	{
		if (quest.parent != null)
		{
			return quest.parent == parent;
		}
		return false;
	}

	public static bool IsGoodwillLockedByQuest(Faction a, Faction b)
	{
		List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
		for (int i = 0; i < questsListForReading.Count; i++)
		{
			if (questsListForReading[i].State != QuestState.Ongoing)
			{
				continue;
			}
			List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
			for (int j = 0; j < partsListForReading.Count; j++)
			{
				if (partsListForReading[j] is QuestPart_FactionGoodwillLocked { State: QuestPartState.Enabled } questPart_FactionGoodwillLocked && questPart_FactionGoodwillLocked.AppliesTo(a, b))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool IsEndOnNewArchonexusSettlement(this Quest quest)
	{
		if (quest.root.endOnColonyMove)
		{
			if (quest.State != 0)
			{
				return quest.State == QuestState.Ongoing;
			}
			return true;
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CGetGiverQuests_003Ed__132))]
	public static IEnumerable<QuestScriptDef> GetGiverQuests(QuestGiverTag tag)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGiverQuests_003Ed__132(-2)
		{
			_003C_003E3__tag = tag
		};
	}
}
