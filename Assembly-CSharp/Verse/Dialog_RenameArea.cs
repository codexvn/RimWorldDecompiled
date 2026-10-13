using System;

namespace Verse
{
	// Token: 0x02000C39 RID: 3129
	public class Dialog_RenameArea : Dialog_Rename<Area>
	{
		// Token: 0x0600501A RID: 20506 RVA: 0x0019CC77 File Offset: 0x0019AE77
		public Dialog_RenameArea(Area area) : base(area)
		{
		}

		// Token: 0x0600501B RID: 20507 RVA: 0x0019CC80 File Offset: 0x0019AE80
		protected override AcceptanceReport NameIsValid(string name)
		{
			AcceptanceReport result = base.NameIsValid(name);
			if (!result.Accepted)
			{
				return result;
			}
			if (this.renaming.Map.areaManager.AllAreas.Any((Area a) => a != this.renaming && a.Label == name))
			{
				return "NameIsInUse".Translate();
			}
			return true;
		}
	}
}
