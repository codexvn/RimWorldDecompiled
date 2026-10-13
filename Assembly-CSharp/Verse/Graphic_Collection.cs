using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000891 RID: 2193
	public abstract class Graphic_Collection : Graphic
	{
		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x060037A5 RID: 14245 RVA: 0x00129599 File Offset: 0x00127799
		protected virtual Type SingleGraphicType
		{
			get
			{
				return typeof(Graphic_Single);
			}
		}

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x060037A6 RID: 14246 RVA: 0x001295A5 File Offset: 0x001277A5
		protected virtual Type MultiGraphicType
		{
			get
			{
				return typeof(Graphic_Multi);
			}
		}

		// Token: 0x060037A7 RID: 14247 RVA: 0x001295B4 File Offset: 0x001277B4
		public override void TryInsertIntoAtlas(TextureAtlasGroup groupKey)
		{
			Graphic[] array = this.subGraphics;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].TryInsertIntoAtlas(groupKey);
			}
		}

		// Token: 0x060037A8 RID: 14248 RVA: 0x001295E0 File Offset: 0x001277E0
		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			if (req.path.NullOrEmpty())
			{
				throw new ArgumentNullException("folderPath");
			}
			if (req.shader == null)
			{
				throw new ArgumentNullException("shader");
			}
			this.path = req.path;
			this.maskPath = req.maskPath;
			this.color = req.color;
			this.colorTwo = req.colorTwo;
			this.drawSize = req.drawSize;
			List<ValueTuple<Texture2D, string>> list = (from x in ContentFinder<Texture2D>.GetAllInFolder(req.path)
			where !x.name.EndsWith(Graphic_Single.MaskSuffix)
			orderby x.name
			select new ValueTuple<Texture2D, string>(x, x.name.Split('_', StringSplitOptions.None)[0])).ToList<ValueTuple<Texture2D, string>>();
			if (list.NullOrEmpty<ValueTuple<Texture2D, string>>())
			{
				Log.Error("Collection cannot init: No textures found at path " + req.path);
				this.subGraphics = new Graphic[]
				{
					BaseContent.BadGraphic
				};
				return;
			}
			List<Graphic> list2 = new List<Graphic>();
			foreach (IGrouping<string, ValueTuple<Texture2D, string>> grouping in from s in list
			group s by s.Item2)
			{
				List<ValueTuple<Texture2D, string>> list3 = grouping.ToList<ValueTuple<Texture2D, string>>();
				string path = req.path + "/" + grouping.Key;
				bool flag = false;
				for (int i = list3.Count - 1; i >= 0; i--)
				{
					if (list3[i].Item1.name.Contains("_east") || list3[i].Item1.name.Contains("_north") || list3[i].Item1.name.Contains("_west") || list3[i].Item1.name.Contains("_south"))
					{
						list3.RemoveAt(i);
						flag = true;
					}
				}
				if (list3.Count > 0)
				{
					foreach (ValueTuple<Texture2D, string> valueTuple in list3)
					{
						string path2 = req.path + "/" + valueTuple.Item1.name;
						string text = this.path + Graphic_Single.MaskSuffix;
						if (!ContentFinder<Texture2D>.Get(text, false))
						{
							text = null;
						}
						list2.Add(GraphicDatabase.Get(this.SingleGraphicType, path2, req.shader, this.drawSize, this.color, this.colorTwo, this.data, req.shaderParameters, text));
					}
				}
				if (flag)
				{
					list2.Add(GraphicDatabase.Get(this.MultiGraphicType, path, req.shader, this.drawSize, this.color, this.colorTwo, this.data, req.shaderParameters, null));
				}
			}
			this.subGraphics = list2.ToArray();
		}

		// Token: 0x04002985 RID: 10629
		protected Graphic[] subGraphics;
	}
}
