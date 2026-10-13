using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B3B RID: 6971
	public class AnalysisManager : IExposable
	{
		// Token: 0x170017FB RID: 6139
		// (get) Token: 0x06009BB5 RID: 39861 RVA: 0x002D27BC File Offset: 0x002D09BC
		public IEnumerable<AnalysisDetails> AnalysisDetailsForReading
		{
			get
			{
				return this.analysisDetails.Values;
			}
		}

		// Token: 0x06009BB6 RID: 39862 RVA: 0x002D27C9 File Offset: 0x002D09C9
		public bool TryGetAnalysisProgress(int id, out AnalysisDetails details)
		{
			return this.analysisDetails.TryGetValue(id, out details);
		}

		// Token: 0x06009BB7 RID: 39863 RVA: 0x002D27D8 File Offset: 0x002D09D8
		public bool TryIncrementAnalysisProgress(int id, out AnalysisDetails details)
		{
			if (!this.analysisDetails.TryGetValue(id, out details))
			{
				return false;
			}
			if (details.timesDone >= details.required)
			{
				return false;
			}
			details.timesDone++;
			return true;
		}

		// Token: 0x06009BB8 RID: 39864 RVA: 0x002D2810 File Offset: 0x002D0A10
		public bool IsAnalysisComplete(int id)
		{
			AnalysisDetails analysisDetails;
			return !this.analysisDetails.TryGetValue(id, out analysisDetails) || analysisDetails.Satisfied;
		}

		// Token: 0x06009BB9 RID: 39865 RVA: 0x002D2838 File Offset: 0x002D0A38
		public void ForceCompleteAnalysisProgress(int id)
		{
			if (!this.HasAnalysisWithID(id))
			{
				this.AddAnalysisTask(id, 1);
			}
			AnalysisDetails analysisDetails;
			if (!this.analysisDetails.TryGetValue(id, out analysisDetails))
			{
				return;
			}
			analysisDetails.timesDone = analysisDetails.required;
		}

		// Token: 0x06009BBA RID: 39866 RVA: 0x002D2873 File Offset: 0x002D0A73
		public void AddAnalysisTask(int id, int requiredTimes)
		{
			if (this.analysisDetails.ContainsKey(id))
			{
				return;
			}
			this.analysisDetails[id] = new AnalysisDetails
			{
				id = id,
				required = requiredTimes
			};
		}

		// Token: 0x06009BBB RID: 39867 RVA: 0x002D28A3 File Offset: 0x002D0AA3
		public void RemoveAnalysisDetails(int id)
		{
			if (this.analysisDetails.ContainsKey(id))
			{
				this.analysisDetails.Remove(id);
			}
		}

		// Token: 0x06009BBC RID: 39868 RVA: 0x002D28C0 File Offset: 0x002D0AC0
		public bool HasAnalysisWithID(int id)
		{
			return this.analysisDetails.ContainsKey(id);
		}

		// Token: 0x06009BBD RID: 39869 RVA: 0x002D28CE File Offset: 0x002D0ACE
		public void ExposeData()
		{
			Scribe_Collections.Look<int, AnalysisDetails>(ref this.analysisDetails, "analysisDetails", LookMode.Value, LookMode.Deep);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.analysisDetails == null)
			{
				this.analysisDetails = new Dictionary<int, AnalysisDetails>();
			}
		}

		// Token: 0x04006C24 RID: 27684
		private Dictionary<int, AnalysisDetails> analysisDetails = new Dictionary<int, AnalysisDetails>();
	}
}
