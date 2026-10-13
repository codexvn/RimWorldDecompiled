using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200212B RID: 8491
	internal struct StrikeRecord : IExposable
	{
		// Token: 0x17001C1B RID: 7195
		// (get) Token: 0x0600B923 RID: 47395 RVA: 0x00357029 File Offset: 0x00355229
		public bool Expired
		{
			get
			{
				return Find.TickManager.TicksGame > this.ticksGame + 900000;
			}
		}

		// Token: 0x0600B924 RID: 47396 RVA: 0x00357044 File Offset: 0x00355244
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.ticksGame, "ticksGame", 0, false);
			Scribe_Defs.Look<ThingDef>(ref this.def, "def");
		}

		// Token: 0x0600B925 RID: 47397 RVA: 0x00357090 File Offset: 0x00355290
		public override string ToString()
		{
			string[] array = new string[7];
			array[0] = "(";
			int num = 1;
			IntVec3 intVec = this.cell;
			array[num] = intVec.ToString();
			array[2] = ", ";
			int num2 = 3;
			ThingDef thingDef = this.def;
			array[num2] = ((thingDef != null) ? thingDef.ToString() : null);
			array[4] = ", ";
			array[5] = this.ticksGame.ToString();
			array[6] = ")";
			return string.Concat(array);
		}

		// Token: 0x04007FBF RID: 32703
		public IntVec3 cell;

		// Token: 0x04007FC0 RID: 32704
		public int ticksGame;

		// Token: 0x04007FC1 RID: 32705
		public ThingDef def;

		// Token: 0x04007FC2 RID: 32706
		private const int StrikeRecordExpiryDays = 15;
	}
}
