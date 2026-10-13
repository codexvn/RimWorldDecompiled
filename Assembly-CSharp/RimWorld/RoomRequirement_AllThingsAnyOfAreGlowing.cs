using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037B2 RID: 14258
	public class RoomRequirement_AllThingsAnyOfAreGlowing : RoomRequirement_ThingAnyOf
	{
		// Token: 0x06013C29 RID: 80937 RVA: 0x005DC69C File Offset: 0x005DA89C
		public override string Label(Room r = null)
		{
			return this.labelKey.Translate();
		}

		// Token: 0x06013C2A RID: 80938 RVA: 0x005DD970 File Offset: 0x005DBB70
		public override bool Met(Room r, Pawn p = null)
		{
			using (IEnumerator<Thing> enumerator = r.ContainedThingsList(this.things).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.TryGetComp<CompGlower>().Glows)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06013C2B RID: 80939 RVA: 0x005DD9D0 File Offset: 0x005DBBD0
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			foreach (ThingDef thingDef in this.things)
			{
				if (thingDef.GetCompProperties<CompProperties_Glower>() == null)
				{
					yield return "No comp glower on thingDef " + thingDef.defName;
				}
			}
			List<ThingDef>.Enumerator enumerator2 = default(List<ThingDef>.Enumerator);
			yield break;
			yield break;
		}
	}
}
