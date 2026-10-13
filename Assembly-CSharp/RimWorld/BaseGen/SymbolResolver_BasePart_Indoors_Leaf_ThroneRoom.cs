using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020038F3 RID: 14579
	public class SymbolResolver_BasePart_Indoors_Leaf_ThroneRoom : SymbolResolver
	{
		// Token: 0x060141D6 RID: 82390 RVA: 0x005FD3FC File Offset: 0x005FB5FC
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && rp.faction != null && rp.faction == Faction.OfEmpire && (rp.allowGeneratingThronerooms == null || rp.allowGeneratingThronerooms.Value) && (BaseGen.globalSettings.basePart_worshippedTerminalsResolved >= BaseGen.globalSettings.requiredWorshippedTerminalRooms || !SymbolResolver_BasePart_Indoors_Leaf_WorshippedTerminal.CanResolve("basePart_indoors_leaf", rp)) && (BaseGen.globalSettings.basePart_gravcoresResolved >= BaseGen.globalSettings.requiredGravcoreRooms || !SymbolResolver_BasePart_Indoors_Leaf_Gravcore.CanResolve("basePart_indoors_leaf", rp)) && (BaseGen.globalSettings.basePart_barracksResolved >= BaseGen.globalSettings.minBarracks || BaseGen.globalSettings.basePart_throneRoomsResolved < BaseGen.globalSettings.minThroneRooms) && (BaseGen.globalSettings.basePart_throneRoomsResolved == 0 || BaseGen.globalSettings.basePart_throneRoomsResolved < BaseGen.globalSettings.minThroneRooms);
		}

		// Token: 0x060141D7 RID: 82391 RVA: 0x005FD4E6 File Offset: 0x005FB6E6
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("throneRoom", rp, null);
			BaseGen.globalSettings.basePart_throneRoomsResolved++;
		}
	}
}
