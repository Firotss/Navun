using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    void Awake()
    {
        Application.runInBackground = true;
        StartCoroutine(LocationCoroutine());
    }

    public IEnumerator LocationCoroutine()
    {
#if UNITY_ANDROID
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.CoarseLocation)) {
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.CoarseLocation);
        }

        // First, check if user has location service enabled
        if (!UnityEngine.Input.location.isEnabledByUser) {
            // TODO Failure
            Debug.LogFormat("Android and Location not enabled");
            yield break;
        }

#elif UNITY_IOS
        if (!UnityEngine.Input.location.isEnabledByUser) {
            // TODO Failure
            Debug.LogFormat("IOS and Location not enabled");
            yield break;
        }
#endif
        // Start service before querying location
        UnityEngine.Input.location.Start(1.0f, 1.0f);

        // Wait until service initializes
        int maxWait = 15;
        while (UnityEngine.Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            maxWait--;
        }

        // Editor has a bug which doesn't set the service status to Initializing. So extra wait in Editor.
#if UNITY_EDITOR
        int editorMaxWait = 15;
        while (UnityEngine.Input.location.status == LocationServiceStatus.Stopped && editorMaxWait > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            editorMaxWait--;
        }
#endif

        // Service didn't initialize in 15 seconds
        if (maxWait < 1)
        {
            // TODO Failure
            Debug.LogFormat("Timed out");
            yield break;
        }

        // Connection has failed
        if (UnityEngine.Input.location.status != LocationServiceStatus.Running)
        {
            // TODO Failure
            Debug.LogFormat("Unable to determine device location. Failed with status {0}", UnityEngine.Input.location.status);
            yield break;
        }
        else
        {
            // Debug.LogFormat("Location service live. status {0}", UnityEngine.Input.location.status);
            // // Access granted and location value could be retrieved
            // Debug.LogFormat("Location: "
            //     + UnityEngine.Input.location.lastData.latitude + " "
            //     + UnityEngine.Input.location.lastData.longitude + " "
            //     + UnityEngine.Input.location.lastData.altitude + " "
            //     + UnityEngine.Input.location.lastData.horizontalAccuracy + " "
            //     + UnityEngine.Input.location.lastData.timestamp);

            float _latitude = UnityEngine.Input.location.lastData.latitude;
            float _longitude = UnityEngine.Input.location.lastData.longitude;
            // TODO success do something with location
            if (UIManager.client != null)
                if (UIManager.client.myId != 0)
                {
                    ClientSend.Location(_latitude, _longitude);
                }
            if (UIManager.isHost == true)
            {
                ServerSend.Location(_latitude, _longitude);
            }
        }

        yield return new WaitForSecondsRealtime(5.0f);

        StartCoroutine(LocationCoroutine());
    }
}
