using System;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020001BE RID: 446
	public class Verb_LaunchProjectileStaticOneUse : Verb_LaunchProjectileStatic
	{
		// Token: 0x06000C54 RID: 3156 RVA: 0x0003A66A File Offset: 0x0003886A
		protected override bool TryCastShot()
		{
			if (base.TryCastShot())
			{
				if (this.burstShotsLeft <= 1)
				{
					this.SelfConsume();
				}
				return true;
			}
			if (this.burstShotsLeft < base.BurstShotCount)
			{
				this.SelfConsume();
			}
			return false;
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x0003A69A File Offset: 0x0003889A
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			return base.ValidateTarget(target, true) && target.Cell.GetFirstBuilding(Find.CurrentMap) == null && target.Cell.Standable(Find.CurrentMap);
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x0003A6D0 File Offset: 0x000388D0
		public override void OnGUI(LocalTargetInfo target)
		{
			Map currentMap = Find.CurrentMap;
			base.OnGUI(target);
			Texture2D icon;
			if (!target.Cell.InBounds(currentMap))
			{
				icon = TexCommand.CannotShoot;
			}
			else
			{
				icon = (this.ValidateTarget(target, false) ? TexCommand.Attack : TexCommand.CannotShoot);
			}
			GenUI.DrawMouseAttachment(icon);
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x0003A71E File Offset: 0x0003891E
		public override void Notify_EquipmentLost()
		{
			base.Notify_EquipmentLost();
			if (this.state == VerbState.Bursting && this.burstShotsLeft < base.BurstShotCount)
			{
				this.SelfConsume();
			}
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x0003A743 File Offset: 0x00038943
		private void SelfConsume()
		{
			if (base.EquipmentSource != null && !base.EquipmentSource.Destroyed)
			{
				base.EquipmentSource.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x0003A768 File Offset: 0x00038968
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			Job job = JobMaker.MakeJob(JobDefOf.UseVerbOnThingStaticReserve, target);
			job.verbToUse = this;
			this.CasterPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
		}
	}
}
