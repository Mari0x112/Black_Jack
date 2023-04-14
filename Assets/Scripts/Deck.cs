using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;
    private CardHand playerCardHand = new CardHand();
    private CardHand dealerCardHand = new CardHand();

    public int[] values = new int[52];
    int cardIndex = 0;    
       
    private void Awake()
    {    
        InitCardValues();
        playerCardHand = player.GetComponent<CardHand>();
        dealerCardHand = dealer.GetComponent<CardHand>();
    }

    private void Start()
    {
        ShuffleCards();
        StartGame();        
    }

    private void InitCardValues()
    {
        var inicio = 0;
        var valor = 1;
        var limitador = 13;

        for(int i = 0; i < 4; i++)
        {
            for(int j=inicio ; j < limitador; j++) {
                if(j == inicio)
                {
                    values[j] = 11;
                }
                else if(j>=limitador-3) {
                    values[j] = 10;
                }
                else
                {
                    values[j] = valor;
                }
                valor++;
            }
            valor = 1;
            inicio += 13;
            limitador += 13;
        }
        
    }

    private void ShuffleCards()
    {
        for (int i = 0; i < values.Length; i++)
        {
            int temp = values[i];
            Sprite temp2 = faces[i];

            int randomIndex = Random.Range(i, values.Length);
            values[i] = values[randomIndex];
            faces[i] = faces[randomIndex];
            values[randomIndex] = temp;
            faces[randomIndex] = temp2;
        }
    }

    void StartGame()
    {
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();
        }

        if (playerCardHand.points == 21 && dealerCardHand.points == 21)
        {
            print(finalMessage);
        }
        else if (dealerCardHand.points == 21)
        {
            print(finalMessage);
        }
        else if (playerCardHand.points == 21)
        {
            print(finalMessage);
        }

    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */
    }

    void PushDealer()
    {
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
        cardIndex++;        
    }

    void PushPlayer()
    {
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        cardIndex++;
        CalculateProbabilities();
    }       

    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        
        //Repartimos carta al jugador
        PushPlayer();

        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */      

    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */                
         
    }


    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }
    
}
