using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200085A RID: 2138
	public class DebugLoadIDsSavingErrorsChecker
	{
		// Token: 0x060035FE RID: 13822 RVA: 0x001203A9 File Offset: 0x0011E5A9
		public void Clear()
		{
			if (!Prefs.DevMode)
			{
				return;
			}
			this.deepSaved.Clear();
			this.deepSavedInfo.Clear();
			this.referenced.Clear();
		}

		// Token: 0x060035FF RID: 13823 RVA: 0x001203D4 File Offset: 0x0011E5D4
		public void CheckForErrorsAndClear()
		{
			if (!Prefs.DevMode)
			{
				return;
			}
			if (!Scribe.saver.savingForDebug)
			{
				foreach (DebugLoadIDsSavingErrorsChecker.ReferencedObject referencedObject in this.referenced)
				{
					if (!this.deepSaved.Contains(referencedObject.loadID))
					{
						Log.Warning(string.Concat(new string[]
						{
							"Object with load ID ",
							referencedObject.loadID,
							" is referenced (xml node name: ",
							referencedObject.label,
							") but is not deep-saved. This will cause errors during loading."
						}));
					}
				}
			}
			this.Clear();
		}

		// Token: 0x06003600 RID: 13824 RVA: 0x00120488 File Offset: 0x0011E688
		public void RegisterDeepSaved(object obj, string label)
		{
			if (!Prefs.DevMode)
			{
				return;
			}
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				Log.Error("Registered " + ((obj != null) ? obj.ToString() : null) + ", but current mode is " + Scribe.mode.ToString());
				return;
			}
			if (obj == null)
			{
				return;
			}
			ILoadReferenceable loadReferenceable = obj as ILoadReferenceable;
			if (loadReferenceable != null)
			{
				try
				{
					string uniqueLoadID = loadReferenceable.GetUniqueLoadID();
					if (!this.deepSaved.Add(uniqueLoadID))
					{
						Log.Warning(string.Concat(new string[]
						{
							"DebugLoadIDsSavingErrorsChecker error: tried to register deep-saved object with loadID ",
							uniqueLoadID,
							", but it's already here. label=",
							label,
							" (not cleared after the previous save? different objects have the same load ID? the same object is deep-saved twice?)"
						}));
						string str;
						if (this.deepSavedInfo.TryGetValue(uniqueLoadID, out str))
						{
							Type type = loadReferenceable.GetType();
							Log.Warning(((type != null) ? type.ToString() : null) + " was already deepsaved at " + str + ".");
						}
					}
					else
					{
						this.deepSavedInfo.Add(uniqueLoadID, Scribe.saver.CurPath);
					}
				}
				catch (Exception ex)
				{
					string str2 = "Error in GetUniqueLoadID(): ";
					Exception ex2 = ex;
					Log.Error(str2 + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
		}

		// Token: 0x06003601 RID: 13825 RVA: 0x001205AC File Offset: 0x0011E7AC
		public void RegisterReferenced(ILoadReferenceable obj, string label)
		{
			if (!Prefs.DevMode)
			{
				return;
			}
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				Log.Error("Registered " + ((obj != null) ? obj.ToString() : null) + ", but current mode is " + Scribe.mode.ToString());
				return;
			}
			if (obj == null)
			{
				return;
			}
			try
			{
				this.referenced.Add(new DebugLoadIDsSavingErrorsChecker.ReferencedObject(obj.GetUniqueLoadID(), label));
			}
			catch (Exception ex)
			{
				string str = "Error in GetUniqueLoadID(): ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x040028B4 RID: 10420
		private HashSet<string> deepSaved = new HashSet<string>();

		// Token: 0x040028B5 RID: 10421
		private Dictionary<string, string> deepSavedInfo = new Dictionary<string, string>();

		// Token: 0x040028B6 RID: 10422
		private HashSet<DebugLoadIDsSavingErrorsChecker.ReferencedObject> referenced = new HashSet<DebugLoadIDsSavingErrorsChecker.ReferencedObject>();

		// Token: 0x0200085B RID: 2139
		private struct ReferencedObject : IEquatable<DebugLoadIDsSavingErrorsChecker.ReferencedObject>
		{
			// Token: 0x06003603 RID: 13827 RVA: 0x00120675 File Offset: 0x0011E875
			public ReferencedObject(string loadID, string label)
			{
				this.loadID = loadID;
				this.label = label;
			}

			// Token: 0x06003604 RID: 13828 RVA: 0x00120685 File Offset: 0x0011E885
			public override bool Equals(object obj)
			{
				return obj is DebugLoadIDsSavingErrorsChecker.ReferencedObject && this.Equals((DebugLoadIDsSavingErrorsChecker.ReferencedObject)obj);
			}

			// Token: 0x06003605 RID: 13829 RVA: 0x0012069D File Offset: 0x0011E89D
			public bool Equals(DebugLoadIDsSavingErrorsChecker.ReferencedObject other)
			{
				return this.loadID == other.loadID && this.label == other.label;
			}

			// Token: 0x06003606 RID: 13830 RVA: 0x001206C5 File Offset: 0x0011E8C5
			public override int GetHashCode()
			{
				return Gen.HashCombine<string>(Gen.HashCombine<string>(0, this.loadID), this.label);
			}

			// Token: 0x06003607 RID: 13831 RVA: 0x001206DE File Offset: 0x0011E8DE
			public static bool operator ==(DebugLoadIDsSavingErrorsChecker.ReferencedObject lhs, DebugLoadIDsSavingErrorsChecker.ReferencedObject rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x06003608 RID: 13832 RVA: 0x001206E8 File Offset: 0x0011E8E8
			public static bool operator !=(DebugLoadIDsSavingErrorsChecker.ReferencedObject lhs, DebugLoadIDsSavingErrorsChecker.ReferencedObject rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x040028B7 RID: 10423
			public string loadID;

			// Token: 0x040028B8 RID: 10424
			public string label;
		}
	}
}
