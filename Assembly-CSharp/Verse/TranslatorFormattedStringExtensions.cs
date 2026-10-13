using System;

namespace Verse
{
	// Token: 0x020003A3 RID: 931
	public static class TranslatorFormattedStringExtensions
	{
		// Token: 0x060017D6 RID: 6102 RVA: 0x00079721 File Offset: 0x00077921
		public static TaggedString Translate(this string key, NamedArgument arg1)
		{
			return key.Translate().Formatted(arg1);
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x0007972F File Offset: 0x0007792F
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2)
		{
			return key.Translate().Formatted(arg1, arg2);
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x0007973E File Offset: 0x0007793E
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			return key.Translate().Formatted(arg1, arg2, arg3);
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x0007974E File Offset: 0x0007794E
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			return key.Translate().Formatted(arg1, arg2, arg3, arg4);
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x00079760 File Offset: 0x00077960
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5)
		{
			return key.Translate().Formatted(arg1, arg2, arg3, arg4, arg5);
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x00079774 File Offset: 0x00077974
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6)
		{
			return key.Translate().Formatted(arg1, arg2, arg3, arg4, arg5, arg6);
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x0007978A File Offset: 0x0007798A
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6, NamedArgument arg7)
		{
			return key.Translate().Formatted(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x000797A4 File Offset: 0x000779A4
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6, NamedArgument arg7, NamedArgument arg8)
		{
			return key.Translate().Formatted(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x000797C9 File Offset: 0x000779C9
		public static TaggedString Translate(this string key, params NamedArgument[] args)
		{
			return key.Translate().Formatted(args);
		}
	}
}
