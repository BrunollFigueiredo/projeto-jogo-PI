using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner _runner;
    public NetworkObject playerPrefab;

    async void StartGame(GameMode mode)
    {
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Criamos um gerenciador de cena simples que já vem no Fusion Core
        var sceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>();

        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "SalaTeste",
            Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
            SceneManager = sceneManager
        });
    }
    //Configurando o Botão de Host e Client
    private void OnGUI()
    {
        if (_runner == null)
        {
            GUIStyle estilo = new GUIStyle(GUI.skin.button);
            estilo.fontSize = 40;
            if (GUI.Button(new Rect(50, 50, 400, 150), "HOST", estilo)) StartGame(GameMode.Host);
            if (GUI.Button(new Rect(50, 250, 400, 150), "CLIENT", estilo)) StartGame(GameMode.Client);
        }
    }

    // --- MÉTODOS OBRIGATÓRIOS (Copie exatamente assim) ---
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            runner.Spawn(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity, player);
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("Conectado ao Servidor!");
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        Debug.Log("Desconectado: " + reason);
    }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
}