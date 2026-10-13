using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BE7 RID: 7143
	public class QuestPart_WorkDisabled : QuestPartActivable
	{
		// Token: 0x17001879 RID: 6265
		// (get) Token: 0x06009F22 RID: 40738 RVA: 0x002DF6E5 File Offset: 0x002DD8E5
		public IEnumerable<WorkTypeDef> DisabledWorkTypes
		{
			get
			{
				if (base.State == QuestPartState.Enabled)
				{
					List<WorkTypeDef> list = DefDatabase<WorkTypeDef>.AllDefsListForReading;
					int num;
					for (int i = 0; i < list.Count; i = num + 1)
					{
						if ((this.disabledWorkTags & list[i].workTags) != WorkTags.None)
						{
							yield return list[i];
						}
						num = i;
					}
					list = null;
				}
				yield break;
			}
		}

		// Token: 0x06009F23 RID: 40739 RVA: 0x002DF6F5 File Offset: 0x002DD8F5
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.ClearPawnWorkTypesAndSkillsCache();
		}

		// Token: 0x06009F24 RID: 40740 RVA: 0x002DF704 File Offset: 0x002DD904
		public override void Cleanup()
		{
			base.Cleanup();
			this.ClearPawnWorkTypesAndSkillsCache();
		}

		// Token: 0x06009F25 RID: 40741 RVA: 0x002DF714 File Offset: 0x002DD914
		private void ClearPawnWorkTypesAndSkillsCache()
		{
			for (int i = 0; i < this.pawns.Count; i++)
			{
				if (this.pawns[i] != null)
				{
					this.pawns[i].Notify_DisabledWorkTypesChanged();
					if (this.pawns[i].skills != null)
					{
						this.pawns[i].skills.Notify_SkillDisablesChanged();
					}
				}
			}
		}

		// Token: 0x06009F26 RID: 40742 RVA: 0x002DF780 File Offset: 0x002DD980
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<WorkTags>(ref this.disabledWorkTags, "disabledWorkTags", WorkTags.None, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04006E09 RID: 28169
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04006E0A RID: 28170
		public WorkTags disabledWorkTags;
	}
}
