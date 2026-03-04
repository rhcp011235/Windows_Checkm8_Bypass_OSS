using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Titan
{
	internal class Encrypt
	{
		public void FileEncryptAndSave(string FileNameInput, string FileNameOutput)
		{
			string images_path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Utils/");
			string filePath = Path.Combine(images_path, FileNameInput);
			byte[] data = File.ReadAllBytes(filePath);
			byte[] encryptedData = AES256Encrypt(data, EncryptionKey());
			string filePathOutput = Path.Combine(images_path, FileNameOutput);
			File.WriteAllBytes(filePathOutput, encryptedData);
		}

		public void FileDecryptAndSave(string FileNameInput, string FileNameOutput)
		{
			string filePath = Path.Combine(FileNameInput);
			byte[] data = File.ReadAllBytes(filePath);
			byte[] decryptedData = AES256Decrypt(data, EncryptionKey());
			string filePathOutput = Path.Combine(FileNameOutput);
			File.WriteAllBytes(filePathOutput, decryptedData);
		}

		public static string Decrypt_set(string ciphertext)
		{
			string key = "Ad756227f8be6f17";
			string iv = "ba7e0bb79b61f82d";
			using Aes aesAlg = Aes.Create();
			aesAlg.Key = Encoding.UTF8.GetBytes(key);
			aesAlg.IV = Encoding.UTF8.GetBytes(iv);
			ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
			using MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(ciphertext));
			using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
			using StreamReader srDecrypt = new StreamReader(csDecrypt);
			return srDecrypt.ReadToEnd();
		}

		private static byte[] AES256Encrypt(byte[] data, byte[] key)
		{
			using Aes aesAlg = Aes.Create();
			aesAlg.Key = key;
			aesAlg.Mode = CipherMode.ECB;
			aesAlg.Padding = PaddingMode.PKCS7;
			ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
			using MemoryStream msEncrypt = new MemoryStream();
			using CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
			csEncrypt.Write(data, 0, data.Length);
			csEncrypt.FlushFinalBlock();
			return msEncrypt.ToArray();
		}

		private static byte[] AES256Decrypt(byte[] data, byte[] key)
		{
			using Aes aesAlg = Aes.Create();
			aesAlg.Key = key;
			aesAlg.Mode = CipherMode.ECB;
			aesAlg.Padding = PaddingMode.PKCS7;
			ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
			using MemoryStream msDecrypt = new MemoryStream(data);
			using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
			byte[] decryptedData = new byte[data.Length];
			int decryptedLength = csDecrypt.Read(decryptedData, 0, decryptedData.Length);
			byte[] result = new byte[decryptedLength];
			Array.Copy(decryptedData, result, decryptedLength);
			return result;
		}

		private static byte[] EncryptionKey()
		{
			string encryptionKey = "ezdFI3nMFO7AJp4YqvD2rbWi4t2ghBRj";
			return Encoding.UTF8.GetBytes(encryptionKey);
		}
	}
}
