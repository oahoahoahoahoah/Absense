using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

// Token: 0x02000025 RID: 37
internal static class ObfCrypto
{
	// Token: 0x0600010E RID: 270 RVA: 0x000148EC File Offset: 0x00012AEC
	private static uint ReadUInt32LE(byte[] buffer, int offset)
	{
		return (uint)((int)buffer[offset] | ((int)buffer[offset + 1] << 8) | ((int)buffer[offset + 2] << 16) | ((int)buffer[offset + 3] << 24));
	}

	// Token: 0x0600010F RID: 271 RVA: 0x0001491B File Offset: 0x00012B1B
	private static void WriteUInt32LE(byte[] buffer, int offset, uint value)
	{
		buffer[offset] = (byte)value;
		buffer[offset + 1] = (byte)(value >> 8);
		buffer[offset + 2] = (byte)(value >> 16);
		buffer[offset + 3] = (byte)(value >> 24);
	}

	// Token: 0x06000110 RID: 272 RVA: 0x00014940 File Offset: 0x00012B40
	private static void QuarterRound(uint[] state, int a, int b, int c, int d)
	{
		state[a] += state[b];
		uint t = state[d] ^ state[a];
		state[d] = (t << 16) | (t >> 16);
		state[c] += state[d];
		t = state[b] ^ state[c];
		state[b] = (t << 12) | (t >> 20);
		state[a] += state[b];
		t = state[d] ^ state[a];
		state[d] = (t << 8) | (t >> 24);
		state[c] += state[d];
		t = state[b] ^ state[c];
		state[b] = (t << 7) | (t >> 25);
	}

	// Token: 0x06000111 RID: 273 RVA: 0x000149DC File Offset: 0x00012BDC
	private static uint[] Permute(uint[] input)
	{
		uint[] state = new uint[16];
		for (int i = 0; i < 16; i++)
		{
			state[i] = input[i];
		}
		for (int round = 0; round < 10; round++)
		{
			ObfCrypto.QuarterRound(state, 0, 4, 8, 12);
			ObfCrypto.QuarterRound(state, 1, 5, 9, 13);
			ObfCrypto.QuarterRound(state, 2, 6, 10, 14);
			ObfCrypto.QuarterRound(state, 3, 7, 11, 15);
			ObfCrypto.QuarterRound(state, 0, 5, 10, 15);
			ObfCrypto.QuarterRound(state, 1, 6, 11, 12);
			ObfCrypto.QuarterRound(state, 2, 7, 8, 13);
			ObfCrypto.QuarterRound(state, 3, 4, 9, 14);
		}
		return state;
	}

	// Token: 0x06000112 RID: 274 RVA: 0x00014A90 File Offset: 0x00012C90
	private static byte[] ChaChaBlock(uint[] input)
	{
		uint[] permuted = ObfCrypto.Permute(input);
		byte[] block = new byte[64];
		for (int i = 0; i < 16; i++)
		{
			ObfCrypto.WriteUInt32LE(block, i * 4, permuted[i] + input[i]);
		}
		return block;
	}

	// Token: 0x06000113 RID: 275 RVA: 0x00014AD8 File Offset: 0x00012CD8
	private static byte[] HChaCha20(byte[] key, byte[] nonce)
	{
		uint[] state = new uint[]
		{
			1634760805U, 857760878U, 2036477234U, 1797285236U, 0U, 0U, 0U, 0U, 0U, 0U,
			0U, 0U, 0U, 0U, 0U, 0U
		};
		for (int i = 0; i < 8; i++)
		{
			state[4 + i] = ObfCrypto.ReadUInt32LE(key, i * 4);
		}
		for (int j = 0; j < 4; j++)
		{
			state[12 + j] = ObfCrypto.ReadUInt32LE(nonce, j * 4);
		}
		uint[] permuted = ObfCrypto.Permute(state);
		byte[] subkey = new byte[32];
		ObfCrypto.WriteUInt32LE(subkey, 0, permuted[0]);
		ObfCrypto.WriteUInt32LE(subkey, 4, permuted[1]);
		ObfCrypto.WriteUInt32LE(subkey, 8, permuted[2]);
		ObfCrypto.WriteUInt32LE(subkey, 12, permuted[3]);
		ObfCrypto.WriteUInt32LE(subkey, 16, permuted[12]);
		ObfCrypto.WriteUInt32LE(subkey, 20, permuted[13]);
		ObfCrypto.WriteUInt32LE(subkey, 24, permuted[14]);
		ObfCrypto.WriteUInt32LE(subkey, 28, permuted[15]);
		return subkey;
	}

	// Token: 0x06000114 RID: 276 RVA: 0x00014BBC File Offset: 0x00012DBC
	private static void XChaCha20Xor(byte[] key, byte[] nonce24, byte[] data)
	{
		byte[] hNonce = new byte[16];
		for (int i = 0; i < 16; i++)
		{
			hNonce[i] = nonce24[i];
		}
		byte[] subkey = ObfCrypto.HChaCha20(key, hNonce);
		uint[] state = new uint[]
		{
			1634760805U, 857760878U, 2036477234U, 1797285236U, 0U, 0U, 0U, 0U, 0U, 0U,
			0U, 0U, 0U, 0U, 0U, 0U
		};
		for (int j = 0; j < 8; j++)
		{
			state[4 + j] = ObfCrypto.ReadUInt32LE(subkey, j * 4);
		}
		state[12] = 0U;
		state[13] = 0U;
		state[14] = ObfCrypto.ReadUInt32LE(nonce24, 16);
		state[15] = ObfCrypto.ReadUInt32LE(nonce24, 20);
		int pos = 0;
		uint counter = 0U;
		while (pos < data.Length)
		{
			state[12] = counter;
			byte[] keystream = ObfCrypto.ChaChaBlock(state);
			int k = 0;
			while (k < 64 && pos + k < data.Length)
			{
				data[pos + k] = data[pos + k] ^ keystream[k];
				k++;
			}
			pos += 64;
			counter += 1U;
		}
	}

	// Token: 0x06000115 RID: 277 RVA: 0x00014CC0 File Offset: 0x00012EC0
	private static byte[] DecryptMasterKey()
	{
		ICryptoTransform decryptor = new RijndaelManaged
		{
			Key = ObfCrypto.AesKey,
			IV = ObfCrypto.AesIV
		}.CreateDecryptor();
		return decryptor.TransformFinalBlock(ObfCrypto.EncryptedKey, 0, ObfCrypto.EncryptedKey.Length);
	}

	// Token: 0x06000116 RID: 278 RVA: 0x00014D0C File Offset: 0x00012F0C
	public static Stream OpenResourceStream(Assembly assembly, string resourceName)
	{
		Stream resource = assembly.GetManifestResourceStream(resourceName);
		bool flag = resource == null;
		Stream stream;
		if (flag)
		{
			stream = null;
		}
		else
		{
			int length = (int)resource.Length;
			byte[] raw = new byte[length];
			int read;
			for (int i = 0; i < length; i += read)
			{
				read = resource.Read(raw, i, length - i);
				bool flag2 = read <= 0;
				if (flag2)
				{
					break;
				}
			}
			((IDisposable)resource).Dispose();
			bool flag3 = ObfCrypto.cachedKey == null;
			if (flag3)
			{
				ObfCrypto.cachedKey = ObfCrypto.DecryptMasterKey();
			}
			byte[] nonce = new byte[24];
			for (int j = 0; j < 24; j++)
			{
				nonce[j] = raw[j];
			}
			byte[] payload = new byte[length - 24];
			for (int k = 0; k < length - 24; k++)
			{
				payload[k] = raw[24 + k];
			}
			ObfCrypto.XChaCha20Xor(ObfCrypto.cachedKey, nonce, payload);
			stream = new MemoryStream(payload);
		}
		return stream;
	}

	// Token: 0x06000117 RID: 279 RVA: 0x00014E14 File Offset: 0x00013014
	public static int Decode(int state)
	{
		return ((state ^ ObfCrypto.DecodeXor0) + ObfCrypto.DecodeAdd) ^ ObfCrypto.DecodeXor1;
	}

	// Token: 0x04000236 RID: 566
	private static readonly byte[] AesKey = new byte[]
	{
		152, 149, 39, 36, 35, 96, 57, 141, 115, 112,
		50, 35, 126, 27, 172, 210, 212, 90, 149, 245,
		18, 80, 137, 30, 207, 203, 252, 102, 43, 70,
		16, 160
	};

	// Token: 0x04000237 RID: 567
	private static readonly byte[] AesIV = new byte[]
	{
		109, 132, 12, 160, 79, 189, 253, 56, 25, 5,
		162, 97, 69, 163, 76, 117
	};

	// Token: 0x04000238 RID: 568
	private static readonly byte[] EncryptedKey = new byte[]
	{
		217, 20, 185, 197, 31, 126, 112, 136, 15, 182,
		125, 92, 80, 213, 254, 194, 191, 219, 140, 226,
		137, 239, 44, 251, 100, 126, 219, 134, 49, 80,
		153, 75, 80, 158, 108, 85, 8, 241, 1, 251,
		7, 7, 11, 176, 73, 44, 117, 140
	};

	// Token: 0x04000239 RID: 569
	private static byte[] cachedKey;

	// Token: 0x0400023A RID: 570
	private static readonly int DecodeXor0 = 148964451;

	// Token: 0x0400023B RID: 571
	private static readonly int DecodeAdd = 1407527254;

	// Token: 0x0400023C RID: 572
	private static readonly int DecodeXor1 = 1730331157;
}
