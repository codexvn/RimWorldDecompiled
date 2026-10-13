using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037AC RID: 14252
	public class RoomRequirement_ThingAnyOf : RoomRequirement
	{
		// Token: 0x06013BF7 RID: 80887 RVA: 0x005DD0C4 File Offset: 0x005DB2C4
		public override string Label(Room r = null)
		{
			return ((!this.labelKey.NullOrEmpty()) ? this.labelKey.Translate() : this.things[0].label) + ((r != null) ? " 0/1" : "");
		}

		// Token: 0x06013BF8 RID: 80888 RVA: 0x005DD118 File Offset: 0x005DB318
		public override bool Met(Room r, Pawn p = null)
		{
			foreach (ThingDef def in this.things)
			{
				if (r.ContainsThing(def))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06013BF9 RID: 80889 RVA: 0x005DD174 File Offset: 0x005DB374
		public override bool SameOrSubsetOf(RoomRequirement other)
		{
			if (!base.SameOrSubsetOf(other))
			{
				return false;
			}
			RoomRequirement_ThingAnyOf roomRequirement_ThingAnyOf = (RoomRequirement_ThingAnyOf)other;
			foreach (ThingDef item in this.things)
			{
				if (!roomRequirement_ThingAnyOf.things.Contains(item))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06013BFA RID: 80890 RVA: 0x005DD1E8 File Offset: 0x005DB3E8
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.things.NullOrEmpty<ThingDef>())
			{
				yield return "things are null or empty";
			}
			yield break;
			yield break;
		}

		// Token: 0x06013BFB RID: 80891 RVA: 0x005DD1F8 File Offset: 0x005DB3F8
		public override bool PlayerCanBuildNow()
		{
			for (int i = 0; i < this.things.Count; i++)
			{
				if (this.things[i].IsResearchFinished)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06013BFC RID: 80892 RVA: 0x005DD231 File Offset: 0x005DB431
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<ThingDef>(ref this.things, "things", LookMode.Def, Array.Empty<object>());
		}

		// Token: 0x0400DABF RID: 55999
		public List<ThingDef> things;
	}
}
