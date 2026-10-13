using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BFC RID: 15356
	public class Slate
	{
		// Token: 0x17003383 RID: 13187
		// (get) Token: 0x06014D39 RID: 85305 RVA: 0x0063D494 File Offset: 0x0063B694
		public string CurrentPrefix
		{
			get
			{
				return this.prefix;
			}
		}

		// Token: 0x06014D3A RID: 85306 RVA: 0x0063D49C File Offset: 0x0063B69C
		public T Get<T>(string name, T defaultValue = default(T), bool isAbsoluteName = false)
		{
			T result;
			if (this.TryGet<T>(name, out result, isAbsoluteName))
			{
				return result;
			}
			return defaultValue;
		}

		// Token: 0x06014D3B RID: 85307 RVA: 0x0063D4B8 File Offset: 0x0063B6B8
		public bool TryGet<T>(string name, out T var, bool isAbsoluteName = false)
		{
			if (name.NullOrEmpty())
			{
				var = default(T);
				return false;
			}
			if (!isAbsoluteName && !this.prefix.NullOrEmpty())
			{
				name = this.prefix + "/" + name;
			}
			name = QuestGenUtility.NormalizeVarPath(name);
			if (this.allowNonPrefixedLookup)
			{
				name = this.TryResolveFirstAvailableName(name);
			}
			object obj;
			if (!this.vars.TryGetValue(name, out obj))
			{
				var = default(T);
				return false;
			}
			if (obj == null)
			{
				var = default(T);
				return true;
			}
			if (obj is T)
			{
				var = (T)((object)obj);
				return true;
			}
			if (ConvertHelper.CanConvert<T>(obj))
			{
				var = ConvertHelper.Convert<T>(obj);
				return true;
			}
			Log.Error(string.Concat(new string[]
			{
				"Could not convert slate variable \"",
				name,
				"\" (",
				obj.GetType().Name,
				") to ",
				typeof(T).Name
			}));
			var = default(T);
			return false;
		}

		// Token: 0x06014D3C RID: 85308 RVA: 0x0063D5B8 File Offset: 0x0063B7B8
		public void Set<T>(string name, T var, bool isAbsoluteName = false)
		{
			if (name.NullOrEmpty())
			{
				Log.Error("Tried to set a variable with null name. var=" + var.ToStringSafe<T>());
				return;
			}
			if (!isAbsoluteName && !this.prefix.NullOrEmpty())
			{
				name = this.prefix + "/" + name;
			}
			name = QuestGenUtility.NormalizeVarPath(name);
			ISlateRef slateRef = var as ISlateRef;
			if (slateRef != null)
			{
				object value;
				slateRef.TryGetConvertedValue<object>(this, out value);
				this.vars[name] = value;
				return;
			}
			this.vars[name] = var;
		}

		// Token: 0x06014D3D RID: 85309 RVA: 0x0063D647 File Offset: 0x0063B847
		public void SetIfNone<T>(string name, T var, bool isAbsoluteName = false)
		{
			if (this.Exists(name, isAbsoluteName))
			{
				return;
			}
			this.Set<T>(name, var, isAbsoluteName);
		}

		// Token: 0x06014D3E RID: 85310 RVA: 0x0063D660 File Offset: 0x0063B860
		public bool Remove(string name, bool isAbsoluteName = false)
		{
			if (name.NullOrEmpty())
			{
				return false;
			}
			if (!isAbsoluteName && !this.prefix.NullOrEmpty())
			{
				name = this.prefix + "/" + name;
			}
			name = QuestGenUtility.NormalizeVarPath(name);
			return this.vars.Remove(name);
		}

		// Token: 0x06014D3F RID: 85311 RVA: 0x0063D6B0 File Offset: 0x0063B8B0
		public bool Exists(string name, bool isAbsoluteName = false)
		{
			if (name.NullOrEmpty())
			{
				return false;
			}
			if (!isAbsoluteName && !this.prefix.NullOrEmpty())
			{
				name = this.prefix + "/" + name;
			}
			name = QuestGenUtility.NormalizeVarPath(name);
			if (this.allowNonPrefixedLookup)
			{
				name = this.TryResolveFirstAvailableName(name);
			}
			return this.vars.ContainsKey(name);
		}

		// Token: 0x06014D40 RID: 85312 RVA: 0x0063D710 File Offset: 0x0063B910
		private string TryResolveFirstAvailableName(string nameWithPrefix)
		{
			if (nameWithPrefix == null)
			{
				return null;
			}
			nameWithPrefix = QuestGenUtility.NormalizeVarPath(nameWithPrefix);
			if (this.vars.ContainsKey(nameWithPrefix))
			{
				return nameWithPrefix;
			}
			int num = nameWithPrefix.LastIndexOf('/');
			if (num < 0)
			{
				return nameWithPrefix;
			}
			string text = nameWithPrefix.Substring(num + 1);
			string text2 = nameWithPrefix.Substring(0, num);
			string text3;
			for (;;)
			{
				text3 = text;
				if (!text2.NullOrEmpty())
				{
					text3 = text2 + "/" + text3;
				}
				if (this.vars.ContainsKey(text3))
				{
					break;
				}
				if (text2.NullOrEmpty())
				{
					return nameWithPrefix;
				}
				int num2 = text2.LastIndexOf('/');
				if (num2 >= 0)
				{
					text2 = text2.Substring(0, num2);
				}
				else
				{
					text2 = "";
				}
			}
			return text3;
		}

		// Token: 0x06014D41 RID: 85313 RVA: 0x0063D7B0 File Offset: 0x0063B9B0
		public void PushPrefix(string newPrefix, bool allowNonPrefixedLookup = false)
		{
			if (newPrefix.NullOrEmpty())
			{
				Log.Error("Tried to push a null prefix.");
				newPrefix = "unnamed";
			}
			if (!this.prefix.NullOrEmpty())
			{
				this.prefix += "/";
			}
			this.prefix += newPrefix;
			this.prevAllowNonPrefixedLookupStack.Push(this.allowNonPrefixedLookup);
			if (allowNonPrefixedLookup)
			{
				this.allowNonPrefixedLookup = true;
			}
		}

		// Token: 0x06014D42 RID: 85314 RVA: 0x0063D828 File Offset: 0x0063BA28
		public void PopPrefix()
		{
			int num = this.prefix.LastIndexOf('/');
			if (num >= 0)
			{
				this.prefix = this.prefix.Substring(0, num);
			}
			else
			{
				this.prefix = "";
			}
			if (this.prevAllowNonPrefixedLookupStack.Count != 0)
			{
				this.allowNonPrefixedLookup = this.prevAllowNonPrefixedLookupStack.Pop();
			}
		}

		// Token: 0x06014D43 RID: 85315 RVA: 0x0063D888 File Offset: 0x0063BA88
		public Slate.VarRestoreInfo GetRestoreInfo(string name)
		{
			bool flag = this.allowNonPrefixedLookup;
			this.allowNonPrefixedLookup = false;
			Slate.VarRestoreInfo result;
			try
			{
				object value;
				bool exists = this.TryGet<object>(name, out value, false);
				result = new Slate.VarRestoreInfo(name, exists, value);
			}
			finally
			{
				this.allowNonPrefixedLookup = flag;
			}
			return result;
		}

		// Token: 0x06014D44 RID: 85316 RVA: 0x0063D8D4 File Offset: 0x0063BAD4
		public void Restore(Slate.VarRestoreInfo varRestoreInfo)
		{
			if (varRestoreInfo.exists)
			{
				this.Set<object>(varRestoreInfo.name, varRestoreInfo.value, false);
				return;
			}
			this.Remove(varRestoreInfo.name, false);
		}

		// Token: 0x06014D45 RID: 85317 RVA: 0x0063D900 File Offset: 0x0063BB00
		public void SetAll(Slate otherSlate)
		{
			this.vars.Clear();
			foreach (KeyValuePair<string, object> keyValuePair in otherSlate.vars)
			{
				this.vars.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x06014D46 RID: 85318 RVA: 0x0063D970 File Offset: 0x0063BB70
		public void Reset()
		{
			this.vars.Clear();
		}

		// Token: 0x06014D47 RID: 85319 RVA: 0x0063D980 File Offset: 0x0063BB80
		public Slate DeepCopy()
		{
			Slate slate = new Slate();
			slate.prefix = this.prefix;
			foreach (KeyValuePair<string, object> keyValuePair in this.vars)
			{
				slate.vars.Add(keyValuePair.Key, keyValuePair.Value);
			}
			return slate;
		}

		// Token: 0x06014D48 RID: 85320 RVA: 0x0063D9F8 File Offset: 0x0063BBF8
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, object> keyValuePair in from x in this.vars
			orderby x.Key
			select x)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				string str = (keyValuePair.Value is IEnumerable && !(keyValuePair.Value is string)) ? ((IEnumerable)keyValuePair.Value).ToStringSafeEnumerable() : keyValuePair.Value.ToStringSafe<object>();
				stringBuilder.Append(keyValuePair.Key + "=" + str);
			}
			if (stringBuilder.Length == 0)
			{
				stringBuilder.Append("(none)");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400E80C RID: 59404
		private Dictionary<string, object> vars = new Dictionary<string, object>();

		// Token: 0x0400E80D RID: 59405
		private string prefix = "";

		// Token: 0x0400E80E RID: 59406
		private bool allowNonPrefixedLookup;

		// Token: 0x0400E80F RID: 59407
		private Stack<bool> prevAllowNonPrefixedLookupStack = new Stack<bool>();

		// Token: 0x0400E810 RID: 59408
		public const char Separator = '/';

		// Token: 0x02003BFD RID: 15357
		public struct VarRestoreInfo
		{
			// Token: 0x06014D4A RID: 85322 RVA: 0x0063DB11 File Offset: 0x0063BD11
			public VarRestoreInfo(string name, bool exists, object value)
			{
				this.name = name;
				this.exists = exists;
				this.value = value;
			}

			// Token: 0x0400E811 RID: 59409
			public string name;

			// Token: 0x0400E812 RID: 59410
			public bool exists;

			// Token: 0x0400E813 RID: 59411
			public object value;
		}
	}
}
