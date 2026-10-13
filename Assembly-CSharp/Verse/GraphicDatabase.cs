using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using LudeonTK;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000884 RID: 2180
	public static class GraphicDatabase
	{
		// Token: 0x06003755 RID: 14165 RVA: 0x0012839C File Offset: 0x0012659C
		public static Graphic Get<T>(string path) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, ShaderDatabase.Cutout, Vector2.one, Color.white, Color.white, null, 0, null, null));
		}

		// Token: 0x06003756 RID: 14166 RVA: 0x001283DC File Offset: 0x001265DC
		public static Graphic Get<T>(string path, Shader shader) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, Vector2.one, Color.white, Color.white, null, 0, null, null));
		}

		// Token: 0x06003757 RID: 14167 RVA: 0x00128418 File Offset: 0x00126618
		public static Graphic Get<T>(string path, Shader shader, string mask) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, Vector2.one, Color.white, Color.white, null, 0, null, mask));
		}

		// Token: 0x06003758 RID: 14168 RVA: 0x00128454 File Offset: 0x00126654
		public static Graphic Get<T>(string path, Shader shader, Vector2 drawSize, Color color) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, drawSize, color, Color.white, null, 0, null, null));
		}

		// Token: 0x06003759 RID: 14169 RVA: 0x00128488 File Offset: 0x00126688
		public static Graphic Get<T>(string path, Shader shader, string mask, int renderQueue) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, Vector2.one, Color.white, Color.white, null, renderQueue, null, mask));
		}

		// Token: 0x0600375A RID: 14170 RVA: 0x001284C4 File Offset: 0x001266C4
		public static Graphic Get<T>(string path, Shader shader, Vector2 drawSize, Color color, int renderQueue) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, drawSize, color, Color.white, null, renderQueue, null, null));
		}

		// Token: 0x0600375B RID: 14171 RVA: 0x001284F8 File Offset: 0x001266F8
		public static Graphic Get<T>(Texture2D texture, Shader shader, Vector2 drawSize, Color color, int renderQueue) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), texture, shader, drawSize, color, Color.white, null, renderQueue, null, null));
		}

		// Token: 0x0600375C RID: 14172 RVA: 0x0012852C File Offset: 0x0012672C
		public static Graphic Get<T>(string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, drawSize, color, colorTwo, null, 0, null, null));
		}

		// Token: 0x0600375D RID: 14173 RVA: 0x0012855C File Offset: 0x0012675C
		public static Graphic Get<T>(string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo, GraphicData data, string maskPath = null) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, drawSize, color, colorTwo, data, 0, null, maskPath));
		}

		// Token: 0x0600375E RID: 14174 RVA: 0x00128590 File Offset: 0x00126790
		public static Graphic Get(Type graphicClass, string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo, string maskPath = null)
		{
			return GraphicDatabase.Get(graphicClass, path, shader, drawSize, color, colorTwo, null, null, maskPath);
		}

		// Token: 0x0600375F RID: 14175 RVA: 0x001285B0 File Offset: 0x001267B0
		public static Graphic Get(Type graphicClass, Texture2D texture, Shader shader, Vector2 drawSize, Color color, Color colorTwo, GraphicData data, List<ShaderParameter> shaderParameters, string maskPath = null)
		{
			return GraphicDatabase.Get(new GraphicRequest(graphicClass, texture, shader, drawSize, color, colorTwo, data, 0, shaderParameters, maskPath));
		}

		// Token: 0x06003760 RID: 14176 RVA: 0x001285D8 File Offset: 0x001267D8
		public static Graphic Get(Type graphicClass, string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo, GraphicData data, List<ShaderParameter> shaderParameters, string maskPath = null)
		{
			return GraphicDatabase.Get(new GraphicRequest(graphicClass, path, shader, drawSize, color, colorTwo, data, 0, shaderParameters, maskPath));
		}

		// Token: 0x06003761 RID: 14177 RVA: 0x00128600 File Offset: 0x00126800
		private static Graphic Get(GraphicRequest req)
		{
			try
			{
				Func<GraphicRequest, Graphic> func;
				if (!GraphicDatabase.cachedGraphicGetters.TryGetValue(req.graphicClass, out func))
				{
					MethodInfo method = typeof(GraphicDatabase).GetMethod("GetInner", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).MakeGenericMethod(new Type[]
					{
						req.graphicClass
					});
					func = (Func<GraphicRequest, Graphic>)Delegate.CreateDelegate(typeof(Func<GraphicRequest, Graphic>), method);
					GraphicDatabase.cachedGraphicGetters.Add(req.graphicClass, func);
				}
				return func(req);
			}
			catch (Exception ex)
			{
				string[] array = new string[6];
				array[0] = "Exception getting ";
				int num = 1;
				Type graphicClass = req.graphicClass;
				array[num] = ((graphicClass != null) ? graphicClass.ToString() : null);
				array[2] = " at ";
				array[3] = req.path;
				array[4] = ": ";
				array[5] = ex.ToString();
				Log.Error(string.Concat(array));
			}
			return BaseContent.BadGraphic;
		}

		// Token: 0x06003762 RID: 14178 RVA: 0x001286E8 File Offset: 0x001268E8
		private static T GetInner<T>(GraphicRequest req) where T : Graphic, new()
		{
			req.color = req.color;
			req.colorTwo = req.colorTwo;
			req.renderQueue = ((req.renderQueue == 0 && req.graphicData != null) ? req.graphicData.renderQueue : req.renderQueue);
			Graphic graphic;
			if (!GraphicDatabase.allGraphics.TryGetValue(req, out graphic))
			{
				if (!UnityData.IsInMainThread)
				{
					Log.ErrorOnce(string.Format("Attempted to load a graphic off the main thread: {0}", req), req.GetHashCode());
					return default(T);
				}
				graphic = Activator.CreateInstance<T>();
				graphic.Init(req);
				GraphicDatabase.allGraphics.Add(req, graphic);
			}
			return (T)((object)graphic);
		}

		// Token: 0x06003763 RID: 14179 RVA: 0x001287B3 File Offset: 0x001269B3
		public static void Clear()
		{
			GraphicDatabase.allGraphics.Clear();
		}

		// Token: 0x06003764 RID: 14180 RVA: 0x001287C0 File Offset: 0x001269C0
		[DebugOutput("System", false)]
		public static void AllGraphicsLoaded()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("There are " + GraphicDatabase.allGraphics.Count.ToString() + " graphics loaded.");
			int num = 0;
			foreach (Graphic graphic in GraphicDatabase.allGraphics.Values)
			{
				stringBuilder.AppendLine(num.ToString() + " - " + graphic.ToString());
				if (num % 50 == 49)
				{
					Log.Message(stringBuilder.ToString());
					stringBuilder = new StringBuilder();
				}
				num++;
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04002976 RID: 10614
		private static Dictionary<GraphicRequest, Graphic> allGraphics = new Dictionary<GraphicRequest, Graphic>();

		// Token: 0x04002977 RID: 10615
		private static Dictionary<Type, Func<GraphicRequest, Graphic>> cachedGraphicGetters = new Dictionary<Type, Func<GraphicRequest, Graphic>>();
	}
}
