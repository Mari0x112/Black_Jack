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
    public Button standButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;
    public Text pointPlayerMessage;
    public Text pointDealerMessage;
    private CardHand playerCardHand = new CardHand();
    private CardHand dealerCardHand = new CardHand();
    

    public int[] values = new int[52];
    int cardIndex = 0;
       
    private void Awake()
    {    
        InitCardValues();
        //creo las manos de cartas
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
        /*TODO:
         * Asignar un valor a cada una de las 52 cartas del atributo "values".
         * En principio, la posición de cada valor se deberá corresponder con la posición de faces. 
         * Por ejemplo, si en faces[1] hay un 2 de corazones, en values[1] debería haber un 2.
         */
        var inicio = 0;
        var valor = 1;
        var limitador = 13;

        for(int i = 0; i < 4; i++)
        {
            for(int j=inicio ; j < limitador; j++) {
                //para el caso del As que valdrá 11
                if(j == inicio)
                {
                    values[j] = 11;
                }
                //para asignar la repeticion de los 4 dieces
                else if(j>=limitador-3) {
                    values[j] = 10;
                }
                // los valores
                else
                {
                    values[j] = valor;
                }
                valor++;
            }
            //reinicio el valor, y paso a rellenar el siguiente mazo de la baraja
            valor = 1;
            inicio += 13;
            limitador += 13;
        }
        
    }

    private void ShuffleCards()
    {
        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El método Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */
        for (int i = 0; i < values.Length; i++)
        {
            //creo dos variables temporales
            int temp = values[i];
            Sprite temp2 = faces[i];
            // genero un valor random
            int randomIndex = Random.Range(i, values.Length);
            //le asigno ese valor random a las cartas
            values[i] = values[randomIndex];
            faces[i] = faces[randomIndex];
            //guardo las variables para no perderlas
            values[randomIndex] = temp;
            faces[randomIndex] = temp2;
        }
    }

    void StartGame()
    {
        /*TODO:
        * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
        */
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();
        }
        pointPlayerMessage.text = "Puntos: " + playerCardHand.points.ToString();

        if (playerCardHand.points == 21 && dealerCardHand.points == 21)
        {
            dealerCardHand.cards[0].GetComponent<CardModel>().ToggleFace(true);
            finalMessage.text = "EMPATE";
            hitButton.interactable = false;
            standButton.interactable = false;
            pointDealerMessage.text = "Puntos del dealer: " + dealerCardHand.points.ToString();
        }
        else if (dealerCardHand.points == 21)
        {
            dealerCardHand.cards[0].GetComponent<CardModel>().ToggleFace(true);
            finalMessage.text = "PLAYER LOSE";
            hitButton.interactable = false;
            standButton.interactable = false;
            pointDealerMessage.text = "Puntos del dealer: " + dealerCardHand.points.ToString();
        }
        else if (playerCardHand.points == 21)
        {
            dealerCardHand.cards[0].GetComponent<CardModel>().ToggleFace(true);
            finalMessage.text = "PLAYER WINS";
            hitButton.interactable = false;
            standButton.interactable = false;
            pointDealerMessage.text = "Puntos del dealer: " + dealerCardHand.points.ToString();
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
        var favorables = 0;
        var favorables2 = 0;
        var favorables3 = 0;
        var totales = 0;
      
        for (var i = cardIndex; i<values.Length; i++)
        {
            if (values[3] + values[i] > playerCardHand.points)
            {
                favorables++;
            }
            if (playerCardHand.points + values[i] >= 17 && playerCardHand.points + values[i] <= 21)
            {
                favorables2++;
            }
            if (playerCardHand.points + values[i] > 21)
            {
                favorables3++;
            }
            totales++;
        }

        double resultado =((double)favorables/ (double)totales);
        double resultado2 = ((double)favorables2 / (double)totales);
        double resultado3 = ((double)favorables3 / (double)totales);
        string a = resultado.ToString();
        string b = resultado2.ToString();
        string c = resultado3.ToString();


        probMessage.text =
        "Deal > Player: " + a + "\n" +
        "17 <= X <= 21: " + b + "\n" +
        "21 > X: " + c;

        Debug.Log(resultado);
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
        if (playerCardHand.points > 21)
        {
            dealerCardHand.cards[0].GetComponent<CardModel>().ToggleFace(true);
            finalMessage.text = "PLAYER LOSE";
            hitButton.interactable = false;
            standButton.interactable = false;
            pointDealerMessage.text = "Puntos del dealer: " + dealerCardHand.points.ToString();
        }
        pointPlayerMessage.text = "Puntos: "+ playerCardHand.points.ToString();
    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        hitButton.interactable = false;
        standButton.interactable = false;

        while(dealerCardHand.points <= 16)
        {
            PushDealer();
        }
        pointDealerMessage.text = "Puntos del dealer: " + dealerCardHand.points.ToString();

        if (dealerCardHand.points > 21)
        {
            dealerCardHand.cards[0].GetComponent<CardModel>().ToggleFace(true);
            finalMessage.text = "PLAYER WINS";
            hitButton.interactable = false;
            standButton.interactable = false;
        }
        else if (playerCardHand.points == dealerCardHand.points)
        {
            dealerCardHand.cards[0].GetComponent<CardModel>().ToggleFace(true);
            finalMessage.text = "EMPATE";
            hitButton.interactable = false;
            standButton.interactable = false;
        }
        else if (dealerCardHand.points > playerCardHand.points)
        {
            dealerCardHand.cards[0].GetComponent<CardModel>().ToggleFace(true);
            finalMessage.text = "PLAYER LOSE";
            hitButton.interactable = false;
            standButton.interactable = false;
        }
        else if (dealerCardHand.points < playerCardHand.points)
        {
            dealerCardHand.cards[0].GetComponent<CardModel>().ToggleFace(true);
            finalMessage.text = "PLAYER WINS";
            hitButton.interactable = false;
            standButton.interactable = false;
        }
        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */
    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        standButton.interactable = true;
        finalMessage.text = "";
        pointDealerMessage.text = "Puntos del dealer:";
        pointPlayerMessage.text = "Puntos:";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }
    
}
