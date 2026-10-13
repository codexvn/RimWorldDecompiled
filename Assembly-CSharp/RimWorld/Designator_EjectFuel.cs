using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003096 RID: 12438
	public class Designator_EjectFuel : Designator
	{
		// Token: 0x17002D30 RID: 11568
		// (get) Token: 0x0601174E RID: 71502 RVA: 0x00503D02 File Offset: 0x00501F02
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.EjectFuel;
			}
		}

		// Token: 0x17002D31 RID: 11569
		// (get) Token: 0x0601174F RID: 71503 RVA: 0x00501A2B File Offset: 0x004FFC2B
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x06011750 RID: 71504 RVA: 0x00503D0C File Offset: 0x00501F0C
		public Designator_EjectFuel()
		{
			this.defaultLabel = "DesignatorEjectFuel".Translate();
			this.defaultDesc = "DesignatorEjectFuelDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/EjectFuel", true);
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_Claim;
		}

		// Token: 0x06011751 RID: 71505 RVA: 0x00503D6C File Offset: 0x00501F6C
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!this.RefuelablesInCell(c).Any<Thing>())
			{
				return false;
			}
			return true;
		}

		// Token: 0x06011752 RID: 71506 RVA: 0x00503DA0 File Offset: 0x00501FA0
		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Thing t in this.RefuelablesInCell(c))
			{
				this.DesignateThing(t);
			}
		}

		// Token: 0x06011753 RID: 71507 RVA: 0x00503DF0 File Offset: 0x00501FF0
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			CompRefuelable compRefuelable;
			if (!t.TryGetComp(out compRefuelable))
			{
				return false;
			}
			AcceptanceReport result = compRefuelable.CanEjectFuel();
			if (!result.Accepted)
			{
				return result;
			}
			return base.Map.designationManager.DesignationOn(t, this.Designation) == null;
		}

		// Token: 0x06011754 RID: 71508 RVA: 0x00503E3F File Offset: 0x0050203F
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.AddDesignation(new Designation(t, this.Designation, null));
		}

		// Token: 0x06011755 RID: 71509 RVA: 0x00503E63 File Offset: 0x00502063
		private IEnumerable<Thing> RefuelablesInCell(IntVec3 c)
		{
			if (c.Fogged(base.Map))
			{
				yield break;
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			int num;
			for (int i = 0; i < thingList.Count; i = num + 1)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					yield return thingList[i];
				}
				num = i;
			}
			yield break;
		}
	}
}
