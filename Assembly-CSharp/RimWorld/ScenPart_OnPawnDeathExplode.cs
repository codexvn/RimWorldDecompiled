using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002881 RID: 10369
	public class ScenPart_OnPawnDeathExplode : ScenPart
	{
		// Token: 0x0600E2E8 RID: 58088 RVA: 0x004269EF File Offset: 0x00424BEF
		public override void Randomize()
		{
			this.radius = (float)Rand.RangeInclusive(3, 8) - 0.1f;
			this.damage = this.PossibleDamageDefs().RandomElement<DamageDef>();
		}

		// Token: 0x0600E2E9 RID: 58089 RVA: 0x00426A16 File Offset: 0x00424C16
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
			Scribe_Defs.Look<DamageDef>(ref this.damage, "damage");
		}

		// Token: 0x0600E2EA RID: 58090 RVA: 0x00426A44 File Offset: 0x00424C44
		public override string Summary(Scenario scen)
		{
			return "ScenPart_OnPawnDeathExplode".Translate(this.damage.label, this.radius.ToString());
		}

		// Token: 0x0600E2EB RID: 58091 RVA: 0x00426A78 File Offset: 0x00424C78
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			Widgets.TextFieldNumericLabeled<float>(scenPartRect.TopHalf(), "radius".Translate(), ref this.radius, ref this.radiusBuf, 0f, 1E+09f);
			if (Widgets.ButtonText(scenPartRect.BottomHalf(), this.damage.LabelCap, true, true, true, null))
			{
				FloatMenuUtility.MakeMenu<DamageDef>(this.PossibleDamageDefs(), (DamageDef d) => d.LabelCap, (DamageDef d) => delegate()
				{
					this.damage = d;
				});
			}
		}

		// Token: 0x0600E2EC RID: 58092 RVA: 0x00426B28 File Offset: 0x00424D28
		public override void Notify_PawnDied(Corpse corpse)
		{
			if (corpse.Spawned)
			{
				GenExplosion.DoExplosion(corpse.Position, corpse.Map, this.radius, this.damage, null, -1, -1f, null, null, null, null, null, 0f, 1, null, null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
			}
		}

		// Token: 0x0600E2ED RID: 58093 RVA: 0x00426BBA File Offset: 0x00424DBA
		private IEnumerable<DamageDef> PossibleDamageDefs()
		{
			yield return DamageDefOf.Bomb;
			yield return DamageDefOf.Flame;
			yield break;
		}

		// Token: 0x0600E2EE RID: 58094 RVA: 0x00426BC3 File Offset: 0x00424DC3
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ ((this.damage != null) ? this.damage.GetHashCode() : 0) ^ this.radius.GetHashCode();
		}

		// Token: 0x04009824 RID: 38948
		private float radius = 5.9f;

		// Token: 0x04009825 RID: 38949
		private DamageDef damage;

		// Token: 0x04009826 RID: 38950
		private string radiusBuf;
	}
}
