using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000E47 RID: 3655
	public class SustainerManager
	{
		// Token: 0x17000ECD RID: 3789
		// (get) Token: 0x06005C17 RID: 23575 RVA: 0x001D73D8 File Offset: 0x001D55D8
		public List<Sustainer> AllSustainers
		{
			get
			{
				return this.allSustainers;
			}
		}

		// Token: 0x06005C18 RID: 23576 RVA: 0x001D73E0 File Offset: 0x001D55E0
		public void RegisterSustainer(Sustainer newSustainer)
		{
			this.allSustainers.Add(newSustainer);
		}

		// Token: 0x06005C19 RID: 23577 RVA: 0x001D73EE File Offset: 0x001D55EE
		public void DeregisterSustainer(Sustainer oldSustainer)
		{
			this.allSustainers.Remove(oldSustainer);
		}

		// Token: 0x06005C1A RID: 23578 RVA: 0x001D7400 File Offset: 0x001D5600
		public bool SustainerExists(SoundDef def)
		{
			for (int i = 0; i < this.allSustainers.Count; i++)
			{
				if (this.allSustainers[i].def == def)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005C1B RID: 23579 RVA: 0x001D743C File Offset: 0x001D563C
		public void SustainerManagerUpdate()
		{
			for (int i = this.allSustainers.Count - 1; i >= 0; i--)
			{
				this.allSustainers[i].SustainerUpdate();
			}
			this.UpdateAllSustainerScopes();
		}

		// Token: 0x06005C1C RID: 23580 RVA: 0x001D7478 File Offset: 0x001D5678
		public void UpdateAllSustainerScopes()
		{
			SustainerManager.playingPerDef.Clear();
			for (int i = 0; i < this.allSustainers.Count; i++)
			{
				Sustainer sustainer = this.allSustainers[i];
				if (!SustainerManager.playingPerDef.ContainsKey(sustainer.def))
				{
					List<Sustainer> list = SimplePool<List<Sustainer>>.Get();
					list.Add(sustainer);
					SustainerManager.playingPerDef.Add(sustainer.def, list);
				}
				else
				{
					SustainerManager.playingPerDef[sustainer.def].Add(sustainer);
				}
			}
			foreach (KeyValuePair<SoundDef, List<Sustainer>> keyValuePair in SustainerManager.playingPerDef)
			{
				SoundDef key = keyValuePair.Key;
				List<Sustainer> value = keyValuePair.Value;
				if (value.Count - key.maxVoices < 0)
				{
					for (int j = 0; j < value.Count; j++)
					{
						value[j].scopeFader.inScope = true;
					}
				}
				else
				{
					for (int k = 0; k < value.Count; k++)
					{
						value[k].scopeFader.inScope = false;
					}
					value.Sort(SustainerManager.SortSustainersByCameraDistanceCached);
					int num = 0;
					for (int l = 0; l < value.Count; l++)
					{
						value[l].scopeFader.inScope = true;
						num++;
						if (num >= key.maxVoices)
						{
							break;
						}
					}
					for (int m = 0; m < value.Count; m++)
					{
						if (!value[m].scopeFader.inScope)
						{
							value[m].scopeFader.inScopePercent = 0f;
						}
					}
				}
			}
			foreach (KeyValuePair<SoundDef, List<Sustainer>> keyValuePair2 in SustainerManager.playingPerDef)
			{
				keyValuePair2.Value.Clear();
				SimplePool<List<Sustainer>>.Return(keyValuePair2.Value);
			}
			SustainerManager.playingPerDef.Clear();
		}

		// Token: 0x06005C1D RID: 23581 RVA: 0x001D76C0 File Offset: 0x001D58C0
		public void EndAllInMap(Map map)
		{
			for (int i = this.allSustainers.Count - 1; i >= 0; i--)
			{
				if (this.allSustainers[i].info.Maker.Map == map)
				{
					this.allSustainers[i].End();
				}
			}
		}

		// Token: 0x040042BD RID: 17085
		private List<Sustainer> allSustainers = new List<Sustainer>();

		// Token: 0x040042BE RID: 17086
		private static Dictionary<SoundDef, List<Sustainer>> playingPerDef = new Dictionary<SoundDef, List<Sustainer>>();

		// Token: 0x040042BF RID: 17087
		private static readonly Comparison<Sustainer> SortSustainersByCameraDistanceCached = (Sustainer a, Sustainer b) => a.CameraDistanceSquared.CompareTo(b.CameraDistanceSquared);
	}
}
