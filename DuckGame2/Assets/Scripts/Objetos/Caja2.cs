using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caja2 : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar si la colisi�n es con el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<ControlJugador>().EfectoNegativo("SoltarArmas");
            //Animacion caja y eliminar caja.
            Destroy(gameObject);

        }

        // Destruir la caja solo si no es una colisi�n con el jugador
        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);//Animacion caja se rompe
        }
    }

   


}
