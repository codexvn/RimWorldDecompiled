using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E06 RID: 7686
	internal class RaidLootDistributor
	{
		// Token: 0x0600AB19 RID: 43801 RVA: 0x0030E814 File Offset: 0x0030CA14
		public RaidLootDistributor(IncidentParms parms, List<Pawn> allPawns, List<Thing> loot)
		{
			this.parms = parms;
			this.allPawns = allPawns;
			this.loot = loot;
			this.unusedPawns = new List<Pawn>(from x in allPawns
			where !x.RaceProps.Animal
			select x);
		}

		// Token: 0x0600AB1A RID: 43802 RVA: 0x0030E86C File Offset: 0x0030CA6C
		public void DistributeLoot()
		{
			this.recipient = this.unusedPawns.MaxBy((Pawn p) => p.kindDef.combatPower);
			this.recipientMassGiven = 0f;
			foreach (IGrouping<ThingDef, Thing> grouping in from t in this.loot
			group t by t.def)
			{
				foreach (Thing item in grouping)
				{
					this.DistributeItem(item);
				}
				this.NextRecipient();
			}
		}

		// Token: 0x0600AB1B RID: 43803 RVA: 0x0030E94C File Offset: 0x0030CB4C
		private void DistributeItem(Thing item)
		{
			int num = item.stackCount;
			int num2 = 0;
			while (num > 0 && num2++ < 5)
			{
				num -= this.TryGiveToRecipient(item, num, false);
				if (num > 0)
				{
					this.NextRecipient();
				}
			}
			if (num > 0)
			{
				this.NextRecipient();
				this.TryGiveToRecipient(item, num, true);
			}
		}

		// Token: 0x0600AB1C RID: 43804 RVA: 0x0030E99C File Offset: 0x0030CB9C
		private int TryGiveToRecipient(Thing item, int count, bool force = false)
		{
			float num = 10f * Mathf.Max(1f, this.recipient.BodySize) - this.recipientMassGiven;
			float statValue = item.GetStatValue(StatDefOf.Mass, true, -1);
			int num2 = force ? count : Mathf.RoundToInt(Mathf.Clamp(num / statValue, 0f, (float)count));
			if (num2 > 0)
			{
				int num3 = this.recipient.inventory.innerContainer.TryAdd(item, num2, true);
				this.recipientMassGiven += (float)num3 * statValue;
				return num3;
			}
			return 0;
		}

		// Token: 0x0600AB1D RID: 43805 RVA: 0x0030EA26 File Offset: 0x0030CC26
		private void NextRecipient()
		{
			this.recipientMassGiven = 0f;
			if (this.unusedPawns.Any<Pawn>())
			{
				this.recipient = this.unusedPawns.Pop<Pawn>();
				return;
			}
			this.recipient = this.allPawns.RandomElement<Pawn>();
		}

		// Token: 0x040075C2 RID: 30146
		private readonly IncidentParms parms;

		// Token: 0x040075C3 RID: 30147
		private readonly List<Pawn> allPawns;

		// Token: 0x040075C4 RID: 30148
		private readonly List<Thing> loot;

		// Token: 0x040075C5 RID: 30149
		private readonly List<Pawn> unusedPawns;

		// Token: 0x040075C6 RID: 30150
		private Pawn recipient;

		// Token: 0x040075C7 RID: 30151
		private float recipientMassGiven;
	}
}
