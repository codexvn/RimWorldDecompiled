using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse;

public class DynamicPawnRenderNodeSetup_Traits : DynamicPawnRenderNodeSetup
{
	[CompilerGenerated]
	private sealed class _003CGetDynamicNodes_003Ed__2 : IEnumerable<(PawnRenderNode node, PawnRenderNode parent)>, IEnumerable, IEnumerator<(PawnRenderNode node, PawnRenderNode parent)>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private (PawnRenderNode node, PawnRenderNode parent) _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private PawnRenderTree tree;

		public PawnRenderTree _003C_003E3__tree;

		private List<Trait>.Enumerator _003C_003E7__wrap1;

		private Trait _003Ct_003E5__3;

		private List<PawnRenderNodeProperties>.Enumerator _003C_003E7__wrap3;

		(PawnRenderNode node, PawnRenderNode parent) IEnumerator<(PawnRenderNode, PawnRenderNode)>.Current
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
		public _003CGetDynamicNodes_003Ed__2(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<Trait>.Enumerator);
			_003Ct_003E5__3 = null;
			_003C_003E7__wrap3 = default(List<PawnRenderNodeProperties>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -4;
					goto IL_013a;
				}
				_003C_003E1__state = -1;
				if (pawn.story?.traits == null)
				{
					return false;
				}
				_003C_003E7__wrap1 = pawn.story.traits.allTraits.GetEnumerator();
				_003C_003E1__state = -3;
				goto IL_0160;
				IL_0160:
				do
				{
					if (_003C_003E7__wrap1.MoveNext())
					{
						_003Ct_003E5__3 = _003C_003E7__wrap1.Current;
						continue;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<Trait>.Enumerator);
					return false;
				}
				while (_003Ct_003E5__3.Suppressed || !_003Ct_003E5__3.CurrentData.HasDefinedGraphicProperties);
				_003C_003E7__wrap3 = _003Ct_003E5__3.CurrentData.RenderNodeProperties.GetEnumerator();
				_003C_003E1__state = -4;
				goto IL_013a;
				IL_013a:
				while (_003C_003E7__wrap3.MoveNext())
				{
					PawnRenderNodeProperties current = _003C_003E7__wrap3.Current;
					if (tree.ShouldAddNodeToTree(current))
					{
						PawnRenderNode pawnRenderNode = (PawnRenderNode)Activator.CreateInstance(current.nodeClass, pawn, current, tree);
						pawnRenderNode.trait = _003Ct_003E5__3;
						_003C_003E2__current = (node: pawnRenderNode, parent: null);
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap3 = default(List<PawnRenderNodeProperties>.Enumerator);
				_003Ct_003E5__3 = null;
				goto IL_0160;
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			((IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<(PawnRenderNode node, PawnRenderNode parent)> IEnumerable<(PawnRenderNode, PawnRenderNode)>.GetEnumerator()
		{
			_003CGetDynamicNodes_003Ed__2 _003CGetDynamicNodes_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetDynamicNodes_003Ed__ = this;
			}
			else
			{
				_003CGetDynamicNodes_003Ed__ = new _003CGetDynamicNodes_003Ed__2(0);
			}
			_003CGetDynamicNodes_003Ed__.pawn = _003C_003E3__pawn;
			_003CGetDynamicNodes_003Ed__.tree = _003C_003E3__tree;
			return _003CGetDynamicNodes_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<(PawnRenderNode, PawnRenderNode)>)this).GetEnumerator();
		}
	}

	public override bool HumanlikeOnly => true;

	[IteratorStateMachine(typeof(_003CGetDynamicNodes_003Ed__2))]
	public override IEnumerable<(PawnRenderNode node, PawnRenderNode parent)> GetDynamicNodes(Pawn pawn, PawnRenderTree tree)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetDynamicNodes_003Ed__2(-2)
		{
			_003C_003E3__pawn = pawn,
			_003C_003E3__tree = tree
		};
	}
}
