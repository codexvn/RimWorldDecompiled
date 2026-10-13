using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001316 RID: 4886
	public static class RemoteShieldUtility
	{
		// Token: 0x060077EF RID: 30703 RVA: 0x00246134 File Offset: 0x00244334
		private static MechShield GetActiveMechShield(Pawn mech)
		{
			if (!mech.Spawned)
			{
				return null;
			}
			List<Thing> thingList = mech.Position.GetThingList(mech.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				MechShield mechShield = thingList[i] as MechShield;
				if (mechShield != null && mechShield.IsTargeting(mech))
				{
					return mechShield;
				}
			}
			return null;
		}

		// Token: 0x060077F0 RID: 30704 RVA: 0x0024618C File Offset: 0x0024438C
		public static Gizmo GetActiveMechShieldGizmo(Pawn mech)
		{
			if (!Find.Selector.IsSelected(mech))
			{
				return null;
			}
			MechShield activeMechShield = RemoteShieldUtility.GetActiveMechShield(mech);
			if (activeMechShield != null)
			{
				return new Gizmo_ProjectileInterceptorHitPoints
				{
					interceptor = activeMechShield.TryGetComp<CompProjectileInterceptor>()
				};
			}
			return null;
		}
	}
}
