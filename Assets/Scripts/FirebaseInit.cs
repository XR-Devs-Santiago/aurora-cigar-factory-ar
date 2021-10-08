using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Analytics;
using Facebook.Unity;
using static Firebase.Extensions.TaskExtension;

public class FirebaseInit : MonoBehaviour
{
	public GameObject PanelPrincipal;
	public GameObject PanelLogin;
	FirebaseAuth auth;

	// Start is called before the first frame update
	void Start()
    {
	    Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
	    });
    }

    private void Awake()
	{
		auth = FirebaseAuth.DefaultInstance;

		if (!FB.IsInitialized)
        {
            FB.Init(InitCallBack, OnHideUnity);
        }
        else
        {
            FB.ActivateApp();
        }
    }

    private void InitCallBack()
    {
        if (!FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
	        Debug.Log("Fallo de inicializacion");
        }
    }

    private void OnHideUnity(bool isgameshown)
    {
        if (!isgameshown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void facebookSignIn()
    {
        var permission = new List<string>()
        {
            "public_profile", "email"
        };

        FB.LogInWithReadPermissions(permission, AuthCallBack);
    }

    private void AuthCallBack(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
	        facebookFirebaseSignIn();
        }
        else
        {
	        Debug.Log("Usuario Cancelo login");

        }
    }

    public void googleSignIn() {
		string providerId = Firebase.Auth.GoogleAuthProvider.ProviderId;

		Firebase.Auth.FederatedOAuthProvider provider = BuildFederatedOAuthProvider(providerId);
		auth.SignInWithProviderAsync(provider).ContinueWithOnMainThread(task => {
			if (task.IsCanceled) {
			    Debug.LogError("SignInWithCredentialAsync was canceled.");
			    return;
			}
			if (task.IsFaulted) {
			    Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
			    return;
			}

			Firebase.Auth.FirebaseUser newUser = task.Result.User;
			Debug.LogFormat("User signed in successfully: {0} ({1})",
				newUser.DisplayName, newUser.UserId);

			activarPanelPrincipal();
		});
	}
	
	void facebookFirebaseSignIn() {
		var accessToken = Facebook.Unity.AccessToken.CurrentAccessToken.TokenString;
		
		Firebase.Auth.Credential credential = Firebase.Auth.FacebookAuthProvider.GetCredential(accessToken);
		auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task => {
			if (task.IsCanceled) {
			    Debug.LogError("SignInWithCredentialAsync was canceled.");
			    return;
			}
			if (task.IsFaulted) {
			    Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
			    return;
			}

			Firebase.Auth.FirebaseUser newUser = task.Result;
			Debug.LogFormat("User signed in successfully: {0} ({1})",
				newUser.DisplayName, newUser.UserId);
				
			activarPanelPrincipal();
		});
	}
	
	protected Firebase.Auth.FederatedOAuthProvider BuildFederatedOAuthProvider(string providerId) {
		Firebase.Auth.FederatedOAuthProviderData data = new Firebase.Auth.FederatedOAuthProviderData();
		data.ProviderId = providerId;

		return  new Firebase.Auth.FederatedOAuthProvider(data);
	}
	
	void activarPanelPrincipal() {
		PanelPrincipal.SetActive(true);
		PanelLogin.SetActive(false);
	}
}
