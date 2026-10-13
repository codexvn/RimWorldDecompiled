using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001722 RID: 5922
	public class LordToilData_Siege : LordToilData
	{
		// Token: 0x06008B6E RID: 35694 RVA: 0x00298B3C File Offset: 0x00296D3C
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.siegeCenter, "siegeCenter", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.baseRadius, "baseRadius", 16f, false);
			Scribe_Values.Look<float>(ref this.blueprintPoints, "blueprintPoints", 0f, false);
			Scribe_Values.Look<float>(ref this.desiredBuilderFraction, "desiredBuilderFraction", 0.5f, false);
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.blueprints.RemoveAll((Blueprint blue) => blue.Destroyed);
			}
			Scribe_Collections.Look<Blueprint>(ref this.blueprints, "blueprints", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x04005A9B RID: 23195
		public IntVec3 siegeCenter;

		// Token: 0x04005A9C RID: 23196
		public float baseRadius = 16f;

		// Token: 0x04005A9D RID: 23197
		public float blueprintPoints;

		// Token: 0x04005A9E RID: 23198
		public float desiredBuilderFraction = 0.5f;

		// Token: 0x04005A9F RID: 23199
		public List<Blueprint> blueprints = new List<Blueprint>();
	}
}
