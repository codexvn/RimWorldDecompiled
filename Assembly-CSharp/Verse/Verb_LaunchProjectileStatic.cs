using System;
using RimWorld;
using RimWorld.Utility;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000D7F RID: 3455
	public class Verb_LaunchProjectileStatic : Verb_LaunchProjectile
	{
		// Token: 0x17000DF7 RID: 3575
		// (get) Token: 0x0600582A RID: 22570 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool MultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000DF8 RID: 3576
		// (get) Token: 0x0600582B RID: 22571 RVA: 0x001C67B3 File Offset: 0x001C49B3
		public override Texture2D UIIcon
		{
			get
			{
				return TexCommand.Attack;
			}
		}

		// Token: 0x0600582C RID: 22572 RVA: 0x001C67BA File Offset: 0x001C49BA
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			return base.ValidateTarget(target, true) && ReloadableUtility.CanUseConsideringQueuedJobs(this.CasterPawn, base.EquipmentSource, true);
		}

		// Token: 0x0600582D RID: 22573 RVA: 0x001C67E0 File Offset: 0x001C49E0
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			Job job = JobMaker.MakeJob(JobDefOf.UseVerbOnThingStatic, target);
			job.verbToUse = this;
			this.CasterPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
		}
	}
}
