using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A13 RID: 14867
	public class QuestNode_AddShipJob : QuestNode
	{
		// Token: 0x17003344 RID: 13124
		// (get) Token: 0x060145E6 RID: 83430 RVA: 0x00002C42 File Offset: 0x00000E42
		protected virtual ShipJobDef DefaultShipJobDef
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060145E7 RID: 83431 RVA: 0x0061695C File Offset: 0x00614B5C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			ShipJob shipJob = ShipJobMaker.MakeShipJob(this.jobDef.GetValue(slate) ?? this.DefaultShipJobDef);
			this.AddJobVars(shipJob, slate);
			QuestPart_AddShipJob part = new QuestPart_AddShipJob
			{
				inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				shipJob = shipJob,
				shipJobStartMode = this.shipJobStartMode.GetValue(slate).GetValueOrDefault(),
				transportShip = this.transportShip.GetValue(slate)
			};
			QuestGen.quest.AddPart(part);
		}

		// Token: 0x060145E8 RID: 83432 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void AddJobVars(ShipJob shipJob, Slate slate)
		{
		}

		// Token: 0x060145E9 RID: 83433 RVA: 0x00616A03 File Offset: 0x00614C03
		protected override bool TestRunInt(Slate slate)
		{
			return this.jobDef.GetValue(slate) != null || this.DefaultShipJobDef != null;
		}

		// Token: 0x0400E0F6 RID: 57590
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E0F7 RID: 57591
		public SlateRef<TransportShip> transportShip;

		// Token: 0x0400E0F8 RID: 57592
		public SlateRef<ShipJobDef> jobDef;

		// Token: 0x0400E0F9 RID: 57593
		public SlateRef<ShipJobStartMode?> shipJobStartMode;
	}
}
