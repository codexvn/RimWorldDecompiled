using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020030D5 RID: 12501
	public static class BuildDesignatorUtility
	{
		// Token: 0x060118E6 RID: 71910 RVA: 0x0050B148 File Offset: 0x00509348
		public static void TryDrawPowerGridAndAnticipatedConnection(BuildableDef def, Rot4 rotation)
		{
			ThingDef thingDef = def as ThingDef;
			if (thingDef != null && (thingDef.EverTransmitsPower || thingDef.ConnectToPower))
			{
				OverlayDrawHandler.DrawPowerGridOverlayThisFrame();
				if (thingDef.ConnectToPower)
				{
					IntVec3 intVec = UI.MouseCell();
					IntVec3 intVec2 = intVec;
					if (thingDef.building.isAttachment)
					{
						Thing wallAttachedTo = GenConstruct.GetWallAttachedTo(intVec, rotation, Find.CurrentMap);
						if (wallAttachedTo != null)
						{
							intVec2 = wallAttachedTo.Position;
						}
					}
					CompPower compPower = PowerConnectionMaker.BestTransmitterForConnector(intVec2, Find.CurrentMap, null);
					if (compPower != null && !compPower.parent.Position.Fogged(compPower.parent.Map))
					{
						PowerNetGraphics.RenderAnticipatedWirePieceConnecting(intVec2, rotation, def.Size, compPower.parent);
					}
				}
			}
		}
	}
}
