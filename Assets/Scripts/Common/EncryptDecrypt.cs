using System.Collections;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Collections.Generic;
using UnityEngine;

public static class EncryptDecrypt
{
    private static string pswd = "DontReadThis245"; //FIND ANOTHER WAY TO STORE
    public static byte[] encrypt(byte[] data)
    {
        byte[] salt = new byte[4] { (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255) };
        PasswordDeriveBytes pdb = new PasswordDeriveBytes(pswd,salt);
        MemoryStream ms = new MemoryStream();
        Aes aes = new AesManaged();
        aes.Key = pdb.GetBytes(aes.KeySize / 8);
        aes.IV = pdb.GetBytes(aes.BlockSize / 8);
        CryptoStream cs = new CryptoStream(ms,aes.CreateEncryptor(), CryptoStreamMode.Write);
        cs.Write(data, 0, data.Length);
        cs.Close();
        byte[] byteData = ms.ToArray();
        byte[] byteArray = new byte[byteData.Length + 4];
        System.Array.Copy(byteData, 0, byteArray, 0, byteData.Length);
        System.Array.Copy(salt, 0, byteArray, byteData.Length, salt.Length);

        return byteArray;
    }
    public static byte[] decrypt(byte[] data)
    {
        byte[] salt = new byte[4];
        System.Array.Copy(data, data.Length-4, salt, 0, 4);
        PasswordDeriveBytes pdb = new PasswordDeriveBytes(pswd, salt);
        MemoryStream ms = new MemoryStream();
        Aes aes = new AesManaged();
        aes.Key = pdb.GetBytes(aes.KeySize / 8);
        aes.IV = pdb.GetBytes(aes.BlockSize / 8);
        CryptoStream cs = new CryptoStream(ms,aes.CreateDecryptor(), CryptoStreamMode.Write);
        cs.Write(data, 0, data.Length-4);
        cs.Close();
        return ms.ToArray();
    }

    public static void StoreEncryptFile(string filename, byte[] data)
    {
        FileStream fs = new FileStream(filename, FileMode.Create);

        byte[] dataEcrypted = EncryptDecrypt.encrypt(data);
        fs.Write(dataEcrypted, 0, dataEcrypted.Length);

        byte[] md5 = CalculateMD5(dataEcrypted);
        fs.Write(md5, 0, md5.Length);

        fs.Flush();
        fs.Close();
        fs.Dispose();
    }

    public static byte[] LoadDecryptFile(string filename)
    {
        FileStream fs = File.Open(filename, FileMode.Open);

        byte[] bytes = new byte[fs.Length-16];
        fs.Read(bytes, 0, (int)(fs.Length-16));

        byte[] md5 = new byte[16];
        fs.Seek((int)(fs.Length - 16), SeekOrigin.Begin);
        fs.Read(md5, 0, 16);

        fs.Flush();
        fs.Close();
        fs.Dispose();

        if(!VerifyMD5(bytes, md5)){
            Debug.LogError("File Corrupted");
            return null;
        }

        bytes = EncryptDecrypt.decrypt(bytes);

        return bytes;
    }

    public static byte[] CalculateMD5(byte[] data)
    {
        using (MD5 md5 = MD5.Create())
        {
            return md5.ComputeHash(data);
        }
    }

    public static bool VerifyMD5(byte[] data, byte[] md5Data)
    {
        bool res = false;
        using (MD5 md5 = MD5.Create())
        {
            byte[] md5Got = md5.ComputeHash(data);
            res = md5Got.SequenceEqual<byte>(md5Data);
        }
        return res;
    }
}
