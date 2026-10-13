using System;

namespace RimWorld
{
	// Token: 0x020033D0 RID: 13264
	public class ITab_Shells : ITab_Storage
	{
		// Token: 0x170030B7 RID: 12471
		// (get) Token: 0x06012A27 RID: 76327 RVA: 0x00583AB4 File Offset: 0x00581CB4
		protected override IStoreSettingsParent SelStoreSettingsParent
		{
			get
			{
				IStoreSettingsParent selStoreSettingsParent = base.SelStoreSettingsParent;
				if (selStoreSettingsParent != null)
				{
					return selStoreSettingsParent;
				}
				Building_TurretGun building_TurretGun = base.SelObject as Building_TurretGun;
				if (building_TurretGun != null)
				{
					return base.GetThingOrThingCompStoreSettingsParent(building_TurretGun.gun);
				}
				return null;
			}
		}

		// Token: 0x170030B8 RID: 12472
		// (get) Token: 0x06012A28 RID: 76328 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool IsPrioritySettingVisible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06012A29 RID: 76329 RVA: 0x00583AEA File Offset: 0x00581CEA
		public ITab_Shells()
		{
			this.labelKey = "TabShells";
			this.tutorTag = "Shells";
		}
	}
}
