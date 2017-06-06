using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
 
public class EncryptedPlayerPrefs  {
 
    // Encrypted PlayerPrefs
    // Written by Sven Magnus
    //http://forum.unity3d.com/threads/26437-PlayerPrefs-Encryption
    // MD5 code by Matthew Wegner (from [url]http://www.unifycommunity.com/wiki/index.php?title=MD5[/url])
   
   
    //Change this key for each different game :
    private static string privateKey="F32fHYQ3Erz7qs6UUVwq";
   
    // In the gamemaster (or like) set this to a new array of 5, then fill with 8 character random strings 
	// I use http://passwordsgenerator.net/
    public static string[] keys;
   
	//Not for public use 
    private static string Md5(string strToEncrypt) {
        UTF8Encoding ue = new UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);
 
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);
 
        string hashString = "";
 
        for (int i = 0; i < hashBytes.Length; i++) {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }
 
        return hashString.PadLeft(32, '0');
    }
   //Not for public use 
    private static void SaveEncryption(string key, string type, string value) {
        int keyIndex = (int)Mathf.Floor(Random.value * keys.Length);
        string secretKey = keys[keyIndex];
        string check = Md5(key + "_" + type + "_" + privateKey + "_" + secretKey + "_" + value);
        PlayerPrefs.SetString(key + "_encryption_check", check);
        PlayerPrefs.SetInt(key + "_used_key", keyIndex);
    }
   
	/// <summary>
	/// Checks the encryption.
	/// </summary>
	/// <returns>
	/// The encryption.
	/// </returns>
	/// <param name='key'>
	/// If set to <c>true</c> key.
	/// </param>
	/// <param name='type'>
	/// If set to <c>true</c> type.
	/// </param>
	/// <param name='value'>
	/// If set to <c>true</c> value.
	/// </param>
    public static bool CheckEncryption(string key, string type, string value) {
        int keyIndex = PlayerPrefs.GetInt(key + "_used_key");
        string secretKey = keys[keyIndex];
        string check = Md5(key + "_" + type + "_" + privateKey + "_" + secretKey + "_" + value);
        if(!PlayerPrefs.HasKey(key + "_encryption_check")) return false;
        string storedCheck = PlayerPrefs.GetString(key + "_encryption_check");
        return storedCheck == check;
    }
   
	//USE THESE:
	
    public static void SetInt(string key, int value) {
        PlayerPrefs.SetInt(key, value);
        SaveEncryption(key, "int", value.ToString());
    }
   
    public static void SetFloat(string key, float value) {
        PlayerPrefs.SetFloat(key, value);
        SaveEncryption(key, "float", Mathf.Floor(value*1000).ToString());
    }
   
    public static void SetString(string key, string value) {
        PlayerPrefs.SetString(key, value);
        SaveEncryption(key, "string", value);
    }
   
    public static int GetInt(string key) {
        return GetInt(key, 0);
    }
   
    public static float GetFloat(string key) {
        return GetFloat(key, 0f);
    }
   
    public static string GetString(string key) {
        return GetString(key, "");
    }
   
    public static int GetInt(string key,int defaultValue) {
        int value = PlayerPrefs.GetInt(key);
        if(!CheckEncryption(key, "int", value.ToString())) return defaultValue;
        return value;
    }
   
    public static float GetFloat(string key, float defaultValue) {
        float value = PlayerPrefs.GetFloat(key);
        if(!CheckEncryption(key, "float", Mathf.Floor(value*1000).ToString())) return defaultValue;
        return value;
    }
   
    public static string GetString(string key, string defaultValue) {
        string value = PlayerPrefs.GetString(key);
        if(!CheckEncryption(key, "string", value)) return defaultValue;
        return value;
    }
   
	/// <summary>
	/// Determines whether this instance has value in the player pref
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance has key the specified key; otherwise, <c>false</c>.
	/// </returns>
	/// <param name='key'>
	/// If set to <c>true</c> key.
	/// </param>
    public static bool HasKey(string key) {
        return PlayerPrefs.HasKey(key);
    }
   /// <summary>
   /// Deletes a saved value from playerprefs
   /// </summary>
   /// <param name='key'>
   /// Key.
   /// </param>
    public static void DeleteKey(string key) {
        PlayerPrefs.DeleteKey(key);
        PlayerPrefs.DeleteKey(key + "_encryption_check");
        PlayerPrefs.DeleteKey(key + "_used_key");
    }
   
}