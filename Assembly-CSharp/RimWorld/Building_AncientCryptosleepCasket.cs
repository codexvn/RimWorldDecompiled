using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020029C8 RID: 10696
	public class Building_AncientCryptosleepCasket : Building_CryptosleepCasket
	{
		// Token: 0x0600EBC7 RID: 60359 RVA: 0x00447A73 File Offset: 0x00445C73
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.groupID, "groupID", 0, false);
		}

		// Token: 0x0600EBC8 RID: 60360 RVA: 0x00447A90 File Offset: 0x00445C90
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (absorbed)
			{
				return;
			}
			if (!this.contentsKnown && this.innerContainer.Count > 0 && dinfo.Def.harmsHealth && dinfo.Instigator != null && dinfo.Instigator.Faction != null)
			{
				bool flag = false;
				using (IEnumerator<Thing> enumerator = ((IEnumerable<Thing>)this.innerContainer).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current is Pawn)
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					this.EjectContents();
				}
			}
			absorbed = false;
		}

		// Token: 0x0600EBC9 RID: 60361 RVA: 0x00447B34 File Offset: 0x00445D34
		public override void EjectContents()
		{
			bool contentsKnown = this.contentsKnown;
			List<Thing> list = null;
			if (!contentsKnown)
			{
				list = new List<Thing>();
				list.AddRange(this.innerContainer);
				list.AddRange(this.UnopenedCasketsInGroup().SelectMany((Building_AncientCryptosleepCasket c) => c.innerContainer));
				list.RemoveDuplicates(null);
			}
			base.EjectContents();
			if (this.ClaimableBy(Faction.OfPlayer))
			{
				this.SetFaction(null, null);
			}
			if (!contentsKnown)
			{
				ThingDef filth_Slime = ThingDefOf.Filth_Slime;
				FilthMaker.TryMakeFilth(base.Position, base.Map, filth_Slime, Rand.Range(8, 12), FilthSourceFlags.None, true);
				foreach (Building_AncientCryptosleepCasket building_AncientCryptosleepCasket in this.UnopenedCasketsInGroup())
				{
					building_AncientCryptosleepCasket.contentsKnown = true;
					building_AncientCryptosleepCasket.EjectContents();
				}
				IEnumerable<Pawn> enumerable = from p in list.OfType<Pawn>().ToList<Pawn>()
				where p.RaceProps.Humanlike && p.GetLord() == null && p.Faction == Faction.OfAncientsHostile
				select p;
				if (enumerable.Any<Pawn>())
				{
					LordMaker.MakeNewLord(Faction.OfAncientsHostile, new LordJob_AssaultColony(Faction.OfAncientsHostile, false, true, false, false, false, false, false), base.Map, enumerable);
				}
			}
		}

		// Token: 0x0600EBCA RID: 60362 RVA: 0x00447C7C File Offset: 0x00445E7C
		private IEnumerable<Building_AncientCryptosleepCasket> UnopenedCasketsInGroup()
		{
			yield return this;
			if (this.groupID != -1)
			{
				foreach (Thing thing in base.Map.listerThings.ThingsOfDef(ThingDefOf.AncientCryptosleepCasket))
				{
					Building_AncientCryptosleepCasket building_AncientCryptosleepCasket = thing as Building_AncientCryptosleepCasket;
					if (building_AncientCryptosleepCasket.groupID == this.groupID && !building_AncientCryptosleepCasket.contentsKnown)
					{
						yield return building_AncientCryptosleepCasket;
					}
				}
				List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x04009C73 RID: 40051
		public int groupID = -1;
	}
}
