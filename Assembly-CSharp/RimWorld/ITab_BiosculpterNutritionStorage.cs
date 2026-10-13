using System;

namespace RimWorld
{
	// Token: 0x02002C1B RID: 11291
	public class ITab_BiosculpterNutritionStorage : ITab_Storage
	{
		// Token: 0x170027D4 RID: 10196
		// (get) Token: 0x0600FD44 RID: 64836 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool IsPrioritySettingVisible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600FD45 RID: 64837 RVA: 0x00497A83 File Offset: 0x00495C83
		public ITab_BiosculpterNutritionStorage()
		{
			this.labelKey = "Nutrition";
		}
	}
}
