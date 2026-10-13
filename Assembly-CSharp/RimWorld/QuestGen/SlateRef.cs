using System;
using System.Text.RegularExpressions;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003C01 RID: 15361
	public struct SlateRef<T> : ISlateRef, IEquatable<SlateRef<T>>
	{
		// Token: 0x17003385 RID: 13189
		// (get) Token: 0x06014D51 RID: 85329 RVA: 0x0063DB3D File Offset: 0x0063BD3D
		// (set) Token: 0x06014D52 RID: 85330 RVA: 0x0063DB45 File Offset: 0x0063BD45
		string ISlateRef.SlateRef
		{
			get
			{
				return this.slateRef;
			}
			set
			{
				this.slateRef = value;
			}
		}

		// Token: 0x06014D53 RID: 85331 RVA: 0x0063DB45 File Offset: 0x0063BD45
		public SlateRef(string slateRef)
		{
			this.slateRef = slateRef;
		}

		// Token: 0x06014D54 RID: 85332 RVA: 0x0063DB50 File Offset: 0x0063BD50
		public T GetValue(Slate slate)
		{
			T result;
			this.TryGetValue(slate, out result);
			return result;
		}

		// Token: 0x06014D55 RID: 85333 RVA: 0x0063DB68 File Offset: 0x0063BD68
		public bool TryGetValue(Slate slate, out T value)
		{
			return this.TryGetConvertedValue<T>(slate, out value);
		}

		// Token: 0x06014D56 RID: 85334 RVA: 0x0063DB74 File Offset: 0x0063BD74
		public bool TryGetConvertedValue<TAnything>(Slate slate, out TAnything value)
		{
			if (this.slateRef == null)
			{
				value = default(TAnything);
				return true;
			}
			SlateRef<T>.tmpCurSlate = slate;
			string text = SlateRef<T>.HighPriorityVarsRegex.Replace(this.slateRef, new MatchEvaluator(SlateRef<T>.RegexMatchEvaluatorConcatenate));
			object obj;
			bool flag;
			if (!SlateRefUtility.CheckSingleVariableSyntax(text, slate, out obj, out flag))
			{
				obj = SlateRef<T>.MathExprRegex.Replace(text, SlateRef<T>.RegexMatchEvaluatorEvaluateMathExpressionCached);
				obj = SlateRef<T>.VarsRegex.Replace((string)obj, SlateRef<T>.RegexMatchEvaluatorConcatenateCached);
				flag = true;
			}
			SlateRef<T>.tmpCurSlate = null;
			if (!flag)
			{
				value = default(TAnything);
				return false;
			}
			if (obj == null)
			{
				value = default(TAnything);
				return true;
			}
			if (obj is TAnything)
			{
				value = (TAnything)((object)obj);
				return true;
			}
			if (ConvertHelper.CanConvert<TAnything>(obj))
			{
				value = ConvertHelper.Convert<TAnything>(obj);
				return true;
			}
			Log.Error(string.Concat(new string[]
			{
				"Could not convert SlateRef \"",
				this.slateRef,
				"\" (",
				obj.GetType().Name,
				") to ",
				typeof(TAnything).Name
			}));
			value = default(TAnything);
			return false;
		}

		// Token: 0x06014D57 RID: 85335 RVA: 0x0063DC90 File Offset: 0x0063BE90
		private static string RegexMatchEvaluatorConcatenate(Match match)
		{
			string value = match.Groups[1].Value;
			object obj;
			if (!SlateRef<T>.tmpCurSlate.TryGet<object>(value, out obj, false))
			{
				return "";
			}
			if (obj == null)
			{
				return "";
			}
			return obj.ToString();
		}

		// Token: 0x06014D58 RID: 85336 RVA: 0x0063DCD4 File Offset: 0x0063BED4
		private static string RegexMatchEvaluatorConcatenateZeroIfEmpty(Match match)
		{
			string value = match.Groups[1].Value;
			object obj;
			if (!SlateRef<T>.tmpCurSlate.TryGet<object>(value, out obj, false))
			{
				Log.ErrorOnce("Tried to use variable \"" + value + "\" in a math expression but it doesn't exist.", value.GetHashCode() ^ 194857119);
				return "0";
			}
			if (obj == null)
			{
				return "0";
			}
			string text = obj.ToString();
			if (text == "")
			{
				return "0";
			}
			return text;
		}

		// Token: 0x06014D59 RID: 85337 RVA: 0x0063DD50 File Offset: 0x0063BF50
		private static string RegexMatchEvaluatorResolveMathExpression(Match match)
		{
			string text = match.Groups[1].Value;
			text = SlateRef<T>.VarsRegex.Replace(text, SlateRef<T>.RegexMatchEvaluatorConcatenateZeroIfEmptyCached);
			return MathEvaluator.Evaluate(text).ToString();
		}

		// Token: 0x06014D5A RID: 85338 RVA: 0x0063DD8E File Offset: 0x0063BF8E
		public override bool Equals(object obj)
		{
			return obj is SlateRef<T> && this.Equals((SlateRef<T>)obj);
		}

		// Token: 0x06014D5B RID: 85339 RVA: 0x0063DDA6 File Offset: 0x0063BFA6
		public bool Equals(SlateRef<T> other)
		{
			return this == other;
		}

		// Token: 0x06014D5C RID: 85340 RVA: 0x0063DDB4 File Offset: 0x0063BFB4
		public static bool operator ==(SlateRef<T> a, SlateRef<T> b)
		{
			return a.slateRef == b.slateRef;
		}

		// Token: 0x06014D5D RID: 85341 RVA: 0x0063DDC7 File Offset: 0x0063BFC7
		public static bool operator !=(SlateRef<T> a, SlateRef<T> b)
		{
			return !(a == b);
		}

		// Token: 0x06014D5E RID: 85342 RVA: 0x0063DDD3 File Offset: 0x0063BFD3
		public static implicit operator SlateRef<T>(T t)
		{
			return new SlateRef<T>((t != null) ? t.ToString() : null);
		}

		// Token: 0x06014D5F RID: 85343 RVA: 0x0063DDF2 File Offset: 0x0063BFF2
		public override int GetHashCode()
		{
			if (this.slateRef == null)
			{
				return 0;
			}
			return this.slateRef.GetHashCode();
		}

		// Token: 0x06014D60 RID: 85344 RVA: 0x0063DE0C File Offset: 0x0063C00C
		public string ToString(Slate slate)
		{
			string result;
			this.TryGetConvertedValue<string>(slate, out result);
			return result;
		}

		// Token: 0x06014D61 RID: 85345 RVA: 0x0063DE24 File Offset: 0x0063C024
		public override string ToString()
		{
			if (!QuestGen.Working)
			{
				return this.slateRef;
			}
			return this.ToString(QuestGen.slate);
		}

		// Token: 0x0400E84B RID: 59467
		public const string SlateRefFieldName = "slateRef";

		// Token: 0x0400E84C RID: 59468
		[MustTranslate_SlateRef]
		private string slateRef;

		// Token: 0x0400E84D RID: 59469
		private static Slate tmpCurSlate;

		// Token: 0x0400E84E RID: 59470
		private static readonly Regex VarsRegex = new Regex("\\$([a-zA-Z0-1_/]*)");

		// Token: 0x0400E84F RID: 59471
		private static readonly Regex HighPriorityVarsRegex = new Regex("\\(\\(\\$([a-zA-Z0-1_/]*)\\)\\)");

		// Token: 0x0400E850 RID: 59472
		private static readonly Regex MathExprRegex = new Regex("\\$\\((.*)\\)");

		// Token: 0x0400E851 RID: 59473
		private static MatchEvaluator RegexMatchEvaluatorConcatenateCached = new MatchEvaluator(SlateRef<T>.RegexMatchEvaluatorConcatenate);

		// Token: 0x0400E852 RID: 59474
		private static MatchEvaluator RegexMatchEvaluatorConcatenateZeroIfEmptyCached = new MatchEvaluator(SlateRef<T>.RegexMatchEvaluatorConcatenateZeroIfEmpty);

		// Token: 0x0400E853 RID: 59475
		private static MatchEvaluator RegexMatchEvaluatorEvaluateMathExpressionCached = new MatchEvaluator(SlateRef<T>.RegexMatchEvaluatorResolveMathExpression);
	}
}
