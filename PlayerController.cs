
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//import a using for my text
using UnityEngine.UI;
using TMPro;

public class PlayerController: MonoBehaviour
{
    public float speed= 150.0f;
    public float jumpForce= 25.0f;
    private Rigidbody2D _body;
    private BoxCollider2D _box;
    private Animator _anim;
    public Animator gameOverAnim;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI jewelText;
    private int score;
    private int jewelCount;
    public int maxHealth=100;
    public int currentHealth;
    public int lives;
    public HealthBar healthBar;
    private Inventory inventory;
    private bool chestOpened=false;
    public CharacterScript character;
    public UIManager uiManager;
    [SerializeField] private InventoryManager inventoryManager;

    void BeginPlay()
    {
        
    }
    void Start()
    {
        _body= GetComponent<Rigidbody2D>();
        _box = GetComponent<BoxCollider2D>();
        _anim =GetComponent<Animator>();
        
        currentHealth=maxHealth;
        lives=3;
        uiManager.updateLives(lives);
        healthBar.SetMaxHealth(maxHealth);
        
        score=0;
        jewelCount=0;
        jewelText.text=": " + jewelCount.ToString();
        scoreText.text=": " + score.ToString();

        inventory = new Inventory(this);
        inventoryManager.SetInventory(inventory);
        
        Debug.Log(Time.deltaTime);
    }

    void Update()
    {
        //side to side arrow key
        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        Vector2 movement = new Vector2(moveX, _body.velocity.y);
        _body.velocity = movement;

        Vector3 max = _box.bounds.max;
        Vector3 min = _box.bounds.min;

        //checking for collisions with objects
        Vector2 corner1 = new Vector2(max.x, min.y - .1f);
        Vector2 corner2 = new Vector2(min.x, min.y - .2f);
        Collider2D hit = Physics2D.OverlapArea(corner1, corner2);
        bool grounded = false;

        


        if (hit != null)
        {
            grounded = true;
        }

        //ability to jump
        if (grounded && Input.GetKeyDown(KeyCode.UpArrow))
        {
            _body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        //passing speed to animator to trigger condition
        _anim.SetFloat("speed", Mathf.Abs(moveX));
        if (!Mathf.Approximately(moveX, 0))
        {
            transform.localScale = new Vector3(Mathf.Sign(moveX), 1, 1);
        }

        // Check if player is standing on a moving platform
        if (transform.parent != null && transform.parent.CompareTag("MovingPlatform"))
        {
            // Get the platform's rigidbody component
            Rigidbody2D movingPlatform = transform.parent.GetComponent<Rigidbody2D>();
            if (movingPlatform != null)
            {
                // Calculate the position of the player on the platform
                // Calculate the position of the player on the platform
                Vector2 playerOnPlatformPos = (Vector2)transform.position - movingPlatform.position;

                // Use the movePosition method to move the player with the platform
                movingPlatform.MovePosition(Vector2.Lerp(movingPlatform.position, movingPlatform.position + movement, Time.deltaTime));
                // Set the player's position relative to the platform
                transform.position = playerOnPlatformPos + movingPlatform.position;
            }
        }
    }


    // void Update()
    // {
    //     //side to side arrow key
    //     float moveX =Input.GetAxis("Horizontal") * speed * Time.deltaTime;
    //     Vector2 movement = new Vector2(moveX, _body.velocity.y);
    //     _body.velocity = movement;

    //     Vector3 max= _box.bounds.max;
    //     Vector3 min= _box.bounds.min;

    //     //checking for collisions with objects
    //     Vector2 corner1= new Vector2(max.x, min.y - .1f);
    //     Vector2 corner2= new Vector2(min.x, min.y - .2f);
    //     Collider2D hit= Physics2D.OverlapArea(corner1, corner2);
    //     bool grounded= false;
    //     if(hit !=null)
    //     {
    //         grounded=true;
    //     }

    //     //ability to jump
    //     if (grounded && Input.GetKeyDown(KeyCode.UpArrow))
    //     {
    //         _body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    //     }

    //     //passing speed to animator to trigger condition
    //     _anim.SetFloat("speed", Mathf.Abs(moveX));
    //     if(!Mathf.Approximately(moveX, 0)) 
    //     {
    //         transform.localScale = new Vector3(Mathf.Sign(moveX), 1, 1);
    //     }
    // }

    

    public void CheckLivesAndHealth(){
        if (currentHealth<=0)
        {
            lives--;
            uiManager.updateLives(lives);
            currentHealth=maxHealth;
            Debug.Log("lives "+lives);
            healthBar.SetHealth(currentHealth);
        }
        if (lives<=0)
        {
            _body.constraints = RigidbodyConstraints2D.FreezeAll;
            currentHealth=0;
            healthBar.SetHealth(currentHealth);
            gameOverAnim.SetTrigger("pop");
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
    switch (other.gameObject.tag)
        {
            case "Pickup":
                score += 1;
                scoreText.text = ": " + score.ToString();
                other.gameObject.SetActive(false);
                break;
            case "Pickup-Jewel":
                jewelCount += 1;
                jewelText.text = ": " + jewelCount.ToString();
                other.gameObject.SetActive(false);
                break;
            case "Chest":
                if (chestOpened==false)
                {
                    score+=100;
                    scoreText.text = ": " + score.ToString();
                    jewelCount += 10;
                    jewelText.text = ": " + jewelCount.ToString();
                    chestOpened=true;
                }
            break;
            case "HintScroll":
            case "ManipulativesPotion":
            case "HealthPotion":
                inventory.AddItem(other.gameObject);
                other.gameObject.SetActive(false);
                Debug.Log(inventory);
            break;
            case "MovingPlatform":
            // Set the player's parent to the platform
                transform.parent = other.transform;
                Debug.Log(transform.localScale + " before");
                transform.localScale = new Vector3 (0.36f, 1.06f, 1.00f);
                Debug.Log(transform.localScale + " after");
            break;
            case "Character":
                character = other.gameObject.GetComponent<CharacterScript>();
                if (!character.freezePlayer) {
                    _body.constraints = RigidbodyConstraints2D.FreezePositionX;
                }
            break;
            case "Enemy":
                
            break;
            default:
                if (!other.CompareTag("Character"))
                {

                    other.gameObject.SetActive(false);
                }
                break;
        }
    }
    // public void UseItem(Item item)
    // {
    //     switch (item.itemType)
    //     {
    //         case Item.ItemType.HealthPotion:
    //             AddHealth(10);
    //             inventory.RemoveItem(new Item {itemType= Item.ItemType.HealthPotion, amount =1});
    //         break;
    //         case Item.ItemType.ManipulativesPotion:
    //             inventory.RemoveItem(new Item {itemType= Item.ItemType.ManipulativesPotion, amount =1});
    //         break;
    //         case Item.ItemType.HintScroll:
    //             inventory.RemoveItem(new Item {itemType= Item.ItemType.HintScroll, amount =1});
    //         break;
    //     }
    // }

    private void OnTriggerExit2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "MovingPlatform":
                // Remove the player's parent from the platform
                transform.parent = null;
                transform.localScale = new Vector3 (1f, 1f, 1f);
                break;
            default:
                break;
        }
    }

    public void Jump(float force)
    {
        // Set the player's velocity to the jump force, while preserving their horizontal velocity
        Vector2 newVelocity = new Vector2(_body.velocity.x, force);
        _body.velocity = newVelocity;
    }

    public void AddScore(int coinsUpdate, int jewelUpdate)
    {
        score+=coinsUpdate;
        scoreText.text = ": " + score.ToString();
        jewelCount+=jewelUpdate;
        jewelText.text = ": " + jewelCount.ToString();
    }

    public void ResumeMovement()
    {
        _body.constraints = RigidbodyConstraints2D.FreezeRotation;
        character.freezePlayer=true;
    }

    public void TakeDamage(int damage)
    {
        currentHealth-=damage;
        healthBar.SetHealth(currentHealth);
        CheckLivesAndHealth();
    }

    public void AddHealth(int health)
    {
        if (currentHealth>=90)
        {
            currentHealth=maxHealth;
        }
        else
        {
            currentHealth+=health;
        }
        healthBar.SetHealth(currentHealth);
    }

    public void EndOfGame()
    {
        _body.constraints= RigidbodyConstraints2D.FreezeAll;
    }
}


