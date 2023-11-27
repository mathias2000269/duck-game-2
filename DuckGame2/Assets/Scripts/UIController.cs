using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public ControlJugador Player1, Player2;
    public int numeroDeBalasJ1, numeroDeBalasJ2;

    public Image[] balasJ1, balasJ2;

    public Image[] VidasJ1, VidasJ2;
    private int VidaJugador1;
    private int VidaJugador2;


    private Image Player1Vida1, Player1Vida2, Player1Vida3;

    private Image Player1Bala1, Player1Bala2, Player1Bala3, Player1Bala4, Player1Bala5;



    private Image Player2Vida1,Player2Vida2, Player2Vida3;

    private Image Player2Bala1, Player2Bala2, Player2Bala3, Player2Bala4, Player2Bala5;

    // Start is called before the first frame update
    void Start()
    {
        VidaJugador1 = Player1.vida;
        VidaJugador2 = Player2.vida;
    }

    // Update is called once per frame
    void Update()
    {
        if(Player1.principalEnMano != null)
        {
            numeroDeBalasJ1 = Player1.principalEnMano.GetComponent<Objeto>().numUsos;
        }
        else
        {
            numeroDeBalasJ1 = 0;
        }

        if(Player2.principalEnMano != null)
        {
            numeroDeBalasJ2 = Player2.principalEnMano.GetComponent<Objeto>().numUsos;
        }
        else
        {
            numeroDeBalasJ2 = 0;
        }



        if(Input.GetKeyDown(KeyCode.P))
        {
            ActualizarNumeroBalas(1);
            ActualizarNumeroBalas(2);

            ActualizarNumeroVidas(1);
            ActualizarNumeroVidas(2);
        }
    }

    public void ActualizarNumeroBalas(int queJugadorEs)
    {
        if(queJugadorEs == 1)
        {
            for(int i = 0; i < balasJ1.Length; i++)
            {
                if (i < numeroDeBalasJ1)
                {
                    balasJ1[i].enabled = true;
                }
                else
                {
                    balasJ1[i].enabled = false;
                }
            }
        }
        else
        {
            for(int i = 0; i < balasJ2.Length; i++)
            {
                if (i < numeroDeBalasJ2)
                {
                    balasJ2[i].enabled = true;
                }
                else
                {
                    balasJ2[i].enabled = false;
                }
            }
        }
            
    }

    public void ActualizarNumeroVidas(int queJugadorEs)
    {
        if(queJugadorEs == 1)
        {
            for(int i = 0; i < VidasJ1.Length; i++)
            {
                if (i < VidaJugador1)
                {
                    VidasJ1[i].enabled = true;
                }
                else
                {
                    VidasJ1[i].enabled = false;
                }
            }
        }
        else
        {
            for(int i = 0; i < VidasJ2.Length; i++)
            {
                if (i < VidaJugador2)
                {
                    VidasJ2[i].enabled = true;
                }
                else
                {
                    VidasJ2[i].enabled = false;
                }
            }
        }
            
    }
}
