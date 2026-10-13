using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200372B RID: 14123
	public class StatPart_UnfinishedThingIngredientsMass : StatPart
	{
		// Token: 0x0601392D RID: 80173 RVA: 0x005CDA04 File Offset: 0x005CBC04
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x0601392E RID: 80174 RVA: 0x005CDA24 File Offset: 0x005CBC24
		public override string ExplanationPart(StatRequest req)
		{
			float mass;
			if (this.TryGetValue(req, out mass))
			{
				return "StatsReport_IngredientsMass".Translate() + ": " + mass.ToStringMassOffset();
			}
			return null;
		}

		// Token: 0x0601392F RID: 80175 RVA: 0x005CDA64 File Offset: 0x005CBC64
		private bool TryGetValue(StatRequest req, out float value)
		{
			UnfinishedThing unfinishedThing = req.Thing as UnfinishedThing;
			if (unfinishedThing == null)
			{
				value = 0f;
				return false;
			}
			float num = 0f;
			for (int i = 0; i < unfinishedThing.ingredients.Count; i++)
			{
				num += unfinishedThing.ingredients[i].GetStatValue(StatDefOf.Mass, true, -1) * (float)unfinishedThing.ingredients[i].stackCount;
			}
			value = num;
			return true;
		}
	}
}
