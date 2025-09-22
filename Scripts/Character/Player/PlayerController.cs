using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerSound
{
    walk, shoot, hurted,
}

namespace OldVersion
{
    public class PlayerController : MonoBehaviour
    {
        AudioSource audioSource;
        public float moveSpeed = 1;
        public InputAction LeftAction;
        //TODO() - FireAction, TalkAction
        public InputAction MoveAction, FireAction, TalkAction;
        Rigidbody2D rigidbody2d;
        Vector2 move;
        public int maxHealth = 5;
        int currentHealth, projNum;
        public int health { get { return currentHealth; } }
        // Variables related to DURATION
        public float timeInvincible = 2.0f, timeShoot = 1.5f;
        bool isInvincible, isStopMove;
        float damageCooldown, shootCooldown;
        Animator animator;
        Vector2 moveDirection = new Vector2(1, 0);
        public GameObject projectilePrefab;
        //  0     1        2
        //walk  shoot   hurted
        public AudioClip[] audioClips;
        public MicrobarImageAnim microbarImageAnim;
        private ObjectPoolOld objectPool;
        //bulletParent for pt spawn bullet
        //center for dist from enemy
        //effectParent for pt spawn effect
        public GameObject bulletParent, center, effectParent;
        public int playerSoundVol;
        Vector2 prevPos;
        public float prevPosCheckTime = 1.5f;
        float prevPosCheckCd;
        //public ParticleSystem hitEffect, healEffect;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            rigidbody2d = GetComponent<Rigidbody2D>();
            audioSource = GetComponent<AudioSource>();
            objectPool = GetComponentInChildren<ObjectPoolOld>();
            //LeftAction.Enable();
            MoveAction.Enable();
            FireAction.Enable();
            TalkAction.Enable();
            //adjust frame rate - By default, this game runs at 60 fps.
            //QualitySettings.vSyncCount = 0;
            //Application.targetFrameRate = 10;
            currentHealth = maxHealth;
            prevPosCheckCd = prevPosCheckTime;
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.Instance.IsPause) return;
            if (isStopMove)
                return;
            // float horizontal = 0.0f;
            // if (LeftAction.IsPressed())
            //     horizontal = -1.0f;
            // else if (Keyboard.current.rightArrowKey.isPressed)
            //     horizontal = 1.0f;
            //Debug.Log(horizontal);

            // float vertical = 0.0f;
            // if (Keyboard.current.upArrowKey.isPressed)
            //     vertical = 1.0f;
            // else if (Keyboard.current.downArrowKey.isPressed)
            //     vertical = -1.0f;
            //Debug.Log(vertical);

            // Vector2 position = transform.position;
            // position.x += 0.1f * horizontal * moveSpeed * Time.deltaTime;
            // position.y += 0.1f * vertical * moveSpeed * Time.deltaTime;
            // transform.position = position;

            move = MoveAction.ReadValue<Vector2>();
            /*
            This if statement checks whether move.x or move.y (the current user input movement values, stored in the 
            move variable declared in the Update function) are not equal to 0.
            The condition uses Mathf.Approximately to make the check instead of the equality operator (==) because the 
            way that computers store float values means that there is a tiny loss in precision. This loss means that you 
            should not test for perfect quality, because an operation that should return 0.0f could end up returning
            0.0000000001f instead. The bool Approximately takes the imprecision into account, and returns true if the 
            value can be considered equal minus that imprecision.
            */
            if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                moveDirection.Set(move.x, move.y);
                /*
                Normalize on moveDirection, which sets its length to 1 but keeps its direction the same.
                For example, a Vector2 set to (3,0) stores a position one unit to the right of the center of the scene, 
                but it also stores the direction right — if you trace an arrow from (0,0) to (3,0) you get an arrow 
                pointing to the right. Normalizing this vector will set it to (1,0), still pointing to the right but 
                with a length of 1. In general, you normalize vectors that store direction because length is not 
                important.
                Important: Do not normalize vectors storing positions. Normalizing changes the x and y values, 
                so normalizing position vectors will change the position. 
                */
                moveDirection.Normalize();
            }
            animator.SetFloat("Look X", moveDirection.x);
            animator.SetFloat("Look Y", moveDirection.y);
            //passes the length of the move vector to the Speed parameter. This length will be 0 
            //if the player character is stationary, or 1 if the character is moving (because the length is normalized).
            animator.SetFloat("Speed", move.magnitude);

            //Debug.Log(move);
            //deltaTime, contained inside Time, is a variable that Unity uses to store the time it takes for a frame to be rendered.
            //When you use Time.deltaTime, the player character will move the same amount per second no matter what the frame rate is.
            // Vector2 position = (Vector2)transform.position + move * 0.1f * moveSpeed * Time.deltaTime;
            // transform.position = position;
            if (isInvincible)
            {
                damageCooldown -= Time.deltaTime;
                if (damageCooldown < 0)
                    isInvincible = false;
            }
            shootCooldown -= Time.deltaTime;
            if (shootCooldown < 0)
            {
                if (FireAction.IsPressed())
                    Launch();
            }

            if (rigidbody2d.position == prevPos)
            {
                playerSoundVol = 0;
                prevPosCheckCd = prevPosCheckTime;
            }
            else
            {
                playerSoundVol = 1;
                prevPosCheckCd -= Time.deltaTime;
            }


            // if (prevPosCheckCd < 0)
            // {
            //     prevPosCheckCd = prevPosCheckTime;
            //     playerSoundVol = rigidbody2d.position == prevPos ? 0 : 1;
            // }
        }

        void FixedUpdate()
        {
            Vector2 position = (Vector2)rigidbody2d.position + move * moveSpeed * Time.deltaTime;
            if (prevPosCheckCd < 0)
            {
                prevPos = position;
            }
            //make move the rigidbody to avoid jittering
            rigidbody2d.MovePosition(position);
            PlaySound(PlayerSound.walk);
        }

        public void ChangeHealth(int amount, MicrobarAnimType mat)
        {
            if (amount < 0)
            {
                if (isInvincible)
                {
                    return;
                }
                float dmgPercentage = (float)-amount / (float)maxHealth * (float)100;
                //Debug.Log("dmgPercentage " + dmgPercentage);
                //Debug.Log("mat " + mat);
                //microbarImageAnim.Damage(mat, dmgPercentage);
                isInvincible = true;
                damageCooldown = timeInvincible;
                animator.SetTrigger("Hit");
                PlaySound(PlayerSound.hurted);
                Debug.Log("hit");
                //Instantiate(hitEffect, transform);
                GameObject hitEffect = objectPool.GetPooledObject(1);
                if (hitEffect)
                {
                    hitEffect.transform.position = effectParent.transform.position;
                    hitEffect.SetActive(true);
                }
            }
            else
            {
                float healPercentage = (float)amount / (float)maxHealth * (float)100;
                //microbarImageAnim.Heal(mat, healPercentage);
                //Instantiate(healEffect, transform);
                GameObject healEffect = objectPool.GetPooledObject(2);
                if (healEffect)
                {
                    healEffect.transform.position = effectParent.transform.position;
                    healEffect.SetActive(true);
                }
            }
            /*
            This instruction makes sure that currentHealth cannot be set to a value that is over the maxHealth value or 
            below 0. Fixing a range of possible health values will help you to keep the system balanced for a good player experience.
            This instruction uses a built-in function, Mathf.Clamp, to fix the range of possible health values. Mathf.Clamp clamps 
            (restricts) the possible values within a set range using three parameters separated by commas.
            The first parameter is the value that needs to be restricted (currentHealth + amount).
            The second parameter is the minimum allowed value (0).
            The third parameter is the maximum allowed value (maxHealth).
            */
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
            //Debug.Log(currentHealth + "/" + maxHealth);
            //UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
        }

        void Launch()
        {
            projNum++;
            Debug.Log("projectile " + projNum);
            shootCooldown = timeShoot;
            //Quaternions are mathematical operators that can express rotation. All you need to know here is 
            //that Quaternion.identity means no rotation.
            // GameObject projectileObject = Instantiate(
            //     projectilePrefab, 
            //     rigidbody2d.position + Vector2.up * 0.5f, 
            //     Quaternion.identity
            // );
            GameObject projectileObject = objectPool.GetPooledObject(0);
            if (projectileObject)
            {
                projectileObject.transform.position = bulletParent.transform.position;
                projectileObject.transform.rotation = Quaternion.identity;
                projectileObject.SetActive(true);
            }
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(bulletParent.transform.position, moveDirection, 1);
            animator.SetTrigger("Launch");
            PlaySound(PlayerSound.shoot);
        }

        public void StopMove(bool isTrue)
        {
            isStopMove = isTrue;
        }

        void PlaySound(PlayerSound playerSound)
        {
            switch (playerSound)
            {
                case PlayerSound.shoot:
                    PlaySound(audioClips[1]);
                    break;
                case PlayerSound.hurted:
                    PlaySound(audioClips[2]);
                    break;
                //walk
                default:
                    PlaySound(audioClips[0]);
                    break;
            }
        }
        public void PlaySound(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}