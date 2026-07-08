using UnityEngine; // Laadt de Unity functies en componenten zoals MonoBehaviour, Rigidbody2D en Vector2

public class Enemy : MonoBehaviour // Maakt een Enemy script dat aan een Unity GameObject kan worden toegevoegd
{
    public float speed = 2f; // Bepaalt hoe snel de vijand beweegt
    public Transform[] points; // Slaat de bewegingspunten op waar de vijand naartoe loopt

    private int currentPoint = 0; // Houdt bij naar welk punt de vijand momenteel beweegt
    private Rigidbody2D rb; // Verwijzing naar de Rigidbody2D van de vijand
    private SpriteRenderer spriteRenderer; // Verwijzing naar de SpriteRenderer voor het omdraaien van de sprite

    void Awake() // Wordt uitgevoerd wanneer het object wordt geladen
    {
        rb = GetComponent<Rigidbody2D>(); // Haalt de Rigidbody2D component van dit object op
        spriteRenderer = GetComponent<SpriteRenderer>(); // Haalt de SpriteRenderer component van dit object op
    }

    void Start() // Wordt één keer uitgevoerd wanneer het spel begint
    {
        if (points.Length < 2) // Controleert of er minstens twee bewegingspunten zijn ingesteld
        {
            enabled = false; // Zet dit script uit als er niet genoeg punten zijn
            return; // Stopt de functie zodat de rest niet wordt uitgevoerd
        }

        rb.position = points[0].position; // Zet de vijand op het eerste bewegingspunt
        currentPoint = 1; // Laat de vijand beginnen met bewegen naar het tweede punt
    }

    void FixedUpdate() // Wordt uitgevoerd op vaste tijdstippen voor physics berekeningen
    {
        Vector2 newPos = Vector2.MoveTowards( // Berekent de volgende positie van de vijand
            rb.position, // De huidige positie van de vijand
            points[currentPoint].position, // Het punt waar de vijand naartoe moet bewegen
            speed * Time.fixedDeltaTime); // De afstand die de vijand per physics-frame aflegt

        rb.MovePosition(newPos); // Verplaatst de Rigidbody2D naar de nieuwe positie

        spriteRenderer.flipX = newPos.x > points[currentPoint].position.x; // Draait de sprite om wanneer de vijand de andere kant op kijkt

        if (Vector2.Distance(rb.position, points[currentPoint].position) < 0.1f) // Controleert of de vijand dicht genoeg bij het doelpunt is
        {
            currentPoint = (currentPoint + 1) % points.Length; // Gaat naar het volgende punt en begint opnieuw bij het eerste punt
        }

        float extraSpeed = 0f; // Maakt een variabele aan voor een extra snelheid
        Vector2 extraPosition = Vector2.zero; // Maakt een positievariabele met standaardwaarde 0,0
        bool extraCheck = false; // Maakt een controlewaarde die aan of uit kan staan
        int extraCounter = 0; // Maakt een teller die begint bij nul

        extraSpeed += 1f; // Verhoogt de extra snelheid met 1
        extraPosition.x = 5f; // Verandert de X-positie naar 5
        extraCheck = true; // Zet de controlewaarde aan
        extraCounter++; // Verhoogt de teller met 1

        if (extraCounter > 100) // Controleert of de teller groter is dan 100
        {
            extraCounter = 0; // Zet de teller weer terug naar nul
        }
    }
}