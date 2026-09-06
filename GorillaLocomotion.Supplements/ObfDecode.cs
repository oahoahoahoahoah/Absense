using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

// Token: 0x02000026 RID: 38
internal static class ObfDecode
{
	// Token: 0x0600011A RID: 282 RVA: 0x00014F20 File Offset: 0x00013120
	public static uint ReadUInt32LE(byte[] byte_4, int int_3)
	{
		byte b = byte_4[int_3];
		byte b2 = byte_4[int_3 + 1];
		return (uint)((int)b | ((int)b2 << 8) | ((int)byte_4[int_3 + 2] << 16) | ((int)byte_4[int_3 + 3] << 24));
	}

	// Token: 0x0600011B RID: 283 RVA: 0x00014F53 File Offset: 0x00013153
	public static void WriteUInt32LE(byte[] byte_4, int int_3, uint uint_0)
	{
		byte_4[int_3] = (byte)uint_0;
		byte_4[int_3 + 1] = (byte)(uint_0 >> 8);
		byte_4[int_3 + 2] = (byte)(uint_0 >> 16);
		byte_4[int_3 + 3] = (byte)(uint_0 >> 24);
	}

	// Token: 0x0600011C RID: 284 RVA: 0x00014F78 File Offset: 0x00013178
	public static void QuarterRound(uint[] uint_0, int int_3, int int_4, int int_5, int int_6)
	{
		uint_0[int_3] += uint_0[int_4];
		uint num = uint_0[int_6] ^ uint_0[int_3];
		uint num2 = num << 16;
		uint num3 = num;
		uint_0[int_6] = num2 | (num3 >> 16);
		uint_0[int_5] += uint_0[int_6];
		num = uint_0[int_4] ^ uint_0[int_5];
		uint_0[int_4] = (num << 12) | (num >> 20);
		uint_0[int_3] += uint_0[int_4];
		num = uint_0[int_6] ^ uint_0[int_3];
		uint_0[int_6] = (num << 8) | (num >> 24);
		uint_0[int_5] += uint_0[int_6];
		num = uint_0[int_4] ^ uint_0[int_5];
		uint_0[int_4] = (num << 7) | (num >> 25);
	}

	// Token: 0x0600011D RID: 285 RVA: 0x00015018 File Offset: 0x00013218
	public static uint[] Permute(uint[] uint_0)
	{
		int int_ = 467442908;
		int num = 0;
		int num2 = 0;
		uint[] array = null;
		for (;;)
		{
			num++;
			switch (ObfCrypto.Decode(int_) % 7)
			{
			case 0:
				array = new uint[16];
				num2 = 0;
				int_ = 467442850 ^ ((num * (num + 1)) & 1);
				continue;
			case 1:
				array[num2] = uint_0[num2];
				num2++;
				int_ = 467442850 ^ ((num * (num + 1)) & 1);
				continue;
			case 2:
			{
				int num3 = num2;
				int_ = ((num3 < 16) ? (467442909 ^ ((num * (num + 1)) & 1)) : (467442851 ^ ((num * (num + 1)) & 1)));
				continue;
			}
			case 3:
				num2 = 0;
				int_ = 467442905 ^ ((num * (num + 1)) & 1);
				continue;
			case 4:
			{
				uint[] uint_ = array;
				ObfDecode.QuarterRound(uint_, 0, 4, 8, 12);
				uint[] uint_2 = array;
				ObfDecode.QuarterRound(uint_2, 1, 5, 9, 13);
				ObfDecode.QuarterRound(array, 2, 6, 10, 14);
				ObfDecode.QuarterRound(array, 3, 7, 11, 15);
				uint[] uint_3 = array;
				ObfDecode.QuarterRound(uint_3, 0, 5, 10, 15);
				uint[] uint_4 = array;
				ObfDecode.QuarterRound(uint_4, 1, 6, 11, 12);
				uint[] uint_5 = array;
				ObfDecode.QuarterRound(uint_5, 2, 7, 8, 13);
				ObfDecode.QuarterRound(array, 3, 4, 9, 14);
				num2++;
				int_ = 467442905 ^ ((num * (num + 1)) & 1);
				continue;
			}
			case 5:
				int_ = ((num2 < 10) ? (467442904 ^ ((num * (num + 1)) & 1)) : (467442910 ^ ((num * (num + 1)) & 1)));
				continue;
			case 6:
				return array;
			}
			break;
		}
		throw null;
	}

	// Token: 0x0600011E RID: 286 RVA: 0x000151AC File Offset: 0x000133AC
	public static byte[] ChaChaBlock(uint[] uint_0)
	{
		int int_ = 467442908;
		int num = 0;
		int num2 = 0;
		byte[] array = null;
		uint[] array2 = null;
		for (;;)
		{
			num++;
			switch (ObfCrypto.Decode(int_) % 4)
			{
			case 0:
				array2 = ObfDecode.Permute(uint_0);
				array = new byte[64];
				num2 = 0;
				int_ = 467442850 ^ ((num * (num + 1)) & 1);
				continue;
			case 1:
				ObfDecode.WriteUInt32LE(array, num2 * 4, array2[num2] + uint_0[num2]);
				num2++;
				int_ = 467442850 ^ ((num * (num + 1)) & 1);
				continue;
			case 2:
				int_ = ((num2 < 16) ? (467442909 ^ ((num * (num + 1)) & 1)) : (467442851 ^ ((num * (num + 1)) & 1)));
				continue;
			case 3:
				return array;
			}
			break;
		}
		throw null;
	}

	// Token: 0x0600011F RID: 287 RVA: 0x00015278 File Offset: 0x00013478
	public static byte[] HChaCha20(byte[] byte_4, byte[] byte_5)
	{
		int int_ = 467442908;
		int num = 0;
		int num2 = 0;
		uint[] array = null;
		for (;;)
		{
			num++;
			switch (ObfCrypto.Decode(int_) % 7)
			{
			case 0:
			{
				array = new uint[16];
				uint[] array2 = array;
				array2[0] = 1634760805U;
				array[1] = 857760878U;
				array[2] = 2036477234U;
				array[3] = 1797285236U;
				num2 = 0;
				int_ = 467442850 ^ ((num * (num + 1)) & 1);
				continue;
			}
			case 1:
				array[4 + num2] = ObfDecode.ReadUInt32LE(byte_4, num2 * 4);
				num2++;
				int_ = 467442850 ^ ((num * (num + 1)) & 1);
				continue;
			case 2:
				int_ = ((num2 < 8) ? (467442909 ^ ((num * (num + 1)) & 1)) : (467442851 ^ ((num * (num + 1)) & 1)));
				continue;
			case 3:
				num2 = 0;
				int_ = 467442905 ^ ((num * (num + 1)) & 1);
				continue;
			case 4:
				array[12 + num2] = ObfDecode.ReadUInt32LE(byte_5, num2 * 4);
				num2++;
				int_ = 467442905 ^ ((num * (num + 1)) & 1);
				continue;
			case 5:
				int_ = ((num2 < 4) ? (467442904 ^ ((num * (num + 1)) & 1)) : (467442910 ^ ((num * (num + 1)) & 1)));
				continue;
			case 6:
				goto IL_013D;
			}
			break;
		}
		throw null;
		IL_013D:
		uint[] array3 = ObfDecode.Permute(array);
		byte[] array4 = new byte[32];
		ObfDecode.WriteUInt32LE(array4, 0, array3[0]);
		ObfDecode.WriteUInt32LE(array4, 4, array3[1]);
		ObfDecode.WriteUInt32LE(array4, 8, array3[2]);
		ObfDecode.WriteUInt32LE(array4, 12, array3[3]);
		ObfDecode.WriteUInt32LE(array4, 16, array3[12]);
		ObfDecode.WriteUInt32LE(array4, 20, array3[13]);
		ObfDecode.WriteUInt32LE(array4, 24, array3[14]);
		ObfDecode.WriteUInt32LE(array4, 28, array3[15]);
		return array4;
	}

	// Token: 0x06000120 RID: 288 RVA: 0x00015458 File Offset: 0x00013658
	public static void XChaCha20Xor(byte[] byte_4, byte[] byte_5, byte[] byte_6)
	{
		int int_ = 467442908;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		byte[] array = null;
		uint num4 = 0U;
		uint[] array2 = null;
		int num5 = 0;
		byte[] byte_7 = null;
		byte[] array3 = null;
		uint num10;
		for (;;)
		{
			num++;
			int num6 = ObfCrypto.Decode(int_) % 14;
			switch (num6 / 4)
			{
			case 0:
				switch (num6 % 4)
				{
				case 0:
					array3 = new byte[16];
					num5 = 0;
					int_ = 467442850 ^ ((num * (num + 1)) & 1);
					continue;
				case 1:
					array3[num5] = byte_5[num5];
					num5++;
					int_ = 467442850 ^ ((num * (num + 1)) & 1);
					continue;
				case 2:
					int_ = ((num5 < 16) ? (467442909 ^ ((num * (num + 1)) & 1)) : (467442851 ^ ((num * (num + 1)) & 1)));
					continue;
				case 3:
				{
					byte_7 = ObfDecode.HChaCha20(byte_4, array3);
					uint[] array6 = new uint[16];
					array6[0] = 1634760805U;
					array6[1] = 857760878U;
					array2 = array6;
					uint[] array4 = array2;
					array4[2] = 2036477234U;
					array2[3] = 1797285236U;
					num5 = 0;
					int_ = 467442905 ^ ((num * (num + 1)) & 1);
					continue;
				}
				}
				break;
			case 1:
				switch (num6 % 4)
				{
				case 0:
					array2[4 + num5] = ObfDecode.ReadUInt32LE(byte_7, num5 * 4);
					num5++;
					int_ = 467442905 ^ ((num * (num + 1)) & 1);
					continue;
				case 1:
				{
					int num7 = num5;
					int_ = ((num7 < 8) ? (467442904 ^ ((num * (num + 1)) & 1)) : (467442910 ^ ((num * (num + 1)) & 1)));
					continue;
				}
				case 2:
				{
					array2[12] = 0U;
					array2[13] = 0U;
					array2[14] = ObfDecode.ReadUInt32LE(byte_5, 16);
					uint[] array5 = array2;
					array5[15] = ObfDecode.ReadUInt32LE(byte_5, 20);
					num2 = 0;
					num4 = 0U;
					int_ = 467442848 ^ ((num * (num + 1)) & 1);
					continue;
				}
				case 3:
					array2[12] = num4;
					array = ObfDecode.ChaChaBlock(array2);
					num3 = 0;
					int_ = 467442853 ^ ((num * (num + 1)) & 1);
					continue;
				}
				break;
			case 2:
				switch (num6 % 4)
				{
				case 0:
					byte_6[num2 + num3] = byte_6[num2 + num3] ^ array[num3];
					num3++;
					int_ = 467442853 ^ ((num * (num + 1)) & 1);
					continue;
				case 1:
				{
					int num8 = num3;
					int_ = ((num8 >= 64) ? (467442859 ^ ((num * (num + 1)) & 1)) : (467442858 ^ ((num * (num + 1)) & 1)));
					continue;
				}
				case 2:
					int_ = ((num2 + num3 < byte_6.Length) ? (467442852 ^ ((num * (num + 1)) & 1)) : (467442859 ^ ((num * (num + 1)) & 1)));
					continue;
				case 3:
					num2 += 64;
					num4 += 1U;
					int_ = 467442848 ^ ((num * (num + 1)) & 1);
					continue;
				}
				break;
			case 3:
			{
				uint num9 = (uint)(num6 % 4);
				num10 = num9;
				if (num10 != 0U)
				{
					goto Block_2;
				}
				int_ = ((num2 < byte_6.Length) ? (467442911 ^ ((num * (num + 1)) & 1)) : (467442849 ^ ((num * (num + 1)) & 1)));
				continue;
			}
			}
			break;
		}
		goto IL_032B;
		Block_2:
		if (num10 == 1U)
		{
			return;
		}
		IL_032B:
		throw null;
	}

	// Token: 0x06000121 RID: 289 RVA: 0x0001579C File Offset: 0x0001399C
	public static byte[] DecryptMasterKey()
	{
		ICryptoTransform cryptoTransform = new RijndaelManaged
		{
			Key = ObfDecode.AesKey,
			IV = ObfDecode.AesIV
		}.CreateDecryptor();
		return cryptoTransform.TransformFinalBlock(ObfDecode.EncryptedKey, 0, ObfDecode.EncryptedKey.Length);
	}

	// Token: 0x06000122 RID: 290 RVA: 0x000157E8 File Offset: 0x000139E8
	public static Stream OpenResourceStream(Assembly assembly_0, string string_0)
	{
		int int_ = 467442908;
		int num = 0;
		byte[] array = null;
		int num2 = 0;
		byte[] array2 = null;
		int num3 = 0;
		byte[] array3 = null;
		int num4 = 0;
		int num5 = 0;
		Stream stream = null;
		for (;;)
		{
			num++;
			int num6 = ObfCrypto.Decode(int_) % 15;
			switch (num6 / 4)
			{
			case 0:
				switch (num6 % 4)
				{
				case 0:
					stream = ObfCrypto.OpenResourceStream(assembly_0, string_0);
					int_ = ((stream != null) ? (467442850 ^ ((num * (num + 1)) & 1)) : (467442909 ^ ((num * (num + 1)) & 1)));
					continue;
				case 1:
					goto IL_0301;
				case 2:
					num3 = (int)stream.Length;
					array2 = new byte[num3];
					num4 = 0;
					int_ = 467442905 ^ ((num * (num + 1)) & 1);
					continue;
				case 3:
					num5 = stream.Read(array2, num4, num3 - num4);
					int_ = ((num5 <= 0) ? (467442910 ^ ((num * (num + 1)) & 1)) : (467442904 ^ ((num * (num + 1)) & 1)));
					continue;
				}
				break;
			case 1:
				switch (num6 % 4)
				{
				case 0:
					num4 += num5;
					int_ = 467442905 ^ ((num * (num + 1)) & 1);
					continue;
				case 1:
					int_ = ((num4 < num3) ? (467442851 ^ ((num * (num + 1)) & 1)) : (467442910 ^ ((num * (num + 1)) & 1)));
					continue;
				case 2:
					((IDisposable)stream).Dispose();
					int_ = ((ObfDecode.cachedKey != null) ? (467442852 ^ ((num * (num + 1)) & 1)) : (467442911 ^ ((num * (num + 1)) & 1)));
					continue;
				case 3:
					ObfDecode.cachedKey = ObfDecode.DecryptMasterKey();
					int_ = 467442852 ^ ((num * (num + 1)) & 1);
					continue;
				}
				break;
			case 2:
				switch (num6 % 4)
				{
				case 0:
					array3 = new byte[24];
					num2 = 0;
					int_ = 467442858 ^ ((num * (num + 1)) & 1);
					continue;
				case 1:
					array3[num2] = array2[num2];
					num2++;
					int_ = 467442858 ^ ((num * (num + 1)) & 1);
					continue;
				case 2:
					int_ = ((num2 < 24) ? (467442853 ^ ((num * (num + 1)) & 1)) : (467442859 ^ ((num * (num + 1)) & 1)));
					continue;
				case 3:
				{
					int num7 = num3;
					array = new byte[num7 - 24];
					num2 = 0;
					int_ = 467442849 ^ ((num * (num + 1)) & 1);
					continue;
				}
				}
				break;
			case 3:
				switch (num6 % 4)
				{
				case 0:
					array[num2] = array2[24 + num2];
					num2++;
					int_ = 467442849 ^ ((num * (num + 1)) & 1);
					continue;
				case 1:
					int_ = ((num2 < num3 - 24) ? (467442848 ^ ((num * (num + 1)) & 1)) : (467442854 ^ ((num * (num + 1)) & 1)));
					continue;
				case 2:
					goto IL_00C4;
				}
				break;
			}
			break;
		}
		goto IL_0308;
		IL_00C4:
		ObfDecode.XChaCha20Xor(ObfDecode.cachedKey, array3, array);
		return new MemoryStream(array);
		goto IL_0308;
		IL_0301:
		return null;
		IL_0308:
		throw null;
	}

	// Token: 0x06000123 RID: 291 RVA: 0x00015B0C File Offset: 0x00013D0C
	public static int Decode(int int_3)
	{
		return ((int_3 ^ ObfDecode.DecodeXor0) + ObfDecode.DecodeAdd) ^ ObfDecode.DecodeXor1;
	}

	// Token: 0x0400023D RID: 573
	private static byte[] AesKey = new byte[]
	{
		219, 244, 241, 118, 89, 125, 234, 101, 50, 169,
		15, 191, 167, 231, 210, 72, 90, 70, 164, 152,
		142, 196, 35, 68, 23, 212, 230, 63, 126, 57,
		117, 83
	};

	// Token: 0x0400023E RID: 574
	private static byte[] AesIV = new byte[]
	{
		225, 194, 206, 184, 202, 165, 243, 9, 37, 252,
		76, 227, 214, 107, 38, 48
	};

	// Token: 0x0400023F RID: 575
	private static byte[] EncryptedKey = new byte[]
	{
		159, 155, 94, 77, 234, 13, 71, 188, 138, 79,
		50, 44, 195, 13, 59, 114, 230, 162, 15, 85,
		145, 37, 237, 125, 106, 12, 229, 122, 109, 32,
		39, 68, 218, 218, 145, 168, 198, 56, 159, 34,
		33, 0, 169, 27, 174, 175, 17, 191
	};

	// Token: 0x04000240 RID: 576
	private static byte[] cachedKey;

	// Token: 0x04000241 RID: 577
	internal static int DecodeXor0 = 620067302;

	// Token: 0x04000242 RID: 578
	internal static int DecodeAdd = 1397563513;

	// Token: 0x04000243 RID: 579
	internal static int DecodeXor1 = 895536808;
}
