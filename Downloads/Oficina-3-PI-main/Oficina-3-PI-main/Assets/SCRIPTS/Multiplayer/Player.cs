using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private NetworkCharacterController _cc;

    [SerializeField] private float speed = 15f;
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private float alcanceInteracao = 3f;

    [Networked] private NetworkButtons PreviousButtons { get; set; }

    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
    }

    public override void Spawned()
    {
        if (HasInputAuthority && cameraHolder != null)
        {
            cameraHolder.gameObject.SetActive(true);
        }
        else if (cameraHolder != null)
        {
            cameraHolder.gameObject.SetActive(false);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (_cc == null) return;

        if (GetInput(out NetworkInputData data))
        {
            transform.rotation = Quaternion.Euler(0, data.lookYaw, 0);

            if (cameraHolder != null)
            {
                cameraHolder.localRotation = Quaternion.Euler(data.lookPitch, 0, 0);
            }

            Vector3 moveDirection = transform.forward * data.direction.z + transform.right * data.direction.x;
            _cc.Move(speed * moveDirection * Runner.DeltaTime);

            if (data.buttons.WasPressed(PreviousButtons, InputButtons.Jump))
            {
                _cc.Jump();
            }

            if (data.buttons.WasPressed(PreviousButtons, (int)InputButtons.Fire1))
            {
                ChecarInteracao();
            }

            PreviousButtons = data.buttons;
        }
    }

    private void ChecarInteracao()
    {
        if (!HasInputAuthority) return;

        Ray ray = new Ray(cameraHolder.position, cameraHolder.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, alcanceInteracao))
        {
            instante scriptLivro = hit.collider.GetComponent<instante>();
            if (scriptLivro != null)
            {
                scriptLivro.Interagir();
                return;
            }

            PainelBotao scriptBotao = hit.collider.GetComponent<PainelBotao>();
            if (scriptBotao != null)
            {
                FindObjectOfType<Puzzlebotoes>().TenteiPressionar(scriptBotao.idBotao);
            }
        }
    }
}