using UnityEngine;
using System.Collections;

public class PainelBotao : MonoBehaviour
{
    public int idBotao;
    public Transform alvoApertado; // Objeto vazio onde o botão "entra"
    public float velocidade = 15f;

    private Vector3 posOriginal;

    void Start()
    {
        posOriginal = transform.localPosition;
    }

    public void ExecutarMovimento()
    {
        StopAllCoroutines();
        StartCoroutine(Animar());
    }
    private void OnMouseDown()
    {
        Debug.Log("O Unity detectou um clique direto no objeto: " + gameObject.name);
        // Tenta avisar o cérebro do puzzle
        var puzzle = Object.FindFirstObjectByType<Puzzlebotoes>();
        if (puzzle != null)
        {
            puzzle.TenteiPressionar(idBotao);
        }
        else
        {
            Debug.LogWarning("Nenhum Puzzlebotoes encontrado para notificar o pressionamento.");
        }
    }
    IEnumerator Animar()
    {
        if (alvoApertado == null) yield break;

        Vector3 destino = alvoApertado.localPosition;

        // Entra
        while (Vector3.Distance(transform.localPosition, destino) > 0.001f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, destino, velocidade * Time.deltaTime);
            yield return null;
        }

        // Sai
        while (Vector3.Distance(transform.localPosition, posOriginal) > 0.001f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, posOriginal, velocidade * Time.deltaTime);
            yield return null;
        }
    }
}
