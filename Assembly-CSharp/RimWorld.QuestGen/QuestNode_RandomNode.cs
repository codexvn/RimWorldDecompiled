using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.QuestGen;

public class QuestNode_RandomNode : QuestNode
{
	[CompilerGenerated]
	private sealed class _003CGetNodesCanRun_003Ed__3 : IEnumerable<QuestNode>, IEnumerable, IEnumerator<QuestNode>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private QuestNode _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestNode_RandomNode _003C_003E4__this;

		private Slate slate;

		public Slate _003C_003E3__slate;

		private int _003Ci_003E5__2;

		QuestNode IEnumerator<QuestNode>.Current
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
		public _003CGetNodesCanRun_003Ed__3(int _003C_003E1__state)
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
			QuestNode_RandomNode questNode_RandomNode = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0094;
			}
			_003C_003E1__state = -1;
			_003Ci_003E5__2 = 0;
			goto IL_00a4;
			IL_0094:
			_003Ci_003E5__2++;
			goto IL_00a4;
			IL_00a4:
			if (_003Ci_003E5__2 < questNode_RandomNode.nodes.Count)
			{
				if (questNode_RandomNode.nodes[_003Ci_003E5__2].SelectionWeight(slate) > 0f && questNode_RandomNode.nodes[_003Ci_003E5__2].TestRun(slate.DeepCopy()))
				{
					_003C_003E2__current = questNode_RandomNode.nodes[_003Ci_003E5__2];
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0094;
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
		IEnumerator<QuestNode> IEnumerable<QuestNode>.GetEnumerator()
		{
			_003CGetNodesCanRun_003Ed__3 _003CGetNodesCanRun_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetNodesCanRun_003Ed__ = this;
			}
			else
			{
				_003CGetNodesCanRun_003Ed__ = new _003CGetNodesCanRun_003Ed__3(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetNodesCanRun_003Ed__.slate = _003C_003E3__slate;
			return _003CGetNodesCanRun_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<QuestNode>)this).GetEnumerator();
		}
	}

	public List<QuestNode> nodes = new List<QuestNode>();

	protected override bool TestRunInt(Slate slate)
	{
		QuestNode questNode = GetNodesCanRun(slate).FirstOrDefault();
		if (questNode == null)
		{
			return false;
		}
		questNode.TestRun(slate);
		return true;
	}

	protected override void RunInt()
	{
		if (GetNodesCanRun(QuestGen.slate).TryRandomElementByWeight((QuestNode e) => e.SelectionWeight(QuestGen.slate), out var result))
		{
			result.Run();
		}
	}

	[IteratorStateMachine(typeof(_003CGetNodesCanRun_003Ed__3))]
	private IEnumerable<QuestNode> GetNodesCanRun(Slate slate)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetNodesCanRun_003Ed__3(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__slate = slate
		};
	}
}
