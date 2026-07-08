using System.Collections; // Maakt het gebruik van IEnumerator en coroutines mogelijk
using UnityEngine; // Laadt de basis Unity functies
using UnityEngine.UI; // Maakt gebruik van UI onderdelen zoals Image mogelijk

public class Player : MonoBehaviour // Maakt een Player script dat aan een GameObject gekoppeld kan worden
{
    [Header("Movement")] // Maakt een zichtbare categorie in de Unity Inspector
    public int health = 100; // Slaat de huidige levenspunten van de speler op
    public float moveSpeed = 5f; // Bepaalt de snelheid waarmee de speler beweegt
    public float jumpForce = 10f; // Bepaalt de kracht van de sprong

    [Header("Ground Check")] // Maakt een categorie voor grondcontrole instellingen
    public Transform groundCheck; // Punt dat gebruikt wordt om te controleren of de speler op de grond staat
    public float groundCheckRadius = 0.2f; // Grootte van de cirkel waarmee de grond wordt gecontroleerd
    public LayerMask groundLayer; // Bepaalt welke lagen als grond worden gezien
    public Image healthImage; // Verwijzing naar de UI afbeelding die de gezondheid toont

    private Rigidbody2D rb; // Verwijzing naar de Rigidbody2D van de speler
    private bool isGrounded; // Houdt bij of de speler op de grond staat

    private Animator animator; // Verwijzing naar het animatiecomponent van de speler

    private SpriteRenderer spriteRenderer; // Verwijzing naar de sprite voor kleurveranderingen
    public int extraJumpsValue = 1; // Bepaalt hoeveel extra sprongen de speler krijgt
    private int extraJumps; // Houdt het aantal beschikbare extra sprongen bij

    void Start() // Wordt één keer uitgevoerd wanneer het spel start
    {
        rb = GetComponent<Rigidbody2D>(); // Haalt de Rigidbody2D van de speler op
        animator = GetComponent<Animator>(); // Haalt de Animator van de speler op
        spriteRenderer = GetComponent<SpriteRenderer>(); // Haalt de SpriteRenderer van de speler op

        extraJumps = extraJumpsValue; // Zet het aantal extra sprongen op de ingestelde waarde
    }

    void Update() // Wordt elke frame uitgevoerd
    {
        float moveInput = Input.GetAxisRaw("Horizontal"); // Leest de horizontale invoer van de speler

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y); // Verandert de horizontale snelheid van de speler

        if (isGrounded) // Controleert of de speler op de grond staat
        {
            extraJumps = extraJumpsValue; // Reset de extra sprongen wanneer de speler landt
        }

        if (Input.GetKeyDown(KeyCode.Space)) // Controleert of de speler op spatie drukt
        {
            if (isGrounded) // Controleert of de speler op de grond staat
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Geeft de speler een sprong omhoog
                extraJumps = extraJumpsValue; // Reset extra sprongen na een normale sprong
            }
            else if (extraJumps > 0) // Controleert of er nog extra sprongen beschikbaar zijn
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Voert een extra sprong uit
                extraJumps--; // Vermindert het aantal beschikbare extra sprongen
            }
        }

        SetAnimation(moveInput); // Verandert de animatie afhankelijk van de beweging

        healthImage.fillAmount = health / 100f; // Past de gezondheidsbalk aan op basis van de huidige health

        if (moveInput > 0) // Controleert of de speler naar rechts beweegt
            transform.localScale = new Vector3(1, 1, 1); // Richt de sprite naar rechts

        else if (moveInput < 0) // Controleert of de speler naar links beweegt
            transform.localScale = new Vector3(-1, 1, 1); // Richt de sprite naar links
    }

    void FixedUpdate() // Wordt uitgevoerd op vaste tijdstippen voor physics
    {
        isGrounded = Physics2D.OverlapCircle( // Controleert of er grond onder de speler zit
            groundCheck.position, // Positie waar de controle wordt uitgevoerd
            groundCheckRadius, // Grootte van het controlegebied
            groundLayer // Welke lagen als grond gelden
        );
    }

    void SetAnimation(float moveInput) // Regelt welke animatie wordt afgespeeld
    {
        if (animator == null) // Controleert of er een Animator aanwezig is
            return; // Stopt de functie als er geen Animator is

        if (isGrounded) // Controleert of de speler op de grond staat
        {
            if (Mathf.Abs(moveInput) > 0.01f) // Controleert of de speler beweegt
                animator.Play("Player_Run"); // Speelt de ren animatie af
            else
                animator.Play("Player_Idle"); // Speelt de stilstaande animatie af
        }
        else // Wordt uitgevoerd wanneer de speler in de lucht is
        {
            if (rb.linearVelocity.y > 0) // Controleert of de speler omhoog beweegt
                animator.Play("Player_Jump"); // Speelt de spring animatie af
            else
                animator.Play("Player_Fall"); // Speelt de val animatie af
        }
    }

    private void OnDrawGizmosSelected() // Tekent hulpmiddelen zichtbaar in de Unity editor
    {
        if (groundCheck == null) return; // Stopt als er geen groundCheck is ingesteld

        Gizmos.color = Color.green; // Zet de kleur van de gizmo op groen
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius); // Tekent de controlecirkel voor de grond
    }

    private void OnCollisionEnter2D(Collision2D collision) // Wordt uitgevoerd wanneer de speler iets raakt
    {
        if (collision.gameObject.tag == "Damage") // Controleert of het geraakte object schade doet
        {
            health -= 25; // Vermindert de gezondheid met 25
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Geeft een terugslag omhoog
            StartCoroutine(BlinkRed()); // Start de rode knipperanimatie

            if (health <= 0) // Controleert of de gezondheid op of onder nul is
            {
                Die(); // Start de dood functie
            }
        }
    }

    private IEnumerator BlinkRed() // Coroutine die de speler tijdelijk rood maakt
    {
        spriteRenderer.color = Color.red; // Zet de spritekleur op rood
        yield return new WaitForSeconds(0.1f); // Wacht 0.1 seconde
        spriteRenderer.color = Color.white; // Zet de spritekleur terug naar normaal
    }

    private void Die() // Functie voor wanneer de speler doodgaat
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene"); // Laadt de game scene opnieuw
    }
}