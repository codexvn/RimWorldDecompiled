using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020021EC RID: 8684
	public class Command_AbilityTrial : Command_AbilitySpeech
	{
		// Token: 0x0600BE41 RID: 48705 RVA: 0x00372EE5 File Offset: 0x003710E5
		public Command_AbilityTrial(Ability ability, Pawn pawn) : base(ability, pawn)
		{
			this.defaultLabel = "Accuse".Translate();
			this.defaultIconColor = Color.white;
		}
	}
}
