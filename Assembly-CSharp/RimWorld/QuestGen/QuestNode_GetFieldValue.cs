using System;
using System.Reflection;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A46 RID: 14918
	public class QuestNode_GetFieldValue : QuestNode
	{
		// Token: 0x0601468F RID: 83599 RVA: 0x00618C52 File Offset: 0x00616E52
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x06014690 RID: 83600 RVA: 0x00618C5C File Offset: 0x00616E5C
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x06014691 RID: 83601 RVA: 0x00618C6C File Offset: 0x00616E6C
		private void SetVars(Slate slate)
		{
			object obj = (this.type.GetValue(slate) != null) ? ConvertHelper.Convert(this.obj.GetValue(slate), this.type.GetValue(slate)) : this.obj.GetValue(slate);
			FieldInfo fieldInfo = obj.GetType().GetField(this.field.GetValue(slate), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (fieldInfo == null)
			{
				Log.Error("QuestNode error: " + obj.GetType().Name + " doesn't have a field named " + this.field.GetValue(slate));
				return;
			}
			slate.Set<object>(this.storeAs.GetValue(slate), fieldInfo.GetValue(obj), false);
		}

		// Token: 0x0400E1A5 RID: 57765
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E1A6 RID: 57766
		[NoTranslate]
		public SlateRef<object> obj;

		// Token: 0x0400E1A7 RID: 57767
		[NoTranslate]
		public SlateRef<string> field;

		// Token: 0x0400E1A8 RID: 57768
		public SlateRef<Type> type;
	}
}
