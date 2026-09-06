using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

// Token: 0x02000024 RID: 36
internal static class ObfConst
{
	// Token: 0x06000105 RID: 261 RVA: 0x0001403C File Offset: 0x0001223C
	internal static int ReadInt32LE(byte[] byte_0, int int_3)
	{
		byte b = byte_0[int_3];
		byte b2 = byte_0[int_3 + 1];
		int num = (int)b | ((int)b2 << 8);
		byte b3 = byte_0[int_3 + 2];
		int num2 = num | ((int)b3 << 16);
		byte b4 = byte_0[int_3 + 3];
		return num2 | ((int)b4 << 24);
	}

	// Token: 0x06000106 RID: 262 RVA: 0x00014080 File Offset: 0x00012280
	internal static byte[] FindEmbeddedResource(int int_3, int int_4)
	{
		int int_5 = -957950007;
		int num = 0;
		byte[] array = null;
		int num2 = 0;
		string[] array2 = null;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		Stream stream = null;
		int num6;
		for (;;)
		{
			num++;
			num6 = ObfDecode.Decode(int_5) % 13;
			switch (num6 / 4)
			{
			case 0:
				switch (num6 % 4)
				{
				case 0:
					array2 = Assembly.GetExecutingAssembly().GetManifestResourceNames();
					num2 = 0;
					int_5 = -957950004 ^ ((num * (num + 1)) & 1);
					continue;
				case 1:
				{
					stream = ObfDecode.OpenResourceStream(Assembly.GetExecutingAssembly(), array2[num2]);
					bool flag = stream != null;
					if (flag)
					{
						int_5 = -957949993 ^ ((num * (num + 1)) & 1);
						continue;
					}
					int num7 = num;
					int num8 = num;
					int_5 = -957950001 ^ ((num7 * (num8 + 1)) & 1);
					continue;
				}
				case 2:
				{
					num5 = (int)stream.Length;
					array = new byte[num5];
					num3 = 0;
					int num9 = num * (num + 1);
					int_5 = -957949998 ^ (num9 & 1);
					continue;
				}
				case 3:
				{
					num4 = stream.Read(array, num3, num5 - num3);
					int num10 = num4;
					int_5 = ((num10 <= 0) ? (-957949997 ^ ((num * (num + 1)) & 1)) : (-957949995 ^ ((num * (num + 1)) & 1)));
					continue;
				}
				}
				break;
			case 1:
				switch (num6 % 4)
				{
				case 0:
					num3 += num4;
					int_5 = -957949998 ^ ((num * (num + 1)) & 1);
					continue;
				case 1:
					int_5 = ((num3 < num5) ? (-957949996 ^ ((num * (num + 1)) & 1)) : (-957949997 ^ ((num * (num + 1)) & 1)));
					continue;
				case 2:
					int_5 = ((array.Length < 8) ? (-957950001 ^ ((num * (num + 1)) & 1)) : (-957950000 ^ ((num * (num + 1)) & 1)));
					continue;
				case 3:
				{
					byte[] byte_ = array;
					bool flag2 = ObfConst.ReadInt32LE(byte_, 0) == int_3;
					if (flag2)
					{
						int num11 = num;
						int num12 = num;
						int_5 = -957950015 ^ ((num11 * (num12 + 1)) & 1);
					}
					else
					{
						int_5 = -957950001 ^ ((num * (num + 1)) & 1);
					}
					continue;
				}
				}
				break;
			case 2:
				switch (num6 % 4)
				{
				case 0:
					int_5 = ((ObfConst.ReadInt32LE(array, 4) != int_4) ? (-957950001 ^ ((num * (num + 1)) & 1)) : (-957950002 ^ ((num * (num + 1)) & 1)));
					continue;
				case 1:
					goto IL_0107;
				case 2:
					num2++;
					int_5 = -957950004 ^ ((num * (num + 1)) & 1);
					continue;
				case 3:
				{
					bool flag3 = num2 >= array2.Length;
					if (flag3)
					{
						int num13 = num * (num + 1);
						int_5 = -957950003 ^ (num13 & 1);
					}
					else
					{
						int_5 = -957949994 ^ ((num * (num + 1)) & 1);
					}
					continue;
				}
				}
				break;
			case 3:
				goto IL_0309;
			}
			break;
		}
		goto IL_0322;
		IL_0107:
		return array;
		goto IL_0322;
		IL_0309:
		bool flag4 = num6 % 4 != 0;
		if (!flag4)
		{
			return null;
		}
		IL_0322:
		throw null;
	}

	// Token: 0x06000107 RID: 263 RVA: 0x000143C0 File Offset: 0x000125C0
	internal static byte[] StripHeader(byte[] byte_0)
	{
		int num4 = byte_0.Length;
		int num5 = num4 - 8;
		byte[] array = new byte[num5];
		int num6 = 0;
		for (;;)
		{
			bool flag = num6 >= num5;
			if (flag)
			{
				break;
			}
			array[num6] = byte_0[num6 + 8];
			num6++;
		}
		return array;
	}

	// Token: 0x06000108 RID: 264
	[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
	public static extern IntPtr LoadLibraryW(string string_0);

	// Token: 0x06000109 RID: 265
	[DllImport("_f57a87b0ddccecf.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr AbsenseInitX(byte[] byte_0, int int_3, int int_4, int int_5, int int_6, int int_7, int int_8, int int_9, int int_10, int int_11);

	// Token: 0x0600010A RID: 266
	[DllImport("_f57a87b0ddccecf.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr AbsenseGet(IntPtr intptr_1, int int_3);

	// Token: 0x0600010B RID: 267 RVA: 0x00014414 File Offset: 0x00012614
	internal static void EnsureNativeLoaded()
	{
		try
		{
			bool flag = ObfConst.nativeHandle != (IntPtr)0;
			if (!flag)
			{
				byte[] array = ObfConst.FindEmbeddedResource(-930183044, -422949248);
				bool flag2 = array == null;
				if (!flag2)
				{
					string text = Path.Combine(Path.GetTempPath(), "_f57a87b0ddccecf.dll");
					IntPtr intPtr = ObfConst.LoadLibraryW(text);
					bool flag3 = intPtr == (IntPtr)0;
					if (flag3)
					{
						bool flag4 = !File.Exists(text);
						if (flag4)
						{
							File.WriteAllBytes(text, ObfConst.StripHeader(array));
						}
						intPtr = ObfConst.LoadLibraryW(text);
					}
					bool flag5 = intPtr != (IntPtr)0;
					if (flag5)
					{
						byte[] array2 = ObfConst.StripHeader(ObfConst.FindEmbeddedResource(-1366520288, -719826644));
						bool flag6 = array2 != null;
						if (flag6)
						{
							ObfConst.nativeHandle = ObfConst.AbsenseInitX(array2, array2.Length, 1004405970, 1419703100, -75195312, 1401506247, 958978394, 1410372692, 1536928801, -1020260229);
						}
					}
				}
			}
		}
		catch
		{
		}
	}

	// Token: 0x0600010C RID: 268 RVA: 0x00014534 File Offset: 0x00012734
	internal static string NativeGetString(int int_3)
	{
		int int_4 = 467442908;
		int num = 0;
		int int_5 = 0;
		int num2 = 0;
		IntPtr intPtr = 0;
		for (;;)
		{
			num++;
			int num3 = ObfCrypto.Decode(int_4) % 20;
			switch (num3 / 5)
			{
			case 0:
				switch (num3 % 5)
				{
				case 0:
				{
					int num4 = num2 * (num2 + 1);
					int_5 = -957950007 ^ (num4 & 1);
					int_4 = 467442909 ^ ((num * (num + 1)) & 1);
					continue;
				}
				case 1:
				{
					num2++;
					int num5 = ObfDecode.Decode(int_5) % 4;
					if (!true)
					{
					}
					int num7;
					switch (num5)
					{
					case 0:
						num7 = 467442854 ^ ((num * (num + 1)) & 1);
						break;
					case 1:
						num7 = 467442853 ^ ((num * (num + 1)) & 1);
						break;
					case 2:
						num7 = 467442851 ^ ((num * (num + 1)) & 1);
						break;
					case 3:
						num7 = 467442910 ^ ((num * (num + 1)) & 1);
						break;
					default:
						num7 = 467442850 ^ ((num * (num + 1)) & 1);
						break;
					}
					if (!true)
					{
					}
					int num6 = num7;
					int_4 = num6;
					continue;
				}
				case 2:
					int_4 = 467442899 ^ ((num * (num + 1)) & 1);
					continue;
				case 3:
					int_4 = 467442905 ^ ((num * (num + 1)) & 1);
					continue;
				case 4:
					int_4 = 467442899 ^ ((num * (num + 1)) & 1);
					continue;
				}
				break;
			case 1:
				switch (num3 % 5)
				{
				case 0:
					goto IL_024A;
				case 1:
					int_4 = 467442852 ^ ((num * (num + 1)) & 1);
					continue;
				case 2:
					int_4 = 467442899 ^ ((num * (num + 1)) & 1);
					continue;
				case 3:
					goto IL_0258;
				case 4:
					int_4 = 467442859 ^ ((num * (num + 1)) & 1);
					continue;
				}
				break;
			case 2:
				switch (num3 % 5)
				{
				case 0:
					int_4 = 467442899 ^ ((num * (num + 1)) & 1);
					continue;
				case 1:
					intPtr = ObfConst.AbsenseGet(ObfConst.nativeHandle, int_3);
					int_4 = ((intPtr == (IntPtr)0) ? (467442849 ^ ((num * (num + 1)) & 1)) : (467442848 ^ ((num * (num + 1)) & 1)));
					continue;
				case 2:
					int_5 = -957949993 ^ ((num2 * (num2 + 1)) & 1);
					int_4 = 467442909 ^ ((num * (num + 1)) & 1);
					continue;
				case 3:
					int_5 = -957949996 ^ ((num2 * (num2 + 1)) & 1);
					int_4 = 467442909 ^ ((num * (num + 1)) & 1);
					continue;
				case 4:
					int_4 = 467442892 ^ ((num * (num + 1)) & 1);
					continue;
				}
				break;
			case 3:
				switch (num3 % 5)
				{
				case 0:
					int_4 = 467442899 ^ ((num * (num + 1)) & 1);
					continue;
				case 1:
				{
					ObfConst.EnsureNativeLoaded();
					IntPtr intPtr2 = ObfConst.nativeHandle;
					int_4 = ((intPtr2 == (IntPtr)0) ? (467442898 ^ ((num * (num + 1)) & 1)) : (467442893 ^ ((num * (num + 1)) & 1)));
					continue;
				}
				case 2:
					int_5 = -957949994 ^ ((num2 * (num2 + 1)) & 1);
					int_4 = 467442909 ^ ((num * (num + 1)) & 1);
					continue;
				case 3:
					int_5 = -957949996 ^ ((num2 * (num2 + 1)) & 1);
					int_4 = 467442909 ^ ((num * (num + 1)) & 1);
					continue;
				case 4:
					goto IL_010E;
				}
				break;
			}
			break;
		}
		goto IL_0375;
		IL_010E:
		throw null;
		goto IL_0375;
		IL_024A:
		return Marshal.PtrToStringUni(intPtr);
		IL_0258:
		return null;
		IL_0375:
		throw null;
	}

	// Token: 0x0600010D RID: 269 RVA: 0x000148C4 File Offset: 0x00012AC4
	public static int Decode(int int_3)
	{
		return ((int_3 ^ ObfConst.DecodeXor0) + ObfConst.DecodeAdd) ^ ObfConst.DecodeXor1;
	}

	// Token: 0x04000232 RID: 562
	private static IntPtr nativeHandle;

	// Token: 0x04000233 RID: 563
	internal static int DecodeXor0 = 1220103667;

	// Token: 0x04000234 RID: 564
	internal static int DecodeAdd = 1864119945;

	// Token: 0x04000235 RID: 565
	internal static int DecodeXor1 = 958202393;
}
