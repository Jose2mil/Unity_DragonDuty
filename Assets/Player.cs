using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float altura;
    private float velocidadAvance = 10;
    private float velocidadRotacion = 120;
    private float fuerzaSalto = 40;
    private Rigidbody rigidbody;
    private Animator animator;

    private bool saltando = false;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        altura = GetComponent<BoxCollider>().bounds.size.y;
        Debug.Log(altura);
        Debug.Log(altura / 2 + 0.05f);
    }


    void Update()
    {
        float rotacion = Input.GetAxis("Horizontal") * velocidadRotacion * Time.deltaTime;
        float avance = Input.GetAxis("Vertical") * velocidadAvance * Time.deltaTime;

        transform.Rotate(Vector3.up, rotacion, Space.Self);
        transform.position += transform.forward * avance;


        if (avance > 0)
        {
            Debug.Log("Caminando");
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                animator.speed = 0;
                animator.speed = 1;
            }
            animator.SetBool("Walk Forward", true);
        }

        else
        {
            animator.SetBool("Walk Forward", false);
        }

        float jump = Input.GetAxis("Jump");


        if (!saltando && jump > 0)
        {
            if (TocandoSuelo())
            {
                rigidbody.AddForce(transform.up * fuerzaSalto);
                animator.SetTrigger("Jump");
                saltando = true;
            }
        }

        else if (saltando && TocandoSuelo())
        {
            saltando = false;
            animator.speed = 0;
            animator.speed = 1;
            animator.SetTrigger("Touch Ground");
        }
    }

    private bool TocandoSuelo()
    {
        RaycastHit impacto;
        bool tocandoSuelo = Physics.Raycast(
                                transform.position,
                                (-transform.up),
                                out impacto,
                                altura / 2 + 0.001f
                                );

        if (tocandoSuelo && impacto.collider.CompareTag("Plataforma"))
            return true;

        return false;
    }
}
