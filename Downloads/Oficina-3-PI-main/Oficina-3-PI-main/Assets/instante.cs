using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class instante : MonoBehaviour
{
    public GameObject painelLivroUI;
    public Image imagemNoCanvas;
    public Sprite imagemDoLivro;

    // ── Configurações do zoom ─────────────────────────────────────────────────
    // Duração da animação de abertura/fechamento em segundos
    public float duracaoZoom = 0.3f;

    // Escala final do painel ao abrir (1 = tamanho normal definido no Canvas)
    // Aumente este valor para deixar o livro ainda maior, ex: 1.2f
    public float escalaFinal = 1f;
    // ──────────────────────────────────────────────────────────────────────────

    private RectTransform retTransform;
    private Coroutine animacaoAtiva;

    void Awake()
    {
        if (painelLivroUI != null)
            retTransform = painelLivroUI.GetComponent<RectTransform>();
    }

    private void OnMouseDown()
    {
        Interagir();
    }

    public void Interagir()
    {
        if (painelLivroUI == null) return;

        if (painelLivroUI.activeSelf)
            Fechar();
        else
            Abrir();
    }

    public void Abrir()
    {
        if (painelLivroUI == null) return;

        if (imagemNoCanvas != null && imagemDoLivro != null)
            imagemNoCanvas.sprite = imagemDoLivro;

        painelLivroUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (animacaoAtiva != null) StopCoroutine(animacaoAtiva);
        animacaoAtiva = StartCoroutine(AnimarZoom(Vector3.zero, Vector3.one * escalaFinal));
    }

    public void Fechar()
    {
        if (painelLivroUI == null) return;

        if (animacaoAtiva != null) StopCoroutine(animacaoAtiva);
        animacaoAtiva = StartCoroutine(AnimarZoom(retTransform != null ? retTransform.localScale : Vector3.one, Vector3.zero, fecharAoTerminar: true));
    }

    private IEnumerator AnimarZoom(Vector3 de, Vector3 ate, bool fecharAoTerminar = false)
    {
        if (retTransform == null) yield break;

        float tempo = 0f;
        retTransform.localScale = de;

        while (tempo < duracaoZoom)
        {
            tempo += Time.deltaTime;
            float t = Mathf.Clamp01(tempo / duracaoZoom);

            // Curva suave (ease-out)
            t = 1f - (1f - t) * (1f - t);

            retTransform.localScale = Vector3.Lerp(de, ate, t);
            yield return null;
        }

        retTransform.localScale = ate;

        if (fecharAoTerminar)
        {
            painelLivroUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
