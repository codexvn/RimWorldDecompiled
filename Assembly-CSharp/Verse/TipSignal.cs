using System;

namespace Verse
{
	// Token: 0x02000BDD RID: 3037
	public struct TipSignal
	{
		// Token: 0x06004DB8 RID: 19896 RVA: 0x0018B125 File Offset: 0x00189325
		public TipSignal(string text, int uniqueId)
		{
			this.text = text;
			this.textGetter = null;
			this.uniqueId = uniqueId;
			this.priority = TooltipPriority.Default;
			this.delay = 0.45f;
		}

		// Token: 0x06004DB9 RID: 19897 RVA: 0x0018B14E File Offset: 0x0018934E
		public TipSignal(string text, int uniqueId, TooltipPriority priority)
		{
			this.text = text;
			this.textGetter = null;
			this.uniqueId = uniqueId;
			this.priority = priority;
			this.delay = 0.45f;
		}

		// Token: 0x06004DBA RID: 19898 RVA: 0x0018B177 File Offset: 0x00189377
		public TipSignal(string text)
		{
			if (text == null)
			{
				text = "";
			}
			this.text = text;
			this.textGetter = null;
			this.uniqueId = text.GetHashCode();
			this.priority = TooltipPriority.Default;
			this.delay = 0.45f;
		}

		// Token: 0x06004DBB RID: 19899 RVA: 0x0018B1AF File Offset: 0x001893AF
		public TipSignal(string text, float delay)
		{
			if (text == null)
			{
				text = "";
			}
			this.text = text;
			this.textGetter = null;
			this.uniqueId = text.GetHashCode();
			this.priority = TooltipPriority.Default;
			this.delay = delay;
		}

		// Token: 0x06004DBC RID: 19900 RVA: 0x0018B1E4 File Offset: 0x001893E4
		public TipSignal(TaggedString text)
		{
			if (text == null)
			{
				text = "";
			}
			this.text = text.Resolve();
			this.textGetter = null;
			this.uniqueId = text.GetHashCode();
			this.priority = TooltipPriority.Default;
			this.delay = 0.45f;
		}

		// Token: 0x06004DBD RID: 19901 RVA: 0x0018B23E File Offset: 0x0018943E
		public TipSignal(Func<string> textGetter, int uniqueId)
		{
			this.text = "";
			this.textGetter = textGetter;
			this.uniqueId = uniqueId;
			this.priority = TooltipPriority.Default;
			this.delay = 0.45f;
		}

		// Token: 0x06004DBE RID: 19902 RVA: 0x0018B26B File Offset: 0x0018946B
		public TipSignal(Func<string> textGetter, int uniqueId, TooltipPriority priority)
		{
			this.text = "";
			this.textGetter = textGetter;
			this.uniqueId = uniqueId;
			this.priority = priority;
			this.delay = 0.45f;
		}

		// Token: 0x06004DBF RID: 19903 RVA: 0x0018B298 File Offset: 0x00189498
		public TipSignal(TipSignal cloneSource)
		{
			this.text = cloneSource.text;
			this.textGetter = null;
			this.priority = cloneSource.priority;
			this.uniqueId = cloneSource.uniqueId;
			this.delay = 0.45f;
		}

		// Token: 0x06004DC0 RID: 19904 RVA: 0x0018B2D0 File Offset: 0x001894D0
		public static implicit operator TipSignal(string str)
		{
			return new TipSignal(str);
		}

		// Token: 0x06004DC1 RID: 19905 RVA: 0x0018B2D8 File Offset: 0x001894D8
		public static implicit operator TipSignal(TaggedString str)
		{
			return new TipSignal(str);
		}

		// Token: 0x06004DC2 RID: 19906 RVA: 0x0018B2E0 File Offset: 0x001894E0
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Tip(",
				this.text,
				", ",
				this.uniqueId.ToString(),
				")"
			});
		}

		// Token: 0x04003732 RID: 14130
		public const float DefaultDelay = 0.45f;

		// Token: 0x04003733 RID: 14131
		public string text;

		// Token: 0x04003734 RID: 14132
		public Func<string> textGetter;

		// Token: 0x04003735 RID: 14133
		public int uniqueId;

		// Token: 0x04003736 RID: 14134
		public TooltipPriority priority;

		// Token: 0x04003737 RID: 14135
		public float delay;
	}
}
