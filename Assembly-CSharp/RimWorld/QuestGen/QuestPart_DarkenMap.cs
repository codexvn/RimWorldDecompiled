using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BA5 RID: 15269
	public class QuestPart_DarkenMap : QuestPart
	{
		// Token: 0x06014BC4 RID: 84932 RVA: 0x002D92DC File Offset: 0x002D74DC
		public QuestPart_DarkenMap()
		{
		}

		// Token: 0x06014BC5 RID: 84933 RVA: 0x006360FA File Offset: 0x006342FA
		public QuestPart_DarkenMap(string inSignal, MapParent mapParent)
		{
			this.inSignal = inSignal;
			this.mapParent = mapParent;
		}

		// Token: 0x06014BC6 RID: 84934 RVA: 0x00636110 File Offset: 0x00634310
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (this.mapParent.Map == null)
			{
				return;
			}
			if (signal.tag == this.inSignal)
			{
				this.mapParent.Map.gameConditionManager.SetTargetBrightness(0f, 5f);
			}
		}

		// Token: 0x06014BC7 RID: 84935 RVA: 0x00636160 File Offset: 0x00634360
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.mapParent.Map != null)
			{
				GameCondition_UnnaturalDarkness activeCondition = this.mapParent.Map.GameConditionManager.GetActiveCondition<GameCondition_UnnaturalDarkness>();
				if (activeCondition != null)
				{
					activeCondition.Permanent = false;
					activeCondition.TicksLeft = 301;
				}
				this.mapParent.Map.gameConditionManager.SetTargetBrightness(1f, 5f);
			}
		}

		// Token: 0x06014BC8 RID: 84936 RVA: 0x006361CA File Offset: 0x006343CA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
		}

		// Token: 0x0400E6CD RID: 59085
		public string inSignal;

		// Token: 0x0400E6CE RID: 59086
		public MapParent mapParent;
	}
}
