using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using LCGoogleLogin;

public class LCGoogleLoginBridge : MonoBehaviour {

	#region Lifecycle

	private static LCGoogleLoginBridge instance;

	/** 
	 * InitWithClientID() basics
	 * Its safe to pass null here if both of following values are getting picked up properly by google services files.
	 * iOS > client ID automatically picked from GoogleService-Info.plist so any value passed won't have any effect
	 * Android > you need to use web client id, it will be picked up from R.string.default_web_client_id if set by any library. Firebase does set default_web_client_id 
	 * in a file located @ Plugins/Android/Firebase/res/values/google-services.xml 
	 **/
	public string webClientID = "789880338215-3hl813k6cvgdhenkvfsrob2vesp5a86h.apps.googleusercontent.com";
	public static void InitWithClientIDDefault(string webClientID){
		InitWithClientID (webClientID, null);
	}

	// iOS: webclient id could be passed null if no server auth is used
	// Android: webClientId must be passed & iOS client id is ignored internally
	public static void InitWithClientID(string webClientID, string iOSClientId){
		if (instance == null) { 
			instance = FindObjectOfType( typeof(LCGoogleLoginBridge) ) as LCGoogleLoginBridge;
			if(instance == null) {
				instance = new GameObject("LCGoogleLoginBridge").AddComponent<LCGoogleLoginBridge>();

				if (string.IsNullOrEmpty (webClientID)) {
					Debug.LogError ("LCGoogleLoginBridge: InitWithClientID: Google Web Client ID is required");
					return;
				}

#if UNITY_ANDROID
				LCGoogleLoginAndroid.InitiateWithClientID(webClientID);
#elif UNITY_IOS
				LCGoogleLoginiOS.initiateWithClientID (webClientID, iOSClientId);
#else
				if (debugLogs) {
						Debug.Log ("LCGoogleLogin: LoginUserBasic: Unsupported platform");
					}
#endif
			}
		}
		isInitializedAndReady = true;
	}

	//Set Logs to true, if you want to see logs
	static bool debugLogs = false;
	public static bool isInitializedAndReady = false;

	//We need this so that native code has an object to send messages to
	static LCGoogleLoginBridge SharedInstance()
	{
		if(instance == null) {
			//InitWithClientIDDefault ();
		}
		return instance;
	}

	void Awake() {
		// Set the name to allow UnitySendMessage to find this object.
		name = "LCGoogleLoginBridge";
		// Make sure this GameObject persists across scenes
		DontDestroyOnLoad(transform.gameObject);
	}
	#endregion




	#region Native library : Login & Logout

	static Action<bool> authCallback;

	public static bool LoginUser(Action<bool> callback, bool isSilent, bool enableServerAuth = false, bool forceCodeForeRefreshToken = false, List<string> requestedScopes = null){
		LCGoogleLoginBridge.SharedInstance ();
		authCallback = callback;
		string[] strScopesArray = null;
		if (requestedScopes == null || requestedScopes.Count <= 0) {
			strScopesArray = new string[0];
		} else {
			strScopesArray = requestedScopes.ToArray();
		}

#if UNITY_ANDROID
		return LCGoogleLoginAndroid.CallAndroidLoginMethod(LCGoogleLoginAndroid.kUserLogin, isSilent, enableServerAuth,
				forceCodeForeRefreshToken, strScopesArray);
#elif UNITY_IOS
		int noOfScopes = (strScopesArray == null) ? 0 : strScopesArray.Length;
			return LCGoogleLoginiOS.userLogin (isSilent, enableServerAuth, forceCodeForeRefreshToken, strScopesArray, noOfScopes);
		
#else
			if (debugLogs) {
				Debug.Log ("LCGoogleLogin: LoginUser: Unsupported platform");
			}

			if (authCallback != null) {
				authCallback (false);
			}
		return false;
#endif
	}

	public static bool LogoutUser(){
		LCGoogleLoginBridge.SharedInstance ();
		PlayerPrefs.SetInt (kLCGoogleLoginSessionExists, 0);

#if UNITY_ANDROID
		return LCGoogleLoginAndroid.CallAndroidBoolMethod(LCGoogleLoginAndroid.kUserLogout);
#elif UNITY_IOS
		return LCGoogleLoginiOS.userLogout ();
#else
			if (debugLogs) {
				Debug.Log ("LCGoogleLogin: LogoutUser: Unsupported platform");
			}
		return false;
#endif
	}
	#endregion




	#region Native library : Access Data like access token etc

	//Profile Access
	public static string GSIUserName()
	{
		LCGoogleLoginBridge.SharedInstance ();

#if UNITY_ANDROID
		return LCGoogleLoginAndroid.CallAndroidStringMethod(LCGoogleLoginAndroid.kStrUserDisplayName);
#elif UNITY_IOS
				return LCGoogleLoginiOS.userDisplayName ();
#else
				if (debugLogs) {
				Debug.Log ("LCGoogleLogin: GSIUserName: Unsupported platform");
			}
		return null;
#endif
	}

	public static string GSIUserID()
	{
		LCGoogleLoginBridge.SharedInstance ();

#if UNITY_ANDROID
			return LCGoogleLoginAndroid.CallAndroidStringMethod (LCGoogleLoginAndroid.kStrUserActualID);
#elif UNITY_IOS
		return LCGoogleLoginiOS.userActualID ();
#else
			if (debugLogs) {
				Debug.Log ("LCGoogleLogin: GSIUserID: Unsupported platform");
			}
		return null;
#endif
	}

	public static string GSIEmail()
	{
		LCGoogleLoginBridge.SharedInstance ();

#if UNITY_ANDROID
		return LCGoogleLoginAndroid.CallAndroidStringMethod(LCGoogleLoginAndroid.kStrUserEmail);
#elif UNITY_IOS
		return LCGoogleLoginiOS.userEmail ();
#else
		if (debugLogs) {
				Debug.Log ("LCGoogleLogin: GSIPhotoUrl: Unsupported platform");
			}
		return null;
#endif
	}

	public static string GSIPhotoUrl()
	{
		LCGoogleLoginBridge.SharedInstance ();

#if UNITY_ANDROID
		return LCGoogleLoginAndroid.CallAndroidStringMethod(LCGoogleLoginAndroid.kStrUserPhotoUrl);
#elif UNITY_IOS
				return LCGoogleLoginiOS.userPhotoUrl ();
#else
				if (debugLogs) {
				Debug.Log ("LCGoogleLogin: GSIPhotoUrl: Unsupported platform");
			}
		return null;
#endif
	}

	public static string GSIIDUserToken()
	{
		LCGoogleLoginBridge.SharedInstance ();

#if UNITY_ANDROID
		return LCGoogleLoginAndroid.CallAndroidStringMethod(LCGoogleLoginAndroid.kStrUserIDToken);
#elif UNITY_IOS
				return LCGoogleLoginiOS.userIDToken ();
#else
				if (debugLogs) {
				Debug.Log ("LCGoogleLogin: GSIIDUserToken: Unsupported platform");
			}
		return null;
#endif
	}

	public static string GSIAccessToken()
	{
		LCGoogleLoginBridge.SharedInstance ();
#if UNITY_ANDROID
		if (debugLogs) {
			Debug.Log("LCGoogleLogin: GSIAccessToken: Always null for android. Check google docs for why so");
		}
		return LCGoogleLoginAndroid.CallAndroidStringMethod(LCGoogleLoginAndroid.kStrUserAccessToken);
#elif UNITY_IOS
				return LCGoogleLoginiOS.userAccessToken ();
#else
				if (debugLogs) {
				Debug.Log ("LCGoogleLogin: GSIIDUserToken: Unsupported platform");
			}
		return null;
#endif
	}

	public static string[] GSIGrantedScopes()
	{
		LCGoogleLoginBridge.SharedInstance ();

#if UNITY_ANDROID
		return LCGoogleLoginAndroid.CallAndroidStringArrayMethod(LCGoogleLoginAndroid.kStrArrScopes);
#elif UNITY_IOS
				IntPtr scopesArray = LCGoogleLoginiOS.avalableScopes ();
			int noScopes = LCGoogleLoginiOS.noOfAvalableScopes ();
			return LCGoogleLoginiOS.GetCListFromiOSNativeSentData (scopesArray, noScopes);
#else
				if (debugLogs) {
				Debug.Log ("LCGoogleLogin: GSIGrantedScopes: Unsupported platform");
			}
		return null;
#endif

	}

	public static string GSIRefreshToken()
	{
		LCGoogleLoginBridge.SharedInstance ();

#if UNITY_ANDROID
		if (debugLogs) {
			Debug.Log("LCGoogleLogin: GSIRefreshToken: Always null for android. Check google docs for why so");
		}
		return LCGoogleLoginAndroid.CallAndroidStringMethod(LCGoogleLoginAndroid.kStrRefreshToken);
#elif UNITY_IOS
		return LCGoogleLoginiOS.refreshToken ();
#else
		if (debugLogs) {
				Debug.Log ("LCGoogleLogin: GSIRefreshToken: Unsupported platform");
			}
		return null;
#endif
	}

	public static string GSIServerAuthCode()
	{
		LCGoogleLoginBridge.SharedInstance ();
#if UNITY_ANDROID
		return LCGoogleLoginAndroid.CallAndroidStringMethod(LCGoogleLoginAndroid.kStrServerAuthCode);
#elif UNITY_IOS
		return LCGoogleLoginiOS.serverAuthCode ();
#else
		if (debugLogs) {
				Debug.Log ("LCGoogleLogin: GSIIDUserToken: Unsupported platform");
			}
		return null;
#endif
	}


	public static bool IsLoggedIn(){
		return (GSIUserID () != null) ? true : false;
	}
#endregion


#region Past Login State
	const string kLCGoogleLoginSessionExists = "kLCGoogleLoginSessionExists";
	public static bool HavePastLoggedInSession(){
		int retInt = PlayerPrefs.GetInt (kLCGoogleLoginSessionExists, 0);
		return (retInt == 1);
	}
#endregion


#region Native library : Logging Changes

	public static bool ChangeLoggingLevel(bool enabled)
	{
		debugLogs = enabled;
#if UNITY_ANDROID
		return LCGoogleLoginAndroid.CallAndroidInBoolMethod(LCGoogleLoginAndroid.kInBoolChangeLogLevel, enabled);
#elif UNITY_IOS
		LCGoogleLoginiOS.changeLogLevel (enabled);
			return true;
#else
		if (debugLogs) {
				Debug.Log ("LCGoogleLogin: ChangeLoggingLevel: Unsupported platform");
			}
		return false;
#endif
	}


	static bool ChangeLoggingDevLevel(bool enabled)
	{
		SharedInstance ();

#if UNITY_ANDROID
		return LCGoogleLoginAndroid.CallAndroidInBoolMethod(LCGoogleLoginAndroid.kInBoolChangeDevLogLevel, enabled);

#elif UNITY_IOS
		LCGoogleLoginiOS.changeDevLogLevel (enabled);
			return true;
#else
		if (debugLogs) {
				Debug.Log ("LCGoogleLogin: ChangeLoggingDevLevel: Unsupported platform");
			}
		return false;
#endif
	}
#endregion


	// Callback from native client libraries

#region Callbacks from Native iOS / Android library

	public void LCGoogleSignInSuccess( string args ) {
		PlayerPrefs.SetInt (kLCGoogleLoginSessionExists, 1);

		if (debugLogs) {
			Debug.Log ("LCGCAuthenticated: Static: Success: " + args);
		}
		if (authCallback != null) {
			authCallback (true);
			authCallback = null;
		}
	}

	public void LCGoogleSignInFailed( string args ) {
		if (debugLogs) {
			Debug.Log ("LCGCAuthenticated: Static: Failed: " + args);
		}
		if (authCallback != null) {
			authCallback (false);
			authCallback = null;
		}
	}

	public void LCGoogleSignedOut( string args ) {
		if (debugLogs) {
			Debug.Log ("LCGoogleSignedOut1: Static: " + args);
		}
		if(authCallback != null){
			if (args == "true") {
				//authCallback (true);
				//authCallback = null;
			} else {
			}
			//authCallback (false);
			//authCallback = null;
		}
	}


	public void LCGoogleSignedOut() {
		if (debugLogs) {
			Debug.Log ("LCGoogleSignedOut2");
		}
	}
#endregion
}
