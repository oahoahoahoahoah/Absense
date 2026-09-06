using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;

// Token: 0x0200001D RID: 29
public static class Menu
{
	// Token: 0x17000009 RID: 9
	// (get) Token: 0x060000D2 RID: 210 RVA: 0x000102F1 File Offset: 0x0000E4F1
	public static bool IsRunning
	{
		get
		{
			return Menu.running;
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x060000D3 RID: 211 RVA: 0x000102F8 File Offset: 0x0000E4F8
	private static Dictionary<string, Menu.SettingField> HandlerMap
	{
		get
		{
			bool flag = Menu.setHandlers != null;
			Dictionary<string, Menu.SettingField> dictionary2;
			if (flag)
			{
				dictionary2 = Menu.setHandlers;
			}
			else
			{
				Dictionary<string, Menu.SettingField> dictionary = new Dictionary<string, Menu.SettingField>(StringComparer.Ordinal);
				foreach (KeyValuePair<string, Type> item in Menu.getHandlers)
				{
					FieldInfo[] fields = item.Value.GetFields(BindingFlags.Static | BindingFlags.Public);
					FieldInfo[] array = fields;
					FieldInfo[] array2 = array;
					int i = 0;
					while (i < array2.Length)
					{
						FieldInfo fieldInfo = array2[i];
						Type fieldType = fieldInfo.FieldType;
						bool flag2 = fieldType == typeof(bool);
						string kind;
						if (flag2)
						{
							kind = "bool";
							goto IL_0118;
						}
						bool flag3 = fieldType == typeof(int);
						if (flag3)
						{
							kind = "int";
							goto IL_0118;
						}
						bool flag4 = fieldType == typeof(float);
						if (flag4)
						{
							kind = "float";
							goto IL_0118;
						}
						bool flag5 = fieldType == typeof(string);
						if (flag5)
						{
							kind = "string";
							goto IL_0118;
						}
						bool flag6 = !fieldType.IsEnum;
						if (!flag6)
						{
							kind = "enum";
							goto IL_0118;
						}
						IL_0174:
						i++;
						continue;
						IL_0118:
						string key = item.Key + "." + fieldInfo.Name;
						dictionary[key] = new Menu.SettingField
						{
							Scope = item.Key,
							Name = fieldInfo.Name,
							Info = fieldInfo,
							Kind = kind
						};
						goto IL_0174;
					}
				}
				Menu.setHandlers = dictionary;
				dictionary2 = Menu.setHandlers;
			}
			return dictionary2;
		}
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x000104D4 File Offset: 0x0000E6D4
	public static void Start()
	{
		bool flag = Menu.running;
		if (!flag)
		{
			try
			{
				Menu.listener = new TcpListener(IPAddress.Loopback, 47811);
				Menu.listener.Start();
				Menu.running = true;
				ThreadStart threadStart;
				if ((threadStart = Menu.<>O.<0>__AcceptLoop) == null)
				{
					threadStart = (Menu.<>O.<0>__AcceptLoop = new ThreadStart(Menu.AcceptLoop));
				}
				Menu.serverThread = new Thread(threadStart)
				{
					IsBackground = true,
					Name = "AbsensePipeline"
				};
				Menu.serverThread.Start();
			}
			catch (Exception)
			{
				Menu.running = false;
			}
		}
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x00010578 File Offset: 0x0000E778
	public static void Stop()
	{
		Menu.running = false;
		try
		{
			TcpListener tcpListener = Menu.listener;
			if (tcpListener != null)
			{
				tcpListener.Stop();
			}
		}
		catch
		{
		}
		Menu.listener = null;
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x000105BC File Offset: 0x0000E7BC
	public static void Update()
	{
		int num = 0;
		for (;;)
		{
			int num2 = num;
			bool flag = num2 >= 32;
			if (flag)
			{
				break;
			}
			Action action = null;
			Queue<Action> queue = Menu.mainThreadQueue;
			lock (queue)
			{
				bool flag3 = Menu.mainThreadQueue.Count == 0;
				if (flag3)
				{
					break;
				}
				action = Menu.mainThreadQueue.Dequeue();
			}
			try
			{
				if (action != null)
				{
					action();
				}
			}
			catch (Exception)
			{
			}
			num++;
		}
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x00010664 File Offset: 0x0000E864
	private static void Enqueue(Action action)
	{
		Queue<Action> queue = Menu.mainThreadQueue;
		lock (queue)
		{
			Menu.mainThreadQueue.Enqueue(action);
		}
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x000106B0 File Offset: 0x0000E8B0
	public static void UpdateOverlay()
	{
		try
		{
			Camera main = Camera.main;
			bool flag3 = !(main == null) && !(GorillaParent.instance == null) && VRRigCache.ActiveRigs != null;
			if (flag3)
			{
				Vector3 position = main.transform.position;
				Vector3 forward = main.transform.forward;
				Quaternion rotation = main.transform.rotation;
				float fieldOfView = main.fieldOfView;
				int width = Screen.width;
				int height = Screen.height;
				StringBuilder stringBuilder = new StringBuilder(2048);
				stringBuilder.Append("{\"cam\":{\"x\":").Append(position.x.ToString("F2")).Append(",\"y\":")
					.Append(position.y.ToString("F2"))
					.Append(",\"z\":")
					.Append(position.z.ToString("F2"));
				stringBuilder.Append(",\"fx\":").Append(forward.x.ToString("F3")).Append(",\"fy\":")
					.Append(forward.y.ToString("F3"))
					.Append(",\"fz\":")
					.Append(forward.z.ToString("F3"));
				stringBuilder.Append(",\"rx\":").Append(rotation.x.ToString("F4")).Append(",\"ry\":")
					.Append(rotation.y.ToString("F4"))
					.Append(",\"rz\":")
					.Append(rotation.z.ToString("F4"))
					.Append(",\"rw\":")
					.Append(rotation.w.ToString("F4"));
				stringBuilder.Append(",\"fov\":").Append(fieldOfView.ToString("F1")).Append(",\"w\":")
					.Append(width)
					.Append(",\"h\":")
					.Append(height);
				stringBuilder.Append("},\"p\":[");
				GorillaTagger instance = GorillaTagger.Instance;
				bool flag = true;
				foreach (VRRig activeRig in VRRigCache.ActiveRigs)
				{
					bool flag4 = activeRig == null || instance == null || activeRig == instance.offlineVRRig || !activeRig.gameObject.activeInHierarchy;
					if (!flag4)
					{
						Vector3 position2 = activeRig.transform.position;
						Transform val = activeRig.transform.Find("rig/body/head");
						Vector3 val2 = ((val != null) ? val.position : (position2 + Vector3.up * 1.7f));
						Color playerColor = activeRig.playerColor;
						int value = ((int)(playerColor.r * 255f) << 16) | ((int)(playerColor.g * 255f) << 8) | (int)(playerColor.b * 255f);
						bool flag5 = !flag;
						if (flag5)
						{
							stringBuilder.Append(',');
						}
						flag = false;
						stringBuilder.Append("{\"hx\":").Append(val2.x.ToString("F2")).Append(",\"hy\":")
							.Append(val2.y.ToString("F2"))
							.Append(",\"hz\":")
							.Append(val2.z.ToString("F2"));
						stringBuilder.Append(",\"fx\":").Append(position2.x.ToString("F2")).Append(",\"fy\":")
							.Append(position2.y.ToString("F2"))
							.Append(",\"fz\":")
							.Append(position2.z.ToString("F2"));
						stringBuilder.Append(",\"c\":").Append(value);
						stringBuilder.Append(",\"b\":[");
						int num = ((activeRig.mainSkin != null && activeRig.mainSkin.bones != null) ? Mathf.Min(24, activeRig.mainSkin.bones.Length) : 0);
						bool flag2 = true;
						for (int i = 0; i < 24; i++)
						{
							bool flag6 = !flag2;
							if (flag6)
							{
								stringBuilder.Append(',');
							}
							flag2 = false;
							Transform val3 = ((i < num) ? activeRig.mainSkin.bones[i] : null);
							bool flag7 = val3 != null;
							if (flag7)
							{
								Vector3 position3 = val3.position;
								stringBuilder.Append(position3.x.ToString("F2")).Append(',').Append(position3.y.ToString("F2"))
									.Append(',')
									.Append(position3.z.ToString("F2"));
							}
							else
							{
								stringBuilder.Append("0,0,0");
							}
						}
						stringBuilder.Append("]}");
					}
				}
				stringBuilder.Append("]}");
				object obj = Menu.object_1;
				lock (obj)
				{
					Menu.string_0 = stringBuilder.ToString();
					return;
				}
			}
			object obj2 = Menu.object_1;
			lock (obj2)
			{
				Menu.string_0 = "{\"cam\":{\"x\":0,\"y\":0,\"z\":0,\"fx\":0,\"fy\":0,\"fz\":0,\"rx\":0,\"ry\":0,\"rz\":0,\"rw\":1,\"fov\":90,\"w\":1920,\"h\":1080},\"p\":[]}";
			}
		}
		catch
		{
		}
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x00010CCC File Offset: 0x0000EECC
	private static void AcceptLoop()
	{
		while (Menu.running)
		{
			try
			{
				TcpClient client = Menu.listener.AcceptTcpClient();
				IPEndPoint iPEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
				bool flag = iPEndPoint != null && IPAddress.IsLoopback(iPEndPoint.Address);
				if (flag)
				{
					new Thread(delegate
					{
						Menu.HandleClient(client);
					})
					{
						IsBackground = true,
						Name = "AbsensePipelineClient"
					}.Start();
				}
				else
				{
					try
					{
						client.Close();
					}
					catch
					{
					}
				}
			}
			catch
			{
				bool flag2 = !Menu.running;
				if (flag2)
				{
					break;
				}
			}
		}
	}

	// Token: 0x060000DA RID: 218 RVA: 0x00010DB0 File Offset: 0x0000EFB0
	private static void HandleClient(TcpClient client)
	{
		try
		{
			client.NoDelay = true;
			using (NetworkStream stream = client.GetStream())
			{
				using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
				{
					using (StreamWriter streamWriter = new StreamWriter(stream, new UTF8Encoding(false))
					{
						AutoFlush = true,
						NewLine = "\n"
					})
					{
						while (Menu.running && client.Connected)
						{
							string text = streamReader.ReadLine();
							bool flag = text != null;
							if (!flag)
							{
								break;
							}
							string value;
							try
							{
								value = Menu.HandleRequest(text);
							}
							catch (Exception ex)
							{
								value = "{\"ok\":false,\"err\":" + Menu.JsonEscape(ex.Message) + "}";
							}
							streamWriter.WriteLine(value);
						}
					}
				}
			}
		}
		catch
		{
		}
		finally
		{
			try
			{
				client.Close();
			}
			catch
			{
			}
		}
	}

	// Token: 0x060000DB RID: 219 RVA: 0x00010F00 File Offset: 0x0000F100
	private static string HandleRequest(string input)
	{
		string text = Menu.HandleSet(input, "op");
		bool flag = string.IsNullOrEmpty(text);
		string text4;
		if (flag)
		{
			text4 = "{\"ok\":false,\"err\":\"missing op\"}";
		}
		else
		{
			string text2 = text;
			bool flag2 = text2 == "ping";
			if (flag2)
			{
				text4 = "{\"ok\":true,\"data\":\"pong\"}";
			}
			else
			{
				bool flag3 = text == "schema";
				if (flag3)
				{
					text4 = Menu.BuildStateJson();
				}
				else
				{
					bool flag4 = text == "get";
					if (flag4)
					{
						text4 = Menu.BuildFeaturesJson();
					}
					else
					{
						string text3 = text;
						bool flag5 = !(text3 == "set");
						if (flag5)
						{
							bool flag6 = text == "action";
							if (flag6)
							{
								text4 = Menu.HandleCommand(Menu.HandleSet(input, "name"), Menu.HandleSet(input, "arg"));
							}
							else
							{
								text4 = "{\"ok\":false,\"err\":\"unknown op\"}";
							}
						}
						else
						{
							text4 = Menu.JsonUnescape(input);
						}
					}
				}
			}
		}
		return text4;
	}

	// Token: 0x060000DC RID: 220 RVA: 0x00010FEC File Offset: 0x0000F1EC
	private static string BuildStateJson()
	{
		StringBuilder stringBuilder = new StringBuilder(4096);
		stringBuilder.Append("{\"ok\":true,\"version\":1,\"fields\":[");
		bool flag = true;
		foreach (KeyValuePair<string, Menu.SettingField> item in Menu.HandlerMap)
		{
			bool flag2 = !flag;
			if (flag2)
			{
				stringBuilder.Append(',');
			}
			flag = false;
			stringBuilder.Append('{');
			stringBuilder.Append("\"key\":").Append(Menu.JsonEscape(item.Key)).Append(',');
			stringBuilder.Append("\"kind\":\"").Append(item.Value.Kind).Append('"');
			bool flag3 = item.Value.Kind == "enum";
			if (flag3)
			{
				stringBuilder.Append(",\"enum\":\"").Append(item.Value.Info.FieldType.Name).Append('"');
				string[] names = Enum.GetNames(item.Value.Info.FieldType);
				int[] array = (int[])Enum.GetValues(item.Value.Info.FieldType);
				stringBuilder.Append(",\"options\":[");
				for (int i = 0; i < names.Length; i++)
				{
					bool flag4 = i > 0;
					if (flag4)
					{
						stringBuilder.Append(',');
					}
					stringBuilder.Append(Menu.JsonEscape(names[i]));
				}
				stringBuilder.Append("],\"values\":[");
				for (int j = 0; j < array.Length; j++)
				{
					bool flag5 = j > 0;
					if (flag5)
					{
						stringBuilder.Append(',');
					}
					stringBuilder.Append(array[j].ToString(CultureInfo.InvariantCulture));
				}
				stringBuilder.Append(']');
			}
			stringBuilder.Append('}');
		}
		stringBuilder.Append("]}");
		return stringBuilder.ToString();
	}

	// Token: 0x060000DD RID: 221 RVA: 0x00011224 File Offset: 0x0000F424
	private static string BuildFeaturesJson()
	{
		StringBuilder stringBuilder = new StringBuilder(8192);
		stringBuilder.Append("{\"ok\":true,\"values\":{");
		bool flag = true;
		foreach (KeyValuePair<string, Menu.SettingField> item in Menu.HandlerMap)
		{
			bool flag2 = !flag;
			if (flag2)
			{
				stringBuilder.Append(',');
			}
			flag = false;
			stringBuilder.Append(Menu.JsonEscape(item.Key)).Append(':');
			object object_ = null;
			try
			{
				object_ = item.Value.Info.GetValue(null);
			}
			catch
			{
			}
			Menu.AppendJsonField(stringBuilder, object_, item.Value.Kind);
		}
		stringBuilder.Append("},\"meta\":{");
		StringBuilder stringBuilder2 = stringBuilder.Append("\"panicActive\":");
		string value = (Settings.PanicActive ? "true" : "false");
		stringBuilder2.Append(value);
		StringBuilder stringBuilder3 = stringBuilder.Append(",\"showMenu\":");
		string value2 = (Settings.ShowMenu ? "true" : "false");
		stringBuilder3.Append(value2);
		stringBuilder.Append(",\"authenticated\":true");
		StringBuilder stringBuilder4 = stringBuilder.Append(",\"username\":");
		stringBuilder4.Append(Menu.JsonEscape(""));
		stringBuilder.Append("}}");
		return stringBuilder.ToString();
	}

	// Token: 0x060000DE RID: 222 RVA: 0x000113A4 File Offset: 0x0000F5A4
	private static void AppendJsonField(StringBuilder sb, object value, string input)
	{
		bool flag = value == null;
		if (flag)
		{
			sb.Append("null");
		}
		else if (!(input == "string"))
		{
			if (!(input == "enum"))
			{
				if (!(input == "float"))
				{
					if (!(input == "int"))
					{
						if (!(input == "bool"))
						{
							sb.Append("null");
						}
						else
						{
							sb.Append(((bool)value) ? "true" : "false");
						}
					}
					else
					{
						sb.Append(((int)value).ToString(CultureInfo.InvariantCulture));
					}
				}
				else
				{
					float f = (float)value;
					bool flag2 = !float.IsNaN(f) && !float.IsInfinity(f);
					if (flag2)
					{
						sb.Append(f.ToString("R", CultureInfo.InvariantCulture));
					}
					else
					{
						sb.Append("0");
					}
				}
			}
			else
			{
				sb.Append(((int)value).ToString(CultureInfo.InvariantCulture));
			}
		}
		else
		{
			sb.Append(Menu.JsonEscape((string)value));
		}
	}

	// Token: 0x060000DF RID: 223 RVA: 0x000114EC File Offset: 0x0000F6EC
	private static string JsonUnescape(string input)
	{
		string text = Menu.HandleAction(input, "values");
		bool flag = string.IsNullOrEmpty(text);
		string text2;
		if (flag)
		{
			text2 = "{\"ok\":true,\"changed\":0}";
		}
		else
		{
			int num = 0;
			int i = 0;
			while (i < text.Length)
			{
				int num2 = text.IndexOf('"', i);
				bool flag2 = num2 < 0;
				if (flag2)
				{
					break;
				}
				int num3 = Menu.GetJsonInt(text, num2 + 1);
				bool flag3 = num3 < 0;
				if (flag3)
				{
					break;
				}
				string keyName = Menu.GetJsonString(text.Substring(num2 + 1, num3 - num2 - 1));
				int num4 = text.IndexOf(':', num3);
				bool flag4 = num4 < 0;
				if (flag4)
				{
					break;
				}
				int j = num4 + 1;
				while (j < text.Length && char.IsWhiteSpace(text[j]))
				{
					j++;
				}
				bool flag5 = j >= text.Length;
				if (flag5)
				{
					break;
				}
				int int_;
				string key = Menu.ReadJsonToken(text, j, out int_);
				i = int_;
				Menu.SettingField value;
				bool flag6 = Menu.HandlerMap.TryGetValue(key, out value);
				if (flag6)
				{
					try
					{
						bool flag7 = Menu.MatchesFilter(value, key);
						if (flag7)
						{
							num++;
						}
					}
					catch
					{
					}
				}
				while (i < text.Length)
				{
					char c = text[i];
					bool flag8 = c != ',' && !char.IsWhiteSpace(text[i]);
					if (flag8)
					{
						break;
					}
					i++;
				}
			}
			text2 = "{\"ok\":true,\"changed\":" + num.ToString() + "}";
		}
		return text2;
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x000116A4 File Offset: 0x0000F8A4
	private static bool MatchesFilter(Menu.SettingField field, string input)
	{
		input = ((input != null) ? input.Trim() : null);
		bool flag2 = string.IsNullOrEmpty(input);
		bool flag3;
		if (flag2)
		{
			flag3 = false;
		}
		else
		{
			string kind = field.Kind;
			string text6 = kind;
			if (!(text6 == "string"))
			{
				if (!(text6 == "enum"))
				{
					if (!(text6 == "float"))
					{
						if (!(text6 == "int"))
						{
							if (!(text6 == "bool"))
							{
								flag3 = false;
							}
							else
							{
								bool flag4 = !(input == "true");
								int num;
								if (flag4)
								{
									string text = input;
									num = ((text == "1") ? 1 : 0);
								}
								else
								{
									num = 1;
								}
								bool flag = (byte)num > 0;
								field.Info.SetValue(null, flag);
								flag3 = true;
							}
						}
						else
						{
							int result;
							bool flag5 = int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
							if (flag5)
							{
								field.Info.SetValue(null, result);
								flag3 = true;
							}
							else
							{
								flag3 = false;
							}
						}
					}
					else
					{
						string s = input;
						float result2;
						bool flag6 = float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out result2);
						if (flag6)
						{
							field.Info.SetValue(null, result2);
							flag3 = true;
						}
						else
						{
							flag3 = false;
						}
					}
				}
				else
				{
					int result3;
					bool flag7 = int.TryParse(input, out result3);
					if (flag7)
					{
						field.Info.SetValue(null, Enum.ToObject(field.Info.FieldType, result3));
						flag3 = true;
					}
					else
					{
						flag3 = false;
					}
				}
			}
			else
			{
				string text2 = input;
				int length = text2.Length;
				bool flag8 = length >= 2;
				if (flag8)
				{
					string text3 = text2;
					char c = text3[0];
					bool flag9 = c == '"';
					if (flag9)
					{
						string text4 = text2;
						int length2 = text2.Length;
						char c2 = text4[length2 - 1];
						bool flag10 = c2 == '"';
						if (flag10)
						{
							string text5 = text2;
							int length3 = text2.Length;
							text2 = Menu.GetJsonString(text5.Substring(1, length3 - 2));
						}
					}
				}
				field.Info.SetValue(null, text2);
				flag3 = true;
			}
		}
		return flag3;
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x000118D0 File Offset: 0x0000FAD0
	private static string HandleCommand(string input, string key)
	{
		bool flag = string.IsNullOrEmpty(input);
		string text4;
		if (flag)
		{
			text4 = "{\"ok\":false,\"err\":\"missing name\"}";
		}
		else
		{
			string extra = "";
			bool flag2 = input != null;
			if (flag2)
			{
				switch (input.Length)
				{
				case 8:
				{
					char c11 = input[0];
					bool flag3 = c11 <= 'h';
					if (flag3)
					{
						bool flag4 = c11 != 'e';
						if (flag4)
						{
							bool flag5 = c11 != 'h' || !(input == "hideMenu");
							if (flag5)
							{
								goto IL_0A37;
							}
							Settings.ShowMenu = false;
						}
						else
						{
							bool flag6 = !(input == "esp.data");
							if (flag6)
							{
								goto IL_0A37;
							}
							extra = Menu.string_0 ?? "";
						}
					}
					else
					{
						bool flag7 = c11 != 's';
						if (flag7)
						{
							bool flag8 = c11 != 't' || !(input == "time.set");
							if (flag8)
							{
								goto IL_0A37;
							}
							int t;
							bool flag9 = int.TryParse(key ?? "0", out t);
							if (flag9)
							{
								Menu.Enqueue(delegate
								{
									try
									{
										Settings.CurrentTimeOfDay = t;
									}
									catch
									{
									}
								});
							}
						}
						else
						{
							bool flag10 = !(input == "showMenu");
							if (flag10)
							{
								goto IL_0A37;
							}
							Settings.ShowMenu = true;
						}
					}
					break;
				}
				case 9:
				{
					bool flag11 = !(input == "room.join");
					if (flag11)
					{
						goto IL_0A37;
					}
					Menu.Enqueue(delegate
					{
						try
						{
							RoomJoiner.RoomCode = key ?? "";
							RoomJoiner.JoinRoom();
						}
						catch
						{
						}
					});
					break;
				}
				case 10:
				{
					bool flag12 = !(input == "toggleMenu");
					if (flag12)
					{
						goto IL_0A37;
					}
					Settings.ShowMenu = !Settings.ShowMenu;
					break;
				}
				case 11:
				{
					char c12 = input[8];
					bool flag13 = c12 != 'a';
					if (flag13)
					{
						bool flag14 = c12 != 'i';
						if (flag14)
						{
							bool flag15 = c12 != 'o' || !(input == "config.load");
							if (flag15)
							{
								goto IL_0A37;
							}
							Menu.Enqueue(delegate
							{
								ConfigManager.LoadConfig(string.IsNullOrEmpty(key) ? null : key);
							});
						}
						else
						{
							bool flag16 = !(input == "config.list");
							if (flag16)
							{
								goto IL_0A37;
							}
							try
							{
								ConfigManager.SaveCurrentConfig();
							}
							catch
							{
							}
							extra = string.Join("|", ConfigManager.ListConfigNames() ?? new List<string>());
						}
					}
					else
					{
						bool flag17 = !(input == "config.save");
						if (flag17)
						{
							goto IL_0A37;
						}
						Menu.Enqueue(delegate
						{
							ConfigManager.SaveConfig(string.IsNullOrEmpty(key) ? null : key);
						});
					}
					break;
				}
				case 12:
				{
					char c13 = input[7];
					bool flag18 = c13 != 'a';
					if (flag18)
					{
						char c14 = c13;
						bool flag19 = c14 != 'o';
						if (flag19)
						{
							bool flag20 = c13 != 'r' || !(input == "skybox.reset");
							if (flag20)
							{
								goto IL_0A37;
							}
							Menu.Enqueue(delegate
							{
								try
								{
									Skybox.Disable();
								}
								catch
								{
								}
							});
						}
						else
						{
							bool flag21 = !(input == "panic.toggle");
							if (flag21)
							{
								goto IL_0A37;
							}
							Menu.Enqueue(delegate
							{
								try
								{
									TagFreeze.TriggerPanic();
								}
								catch
								{
								}
							});
						}
					}
					else
					{
						bool flag22 = !(input == "skybox.apply");
						if (flag22)
						{
							goto IL_0A37;
						}
						Menu.Enqueue(delegate
						{
							try
							{
								Skybox.Rebuild();
							}
							catch
							{
							}
						});
					}
					break;
				}
				case 13:
				{
					char c15 = input[0];
					char c16 = c15;
					bool flag23 = c16 != 'a';
					if (flag23)
					{
						bool flag24 = c15 != 'c';
						if (flag24)
						{
							bool flag25 = c15 != 'w' || !(input == "weather.cycle");
							if (flag25)
							{
								goto IL_0A37;
							}
							Menu.Enqueue(delegate
							{
								try
								{
									Weather.ApplyWeather();
								}
								catch
								{
								}
							});
						}
						else
						{
							bool flag26 = !(input == "config.delete");
							if (flag26)
							{
								goto IL_0A37;
							}
							Menu.Enqueue(delegate
							{
								ConfigManager.DeleteConfig(key);
							});
						}
					}
					else
					{
						bool flag27 = !(input == "auth.setToken");
						if (flag27)
						{
							goto IL_0A37;
						}
						bool flag28 = !string.IsNullOrEmpty(key);
						if (flag28)
						{
							int num2 = key.IndexOf('|');
							bool flag29 = num2 >= 0;
							if (flag29)
							{
								Menu.AuthToken = key.Substring(0, num2);
								string text = key;
								Menu.Hwid = text.Substring(num2 + 1);
							}
							else
							{
								Menu.AuthToken = key;
							}
						}
						else
						{
							Menu.AuthToken = "";
							Menu.Hwid = "";
						}
						try
						{
							ConfigManager.AutoLoad();
						}
						catch
						{
						}
					}
					break;
				}
				case 14:
				{
					char c17 = input[12];
					char c18 = c17;
					bool flag30 = c18 != 'k';
					if (flag30)
					{
						bool flag31 = c17 != 's' || !(input == "community.list");
						if (flag31)
						{
							goto IL_0A37;
						}
						try
						{
							CommunityConfigBrowser.Refresh(key ?? "");
						}
						catch
						{
						}
						for (int num3 = 0; num3 < 30; num3++)
						{
							bool flag32 = !CommunityConfigBrowser.IsLoading;
							if (flag32)
							{
								break;
							}
							Thread.Sleep(100);
						}
						extra = Menu.BuildConfigListJson();
					}
					else
					{
						bool flag33 = !(input == "community.like");
						if (flag33)
						{
							goto IL_0A37;
						}
						bool flag34 = !string.IsNullOrEmpty(key);
						if (flag34)
						{
							Menu.Enqueue(delegate
							{
								try
								{
									CommunityConfigBrowser.Delete(key);
								}
								catch
								{
								}
							});
						}
					}
					break;
				}
				case 15:
				{
					char c19 = input[0];
					bool flag35 = c19 != 'a';
					if (flag35)
					{
						bool flag36 = c19 != 'r' || !(input == "room.disconnect");
						if (flag36)
						{
							goto IL_0A37;
						}
						Menu.Enqueue(delegate
						{
							try
							{
								RoomJoiner.LeaveRoom();
							}
							catch
							{
							}
						});
					}
					else
					{
						bool flag37 = !(input == "autoBranch.stop");
						if (flag37)
						{
							goto IL_0A37;
						}
						Menu.Enqueue(delegate
						{
							try
							{
								Macros.StartReplay();
								Macros.StopReplayAll();
							}
							catch
							{
							}
						});
					}
					break;
				}
				case 16:
				{
					bool flag38 = !(input == "autoBranch.clear");
					if (flag38)
					{
						goto IL_0A37;
					}
					Menu.Enqueue(delegate
					{
						try
						{
							Macros.UploadMacro();
						}
						catch
						{
						}
					});
					break;
				}
				case 17:
				{
					char c20 = input[13];
					bool flag39 = c20 != 'c';
					if (flag39)
					{
						bool flag40 = c20 != 'l';
						if (flag40)
						{
							bool flag41 = c20 != 'p' || !(input == "autoBranch.replay");
							if (flag41)
							{
								goto IL_0A37;
							}
							Menu.Enqueue(delegate
							{
								try
								{
									Macros.RefreshMacroList();
								}
								catch
								{
								}
							});
						}
						else
						{
							bool flag42 = !(input == "community.publish");
							if (flag42)
							{
								goto IL_0A37;
							}
							Menu.Enqueue(delegate
							{
								try
								{
									CommunityConfigBrowser.Like(key);
								}
								catch
								{
								}
							});
						}
					}
					else
					{
						bool flag43 = !(input == "autoBranch.record");
						if (flag43)
						{
							goto IL_0A37;
						}
						Menu.Enqueue(delegate
						{
							try
							{
								Macros.StartRecording();
							}
							catch
							{
							}
						});
					}
					break;
				}
				case 18:
				{
					bool flag44 = !(input == "community.download");
					if (flag44)
					{
						goto IL_0A37;
					}
					bool flag45 = !string.IsNullOrEmpty(key);
					if (flag45)
					{
						int num4 = key.IndexOf('|');
						string id = ((num4 > 0) ? key.Substring(0, num4) : key);
						string nm = ((num4 > 0) ? key.Substring(num4 + 1) : key);
						Menu.Enqueue(delegate
						{
							try
							{
								CommunityConfigBrowser.Upload(id, nm);
							}
							catch
							{
							}
						});
					}
					break;
				}
				case 19:
				{
					char c21 = input[16];
					bool flag46 = c21 != 'g';
					if (flag46)
					{
						bool flag47 = c21 != 's' || !(input == "config.autoload.set");
						if (flag47)
						{
							goto IL_0A37;
						}
						bool flag48 = !string.IsNullOrEmpty(key);
						if (flag48)
						{
							int num5 = key.IndexOf('|');
							string text2 = ((num5 >= 0) ? key.Substring(0, num5) : key);
							string text3 = ((num5 >= 0) ? key.Substring(num5 + 1) : "");
							ConfigManager.AutoLoadEnabled = text2 == "1";
							bool flag49 = !string.IsNullOrEmpty(text3);
							if (flag49)
							{
								ConfigManager.AutoLoadConfigName = text3;
							}
							Menu.Enqueue(delegate
							{
								ConfigManager.RefreshList();
							});
						}
					}
					else
					{
						bool flag50 = !(input == "config.autoload.get");
						if (flag50)
						{
							goto IL_0A37;
						}
						try
						{
							extra = ConfigManager.GetStatus();
						}
						catch
						{
							extra = "0|";
						}
					}
					break;
				}
				case 20:
				{
					bool flag51 = !(input == "config.autoload.save");
					if (flag51)
					{
						goto IL_0A37;
					}
					Menu.Enqueue(delegate
					{
						ConfigManager.RefreshList();
					});
					break;
				}
				default:
					goto IL_0A37;
				}
				return "{\"ok\":true,\"data\":" + Menu.JsonEscape(extra) + "}";
				IL_0A37:;
			}
			text4 = "{\"ok\":false,\"err\":\"unknown action\"}";
		}
		return text4;
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x00012354 File Offset: 0x00010554
	private static string BuildConfigListJson()
	{
		List<CommunityConfig> cachedConfigs = CommunityConfigBrowser.CachedConfigs;
		bool flag = cachedConfigs == null;
		string text;
		if (flag)
		{
			text = "[]";
		}
		else
		{
			StringBuilder stringBuilder = new StringBuilder(1024);
			stringBuilder.Append('[');
			for (int i = 0; i < cachedConfigs.Count; i++)
			{
				CommunityConfig gClass = cachedConfigs[i];
				int num = i;
				bool flag2 = num > 0;
				if (flag2)
				{
					stringBuilder.Append(',');
				}
				stringBuilder.Append('{');
				StringBuilder stringBuilder2 = stringBuilder.Append("\"id\":").Append(Menu.JsonEscape(gClass.Id ?? ""));
				stringBuilder2.Append(',');
				stringBuilder.Append("\"name\":").Append(Menu.JsonEscape(gClass.Name ?? "")).Append(',');
				stringBuilder.Append("\"uploader\":").Append(Menu.JsonEscape(gClass.UploaderName ?? "")).Append(',');
				StringBuilder stringBuilder3 = stringBuilder.Append("\"downloads\":").Append(gClass.Downloads);
				stringBuilder3.Append(',');
				StringBuilder stringBuilder4 = stringBuilder.Append("\"likes\":").Append(gClass.Likes);
				stringBuilder4.Append(',');
				stringBuilder.Append("\"liked\":").Append(gClass.Liked ? "true" : "false");
				stringBuilder.Append('}');
			}
			stringBuilder.Append(']');
			text = stringBuilder.ToString();
		}
		return text;
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x000124F4 File Offset: 0x000106F4
	private static string JsonEscape(string input)
	{
		bool flag = input != null;
		string text;
		if (flag)
		{
			int length = input.Length;
			StringBuilder stringBuilder = new StringBuilder(length + 2);
			stringBuilder.Append('"');
			int num2 = 0;
			for (;;)
			{
				bool flag2 = num2 >= input.Length;
				if (flag2)
				{
					break;
				}
				char c = input[num2];
				switch (c)
				{
				case '\b':
					stringBuilder.Append("\\b");
					break;
				case '\t':
					stringBuilder.Append("\\t");
					break;
				case '\n':
					stringBuilder.Append("\\n");
					break;
				case '\v':
				{
					bool flag3 = c < ' ';
					if (flag3)
					{
						StringBuilder stringBuilder2 = stringBuilder;
						StringBuilder stringBuilder3 = stringBuilder2.Append("\\u");
						int num3 = (int)c;
						stringBuilder3.Append(num3.ToString("x4"));
					}
					else
					{
						stringBuilder.Append(c);
					}
					break;
				}
				case '\f':
				{
					StringBuilder stringBuilder4 = stringBuilder;
					stringBuilder4.Append("\\f");
					break;
				}
				case '\r':
					stringBuilder.Append("\\r");
					break;
				default:
				{
					char c2 = c;
					bool flag4 = c2 == '"';
					if (flag4)
					{
						stringBuilder.Append("\\\"");
					}
					else
					{
						char c3 = c;
						bool flag5 = c3 == '\\';
						if (flag5)
						{
							stringBuilder.Append("\\\\");
						}
						bool flag6 = c < ' ';
						if (flag6)
						{
							StringBuilder stringBuilder2 = stringBuilder;
							StringBuilder stringBuilder3 = stringBuilder2.Append("\\u");
							int num3 = (int)c;
							stringBuilder3.Append(num3.ToString("x4"));
						}
						else
						{
							stringBuilder.Append(c);
						}
					}
					break;
				}
				}
				num2++;
			}
			StringBuilder stringBuilder5 = stringBuilder;
			stringBuilder5.Append('"');
			text = stringBuilder.ToString();
		}
		else
		{
			text = "\"\"";
		}
		return text;
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x000126DC File Offset: 0x000108DC
	private static string HandleSet(string input, string key)
	{
		bool flag = string.IsNullOrEmpty(input);
		string text2;
		if (flag)
		{
			text2 = "";
		}
		else
		{
			string text = "\"" + key + "\"";
			int num3 = input.IndexOf(text);
			int num4 = num3;
			bool flag2 = num4 >= 0;
			if (flag2)
			{
				num3 += text.Length;
				for (;;)
				{
					bool flag3 = num3 < input.Length;
					if (!flag3)
					{
						break;
					}
					char c = input[num3];
					bool flag4 = c != ' ';
					if (flag4)
					{
						char c2 = input[num3];
						bool flag5 = c2 == ':';
						if (!flag5)
						{
							break;
						}
						num3++;
					}
					else
					{
						num3++;
					}
				}
				bool flag6 = num3 < input.Length;
				if (flag6)
				{
					char c3 = input[num3];
					bool flag7 = c3 != '"';
					if (flag7)
					{
						text2 = "";
					}
					else
					{
						int num5 = Menu.GetJsonInt(input, num3 + 1);
						int num6 = num5;
						bool flag8 = num6 >= 0;
						if (flag8)
						{
							int num7 = num3;
							int startIndex = num7 + 1;
							int num8 = num5 - num3;
							text2 = Menu.GetJsonString(input.Substring(startIndex, num8 - 1));
						}
						else
						{
							text2 = "";
						}
					}
				}
				else
				{
					text2 = "";
				}
			}
			else
			{
				text2 = "";
			}
		}
		return text2;
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x00012830 File Offset: 0x00010A30
	private static string HandleAction(string input, string key)
	{
		bool flag = !string.IsNullOrEmpty(input);
		string text2;
		if (flag)
		{
			string text = "\"" + key + "\"";
			int num5 = input.IndexOf(text);
			int num6 = num5;
			bool flag2 = num6 >= 0;
			if (flag2)
			{
				num5 += text.Length;
				for (;;)
				{
					bool flag3 = num5 < input.Length;
					if (!flag3)
					{
						break;
					}
					char c3 = input[num5];
					bool flag4 = c3 == ' ';
					if (flag4)
					{
						num5++;
					}
					else
					{
						char c4 = input[num5];
						bool flag5 = c4 != ':';
						if (flag5)
						{
							break;
						}
						num5++;
					}
				}
				bool flag6 = num5 < input.Length;
				if (flag6)
				{
					char c5 = input[num5];
					bool flag7 = c5 == '{';
					if (flag7)
					{
						int num7 = 0;
						int num8 = num5 + 1;
						int num9 = num5;
						for (;;)
						{
							bool flag8 = num9 < input.Length;
							if (!flag8)
							{
								goto IL_01B2;
							}
							char c6 = input[num9];
							char c7 = c6;
							bool flag9 = c7 == '"';
							if (flag9)
							{
								int num10 = num9;
								num9 = Menu.GetJsonInt(input, num10 + 1);
								int num11 = num9;
								bool flag10 = num11 < 0;
								if (flag10)
								{
									break;
								}
								num9++;
							}
							else
							{
								bool flag11 = c6 == '{';
								if (flag11)
								{
									num7++;
									num9++;
								}
								else
								{
									char c8 = c6;
									bool flag12 = c8 == '}';
									if (flag12)
									{
										num7--;
										bool flag13 = num7 != 0;
										if (!flag13)
										{
											goto IL_019C;
										}
										num9++;
									}
									else
									{
										num9++;
									}
								}
							}
						}
						return "";
						IL_019C:
						return input.Substring(num8, num9 - num8);
						IL_01B2:
						text2 = "";
					}
					else
					{
						text2 = "";
					}
				}
				else
				{
					text2 = "";
				}
			}
			else
			{
				text2 = "";
			}
		}
		else
		{
			text2 = "";
		}
		return text2;
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x00012A2C File Offset: 0x00010C2C
	private static int GetJsonInt(string input, int defaultValue)
	{
		int num2 = defaultValue;
		for (;;)
		{
			bool flag = num2 >= input.Length;
			if (flag)
			{
				break;
			}
			char c = input[num2];
			bool flag2 = c != '\\';
			if (flag2)
			{
				char c2 = input[num2];
				bool flag3 = c2 != '"';
				if (!flag3)
				{
					goto IL_0053;
				}
				num2++;
			}
			else
			{
				num2++;
				num2++;
			}
		}
		return -1;
		IL_0053:
		return num2;
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x00012AA4 File Offset: 0x00010CA4
	private static string GetJsonString(string input)
	{
		StringBuilder stringBuilder = new StringBuilder(input.Length);
		int num2 = 0;
		for (;;)
		{
			bool flag = num2 < input.Length;
			if (!flag)
			{
				break;
			}
			char c = input[num2];
			bool flag2 = c != '\\';
			if (flag2)
			{
				stringBuilder.Append(c);
			}
			else
			{
				int num3 = num2;
				bool flag3 = num3 + 1 >= input.Length;
				if (flag3)
				{
					stringBuilder.Append(c);
				}
				else
				{
					char c2 = input[++num2];
					char c3 = c2;
					bool flag4 = c3 > '/';
					if (flag4)
					{
						bool flag5 = c2 == '\\';
						if (flag5)
						{
							stringBuilder.Append('\\');
						}
						else
						{
							bool flag6 = c2 == 'n';
							if (flag6)
							{
								StringBuilder stringBuilder2 = stringBuilder;
								stringBuilder2.Append('\n');
							}
							else
							{
								switch (c2)
								{
								case 'r':
									stringBuilder.Append('\r');
									break;
								case 's':
									stringBuilder.Append(c2);
									break;
								case 't':
									stringBuilder.Append('\t');
									break;
								case 'u':
								{
									bool flag7 = num2 + 4 >= input.Length;
									if (!flag7)
									{
										StringBuilder stringBuilder3 = stringBuilder;
										int startIndex = num2 + 1;
										stringBuilder3.Append((char)Convert.ToInt32(input.Substring(startIndex, 4), 16));
										num2 += 4;
									}
									break;
								}
								default:
									stringBuilder.Append(c2);
									break;
								}
							}
						}
					}
					else
					{
						bool flag8 = c2 == '"';
						if (flag8)
						{
							stringBuilder.Append('"');
						}
						else
						{
							bool flag9 = c2 == '/';
							if (flag9)
							{
								stringBuilder.Append('/');
							}
							else
							{
								stringBuilder.Append(c2);
							}
						}
					}
				}
			}
			num2++;
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x00012C80 File Offset: 0x00010E80
	private static string ReadJsonToken(string input, int defaultValue, out int length)
	{
		bool flag = defaultValue >= input.Length;
		string text;
		if (flag)
		{
			length = defaultValue;
			text = "";
		}
		else
		{
			char c = input[defaultValue];
			char c4 = c;
			char c5 = c4;
			if (c5 != '"')
			{
				if (c5 != '{')
				{
					bool flag2 = c == '[';
					if (!flag2)
					{
						int i;
						for (i = defaultValue; i < input.Length; i++)
						{
							int num = ",}]\n\r\t ".IndexOf(input[i]);
							bool flag3 = num >= 0;
							if (flag3)
							{
								break;
							}
						}
						length = i;
						return input.Substring(defaultValue, i - defaultValue);
					}
				}
				int num2 = ((c == '{') ? 125 : 93);
				char c2 = (char)num2;
				int num3 = 0;
				int num4 = defaultValue;
				for (;;)
				{
					bool flag4 = num4 < input.Length;
					if (!flag4)
					{
						goto IL_018E;
					}
					char c3 = input[num4];
					bool flag5 = c3 != '"';
					if (flag5)
					{
						bool flag6 = c3 == c;
						if (flag6)
						{
							num3++;
						}
						else
						{
							bool flag7 = c3 == c2;
							if (flag7)
							{
								num3--;
								bool flag8 = num3 == 0;
								if (flag8)
								{
									break;
								}
							}
						}
					}
					else
					{
						num4 = Menu.GetJsonInt(input, num4 + 1);
						int num5 = num4;
						bool flag9 = num5 >= 0;
						if (!flag9)
						{
							goto IL_018D;
						}
					}
					num4++;
				}
				int num6 = num4;
				length = num6 + 1;
				return input.Substring(defaultValue, length - defaultValue);
				IL_018D:
				IL_018E:
				length = input.Length;
				text = input.Substring(defaultValue);
			}
			else
			{
				int num7 = Menu.GetJsonInt(input, defaultValue + 1);
				bool flag10 = num7 < 0;
				if (flag10)
				{
					length = input.Length;
					text = input.Substring(defaultValue);
				}
				else
				{
					length = num7 + 1;
					text = input.Substring(defaultValue, length - defaultValue);
				}
			}
		}
		return text;
	}

	// Token: 0x04000207 RID: 519
	public const int PORT = 47811;

	// Token: 0x04000208 RID: 520
	private static TcpListener listener;

	// Token: 0x04000209 RID: 521
	private static Thread serverThread;

	// Token: 0x0400020A RID: 522
	private static bool running;

	// Token: 0x0400020B RID: 523
	private static readonly object object_0 = new object();

	// Token: 0x0400020C RID: 524
	public static string AuthToken = "";

	// Token: 0x0400020D RID: 525
	public static string Hwid = "";

	// Token: 0x0400020E RID: 526
	private static readonly Queue<Action> mainThreadQueue = new Queue<Action>();

	// Token: 0x0400020F RID: 527
	private static string string_0;

	// Token: 0x04000210 RID: 528
	private static readonly object object_1 = new object();

	// Token: 0x04000211 RID: 529
	private static readonly Dictionary<string, Type> getHandlers = new Dictionary<string, Type>
	{
		{
			"Settings",
			typeof(Settings)
		},
		{
			"ConfigManager",
			typeof(ConfigManager)
		}
	};

	// Token: 0x04000212 RID: 530
	private static Dictionary<string, Menu.SettingField> setHandlers;

	// Token: 0x02000064 RID: 100
	private struct SettingField
	{
		// Token: 0x04000502 RID: 1282
		public string Scope;

		// Token: 0x04000503 RID: 1283
		public string Name;

		// Token: 0x04000504 RID: 1284
		public FieldInfo Info;

		// Token: 0x04000505 RID: 1285
		public string Kind;
	}

	// Token: 0x02000065 RID: 101
	[CompilerGenerated]
	[Serializable]
	private sealed class Class22
	{
		// Token: 0x0600023B RID: 571 RVA: 0x000230EC File Offset: 0x000212EC
		internal void method_0()
		{
			ConfigManager.RefreshList();
		}

		// Token: 0x0600023C RID: 572 RVA: 0x000230F5 File Offset: 0x000212F5
		internal void method_1()
		{
			ConfigManager.RefreshList();
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00023100 File Offset: 0x00021300
		internal void method_2()
		{
			try
			{
				Macros.StartRecording();
			}
			catch
			{
			}
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00023130 File Offset: 0x00021330
		internal void method_3()
		{
			try
			{
				Macros.StartReplay();
				Macros.StopReplayAll();
			}
			catch
			{
			}
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00023164 File Offset: 0x00021364
		internal void method_4()
		{
			try
			{
				Macros.RefreshMacroList();
			}
			catch
			{
			}
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00023194 File Offset: 0x00021394
		internal void method_5()
		{
			try
			{
				Macros.UploadMacro();
			}
			catch
			{
			}
		}

		// Token: 0x06000241 RID: 577 RVA: 0x000231C4 File Offset: 0x000213C4
		internal void method_6()
		{
			try
			{
				Skybox.Rebuild();
			}
			catch
			{
			}
		}

		// Token: 0x06000242 RID: 578 RVA: 0x000231F4 File Offset: 0x000213F4
		internal void method_7()
		{
			try
			{
				Skybox.Disable();
			}
			catch
			{
			}
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00023224 File Offset: 0x00021424
		internal void method_8()
		{
			try
			{
				Weather.ApplyWeather();
			}
			catch
			{
			}
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00023254 File Offset: 0x00021454
		internal void method_9()
		{
			try
			{
				TagFreeze.TriggerPanic();
			}
			catch
			{
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00023284 File Offset: 0x00021484
		internal void method_10()
		{
			try
			{
				RoomJoiner.LeaveRoom();
			}
			catch
			{
			}
		}

		// Token: 0x04000506 RID: 1286
		public static readonly Menu.Class22 _003C_003E9 = new Menu.Class22();

		// Token: 0x04000507 RID: 1287
		public static Action _003C_003E9__30_3;

		// Token: 0x04000508 RID: 1288
		public static Action _003C_003E9__30_17;

		// Token: 0x04000509 RID: 1289
		public static Action _003C_003E9__30_6;

		// Token: 0x0400050A RID: 1290
		public static Action _003C_003E9__30_7;

		// Token: 0x0400050B RID: 1291
		public static Action _003C_003E9__30_8;

		// Token: 0x0400050C RID: 1292
		public static Action _003C_003E9__30_9;

		// Token: 0x0400050D RID: 1293
		public static Action _003C_003E9__30_10;

		// Token: 0x0400050E RID: 1294
		public static Action _003C_003E9__30_11;

		// Token: 0x0400050F RID: 1295
		public static Action _003C_003E9__30_12;

		// Token: 0x04000510 RID: 1296
		public static Action _003C_003E9__30_14;

		// Token: 0x04000511 RID: 1297
		public static Action _003C_003E9__30_16;
	}

	// Token: 0x02000066 RID: 102
	[CompilerGenerated]
	private sealed class Class23
	{
		// Token: 0x06000248 RID: 584 RVA: 0x000232C9 File Offset: 0x000214C9
		internal void method_0()
		{
			Menu.HandleClient(this.client);
		}

		// Token: 0x04000512 RID: 1298
		public TcpClient client;
	}

	// Token: 0x02000067 RID: 103
	[CompilerGenerated]
	private sealed class Class24
	{
		// Token: 0x0600024A RID: 586 RVA: 0x000232E1 File Offset: 0x000214E1
		internal void method_0()
		{
			ConfigManager.SaveConfig(string.IsNullOrEmpty(this.arg) ? null : this.arg);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00023300 File Offset: 0x00021500
		internal void method_1()
		{
			ConfigManager.LoadConfig(string.IsNullOrEmpty(this.arg) ? null : this.arg);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0002331F File Offset: 0x0002151F
		internal void method_2()
		{
			ConfigManager.DeleteConfig(this.arg);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00023330 File Offset: 0x00021530
		internal void method_3()
		{
			try
			{
				CommunityConfigBrowser.Delete(this.arg);
			}
			catch
			{
			}
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00023364 File Offset: 0x00021564
		internal void method_4()
		{
			try
			{
				CommunityConfigBrowser.Like(this.arg);
			}
			catch
			{
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00023398 File Offset: 0x00021598
		internal void method_5()
		{
			try
			{
				RoomJoiner.RoomCode = this.arg ?? "";
				RoomJoiner.JoinRoom();
			}
			catch
			{
			}
		}

		// Token: 0x04000513 RID: 1299
		public string arg;
	}

	// Token: 0x02000068 RID: 104
	[CompilerGenerated]
	private sealed class Class25
	{
		// Token: 0x06000251 RID: 593 RVA: 0x000233E8 File Offset: 0x000215E8
		internal void method_0()
		{
			try
			{
				Settings.CurrentTimeOfDay = this.t;
			}
			catch
			{
			}
		}

		// Token: 0x04000514 RID: 1300
		public int t;
	}

	// Token: 0x02000069 RID: 105
	[CompilerGenerated]
	private sealed class Class26
	{
		// Token: 0x06000253 RID: 595 RVA: 0x00023428 File Offset: 0x00021628
		internal void method_0()
		{
			try
			{
				CommunityConfigBrowser.Upload(this.id, this.nm);
			}
			catch
			{
			}
		}

		// Token: 0x04000515 RID: 1301
		public string id;

		// Token: 0x04000516 RID: 1302
		public string nm;
	}

	// Token: 0x0200006A RID: 106
	[CompilerGenerated]
	private static class <>O
	{
		// Token: 0x04000517 RID: 1303
		public static ThreadStart <0>__AcceptLoop;
	}
}
