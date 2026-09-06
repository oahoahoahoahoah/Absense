using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x0200001B RID: 27
public static class Macros
{
	// Token: 0x17000003 RID: 3
	// (get) Token: 0x0600008E RID: 142 RVA: 0x0000C724 File Offset: 0x0000A924
	// (set) Token: 0x0600008F RID: 143 RVA: 0x0000C73B File Offset: 0x0000A93B
	public static bool IsRecording { get; private set; }

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000090 RID: 144 RVA: 0x0000C744 File Offset: 0x0000A944
	// (set) Token: 0x06000091 RID: 145 RVA: 0x0000C75B File Offset: 0x0000A95B
	public static bool IsReplaying { get; private set; }

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000092 RID: 146 RVA: 0x0000C764 File Offset: 0x0000A964
	// (set) Token: 0x06000093 RID: 147 RVA: 0x0000C77B File Offset: 0x0000A97B
	public static bool IsLooping { get; private set; }

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000094 RID: 148 RVA: 0x0000C784 File Offset: 0x0000A984
	public static int RecordingFrameCount
	{
		get
		{
			return Macros.recordingFrames.Count;
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000095 RID: 149 RVA: 0x0000C790 File Offset: 0x0000A990
	public static int MacroCount
	{
		get
		{
			return Macros.macros.Count;
		}
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x06000096 RID: 150 RVA: 0x0000C79C File Offset: 0x0000A99C
	// (set) Token: 0x06000097 RID: 151 RVA: 0x0000C7B3 File Offset: 0x0000A9B3
	public static bool IsSyncStarted { get; private set; }

	// Token: 0x06000098 RID: 152 RVA: 0x0000C7BC File Offset: 0x0000A9BC
	public static void Update()
	{
		List<Macros.Macro> list = Macros.syncedMacros;
		bool flag4 = list != null;
		if (flag4)
		{
			Macros.macros.Clear();
			Macros.macros.AddRange(list);
			Macros.syncedMacros = null;
			Macros.IsRecording = true;
		}
		int count = Macros.pendingUploads.Count;
		bool flag5 = count > 0;
		if (flag5)
		{
			object obj = Macros.syncLock;
			List<KeyValuePair<string, string>> list2;
			lock (obj)
			{
				list2 = new List<KeyValuePair<string, string>>(Macros.pendingUploads);
				Macros.pendingUploads.Clear();
			}
			foreach (KeyValuePair<string, string> item in list2)
			{
				Macros.LoadMacroFromDisk(item.Key, item.Value);
			}
		}
		bool flag7 = !Settings.AutoBranchEnabled;
		if (flag7)
		{
			Macros.IsReplaying = false;
			Macros.IsLooping = false;
			Macros.IsSyncStarted = false;
			Macros.UpdatePathVisual();
			Macros.ClearPathVisual();
		}
		else
		{
			bool flag8 = Settings.AutoBranchSmartMode && Settings.AutoBranchPathFinder;
			if (flag8)
			{
				Macros.BuildPathVisual();
			}
			else
			{
				Macros.UpdatePathVisual();
			}
			bool flag = Settings.IsPressed(Settings.AutoBranchRecordKeybind);
			bool flag9 = flag && !Macros.paused;
			if (flag9)
			{
				Macros.StartRecording();
			}
			Macros.paused = flag;
			bool flag2 = Settings.IsPressed(Settings.AutoBranchStopKeybind);
			bool flag10 = flag2 && !Macros.movePlayer;
			if (flag10)
			{
				bool isReplaying = Macros.IsReplaying;
				if (isReplaying)
				{
					Macros.StartReplay();
				}
				else
				{
					bool isLooping = Macros.IsLooping;
					if (isLooping)
					{
						Macros.StopReplayAll();
					}
				}
			}
			Macros.movePlayer = flag2;
			bool flag3 = Settings.IsPressed(Settings.AutoBranchReplayKeybind);
			bool flag11 = flag3 && !Macros.smartMode;
			if (flag11)
			{
				Macros.RefreshMacroList();
			}
			Macros.smartMode = flag3;
			Macros.ClearPathVisual();
		}
	}

	// Token: 0x06000099 RID: 153 RVA: 0x0000C9CC File Offset: 0x0000ABCC
	public static void StartRecording()
	{
		bool isLooping = Macros.IsLooping;
		if (isLooping)
		{
			Macros.StopReplayAll();
		}
		Macros.recordingFrames.Clear();
	}

	// Token: 0x0600009A RID: 154 RVA: 0x0000C9F8 File Offset: 0x0000ABF8
	public static void StartReplay()
	{
		int count = Macros.recordingFrames.Count;
		bool flag = count > 1;
		if (flag)
		{
			Macros.FinishRecording();
		}
	}

	// Token: 0x0600009B RID: 155 RVA: 0x0000CA24 File Offset: 0x0000AC24
	private static void FinishRecording()
	{
		int count = Macros.macros.Count;
		bool flag = count < 100;
		if (flag)
		{
			Macros.Macro @class = new Macros.Macro();
			Macros.macros.Add(@class);
			Macros.AddMacro(@class);
		}
	}

	// Token: 0x0600009C RID: 156 RVA: 0x0000CA68 File Offset: 0x0000AC68
	private static string SanitizeName(string name)
	{
		Macros.MacroNamePredicate @class = new Macros.MacroNamePredicate();
		bool flag;
		do
		{
			flag = Macros.macros.Exists(new Predicate<Macros.Macro>(@class.method_0));
		}
		while (flag);
		return @class.name;
	}

	// Token: 0x0600009D RID: 157 RVA: 0x0000CAB0 File Offset: 0x0000ACB0
	private static Vector3[] ComputeMoveDirs(List<Macros.MacroFrame> frames)
	{
		Vector3[] array = (Vector3[])new Vector3[frames.Count];
		int num2 = 0;
		for (;;)
		{
			bool flag = num2 >= frames.Count;
			if (flag)
			{
				break;
			}
			int num3 = num2;
			int index = Mathf.Min(num3 + 6, frames.Count - 1);
			array[num2] = Macros.ApplyReplayOffset(frames[index].playerPos - frames[num2].playerPos);
			num2++;
		}
		return array;
	}

	// Token: 0x0600009E RID: 158 RVA: 0x0000CB3C File Offset: 0x0000AD3C
	public static void StopReplay()
	{
		foreach (Macros.Macro item in Macros.macros)
		{
			Macros.RequestUpload(item.Name);
		}
		Macros.macros.Clear();
		Macros.IsSyncStarted = false;
		Macros.activeMacro = null;
		Macros.UpdatePathVisual();
	}

	// Token: 0x0600009F RID: 159 RVA: 0x0000CBB8 File Offset: 0x0000ADB8
	private static Vector3 ApplyReplayOffset(Vector3 posA)
	{
		bool flag = posA.sqrMagnitude > 0.0001f;
		Vector3 vector;
		if (flag)
		{
			vector = posA.normalized;
		}
		else
		{
			vector = Vector3.zero;
		}
		return vector;
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x0000CBED File Offset: 0x0000ADED
	public static void SyncMacros()
	{
		Macros.CloudStatus = "Offline — cloud sync disabled.";
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x0000CBFA File Offset: 0x0000ADFA
	private static void AddMacro(Macros.Macro macro)
	{
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x0000CBFD File Offset: 0x0000ADFD
	private static void RequestUpload(string name)
	{
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x0000CC00 File Offset: 0x0000AE00
	public static void SaveMacroToDisk(string name, string data)
	{
		object obj = Macros.syncLock;
		lock (obj)
		{
			Macros.pendingUploads.Add(new KeyValuePair<string, string>(name, data));
		}
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x0000CC50 File Offset: 0x0000AE50
	public static string LoadMacroFromDisk(string name, string data)
	{
		bool flag = string.IsNullOrEmpty(data);
		string text;
		if (flag)
		{
			text = "Empty macro.";
		}
		else
		{
			List<Macros.MacroFrame> list = Macros.DeserializeFrames(data);
			bool flag2 = list != null;
			if (flag2)
			{
				int count = list.Count;
				bool flag3 = count >= 2;
				if (flag3)
				{
					int count2 = Macros.macros.Count;
					bool flag4 = count2 >= 100;
					if (flag4)
					{
						return "Macro limit reached.";
					}
					string finalName = (string.IsNullOrEmpty(name) ? Macros.SanitizeName("Macro") : name);
					Predicate<Macros.Macro> <>9__0;
					for (;;)
					{
						List<Macros.Macro> list2 = Macros.macros;
						Predicate<Macros.Macro> predicate;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = (Macros.Macro activeMacro) => activeMacro.Name == finalName);
						}
						if (!list2.Exists(predicate))
						{
							break;
						}
						finalName = Macros.SanitizeName(finalName);
					}
					Macros.Macro @class = new Macros.Macro
					{
						Name = finalName,
						Frames = list,
						MoveDirs = Macros.ComputeMoveDirs(list)
					};
					Macros.macros.Add(@class);
					Macros.AddMacro(@class);
					return "Added: " + finalName;
				}
			}
			text = "Invalid macro data.";
		}
		return text;
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x0000CD90 File Offset: 0x0000AF90
	public static string SerializeMacro(int index)
	{
		bool flag = index < 0;
		string text;
		if (flag)
		{
			text = "";
		}
		else
		{
			bool flag2 = index < Macros.macros.Count;
			if (flag2)
			{
				text = Macros.SerializeFrames(Macros.macros[index].Frames);
			}
			else
			{
				text = "";
			}
		}
		return text;
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x0000CDE4 File Offset: 0x0000AFE4
	private static string SerializeFrames(List<Macros.MacroFrame> frames)
	{
		return "GZ1" + Macros.EscapeField(Macros.SerializeMoveDirs(frames));
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x0000CE0C File Offset: 0x0000B00C
	private static List<Macros.MacroFrame> DeserializeFrames(string name)
	{
		bool flag = string.IsNullOrEmpty(name);
		List<Macros.MacroFrame> list;
		if (flag)
		{
			list = null;
		}
		else
		{
			try
			{
				bool flag2 = name.StartsWith("GZ1");
				if (flag2)
				{
					return Macros.ParseFrames(Macros.UnescapeField(name.Substring("GZ1".Length)));
				}
			}
			catch (Exception)
			{
				return null;
			}
			list = Macros.ParseFrames(name);
		}
		return list;
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x0000CE7C File Offset: 0x0000B07C
	private static string EscapeField(string name)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(name);
		string text;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
			{
				gZipStream.Write(bytes, 0, bytes.Length);
			}
			text = Convert.ToBase64String(memoryStream.ToArray());
		}
		return text;
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x0000CEF8 File Offset: 0x0000B0F8
	private static string UnescapeField(string name)
	{
		string @string;
		using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(name)))
		{
			using (GZipStream gZipStream = new GZipStream(stream, CompressionMode.Decompress))
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					gZipStream.CopyTo(memoryStream);
					@string = Encoding.UTF8.GetString(memoryStream.ToArray());
				}
			}
		}
		return @string;
	}

	// Token: 0x060000AA RID: 170 RVA: 0x0000CF88 File Offset: 0x0000B188
	private static string SerializeMoveDirs(List<Macros.MacroFrame> frames)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("ABM1");
		foreach (Macros.MacroFrame item in frames)
		{
			stringBuilder.Append(';');
			Macros.WriteFrame(stringBuilder, item);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060000AB RID: 171 RVA: 0x0000D004 File Offset: 0x0000B204
	private static void WriteFrame(StringBuilder sb, Macros.MacroFrame frame)
	{
		float[] array = new float[]
		{
			frame.time,
			frame.playerPos.x,
			frame.playerPos.y,
			frame.playerPos.z,
			frame.playerRot.x,
			frame.playerRot.y,
			frame.playerRot.z,
			frame.playerRot.w,
			frame.rigRootPos.x,
			frame.rigRootPos.y,
			frame.rigRootPos.z,
			frame.rigRootRot.x,
			frame.rigRootRot.y,
			frame.rigRootRot.z,
			frame.rigRootRot.w,
			frame.headRigTargetPos.x,
			frame.headRigTargetPos.y,
			frame.headRigTargetPos.z,
			frame.headRigTargetRot.x,
			frame.headRigTargetRot.y,
			frame.headRigTargetRot.z,
			frame.headRigTargetRot.w,
			frame.leftHandRigTargetPos.x,
			frame.leftHandRigTargetPos.y,
			frame.leftHandRigTargetPos.z,
			frame.leftHandRigTargetRot.x,
			frame.leftHandRigTargetRot.y,
			frame.leftHandRigTargetRot.z,
			frame.leftHandRigTargetRot.w,
			frame.rightHandRigTargetPos.x,
			frame.rightHandRigTargetPos.y,
			frame.rightHandRigTargetPos.z,
			frame.rightHandRigTargetRot.x,
			frame.rightHandRigTargetRot.y,
			frame.rightHandRigTargetRot.z,
			frame.rightHandRigTargetRot.w,
			frame.headMeshPos.x,
			frame.headMeshPos.y,
			frame.headMeshPos.z,
			frame.headMeshRot.x,
			frame.headMeshRot.y,
			frame.headMeshRot.z,
			frame.headMeshRot.w,
			frame.headMeshScale.x,
			frame.headMeshScale.y,
			frame.headMeshScale.z
		};
		for (int i = 0; i < array.Length; i++)
		{
			bool flag = i > 0;
			if (flag)
			{
				sb.Append(',');
			}
			sb.Append(array[i].ToString("0.#####", CultureInfo.InvariantCulture));
		}
		int num = ((frame.bones != null) ? frame.bones.Length : 0);
		sb.Append(',').Append(num);
		for (int j = 0; j < num; j++)
		{
			Macros.BoneTransform @struct = frame.bones[j];
			Macros.WriteFloat(sb, @struct.localPos.x);
			Macros.WriteFloat(sb, @struct.localPos.y);
			Macros.WriteFloat(sb, @struct.localPos.z);
			Macros.WriteFloat(sb, @struct.localRot.x);
			Macros.WriteFloat(sb, @struct.localRot.y);
			Macros.WriteFloat(sb, @struct.localRot.z);
			Macros.WriteFloat(sb, @struct.localRot.w);
			Macros.WriteFloat(sb, @struct.localScale.x);
			Macros.WriteFloat(sb, @struct.localScale.y);
			Macros.WriteFloat(sb, @struct.localScale.z);
		}
	}

	// Token: 0x060000AC RID: 172 RVA: 0x0000D414 File Offset: 0x0000B614
	private static void WriteFloat(StringBuilder sb, float valD)
	{
		StringBuilder stringBuilder = sb.Append(',');
		stringBuilder.Append(valD.ToString("0.#####", CultureInfo.InvariantCulture));
	}

	// Token: 0x060000AD RID: 173 RVA: 0x0000D444 File Offset: 0x0000B644
	private static List<Macros.MacroFrame> ParseFrames(string name)
	{
		bool flag = string.IsNullOrEmpty(name);
		List<Macros.MacroFrame> list2;
		if (flag)
		{
			list2 = null;
		}
		else
		{
			string[] array = name.Split(new char[] { ';' });
			bool flag2 = array.Length >= 2 && !(array[0] != "ABM1");
			if (flag2)
			{
				List<Macros.MacroFrame> list = new List<Macros.MacroFrame>(array.Length - 1);
				for (int i = 1; i < array.Length; i++)
				{
					string text = array[i];
					string[] array2 = text.Split(new char[] { ',' });
					bool flag3 = array2.Length < 46;
					if (!flag3)
					{
						int num = 0;
						Macros.MacroFrame item = new Macros.MacroFrame
						{
							time = Macros.ParseFloat(array2[num++])
						};
						float num2 = Macros.ParseFloat(array2[num++]);
						float num3 = Macros.ParseFloat(array2[num++]);
						item.playerPos = new Vector3(num2, num3, Macros.ParseFloat(array2[num++]));
						float num4 = Macros.ParseFloat(array2[num++]);
						float num5 = Macros.ParseFloat(array2[num++]);
						float num6 = Macros.ParseFloat(array2[num++]);
						item.playerRot = new Quaternion(num4, num5, num6, Macros.ParseFloat(array2[num++]));
						float num7 = Macros.ParseFloat(array2[num++]);
						float num8 = Macros.ParseFloat(array2[num++]);
						item.rigRootPos = new Vector3(num7, num8, Macros.ParseFloat(array2[num++]));
						float num9 = Macros.ParseFloat(array2[num++]);
						float num10 = Macros.ParseFloat(array2[num++]);
						float num11 = Macros.ParseFloat(array2[num++]);
						item.rigRootRot = new Quaternion(num9, num10, num11, Macros.ParseFloat(array2[num++]));
						float num12 = Macros.ParseFloat(array2[num++]);
						float num13 = Macros.ParseFloat(array2[num++]);
						item.headRigTargetPos = new Vector3(num12, num13, Macros.ParseFloat(array2[num++]));
						float num14 = Macros.ParseFloat(array2[num++]);
						float num15 = Macros.ParseFloat(array2[num++]);
						float num16 = Macros.ParseFloat(array2[num++]);
						item.headRigTargetRot = new Quaternion(num14, num15, num16, Macros.ParseFloat(array2[num++]));
						float num17 = Macros.ParseFloat(array2[num++]);
						float num18 = Macros.ParseFloat(array2[num++]);
						item.leftHandRigTargetPos = new Vector3(num17, num18, Macros.ParseFloat(array2[num++]));
						float num19 = Macros.ParseFloat(array2[num++]);
						float num20 = Macros.ParseFloat(array2[num++]);
						float num21 = Macros.ParseFloat(array2[num++]);
						item.leftHandRigTargetRot = new Quaternion(num19, num20, num21, Macros.ParseFloat(array2[num++]));
						float num22 = Macros.ParseFloat(array2[num++]);
						float num23 = Macros.ParseFloat(array2[num++]);
						item.rightHandRigTargetPos = new Vector3(num22, num23, Macros.ParseFloat(array2[num++]));
						float num24 = Macros.ParseFloat(array2[num++]);
						float num25 = Macros.ParseFloat(array2[num++]);
						float num26 = Macros.ParseFloat(array2[num++]);
						item.rightHandRigTargetRot = new Quaternion(num24, num25, num26, Macros.ParseFloat(array2[num++]));
						float num27 = Macros.ParseFloat(array2[num++]);
						float num28 = Macros.ParseFloat(array2[num++]);
						item.headMeshPos = new Vector3(num27, num28, Macros.ParseFloat(array2[num++]));
						float num29 = Macros.ParseFloat(array2[num++]);
						float num30 = Macros.ParseFloat(array2[num++]);
						float num31 = Macros.ParseFloat(array2[num++]);
						item.headMeshRot = new Quaternion(num29, num30, num31, Macros.ParseFloat(array2[num++]));
						float num32 = Macros.ParseFloat(array2[num++]);
						float num33 = Macros.ParseFloat(array2[num++]);
						item.headMeshScale = new Vector3(num32, num33, Macros.ParseFloat(array2[num++]));
						item.bones = null;
						bool flag4 = num < array2.Length;
						if (flag4)
						{
							int num34 = (int)Macros.ParseFloat(array2[num++]);
							bool flag5 = num34 > 0;
							if (flag5)
							{
								int num35 = num;
								bool flag6 = num35 + num34 * 10 <= array2.Length;
								if (flag6)
								{
									item.bones = new Macros.BoneTransform[num34];
									for (int j = 0; j < num34; j++)
									{
										ref Macros.BoneTransform reference = ref item.bones[j];
										float num36 = Macros.ParseFloat(array2[num++]);
										float num37 = Macros.ParseFloat(array2[num++]);
										reference.localPos = new Vector3(num36, num37, Macros.ParseFloat(array2[num++]));
										ref Macros.BoneTransform reference2 = ref item.bones[j];
										float num38 = Macros.ParseFloat(array2[num++]);
										float num39 = Macros.ParseFloat(array2[num++]);
										float num40 = Macros.ParseFloat(array2[num++]);
										reference2.localRot = new Quaternion(num38, num39, num40, Macros.ParseFloat(array2[num++]));
										ref Macros.BoneTransform reference3 = ref item.bones[j];
										float num41 = Macros.ParseFloat(array2[num++]);
										float num42 = Macros.ParseFloat(array2[num++]);
										reference3.localScale = new Vector3(num41, num42, Macros.ParseFloat(array2[num++]));
									}
								}
							}
						}
						list.Add(item);
					}
				}
				list2 = list;
			}
			else
			{
				list2 = null;
			}
		}
		return list2;
	}

	// Token: 0x060000AE RID: 174 RVA: 0x0000DA7C File Offset: 0x0000BC7C
	private static float ParseFloat(string name)
	{
		float result = 0f;
		bool flag = float.TryParse(name, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
		float num;
		if (flag)
		{
			num = result;
		}
		else
		{
			num = 0f;
		}
		return num;
	}

	// Token: 0x060000AF RID: 175 RVA: 0x0000DAB8 File Offset: 0x0000BCB8
	public static void RefreshMacroList()
	{
		bool flag = !Macros.IsReplaying;
		if (!flag)
		{
			Macros.StartReplay();
		}
		bool flag2 = Macros.recordingFrames.Count != 0;
		if (flag2)
		{
		}
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x0000DAF8 File Offset: 0x0000BCF8
	public static void PlayMacro(int index)
	{
		bool flag = index >= 0;
		if (flag)
		{
			bool flag2 = index < Macros.macros.Count;
			if (flag2)
			{
				bool isReplaying = Macros.IsReplaying;
				if (isReplaying)
				{
					Macros.StartReplay();
				}
				List<Macros.MacroFrame> list = Macros.macros[index].Frames;
				bool flag3 = list == null;
				if (!flag3)
				{
					bool flag4 = list.Count != 0;
					if (flag4)
					{
					}
				}
			}
		}
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x0000DB71 File Offset: 0x0000BD71
	public static void StopReplayAll()
	{
		Macros.IsLooping = false;
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x0000DB7C File Offset: 0x0000BD7C
	public static void DeleteMacro(int index)
	{
		bool flag = index < 0;
		if (!flag)
		{
			bool flag2 = index >= Macros.macros.Count;
			if (!flag2)
			{
				Macros.Macro @class = Macros.macros[index];
				bool flag3 = Macros.replayFrames != @class.Frames;
				if (!flag3)
				{
					Macros.replayFrames = null;
				}
				bool flag4 = Macros.activeMacro != @class;
				if (!flag4)
				{
					Macros.activeMacro = null;
				}
				Macros.RequestUpload(@class.Name);
				Macros.macros.RemoveAt(index);
			}
		}
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x0000DC14 File Offset: 0x0000BE14
	public static List<string> ListMacroNames()
	{
		List<string> list = new List<string>(Macros.macros.Count);
		foreach (Macros.Macro item in Macros.macros)
		{
			list.Add(item.Name + "  (" + ((item.Frames != null) ? item.Frames.Count : 0).ToString() + "f)");
		}
		return list;
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x0000DCB8 File Offset: 0x0000BEB8
	public static void UploadMacro()
	{
		Macros.recordingFrames.Clear();
		Macros.IsReplaying = false;
		Macros.IsLooping = false;
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x0000DCD4 File Offset: 0x0000BED4
	public static void LateUpdate()
	{
		bool flag = !Settings.AutoBranchEnabled;
		if (!flag)
		{
			VRRig val = ((GorillaTagger.Instance != null) ? GorillaTagger.Instance.offlineVRRig : null);
			bool flag2 = val == null;
			if (!flag2)
			{
				bool isReplaying = Macros.IsReplaying;
				if (isReplaying)
				{
					Macros.MacroFrame item = Macros.CaptureFrame(val);
					item.time = Time.time - Macros.recordTime;
					Macros.recordingFrames.Add(item);
					int count = Macros.recordingFrames.Count;
					bool flag3 = count > 7200;
					if (flag3)
					{
						Macros.StartReplay();
					}
				}
				else
				{
					bool autoBranchSmartMode = Settings.AutoBranchSmartMode;
					if (autoBranchSmartMode)
					{
						Macros.CaptureRecordingFrame(val);
					}
					else
					{
						Macros.IsSyncStarted = false;
						bool flag4 = !Macros.IsLooping;
						if (!flag4)
						{
							List<Macros.MacroFrame> list = Macros.replayFrames;
							bool flag5 = list != null && list.Count != 0;
							if (flag5)
							{
								float num = Time.time - Macros.replayTime;
								int count2 = list.Count;
								float time = list[count2 - 1].time;
								bool flag6 = num > time;
								if (flag6)
								{
									bool flag7 = !Settings.AutoBranchLoop;
									if (flag7)
									{
										Macros.StopReplayAll();
										return;
									}
									Macros.replayTime = Time.time;
									num = 0f;
								}
								Macros.MacroFrame struct6_ = Macros.SampleFrame(list, num);
								Macros.ApplyFrame(val, struct6_, Vector3.zero);
								bool flag8 = Settings.AutoBranchMovePlayer && GTPlayer.Instance != null;
								if (flag8)
								{
									GTPlayer.Instance.transform.position = struct6_.playerPos;
									GTPlayer.Instance.transform.rotation = struct6_.playerRot;
								}
							}
							else
							{
								Macros.StopReplayAll();
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x0000DE9C File Offset: 0x0000C09C
	private static void CaptureRecordingFrame(VRRig rig)
	{
		bool flag2 = Macros.macros.Count == 0;
		if (flag2)
		{
			Macros.IsSyncStarted = false;
		}
		else
		{
			bool flag3 = GTPlayer.Instance == null;
			if (flag3)
			{
				Macros.IsSyncStarted = false;
			}
			else
			{
				Vector3 position = GTPlayer.Instance.transform.position;
				Vector3 vector3_ = Macros.GetPlayerPosition();
				Vector3 averagedVelocity = GTPlayer.Instance.AveragedVelocity;
				float magnitude = averagedVelocity.magnitude;
				Vector3 posA = Macros.ApplyReplayOffset(averagedVelocity);
				int int_;
				float float_;
				Macros.Macro @class = Macros.FindClosestFrame(position, vector3_, posA, magnitude, out int_, out float_);
				bool flag4 = !Macros.IsSyncStarted;
				if (flag4)
				{
					bool flag5 = @class == null;
					if (flag5)
					{
						return;
					}
					Macros.ApplyMacroFrame(@class, int_, float_, position);
				}
				else
				{
					float num = Macros.replaySpeed + (Time.time - Macros.blendTimer);
					List<Macros.MacroFrame> frames = Macros.activeMacro.Frames;
					int count = frames.Count;
					bool flag = num > frames[count - 1].time;
					int countA = Macros.FindFrameIndex(frames, num);
					float num2 = Macros.ScoreFrame(Macros.activeMacro, countA, position, vector3_, posA, magnitude);
					bool flag6 = ((Time.time - Macros.lastFrameTime > 0.4f) & (@class != null && (@class != Macros.activeMacro || Mathf.Abs(@class.Frames[int_].time - num) > 0.75f))) && float_ > num2;
					if (flag6)
					{
						Macros.ApplyMacroFrame(@class, int_, float_, Macros.startOffset);
					}
					else
					{
						bool flag7 = flag;
						if (flag7)
						{
							bool flag8 = @class == null;
							if (flag8)
							{
								Macros.IsSyncStarted = false;
								return;
							}
							Macros.ApplyMacroFrame(@class, int_, float_, Macros.startOffset);
						}
					}
				}
				float replaySpeed = Macros.replaySpeed + (Time.time - Macros.blendTimer);
				Macros.MacroFrame struct6_ = Macros.SampleFrame(Macros.activeMacro.Frames, replaySpeed);
				float num3 = 1f - Mathf.Clamp01((Time.time - Macros.matchTimer) / Mathf.Max(0.01f, Settings.AutoBranchBlendTime));
				Vector3 val = Macros.replayOrigin * num3;
				Macros.ApplyFrame(rig, struct6_, val);
				Vector3 position2 = struct6_.playerPos + val;
				bool flag9 = Settings.AutoBranchMovePlayer && GTPlayer.Instance != null;
				if (flag9)
				{
					GTPlayer.Instance.transform.position = position2;
				}
				Macros.startOffset = position2;
			}
		}
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x0000E108 File Offset: 0x0000C308
	private static void ApplyMacroFrame(Macros.Macro macro, int index, float valD, Vector3 posA)
	{
		Macros.activeMacro = macro;
		Macros.replaySpeed = macro.Frames[index].time;
		Macros.blendTimer = Time.time;
		Macros.replayOrigin = posA - macro.Frames[index].playerPos;
		Macros.matchTimer = Time.time;
		Macros.lastFrameTime = Time.time;
		Macros.startOffset = posA;
		Macros.IsSyncStarted = true;
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x0000E17C File Offset: 0x0000C37C
	private static Vector3 GetPlayerPosition()
	{
		bool flag = Camera.main != null;
		Vector3 vector;
		if (flag)
		{
			vector = Macros.ApplyReplayOffset(Camera.main.transform.forward);
		}
		else
		{
			VRRig val = ((GorillaTagger.Instance != null) ? GorillaTagger.Instance.offlineVRRig : null);
			bool flag2 = val != null && val.head != null && val.head.rigTarget != null;
			if (flag2)
			{
				vector = Macros.ApplyReplayOffset(val.head.rigTarget.forward);
			}
			else
			{
				vector = Vector3.forward;
			}
		}
		return vector;
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x0000E218 File Offset: 0x0000C418
	private static Macros.Macro FindClosestFrame(Vector3 posA, Vector3 posB, Vector3 posC, float valD, out int index, out float valE)
	{
		Macros.Macro result = null;
		index = 0;
		valE = float.NegativeInfinity;
		float num = Mathf.Max(0.5f, Settings.AutoBranchMatchRadius);
		foreach (Macros.Macro item in Macros.macros)
		{
			bool flag = item.Frames == null || item.Frames.Count < 2;
			if (!flag)
			{
				int num2 = Mathf.Max(1, item.Frames.Count / 250);
				for (int i = 0; i < item.Frames.Count - 1; i += num2)
				{
					Vector3 val = item.MoveDirs[i];
					bool flag2 = val == Vector3.zero;
					if (!flag2)
					{
						float num3 = Vector3.Distance(posA, item.Frames[i].playerPos);
						bool flag3 = num3 > num;
						if (!flag3)
						{
							float num4 = Vector3.Dot(posB, val);
							bool flag4 = num4 >= 0.2f;
							if (flag4)
							{
								float num5 = Macros.Distance2D(num3, num4, posC, valD, val, num);
								bool flag5 = num5 > valE;
								if (flag5)
								{
									valE = num5;
									index = i;
									result = item;
								}
							}
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x060000BA RID: 186 RVA: 0x0000E3A0 File Offset: 0x0000C5A0
	private static float ScoreFrame(Macros.Macro macro, int index, Vector3 posA, Vector3 posB, Vector3 posC, float valD)
	{
		Vector3 val = default(Vector3);
		bool flag = macro == null;
		float num;
		if (flag)
		{
			num = float.NegativeInfinity;
		}
		else
		{
			bool flag2 = macro.MoveDirs == null;
			if (flag2)
			{
				num = float.NegativeInfinity;
			}
			else
			{
				bool flag3 = index < 0;
				if (flag3)
				{
					num = float.NegativeInfinity;
				}
				else
				{
					bool flag4 = index < macro.MoveDirs.Length;
					if (flag4)
					{
						val = macro.MoveDirs[index];
						bool flag5 = val == Vector3.zero;
						if (flag5)
						{
							num = float.NegativeInfinity;
						}
						else
						{
							float valE = Vector3.Distance(posA, macro.Frames[index].playerPos);
							float valF = Vector3.Dot(posB, val);
							float valG = Mathf.Max(0.5f, Settings.AutoBranchMatchRadius);
							num = Macros.Distance2D(valE, valF, posC, valD, val, valG);
						}
					}
					else
					{
						num = float.NegativeInfinity;
					}
				}
			}
		}
		return num;
	}

	// Token: 0x060000BB RID: 187 RVA: 0x0000E48C File Offset: 0x0000C68C
	private static float Distance2D(float valD, float valE, Vector3 posA, float valF, Vector3 posB, float valG)
	{
		float num = 1f - Mathf.Clamp01(valD / valG);
		float num2 = ((valF > 0.5f) ? Mathf.Max(0f, Vector3.Dot(posA, posB)) : 0f);
		return valE + num + num2 * 0.3f;
	}

	// Token: 0x060000BC RID: 188 RVA: 0x0000E4DC File Offset: 0x0000C6DC
	private static int FindFrameIndex(List<Macros.MacroFrame> frames, float valD)
	{
		int count = frames.Count;
		int num2 = count - 1;
		for (;;)
		{
			int num3 = num2;
			bool flag = num3 < 0;
			if (flag)
			{
				break;
			}
			bool flag2 = frames[num2].time > valD;
			if (!flag2)
			{
				goto IL_003E;
			}
			num2--;
		}
		return 0;
		IL_003E:
		return num2;
	}

	// Token: 0x060000BD RID: 189 RVA: 0x0000E534 File Offset: 0x0000C734
	private static Macros.MacroFrame SampleFrame(List<Macros.MacroFrame> frames, float valD)
	{
		Macros.MacroFrame @struct = default(Macros.MacroFrame);
		Macros.MacroFrame struct6_ = default(Macros.MacroFrame);
		bool flag = frames.Count != 1;
		Macros.MacroFrame macroFrame;
		if (flag)
		{
			int num2 = -1;
			int count = frames.Count;
			int num3 = count - 1;
			for (;;)
			{
				int num4 = num3;
				bool flag2 = num4 >= 0;
				if (!flag2)
				{
					goto IL_007D;
				}
				bool flag3 = frames[num3].time > valD;
				if (!flag3)
				{
					break;
				}
				num3--;
			}
			num2 = num3;
			IL_007D:
			int num5 = num2;
			bool flag4 = num5 >= 0;
			if (flag4)
			{
				int num6 = num2;
				int count2 = frames.Count;
				bool flag5 = num6 < count2 - 1;
				if (flag5)
				{
					@struct = frames[num2];
					int num7 = num2;
					struct6_ = frames[num7 + 1];
					float num8 = struct6_.time - @struct.time;
					bool flag6 = num8 <= 0f;
					if (flag6)
					{
						macroFrame = @struct;
					}
					else
					{
						float valE = Mathf.Clamp01((valD - @struct.time) / num8);
						macroFrame = Macros.LerpFrame(@struct, struct6_, valE);
					}
				}
				else
				{
					macroFrame = frames[num2];
				}
			}
			else
			{
				macroFrame = frames[0];
			}
		}
		else
		{
			macroFrame = frames[0];
		}
		return macroFrame;
	}

	// Token: 0x060000BE RID: 190 RVA: 0x0000E678 File Offset: 0x0000C878
	private static Macros.MacroFrame CaptureFrame(VRRig rig)
	{
		Macros.MacroFrame result = default(Macros.MacroFrame);
		result = default(Macros.MacroFrame);
		bool flag = GTPlayer.Instance != null;
		Macros.MacroFrame macroFrame;
		if (flag)
		{
			bool flag2 = rig.head == null;
			if (!flag2)
			{
				bool flag3 = rig.head.rigTarget != null;
				if (flag3)
				{
				}
			}
			bool flag4 = rig.leftHand == null;
			if (!flag4)
			{
				bool flag5 = !(rig.leftHand.rigTarget != null);
				if (flag5)
				{
				}
			}
			bool flag6 = rig.rightHand == null;
			if (!flag6)
			{
				bool flag7 = !(rig.rightHand.rigTarget != null);
				if (flag7)
				{
				}
			}
			bool flag8 = rig.headMesh != null;
			if (flag8)
			{
				bool flag9 = !(rig.mainSkin != null);
				if (!flag9)
				{
					bool flag10 = rig.mainSkin.bones == null;
					if (!flag10)
					{
						Transform[] array = rig.mainSkin.bones;
						result.bones = new Macros.BoneTransform[array.Length];
						int num2 = 0;
						for (;;)
						{
							bool flag11 = num2 < array.Length;
							if (!flag11)
							{
								break;
							}
							bool flag12 = !(array[num2] != null);
							if (flag12)
							{
							}
							num2++;
						}
					}
				}
				macroFrame = result;
			}
			else
			{
				bool flag13 = !(rig.mainSkin != null);
				if (!flag13)
				{
					bool flag14 = rig.mainSkin.bones == null;
					if (!flag14)
					{
						Transform[] array = rig.mainSkin.bones;
						result.bones = new Macros.BoneTransform[array.Length];
						int num2 = 0;
						for (;;)
						{
							bool flag15 = num2 < array.Length;
							if (!flag15)
							{
								break;
							}
							bool flag16 = !(array[num2] != null);
							if (flag16)
							{
							}
							num2++;
						}
					}
				}
				macroFrame = result;
			}
		}
		else
		{
			bool flag17 = rig.head == null;
			if (!flag17)
			{
				bool flag18 = rig.head.rigTarget != null;
				if (flag18)
				{
				}
			}
			bool flag19 = rig.leftHand == null;
			if (!flag19)
			{
				bool flag20 = !(rig.leftHand.rigTarget != null);
				if (flag20)
				{
				}
			}
			bool flag21 = rig.rightHand == null;
			if (!flag21)
			{
				bool flag22 = !(rig.rightHand.rigTarget != null);
				if (flag22)
				{
				}
			}
			bool flag23 = rig.headMesh != null;
			if (flag23)
			{
				bool flag24 = !(rig.mainSkin != null);
				if (!flag24)
				{
					bool flag25 = rig.mainSkin.bones == null;
					if (!flag25)
					{
						Transform[] array = rig.mainSkin.bones;
						result.bones = new Macros.BoneTransform[array.Length];
						int num2 = 0;
						for (;;)
						{
							bool flag26 = num2 < array.Length;
							if (!flag26)
							{
								break;
							}
							bool flag27 = !(array[num2] != null);
							if (flag27)
							{
							}
							num2++;
						}
					}
				}
				macroFrame = result;
			}
			else
			{
				bool flag28 = !(rig.mainSkin != null);
				if (!flag28)
				{
					bool flag29 = rig.mainSkin.bones == null;
					if (!flag29)
					{
						Transform[] array = rig.mainSkin.bones;
						result.bones = new Macros.BoneTransform[array.Length];
						int num2 = 0;
						for (;;)
						{
							bool flag30 = num2 < array.Length;
							if (!flag30)
							{
								break;
							}
							bool flag31 = !(array[num2] != null);
							if (flag31)
							{
							}
							num2++;
						}
					}
				}
				macroFrame = result;
			}
		}
		return macroFrame;
	}

	// Token: 0x060000BF RID: 191 RVA: 0x0000EA28 File Offset: 0x0000CC28
	private static void ApplyFrame(VRRig rig, Macros.MacroFrame frame, Vector3 posA)
	{
		bool flag = rig.head == null;
		if (!flag)
		{
			bool flag2 = rig.head.rigTarget != null;
			if (flag2)
			{
			}
		}
		bool flag3 = rig.leftHand == null;
		if (!flag3)
		{
			bool flag4 = rig.leftHand.rigTarget != null;
			if (flag4)
			{
			}
		}
		bool flag5 = rig.rightHand == null;
		if (!flag5)
		{
			bool flag6 = rig.rightHand.rigTarget != null;
			if (flag6)
			{
			}
		}
		bool flag7 = !(rig.headMesh != null);
		if (flag7)
		{
		}
		bool flag8 = rig.mainSkin != null;
		if (flag8)
		{
			bool flag9 = rig.mainSkin.bones == null;
			if (!flag9)
			{
				bool flag10 = frame.bones != null;
				if (flag10)
				{
					Transform[] array = rig.mainSkin.bones;
					int num3 = Mathf.Min(array.Length, frame.bones.Length);
					int num4 = 0;
					for (;;)
					{
						bool flag11 = num4 >= num3;
						if (flag11)
						{
							break;
						}
						bool flag12 = array[num4] != null;
						if (flag12)
						{
						}
						num4++;
					}
				}
			}
		}
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x0000EB6C File Offset: 0x0000CD6C
	private static Macros.MacroFrame LerpFrame(Macros.MacroFrame frame, Macros.MacroFrame frameB, float valD)
	{
		Macros.MacroFrame result = default(Macros.MacroFrame);
		result = new Macros.MacroFrame
		{
			time = Mathf.Lerp(frame.time, frameB.time, valD),
			playerPos = Vector3.Lerp(frame.playerPos, frameB.playerPos, valD),
			playerRot = Quaternion.Slerp(frame.playerRot, frameB.playerRot, valD),
			rigRootPos = Vector3.Lerp(frame.rigRootPos, frameB.rigRootPos, valD),
			rigRootRot = Quaternion.Slerp(frame.rigRootRot, frameB.rigRootRot, valD),
			headRigTargetPos = Vector3.Lerp(frame.headRigTargetPos, frameB.headRigTargetPos, valD),
			headRigTargetRot = Quaternion.Slerp(frame.headRigTargetRot, frameB.headRigTargetRot, valD),
			leftHandRigTargetPos = Vector3.Lerp(frame.leftHandRigTargetPos, frameB.leftHandRigTargetPos, valD),
			leftHandRigTargetRot = Quaternion.Slerp(frame.leftHandRigTargetRot, frameB.leftHandRigTargetRot, valD),
			rightHandRigTargetPos = Vector3.Lerp(frame.rightHandRigTargetPos, frameB.rightHandRigTargetPos, valD),
			rightHandRigTargetRot = Quaternion.Slerp(frame.rightHandRigTargetRot, frameB.rightHandRigTargetRot, valD),
			headMeshPos = Vector3.Lerp(frame.headMeshPos, frameB.headMeshPos, valD),
			headMeshRot = Quaternion.Slerp(frame.headMeshRot, frameB.headMeshRot, valD),
			headMeshScale = Vector3.Lerp(frame.headMeshScale, frameB.headMeshScale, valD)
		};
		bool flag = frame.bones == null;
		if (!flag)
		{
			bool flag2 = frameB.bones == null;
			if (!flag2)
			{
				bool flag3 = frame.bones.Length != frameB.bones.Length;
				if (!flag3)
				{
					int num2 = 0;
					for (;;)
					{
						bool flag4 = num2 >= frame.bones.Length;
						if (flag4)
						{
							break;
						}
						num2++;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x0000ED64 File Offset: 0x0000CF64
	private static void BuildPathVisual()
	{
		bool flag2 = Macros.macros.Count == 0;
		if (flag2)
		{
			Macros.UpdatePathVisual();
		}
		else
		{
			bool flag3 = Macros.pathContainer == null;
			if (flag3)
			{
				Macros.pathContainer = new GameObject("Absense_PathFinder");
				Macros.pathContainer.hideFlags = 61;
			}
			while (Macros.pathLines.Count < Macros.macros.Count)
			{
				GameObject val = new GameObject("PathLine_" + Macros.pathLines.Count.ToString());
				val.hideFlags = 61;
				val.transform.SetParent(Macros.pathContainer.transform, false);
				LineRenderer val2 = val.AddComponent<LineRenderer>();
				val2.useWorldSpace = true;
				val2.material = Macros.CreatePathMaterial();
				val2.startColor = Color.green;
				val2.endColor = Color.green;
				val2.startWidth = 0.04f;
				val2.endWidth = 0.04f;
				val2.numCapVertices = 2;
				val2.shadowCastingMode = 0;
				val2.receiveShadows = false;
				Macros.pathLines.Add(val2);
			}
			for (int i = Macros.macros.Count; i < Macros.pathLines.Count; i++)
			{
				bool flag4 = Macros.pathLines[i] != null;
				if (flag4)
				{
					GameObject gameObject = Macros.pathLines[i].gameObject;
					gameObject.SetActive(false);
				}
			}
			Macros.Macro @class = (Macros.IsSyncStarted ? Macros.activeMacro : null);
			for (int j = 0; j < Macros.macros.Count; j++)
			{
				Macros.Macro class2 = Macros.macros[j];
				LineRenderer val3 = Macros.pathLines[j];
				bool flag5 = val3 == null;
				if (!flag5)
				{
					GameObject gameObject2 = val3.gameObject;
					gameObject2.SetActive(true);
					List<Macros.MacroFrame> frames = class2.Frames;
					bool flag6 = frames != null;
					if (flag6)
					{
						int count = frames.Count;
						bool flag7 = count >= 2;
						if (flag7)
						{
							int count2 = frames.Count;
							int num = Mathf.Max(1, count2 / 300);
							int num2 = 0;
							for (int k = 0; k < frames.Count; k += num)
							{
								num2++;
							}
							val3.positionCount = num2;
							int num3 = 0;
							for (int l = 0; l < frames.Count; l += num)
							{
								val3.SetPosition(num3++, frames[l].playerPos);
							}
							bool flag = class2 == @class;
							Color val4 = (flag ? Color.green : new Color(0f, 0.5f, 0f, 0.6f));
							float num4 = (flag ? 0.07f : 0.035f);
							val3.startColor = val4;
							val3.endColor = val4;
							val3.startWidth = num4;
							val3.endWidth = num4;
							goto IL_031A;
						}
					}
					val3.positionCount = 0;
				}
				IL_031A:;
			}
		}
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x0000F0A8 File Offset: 0x0000D2A8
	private static Material CreatePathMaterial()
	{
		bool flag = !(Macros.pathMaterial == null);
		if (!flag)
		{
			Shader val = Shader.Find("Sprites/Default");
			bool flag2 = !(val == null);
			if (!flag2)
			{
				val = Shader.Find("Unlit/Color");
			}
			Macros.pathMaterial = new Material(val);
		}
		return Macros.pathMaterial;
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x0000F110 File Offset: 0x0000D310
	private static void UpdatePathVisual()
	{
		bool flag = Macros.pathContainer != null;
		if (flag)
		{
			Object.Destroy(Macros.pathContainer);
			Macros.pathContainer = null;
		}
		Macros.pathLines.Clear();
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x0000F150 File Offset: 0x0000D350
	private static void ClearPathVisual()
	{
		bool flag = Macros.IsReplaying || Macros.IsLooping || (Settings.AutoBranchSmartMode && Macros.IsSyncStarted);
		GTPlayer instance = GTPlayer.Instance;
		bool flag2 = flag && !(instance == null) && !(instance.RightHand.controllerTransform == null);
		if (flag2)
		{
			bool flag3 = Macros.ghostObject == null;
			if (flag3)
			{
				Macros.ghostObject = new GameObject("AutoBranch_HandText");
				GameObject val = Macros.ghostObject;
				val.hideFlags = 61;
				Macros.ghostLabel = Macros.ghostObject.AddComponent<TextMesh>();
				TextMesh val2 = Macros.ghostLabel;
				val2.alignment = 1;
				TextMesh val3 = Macros.ghostLabel;
				val3.anchor = 7;
				TextMesh val4 = Macros.ghostLabel;
				val4.fontSize = 28;
				Macros.ghostLabel.characterSize = 0.014f;
				TextMesh val5 = Macros.ghostLabel;
				val5.fontStyle = 1;
			}
			Transform controllerTransform = instance.RightHand.controllerTransform;
			Macros.ghostObject.transform.position = controllerTransform.position + controllerTransform.up * 0.08f;
			bool flag4 = Camera.main != null;
			if (flag4)
			{
				Macros.ghostObject.transform.rotation = Quaternion.LookRotation(Macros.ghostObject.transform.position - Camera.main.transform.position);
			}
			bool isReplaying = Macros.IsReplaying;
			if (isReplaying)
			{
				TextMesh val6 = Macros.ghostLabel;
				string text = Macros.recordingFrames.Count.ToString();
				val6.text = "● RECORDING (" + text + ")";
				Macros.ghostLabel.color = Color.red;
			}
			else
			{
				bool flag5 = Settings.AutoBranchSmartMode && Macros.IsSyncStarted;
				if (flag5)
				{
					Macros.ghostLabel.text = "◆ SMART BRANCH";
					Macros.ghostLabel.color = new Color(0.4f, 0.7f, 1f);
				}
				else
				{
					Macros.ghostLabel.text = "▶ REPLAYING";
					Macros.ghostLabel.color = Color.green;
				}
			}
			GameObject val7 = Macros.ghostObject;
			val7.SetActive(true);
		}
		else
		{
			bool flag6 = Macros.ghostObject != null;
			if (flag6)
			{
				GameObject val8 = Macros.ghostObject;
				val8.SetActive(false);
			}
		}
	}

	// Token: 0x040001D4 RID: 468
	private static readonly List<Macros.MacroFrame> recordingFrames = new List<Macros.MacroFrame>(3600);

	// Token: 0x040001D5 RID: 469
	private static readonly List<Macros.Macro> macros = new List<Macros.Macro>();

	// Token: 0x040001D6 RID: 470
	private const int MaxFrames = 7200;

	// Token: 0x040001D7 RID: 471
	private const int UploadBatch = 100;

	// Token: 0x040001D8 RID: 472
	private static volatile List<Macros.Macro> syncedMacros;

	// Token: 0x040001D9 RID: 473
	private static readonly List<KeyValuePair<string, string>> pendingUploads = new List<KeyValuePair<string, string>>();

	// Token: 0x040001DA RID: 474
	private static readonly object syncLock = new object();

	// Token: 0x040001DB RID: 475
	public static string CloudStatus = "";

	// Token: 0x040001DC RID: 476
	[CompilerGenerated]
	private static bool recording;

	// Token: 0x040001DD RID: 477
	[CompilerGenerated]
	private static bool replaying;

	// Token: 0x040001DE RID: 478
	[CompilerGenerated]
	private static bool loop;

	// Token: 0x040001DF RID: 479
	[CompilerGenerated]
	private static bool syncStarted;

	// Token: 0x040001E0 RID: 480
	private static float recordTime;

	// Token: 0x040001E1 RID: 481
	private static float replayTime;

	// Token: 0x040001E2 RID: 482
	private static List<Macros.MacroFrame> replayFrames;

	// Token: 0x040001E3 RID: 483
	private static bool paused;

	// Token: 0x040001E4 RID: 484
	private static bool movePlayer;

	// Token: 0x040001E5 RID: 485
	private static bool smartMode;

	// Token: 0x040001E6 RID: 486
	private static Macros.Macro activeMacro;

	// Token: 0x040001E7 RID: 487
	private static float replaySpeed;

	// Token: 0x040001E8 RID: 488
	private static float blendTimer;

	// Token: 0x040001E9 RID: 489
	private static Vector3 replayOrigin;

	// Token: 0x040001EA RID: 490
	private static float matchTimer;

	// Token: 0x040001EB RID: 491
	private static float lastFrameTime;

	// Token: 0x040001EC RID: 492
	private static Vector3 startOffset;

	// Token: 0x040001ED RID: 493
	private const int countA = 6;

	// Token: 0x040001EE RID: 494
	private const float valA = 0.25f;

	// Token: 0x040001EF RID: 495
	private const float valB = 0.4f;

	// Token: 0x040001F0 RID: 496
	private const float valC = 0.2f;

	// Token: 0x040001F1 RID: 497
	private static GameObject ghostObject;

	// Token: 0x040001F2 RID: 498
	private static TextMesh ghostLabel;

	// Token: 0x040001F3 RID: 499
	private static readonly List<LineRenderer> pathLines = new List<LineRenderer>();

	// Token: 0x040001F4 RID: 500
	private static GameObject pathContainer;

	// Token: 0x040001F5 RID: 501
	private static Material pathMaterial;

	// Token: 0x040001F6 RID: 502
	private const string strA = "GZ1";

	// Token: 0x040001F7 RID: 503
	private const string strB = "ABM1";

	// Token: 0x040001F8 RID: 504
	private const string strC = "0.#####";

	// Token: 0x0200005E RID: 94
	private struct BoneTransform
	{
		// Token: 0x040004E9 RID: 1257
		public Vector3 localPos;

		// Token: 0x040004EA RID: 1258
		public Quaternion localRot;

		// Token: 0x040004EB RID: 1259
		public Vector3 localScale;
	}

	// Token: 0x0200005F RID: 95
	private struct MacroFrame
	{
		// Token: 0x040004EC RID: 1260
		public float time;

		// Token: 0x040004ED RID: 1261
		public Vector3 playerPos;

		// Token: 0x040004EE RID: 1262
		public Quaternion playerRot;

		// Token: 0x040004EF RID: 1263
		public Vector3 rigRootPos;

		// Token: 0x040004F0 RID: 1264
		public Quaternion rigRootRot;

		// Token: 0x040004F1 RID: 1265
		public Vector3 headRigTargetPos;

		// Token: 0x040004F2 RID: 1266
		public Quaternion headRigTargetRot;

		// Token: 0x040004F3 RID: 1267
		public Vector3 leftHandRigTargetPos;

		// Token: 0x040004F4 RID: 1268
		public Quaternion leftHandRigTargetRot;

		// Token: 0x040004F5 RID: 1269
		public Vector3 rightHandRigTargetPos;

		// Token: 0x040004F6 RID: 1270
		public Quaternion rightHandRigTargetRot;

		// Token: 0x040004F7 RID: 1271
		public Vector3 headMeshPos;

		// Token: 0x040004F8 RID: 1272
		public Quaternion headMeshRot;

		// Token: 0x040004F9 RID: 1273
		public Vector3 headMeshScale;

		// Token: 0x040004FA RID: 1274
		public Macros.BoneTransform[] bones;
	}

	// Token: 0x02000060 RID: 96
	private class Macro
	{
		// Token: 0x040004FB RID: 1275
		public string Name;

		// Token: 0x040004FC RID: 1276
		public List<Macros.MacroFrame> Frames;

		// Token: 0x040004FD RID: 1277
		public Vector3[] MoveDirs;
	}

	// Token: 0x02000061 RID: 97
	[CompilerGenerated]
	private sealed class MacroNamePredicate
	{
		// Token: 0x06000235 RID: 565 RVA: 0x00023078 File Offset: 0x00021278
		internal bool method_0(Macros.Macro activeMacro)
		{
			return activeMacro.Name == this.name;
		}

		// Token: 0x040004FE RID: 1278
		public string name;
	}

	// Token: 0x02000062 RID: 98
	[CompilerGenerated]
	private sealed class MacroNamePredicate2
	{
		// Token: 0x06000237 RID: 567 RVA: 0x000230A4 File Offset: 0x000212A4
		internal bool method_0(Macros.Macro activeMacro)
		{
			return activeMacro.Name == this.finalName;
		}

		// Token: 0x040004FF RID: 1279
		public string finalName;
	}
}
