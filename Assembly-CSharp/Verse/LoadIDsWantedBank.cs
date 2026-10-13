using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x02000852 RID: 2130
	public class LoadIDsWantedBank
	{
		// Token: 0x060035D8 RID: 13784 RVA: 0x0011EB7C File Offset: 0x0011CD7C
		public void ConfirmClear()
		{
			if (this.idsRead.Count > 0 || this.idListsRead.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Not all loadIDs which were read were consumed.");
				if (this.idsRead.Count > 0)
				{
					stringBuilder.AppendLine("Singles:");
					foreach (KeyValuePair<ValueTuple<IExposable, string>, LoadIDsWantedBank.IdRecord> keyValuePair in this.idsRead)
					{
						StringBuilder stringBuilder2 = stringBuilder;
						string[] array = new string[8];
						array[0] = "  ";
						array[1] = keyValuePair.Value.targetLoadID.ToStringSafe<string>();
						array[2] = " of type ";
						int num = 3;
						Type targetType = keyValuePair.Value.targetType;
						array[num] = ((targetType != null) ? targetType.ToString() : null);
						array[4] = ". pathRelToParent=";
						array[5] = keyValuePair.Value.pathRelToParent;
						array[6] = ", parent=";
						array[7] = keyValuePair.Value.parent.ToStringSafe<IExposable>();
						stringBuilder2.AppendLine(string.Concat(array));
					}
				}
				if (this.idListsRead.Count > 0)
				{
					stringBuilder.AppendLine("Lists:");
					foreach (KeyValuePair<ValueTuple<IExposable, string>, LoadIDsWantedBank.IdListRecord> keyValuePair2 in this.idListsRead)
					{
						stringBuilder.AppendLine(string.Concat(new string[]
						{
							"  List with ",
							((keyValuePair2.Value.targetLoadIDs != null) ? keyValuePair2.Value.targetLoadIDs.Count : 0).ToString(),
							" elements. pathRelToParent=",
							keyValuePair2.Value.pathRelToParent,
							", parent=",
							keyValuePair2.Value.parent.ToStringSafe<IExposable>()
						}));
					}
				}
				Log.Warning(stringBuilder.ToString().TrimEndNewlines());
			}
			this.Clear();
		}

		// Token: 0x060035D9 RID: 13785 RVA: 0x0011ED90 File Offset: 0x0011CF90
		public void Clear()
		{
			this.idsRead.Clear();
			this.idListsRead.Clear();
		}

		// Token: 0x060035DA RID: 13786 RVA: 0x0011EDA8 File Offset: 0x0011CFA8
		public void RegisterLoadIDReadFromXml(string targetLoadID, Type targetType, string pathRelToParent, IExposable parent)
		{
			if (this.idsRead.ContainsKey(new ValueTuple<IExposable, string>(parent, pathRelToParent)))
			{
				Log.Error(string.Concat(new string[]
				{
					"Tried to register the same load ID twice: ",
					targetLoadID,
					", pathRelToParent=",
					pathRelToParent,
					", parent=",
					parent.ToStringSafe<IExposable>()
				}));
				return;
			}
			LoadIDsWantedBank.IdRecord value = new LoadIDsWantedBank.IdRecord(targetLoadID, targetType, pathRelToParent, parent);
			this.idsRead.Add(new ValueTuple<IExposable, string>(parent, pathRelToParent), value);
		}

		// Token: 0x060035DB RID: 13787 RVA: 0x0011EE28 File Offset: 0x0011D028
		public void RegisterLoadIDReadFromXml(string targetLoadID, Type targetType, string toAppendToPathRelToParent)
		{
			string text = Scribe.loader.curPathRelToParent;
			if (!toAppendToPathRelToParent.NullOrEmpty())
			{
				text = text + "/" + toAppendToPathRelToParent;
			}
			this.RegisterLoadIDReadFromXml(targetLoadID, targetType, text, Scribe.loader.curParent);
		}

		// Token: 0x060035DC RID: 13788 RVA: 0x0011EE68 File Offset: 0x0011D068
		public void RegisterLoadIDListReadFromXml(List<string> targetLoadIDList, string pathRelToParent, IExposable parent)
		{
			if (this.idListsRead.ContainsKey(new ValueTuple<IExposable, string>(parent, pathRelToParent)))
			{
				Log.Error("Tried to register the same list of load IDs twice. pathRelToParent=" + pathRelToParent + ", parent=" + parent.ToStringSafe<IExposable>());
				return;
			}
			LoadIDsWantedBank.IdListRecord value = new LoadIDsWantedBank.IdListRecord(targetLoadIDList, pathRelToParent, parent);
			this.idListsRead.Add(new ValueTuple<IExposable, string>(parent, pathRelToParent), value);
		}

		// Token: 0x060035DD RID: 13789 RVA: 0x0011EEC4 File Offset: 0x0011D0C4
		public void RegisterLoadIDListReadFromXml(List<string> targetLoadIDList, string toAppendToPathRelToParent)
		{
			string text = Scribe.loader.curPathRelToParent;
			if (!toAppendToPathRelToParent.NullOrEmpty())
			{
				text = text + "/" + toAppendToPathRelToParent;
			}
			this.RegisterLoadIDListReadFromXml(targetLoadIDList, text, Scribe.loader.curParent);
		}

		// Token: 0x060035DE RID: 13790 RVA: 0x0011EF04 File Offset: 0x0011D104
		public string Take<T>(string pathRelToParent, IExposable parent)
		{
			LoadIDsWantedBank.IdRecord idRecord;
			if (this.idsRead.TryGetValue(new ValueTuple<IExposable, string>(parent, pathRelToParent), out idRecord))
			{
				string targetLoadID = idRecord.targetLoadID;
				if (typeof(T) != idRecord.targetType)
				{
					string[] array = new string[8];
					array[0] = "Trying to get load ID of object of type ";
					int num = 1;
					Type typeFromHandle = typeof(T);
					array[num] = ((typeFromHandle != null) ? typeFromHandle.ToString() : null);
					array[2] = ", but it was registered as ";
					int num2 = 3;
					Type targetType = idRecord.targetType;
					array[num2] = ((targetType != null) ? targetType.ToString() : null);
					array[4] = ". pathRelToParent=";
					array[5] = pathRelToParent;
					array[6] = ", parent=";
					array[7] = parent.ToStringSafe<IExposable>();
					Log.Error(string.Concat(array));
				}
				this.idsRead.Remove(new ValueTuple<IExposable, string>(parent, pathRelToParent));
				return targetLoadID;
			}
			Log.Error("Could not get load ID. We're asking for something which was never added during LoadingVars. pathRelToParent=" + pathRelToParent + ", parent=" + parent.ToStringSafe<IExposable>());
			return null;
		}

		// Token: 0x060035DF RID: 13791 RVA: 0x0011EFE4 File Offset: 0x0011D1E4
		public List<string> TakeList(string pathRelToParent, IExposable parent)
		{
			LoadIDsWantedBank.IdListRecord idListRecord;
			if (this.idListsRead.TryGetValue(new ValueTuple<IExposable, string>(parent, pathRelToParent), out idListRecord))
			{
				List<string> targetLoadIDs = idListRecord.targetLoadIDs;
				this.idListsRead.Remove(new ValueTuple<IExposable, string>(parent, pathRelToParent));
				return targetLoadIDs;
			}
			Log.Error("Could not get load IDs list. We're asking for something which was never added during LoadingVars. pathRelToParent=" + pathRelToParent + ", parent=" + parent.ToStringSafe<IExposable>());
			return new List<string>();
		}

		// Token: 0x04002894 RID: 10388
		private Dictionary<ValueTuple<IExposable, string>, LoadIDsWantedBank.IdRecord> idsRead = new Dictionary<ValueTuple<IExposable, string>, LoadIDsWantedBank.IdRecord>();

		// Token: 0x04002895 RID: 10389
		private Dictionary<ValueTuple<IExposable, string>, LoadIDsWantedBank.IdListRecord> idListsRead = new Dictionary<ValueTuple<IExposable, string>, LoadIDsWantedBank.IdListRecord>();

		// Token: 0x02000853 RID: 2131
		private struct IdRecord
		{
			// Token: 0x060035E1 RID: 13793 RVA: 0x0011F05F File Offset: 0x0011D25F
			public IdRecord(string targetLoadID, Type targetType, string pathRelToParent, IExposable parent)
			{
				this.targetLoadID = targetLoadID;
				this.targetType = targetType;
				this.pathRelToParent = pathRelToParent;
				this.parent = parent;
			}

			// Token: 0x04002896 RID: 10390
			public string targetLoadID;

			// Token: 0x04002897 RID: 10391
			public Type targetType;

			// Token: 0x04002898 RID: 10392
			public string pathRelToParent;

			// Token: 0x04002899 RID: 10393
			public IExposable parent;
		}

		// Token: 0x02000854 RID: 2132
		private struct IdListRecord
		{
			// Token: 0x060035E2 RID: 13794 RVA: 0x0011F07E File Offset: 0x0011D27E
			public IdListRecord(List<string> targetLoadIDs, string pathRelToParent, IExposable parent)
			{
				this.targetLoadIDs = targetLoadIDs;
				this.pathRelToParent = pathRelToParent;
				this.parent = parent;
			}

			// Token: 0x0400289A RID: 10394
			public List<string> targetLoadIDs;

			// Token: 0x0400289B RID: 10395
			public string pathRelToParent;

			// Token: 0x0400289C RID: 10396
			public IExposable parent;
		}
	}
}
