using Fusion;
using UnityEngine;

public class Puzzlebotoes : NetworkBehaviour
{

    public int[] sequenciaCorreta = { 2, 1, 3, 4 };

    [Networked] private int indiceAtual { get; set; }


    public PainelBotao[] listaDeBotoes;

    public void TenteiPressionar(int idDoBotaoClicado)
    {
        if (HasStateAuthority == false) return;

        // FEEDBACK NO CONSOLE: Para saber se o toque chegou aqui
        Debug.Log("<color=blue>Toque detectado no Botão ID: </color>" + idDoBotaoClicado);

        if (idDoBotaoClicado == sequenciaCorreta[indiceAtual])
        {
            Debug.Log("<color=green>Acertou o passo: </color>" + (indiceAtual + 1));
            RPC_AnimarBotao(idDoBotaoClicado, Color.white); // Cor normal ao apertar
            indiceAtual++;

            if (indiceAtual >= sequenciaCorreta.Length)
            {
                Debug.Log("<color=gold>PUZZLE RESOLVIDO!</color>");
                RPC_FinalizarPuzzle(Color.green); // TODOS VERDES
                indiceAtual = 0;
            }
        }
        else
        {
            Debug.Log("<color=red>ERROU A SEQUÊNCIA! Resetando...</color>");
            RPC_FinalizarPuzzle(Color.red); // TODOS VERMELHOS
            indiceAtual = 0;
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_AnimarBotao(int id, Color cor)
    {
        foreach (var btn in listaDeBotoes)
        {
            if (btn != null && btn.idBotao == id)
            {
                btn.ExecutarMovimento();
                break;
            }
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_FinalizarPuzzle(Color corFinal)
    {
        foreach (var btn in listaDeBotoes)
        {
            if (btn != null)
            {
                // Muda a cor do material do botão
                btn.GetComponent<MeshRenderer>().material.color = corFinal;
            }
        }
    }
}
