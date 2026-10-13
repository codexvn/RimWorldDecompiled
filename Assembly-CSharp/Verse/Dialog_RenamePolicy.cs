using System;
using System.Text.RegularExpressions;
using RimWorld;

namespace Verse
{
	// Token: 0x02000C3C RID: 3132
	public class Dialog_RenamePolicy : Dialog_Rename<Policy>
	{
		// Token: 0x06005020 RID: 20512 RVA: 0x0019CD88 File Offset: 0x0019AF88
		public Dialog_RenamePolicy(Policy policy) : base(policy)
		{
		}

		// Token: 0x06005021 RID: 20513 RVA: 0x0019CD94 File Offset: 0x0019AF94
		protected override AcceptanceReport NameIsValid(string name)
		{
			AcceptanceReport result = base.NameIsValid(name);
			if (!result.Accepted)
			{
				return result;
			}
			if (!Dialog_RenamePolicy.ValidNameRegex.IsMatch(name))
			{
				return "InvalidName".Translate();
			}
			return true;
		}

		// Token: 0x040039BF RID: 14783
		private static readonly Regex ValidNameRegex = new Regex("^[\\p{L}0-9 '\\-]*$");
	}
}
