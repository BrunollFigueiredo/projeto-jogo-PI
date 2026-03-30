using UnityEngine;
using Fusion; // Necessário para o Photon Fusion

public class PlayerMovement : NetworkBehaviour
{

    public float speed = 5f;

    private Rigidbody rb;
    private VirtualJoystick joystick;

    // O Spawned roda assim que o boneco aparece na rede
    public override void Spawned()
    {
        rb = GetComponent<Rigidbody>();

        // Procura o joystick na UI do seu Canvas
        // Corrigido para evitar o erro CS0176
        joystick = FindFirstObjectByType<VirtualJoystick>();

        // Se NÃO for o meu boneco (State Authority), eu desativo a física local
        // Isso impede que o boneco dos outros "caia" ou se mova no seu PC
        if (!HasStateAuthority)
        {
            rb.isKinematic = true;
        }
    }

    // O FixedUpdateNetwork é o Update da rede do Fusion
    public override void FixedUpdateNetwork()
    {
        // Só move se este objeto pertencer a quem está tocando na tela
        if (HasStateAuthority && joystick != null)
        {
            // Pegamos os valores do Joystick (-1 a 1)
            float h = joystick.Horizontal;
            float v = joystick.Vertical;

            // Criamos o vetor de movimento
            Vector3 direcao = new Vector3(h, 0, v).normalized;
            Vector3 velocidadeFinal = direcao * speed;

            // Mantemos a gravidade atual do Rigidbody
            velocidadeFinal.y = rb.linearVelocity.y;

            // Aplicamos a velocidade
            rb.linearVelocity = velocidadeFinal;
        }
    }
}