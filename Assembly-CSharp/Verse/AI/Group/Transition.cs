using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x020010AD RID: 4269
	public class Transition
	{
		// Token: 0x17001039 RID: 4153
		// (get) Token: 0x060068C5 RID: 26821 RVA: 0x00211690 File Offset: 0x0020F890
		public Map Map
		{
			get
			{
				return this.target.Map;
			}
		}

		// Token: 0x060068C6 RID: 26822 RVA: 0x002116A0 File Offset: 0x0020F8A0
		public Transition(LordToil firstSource, LordToil target, bool canMoveToSameState = false, bool updateDutiesIfMovedToSameState = true)
		{
			this.canMoveToSameState = canMoveToSameState;
			this.updateDutiesIfMovedToSameState = updateDutiesIfMovedToSameState;
			this.target = target;
			this.sources = new List<LordToil>();
			this.AddSource(firstSource);
		}

		// Token: 0x060068C7 RID: 26823 RVA: 0x00211704 File Offset: 0x0020F904
		public void AddSource(LordToil source)
		{
			if (this.sources.Contains(source))
			{
				Log.Error("Double-added source to Transition: " + ((source != null) ? source.ToString() : null));
				return;
			}
			if (!this.canMoveToSameState && this.target == source)
			{
				Log.Error("Transition !canMoveToSameState and target is source: " + ((source != null) ? source.ToString() : null));
			}
			this.sources.Add(source);
		}

		// Token: 0x060068C8 RID: 26824 RVA: 0x00211778 File Offset: 0x0020F978
		public void AddSources(IEnumerable<LordToil> sources)
		{
			foreach (LordToil source in sources)
			{
				this.AddSource(source);
			}
		}

		// Token: 0x060068C9 RID: 26825 RVA: 0x002117C0 File Offset: 0x0020F9C0
		public void AddSources(params LordToil[] sources)
		{
			for (int i = 0; i < sources.Length; i++)
			{
				this.AddSource(sources[i]);
			}
		}

		// Token: 0x060068CA RID: 26826 RVA: 0x002117E4 File Offset: 0x0020F9E4
		public void AddTrigger(Trigger trigger)
		{
			this.triggers.Add(trigger);
		}

		// Token: 0x060068CB RID: 26827 RVA: 0x002117F2 File Offset: 0x0020F9F2
		public void AddPreAction(TransitionAction action)
		{
			this.preActions.Add(action);
		}

		// Token: 0x060068CC RID: 26828 RVA: 0x00211800 File Offset: 0x0020FA00
		public void AddPostAction(TransitionAction action)
		{
			this.postActions.Add(action);
		}

		// Token: 0x060068CD RID: 26829 RVA: 0x00211810 File Offset: 0x0020FA10
		public void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			for (int i = 0; i < this.triggers.Count; i++)
			{
				this.triggers[i].SourceToilBecameActive(transition, previousToil);
			}
		}

		// Token: 0x060068CE RID: 26830 RVA: 0x00211848 File Offset: 0x0020FA48
		public bool CheckSignal(Lord lord, TriggerSignal signal)
		{
			for (int i = 0; i < this.triggers.Count; i++)
			{
				if (this.triggers[i].ActivateOn(lord, signal))
				{
					if (this.triggers[i].filters != null)
					{
						bool flag = true;
						for (int j = 0; j < this.triggers[i].filters.Count; j++)
						{
							if (!this.triggers[i].filters[j].AllowActivation(lord, signal))
							{
								flag = false;
								break;
							}
						}
						if (!flag)
						{
							goto IL_119;
						}
					}
					if (DebugViewSettings.logLordToilTransitions)
					{
						string[] array = new string[8];
						array[0] = "Transitioning ";
						int num = 1;
						List<LordToil> list = this.sources;
						array[num] = ((list != null) ? list.ToString() : null);
						array[2] = " to ";
						int num2 = 3;
						LordToil lordToil = this.target;
						array[num2] = ((lordToil != null) ? lordToil.ToString() : null);
						array[4] = " by trigger ";
						int num3 = 5;
						Trigger trigger = this.triggers[i];
						array[num3] = ((trigger != null) ? trigger.ToString() : null);
						array[6] = " on signal ";
						int num4 = 7;
						TriggerSignal triggerSignal = signal;
						array[num4] = triggerSignal.ToString();
						Log.Message(string.Concat(array));
					}
					this.Execute(lord);
					return true;
				}
				IL_119:;
			}
			return false;
		}

		// Token: 0x060068CF RID: 26831 RVA: 0x00211984 File Offset: 0x0020FB84
		public void Execute(Lord lord)
		{
			if (!this.canMoveToSameState && this.target == lord.CurLordToil)
			{
				return;
			}
			for (int i = 0; i < this.preActions.Count; i++)
			{
				try
				{
					this.preActions[i].DoAction(this);
				}
				catch (Exception ex)
				{
					string str = "Error in lord's preAction: ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			if (this.target != lord.CurLordToil || this.updateDutiesIfMovedToSameState)
			{
				lord.GotoToil(this.target);
			}
			for (int j = 0; j < this.postActions.Count; j++)
			{
				try
				{
					this.postActions[j].DoAction(this);
				}
				catch (Exception ex3)
				{
					string str2 = "Error in lord's postAction: ";
					Exception ex4 = ex3;
					Log.Error(str2 + ((ex4 != null) ? ex4.ToString() : null));
				}
			}
		}

		// Token: 0x060068D0 RID: 26832 RVA: 0x00211A7C File Offset: 0x0020FC7C
		public override string ToString()
		{
			string text = this.sources.NullOrEmpty<LordToil>() ? "null" : this.sources[0].ToString();
			int num = (this.sources == null) ? 0 : this.sources.Count;
			string text2 = (this.target == null) ? "null" : this.target.ToString();
			return string.Concat(new string[]
			{
				text,
				"(",
				num.ToString(),
				")->",
				text2
			});
		}

		// Token: 0x04004A42 RID: 19010
		public List<LordToil> sources;

		// Token: 0x04004A43 RID: 19011
		public LordToil target;

		// Token: 0x04004A44 RID: 19012
		public List<Trigger> triggers = new List<Trigger>();

		// Token: 0x04004A45 RID: 19013
		public List<TransitionAction> preActions = new List<TransitionAction>();

		// Token: 0x04004A46 RID: 19014
		public List<TransitionAction> postActions = new List<TransitionAction>();

		// Token: 0x04004A47 RID: 19015
		public bool canMoveToSameState;

		// Token: 0x04004A48 RID: 19016
		public bool updateDutiesIfMovedToSameState = true;
	}
}
