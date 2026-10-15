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

public class Pawn_RelationsTracker : IExposable
{
	[CompilerGenerated]
	private sealed class _003Cget_Children_003Ed__27 : IEnumerable<Pawn>, IEnumerable, IEnumerator<Pawn>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pawn _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Pawn_RelationsTracker _003C_003E4__this;

		private HashSet<Pawn>.Enumerator _003C_003E7__wrap1;

		Pawn IEnumerator<Pawn>.Current
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
		public _003Cget_Children_003Ed__27(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(HashSet<Pawn>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Pawn_RelationsTracker pawn_RelationsTracker = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = pawn_RelationsTracker.pawnsWithDirectRelationsWithMe.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					Pawn current = _003C_003E7__wrap1.Current;
					List<DirectPawnRelation> directRelations = current.relations.directRelations;
					for (int i = 0; i < directRelations.Count; i++)
					{
						if (directRelations[i].otherPawn == pawn_RelationsTracker.pawn && directRelations[i].def == PawnRelationDefOf.Parent)
						{
							_003C_003E2__current = current;
							_003C_003E1__state = 1;
							return true;
						}
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(HashSet<Pawn>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
		{
			_003Cget_Children_003Ed__27 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Children_003Ed__27(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Pawn>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_FamilyByBlood_Internal_003Ed__37 : IEnumerable<Pawn>, IEnumerable, IEnumerator<Pawn>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pawn _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Pawn_RelationsTracker _003C_003E4__this;

		private List<Pawn> _003CfamilyStack_003E5__2;

		private List<Pawn> _003CfamilyChildrenStack_003E5__3;

		private HashSet<Pawn> _003CfamilyVisited_003E5__4;

		private Pawn _003Cp_003E5__5;

		private Pawn _003Cchild_003E5__6;

		Pawn IEnumerator<Pawn>.Current
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
		public _003Cget_FamilyByBlood_Internal_003Ed__37(int _003C_003E1__state)
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
			_003CfamilyStack_003E5__2 = null;
			_003CfamilyChildrenStack_003E5__3 = null;
			_003CfamilyVisited_003E5__4 = null;
			_003Cp_003E5__5 = null;
			_003Cchild_003E5__6 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Pawn_RelationsTracker pawn_RelationsTracker = _003C_003E4__this;
				Pawn father;
				Pawn mother;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!pawn_RelationsTracker.RelatedToAnyoneOrAnyoneRelatedToMe)
					{
						return false;
					}
					_003CfamilyStack_003E5__2 = null;
					_003CfamilyChildrenStack_003E5__3 = null;
					_003CfamilyVisited_003E5__4 = null;
					_003C_003E1__state = -3;
					_003CfamilyStack_003E5__2 = SimplePool<List<Pawn>>.Get();
					_003CfamilyChildrenStack_003E5__3 = SimplePool<List<Pawn>>.Get();
					_003CfamilyVisited_003E5__4 = SimplePool<HashSet<Pawn>>.Get();
					_003CfamilyStack_003E5__2.Add(pawn_RelationsTracker.pawn);
					_003CfamilyVisited_003E5__4.Add(pawn_RelationsTracker.pawn);
					goto IL_0276;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00fc;
				case 2:
					{
						_003C_003E1__state = -3;
						goto IL_01f5;
					}
					IL_01f5:
					foreach (Pawn child in _003Cchild_003E5__6.relations.Children)
					{
						if (!_003CfamilyVisited_003E5__4.Contains(child))
						{
							_003CfamilyChildrenStack_003E5__3.Add(child);
							_003CfamilyVisited_003E5__4.Add(child);
						}
					}
					_003Cchild_003E5__6 = null;
					goto IL_025f;
					IL_00fc:
					father = _003Cp_003E5__5.GetFather();
					if (father != null && !_003CfamilyVisited_003E5__4.Contains(father))
					{
						_003CfamilyStack_003E5__2.Add(father);
						_003CfamilyVisited_003E5__4.Add(father);
					}
					mother = _003Cp_003E5__5.GetMother();
					if (mother != null && !_003CfamilyVisited_003E5__4.Contains(mother))
					{
						_003CfamilyStack_003E5__2.Add(mother);
						_003CfamilyVisited_003E5__4.Add(mother);
					}
					_003CfamilyChildrenStack_003E5__3.Clear();
					_003CfamilyChildrenStack_003E5__3.Add(_003Cp_003E5__5);
					goto IL_025f;
					IL_0276:
					if (_003CfamilyStack_003E5__2.Any())
					{
						_003Cp_003E5__5 = _003CfamilyStack_003E5__2[_003CfamilyStack_003E5__2.Count - 1];
						_003CfamilyStack_003E5__2.RemoveLast();
						if (_003Cp_003E5__5 != pawn_RelationsTracker.pawn)
						{
							_003C_003E2__current = _003Cp_003E5__5;
							_003C_003E1__state = 1;
							return true;
						}
						goto IL_00fc;
					}
					_003C_003Em__Finally1();
					return false;
					IL_025f:
					if (_003CfamilyChildrenStack_003E5__3.Any())
					{
						_003Cchild_003E5__6 = _003CfamilyChildrenStack_003E5__3[_003CfamilyChildrenStack_003E5__3.Count - 1];
						_003CfamilyChildrenStack_003E5__3.RemoveLast();
						if (_003Cchild_003E5__6 != _003Cp_003E5__5 && _003Cchild_003E5__6 != pawn_RelationsTracker.pawn)
						{
							_003C_003E2__current = _003Cchild_003E5__6;
							_003C_003E1__state = 2;
							return true;
						}
						goto IL_01f5;
					}
					_003Cp_003E5__5 = null;
					goto IL_0276;
				}
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
			_003CfamilyStack_003E5__2.Clear();
			SimplePool<List<Pawn>>.Return(_003CfamilyStack_003E5__2);
			_003CfamilyChildrenStack_003E5__3.Clear();
			SimplePool<List<Pawn>>.Return(_003CfamilyChildrenStack_003E5__3);
			_003CfamilyVisited_003E5__4.Clear();
			SimplePool<HashSet<Pawn>>.Return(_003CfamilyVisited_003E5__4);
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
		{
			_003Cget_FamilyByBlood_Internal_003Ed__37 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_FamilyByBlood_Internal_003Ed__37(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Pawn>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_PotentiallyRelatedPawns_003Ed__39 : IEnumerable<Pawn>, IEnumerable, IEnumerator<Pawn>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pawn _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Pawn_RelationsTracker _003C_003E4__this;

		private List<Pawn> _003Cstack_003E5__2;

		private HashSet<Pawn> _003Cvisited_003E5__3;

		private Pawn _003Cp_003E5__4;

		Pawn IEnumerator<Pawn>.Current
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
		public _003Cget_PotentiallyRelatedPawns_003Ed__39(int _003C_003E1__state)
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
			_003Cstack_003E5__2 = null;
			_003Cvisited_003E5__3 = null;
			_003Cp_003E5__4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Pawn_RelationsTracker pawn_RelationsTracker = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
					goto IL_00e2;
				}
				_003C_003E1__state = -1;
				if (!pawn_RelationsTracker.RelatedToAnyoneOrAnyoneRelatedToMe)
				{
					return false;
				}
				_003Cstack_003E5__2 = null;
				_003Cvisited_003E5__3 = null;
				_003C_003E1__state = -3;
				_003Cstack_003E5__2 = SimplePool<List<Pawn>>.Get();
				_003Cvisited_003E5__3 = SimplePool<HashSet<Pawn>>.Get();
				_003Cstack_003E5__2.Add(pawn_RelationsTracker.pawn);
				_003Cvisited_003E5__3.Add(pawn_RelationsTracker.pawn);
				goto IL_025d;
				IL_025d:
				if (_003Cstack_003E5__2.Any())
				{
					_003Cp_003E5__4 = _003Cstack_003E5__2[_003Cstack_003E5__2.Count - 1];
					_003Cstack_003E5__2.RemoveLast();
					if (_003Cp_003E5__4 != pawn_RelationsTracker.pawn)
					{
						_003C_003E2__current = _003Cp_003E5__4;
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_00e2;
				}
				_003C_003Em__Finally1();
				return false;
				IL_00e2:
				for (int i = 0; i < _003Cp_003E5__4.relations.directRelations.Count; i++)
				{
					Pawn otherPawn = _003Cp_003E5__4.relations.directRelations[i].otherPawn;
					if (otherPawn != null && !_003Cvisited_003E5__3.Contains(otherPawn))
					{
						_003Cstack_003E5__2.Add(otherPawn);
						_003Cvisited_003E5__3.Add(otherPawn);
					}
				}
				foreach (VirtualPawnRelation virtualRelation in _003Cp_003E5__4.relations.VirtualRelations)
				{
					foreach (Pawn reference in virtualRelation.record.References)
					{
						if (reference != pawn_RelationsTracker.pawn && reference != null && !_003Cvisited_003E5__3.Contains(reference))
						{
							_003Cstack_003E5__2.Add(reference);
							_003Cvisited_003E5__3.Add(reference);
						}
					}
				}
				foreach (Pawn item in _003Cp_003E5__4.relations.pawnsWithDirectRelationsWithMe)
				{
					if (!_003Cvisited_003E5__3.Contains(item))
					{
						_003Cstack_003E5__2.Add(item);
						_003Cvisited_003E5__3.Add(item);
					}
				}
				_003Cp_003E5__4 = null;
				goto IL_025d;
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
			_003Cstack_003E5__2.Clear();
			SimplePool<List<Pawn>>.Return(_003Cstack_003E5__2);
			_003Cvisited_003E5__3.Clear();
			SimplePool<HashSet<Pawn>>.Return(_003Cvisited_003E5__3);
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
		{
			_003Cget_PotentiallyRelatedPawns_003Ed__39 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_PotentiallyRelatedPawns_003Ed__39(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Pawn>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_RelatedPawns_003Ed__41 : IEnumerable<Pawn>, IEnumerable, IEnumerator<Pawn>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pawn _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Pawn_RelationsTracker _003C_003E4__this;

		private IEnumerator<Pawn> _003C_003E7__wrap1;

		Pawn IEnumerator<Pawn>.Current
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
		public _003Cget_RelatedPawns_003Ed__41(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 1)
			{
				try
				{
					if (num == -4 || num == 1)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
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
				Pawn_RelationsTracker pawn_RelationsTracker = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					pawn_RelationsTracker.canCacheFamilyByBlood = true;
					pawn_RelationsTracker.familyByBloodIsCached = false;
					pawn_RelationsTracker.cachedFamilyByBlood.Clear();
					_003C_003E1__state = -3;
					_003C_003E7__wrap1 = pawn_RelationsTracker.PotentiallyRelatedPawns.GetEnumerator();
					_003C_003E1__state = -4;
					break;
				case 1:
					_003C_003E1__state = -4;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					Pawn current = _003C_003E7__wrap1.Current;
					if ((pawn_RelationsTracker.familyByBloodIsCached && pawn_RelationsTracker.cachedFamilyByBlood.Contains(current)) || pawn_RelationsTracker.pawn.GetRelations(current).Any())
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap1 = null;
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
			Pawn_RelationsTracker pawn_RelationsTracker = _003C_003E4__this;
			pawn_RelationsTracker.canCacheFamilyByBlood = false;
			pawn_RelationsTracker.familyByBloodIsCached = false;
			pawn_RelationsTracker.cachedFamilyByBlood.Clear();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
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
		IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
		{
			_003Cget_RelatedPawns_003Ed__41 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_RelatedPawns_003Ed__41(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Pawn>)this).GetEnumerator();
		}
	}

	private Pawn pawn;

	private List<DirectPawnRelation> directRelations = new List<DirectPawnRelation>();

	private List<VirtualPawnRelation> virtualRelations = new List<VirtualPawnRelation>();

	public bool everSeenByPlayer;

	public bool canGetRescuedThought = true;

	public Pawn relativeInvolvedInRescueQuest;

	public MarriageNameChange nextMarriageNameChange;

	public bool hidePawnRelations;

	[LoadAlias("rhythmModes")]
	private Dictionary<Pawn, PregnancyApproach> pregnancyApproaches = new Dictionary<Pawn, PregnancyApproach>();

	public int romanceEnableTick = -1;

	private HashSet<Pawn> pawnsWithDirectRelationsWithMe = new HashSet<Pawn>();

	private HashSet<Pawn> cachedFamilyByBlood = new HashSet<Pawn>();

	private bool familyByBloodIsCached;

	private bool canCacheFamilyByBlood;

	private const int CheckDevelopBondRelationIntervalTicks = 2500;

	private const float MaxBondRelationCheckDist = 12f;

	private const float BondRelationPerIntervalChance = 0.001f;

	public const int FriendOpinionThreshold = 20;

	public const int RivalOpinionThreshold = -20;

	public const int MinLovinAge = 16;

	private const float MinAgeFactor = 0.2f;

	public const float PsychicLoveFactor = 10f;

	private List<Pawn> tmpPawns = new List<Pawn>();

	private List<PregnancyApproach> tmpPregnancyApproaches = new List<PregnancyApproach>();

	private static List<ISocialThought> tmpSocialThoughts = new List<ISocialThought>();

	public List<DirectPawnRelation> DirectRelations => directRelations;

	public List<VirtualPawnRelation> VirtualRelations => virtualRelations;

	public IEnumerable<Pawn> Children
	{
		[IteratorStateMachine(typeof(_003Cget_Children_003Ed__27))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Children_003Ed__27(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public int ChildrenCount => Children.Count();

	public bool IsTryRomanceOnCooldown => romanceEnableTick > Find.TickManager.TicksGame;

	public bool RelatedToAnyoneOrAnyoneRelatedToMe
	{
		get
		{
			if (!directRelations.Any() && !virtualRelations.Any())
			{
				return pawnsWithDirectRelationsWithMe.Any();
			}
			return true;
		}
	}

	public IEnumerable<Pawn> FamilyByBlood
	{
		get
		{
			if (canCacheFamilyByBlood)
			{
				if (!familyByBloodIsCached)
				{
					cachedFamilyByBlood.Clear();
					foreach (Pawn item in FamilyByBlood_Internal)
					{
						cachedFamilyByBlood.Add(item);
					}
					familyByBloodIsCached = true;
				}
				return cachedFamilyByBlood;
			}
			return FamilyByBlood_Internal;
		}
	}

	private IEnumerable<Pawn> FamilyByBlood_Internal
	{
		[IteratorStateMachine(typeof(_003Cget_FamilyByBlood_Internal_003Ed__37))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_FamilyByBlood_Internal_003Ed__37(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public IEnumerable<Pawn> PotentiallyRelatedPawns
	{
		[IteratorStateMachine(typeof(_003Cget_PotentiallyRelatedPawns_003Ed__39))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_PotentiallyRelatedPawns_003Ed__39(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public IEnumerable<Pawn> RelatedPawns
	{
		[IteratorStateMachine(typeof(_003Cget_RelatedPawns_003Ed__41))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_RelatedPawns_003Ed__41(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public Pawn_RelationsTracker(Pawn pawn)
	{
		this.pawn = pawn;
	}

	public void ExposeData()
	{
		Scribe_Collections.Look(ref directRelations, "directRelations", LookMode.Deep);
		Scribe_Collections.Look(ref virtualRelations, "virtualRelations", LookMode.Deep);
		if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
		{
			if (virtualRelations == null)
			{
				virtualRelations = new List<VirtualPawnRelation>();
			}
		}
		else if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (directRelations[i].otherPawn == null)
				{
					Log.Warning("Pawn " + pawn?.ToString() + " has relation \"" + directRelations[i].def.defName + "\" with null pawn after loading. This means that we forgot to serialize pawns somewhere (e.g. pawns from passing trade ships).");
				}
			}
			directRelations.RemoveAll((DirectPawnRelation x) => x.otherPawn == null);
			for (int j = 0; j < directRelations.Count; j++)
			{
				directRelations[j].otherPawn.relations.pawnsWithDirectRelationsWithMe.Add(pawn);
			}
			if (pregnancyApproaches == null)
			{
				pregnancyApproaches = new Dictionary<Pawn, PregnancyApproach>();
			}
			pregnancyApproaches.RemoveAll((KeyValuePair<Pawn, PregnancyApproach> x) => x.Key == null);
			for (int num = virtualRelations.Count - 1; num >= 0; num--)
			{
				if (virtualRelations[num].record == null)
				{
					virtualRelations.RemoveAt(num);
				}
			}
		}
		Scribe_Values.Look(ref everSeenByPlayer, "everSeenByPlayer", defaultValue: true);
		Scribe_Values.Look(ref canGetRescuedThought, "canGetRescuedThought", defaultValue: true);
		Scribe_Values.Look(ref nextMarriageNameChange, "nextMarriageNameChange", MarriageNameChange.NoChange);
		Scribe_References.Look(ref relativeInvolvedInRescueQuest, "relativeInvolvedInRescueQuest");
		Scribe_Values.Look(ref hidePawnRelations, "hidePawnRelations", defaultValue: false);
		Scribe_Collections.Look(ref pregnancyApproaches, "pregnancyApproaches", LookMode.Reference, LookMode.Value, ref tmpPawns, ref tmpPregnancyApproaches, logNullErrors: false);
		Scribe_Values.Look(ref romanceEnableTick, "romanceEnableTick", 0);
	}

	public PregnancyApproach GetPregnancyApproachForPartner(Pawn partner)
	{
		if (!pregnancyApproaches.ContainsKey(partner))
		{
			if (partner.relations.pregnancyApproaches.TryGetValue(pawn, out var value))
			{
				pregnancyApproaches.Add(partner, value);
			}
			else
			{
				SetPregnancyApproach(partner, PregnancyApproach.Normal);
			}
		}
		return pregnancyApproaches[partner];
	}

	public void SetPregnancyApproach(Pawn partner, PregnancyApproach mode)
	{
		pregnancyApproaches[partner] = mode;
		partner.relations.pregnancyApproaches[pawn] = mode;
	}

	public void RelationsTrackerTickInterval(int delta)
	{
		if (!pawn.Dead)
		{
			Tick_CheckStartMarriageCeremony(delta);
			Tick_CheckDevelopBondRelation(delta);
		}
	}

	public DirectPawnRelation GetDirectRelation(PawnRelationDef def, Pawn otherPawn)
	{
		if (def.implied)
		{
			Log.Warning(def?.ToString() + " is not a direct relation.");
			return null;
		}
		foreach (DirectPawnRelation directRelation in directRelations)
		{
			if (directRelation.def == def && directRelation.otherPawn == otherPawn)
			{
				return directRelation;
			}
		}
		return null;
	}

	public Pawn GetFirstDirectRelationPawn(PawnRelationDef def, Predicate<Pawn> predicate = null)
	{
		if (def.implied)
		{
			Log.Warning(def?.ToString() + " is not a direct relation.");
			return null;
		}
		for (int i = 0; i < directRelations.Count; i++)
		{
			DirectPawnRelation directPawnRelation = directRelations[i];
			if (directPawnRelation.def == def && (predicate == null || predicate(directPawnRelation.otherPawn)))
			{
				return directPawnRelation.otherPawn;
			}
		}
		return null;
	}

	public int GetDirectRelationsCount(PawnRelationDef def, Predicate<Pawn> predicate = null)
	{
		int num = 0;
		if (def.implied)
		{
			Log.Warning(def?.ToString() + " is not a direct relation.");
			return num;
		}
		for (int i = 0; i < directRelations.Count; i++)
		{
			DirectPawnRelation directPawnRelation = directRelations[i];
			if (directPawnRelation.def == def && (predicate == null || predicate(directPawnRelation.otherPawn)))
			{
				num++;
			}
		}
		return num;
	}

	public bool DirectRelationExists(PawnRelationDef def, Pawn otherPawn)
	{
		if (def.implied)
		{
			Log.Warning(def?.ToString() + " is not a direct relation.");
			return false;
		}
		for (int i = 0; i < directRelations.Count; i++)
		{
			DirectPawnRelation directPawnRelation = directRelations[i];
			if (directPawnRelation.def == def && directPawnRelation.otherPawn == otherPawn)
			{
				return true;
			}
		}
		return false;
	}

	public bool VirtualRelationExists(PawnRelationDef def, RelationshipRecord record)
	{
		if (def.implied)
		{
			Log.Warning(def?.ToString() + " is not a direct relation.");
			return false;
		}
		for (int i = 0; i < virtualRelations.Count; i++)
		{
			VirtualPawnRelation virtualPawnRelation = virtualRelations[i];
			if (virtualPawnRelation.def == def && virtualPawnRelation.record.ID == record.ID)
			{
				return true;
			}
		}
		return false;
	}

	public void GetDirectRelations(PawnRelationDef def, ref List<Pawn> list, Predicate<Pawn> predicate = null)
	{
		if (def.implied)
		{
			Log.Warning(def?.ToString() + " is not a direct relation.");
			return;
		}
		for (int i = 0; i < directRelations.Count; i++)
		{
			DirectPawnRelation directPawnRelation = directRelations[i];
			if (directPawnRelation.def == def && (predicate == null || predicate(directPawnRelation.otherPawn)))
			{
				list.Add(directPawnRelation.otherPawn);
			}
		}
	}

	public void ElevateToVirtualRelation(DirectPawnRelation relation)
	{
		RelationshipRecord orCreateRecord = Find.RelationshipRecords.GetOrCreateRecord(relation.otherPawn);
		virtualRelations.Add(new VirtualPawnRelation(relation.def, orCreateRecord, relation.startTicks));
		orCreateRecord.AddReference(pawn);
		directRelations.Remove(relation);
	}

	public void AddDirectRelation(PawnRelationDef def, Pawn otherPawn)
	{
		if (def.implied)
		{
			Log.Warning("Tried to directly add implied pawn relation " + def?.ToString() + ", pawn=" + pawn?.ToString() + ", otherPawn=" + otherPawn);
			return;
		}
		if (otherPawn == pawn)
		{
			Log.Warning("Tried to add pawn relation " + def?.ToString() + " with self, pawn=" + pawn);
			return;
		}
		if (DirectRelationExists(def, otherPawn))
		{
			Log.Warning("Tried to add the same relation twice: " + def?.ToString() + ", pawn=" + pawn?.ToString() + ", otherPawn=" + otherPawn);
			return;
		}
		int startTicks = ((Current.ProgramState == ProgramState.Playing) ? Find.TickManager.TicksGame : 0);
		def.Worker.OnRelationCreated(pawn, otherPawn);
		directRelations.Add(new DirectPawnRelation(def, otherPawn, startTicks));
		otherPawn.relations.pawnsWithDirectRelationsWithMe.Add(pawn);
		if (def.reflexive)
		{
			otherPawn.relations.directRelations.Add(new DirectPawnRelation(def, pawn, startTicks));
			pawnsWithDirectRelationsWithMe.Add(otherPawn);
		}
		GainedOrLostDirectRelation();
		otherPawn.relations.GainedOrLostDirectRelation();
		if (Current.ProgramState != ProgramState.Playing)
		{
			return;
		}
		if (!pawn.Dead && pawn.health != null)
		{
			for (int num = pawn.health.hediffSet.hediffs.Count - 1; num >= 0; num--)
			{
				pawn.health.hediffSet.hediffs[num].Notify_RelationAdded(otherPawn, def);
			}
		}
		if (!otherPawn.Dead && otherPawn.health != null)
		{
			for (int num2 = otherPawn.health.hediffSet.hediffs.Count - 1; num2 >= 0; num2--)
			{
				otherPawn.health.hediffSet.hediffs[num2].Notify_RelationAdded(pawn, def);
			}
		}
	}

	public void RemoveRelation(RelationshipRecord record)
	{
		for (int num = virtualRelations.Count - 1; num >= 0; num--)
		{
			if (virtualRelations[num].record == record)
			{
				virtualRelations.RemoveAt(num);
			}
		}
	}

	public void RemoveDirectRelation(DirectPawnRelation relation)
	{
		RemoveDirectRelation(relation.def, relation.otherPawn);
	}

	public void RemoveDirectRelation(PawnRelationDef def, Pawn otherPawn)
	{
		if (!TryRemoveDirectRelation(def, otherPawn))
		{
			Log.Warning("Could not remove relation " + def?.ToString() + " because it's not here. pawn=" + pawn?.ToString() + ", otherPawn=" + otherPawn);
		}
	}

	public bool TryRemoveDirectRelation(PawnRelationDef def, Pawn otherPawn)
	{
		if (def.implied)
		{
			Log.Warning("Tried to remove implied pawn relation " + def?.ToString() + ", pawn=" + pawn?.ToString() + ", otherPawn=" + otherPawn);
			return false;
		}
		for (int i = 0; i < directRelations.Count; i++)
		{
			if (directRelations[i].def != def || directRelations[i].otherPawn != otherPawn)
			{
				continue;
			}
			if (def.reflexive)
			{
				List<DirectPawnRelation> list = otherPawn.relations.directRelations;
				DirectPawnRelation directPawnRelation = list.Find((DirectPawnRelation x) => x.def == def && x.otherPawn == pawn);
				directPawnRelation.def.Worker.OnRelationRemoved(otherPawn, pawn);
				list.Remove(directPawnRelation);
				if (list.Find((DirectPawnRelation x) => x.otherPawn == pawn) == null)
				{
					pawnsWithDirectRelationsWithMe.Remove(otherPawn);
				}
			}
			directRelations[i].def.Worker.OnRelationRemoved(pawn, otherPawn);
			directRelations.RemoveAt(i);
			if (directRelations.Find((DirectPawnRelation x) => x.otherPawn == otherPawn) == null)
			{
				otherPawn.relations.pawnsWithDirectRelationsWithMe.Remove(pawn);
			}
			GainedOrLostDirectRelation();
			otherPawn.relations.GainedOrLostDirectRelation();
			return true;
		}
		return false;
	}

	public int OpinionOf(Pawn other)
	{
		if (!other.RaceProps.Humanlike || pawn == other)
		{
			return 0;
		}
		if (pawn.Dead)
		{
			return 0;
		}
		int num = 0;
		foreach (PawnRelationDef relation in pawn.GetRelations(other))
		{
			num += relation.opinionOffset;
		}
		if (pawn.RaceProps.Humanlike && pawn.needs.mood != null)
		{
			num += pawn.needs.mood.thoughts.TotalOpinionOffset(other);
		}
		if (num != 0)
		{
			float num2 = 1f;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].CurStage != null)
				{
					num2 *= hediffs[i].CurStage.opinionOfOthersFactor;
				}
			}
			num = Mathf.RoundToInt((float)num * num2);
		}
		if (num > 0 && pawn.HostileTo(other))
		{
			num = 0;
		}
		return Mathf.Clamp(num, -100, 100);
	}

	public string OpinionExplanation(Pawn other)
	{
		if (!other.RaceProps.Humanlike || pawn == other)
		{
			return "";
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("OpinionOf".Translate(other.LabelShort).Colorize(ColoredText.TipSectionTitleColor) + ": " + OpinionOf(other).ToStringWithSign());
		string pawnSituationLabel = SocialCardUtility.GetPawnSituationLabel(other, pawn);
		if (!pawnSituationLabel.NullOrEmpty())
		{
			stringBuilder.AppendLine(" (" + pawnSituationLabel + ")");
		}
		else
		{
			stringBuilder.AppendLine();
		}
		bool flag = false;
		if (pawn.Dead)
		{
			stringBuilder.AppendLine(" - " + "IAmDead".Translate());
			flag = true;
		}
		else
		{
			foreach (PawnRelationDef relation in pawn.GetRelations(other))
			{
				stringBuilder.AppendLine(" - " + relation.GetGenderSpecificLabelCap(other) + ": " + relation.opinionOffset.ToStringWithSign());
				flag = true;
			}
			if (pawn.RaceProps.Humanlike && pawn.needs.mood != null)
			{
				ThoughtHandler thoughts = pawn.needs.mood.thoughts;
				thoughts.GetDistinctSocialThoughtGroups(other, tmpSocialThoughts);
				for (int i = 0; i < tmpSocialThoughts.Count; i++)
				{
					ISocialThought socialThought = tmpSocialThoughts[i];
					int num = 1;
					Thought thought = (Thought)socialThought;
					if (thought.def.IsMemory)
					{
						num = thoughts.memories.NumMemoriesInGroup((Thought_MemorySocial)socialThought);
					}
					stringBuilder.Append(" - " + thought.LabelCapSocial);
					if (num != 1)
					{
						stringBuilder.Append(" x" + num);
					}
					stringBuilder.AppendLine(": " + thoughts.OpinionOffsetOfGroup(socialThought, other).ToStringWithSign());
					flag = true;
				}
			}
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int j = 0; j < hediffs.Count; j++)
			{
				HediffStage curStage = hediffs[j].CurStage;
				if (curStage != null && curStage.opinionOfOthersFactor != 1f)
				{
					stringBuilder.Append(" - " + hediffs[j].LabelBaseCap);
					if (curStage.opinionOfOthersFactor != 0f)
					{
						stringBuilder.AppendLine(": x" + curStage.opinionOfOthersFactor.ToStringPercent());
					}
					else
					{
						stringBuilder.AppendLine();
					}
					flag = true;
				}
			}
		}
		if (!flag)
		{
			stringBuilder.AppendLine(" - " + "NoneBrackets".Translate());
		}
		return stringBuilder.ToString().TrimEndNewlines();
	}

	public float SecondaryLovinChanceFactor(Pawn otherPawn)
	{
		if (pawn.def != otherPawn.def || pawn == otherPawn)
		{
			return 0f;
		}
		if (pawn.story != null && pawn.story.traits != null)
		{
			if (pawn.story.traits.HasTrait(TraitDefOf.Asexual))
			{
				return 0f;
			}
			if (!pawn.story.traits.HasTrait(TraitDefOf.Bisexual))
			{
				if (pawn.story.traits.HasTrait(TraitDefOf.Gay))
				{
					if (otherPawn.gender != pawn.gender)
					{
						return 0f;
					}
				}
				else if (otherPawn.gender == pawn.gender)
				{
					return 0f;
				}
			}
		}
		if (pawn.ageTracker.AgeBiologicalYearsFloat < 16f || otherPawn.ageTracker.AgeBiologicalYearsFloat < 16f)
		{
			return 0f;
		}
		return LovinAgeFactor(otherPawn) * PrettinessFactor(otherPawn);
	}

	public float PrettinessFactor(Pawn otherPawn)
	{
		float num = 0f;
		if (otherPawn.RaceProps.Humanlike)
		{
			num = otherPawn.GetStatValue(StatDefOf.PawnBeauty);
		}
		if (num < 0f)
		{
			return 0.3f;
		}
		if (num > 0f)
		{
			return 2.3f;
		}
		return 1f;
	}

	public float LovinAgeFactor(Pawn otherPawn)
	{
		float num = 1f;
		float ageBiologicalYearsFloat = pawn.ageTracker.AgeBiologicalYearsFloat;
		float ageBiologicalYearsFloat2 = otherPawn.ageTracker.AgeBiologicalYearsFloat;
		if (pawn.gender == Gender.Male)
		{
			float min = ageBiologicalYearsFloat - 30f;
			float lower = ageBiologicalYearsFloat - 10f;
			float upper = ageBiologicalYearsFloat + 3f;
			float max = ageBiologicalYearsFloat + 10f;
			num = GenMath.FlatHill(0.2f, min, lower, upper, max, 0.2f, ageBiologicalYearsFloat2);
		}
		else if (pawn.gender == Gender.Female)
		{
			float min2 = ageBiologicalYearsFloat - 10f;
			float lower2 = ageBiologicalYearsFloat - 3f;
			float upper2 = ageBiologicalYearsFloat + 10f;
			float max2 = ageBiologicalYearsFloat + 30f;
			num = GenMath.FlatHill(0.2f, min2, lower2, upper2, max2, 0.2f, ageBiologicalYearsFloat2);
		}
		float num2 = Mathf.InverseLerp(16f, 18f, ageBiologicalYearsFloat);
		float num3 = Mathf.InverseLerp(16f, 18f, ageBiologicalYearsFloat2);
		return num * num2 * num3;
	}

	public float SecondaryRomanceChanceFactor(Pawn otherPawn)
	{
		float num = 1f;
		foreach (PawnRelationDef relation in pawn.GetRelations(otherPawn))
		{
			num *= relation.romanceChanceFactor;
		}
		float num2 = 1f;
		HediffWithTarget hediffWithTarget = (HediffWithTarget)pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicLove);
		if (hediffWithTarget != null && hediffWithTarget.target == otherPawn)
		{
			num2 = 10f;
		}
		float num3 = 1f;
		if (ModsConfig.BiotechActive && pawn.genes != null && (otherPawn.story?.traits == null || !otherPawn.story.traits.HasTrait(TraitDefOf.Kind)))
		{
			List<Gene> genesListForReading = pawn.genes.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (genesListForReading[i].def.missingGeneRomanceChanceFactor != 1f && (otherPawn.genes == null || !otherPawn.genes.HasActiveGene(genesListForReading[i].def)))
				{
					num3 *= genesListForReading[i].def.missingGeneRomanceChanceFactor;
				}
			}
		}
		return SecondaryLovinChanceFactor(otherPawn) * num * num2 * num3;
	}

	public float CompatibilityWith(Pawn otherPawn)
	{
		if (pawn.def != otherPawn.def || pawn == otherPawn)
		{
			return 0f;
		}
		float x = Mathf.Abs(pawn.ageTracker.AgeBiologicalYearsFloat - otherPawn.ageTracker.AgeBiologicalYearsFloat);
		float num = Mathf.Clamp(GenMath.LerpDouble(0f, 20f, 0.45f, -0.45f, x), -0.45f, 0.45f);
		float num2 = ConstantPerPawnsPairCompatibilityOffset(otherPawn.thingIDNumber);
		return num + num2;
	}

	public float ConstantPerPawnsPairCompatibilityOffset(int otherPawnID)
	{
		Rand.PushState();
		Rand.Seed = (pawn.thingIDNumber ^ otherPawnID) * 37;
		float result = Rand.GaussianAsymmetric(0.3f, 1f, 1.4f);
		Rand.PopState();
		return result;
	}

	public void ClearAllRelations()
	{
		CleanupVirtualRelationReferences();
		List<DirectPawnRelation> list = directRelations.ToList();
		for (int i = 0; i < list.Count; i++)
		{
			RemoveDirectRelation(list[i]);
		}
		List<Pawn> list2 = pawnsWithDirectRelationsWithMe.ToList();
		for (int j = 0; j < list2.Count; j++)
		{
			List<DirectPawnRelation> list3 = list2[j].relations.directRelations.ToList();
			for (int k = 0; k < list3.Count; k++)
			{
				if (list3[k].otherPawn == pawn)
				{
					list2[j].relations.RemoveDirectRelation(list3[k]);
				}
			}
		}
	}

	private void CleanupVirtualRelationReferences()
	{
		foreach (VirtualPawnRelation virtualRelation in virtualRelations)
		{
			virtualRelation.record.RemoveReference(pawn);
		}
		virtualRelations.Clear();
	}

	public void ClearAllNonBloodRelations()
	{
		List<DirectPawnRelation> list = directRelations.ToList();
		for (int i = 0; i < list.Count; i++)
		{
			if (!list[i].def.familyByBloodRelation)
			{
				RemoveDirectRelation(list[i]);
			}
		}
		List<Pawn> list2 = pawnsWithDirectRelationsWithMe.ToList();
		for (int j = 0; j < list2.Count; j++)
		{
			List<DirectPawnRelation> list3 = list2[j].relations.directRelations.ToList();
			for (int k = 0; k < list3.Count; k++)
			{
				if (list3[k].otherPawn == pawn && !list3[j].def.familyByBloodRelation)
				{
					list2[j].relations.RemoveDirectRelation(list3[k]);
				}
			}
		}
	}

	internal void Notify_PawnKilled(DamageInfo? dinfo, Map mapBeforeDeath)
	{
		foreach (Pawn potentiallyRelatedPawn in PotentiallyRelatedPawns)
		{
			if (!potentiallyRelatedPawn.Dead && potentiallyRelatedPawn.needs.mood != null)
			{
				potentiallyRelatedPawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
		}
		RemoveMySpouseMarriageRelatedThoughts();
		RemoveRelationsOnDeath();
		if (everSeenByPlayer && !PawnGenerator.IsBeingGenerated(pawn) && !pawn.RaceProps.Animal)
		{
			AffectBondedAnimalsOnMyDeath();
		}
		Notify_FailedRescueQuest();
	}

	public void Notify_PawnDestroyed(DestroyMode mode)
	{
		RemoveRelationsOnDeath();
	}

	public void Notify_PassedToWorld()
	{
		if (!pawn.Dead)
		{
			relativeInvolvedInRescueQuest = null;
		}
	}

	public void Notify_PawnLeftBehind()
	{
		RemoveRelationsOnLeftBehind();
	}

	public void Notify_ExitedMap()
	{
		CheckRescued();
	}

	public void Notify_ChangedFaction()
	{
		if (pawn.Faction == Faction.OfPlayer)
		{
			CheckRescued();
		}
	}

	public void Notify_PawnSold(Pawn playerNegotiator)
	{
		foreach (Pawn potentiallyRelatedPawn in PotentiallyRelatedPawns)
		{
			if (potentiallyRelatedPawn.Dead || potentiallyRelatedPawn.needs.mood == null)
			{
				continue;
			}
			PawnRelationDef mostImportantRelation = potentiallyRelatedPawn.GetMostImportantRelation(pawn);
			if (mostImportantRelation == null || mostImportantRelation.soldThoughts == null)
			{
				continue;
			}
			if (mostImportantRelation == PawnRelationDefOf.Bond)
			{
				pawn.relations.RemoveDirectRelation(mostImportantRelation, potentiallyRelatedPawn);
			}
			foreach (ThoughtDef soldThought in mostImportantRelation.soldThoughts)
			{
				potentiallyRelatedPawn.needs.mood.thoughts.memories.TryGainMemory(soldThought, playerNegotiator);
			}
		}
		RemoveMySpouseMarriageRelatedThoughts();
	}

	public void Notify_PawnKidnapped()
	{
		RemoveMySpouseMarriageRelatedThoughts();
	}

	public void Notify_RescuedBy(Pawn rescuer)
	{
		if (rescuer.RaceProps.Humanlike && pawn.needs.mood != null && canGetRescuedThought)
		{
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RescuedMe, rescuer);
			canGetRescuedThought = false;
		}
	}

	public void Notify_FailedRescueQuest()
	{
		if (relativeInvolvedInRescueQuest != null && !relativeInvolvedInRescueQuest.Dead && relativeInvolvedInRescueQuest.needs.mood != null)
		{
			Messages.Message("MessageFailedToRescueRelative".Translate(pawn.LabelShort, relativeInvolvedInRescueQuest.LabelShort, pawn.Named("PAWN"), relativeInvolvedInRescueQuest.Named("RELATIVE")), relativeInvolvedInRescueQuest, MessageTypeDefOf.PawnDeath);
			relativeInvolvedInRescueQuest.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FailedToRescueRelative, pawn);
		}
		relativeInvolvedInRescueQuest = null;
	}

	private void CheckRescued()
	{
		if (relativeInvolvedInRescueQuest != null && !relativeInvolvedInRescueQuest.Dead && relativeInvolvedInRescueQuest.needs.mood != null)
		{
			Messages.Message("MessageRescuedRelative".Translate(pawn.LabelShort, relativeInvolvedInRescueQuest.LabelShort, pawn.Named("PAWN"), relativeInvolvedInRescueQuest.Named("RELATIVE")), relativeInvolvedInRescueQuest, MessageTypeDefOf.PositiveEvent);
			relativeInvolvedInRescueQuest.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RescuedRelative, pawn);
		}
		relativeInvolvedInRescueQuest = null;
	}

	public float GetFriendDiedThoughtPowerFactor(int opinion)
	{
		return Mathf.Lerp(0.15f, 1f, Mathf.InverseLerp(20f, 100f, opinion));
	}

	public float GetRivalDiedThoughtPowerFactor(int opinion)
	{
		return Mathf.Lerp(0.15f, 1f, Mathf.InverseLerp(-20f, -100f, opinion));
	}

	private void RemoveMySpouseMarriageRelatedThoughts()
	{
		foreach (Pawn spouse in pawn.GetSpouses(includeDead: false))
		{
			if (spouse.needs.mood != null)
			{
				MemoryThoughtHandler memories = spouse.needs.mood.thoughts.memories;
				memories.RemoveMemoriesOfDef(ThoughtDefOf.GotMarried);
				memories.RemoveMemoriesOfDef(ThoughtDefOf.HoneymoonPhase);
			}
		}
	}

	private void RemoveRelationsOnDeath()
	{
		for (int num = directRelations.Count - 1; num >= 0; num--)
		{
			if (directRelations[num].def.removeOnDeath)
			{
				DirectPawnRelation directPawnRelation = directRelations[num];
				Pawn otherPawn = directPawnRelation.otherPawn;
				RemoveDirectRelation(directRelations[num]);
				directPawnRelation.def.Worker.Notify_PostRemovedByDeath(pawn, otherPawn);
			}
		}
	}

	private void RemoveRelationsOnLeftBehind()
	{
		for (int num = directRelations.Count - 1; num >= 0; num--)
		{
			if (directRelations[num].def.removeOnLeftBehind)
			{
				DirectPawnRelation directPawnRelation = directRelations[num];
				Pawn otherPawn = directPawnRelation.otherPawn;
				RemoveDirectRelation(directRelations[num]);
				directPawnRelation.def.Worker.Notify_PostRemovedLeftBehind(pawn, otherPawn);
			}
		}
	}

	public void CheckAppendBondedAnimalDiedInfo(ref TaggedString letter, ref TaggedString label)
	{
		if (!pawn.RaceProps.Animal || !everSeenByPlayer || PawnGenerator.IsBeingGenerated(pawn))
		{
			return;
		}
		Predicate<Pawn> isAffected = delegate(Pawn x)
		{
			if (x.Dead)
			{
				return false;
			}
			return (!x.RaceProps.Humanlike || !x.story.traits.HasTrait(TraitDefOf.Psychopath)) ? true : false;
		};
		int num = 0;
		for (int i = 0; i < directRelations.Count; i++)
		{
			if (directRelations[i].def == PawnRelationDefOf.Bond && isAffected(directRelations[i].otherPawn))
			{
				num++;
			}
		}
		TaggedString taggedString;
		switch (num)
		{
		case 0:
			return;
		case 1:
		{
			Pawn firstDirectRelationPawn = GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, (Pawn x) => isAffected(x));
			taggedString = "LetterPartBondedAnimalDied".Translate(pawn.LabelDefinite(), firstDirectRelationPawn.LabelShort, pawn.Named("ANIMAL"), firstDirectRelationPawn.Named("HUMAN")).CapitalizeFirst();
			break;
		}
		default:
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int j = 0; j < directRelations.Count; j++)
			{
				if (directRelations[j].def == PawnRelationDefOf.Bond && isAffected(directRelations[j].otherPawn))
				{
					stringBuilder.AppendLine("  - " + directRelations[j].otherPawn.LabelShort);
				}
			}
			taggedString = "LetterPartBondedAnimalDiedMulti".Translate(stringBuilder.ToString().TrimEndNewlines());
			break;
		}
		}
		label += " (" + "LetterLabelSuffixBondedAnimalDied".Translate() + ")";
		if (!letter.NullOrEmpty())
		{
			letter += "\n\n";
		}
		letter += taggedString;
	}

	private void AffectBondedAnimalsOnMyDeath()
	{
		int num = 0;
		Pawn pawn = null;
		for (int i = 0; i < directRelations.Count; i++)
		{
			if (directRelations[i].def == PawnRelationDefOf.Bond && directRelations[i].otherPawn.Spawned)
			{
				pawn = directRelations[i].otherPawn;
				num++;
				float value = Rand.Value;
				MentalStateDef stateDef = ((value < 0.25f) ? MentalStateDefOf.Wander_Sad : ((value < 0.5f) ? MentalStateDefOf.Wander_Psychotic : ((!(value < 0.75f)) ? MentalStateDefOf.Manhunter : MentalStateDefOf.Berserk)));
				directRelations[i].otherPawn.mindState.mentalStateHandler.TryStartMentalState(stateDef, "MentalStateReason_BondedHumanDeath".Translate(this.pawn).Resolve(), forced: false, forceWake: true);
			}
		}
		if (num == 1)
		{
			Messages.Message(((pawn.Name == null || pawn.Name.Numerical) ? "MessageBondedAnimalMentalBreak".Translate(pawn.LabelIndefinite(), this.pawn.LabelShort, pawn.Named("ANIMAL"), this.pawn.Named("HUMAN")) : "MessageNamedBondedAnimalMentalBreak".Translate(pawn.KindLabelIndefinite(), pawn.Name.ToStringShort, this.pawn.LabelShort, pawn.Named("ANIMAL"), this.pawn.Named("HUMAN"))).CapitalizeFirst(), pawn, MessageTypeDefOf.ThreatSmall);
		}
		else if (num > 1)
		{
			Messages.Message("MessageBondedAnimalsMentalBreak".Translate(num, this.pawn.LabelShort, this.pawn.Named("HUMAN")), pawn, MessageTypeDefOf.ThreatSmall);
		}
	}

	private void Tick_CheckStartMarriageCeremony(int delta)
	{
		if (!pawn.Spawned || pawn.RaceProps.Animal || !pawn.IsHashIntervalTick(1017, delta))
		{
			return;
		}
		int ticksGame = Find.TickManager.TicksGame;
		for (int i = 0; i < directRelations.Count; i++)
		{
			float num = (float)(ticksGame - directRelations[i].startTicks) / 60000f;
			if (directRelations[i].def == PawnRelationDefOf.Fiance && pawn.thingIDNumber < directRelations[i].otherPawn.thingIDNumber && num > 10f && Rand.MTBEventOccurs(2f, 60000f, 1017f) && pawn.Map == directRelations[i].otherPawn.Map && pawn.Map.IsPlayerHome && MarriageCeremonyUtility.AcceptableGameConditionsToStartCeremony(pawn.Map) && MarriageCeremonyUtility.FianceReadyToStartCeremony(pawn, directRelations[i].otherPawn) && MarriageCeremonyUtility.FianceReadyToStartCeremony(directRelations[i].otherPawn, pawn))
			{
				pawn.Map.lordsStarter.TryStartMarriageCeremony(pawn, directRelations[i].otherPawn);
			}
		}
	}

	private void Tick_CheckDevelopBondRelation(int delta)
	{
		if (pawn.Spawned && pawn.IsAnimal && pawn.Faction == Faction.OfPlayer && pawn.playerSettings.RespectedMaster != null)
		{
			Pawn respectedMaster = pawn.playerSettings.RespectedMaster;
			if (pawn.IsHashIntervalTick(2500, delta) && pawn.Map == respectedMaster.Map && pawn.Position.InHorDistOf(respectedMaster.Position, 12f) && GenSight.LineOfSight(pawn.Position, respectedMaster.Position, pawn.Map))
			{
				RelationsUtility.TryDevelopBondRelation(respectedMaster, pawn, 0.001f);
			}
		}
	}

	private void GainedOrLostDirectRelation()
	{
		if (Current.ProgramState == ProgramState.Playing && !pawn.Dead && pawn.needs.mood != null)
		{
			pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
		}
	}
}
