using System;
using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	// Token: 0x02000C4C RID: 3148
	public abstract class BackCompatibilityConverter
	{
		// Token: 0x060050B3 RID: 20659
		public abstract bool AppliesToVersion(int majorVer, int minorVer);

		// Token: 0x060050B4 RID: 20660
		public abstract string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null);

		// Token: 0x060050B5 RID: 20661
		public abstract Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node);

		// Token: 0x060050B6 RID: 20662 RVA: 0x00161C9F File Offset: 0x0015FE9F
		public virtual int GetBackCompatibleBodyPartIndex(BodyDef body, int index)
		{
			return index;
		}

		// Token: 0x060050B7 RID: 20663
		public abstract void PostExposeData(object obj);

		// Token: 0x060050B8 RID: 20664 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostCouldntLoadDef(string defName)
		{
		}

		// Token: 0x060050B9 RID: 20665 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PreLoadSavegame(string loadingVersion)
		{
		}

		// Token: 0x060050BA RID: 20666 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostLoadSavegame(string loadingVersion)
		{
		}

		// Token: 0x060050BB RID: 20667 RVA: 0x0019FC35 File Offset: 0x0019DE35
		public bool AppliesToLoadedGameVersion(bool allowInactiveScribe = false)
		{
			return !ScribeMetaHeaderUtility.loadedGameVersion.NullOrEmpty() && (allowInactiveScribe || Scribe.mode != LoadSaveMode.Inactive) && this.AppliesToVersion(ScribeMetaHeaderUtility.loadedGameVersionMajor, ScribeMetaHeaderUtility.loadedGameVersionMinor);
		}

		// Token: 0x060050BC RID: 20668 RVA: 0x0019FC64 File Offset: 0x0019DE64
		protected void Scribe_TypeToType<T1, T2>(object obj, ref T2 v, string label, Func<T1, T2> converter) where T2 : new()
		{
			if (!this.typeToTypeLookup.ContainsKey(obj))
			{
				this.typeToTypeLookup[obj] = new Dictionary<string, object>();
			}
			object value = this.typeToTypeLookup[obj][label];
			Scribe_Values.Look<object>(ref value, label, null, false);
			this.typeToTypeLookup[obj][label] = value;
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				v = converter((T1)((object)this.typeToTypeLookup[obj][label]));
			}
		}

		// Token: 0x04003A1A RID: 14874
		private readonly Dictionary<object, Dictionary<string, object>> typeToTypeLookup = new Dictionary<object, Dictionary<string, object>>();
	}
}
