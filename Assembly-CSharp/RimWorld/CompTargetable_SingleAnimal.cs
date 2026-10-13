using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F24 RID: 12068
	public class CompTargetable_SingleAnimal : CompTargetable
	{
		// Token: 0x17002C04 RID: 11268
		// (get) Token: 0x0601109C RID: 69788 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002C05 RID: 11269
		// (get) Token: 0x0601109D RID: 69789 RVA: 0x004E641E File Offset: 0x004E461E
		public new CompProperties_Targetable_SingleAnimal Props
		{
			get
			{
				return (CompProperties_Targetable_SingleAnimal)this.props;
			}
		}

		// Token: 0x0601109E RID: 69790 RVA: 0x004E642B File Offset: 0x004E462B
		protected override TargetingParameters GetTargetingParameters()
		{
			return new TargetingParameters
			{
				canTargetPawns = true,
				canTargetBuildings = false,
				canTargetAnimals = true,
				canTargetHumans = false,
				canTargetMechs = false,
				validator = delegate(TargetInfo target)
				{
					if (!this.Props.allowWildMan)
					{
						Pawn pawn = target.Thing as Pawn;
						return pawn != null && !pawn.IsWildMan();
					}
					return true;
				}
			};
		}

		// Token: 0x0601109F RID: 69791 RVA: 0x004E6467 File Offset: 0x004E4667
		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			yield return targetChosenByPlayer;
			yield break;
		}
	}
}
