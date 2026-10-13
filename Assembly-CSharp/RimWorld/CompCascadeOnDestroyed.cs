using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C3B RID: 11323
	public class CompCascadeOnDestroyed : ThingComp
	{
		// Token: 0x170027F3 RID: 10227
		// (get) Token: 0x0600FDE4 RID: 64996 RVA: 0x0049A39C File Offset: 0x0049859C
		private CompProperties_CascadeOnDestroyed Props
		{
			get
			{
				return (CompProperties_CascadeOnDestroyed)this.props;
			}
		}

		// Token: 0x0600FDE5 RID: 64997 RVA: 0x0049A3AC File Offset: 0x004985AC
		public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
		{
			if (dinfo != null && dinfo.Value.PreventCascade)
			{
				return;
			}
			int randomInRange = this.Props.cascadeCountRange.RandomInRange;
			int num = 0;
			Queue<Thing> queue = new Queue<Thing>();
			using (IEnumerator<Thing> enumerator = this.AdjacentCascadableThings(this.parent, prevMap).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Thing item = enumerator.Current;
					queue.Enqueue(item);
				}
				goto IL_10B;
			}
			IL_75:
			Thing thing = queue.Dequeue();
			foreach (Thing item2 in this.AdjacentCascadableThings(thing, prevMap))
			{
				if (!queue.Contains(item2))
				{
					queue.Enqueue(item2);
				}
			}
			thing.Kill(new DamageInfo?(new DamageInfo(null, 99999f, 0f, -1f, (dinfo != null) ? dinfo.GetValueOrDefault().Instigator : null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, true)), null);
			num++;
			IL_10B:
			if (num >= randomInRange || queue.Empty<Thing>())
			{
				return;
			}
			goto IL_75;
		}

		// Token: 0x0600FDE6 RID: 64998 RVA: 0x0049A4F0 File Offset: 0x004986F0
		private IEnumerable<Thing> AdjacentCascadableThings(Thing mass, Map map)
		{
			int num;
			for (int i = 0; i < 4; i = num + 1)
			{
				IntVec3 c = mass.Position + GenAdj.CardinalDirections[i];
				if (c.InBounds(map))
				{
					Building edifice = c.GetEdifice(map);
					if (edifice != null)
					{
						if (this.Props.cascadeThingDefs.NullOrEmpty<ThingDef>() && edifice.def == this.parent.def)
						{
							yield return edifice;
						}
						else if (this.Props.cascadeThingDefs.NotNullAndContains(edifice.def))
						{
							yield return edifice;
						}
					}
				}
				num = i;
			}
			yield break;
		}
	}
}
