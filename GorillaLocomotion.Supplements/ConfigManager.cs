using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using UnityEngine;

// Token: 0x0200000E RID: 14
public static class ConfigManager
{
	// Token: 0x06000020 RID: 32 RVA: 0x000033C8 File Offset: 0x000015C8
	static ConfigManager()
	{
		byte[] array = new byte[]
		{
			206, 166, 102, 212, 61, 127, 94, 159, 236, 193,
			31, 29, 232, 137, 77, 186, 6, 30, 124, 9,
			123, 224, 139, 113, 111, 239, 68, 108, 112, 98,
			50, 240
		};
		ConfigManager.keySalt = array;
		ConfigManager.keyByte = byte.MaxValue;
		ConfigManager.LastSaveTime = "Never";
		ConfigManager.LastLoadTime = "Never";
		ConfigManager.CurrentConfigName = "default";
		ConfigManager.AutoLoadEnabled = false;
		ConfigManager.AutoLoadConfigName = "default";
		ConfigManager.configNames = new List<string>();
		ConfigManager.SelectedConfigIndex = 0;
		ConfigManager.IsOperationPending = false;
		ConfigManager.OperationStatus = "";
		ConfigManager.MaxConfigs = 5;
		try
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
		}
		catch
		{
		}
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00003474 File Offset: 0x00001674
	private static byte[] DeriveKey()
	{
		byte[] array = new byte[ConfigManager.keySalt.Length];
		int num2 = 0;
		for (;;)
		{
			bool flag = num2 >= ConfigManager.keySalt.Length;
			if (flag)
			{
				break;
			}
			array[num2] = ConfigManager.keySalt[num2] ^ ConfigManager.keyByte;
			num2++;
		}
		return array;
	}

	// Token: 0x06000022 RID: 34 RVA: 0x000034D0 File Offset: 0x000016D0
	private static string GetHwid()
	{
		bool flag = !string.IsNullOrEmpty(Menu.Hwid);
		string text2;
		if (flag)
		{
			text2 = Menu.Hwid;
		}
		else
		{
			string text = "";
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Cryptography"))
				{
					object obj = ((registryKey != null) ? registryKey.GetValue("MachineGuid") : null);
					bool flag2 = obj != null;
					if (flag2)
					{
						text = obj.ToString();
					}
				}
			}
			catch
			{
			}
			bool flag3 = string.IsNullOrEmpty(text);
			if (flag3)
			{
				string machineName = Environment.MachineName;
				text = machineName + "-" + Environment.UserName;
			}
			using (SHA256 sHA = SHA256.Create())
			{
				text2 = Convert.ToBase64String(sHA.ComputeHash(Encoding.UTF8.GetBytes("absense-hwid-v1:" + text)));
			}
		}
		return text2;
	}

	// Token: 0x06000023 RID: 35 RVA: 0x000035D8 File Offset: 0x000017D8
	private static byte[] Encrypt(byte[] data, byte[] keyBytes, byte[] nonce)
	{
		byte[] array;
		using (Aes aes = Aes.Create())
		{
			aes.Key = keyBytes;
			aes.IV = nonce;
			aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.PKCS7;
			using (ICryptoTransform transform = aes.CreateEncryptor())
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
					{
						cryptoStream.Write(data, 0, data.Length);
						cryptoStream.FlushFinalBlock();
						array = memoryStream.ToArray();
					}
				}
			}
		}
		return array;
	}

	// Token: 0x06000024 RID: 36 RVA: 0x000036A4 File Offset: 0x000018A4
	private static byte[] Decrypt(byte[] data, byte[] keyBytes, byte[] nonce)
	{
		byte[] array;
		try
		{
			using (Aes aes = Aes.Create())
			{
				aes.Key = keyBytes;
				aes.IV = nonce;
				aes.Mode = CipherMode.CBC;
				aes.Padding = PaddingMode.PKCS7;
				using (ICryptoTransform transform = aes.CreateDecryptor())
				{
					using (MemoryStream stream = new MemoryStream(data))
					{
						using (CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Read))
						{
							using (MemoryStream memoryStream = new MemoryStream())
							{
								cryptoStream.CopyTo(memoryStream);
								array = memoryStream.ToArray();
							}
						}
					}
				}
			}
		}
		catch
		{
			array = null;
		}
		return array;
	}

	// Token: 0x06000025 RID: 37 RVA: 0x0000379C File Offset: 0x0000199C
	internal static string PostApi(string input, string value)
	{
		string text;
		try
		{
			bool flag = string.IsNullOrEmpty(Menu.AuthToken);
			if (flag)
			{
				text = "";
			}
			else
			{
				string s = string.Concat(new string[]
				{
					"{\"t\":\"",
					ConfigManager.JsonEscape(Menu.AuthToken),
					"\",\"hwid\":\"",
					ConfigManager.JsonEscape(ConfigManager.GetHwid()),
					"\"",
					value.EndsWith("}") ? value : (value + "}")
				});
				byte[] array = new byte[16];
				using (RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider())
				{
					rNGCryptoServiceProvider.GetBytes(array);
				}
				byte[] array2 = ConfigManager.DeriveKey();
				byte[] array3 = ConfigManager.Encrypt(Encoding.UTF8.GetBytes(s), array2, array);
				byte[] array4 = new byte[16 + array3.Length];
				Buffer.BlockCopy(array, 0, array4, 0, 16);
				Buffer.BlockCopy(array3, 0, array4, 16, array3.Length);
				string s2 = "{\"d\":\"" + Convert.ToBase64String(array4) + "\"}";
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://absentauth.com" + input);
				httpWebRequest.Method = "POST";
				httpWebRequest.Timeout = 10000;
				httpWebRequest.ContentType = "application/json";
				byte[] bytes = Encoding.UTF8.GetBytes(s2);
				httpWebRequest.ContentLength = (long)bytes.Length;
				using (Stream stream = httpWebRequest.GetRequestStream())
				{
					stream.Write(bytes, 0, bytes.Length);
				}
				string string_3;
				try
				{
					using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
					{
						using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8))
						{
							string_3 = streamReader.ReadToEnd();
						}
					}
				}
				catch (WebException ex) when (ex.Response != null)
				{
					using (StreamReader streamReader2 = new StreamReader(ex.Response.GetResponseStream(), Encoding.UTF8))
					{
						string_3 = streamReader2.ReadToEnd();
					}
				}
				text = ConfigManager.PostApiMultipart(string_3, array2);
			}
		}
		catch
		{
			text = "";
		}
		return text;
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00003A94 File Offset: 0x00001C94
	private static string PostApiMultipart(string input, byte[] data)
	{
		bool flag = string.IsNullOrEmpty(input);
		string text2;
		if (flag)
		{
			text2 = "";
		}
		else
		{
			string text = ConfigManager.GetJsonString(input, "d");
			bool flag2 = string.IsNullOrEmpty(text);
			if (flag2)
			{
				text2 = input;
			}
			else
			{
				try
				{
					byte[] array = Convert.FromBase64String(text);
					bool flag3 = array.Length < 17;
					if (flag3)
					{
						text2 = input;
					}
					else
					{
						byte[] array2 = new byte[16];
						byte[] array3 = new byte[array.Length - 16];
						Buffer.BlockCopy(array, 0, array2, 0, 16);
						Buffer.BlockCopy(array, 16, array3, 0, array3.Length);
						byte[] array4 = ConfigManager.Decrypt(array3, data, array2);
						text2 = ((array4 == null) ? input : Encoding.UTF8.GetString(array4));
					}
				}
				catch
				{
					text2 = input;
				}
			}
		}
		return text2;
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00003B60 File Offset: 0x00001D60
	internal static string JsonEscape(string input)
	{
		bool flag = string.IsNullOrEmpty(input);
		string text;
		if (flag)
		{
			text = "";
		}
		else
		{
			StringBuilder stringBuilder = new StringBuilder();
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
				case '\t':
					stringBuilder.Append("\\t");
					break;
				case '\n':
					stringBuilder.Append("\\n");
					break;
				case '\v':
					stringBuilder.Append(c);
					break;
				case '\f':
					stringBuilder.Append(c);
					break;
				case '\r':
					stringBuilder.Append("\\r");
					break;
				default:
				{
					char c2 = c;
					bool flag3 = c2 == '"';
					if (flag3)
					{
						stringBuilder.Append("\\\"");
					}
					else
					{
						bool flag4 = c == '\\';
						if (flag4)
						{
							stringBuilder.Append("\\\\");
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
			text = stringBuilder.ToString();
		}
		return text;
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00003C88 File Offset: 0x00001E88
	internal static string GetJsonString(string input, string value)
	{
		bool flag = !string.IsNullOrEmpty(input);
		string text2;
		if (flag)
		{
			string text = "\"" + value + "\":";
			int num3 = input.IndexOf(text);
			bool flag2 = num3 >= 0;
			if (flag2)
			{
				num3 += text.Length;
				for (;;)
				{
					bool flag3 = num3 >= input.Length;
					if (flag3)
					{
						break;
					}
					bool flag4 = input[num3] == ' ';
					if (!flag4)
					{
						break;
					}
					num3++;
				}
				bool flag5 = num3 < input.Length;
				if (flag5)
				{
					bool flag6 = input[num3] != '"';
					if (flag6)
					{
						int num4 = num3;
						for (;;)
						{
							bool flag7 = num4 >= input.Length;
							if (flag7)
							{
								break;
							}
							char c4 = input[num4];
							bool flag8 = c4 == ',';
							if (flag8)
							{
								break;
							}
							bool flag9 = input[num4] == '}';
							if (flag9)
							{
								break;
							}
							bool flag10 = input[num4] != ']';
							if (!flag10)
							{
								break;
							}
							num4++;
						}
						text2 = input.Substring(num3, num4 - num3);
					}
					else
					{
						num3++;
						StringBuilder stringBuilder = new StringBuilder();
						int num5 = num3;
						for (;;)
						{
							bool flag11 = num5 < input.Length;
							if (!flag11)
							{
								goto IL_02F3;
							}
							char c5 = input[num5];
							bool flag12 = c5 != '\\';
							if (flag12)
							{
								bool flag13 = c5 != '"';
								if (!flag13)
								{
									break;
								}
								stringBuilder.Append(c5);
								num5++;
							}
							else
							{
								bool flag14 = num5 + 1 >= input.Length;
								if (flag14)
								{
									bool flag15 = c5 != '"';
									if (!flag15)
									{
										goto IL_01E2;
									}
									stringBuilder.Append(c5);
									num5++;
								}
								else
								{
									char c6 = input[num5 + 1];
									bool flag16 = c6 != 'n';
									if (flag16)
									{
										bool flag17 = c6 != 'r';
										if (flag17)
										{
											bool flag18 = c6 != 't';
											if (flag18)
											{
												char c7 = c6;
												bool flag19 = c7 != '\\';
												if (flag19)
												{
													bool flag20 = c6 != '"';
													if (flag20)
													{
														bool flag21 = c6 != '/';
														if (flag21)
														{
															stringBuilder.Append(c5);
														}
														else
														{
															stringBuilder.Append('/');
															num5++;
														}
													}
													else
													{
														stringBuilder.Append('"');
														num5++;
													}
												}
												else
												{
													stringBuilder.Append('\\');
													num5++;
												}
											}
											else
											{
												stringBuilder.Append('\t');
												num5++;
											}
										}
										else
										{
											stringBuilder.Append('\r');
											num5++;
										}
									}
									else
									{
										StringBuilder stringBuilder2 = stringBuilder;
										stringBuilder2.Append('\n');
										num5++;
									}
									num5++;
								}
							}
						}
						return stringBuilder.ToString();
						IL_01E2:
						return stringBuilder.ToString();
						IL_02F3:
						text2 = stringBuilder.ToString();
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

	// Token: 0x06000029 RID: 41 RVA: 0x00003FBC File Offset: 0x000021BC
	internal static int GetJsonInt(string input, string value)
	{
		int result = 0;
		bool flag = int.TryParse(ConfigManager.GetJsonString(input, value), out result);
		int num;
		if (flag)
		{
			num = result;
		}
		else
		{
			num = 0;
		}
		return num;
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00003FEC File Offset: 0x000021EC
	private static bool JsonHasField(string input, string value)
	{
		string text = ConfigManager.GetJsonString(input, value);
		return text == "true";
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00004014 File Offset: 0x00002214
	public static List<string> ListConfigNames()
	{
		int count = ConfigManager.configNames.Count;
		bool flag = count > 0;
		List<string> list;
		if (flag)
		{
			list = ConfigManager.configNames;
		}
		else
		{
			list = new List<string> { "default" };
		}
		return list;
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00004056 File Offset: 0x00002256
	public static void EnsureConfigDir()
	{
		ThreadPool.QueueUserWorkItem(delegate
		{
			ConfigManager.SaveCurrentConfig();
		});
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00004080 File Offset: 0x00002280
	public static void SaveCurrentConfig()
	{
		try
		{
			string text = ConfigManager.PostApi("/api/configs/list", "");
			ConfigManager.configNames.Clear();
			bool flag = ConfigManager.GetJsonInt(text, "r") == 1;
			if (flag)
			{
				int num = ConfigManager.GetJsonInt(text, "max");
				bool flag2 = num > 0;
				if (flag2)
				{
					ConfigManager.MaxConfigs = num;
				}
				string text2 = text;
				int num2 = text.IndexOf("\"configs\"");
				bool flag3 = num2 >= 0;
				if (flag3)
				{
					num2 = text.IndexOf('[', num2);
					int num3 = text.LastIndexOf(']');
					bool flag4 = num2 >= 0 && num3 > num2;
					if (flag4)
					{
						text2 = text.Substring(num2, num3 - num2 + 1);
					}
				}
				else
				{
					text2 = "";
				}
				bool flag5 = text2.Length > 2;
				if (flag5)
				{
					string text3 = "";
					int startIndex = 0;
					for (;;)
					{
						int num4 = text2.IndexOf('{', startIndex);
						bool flag6 = num4 < 0;
						if (flag6)
						{
							break;
						}
						int num5 = text2.IndexOf('}', num4);
						bool flag7 = num5 < 0;
						if (flag7)
						{
							break;
						}
						string string_ = text2.Substring(num4, num5 - num4 + 1);
						startIndex = num5 + 1;
						string text4 = ConfigManager.GetJsonString(string_, "name");
						bool flag8 = !string.IsNullOrEmpty(text4);
						if (flag8)
						{
							ConfigManager.configNames.Add(text4);
							bool flag9 = ConfigManager.JsonHasField(string_, "autoLoad");
							if (flag9)
							{
								text3 = text4;
							}
						}
					}
					bool flag10 = !string.IsNullOrEmpty(text3);
					if (flag10)
					{
						ConfigManager.AutoLoadEnabled = true;
						ConfigManager.AutoLoadConfigName = text3;
					}
					else
					{
						ConfigManager.AutoLoadEnabled = false;
					}
				}
			}
			bool flag11 = ConfigManager.configNames.Count == 0;
			if (flag11)
			{
				ConfigManager.configNames.Add("default");
			}
			ConfigManager.OperationStatus = "Loaded " + ConfigManager.configNames.Count.ToString() + " configs";
		}
		catch
		{
			ConfigManager.OperationStatus = "Failed";
		}
	}

	// Token: 0x0600002E RID: 46 RVA: 0x000042A4 File Offset: 0x000024A4
	public static string ReadSelectedConfig()
	{
		bool flag = ConfigManager.configNames.Count == 0;
		string text;
		if (flag)
		{
			text = "default";
		}
		else
		{
			int selectedConfigIndex = ConfigManager.SelectedConfigIndex;
			bool flag2 = selectedConfigIndex >= 0;
			if (flag2)
			{
				bool flag3 = ConfigManager.SelectedConfigIndex < ConfigManager.configNames.Count;
				if (flag3)
				{
					text = ConfigManager.configNames[ConfigManager.SelectedConfigIndex];
				}
				else
				{
					text = "default";
				}
			}
			else
			{
				text = "default";
			}
		}
		return text;
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00004320 File Offset: 0x00002520
	public static void ApplySelectedConfig()
	{
		bool flag = ConfigManager.configNames.Count != 0;
		if (flag)
		{
			int selectedConfigIndex = ConfigManager.SelectedConfigIndex;
			ConfigManager.SelectedConfigIndex = (selectedConfigIndex + 1) % ConfigManager.configNames.Count;
		}
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00004360 File Offset: 0x00002560
	public static int CountConfigs()
	{
		int count = ConfigManager.configNames.Count;
		bool flag = count > 0;
		int num;
		if (flag)
		{
			num = ConfigManager.configNames.Count;
		}
		else
		{
			num = 0;
		}
		return num;
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00004398 File Offset: 0x00002598
	public static void SaveConfig(string input = null)
	{
		ConfigManager.Class18 @class = new ConfigManager.Class18();
		@class.configName = input;
		bool flag = !string.IsNullOrEmpty(@class.configName);
		if (flag)
		{
		}
		ThreadPool.QueueUserWorkItem(new WaitCallback(@class.method_0));
	}

	// Token: 0x06000032 RID: 50 RVA: 0x000043E0 File Offset: 0x000025E0
	public static void LoadConfig(string input = null)
	{
		ConfigManager.Class19 @class = new ConfigManager.Class19();
		@class.configName = input;
		bool flag = string.IsNullOrEmpty(@class.configName);
		if (flag)
		{
		}
		ThreadPool.QueueUserWorkItem(new WaitCallback(@class.method_0));
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00004424 File Offset: 0x00002624
	public static void UploadConfig(string input, string value)
	{
		try
		{
			ConfigManager.ApplyConfigJson(input);
			ConfigManager.CurrentConfigName = value;
			ConfigManager.LastLoadTime = DateTime.Now.ToString("HH:mm:ss");
			ConfigManager.OperationStatus = "Loaded!";
		}
		catch (Exception)
		{
			ConfigManager.OperationStatus = "Load failed";
		}
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00004484 File Offset: 0x00002684
	public static void DeleteConfig(string input)
	{
		ConfigManager.IsOperationPending = true;
		ConfigManager.OperationStatus = "Deleting...";
		ThreadPool.QueueUserWorkItem(delegate
		{
			try
			{
				bool flag = ConfigManager.GetJsonInt(ConfigManager.PostApi("/api/configs/delete", ",\"name\":\"" + ConfigManager.JsonEscape(input) + "\"}"), "r") == 1;
				if (flag)
				{
					ConfigManager.configNames.Remove(input);
					ConfigManager.OperationStatus = "Deleted!";
				}
				else
				{
					ConfigManager.OperationStatus = "Delete failed";
				}
			}
			catch
			{
				ConfigManager.OperationStatus = "Delete failed";
			}
			finally
			{
				ConfigManager.IsOperationPending = false;
			}
		});
	}

	// Token: 0x06000035 RID: 53 RVA: 0x000044C4 File Offset: 0x000026C4
	public static void RenameConfig(string input, string value)
	{
		ConfigManager.IsOperationPending = true;
		ConfigManager.OperationStatus = "Renaming...";
		ThreadPool.QueueUserWorkItem(delegate
		{
			try
			{
				bool flag = ConfigManager.GetJsonInt(ConfigManager.PostApi("/api/configs/rename", string.Concat(new string[]
				{
					",\"oldName\":\"",
					ConfigManager.JsonEscape(input),
					"\",\"newName\":\"",
					ConfigManager.JsonEscape(value),
					"\"}"
				})), "r") == 1;
				if (flag)
				{
					int num = ConfigManager.configNames.IndexOf(input);
					bool flag2 = num >= 0;
					if (flag2)
					{
						ConfigManager.configNames[num] = value;
					}
					bool flag3 = ConfigManager.CurrentConfigName == input;
					if (flag3)
					{
						ConfigManager.CurrentConfigName = value;
					}
					bool flag4 = ConfigManager.AutoLoadConfigName == input;
					if (flag4)
					{
						ConfigManager.AutoLoadConfigName = value;
					}
					ConfigManager.OperationStatus = "Renamed!";
				}
				else
				{
					ConfigManager.OperationStatus = "Rename failed";
				}
			}
			catch
			{
				ConfigManager.OperationStatus = "Rename failed";
			}
			finally
			{
				ConfigManager.IsOperationPending = false;
			}
		});
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00004508 File Offset: 0x00002708
	public static void RefreshList()
	{
		ThreadPool.QueueUserWorkItem(delegate
		{
			try
			{
				ConfigManager.OperationStatus = ((ConfigManager.GetJsonInt(ConfigManager.PostApi("/api/configs/autoload", string.Concat(new string[]
				{
					",\"enabled\":",
					ConfigManager.AutoLoadEnabled ? "true" : "false",
					",\"name\":\"",
					ConfigManager.JsonEscape(ConfigManager.AutoLoadConfigName),
					"\"}"
				})), "r") == 1) ? "Saved!" : "Failed");
			}
			catch
			{
				ConfigManager.OperationStatus = "Failed";
			}
		});
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00004530 File Offset: 0x00002730
	public static string GetStatus()
	{
		string text2;
		try
		{
			string string_ = ConfigManager.PostApi("/api/configs/autoload-data", "}");
			bool flag = ConfigManager.GetJsonInt(string_, "r") != 1;
			if (flag)
			{
				text2 = "0|";
			}
			else
			{
				bool flag2 = !ConfigManager.JsonHasField(string_, "found");
				if (flag2)
				{
					ConfigManager.AutoLoadEnabled = false;
					text2 = "0|";
				}
				else
				{
					string text = ConfigManager.GetJsonString(string_, "name");
					bool flag3 = string.IsNullOrEmpty(text);
					if (flag3)
					{
						text2 = "0|";
					}
					else
					{
						ConfigManager.AutoLoadEnabled = true;
						ConfigManager.AutoLoadConfigName = text;
						text2 = "1|" + text;
					}
				}
			}
		}
		catch
		{
			text2 = "0|";
		}
		return text2;
	}

	// Token: 0x06000038 RID: 56 RVA: 0x000045E8 File Offset: 0x000027E8
	public static void AutoLoad()
	{
		bool flag = ConfigManager.autoLoadStarted || string.IsNullOrEmpty(Menu.AuthToken);
		if (!flag)
		{
			ConfigManager.autoLoadStarted = true;
			ThreadPool.QueueUserWorkItem(delegate
			{
				try
				{
					string string_ = ConfigManager.PostApi("/api/configs/autoload-data", "}");
					bool flag2 = ConfigManager.GetJsonInt(string_, "r") == 1 && ConfigManager.JsonHasField(string_, "found");
					if (flag2)
					{
						string text = ConfigManager.GetJsonString(string_, "name");
						string text2 = ConfigManager.GetJsonString(string_, "data");
						bool flag3 = !string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2);
						if (flag3)
						{
							ConfigManager.AutoLoadEnabled = true;
							ConfigManager.AutoLoadConfigName = text;
							ConfigManager.ApplyConfigJson(text2);
							ConfigManager.CurrentConfigName = text;
							ConfigManager.LastLoadTime = DateTime.Now.ToString("HH:mm:ss");
							ConfigManager.OperationStatus = "Auto-loaded " + text;
						}
					}
				}
				catch
				{
				}
			});
		}
	}

	// Token: 0x06000039 RID: 57 RVA: 0x0000463C File Offset: 0x0000283C
	public static string ExportCurrentConfig()
	{
		return ConfigManager.BuildConfigJson();
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00004654 File Offset: 0x00002854
	private static string BuildConfigJson()
	{
		ConfigData gClass = new ConfigData
		{
			PullModEnabled = Settings.PullModEnabled,
			PullStrength = Settings.PullStrength,
			PullThreshold = Settings.PullThreshold,
			PullTpTime = Settings.PullTpTime,
			PullActivation = (int)Settings.PullActivation,
			TestPullModEnabled = Settings.TestPullModEnabled,
			TestPullStrength = Settings.TestPullStrength,
			TestPullThreshold = Settings.TestPullThreshold,
			TestPullTpTime = Settings.TestPullTpTime,
			TestPullActivation = (int)Settings.TestPullActivation,
			TestPullModToggleMode = Settings.TestPullModToggleMode,
			TestPullSmoothing = Settings.TestPullSmoothing,
			TestPullRaycastEnabled = Settings.TestPullRaycastEnabled,
			WallWalkEnabled = Settings.WallWalkEnabled,
			WallWalkDistance = Settings.WallWalkDistance,
			WallWalkPower = Settings.WallWalkPower,
			WallWalkLeftHand = Settings.WallWalkLeftHand,
			WallWalkRightHand = Settings.WallWalkRightHand,
			WallWalkKeybind = (int)Settings.WallWalkKeybind,
			WallWalkPerHandBinds = Settings.WallWalkPerHandBinds,
			WallWalkLeftKeybind = (int)Settings.WallWalkLeftKeybind,
			WallWalkRightKeybind = (int)Settings.WallWalkRightKeybind,
			CGTWallWalkEnabled = Settings.CGTWallWalkEnabled,
			CGTWallWalkDistance = Settings.CGTWallWalkDistance,
			CGTWallWalkPower = Settings.CGTWallWalkPower,
			CGTWallWalkReach = Settings.CGTWallWalkReach,
			CGTWallWalkKeybind = (int)Settings.CGTWallWalkKeybind,
			DCFlickEnabled = Settings.DCFlickEnabled,
			DcFlickAutoEnabled = Settings.DcFlickAutoEnabled,
			DcFlickAutoHoldTime = Settings.DcFlickAutoHoldTime,
			DcFlickAutoLOS = Settings.DcFlickAutoLOS,
			DcFlickAutoMaxDistance = Settings.DcFlickAutoMaxDistance,
			DcFlickAutoFov = Settings.DcFlickAutoFov,
			DcFlickAutoKeybind = (int)Settings.DcFlickAutoKeybind,
			TagAuraEnabled = Settings.TagAuraEnabled,
			TagAuraDistance = Settings.TagAuraDistance,
			TagAuraKeybind = (int)Settings.TagAuraKeybind,
			TagAuraFovEnabled = Settings.TagAuraFovEnabled,
			TagAuraFov = Settings.TagAuraFov,
			HitboxExpanderEnabled = Settings.HitboxExpanderEnabled,
			HitboxExpanderSize = Settings.HitboxExpanderSize,
			HitboxExpanderOpacity = Settings.HitboxExpanderOpacity,
			PSAEnabled = Settings.PSAEnabled,
			PSASpeed = Settings.PSASpeed,
			PSAKeybind = (int)Settings.PSAKeybind,
			VeloPSAEnabled = Settings.VeloPSAEnabled,
			VeloPSASpeed = Settings.VeloPSASpeed,
			VeloPSAKeybind = (int)Settings.VeloPSAKeybind,
			VeloPSAMinSpeed = Settings.VeloPSAMinSpeed,
			VeloPSAStickyDirection = Settings.VeloPSAStickyDirection,
			VeloPSAAirTurnAssist = Settings.VeloPSAAirTurnAssist,
			VeloPSAAirTurnBlend = Settings.VeloPSAAirTurnBlend,
			BurstPSAEnabled = Settings.BurstPSAEnabled,
			BurstPSASpeed = Settings.BurstPSASpeed,
			BurstPSAKeybind = (int)Settings.BurstPSAKeybind,
			BurstPSAMinSpeed = Settings.BurstPSAMinSpeed,
			BurstPSAStickyDirection = Settings.BurstPSAStickyDirection,
			BurstPSAAirTurnAssist = Settings.BurstPSAAirTurnAssist,
			BurstPSAAirTurnBlend = Settings.BurstPSAAirTurnBlend,
			BurstPSADuration = Settings.BurstPSADuration,
			HighJumpEnabled = Settings.HighJumpEnabled,
			HighJumpSpeed = Settings.HighJumpSpeed,
			HighJumpKeybind = (int)Settings.HighJumpKeybind,
			Cr1ptsPSAEnabled = Settings.Cr1ptsPSAEnabled,
			Cr1ptsPSAStrength = Settings.Cr1ptsPSAStrength,
			Cr1ptsPSALerpSpeed = Settings.Cr1ptsPSALerpSpeed,
			Cr1ptsPSAKeybind = (int)Settings.Cr1ptsPSAKeybind,
			VelmaxEnabled = Settings.VelmaxEnabled,
			VelmaxMaxJumpSpeed = Settings.VelmaxMaxJumpSpeed,
			VelmaxJumpMultiplier = Settings.VelmaxJumpMultiplier,
			ForceTagFreezeEnabled = Settings.ForceTagFreezeEnabled,
			ForceTagFreezeKeybind = (int)Settings.ForceTagFreezeKeybind,
			PanicEnabled = Settings.PanicEnabled,
			PanicKeybind = (int)Settings.PanicKeybind,
			LongArmsEnabled = Settings.LongArmsEnabled,
			LongArmsAmount = Settings.LongArmsAmount,
			LongArmsBypassEnabled = Settings.LongArmsBypassEnabled,
			LongArmsBypassKeybind = (int)Settings.LongArmsBypassKeybind,
			LongArmsBypassLeftHand = Settings.LongArmsBypassLeftHand,
			LongArmsBypassRightHand = Settings.LongArmsBypassRightHand,
			LongArmsBypassSmoothEnabled = Settings.LongArmsBypassSmoothEnabled,
			LongArmsBypassSmoothness = Settings.LongArmsBypassSmoothness,
			LongArmsBypassYOffsetEnabled = Settings.LongArmsBypassYOffsetEnabled,
			LongArmsBypassYOffset = Settings.LongArmsBypassYOffset,
			NoFingerMovementEnabled = Settings.NoFingerMovementEnabled,
			FakeQuestMenuEnabled = Settings.FakeQuestMenuEnabled,
			FakeQuestMenuKeybind = (int)Settings.FakeQuestMenuKeybind,
			FakeReportMenuEnabled = Settings.FakeReportMenuEnabled,
			FakeReportMenuKeybind = (int)Settings.FakeReportMenuKeybind,
			FakePowerOffEnabled = Settings.FakePowerOffEnabled,
			FakePowerOffKeybind = (int)Settings.FakePowerOffKeybind,
			GrayScreenEnabled = Settings.GrayScreenEnabled,
			GrayScreenKeybind = (int)Settings.GrayScreenKeybind,
			GrayScreenDirection = (int)Settings.GrayScreenDirection,
			GrayScreenDuration = Settings.GrayScreenDuration,
			GrayScreenArmDistance = Settings.GrayScreenArmDistance,
			GrayScreenArmSpread = Settings.GrayScreenArmSpread,
			HzSliderEnabled = Settings.HzSliderEnabled,
			TargetHz = Settings.TargetHz,
			NoSlipEnabled = Settings.NoSlipEnabled,
			SlipSlapEnabled = Settings.SlipSlapEnabled,
			SlipSlapMultiplier = Settings.SlipSlapMultiplier,
			SurfaceSlipEnabled = Settings.SurfaceSlipEnabled,
			SurfaceSlipWalls = Settings.SurfaceSlipWalls,
			SurfaceSlipLowerSlippery = Settings.SurfaceSlipLowerSlippery,
			SurfaceSlipUpperSlippery = Settings.SurfaceSlipUpperSlippery,
			RemoveWindBarrierEnabled = Settings.RemoveWindBarrierEnabled,
			NoArmCapEnabled = Settings.NoArmCapEnabled,
			ArmCapEnabled = Settings.ArmCapEnabled,
			ArmCapValue = Settings.ArmCapValue,
			PredsEnabled = Settings.PredsEnabled,
			PredsAmount = Settings.PredsAmount,
			PredsAlwaysOn = Settings.PredsAlwaysOn,
			PredsAlwaysAmount = Settings.PredsAlwaysAmount,
			PredsHand = (int)Settings.PredsHand,
			PredsLeftHandEnabled = Settings.PredsLeftHandEnabled,
			PredsRightHandEnabled = Settings.PredsRightHandEnabled,
			AntiPredEnabled = Settings.AntiPredEnabled,
			VisualiseServerEnabled = Settings.VisualiseServerEnabled,
			VisualiseClientEnabled = Settings.VisualiseClientEnabled,
			DesyncEnabled = Settings.DesyncEnabled,
			DesyncMode = (int)Settings.DesyncMode,
			DesyncDelayMs = Settings.DesyncDelayMs,
			DesyncVisualise = Settings.DesyncVisualise,
			DesyncLagSwitchKeybind = (int)Settings.DesyncLagSwitchKeybind,
			DesyncFakeLagFreezeMs = Settings.DesyncFakeLagFreezeMs,
			DesyncFakeLagIntervalMs = Settings.DesyncFakeLagIntervalMs,
			ESPEnabled = Settings.ESPEnabled,
			ESPColorHue = Settings.ESPColorHue,
			ESPColorSat = Settings.ESPColorSat,
			ESPColorVal = Settings.ESPColorVal,
			TracersEnabled = Settings.TracersEnabled,
			HitboxesEnabled = Settings.HitboxesEnabled,
			NameTagsEnabled = Settings.NameTagsEnabled,
			CornerESPEnabled = Settings.CornerESPEnabled,
			BoneESPEnabled = Settings.BoneESPEnabled,
			ChamsEnabled = Settings.ChamsEnabled,
			RecolorTaggedEnabled = Settings.RecolorTaggedEnabled,
			RecolorTaggedColorHue = Settings.RecolorTaggedColorHue,
			RecolorTaggedColorSat = Settings.RecolorTaggedColorSat,
			RecolorTaggedColorVal = Settings.RecolorTaggedColorVal,
			RGBESPEnabled = Settings.RGBESPEnabled,
			BoxESPMode = Settings.BoxESPMode,
			FillESPEnabled = Settings.FillESPEnabled,
			FillESPOpacity = Settings.FillESPOpacity,
			FillESPColorHue = Settings.FillESPColorHue,
			FillESPColorSat = Settings.FillESPColorSat,
			FillESPColorVal = Settings.FillESPColorVal,
			TrailsEnabled = Settings.TrailsEnabled,
			TrailMinSpeed = Settings.TrailMinSpeed,
			TrailSpeedScale = Settings.TrailSpeedScale,
			TrailMinTime = Settings.TrailMinTime,
			TrailMaxTime = Settings.TrailMaxTime,
			TrailWidth = Settings.TrailWidth,
			TrailUsePlayerColor = Settings.TrailUsePlayerColor,
			TrailColorHue = Settings.TrailColorHue,
			TrailColorSat = Settings.TrailColorSat,
			TrailColorVal = Settings.TrailColorVal,
			BeaconsEnabled = Settings.BeaconsEnabled,
			BeaconWidth = Settings.BeaconWidth,
			ChinaHatESPEnabled = Settings.ChinaHatESPEnabled,
			RingESPEnabled = Settings.RingESPEnabled,
			RemoveLeavesEnabled = Settings.RemoveLeavesEnabled,
			VibrationAlertsEnabled = Settings.VibrationAlertsEnabled,
			VibrationAlertDistance = Settings.VibrationAlertDistance,
			VibrationAlertStrength = Settings.VibrationAlertStrength,
			PlayerGlowEnabled = Settings.PlayerGlowEnabled,
			PlayerGlowColorHue = Settings.PlayerGlowColorHue,
			PlayerGlowIntensity = Settings.PlayerGlowIntensity,
			BreadcrumbsEnabled = Settings.BreadcrumbsEnabled,
			BreadcrumbSpacing = Settings.BreadcrumbSpacing,
			BreadcrumbSize = Settings.BreadcrumbSize,
			BreadcrumbColorHue = Settings.BreadcrumbColorHue,
			BreadcrumbColorSat = Settings.BreadcrumbColorSat,
			BreadcrumbColorVal = Settings.BreadcrumbColorVal,
			KillFeedEnabled = Settings.KillFeedEnabled,
			KillFeedDuration = Settings.KillFeedDuration,
			KillFeedWorldSpace = Settings.KillFeedWorldSpace,
			TickRateEnabled = Settings.TickRateEnabled,
			TickRate = Settings.TickRate,
			SoundESPEnabled = Settings.SoundESPEnabled,
			SoundESPDistance = Settings.SoundESPDistance,
			SoundESPVolume = Settings.SoundESPVolume,
			SoundESPCooldown = Settings.SoundESPCooldown,
			SoundESPPreset = Settings.SoundESPPreset,
			SoundESPCustomPath = Settings.SoundESPCustomPath,
			SlideControlEnabled = Settings.SlideControlEnabled,
			SlideControlAmount = Settings.SlideControlAmount,
			TagNotificationEnabled = Settings.TagNotificationEnabled,
			TagNotificationDistance = Settings.TagNotificationDistance,
			NameTagsModEnabled = Settings.NameTagsModEnabled,
			NameTagFontSize = Settings.NameTagFontSize,
			NameTagOffset = Settings.NameTagOffset,
			NameTagRenderDistance = Settings.NameTagRenderDistance,
			PlatformTagsEnabled = Settings.PlatformTagsEnabled,
			PlatformTagFontSize = Settings.PlatformTagFontSize,
			PlatformTagOffset = Settings.PlatformTagOffset,
			FPSTagsModEnabled = Settings.FPSTagsModEnabled,
			FPSTagFontSize = Settings.FPSTagFontSize,
			FPSTagOffset = Settings.FPSTagOffset,
			AutoBranchEnabled = Settings.AutoBranchEnabled,
			AutoBranchRecordKeybind = (int)Settings.AutoBranchRecordKeybind,
			AutoBranchStopKeybind = (int)Settings.AutoBranchStopKeybind,
			AutoBranchReplayKeybind = (int)Settings.AutoBranchReplayKeybind,
			AutoBranchLoop = Settings.AutoBranchLoop,
			AutoBranchMovePlayer = Settings.AutoBranchMovePlayer,
			AutoBranchSmartMode = Settings.AutoBranchSmartMode,
			AutoBranchMatchRadius = Settings.AutoBranchMatchRadius,
			AutoBranchBlendTime = Settings.AutoBranchBlendTime,
			SkyboxEnabled = Settings.SkyboxEnabled,
			SkyboxMode = Settings.SkyboxMode,
			SkyboxColorHue = Settings.SkyboxColorHue,
			SkyboxColorSat = Settings.SkyboxColorSat,
			SkyboxColorVal = Settings.SkyboxColorVal,
			SkyboxColor2Hue = Settings.SkyboxColor2Hue,
			SkyboxColor2Sat = Settings.SkyboxColor2Sat,
			SkyboxColor2Val = Settings.SkyboxColor2Val,
			SkyboxImagePath = Settings.SkyboxImagePath,
			SkyboxGradientSpeed = Settings.SkyboxGradientSpeed,
			SkyboxStarCount = Settings.SkyboxStarCount,
			SkyboxStarSize = Settings.SkyboxStarSize,
			JewishMusicEnabled = Settings.JewishMusicEnabled,
			PullModMode = Settings.PullModMode,
			PullRandomiseMin = Settings.PullRandomiseMin,
			PullRandomiseMax = Settings.PullRandomiseMax,
			OneFramePullEnabled = Settings.OneFramePullEnabled,
			OneFramePullStrength = Settings.OneFramePullStrength,
			OneFramePullThreshold = Settings.OneFramePullThreshold,
			OneFramePullActivation = (int)Settings.OneFramePullActivation,
			OneFramePullToggleMode = Settings.OneFramePullToggleMode,
			RealPullEnabled = Settings.RealPullEnabled,
			RealPullDistance = Settings.RealPullDistance,
			RealPullThreshold = Settings.RealPullThreshold,
			RealPullActivation = (int)Settings.RealPullActivation,
			RealPullToggleMode = Settings.RealPullToggleMode,
			RealPullDownAmount = Settings.RealPullDownAmount,
			RealPullSlopeFix = Settings.RealPullSlopeFix,
			DownControllerEnabled = Settings.DownControllerEnabled,
			DownControllerKeybind = (int)Settings.DownControllerKeybind,
			DownControllerDistance = Settings.DownControllerDistance,
			DownControllerSpeed = Settings.DownControllerSpeed,
			MainPullEnabled = Settings.MainPullEnabled,
			MainPullStrength = Settings.MainPullStrength,
			MainPullTpTime = Settings.MainPullTpTime,
			MainPullSmoothing = Settings.MainPullSmoothing,
			MainPullActivation = (int)Settings.MainPullActivation,
			MainPullToggleMode = Settings.MainPullToggleMode,
			MainPullRaycastEnabled = Settings.MainPullRaycastEnabled,
			MainPullVelThresholdEnabled = Settings.MainPullVelThresholdEnabled,
			MainPullVelThreshold = Settings.MainPullVelThreshold,
			MainPullThresholdEnabled = Settings.MainPullThresholdEnabled,
			MainPullThresholdTime = Settings.MainPullThresholdTime,
			MainPullPredictEnabled = Settings.MainPullPredictEnabled,
			MainPullPredictPoints = Settings.MainPullPredictPoints,
			MainPullPredictCheckStart = Settings.MainPullPredictCheckStart,
			MainPullPredictCheckEnd = Settings.MainPullPredictCheckEnd,
			MainPullPredictStopBack = Settings.MainPullPredictStopBack,
			MainPullPredictFromHand = Settings.MainPullPredictFromHand,
			MainPullSurfaceAlignEnabled = Settings.MainPullSurfaceAlignEnabled,
			MainPullSurfaceAlignRadius = Settings.MainPullSurfaceAlignRadius,
			MainPullWallPullEnabled = Settings.MainPullWallPullEnabled,
			MainPullWallPullKeybind = (int)Settings.MainPullWallPullKeybind,
			MainPullWallPullStrength = Settings.MainPullWallPullStrength,
			MainPullWallPullTpTime = Settings.MainPullWallPullTpTime,
			MainPullWallPullSmoothing = Settings.MainPullWallPullSmoothing,
			WallPullModEnabled = Settings.WallPullModEnabled,
			WallPullModStrength = Settings.WallPullModStrength,
			WallPullModTpTime = Settings.WallPullModTpTime,
			WallPullModSmoothing = Settings.WallPullModSmoothing,
			WallPullModVelThreshold = Settings.WallPullModVelThreshold,
			WallPullModActivation = (int)Settings.WallPullModActivation,
			WallPullModToggleMode = Settings.WallPullModToggleMode,
			PullV3Enabled = Settings.PullV3Enabled,
			PullV3Strength = Settings.PullV3Strength,
			PullV3Threshold = Settings.PullV3Threshold,
			PullV3TpTime = Settings.PullV3TpTime,
			PullV3Activation = (int)Settings.PullV3Activation,
			PullV3ToggleMode = Settings.PullV3ToggleMode,
			PullV3LeftHand = Settings.PullV3LeftHand,
			PullV3RightHand = Settings.PullV3RightHand,
			PullV3MidPullRequired = Settings.PullV3MidPullRequired,
			PullV3ResetTime = Settings.PullV3ResetTime,
			PullV3MaxStacks = Settings.PullV3MaxStacks,
			PullV3FreezeStrength = Settings.PullV3FreezeStrength
		};
		return JsonUtility.ToJson(gClass, false);
	}

	// Token: 0x0600003B RID: 59 RVA: 0x000052A8 File Offset: 0x000034A8
	private static void ApplyConfigJson(string input)
	{
		ConfigData gClass = JsonUtility.FromJson<ConfigData>(input);
		bool flag = gClass != null;
		if (flag)
		{
		}
	}

	// Token: 0x04000159 RID: 345
	private const string string_0 = "https://absentauth.com";

	// Token: 0x0400015A RID: 346
	private static readonly byte[] keySalt;

	// Token: 0x0400015B RID: 347
	private static readonly byte keyByte;

	// Token: 0x0400015C RID: 348
	public static string LastSaveTime;

	// Token: 0x0400015D RID: 349
	public static string LastLoadTime;

	// Token: 0x0400015E RID: 350
	public static string CurrentConfigName;

	// Token: 0x0400015F RID: 351
	public static bool AutoLoadEnabled;

	// Token: 0x04000160 RID: 352
	public static string AutoLoadConfigName;

	// Token: 0x04000161 RID: 353
	private static List<string> configNames;

	// Token: 0x04000162 RID: 354
	public static int SelectedConfigIndex;

	// Token: 0x04000163 RID: 355
	public static bool IsOperationPending;

	// Token: 0x04000164 RID: 356
	public static string OperationStatus;

	// Token: 0x04000165 RID: 357
	public static int MaxConfigs;

	// Token: 0x04000166 RID: 358
	private static bool autoLoadStarted;

	// Token: 0x02000051 RID: 81
	[CompilerGenerated]
	[Serializable]
	private sealed class Class17
	{
		// Token: 0x06000218 RID: 536 RVA: 0x000226E3 File Offset: 0x000208E3
		internal void method_0(object object_0)
		{
			ConfigManager.SaveCurrentConfig();
		}

		// Token: 0x06000219 RID: 537 RVA: 0x000226EC File Offset: 0x000208EC
		internal void method_1(object object_0)
		{
			try
			{
				ConfigManager.OperationStatus = ((ConfigManager.GetJsonInt(ConfigManager.PostApi("/api/configs/autoload", string.Concat(new string[]
				{
					",\"enabled\":",
					ConfigManager.AutoLoadEnabled ? "true" : "false",
					",\"name\":\"",
					ConfigManager.JsonEscape(ConfigManager.AutoLoadConfigName),
					"\"}"
				})), "r") == 1) ? "Saved!" : "Failed");
			}
			catch
			{
				ConfigManager.OperationStatus = "Failed";
			}
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0002278C File Offset: 0x0002098C
		internal void method_2(object object_0)
		{
			try
			{
				string string_ = ConfigManager.PostApi("/api/configs/autoload-data", "}");
				bool flag = ConfigManager.GetJsonInt(string_, "r") == 1 && ConfigManager.JsonHasField(string_, "found");
				if (flag)
				{
					string text = ConfigManager.GetJsonString(string_, "name");
					string text2 = ConfigManager.GetJsonString(string_, "data");
					bool flag2 = !string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2);
					if (flag2)
					{
						ConfigManager.AutoLoadEnabled = true;
						ConfigManager.AutoLoadConfigName = text;
						ConfigManager.ApplyConfigJson(text2);
						ConfigManager.CurrentConfigName = text;
						ConfigManager.LastLoadTime = DateTime.Now.ToString("HH:mm:ss");
						ConfigManager.OperationStatus = "Auto-loaded " + text;
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x040004C3 RID: 1219
		public static readonly ConfigManager.Class17 _003C_003E9 = new ConfigManager.Class17();

		// Token: 0x040004C4 RID: 1220
		public static WaitCallback _003C_003E9__25_0;

		// Token: 0x040004C5 RID: 1221
		public static WaitCallback _003C_003E9__35_0;

		// Token: 0x040004C6 RID: 1222
		public static WaitCallback _003C_003E9__38_0;
	}

	// Token: 0x02000052 RID: 82
	[CompilerGenerated]
	private sealed class Class18
	{
		// Token: 0x0600021D RID: 541 RVA: 0x00022874 File Offset: 0x00020A74
		internal void method_0(object object_0)
		{
			try
			{
				string string_ = ConfigManager.BuildConfigJson();
				string value = string.Concat(new string[]
				{
					",\"name\":\"",
					ConfigManager.JsonEscape(this.configName),
					"\",\"data\":\"",
					ConfigManager.JsonEscape(string_),
					"\"}"
				});
				bool flag = ConfigManager.GetJsonInt(ConfigManager.PostApi("/api/configs/save", value), "r") == 1;
				if (flag)
				{
					ConfigManager.OperationStatus = "Saved!";
					ConfigManager.LastSaveTime = DateTime.Now.ToString("HH:mm:ss");
					ConfigManager.CurrentConfigName = this.configName;
					bool flag2 = !ConfigManager.configNames.Contains(this.configName);
					if (flag2)
					{
						ConfigManager.configNames.Add(this.configName);
					}
				}
				else
				{
					ConfigManager.OperationStatus = "Save failed";
				}
			}
			catch
			{
				ConfigManager.OperationStatus = "Save failed";
			}
			finally
			{
				ConfigManager.IsOperationPending = false;
			}
		}

		// Token: 0x040004C7 RID: 1223
		public string configName;
	}

	// Token: 0x02000053 RID: 83
	[CompilerGenerated]
	private sealed class Class19
	{
		// Token: 0x0600021F RID: 543 RVA: 0x00022990 File Offset: 0x00020B90
		internal void method_0(object object_0)
		{
			try
			{
				string text = ConfigManager.GetJsonString(ConfigManager.PostApi("/api/configs/load", ",\"name\":\"" + ConfigManager.JsonEscape(this.configName) + "\"}"), "data");
				bool flag = !string.IsNullOrEmpty(text);
				if (flag)
				{
					ConfigManager.ApplyConfigJson(text);
					ConfigManager.CurrentConfigName = this.configName;
					ConfigManager.LastLoadTime = DateTime.Now.ToString("HH:mm:ss");
					ConfigManager.OperationStatus = "Loaded!";
				}
				else
				{
					ConfigManager.OperationStatus = "Not found";
				}
			}
			catch
			{
				ConfigManager.OperationStatus = "Load failed";
			}
			finally
			{
				ConfigManager.IsOperationPending = false;
			}
		}

		// Token: 0x040004C8 RID: 1224
		public string configName;
	}

	// Token: 0x02000054 RID: 84
	[CompilerGenerated]
	private sealed class Class20
	{
		// Token: 0x06000221 RID: 545 RVA: 0x00022A64 File Offset: 0x00020C64
		internal void method_0(object object_0)
		{
			try
			{
				bool flag = ConfigManager.GetJsonInt(ConfigManager.PostApi("/api/configs/delete", ",\"name\":\"" + ConfigManager.JsonEscape(this.configName) + "\"}"), "r") == 1;
				if (flag)
				{
					ConfigManager.configNames.Remove(this.configName);
					ConfigManager.OperationStatus = "Deleted!";
				}
				else
				{
					ConfigManager.OperationStatus = "Delete failed";
				}
			}
			catch
			{
				ConfigManager.OperationStatus = "Delete failed";
			}
			finally
			{
				ConfigManager.IsOperationPending = false;
			}
		}

		// Token: 0x040004C9 RID: 1225
		public string configName;
	}

	// Token: 0x02000055 RID: 85
	[CompilerGenerated]
	private sealed class Class21
	{
		// Token: 0x06000223 RID: 547 RVA: 0x00022B18 File Offset: 0x00020D18
		internal void method_0(object object_0)
		{
			try
			{
				bool flag = ConfigManager.GetJsonInt(ConfigManager.PostApi("/api/configs/rename", string.Concat(new string[]
				{
					",\"oldName\":\"",
					ConfigManager.JsonEscape(this.oldName),
					"\",\"newName\":\"",
					ConfigManager.JsonEscape(this.newName),
					"\"}"
				})), "r") == 1;
				if (flag)
				{
					int num = ConfigManager.configNames.IndexOf(this.oldName);
					bool flag2 = num >= 0;
					if (flag2)
					{
						ConfigManager.configNames[num] = this.newName;
					}
					bool flag3 = ConfigManager.CurrentConfigName == this.oldName;
					if (flag3)
					{
						ConfigManager.CurrentConfigName = this.newName;
					}
					bool flag4 = ConfigManager.AutoLoadConfigName == this.oldName;
					if (flag4)
					{
						ConfigManager.AutoLoadConfigName = this.newName;
					}
					ConfigManager.OperationStatus = "Renamed!";
				}
				else
				{
					ConfigManager.OperationStatus = "Rename failed";
				}
			}
			catch
			{
				ConfigManager.OperationStatus = "Rename failed";
			}
			finally
			{
				ConfigManager.IsOperationPending = false;
			}
		}

		// Token: 0x040004CA RID: 1226
		public string oldName;

		// Token: 0x040004CB RID: 1227
		public string newName;
	}
}
