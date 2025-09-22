using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyPattern
{
    single_dir, double_dir, double_rand_dir, square_dir
}
namespace OldVersion
{
    public class EnemyController : MonoBehaviour
    {
        AudioSource audioSource;
        public float speed;
        Rigidbody2D rigidbody2d;
        public bool vertical;
        public float changeTime = 3.0f;
        float timer;
        int direction = 1, chgDirTime = 0;
        public EnemyPattern enemyPattern;
        bool isAtStartPt;
        Vector2 originPos;
        int[] dirRandNos = { 0, 1, 1, 1 };
        Animator animator;
        bool broken = true;
        public int MAX_HP = 5;
        public int health { get; private set; }
        //  0       1
        //broken   fix
        public AudioClip[] audioClipLoops;
        //  0       1
        //hurted   fixed_by
        public AudioClip[] audioClipOnes;
        public ParticleSystem smokeEffect;
        public float effectPosY = 0.5f;
        public MicrobarSpriteAnim microbarSpriteAnim;
        public MicrobarAnimType microbarAnimType;
        private ObjectPoolOld objectPool;
        public GameObject bulletParent, center;
        Transform player;
        public float lineOfSight, shootingRange;
        public EnemyChaseType enemyChaseType;
        float fireCooldown;
        public float timeFire = 1;
        //private Rigidbody2D rigidbody2D;
        public bool isShootHoming;

        // Start is called before the first frame update
        void Start()
        {
            rigidbody2d = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            player = GameObject.FindGameObjectWithTag("Player").transform;
            objectPool = GetComponentInChildren<ObjectPoolOld>();
            //microbarSpriteAnim = GetComponent<MicrobarSpriteAnim>();
            timer = changeTime;
            originPos = rigidbody2d.position;
            health = MAX_HP;
        }

        void Update()
        {
            if (GameManager.Instance.IsPause) return;
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                ChangeDirectionAxis();
            }
        }

        void FixedUpdate()
        {
            if (GameManager.Instance.IsPause) return;
            if (!broken)
            {
                return;
            }
            Roaming();
        }

        void Roaming()
        {
            Vector2 position = rigidbody2d.position;
            if (vertical)
            {
                position.y += speed * direction * Time.deltaTime;
                animator.SetFloat("Move X", 0);
                animator.SetFloat("Move Y", direction);
            }
            else
            {
                position.x += speed * direction * Time.deltaTime;
                animator.SetFloat("Move X", direction);
                animator.SetFloat("Move Y", 0);
            }
            rigidbody2d.MovePosition(position);
            if (enemyPattern == EnemyPattern.double_dir)
                isAtStartPt = position == originPos;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (broken)
                BodyHit(other);
        }
        void BodyHit(Collider2D other)
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player)
            {
                player.ChangeHealth(-1, microbarAnimType);
            }
        }

        void ChangeDirectionAxis()
        {
            int randNo = dirRandNos[RandomGenerator.GenerateRandomNumber(0, dirRandNos.Length - 1)];
            //Debug.Log("rand no dir" + randNo);
            switch (enemyPattern)
            {
                case EnemyPattern.double_dir:
                    if (isAtStartPt)
                        vertical = randNo == 1;
                    ChangeDirection();
                    break;
                case EnemyPattern.double_rand_dir:
                    vertical = randNo == 1;
                    ChangeDirection();
                    break;
                case EnemyPattern.square_dir:
                    vertical = !vertical;
                    timer = changeTime;
                    chgDirTime++;
                    if (chgDirTime > 3)
                        chgDirTime = 0;
                    if (chgDirTime == 2 || chgDirTime == 0)
                        direction = -direction;
                    break;
                default:
                    ChangeDirection();
                    break;
            }
        }

        void ChangeDirection()
        {
            direction = -direction;
            timer = changeTime;
        }

        public void GetAttacked(MicrobarAnimType mat, int damageAmount)
        {
            //Debug.Log("GetAttacked " + damageAmount);
            float dmgPercentage = (float)damageAmount / (float)MAX_HP * (float)100;
            health -= damageAmount;
            //microbarSpriteAnim.Damage(mat, dmgPercentage);
            //Instantiate(hitEffect, transform);
            GameObject hitEffect = objectPool.GetPooledObject(0);
            if (hitEffect)
            {
                hitEffect.transform.position = rigidbody2d.position + Vector2.up * effectPosY;
                hitEffect.SetActive(true);
            }
            if (health > 0)
            {
                PlaySound(EnemySound.hurted);
                return;
            }
            Fix();
        }
        void Fix()
        {

            PlaySound(EnemySound.fixed_by);
            broken = false;
            rigidbody2d.simulated = false;
            animator.SetTrigger("Fixed");
            ChangeAudioClip(audioClipLoops[1]);
            smokeEffect.Stop();
        }

        void ChangeAudioClip(AudioClip clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }

        void PlaySound(EnemySound enemySound)
        {
            switch (enemySound)
            {
                case EnemySound.hurted:
                    audioSource.PlayOneShot(audioClipOnes[0]);
                    break;
                //fixed_by
                default:
                    audioSource.PlayOneShot(audioClipOnes[1]);
                    break;
            }
        }
    }
}