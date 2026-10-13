using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020026C1 RID: 9921
	public class RitualBehaviorDef : Def
	{
		// Token: 0x1700214E RID: 8526
		// (get) Token: 0x0600D978 RID: 55672 RVA: 0x00405F64 File Offset: 0x00404164
		public RitualPosition_Lectern FirstLecternPosition
		{
			get
			{
				foreach (RitualStage ritualStage in this.stages)
				{
					if (ritualStage.roleBehaviors != null)
					{
						foreach (RitualRoleBehavior ritualRoleBehavior in ritualStage.roleBehaviors)
						{
							if (ritualRoleBehavior.CustomPositionsForReading != null)
							{
								foreach (RitualPosition ritualPosition in ritualRoleBehavior.CustomPositionsForReading)
								{
									RitualPosition_Lectern ritualPosition_Lectern = ritualPosition as RitualPosition_Lectern;
									if (ritualPosition_Lectern != null)
									{
										return ritualPosition_Lectern;
									}
								}
							}
						}
					}
				}
				return null;
			}
		}

		// Token: 0x1700214F RID: 8527
		// (get) Token: 0x0600D979 RID: 55673 RVA: 0x00406054 File Offset: 0x00404254
		public bool UsesLectern
		{
			get
			{
				return this.FirstLecternPosition != null;
			}
		}

		// Token: 0x0600D97A RID: 55674 RVA: 0x0040605F File Offset: 0x0040425F
		public RitualBehaviorWorker GetInstance()
		{
			return (RitualBehaviorWorker)Activator.CreateInstance(this.workerClass, new object[]
			{
				this
			});
		}

		// Token: 0x0600D97B RID: 55675 RVA: 0x0040607B File Offset: 0x0040427B
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			using (List<RitualStage>.Enumerator enumerator2 = this.stages.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.endTriggers.NullOrEmpty<StageEndTrigger>())
					{
						yield return "ritual stage with no endTrigger";
					}
				}
			}
			List<RitualStage>.Enumerator enumerator2 = default(List<RitualStage>.Enumerator);
			if (!string.IsNullOrEmpty(this.spectatorsLabel) && string.IsNullOrEmpty(this.spectatorGerund))
			{
				yield return "ritual has spectatorLabel but no spectatorGerund";
			}
			if (!string.IsNullOrEmpty(this.spectatorGerund) && string.IsNullOrEmpty(this.spectatorsLabel))
			{
				yield return "ritual has spectatorGerund but no spectatorLabel";
			}
			if (!this.roles.NullOrEmpty<RitualRole>())
			{
				if (this.roles.Count((RitualRole x) => x.defaultForSelectedColonist) > 1)
				{
					yield return ">1 default role for selected pawn";
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600D97C RID: 55676 RVA: 0x0040608C File Offset: 0x0040428C
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.stages.NullOrEmpty<RitualStage>())
			{
				return;
			}
			for (int i = 0; i < this.stages.Count; i++)
			{
				this.stages[i].parent = this;
			}
		}

		// Token: 0x0600D97D RID: 55677 RVA: 0x004060D5 File Offset: 0x004042D5
		public IEnumerable<RitualRole> RequiredRoles()
		{
			if (this.roles.NullOrEmpty<RitualRole>())
			{
				yield break;
			}
			int num;
			for (int i = 0; i < this.roles.Count; i = num + 1)
			{
				if (this.roles[i].required)
				{
					yield return this.roles[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0400935E RID: 37726
		public Type workerClass = typeof(RitualBehaviorWorker);

		// Token: 0x0400935F RID: 37727
		public List<RitualRole> roles;

		// Token: 0x04009360 RID: 37728
		public List<RitualStage> stages;

		// Token: 0x04009361 RID: 37729
		public IntRange durationTicks = new IntRange(7500, 7500);

		// Token: 0x04009362 RID: 37730
		public List<PreceptRequirement> preceptRequirements;

		// Token: 0x04009363 RID: 37731
		public List<RitualCancellationTrigger> cancellationTriggers;

		// Token: 0x04009364 RID: 37732
		public bool displayCompletedMessage = true;

		// Token: 0x04009365 RID: 37733
		[NoTranslate]
		public string useVisualEffectsFromRoleIdIdeo;

		// Token: 0x04009366 RID: 37734
		public int displayOrder;

		// Token: 0x04009367 RID: 37735
		[MustTranslate]
		public string letterTitle;

		// Token: 0x04009368 RID: 37736
		[MustTranslate]
		public string letterText;

		// Token: 0x04009369 RID: 37737
		[MustTranslate]
		public string spectatorsLabel;

		// Token: 0x0400936A RID: 37738
		[MustTranslate]
		public string spectatorGerund;

		// Token: 0x0400936B RID: 37739
		public bool spectatorsIgnoreBleeding;

		// Token: 0x0400936C RID: 37740
		public List<SoundDef> soundDefsPerEnhancerCount;

		// Token: 0x0400936D RID: 37741
		public int maxEnhancerDistance;

		// Token: 0x0400936E RID: 37742
		public RitualSpectatorFilter spectatorFilter;
	}
}
