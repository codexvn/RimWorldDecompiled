using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002973 RID: 10611
	public class Building_BlastingCharge : Building
	{
		// Token: 0x0600E907 RID: 59655 RVA: 0x0043D960 File Offset: 0x0043BB60
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Command_Action command_Action = new Command_Action();
			command_Action.icon = ContentFinder<Texture2D>.Get("UI/Commands/Detonate", true);
			command_Action.defaultDesc = "CommandDetonateDesc".Translate();
			command_Action.action = new Action(this.Command_Detonate);
			if (base.GetComp<CompExplosive>().wickStarted)
			{
				command_Action.Disable(null);
			}
			command_Action.defaultLabel = "CommandDetonateLabel".Translate();
			yield return command_Action;
			yield break;
		}

		// Token: 0x0600E908 RID: 59656 RVA: 0x0043D970 File Offset: 0x0043BB70
		private void Command_Detonate()
		{
			base.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
