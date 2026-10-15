using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Verse;

public class DynamicPawnRenderNodeSetup_Mutants : DynamicPawnRenderNodeSetup
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

		private List<PawnRenderNodeProperties>.Enumerator _003C_003E7__wrap1;

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
			_003C_003E7__wrap1 = default(List<PawnRenderNodeProperties>.Enumerator);
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
					if (!pawn.IsMutant || !pawn.mutant.Def.HasDefinedGraphicProperties)
					{
						return false;
					}
					_003C_003E7__wrap1 = pawn.mutant.Def.RenderNodeProperties.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					PawnRenderNodeProperties current = _003C_003E7__wrap1.Current;
					if (tree.ShouldAddNodeToTree(current))
					{
						_003C_003E2__current = (node: (PawnRenderNode)Activator.CreateInstance(current.nodeClass, pawn, current, tree), parent: null);
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<PawnRenderNodeProperties>.Enumerator);
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

	public override bool HumanlikeOnly => false;

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
