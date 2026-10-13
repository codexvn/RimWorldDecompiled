using System;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F61 RID: 12129
	public class CompUseEffect_GiveQuest : CompUseEffect
	{
		// Token: 0x17002C34 RID: 11316
		// (get) Token: 0x06011180 RID: 70016 RVA: 0x004E8855 File Offset: 0x004E6A55
		public CompProperties_UseEffectGiveQuest Props
		{
			get
			{
				return (CompProperties_UseEffectGiveQuest)this.props;
			}
		}

		// Token: 0x06011181 RID: 70017 RVA: 0x004E8864 File Offset: 0x004E6A64
		public override void DoEffect(Pawn user)
		{
			if (this.Props.skipIfMechlinkAlreadySentMechs)
			{
				Mechlink mechlink = this.parent as Mechlink;
				if (mechlink != null && mechlink.sentMechsToPlayer)
				{
					return;
				}
			}
			Slate slate = new Slate();
			slate.Set<float>("points", StorytellerUtility.DefaultThreatPointsNow(user.Map), false);
			slate.Set<Pawn>("asker", user, false);
			slate.Set<Map>("map", user.Map, false);
			if (this.Props.discovered != null)
			{
				slate.Set<bool>("discovered", this.Props.discovered.Value, false);
			}
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(this.Props.quest, slate);
			if (!quest.hidden && quest.root.sendAvailableLetter)
			{
				QuestUtility.SendLetterQuestAvailable(quest, null);
			}
		}
	}
}
