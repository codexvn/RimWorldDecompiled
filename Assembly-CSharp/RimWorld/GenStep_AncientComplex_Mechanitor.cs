using System;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x0200204E RID: 8270
	public class GenStep_AncientComplex_Mechanitor : GenStep_AncientComplex
	{
		// Token: 0x0600B5AA RID: 46506 RVA: 0x0034364B File Offset: 0x0034184B
		protected override void GenerateComplex(Map map, ResolveParams parms)
		{
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("ancientMechanitorComplex", parms, null);
			BaseGen.Generate();
		}
	}
}
