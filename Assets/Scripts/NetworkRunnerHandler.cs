//using UnityEngine;
//using Fusion;
//using Fusion.Sockets;
//using System.Collections.Generic;
//using System;
//using UnityEngine.SceneManagement;

//public class NetworkRunnerHandler : MonoBehaviour, INetworkRunnerCallbacks
//{
//    [Header("Setup")]
//    public NetworkObject playerPrefab;

//    private NetworkRunner _runner;

//    async void Start()
//    {
//        _runner = gameObject.AddComponent<NetworkRunner>();
//        _runner.ProvideInput = true;

//        await _runner.StartGame(new StartGameArgs()
//        {
//            GameMode = GameMode.Shared,
//            SessionName = "TestRoom",
//            Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
//            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
//        });
//    }

//    // --- אירועים מרכזיים (לוגיקה) ---

//    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
//    {
//        if (player == runner.LocalPlayer)
//        {
//            runner.Spawn(playerPrefab, Vector3.zero, Quaternion.identity, player);
//        }
//    }

//    // --- טיפול בקלט (כאן השינוי הגדול) ---

//    public void OnInput(NetworkRunner runner, NetworkInput input)
//    {
//        var data = new NetworkInputData();

//        // מיפוי כפתורים למבנה הרשת
//        if (Input.GetKey(KeyCode.R))
//            data.buttons.Set(MyButtons.Boost, true);

//        if (Input.GetKey(KeyCode.Space))
//            data.buttons.Set(MyButtons.Repel, true);

//        // אם תרצה להוסיף תזוזה בעתיד דרך Fusion Input:
//        // data.movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

//        input.Set(data);
//    }

//    // --- תיקוני גרסאות קריטיים ---

//    // תיקון 1: OnConnectRequest
//    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

//    // תיקון 2: OnConnectFailed
//    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

//    // תיקון 3: ReliableKey ב-DataReceived
//    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }

//    // תיקון 4: ReliableKey ב-DataProgress
//    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }


//    // --- שאר הממשק (Boilerplate) ---
//    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
//    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
//    public void OnConnectedToServer(NetworkRunner runner) { }
//    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
//    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
//    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
//    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
//    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
//    public void OnSceneLoadDone(NetworkRunner runner) { }
//    public void OnSceneLoadStart(NetworkRunner runner) { }
//    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
//    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
//    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
//}

//// --- מבני נתונים חיצוניים (חובה שיהיו כאן כדי שהסקריפטים האחרים יכירו אותם) ---

//public struct NetworkInputData : INetworkInput
//{
//    public NetworkButtons buttons;
//    // public Vector2 movementInput; // לשימוש עתידי אם תרצה
//}

//public enum MyButtons
//{
//    Boost = 0, // כפתור 0 ברשת מייצג בוסט (R)
//    Repel = 1  // כפתור 1 ברשת מייצג הדיפה (Space)
//}


using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class NetworkRunnerHandler : MonoBehaviour, INetworkRunnerCallbacks
{
    [Header("Setup")]
    public NetworkObject playerPrefab;

    private NetworkRunner _runner;

    async void Start()
    {
        // --- תיקון קריטי: מניעת ניתוק במעבר סצנה ---
        // מוודא שהאובייקט הזה (שמחזיק את החיבור) לא נמחק כשטוענים שלב חדש
        DontDestroyOnLoad(gameObject);

        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = "TestRoom", // וודא שכולם נכנסים לאותו חדר
            Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    // --- אירועים ולוגיקה ---

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (player == runner.LocalPlayer)
        {
            runner.Spawn(playerPrefab, Vector3.zero, Quaternion.identity, player);
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();

        // מיפוי כפתורים ל-Fusion
        if (Input.GetKey(KeyCode.R))
            data.buttons.Set(MyButtons.Boost, true);

        if (Input.GetKey(KeyCode.Space))
            data.buttons.Set(MyButtons.Repel, true);

        input.Set(data);
    }

    // --- ממשק Fusion (Boilerplate) ---
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
}

// --- הגדרות חיצוניות (כדי למנוע שגיאות כפילות) ---
// וודא שאין לך קובץ אחר בשם NetworkInputData.cs שמגדיר את זה שוב!
public struct NetworkInputData : INetworkInput
{
    public NetworkButtons buttons;
}

public enum MyButtons
{
    Boost = 0,
    Repel = 1
}