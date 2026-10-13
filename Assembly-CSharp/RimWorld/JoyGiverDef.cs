using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A5D RID: 6749
	public class JoyGiverDef : Def
	{
		// Token: 0x17001731 RID: 5937
		// (get) Token: 0x060097A9 RID: 38825 RVA: 0x002C4AE2 File Offset: 0x002C2CE2
		public JoyGiver Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (JoyGiver)Activator.CreateInstance(this.giverClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x060097AA RID: 38826 RVA: 0x002C4B14 File Offset: 0x002C2D14
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.jobDef != null && this.jobDef.joyKind != this.joyKind)
			{
				string[] array = new string[6];
				array[0] = "jobDef ";
				int num = 1;
				JobDef jobDef = this.jobDef;
				array[num] = ((jobDef != null) ? jobDef.ToString() : null);
				array[2] = " has joyKind ";
				int num2 = 3;
				JoyKindDef joyKindDef = this.jobDef.joyKind;
				array[num2] = ((joyKindDef != null) ? joyKindDef.ToString() : null);
				array[4] = " which does not match our joyKind ";
				int num3 = 5;
				JoyKindDef joyKindDef2 = this.joyKind;
				array[num3] = ((joyKindDef2 != null) ? joyKindDef2.ToString() : null);
				yield return string.Concat(array);
			}
			yield break;
			yield break;
		}

		// Token: 0x040064FF RID: 25855
		public Type giverClass;

		// Token: 0x04006500 RID: 25856
		public float baseChance;

		// Token: 0x04006501 RID: 25857
		public bool requireChair = true;

		// Token: 0x04006502 RID: 25858
		public List<ThingDef> thingDefs;

		// Token: 0x04006503 RID: 25859
		public JobDef jobDef;

		// Token: 0x04006504 RID: 25860
		public bool desireSit = true;

		// Token: 0x04006505 RID: 25861
		public float pctPawnsEverDo = 1f;

		// Token: 0x04006506 RID: 25862
		public bool unroofedOnly;

		// Token: 0x04006507 RID: 25863
		public JoyKindDef joyKind;

		// Token: 0x04006508 RID: 25864
		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		// Token: 0x04006509 RID: 25865
		public bool canDoWhileInBed;

		// Token: 0x0400650A RID: 25866
		public bool requiresEnjoyOutdoors;

		// Token: 0x0400650B RID: 25867
		public bool countsForRecRoom = true;

		// Token: 0x0400650C RID: 25868
		private JoyGiver workerInt;
	}
}
