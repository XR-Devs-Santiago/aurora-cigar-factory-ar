package `in`.locomotion.plugins.login

import android.app.Activity
import android.os.Build
import android.content.Intent
import android.content.Context
import androidx.appcompat.app.AppCompatActivity
import androidx.appcompat.app.AppCompatDelegate
import android.util.Log

import android.os.Bundle

import com.google.android.gms.auth.api.Auth
import com.google.android.gms.auth.api.signin.GoogleSignInAccount
import com.google.android.gms.auth.api.signin.GoogleSignInOptions
import com.google.android.gms.auth.api.signin.GoogleSignInResult
import com.google.android.gms.common.ConnectionResult
import com.google.android.gms.common.api.GoogleApiClient
import com.google.android.gms.common.api.Scope
import com.unity3d.player.UnityPlayer

class GoogleLoginKit : AppCompatActivity, GoogleApiClient.OnConnectionFailedListener {

    // Needed to get the battery level.
    private var context: Context? = null;


    // // // // // // // //
    // // Progress Bar// //
    // // // // // // // //


    constructor(context: Context) {
        this.context = context
    }

    constructor() {
        LogMessage("init()")
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        LogMessage("onCreate")

        val isSilentLogin = intent.getBooleanExtra(kSilentLogin, false)
        PrepareGoogleSignIn()

        if (isSilentLogin == false) {
            val signInIntent = Auth.GoogleSignInApi.getSignInIntent(mGoogleApiClient)
            startActivityForResult(signInIntent, RC_SIGN_IN)
        }
    }

    override fun onPause() {
        super.onPause()
        //LogMessage("onPause");
    }

    override fun onDestroy() {
        super.onDestroy()
        //LogMessage("onDestroy");
    }

    public override fun onStart() {
        super.onStart()

        val isSilentLogin = intent.getBooleanExtra(kSilentLogin, false)
        if (isSilentLogin == true) {
            val opr = Auth.GoogleSignInApi.silentSignIn(mGoogleApiClient)
            if (opr.isDone) {
                // If the user's cached credentials are valid, the OptionalPendingResult will be "done"
                // and the GoogleSignInResult will be available instantly.
                LogMessage("Got silent cached sign-in")
                val result = opr.get()
                handleSignInResult(result, 0)
            } else {
                // If the user has not previously signed in on this device or the sign-in has expired,
                // this asynchronous branch will attempt to sign in the user silently.  Cross-device
                // single sign-on will occur in this branch.
                //showProgressDialog();
                opr.setResultCallback { googleSignInResult ->
                    //hideProgressDialog();
                    handleSignInResult(googleSignInResult, 0)
                }
            }
        }
    }

    override fun onResume() {
        super.onResume()
    }

    fun PrepareGoogleSignIn() {
        LogMessage("PrepareGoogleSignIn 0: ")

        if (_googleClientID == null) {
            LogMessage("PrepareGoogleSignIn: Error: You need to initialize with web client id")
            //_googleClientID = getString(R.string.default_web_client_id);
            LogMessage("PrepareGoogleSignIn 0.2")
            return
        }

        if (_googleClientID == null || _googleClientID!!.isEmpty()) {
            LogMessage("PrepareGoogleSignIn 1. Invalid: " + _googleClientID!!)
        } else {
            LogMessage("PrepareGoogleSignIn 1: Valid:  " + _googleClientID!!.subSequence(0, 3) + "******" + _googleClientID!!.substring(_googleClientID!!.length - 4))
        }

        // [END config_signin]
        var gBuiler = GoogleSignInOptions.Builder(GoogleSignInOptions.DEFAULT_SIGN_IN)

        if (_serverScopes != null && _serverScopes!!.size > 0) {
            LogMessage("PrepareGoogleSignIn: Adding Scopes. Start")

            //https://stackoverflow.com/questions/34690310/cannot-set-scope-google-sign-in-on-android
            for (scopeString in _serverScopes!!) {
                if (scopeString.isEmpty() == false) {
                    gBuiler = gBuiler.requestScopes(Scope(scopeString)) //new Scope(Scopes.DRIVE_APPFOLDER)
                }
            }
            LogMessage("PrepareGoogleSignIn: Adding Scopes. Done")
        }

        if (_serverAuthEnabled) {
            LogMessage("PrepareGoogleSignIn: Adding ServerAuth")
            gBuiler = gBuiler.requestServerAuthCode(_googleClientID, _forceCodeForRefreshToken)
        }

        val gso = gBuiler.requestIdToken(_googleClientID).requestEmail().build()

        LogMessage("PrepareGoogleSignIn 2")

        mGoogleApiClient = GoogleApiClient.Builder(this)
                .enableAutoManage(this /* FragmentActivity */, this /* OnConnectionFailedListener */)
                .addApi(Auth.GOOGLE_SIGN_IN_API, gso)
                .build()

        LogMessage("PrepareGoogleSignIn 3")

    }

    fun GoogleRevokeAccess() {
        // Firebase sign out
        googleAccountObj = null

        // Google revoke access
        Auth.GoogleSignInApi.revokeAccess(mGoogleApiClient).setResultCallback {
            //updateUI();
        }
    }

    public override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)

        // Result returned from launching the Intent from GoogleSignInApi.getSignInIntent(...);
        if (requestCode == RC_SIGN_IN) {
            val result = Auth.GoogleSignInApi.getSignInResultFromIntent(data)
            handleSignInResult(result, resultCode)
        }
    }
    // [END onActivityResult]

    // [START handleSignInResult]
    private fun handleSignInResult(result: GoogleSignInResult, resultCode: Int) {
        LogMessage("handleSignInResult: Success? " + result.isSuccess + " : Status? " + result.status
                + " : resultCode? " + resultCode)


        if (result.isSuccess) {
            // Signed in successfully, show authenticated UI.
            googleAccountObj = result.signInAccount

            //String personName = googleAccountObj.getDisplayName();
            //String personGivenName = googleAccountObj.getGivenName();
            //String personFamilyName = googleAccountObj.getFamilyName();
            //String personEmail = googleAccountObj.getEmail();
            //String personId = googleAccountObj.getId();
            //Uri personPhoto = googleAccountObj.getPhotoUrl();

            //mStatusTextView.setText(getString(R.string.signed_in_fmt, acct.getDisplayName()));
            UnityPlayer.UnitySendMessage(gameObjectName, "LCGoogleSignInSuccess", "true")
        } else {
            // Signed out, show unauthenticated UI.
            UnityPlayer.UnitySendMessage(gameObjectName, "LCGoogleSignInFailed", "false")
        }

        finish()
    }


    override fun onConnectionFailed(connectionResult: ConnectionResult) {
        LogMessage("onConnectionFailed")
    }

    companion object {
        init {
            if (Build.VERSION.SDK_INT < 21) {
                AppCompatDelegate.setCompatVectorFromResourcesEnabled(true)
            }
        }

        private val kSilentLogin = "SilentLogin"


        private val LOGTAG = "Unity"
        private val LOG_PREFIX = "LCGoogleLoginKit"
        internal var logEnabled = false
        internal var devLogEnabled = false
        internal var _serverScopes: Array<String>? = arrayOf()

        internal var _serverAuthEnabled = false
        internal var _forceCodeForRefreshToken = false


        internal var gameObjectName = "LCGoogleLoginBridge"


        // // // // // // // //
        // // Google Sign IN //
        // // // // // // // //


        private val RC_SIGN_IN = 9107

        private var mGoogleApiClient: GoogleApiClient? = null

        internal var _googleClientID: String? = null

        @JvmStatic
        fun InitiateWithClientID(clientID: String) {
            LogMessage("InitiateWithClientID() Called")
            _googleClientID = clientID
        }

        //public static void UpdateScopes(String[] scopes) {
        // _serverScopes = scopes.clone();
        //}

        // [START signin]
        @JvmStatic
        fun UserLogin(isSilent: Boolean, enableServerAuth: Boolean,
                      forceCodeForeRefreshToken: Boolean, scopes: Array<String>?): Boolean {
            LogMessage("UserLogin() Silent?" + isSilent + "enableServerAuth?" + enableServerAuth
                    + "forceCodeForeRefreshToken?" + forceCodeForeRefreshToken)

            //Setup
            if (scopes != null && scopes.size > 0) {
                _serverScopes = scopes.clone()
            } else {
                _serverScopes = null
            }
            _serverAuthEnabled = enableServerAuth
            _forceCodeForRefreshToken = forceCodeForeRefreshToken

            //Start Activity

            val intent = Intent(unityActivity, GoogleLoginKit::class.java)
            intent.putExtra(kSilentLogin, isSilent)
            //intent.putExtra(FBUnityLoginActivity.LOGIN_TYPE, FBUnityLoginActivity.LoginType.READ);
            unityActivity.startActivity(intent)

            return true
        }

        // [END signin]

        @JvmStatic
        internal var mLogoutClient: GoogleApiClient? = null

        @JvmStatic
        fun UserLogout(): Boolean {

            mGoogleApiClient = null


            LogMessage("UserLogout 1")
            val gso = GoogleSignInOptions.Builder(GoogleSignInOptions.DEFAULT_SIGN_IN).build()

            LogMessage("UserLogout 2")
            val cxt = unityActivity.applicationContext
            mLogoutClient = GoogleApiClient.Builder(cxt).addApi(Auth.GOOGLE_SIGN_IN_API, gso).build()
            LogMessage("UserLogout 3")

            // Google sign out
            mLogoutClient!!.connect()
            mLogoutClient!!.registerConnectionCallbacks(object : GoogleApiClient.ConnectionCallbacks {
                override fun onConnected(bundle: Bundle?) {

                    if (mLogoutClient == null) {
                        return
                    }

                    if (mLogoutClient!!.isConnected) {
                        Auth.GoogleSignInApi.signOut(mLogoutClient)
                        LogMessage("UserLogout 5: Success")
                    } else {
                        LogMessage("UserLogout 5: Failed")
                    }
                    mLogoutClient = null
                }

                override fun onConnectionSuspended(i: Int) {
                    LogMessage("Google API Client Connection Suspended")
                }
            })

            LogMessage("UserLogout 4")

            googleAccountObj = null
            return true
        }

        @JvmStatic
        private var googleAccountObj: GoogleSignInAccount? = null

        //Access Data
        @JvmStatic
        fun UserAccessToken(): String? {
            return null
        }

        @JvmStatic
        fun UserIDToken(): String? {
            return if (googleAccountObj != null) {
                googleAccountObj!!.idToken
            } else null
        }

        @JvmStatic
        fun RefreshToken(): String? {
            return null
        }

        @JvmStatic
        fun UserActualID(): String? {
            return if (googleAccountObj != null) {
                googleAccountObj!!.id
            } else null
        }

        @JvmStatic
        fun ServerAuthCode(): String? {
            return if (googleAccountObj != null) {
                googleAccountObj!!.serverAuthCode
            } else null
        }

        @JvmStatic
        fun IsLoggedIn(): Boolean {
            return googleAccountObj != null
        }

        @JvmStatic
        fun AvalableScopes(): Array<String?>? {
            if (googleAccountObj == null) {
                return null
            }

            try {
                val retVal = arrayOfNulls<String>(googleAccountObj!!.grantedScopes.size)

                var i = 0
                for (scope in googleAccountObj!!.grantedScopes) {
                    retVal[i] = scope.toString()
                    i++
                }

                return retVal
            } catch (ex: Exception) {
                return null
            }

        }

        @JvmStatic
        fun UserEmail(): String? {
            return if (googleAccountObj != null) {
                //Uri
                googleAccountObj!!.email
            } else null
        }

        @JvmStatic
        fun UserPhotoUrl(): String? {
            if (googleAccountObj != null) {
                //Uri
                val uri = googleAccountObj!!.photoUrl
                if (uri != null) {
                    return uri.toString()
                }
            }
            return null
        }

        @JvmStatic
        fun UserDisplayName(): String? {
            return if (googleAccountObj != null) {
                googleAccountObj!!.displayName
            } else null
        }

        @JvmStatic
        fun ChangeLogLevel(enabled: Boolean) {
            logEnabled = enabled
        }

        @JvmStatic
        fun ChangeDevLogLevel(enabled: Boolean) {
            devLogEnabled = enabled
        }

        @JvmStatic
        internal fun LogDevMessage(message: String) {
            if (devLogEnabled) {
                Log.d(LOGTAG, "$LOG_PREFIX : $message")
            }
        }

        @JvmStatic
        internal fun LogMessage(message: String) {
            if (logEnabled) {
                Log.d(LOGTAG, "$LOG_PREFIX : $message")
            }
        }


        //////// Unity Messaging ////////

        @JvmStatic
        private val unityPlayer: Class<*>? = null

        /*
    public static void SendMessage(String unityObject, String unityMethod, String message) {
        try {
            if (unityPlayer == null) {
                unityPlayer = Class.forName("com.unity3d.player.UnityPlayer");
            }

            Method method = unityPlayer.getMethod(
                    "UnitySendMessage", String.class, String.class, String.class);
            method.invoke(unityPlayer, unityObject, unityMethod, message);
        } catch (Exception ex) {
            Log.d("TODO", ex.toString());
        }
    }
    */

        /*
        try {
            if (unityPlayer == null) {
                unityPlayer = Class.forName("com.unity3d.player.UnityPlayer");
            }

            Activity activity = (Activity) unityPlayer.getField("currentActivity").get(null);
            if (activity == null) {
                Log.d(LOGTAG, LOG_PREFIX + " Current unity activity is null");
            }
            return activity;
        } catch (Exception ex) {
            Log.d(LOGTAG, LOG_PREFIX + ex.toString());
            return null;
        }
        */

        @JvmStatic
        val unityActivity: Activity
            get() = UnityPlayer.currentActivity
    }
}

