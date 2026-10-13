using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BD7 RID: 15319
	public abstract class QuestNode
	{
		// Token: 0x06014CC0 RID: 85184 RVA: 0x0063B050 File Offset: 0x00639250
		public QuestNode()
		{
			this.myTypeShort = base.GetType().Name;
			if (this.myTypeShort.StartsWith("QuestNode_"))
			{
				this.myTypeShort = this.myTypeShort.Substring("QuestNode_".Length);
			}
		}

		// Token: 0x06014CC1 RID: 85185 RVA: 0x0063B0A4 File Offset: 0x006392A4
		public float SelectionWeight(Slate slate)
		{
			float? value = this.selectionWeight.GetValue(slate);
			if (value == null)
			{
				return 1f;
			}
			return value.GetValueOrDefault();
		}

		// Token: 0x06014CC2 RID: 85186 RVA: 0x0063B0D4 File Offset: 0x006392D4
		public void Run()
		{
			if (DeepProfiler.enabled)
			{
				DeepProfiler.Start(this.ToString());
			}
			try
			{
				this.RunInt();
			}
			catch (Exception ex)
			{
				string[] array = new string[6];
				array[0] = "Exception running ";
				array[1] = base.GetType().Name;
				array[2] = ": ";
				int num = 3;
				Exception ex2 = ex;
				array[num] = ((ex2 != null) ? ex2.ToString() : null);
				array[4] = "\n\nSlate vars:\n";
				array[5] = QuestGen.slate.ToString();
				Log.Error(string.Concat(array));
			}
			if (DeepProfiler.enabled)
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06014CC3 RID: 85187 RVA: 0x0063B174 File Offset: 0x00639374
		public bool TestRun(Slate slate)
		{
			bool result;
			try
			{
				Action<QuestNode, Slate> action;
				if (slate.TryGet<Action<QuestNode, Slate>>("testRunCallback", out action, false) && action != null)
				{
					action(this, slate);
				}
				result = this.TestRunInt(slate);
			}
			catch (Exception arg)
			{
				Log.Error(string.Format("Exception test running {0}: {1}\n\nSlate vars:\n{2}", base.GetType().Name, arg, slate));
				result = false;
			}
			finally
			{
			}
			return result;
		}

		// Token: 0x06014CC4 RID: 85188
		protected abstract void RunInt();

		// Token: 0x06014CC5 RID: 85189
		protected abstract bool TestRunInt(Slate slate);

		// Token: 0x06014CC6 RID: 85190 RVA: 0x0029EF31 File Offset: 0x0029D131
		public override string ToString()
		{
			return base.GetType().Name;
		}

		// Token: 0x0400E780 RID: 59264
		[Unsaved(false)]
		[TranslationHandle]
		public string myTypeShort;

		// Token: 0x0400E781 RID: 59265
		public SlateRef<float?> selectionWeight;
	}
}
