using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001795 RID: 6037
	public class ThinkNode_ConditionalPlayerControlledColonist : ThinkNode_Conditional
	{
		// Token: 0x06008D73 RID: 36211 RVA: 0x002A0B44 File Offset: 0x0029ED44
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsColonistPlayerControlled;
		}
	}
}
