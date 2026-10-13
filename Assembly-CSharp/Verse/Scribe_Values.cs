using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000868 RID: 2152
	public static class Scribe_Values
	{
		// Token: 0x06003650 RID: 13904 RVA: 0x00123324 File Offset: 0x00121524
		public static void Look<T>(ref T value, string label, T defaultValue = default(T), bool forceSave = false)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (typeof(T) == typeof(TargetInfo))
				{
					Log.Error("Saving a TargetInfo " + label + " with Scribe_Values. TargetInfos must be saved with Scribe_TargetInfo.");
					return;
				}
				if (typeof(Thing).IsAssignableFrom(typeof(T)))
				{
					Log.Error("Using Scribe_Values with a Thing reference " + label + ". Use Scribe_References or Scribe_Deep instead.");
					return;
				}
				if (typeof(IExposable).IsAssignableFrom(typeof(T)))
				{
					Log.Error("Using Scribe_Values with a IExposable reference " + label + ". Use Scribe_References or Scribe_Deep instead.");
					return;
				}
				if (GenTypes.IsDef(typeof(T)))
				{
					Log.Error("Using Scribe_Values with a Def " + label + ". Use Scribe_Defs instead.");
					return;
				}
				if (forceSave || (value == null && defaultValue != null) || (value != null && !value.Equals(defaultValue)))
				{
					if (value == null)
					{
						if (!Scribe.EnterNode(label))
						{
							return;
						}
						try
						{
							Scribe.saver.WriteAttribute("IsNull", "True");
							return;
						}
						finally
						{
							Scribe.ExitNode();
						}
					}
					T t = value;
					string value2;
					if (t is Vector2)
					{
						value2 = (t as Vector2).ToString("G9");
					}
					else
					{
						t = value;
						if (t is Vector3)
						{
							value2 = (t as Vector3).ToString("G9");
						}
						else
						{
							t = value;
							if (t is Quaternion)
							{
								value2 = (t as Quaternion).ToString("G9");
							}
							else
							{
								t = value;
								if (t is float)
								{
									value2 = (t as float).ToString("G9");
								}
								else
								{
									value2 = value.ToString();
								}
							}
						}
					}
					Scribe.saver.WriteElement(label, value2);
					return;
				}
			}
			else if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				value = ScribeExtractor.ValueFromNode<T>(Scribe.loader.curXmlParent[label], defaultValue);
			}
		}
	}
}
