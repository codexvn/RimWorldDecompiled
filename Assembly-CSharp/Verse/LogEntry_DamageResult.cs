using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000340 RID: 832
	public abstract class LogEntry_DamageResult : LogEntry
	{
		// Token: 0x0600152D RID: 5421 RVA: 0x00064141 File Offset: 0x00062341
		public LogEntry_DamageResult(LogEntryDef def = null) : base(def)
		{
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x0006414A File Offset: 0x0006234A
		public void FillTargets(List<BodyPartRecord> recipientParts, List<bool> recipientPartsDestroyed, bool deflected)
		{
			this.damagedParts = recipientParts;
			this.damagedPartsDestroyed = recipientPartsDestroyed;
			this.deflected = deflected;
			base.ResetCache();
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x00002C42 File Offset: 0x00000E42
		protected virtual BodyDef DamagedBody()
		{
			return null;
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x00064168 File Offset: 0x00062368
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Rules.AddRange(PlayLogEntryUtility.RulesForDamagedParts("recipient_part", this.DamagedBody(), this.damagedParts, this.damagedPartsDestroyed, result.Constants));
			result.Constants.Add("deflected", this.deflected.ToString());
			return result;
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x000641C8 File Offset: 0x000623C8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<BodyPartRecord>(ref this.damagedParts, "damagedParts", LookMode.BodyPart, Array.Empty<object>());
			Scribe_Collections.Look<bool>(ref this.damagedPartsDestroyed, "damagedPartsDestroyed", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.deflected, "deflected", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.damagedParts != null)
			{
				for (int i = this.damagedParts.Count - 1; i >= 0; i--)
				{
					if (this.damagedParts[i] == null)
					{
						this.damagedParts.RemoveAt(i);
						if (i < this.damagedPartsDestroyed.Count)
						{
							this.damagedPartsDestroyed.RemoveAt(i);
						}
					}
				}
			}
		}

		// Token: 0x040013CA RID: 5066
		protected List<BodyPartRecord> damagedParts;

		// Token: 0x040013CB RID: 5067
		protected List<bool> damagedPartsDestroyed;

		// Token: 0x040013CC RID: 5068
		protected bool deflected;
	}
}
