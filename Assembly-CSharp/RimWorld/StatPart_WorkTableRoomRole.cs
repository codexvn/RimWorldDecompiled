using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003730 RID: 14128
	public class StatPart_WorkTableRoomRole : StatPart
	{
		// Token: 0x06013940 RID: 80192 RVA: 0x005CDDA4 File Offset: 0x005CBFA4
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && req.Thing.def.building != null && StatPart_WorkTableRoomRole.Applies(req.Thing))
			{
				val *= req.Thing.def.building.workTableNotInRoomRoleFactor;
			}
		}

		// Token: 0x06013941 RID: 80193 RVA: 0x005CDDF8 File Offset: 0x005CBFF8
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && req.Thing.def.building != null && StatPart_WorkTableRoomRole.Applies(req.Thing))
			{
				return "NotInRoomRole".Translate(req.Thing.def.building.workTableRoomRole.label).CapitalizeFirst() + ": x" + req.Thing.def.building.workTableNotInRoomRoleFactor.ToStringPercent();
			}
			return null;
		}

		// Token: 0x06013942 RID: 80194 RVA: 0x005CDE98 File Offset: 0x005CC098
		public static bool Applies(Thing parent)
		{
			ThingDef def = parent.def;
			bool flag;
			if (def == null)
			{
				flag = (null != null);
			}
			else
			{
				BuildingProperties building = def.building;
				flag = (((building != null) ? building.workTableRoomRole : null) != null);
			}
			if (!flag)
			{
				return false;
			}
			Room room = parent.GetRoom(RegionType.Set_All);
			return room != null && !room.PsychologicallyOutdoors && room.Role != parent.def.building.workTableRoomRole;
		}

		// Token: 0x06013943 RID: 80195 RVA: 0x005CDEF8 File Offset: 0x005CC0F8
		public static bool WouldApplyToBuildingIfPlaced(ThingDef def, Map map, IntVec3 cell)
		{
			bool flag;
			if (def == null)
			{
				flag = (null != null);
			}
			else
			{
				BuildingProperties building = def.building;
				flag = (((building != null) ? building.workTableRoomRole : null) != null);
			}
			if (!flag || Mathf.Approximately(def.building.workTableNotInRoomRoleFactor, 1f))
			{
				return false;
			}
			Room room = cell.GetRoom(map);
			return room != null && !room.PsychologicallyOutdoors && room.Role != def.building.workTableRoomRole && room.GetRoomRoleIfBuildingPlaced(def) != def.building.workTableRoomRole;
		}
	}
}
