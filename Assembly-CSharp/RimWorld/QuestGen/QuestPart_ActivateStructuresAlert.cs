using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BB7 RID: 15287
	public class QuestPart_ActivateStructuresAlert : QuestPart_Alert
	{
		// Token: 0x06014C2C RID: 85036 RVA: 0x0062423D File Offset: 0x0062243D
		public QuestPart_ActivateStructuresAlert()
		{
		}

		// Token: 0x06014C2D RID: 85037 RVA: 0x0063852C File Offset: 0x0063672C
		public QuestPart_ActivateStructuresAlert(List<Thing> structures, string inSignalEnable, string inSignalDisable)
		{
			this.structures = structures;
			this.inSignalEnable = inSignalEnable;
			this.inSignalDisable = inSignalDisable;
			this.lookTargets = structures;
		}

		// Token: 0x1700337A RID: 13178
		// (get) Token: 0x06014C2E RID: 85038 RVA: 0x00638558 File Offset: 0x00636758
		public override string AlertLabel
		{
			get
			{
				return "StructuresActivatedAlertLabel".Translate() + ": " + this.structures.Count(delegate(Thing s)
				{
					bool? flag;
					if (s == null)
					{
						flag = null;
					}
					else
					{
						CompVoidStructure compVoidStructure = s.TryGetComp<CompVoidStructure>();
						flag = ((compVoidStructure != null) ? new bool?(compVoidStructure.Active) : null);
					}
					bool? flag2 = flag;
					return flag2.GetValueOrDefault();
				}).ToString() + "/" + this.structures.Count.ToString();
			}
		}

		// Token: 0x1700337B RID: 13179
		// (get) Token: 0x06014C2F RID: 85039 RVA: 0x006385CD File Offset: 0x006367CD
		public override string AlertExplanation
		{
			get
			{
				return "StructuresActivatedAlertDescription".Translate();
			}
		}

		// Token: 0x06014C30 RID: 85040 RVA: 0x006385DE File Offset: 0x006367DE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.structures, "structures", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x06014C31 RID: 85041 RVA: 0x006385FC File Offset: 0x006367FC
		public override void Cleanup()
		{
			base.Cleanup();
			this.structures.Clear();
		}

		// Token: 0x0400E722 RID: 59170
		private List<Thing> structures;
	}
}
