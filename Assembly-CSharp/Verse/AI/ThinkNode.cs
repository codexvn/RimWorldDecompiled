using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02001011 RID: 4113
	public abstract class ThinkNode
	{
		// Token: 0x17000FC6 RID: 4038
		// (get) Token: 0x060064C6 RID: 25798 RVA: 0x001FE4C0 File Offset: 0x001FC6C0
		public int UniqueSaveKey
		{
			get
			{
				return this.uniqueSaveKeyInt;
			}
		}

		// Token: 0x17000FC7 RID: 4039
		// (get) Token: 0x060064C7 RID: 25799 RVA: 0x001FE4C8 File Offset: 0x001FC6C8
		public IEnumerable<ThinkNode> ThisAndChildrenRecursive
		{
			get
			{
				yield return this;
				foreach (ThinkNode thinkNode in this.ChildrenRecursive)
				{
					yield return thinkNode;
				}
				IEnumerator<ThinkNode> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000FC8 RID: 4040
		// (get) Token: 0x060064C8 RID: 25800 RVA: 0x001FE4D8 File Offset: 0x001FC6D8
		public IEnumerable<ThinkNode> ChildrenRecursive
		{
			get
			{
				if (this.subNodes.Count == 0)
				{
					yield break;
				}
				Stack<ThinkNode> stack = new Stack<ThinkNode>();
				for (int i = this.subNodes.Count - 1; i >= 0; i--)
				{
					stack.Push(this.subNodes[i]);
				}
				while (stack.Count != 0)
				{
					ThinkNode elem = stack.Pop();
					yield return elem;
					for (int j = elem.subNodes.Count - 1; j >= 0; j--)
					{
						stack.Push(elem.subNodes[j]);
					}
					elem = null;
				}
				yield break;
			}
		}

		// Token: 0x060064C9 RID: 25801 RVA: 0x001FE4E8 File Offset: 0x001FC6E8
		protected ThinkNode()
		{
			this.hasPriority = (this.priority >= 0f || base.GetType().IsMethodOverriden("GetPriority"));
		}

		// Token: 0x060064CA RID: 25802 RVA: 0x001FE540 File Offset: 0x001FC740
		public T FirstNodeOfType<T>(bool includeThis = false) where T : ThinkNode
		{
			if (includeThis)
			{
				T t = this as T;
				if (t != null)
				{
					return t;
				}
			}
			foreach (ThinkNode thinkNode in this.subNodes)
			{
				T t2 = thinkNode.FirstNodeOfType<T>(true);
				if (t2 != null)
				{
					return t2;
				}
			}
			return default(T);
		}

		// Token: 0x060064CB RID: 25803 RVA: 0x001FE5C4 File Offset: 0x001FC7C4
		public bool TryGetPriority(Pawn pawn, out float value)
		{
			if (!this.hasPriority)
			{
				value = 0f;
				return false;
			}
			value = this.GetPriority(pawn);
			return true;
		}

		// Token: 0x060064CC RID: 25804 RVA: 0x001FE5E1 File Offset: 0x001FC7E1
		public virtual float GetPriority(Pawn pawn)
		{
			if (this.priority < 0f)
			{
				Log.ErrorOnce("ThinkNode_PrioritySorter has child node which didn't give a priority: " + ((this != null) ? this.ToString() : null), this.GetHashCode());
				return 0f;
			}
			return this.priority;
		}

		// Token: 0x060064CD RID: 25805
		public abstract ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams);

		// Token: 0x060064CE RID: 25806 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void ResolveSubnodes()
		{
		}

		// Token: 0x060064CF RID: 25807 RVA: 0x001FE620 File Offset: 0x001FC820
		public void ResolveSubnodesAndRecur()
		{
			if (this.uniqueSaveKeyInt != -2)
			{
				return;
			}
			this.ResolveSubnodes();
			for (int i = 0; i < this.subNodes.Count; i++)
			{
				this.subNodes[i].ResolveSubnodesAndRecur();
			}
		}

		// Token: 0x060064D0 RID: 25808 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ResolveReferences()
		{
		}

		// Token: 0x060064D1 RID: 25809 RVA: 0x001FE668 File Offset: 0x001FC868
		public virtual ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode thinkNode = (ThinkNode)Activator.CreateInstance(base.GetType());
			for (int i = 0; i < this.subNodes.Count; i++)
			{
				thinkNode.subNodes.Add(this.subNodes[i].DeepCopy(resolve));
			}
			thinkNode.priority = this.priority;
			thinkNode.leaveJoinableLordIfIssuesJob = this.leaveJoinableLordIfIssuesJob;
			thinkNode.uniqueSaveKeyInt = this.uniqueSaveKeyInt;
			thinkNode.tag = this.tag;
			if (resolve)
			{
				thinkNode.ResolveSubnodesAndRecur();
			}
			ThinkTreeKeyAssigner.AssignSingleKey(thinkNode, 0);
			return thinkNode;
		}

		// Token: 0x060064D2 RID: 25810 RVA: 0x001FE6FA File Offset: 0x001FC8FA
		internal void SetUniqueSaveKey(int key)
		{
			this.uniqueSaveKeyInt = key;
		}

		// Token: 0x060064D3 RID: 25811 RVA: 0x001FE703 File Offset: 0x001FC903
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.uniqueSaveKeyInt, 1157295731);
		}

		// Token: 0x04004803 RID: 18435
		public List<ThinkNode> subNodes = new List<ThinkNode>();

		// Token: 0x04004804 RID: 18436
		public bool leaveJoinableLordIfIssuesJob;

		// Token: 0x04004805 RID: 18437
		protected float priority = -1f;

		// Token: 0x04004806 RID: 18438
		[Unsaved(false)]
		private int uniqueSaveKeyInt = -2;

		// Token: 0x04004807 RID: 18439
		[NoTranslate]
		public string tag;

		// Token: 0x04004808 RID: 18440
		[Unsaved(false)]
		public ThinkNode parent;

		// Token: 0x04004809 RID: 18441
		[Unsaved(false)]
		private bool hasPriority;

		// Token: 0x0400480A RID: 18442
		public const int InvalidSaveKey = -1;

		// Token: 0x0400480B RID: 18443
		protected const int UnresolvedSaveKey = -2;
	}
}
