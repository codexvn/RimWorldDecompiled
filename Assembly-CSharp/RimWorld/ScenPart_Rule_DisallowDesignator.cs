using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002892 RID: 10386
	public class ScenPart_Rule_DisallowDesignator : ScenPart_Rule
	{
		// Token: 0x0600E345 RID: 58181 RVA: 0x00427913 File Offset: 0x00425B13
		protected override void ApplyRule()
		{
			Current.Game.Rules.SetAllowDesignator(this.def.designatorType, false);
		}
	}
}
