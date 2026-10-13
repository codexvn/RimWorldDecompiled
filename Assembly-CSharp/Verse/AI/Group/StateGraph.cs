using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.AI.Group
{
	// Token: 0x020010AA RID: 4266
	public class StateGraph : IDisposable
	{
		// Token: 0x17001038 RID: 4152
		// (get) Token: 0x060068B8 RID: 26808 RVA: 0x0021130D File Offset: 0x0020F50D
		// (set) Token: 0x060068B9 RID: 26809 RVA: 0x0021131B File Offset: 0x0020F51B
		public LordToil StartingToil
		{
			get
			{
				return this.lordToils[0];
			}
			set
			{
				if (this.lordToils.Contains(value))
				{
					this.lordToils.Remove(value);
				}
				this.lordToils.Insert(0, value);
			}
		}

		// Token: 0x060068BA RID: 26810 RVA: 0x00211345 File Offset: 0x0020F545
		public void AddToil(LordToil toil)
		{
			this.lordToils.Add(toil);
		}

		// Token: 0x060068BB RID: 26811 RVA: 0x00211353 File Offset: 0x0020F553
		public void AddTransition(Transition transition, bool highPriority = false)
		{
			if (highPriority)
			{
				this.transitions.Insert(0, transition);
				return;
			}
			this.transitions.Add(transition);
		}

		// Token: 0x060068BC RID: 26812 RVA: 0x00211374 File Offset: 0x0020F574
		public StateGraph AttachSubgraph(StateGraph subGraph)
		{
			for (int i = 0; i < subGraph.lordToils.Count; i++)
			{
				this.lordToils.Add(subGraph.lordToils[i]);
			}
			for (int j = 0; j < subGraph.transitions.Count; j++)
			{
				this.transitions.Add(subGraph.transitions[j]);
			}
			return subGraph;
		}

		// Token: 0x060068BD RID: 26813 RVA: 0x002113DC File Offset: 0x0020F5DC
		public void ErrorCheck()
		{
			if (this.lordToils.Count == 0)
			{
				Log.Error("Graph has 0 lord toils.");
			}
			using (IEnumerator<LordToil> enumerator = this.lordToils.Distinct<LordToil>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LordToil toil = enumerator.Current;
					int num = this.lordToils.Count((LordToil s) => s == toil);
					if (num != 1)
					{
						string[] array = new string[5];
						array[0] = "Graph has lord toil ";
						int num2 = 1;
						LordToil toil2 = toil;
						array[num2] = ((toil2 != null) ? toil2.ToString() : null);
						array[2] = " registered ";
						array[3] = num.ToString();
						array[4] = " times.";
						Log.Error(string.Concat(array));
					}
				}
			}
			using (List<Transition>.Enumerator enumerator2 = this.transitions.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Transition trans = enumerator2.Current;
					int num3 = this.transitions.Count((Transition t) => t == trans);
					if (num3 != 1)
					{
						string[] array2 = new string[5];
						array2[0] = "Graph has transition ";
						int num4 = 1;
						Transition trans2 = trans;
						array2[num4] = ((trans2 != null) ? trans2.ToString() : null);
						array2[2] = " registered ";
						array2[3] = num3.ToString();
						array2[4] = " times.";
						Log.Error(string.Concat(array2));
					}
				}
			}
			StateGraph.checkedToils = new HashSet<LordToil>();
			this.CheckForUnregisteredLinkedToilsRecursive(this.StartingToil);
			StateGraph.checkedToils = null;
		}

		// Token: 0x060068BE RID: 26814 RVA: 0x0021157C File Offset: 0x0020F77C
		private void CheckForUnregisteredLinkedToilsRecursive(LordToil toil)
		{
			if (!this.lordToils.Contains(toil))
			{
				Log.Error("Unregistered linked lord toil: " + ((toil != null) ? toil.ToString() : null));
			}
			StateGraph.checkedToils.Add(toil);
			for (int i = 0; i < this.transitions.Count; i++)
			{
				Transition transition = this.transitions[i];
				if (transition.sources.Contains(toil) && !StateGraph.checkedToils.Contains(toil))
				{
					this.CheckForUnregisteredLinkedToilsRecursive(transition.target);
				}
			}
		}

		// Token: 0x060068BF RID: 26815 RVA: 0x0021160C File Offset: 0x0020F80C
		public void Dispose()
		{
			foreach (LordToil lordToil in this.lordToils)
			{
				lordToil.Dispose();
			}
		}

		// Token: 0x04004A3D RID: 19005
		public List<LordToil> lordToils = new List<LordToil>();

		// Token: 0x04004A3E RID: 19006
		public List<Transition> transitions = new List<Transition>();

		// Token: 0x04004A3F RID: 19007
		private static HashSet<LordToil> checkedToils;
	}
}
