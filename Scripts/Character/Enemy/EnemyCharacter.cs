using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#region enum
public enum EnemySound
{
    hurted, fixed_by,
}
public enum EnemyChaseType
{
    hit, shoot, shootAndHit,
}
public enum EnemyShootAndHitState
{
    hit, shoot,
}
public enum EnemyHitType
{
    normal, dash_slam, attack
}
public enum EnemyShootType
{
    single, homing_single, multi, multi_homing,
}
public enum EnemyState
{
    roam, chase, attack,
}
public enum EnemyRoamPattern
{
    fix, random
}
public enum EnemySightType
{
    distance, raycast, raycastAndDistance,
}
public enum OperatorType
{
    equal, less, lessEqual, more, moreEqual, notEqual
}
public enum AnimType
{
    walk, attack, dash, dash_slam, shoot, fix
}
public enum AddAnimType
{
    noticePlayer, playerEscaped,
}
#endregion

namespace OldVersion
{
    public class EnemyCharacter : MonoBehaviour
    {
        AudioSource audioSource;
        public EnemyRoamPattern enemyRoamPattern;
        Animator animator;
        public bool broken { get; private set; }
        int directionX, directionY;
        public int MAX_HP = 5;
        public int health { get; private set; }
        //  0       1
        //broken   fix
        public AudioClip[] audioClipLoops;
        //  0       1
        //hurted   fixed_by
        public AudioClip[] audioClipOnes;
        public ParticleSystem smokeEffect;
        public MicrobarSpriteAnim microbarSpriteAnim;
        MicrobarAnimType microbarAnimType;
        private ObjectPoolOld objectPool;
        //bulletParent for pt spawn bullet
        //center for dist from player
        //centerTransf for dist from goal
        //effectParent for pt spawn effect
        public GameObject bulletParent, center, centerTransf, effectParent;
        Transform centerPlayer;
        public float lineOfSight, shootingRange, addChaseLine, hitRange;
        //arrange in descending order
        //id 0 must be the longest line
        [Tooltip("must be same as lineOfHearSounds")] public float[] lineOfHears;
        //if want to make it deaf - make it 100, if want to make it detect by presence - make it 0
        [Tooltip("must be same as lineOfHears")] public int[] lineOfHearSounds;
        float lineOfChase;
        public float viewAngle;
        public EnemyChaseType enemyChaseType;
        public EnemyHitType enemyHitType;
        public EnemyShootType enemyShootType;
        public EnemySightType enemySightType;
        float fireCooldown, trackCooldown, attackCooldown, shootAndHitCd, staticCd, changeGoalCd, checkCd;
        public float timeFire = 1, trackTime = 1, attackTime = 1, shootAndHitTime = 1, attackAnimTime = 0.5f;
        public float staticTime = 3, changeGoalTime = 1, checkTime = 0.5f;
        public float attackForce, shootForce;
        //  0           1             2         3
        //single    single_homin    multi   multi_homing
        public int[] projectileId;
        public int effectId;
        private new Rigidbody2D rigidbody2D;
        //public bool isShootHoming;
        NavMeshAgent agent;
        [SerializeField] private EnemyState enemyState;
        //bool isBackToOriPos = true;
        public Transform[] goals = new Transform[3];
        [SerializeField] private int m_NextGoal = 1;
        public bool isDebug;
        float goalDist = 0.5f;
        [SerializeField] private bool isPlayerEscape;
        [SerializeField] private Vector3 lastPlayerPosSight;
        Vector2 moveDirection = new Vector2(0, 0);
        Vector3 prevPos;
        [SerializeField] private EnemyShootAndHitState enemyShootAndHitState = EnemyShootAndHitState.shoot;
        private RaycastHit hit;
        private PlayerController playerController;
        public Animator addAnimator;

        // Start is called before the first frame update
        void Start()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            centerPlayer = playerController.center.transform;
            objectPool = GetComponentInChildren<ObjectPoolOld>();
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            //microbarSpriteAnim = GetComponent<MicrobarSpriteAnim>();
            //originPos = centerTransf.transform.position;
            broken = true;
            health = MAX_HP;
            staticCd = staticTime;
            shootAndHitCd = shootAndHitTime;
            enemyState = EnemyState.roam;
            if (shootingRange > 1)
                agent.stoppingDistance = shootingRange - 1f;
            else if (enemyHitType == EnemyHitType.attack)
                agent.stoppingDistance = hitRange;
            if (agent.stoppingDistance > 2f)
            {
                agent.stoppingDistance = 2f;
                goalDist = 2.1f;
            }
            else if (agent.stoppingDistance > 0)
                goalDist = agent.stoppingDistance + 0.1f;
        }

        void Update()
        {
            if (GameManager.Instance.IsPause) return;
            if (!broken)
            {
                return;
            }
            float distanceFromPlayer = Vector2.Distance(center.transform.position, centerPlayer.position);
            switch (enemyState)
            {
                case EnemyState.chase:
                    ChaseState(distanceFromPlayer);
                    shootAndHitCd -= Time.deltaTime;
                    break;
                case EnemyState.attack:
                    AttackState(distanceFromPlayer);
                    shootAndHitCd -= Time.deltaTime;
                    break;
                //roam
                default:
                    RoamState(distanceFromPlayer);
                    break;
            }
            attackCooldown -= Time.deltaTime;
        }

        void FixedUpdate()
        {
            if (enemyState == EnemyState.roam)
                CheckStuck();
        }

        void CheckStuck()
        {
            checkCd -= Time.deltaTime;
            if (checkCd >= 0)
                return;
            checkCd = checkTime;
            if (transform.position == prevPos)
            {
                GenDebug("static");
                staticCd -= Time.deltaTime;
            }
            else
                staticCd = staticTime;
            if (staticCd < 0)
            {
                GenDebug("change due to stuck");
                staticCd = staticTime;
                ChangeNextGoal();
            }
        }

        #region State
        void ChaseState(float distanceFromPlayer)
        {
            if (isPlayerEscape)
            {
                //in state track player last pos sight
                Chase(distanceFromPlayer, lastPlayerPosSight);
                float distance = Vector2.Distance(centerTransf.transform.position, lastPlayerPosSight);
                GenDebug("dist goal " + centerTransf.transform.position + " - " + lastPlayerPosSight
                    + " = " + distance);
                lineOfChase = lineOfSight + addChaseLine;
                if (SightDecision(distanceFromPlayer, lineOfHears[0] + addChaseLine, OperatorType.less))
                    //back to state chase player
                    isPlayerEscape = false;
                else if (distance < goalDist + 0.5f || trackCooldown < 0)
                {
                    //back to roam
                    enemyState = EnemyState.roam;
                    isPlayerEscape = false;
                    SetAddAnimation(AddAnimType.playerEscaped);
                }
                trackCooldown -= Time.deltaTime;
            }
            else
            {
                //in state chase player
                lineOfChase = lineOfSight + addChaseLine;
                if (SightDecision(distanceFromPlayer, lineOfHears[0] + addChaseLine, OperatorType.less))
                    Chase(distanceFromPlayer, centerPlayer.position);
                else
                {
                    isPlayerEscape = true;
                    lineOfChase = lineOfSight * 2f + addChaseLine;
                    if (SightDecision(distanceFromPlayer, lineOfHears[0] * 1.5f + addChaseLine, OperatorType.less))
                        lastPlayerPosSight = centerPlayer.position;
                }
                trackCooldown = trackTime;
            }
        }
        void AttackState(float distanceFromPlayer)
        {
            if (attackCooldown < 0)
                Attack();
            if (attackCooldown < attackTime - attackAnimTime)
                rigidbody2D.linearVelocity = Vector2.zero;
            if (distanceFromPlayer > hitRange)
                enemyState = EnemyState.chase;
            //SetWalAnimation(center.transform.position, centerPlayer.position);
        }
        void RoamState(float distanceFromPlayer)
        {
            lineOfChase = lineOfSight;
            if (SightDecision(distanceFromPlayer, lineOfHears[0], OperatorType.less))
            {
                SetAddAnimation(AddAnimType.noticePlayer);
                Chase(distanceFromPlayer, centerPlayer.position);
            }
            else
                Roaming();
        }
        #endregion

        void Roaming()
        {
            enemyState = EnemyState.roam;
            float distance = Vector2.Distance(centerTransf.transform.position, goals[m_NextGoal].position);
            //GenDebug("dist goal " + centerTransf.transform.position + " - " + goals[m_NextGoal].position + " = " + distance);
            if (distance < goalDist)
            {
                ChangeNextGoal();
            }
            agent.destination = goals[m_NextGoal].position;
            prevPos = transform.position;
            SetAnimation(AnimType.walk, centerTransf.transform.position, goals[m_NextGoal].position);
        }

        void ChangeNextGoal()
        {
            changeGoalCd -= Time.deltaTime;
            if (changeGoalCd >= 0)
                return;
            changeGoalCd = changeGoalTime;
            switch (enemyRoamPattern)
            {
                case EnemyRoamPattern.random:
                    int curGoal = m_NextGoal;
                    m_NextGoal = RandomGenerator.GenerateRandomNumber(0, goals.Length);
                    //get next goal until not same as current goal
                    while (m_NextGoal == curGoal)
                    {
                        m_NextGoal = RandomGenerator.GenerateRandomNumber(0, goals.Length);
                    }
                    break;
                //fix
                default:
                    m_NextGoal = m_NextGoal < goals.Length - 1 ? m_NextGoal + 1 : 0;
                    break;
            }
        }

        void Chase(float distanceFromPlayer, Vector3 targetPos)
        {
            //isBackToOriPos = false;
            enemyState = EnemyState.chase;
            switch (enemyChaseType)
            {
                case EnemyChaseType.shoot:
                    ShootDecision(distanceFromPlayer, targetPos);
                    break;
                case EnemyChaseType.shootAndHit:
                    if (enemyShootAndHitState == EnemyShootAndHitState.shoot)
                        ShootDecision(distanceFromPlayer, targetPos);
                    else
                        HitDecision(distanceFromPlayer, targetPos);
                    break;
                //hit
                default:
                    HitDecision(distanceFromPlayer, targetPos);
                    //transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
                    break;
            }
            fireCooldown -= Time.deltaTime;
            SetAnimation(AnimType.walk, center.transform.position, targetPos);
        }
        #region Decision
        void ShootDecision(float distanceFromPlayer, Vector3 targetPos)
        {
            //GenDebug("distanceFromPlayer " + distanceFromPlayer + " fireCooldown " + fireCooldown);
            if (distanceFromPlayer > shootingRange)
                agent.SetDestination(targetPos);
            // transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
            else if (distanceFromPlayer <= shootingRange && fireCooldown < 0)
                Shoot();
            if (enemyChaseType == EnemyChaseType.shootAndHit)
                ChangeShootAndHitState();
        }

        void HitDecision(float distanceFromPlayer, Vector3 targetPos)
        {
            if (enemyHitType != EnemyHitType.normal && distanceFromPlayer < hitRange)
            {
                GenDebug("EnemyState.attack");
                enemyState = EnemyState.attack;
            }
            else if (distanceFromPlayer < lineOfChase)
                agent.SetDestination(targetPos);
            if (enemyChaseType == EnemyChaseType.shootAndHit)
                ChangeShootAndHitState();
        }
        void ChangeShootAndHitState()
        {
            if (shootAndHitCd >= 0)
                return;
            shootAndHitCd = shootAndHitTime;
            if (enemyShootAndHitState == EnemyShootAndHitState.shoot)
            {
                enemyShootAndHitState = EnemyShootAndHitState.hit;
                agent.stoppingDistance = 0;
            }
            else
            {
                enemyShootAndHitState = EnemyShootAndHitState.shoot;
                if (shootingRange > 1)
                    agent.stoppingDistance = shootingRange - 1f;
            }
        }

        bool SightDecision(float distFromPlayer, float distTigger, OperatorType oprt)
        {
            switch (enemySightType)
            {
                case EnemySightType.raycast:
                    return SeePlayerWideView();
                case EnemySightType.raycastAndDistance:
                    return SeePlayerWideView() || DetectPresenceDesicion(distFromPlayer, distTigger, oprt);
                //distance
                default:
                    return DetectPresenceDesicion(distFromPlayer, distTigger, oprt);
            }
        }
        bool DetectPresenceDesicion(float distFromPlayer, float distTigger, OperatorType oprt)
        {
            if (enemyState == EnemyState.roam && HearDecision(distFromPlayer))
                return true;
            return DistDecision(distFromPlayer, distTigger, oprt);
        }
        bool DistDecision(float distFromPlayer, float distTigger, OperatorType oprt)
        {
            switch (oprt)
            {
                case OperatorType.less:
                    return distFromPlayer < distTigger;
                case OperatorType.lessEqual:
                    return distFromPlayer <= distTigger;
                case OperatorType.more:
                    return distFromPlayer > distTigger;
                case OperatorType.moreEqual:
                    return distFromPlayer >= distTigger;
                case OperatorType.notEqual:
                    return distFromPlayer != distTigger;
                //equal
                default:
                    return distFromPlayer == distTigger;
            }
        }
        bool HearDecision(float distFromPlayer)
        {
            for (int i = 0; i < lineOfHears.Length; i++)
            {
                //player in range of lineOfHears && player make soundlouder than lineOfHearSounds
                if (distFromPlayer < lineOfHears[i] && lineOfHearSounds[i] <= playerController.playerSoundVol)
                    return true;
            }
            return false;
        }
        #endregion

        #region Attack
        void Shoot()
        {
            GenDebug("shoot");
            fireCooldown = timeFire;
            Vector2 direction = centerPlayer.position - transform.position;
            SetAnimation(AnimType.shoot, transform.position, centerPlayer.position);
            switch (enemyShootType)
            {
                case EnemyShootType.homing_single:
                    Projectile projectile1 = GetProjectile(projectileId[1]);
                    projectile1.Launch(bulletParent.transform.position, centerPlayer, 1);
                    break;
                case EnemyShootType.multi:
                case EnemyShootType.multi_homing:
                    //TODO
                    break;
                //single
                default:
                    Projectile projectile0 = GetProjectile(projectileId[0]);
                    projectile0.Launch(bulletParent.transform.position, centerPlayer, 1);
                    break;
            }
        }
        Projectile GetProjectile(int id)
        {
            GameObject projectileObject = objectPool.GetPooledObject(id);
            if (projectileObject)
            {
                projectileObject.transform.position = bulletParent.transform.position;
                projectileObject.transform.rotation = Quaternion.identity;
                projectileObject.SetActive(true);
            }
            return projectileObject.GetComponent<Projectile>();
        }

        void Attack()
        {
            GenDebug("Attack");
            switch (enemyHitType)
            {
                case EnemyHitType.attack:
                    //TODO
                    animator.SetTrigger("attack");
                    break;
                case EnemyHitType.dash_slam:
                    animator.SetTrigger("dash_slam");
                    Vector2 direction = centerPlayer.position - center.transform.position;
                    rigidbody2D.AddForce(direction * attackForce);
                    break;
                default:
                    break;
            }
            attackCooldown = attackTime;
            SetAnimation(AnimType.walk, center.transform.position, centerPlayer.position);
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
        #endregion

        #region Animation
        void SetAnimation(AnimType animType, Vector2 ownerPos, Vector2 goalPos)
        {
            float distX = ownerPos.x - goalPos.x;
            float uDistX = distX < 0 ? -distX : distX;
            float distY = ownerPos.y - goalPos.y;
            float uDistY = distY < 0 ? -distY : distY;
            if (uDistX > uDistY)
            {
                directionX = distX < 0 ? 1 : -1;
                directionY = 0;
            }
            else
            {
                directionX = 0;
                directionY = distY < 0 ? 1 : -1;
            }
            animator.SetFloat("Move X", directionX);
            animator.SetFloat("Move Y", directionY);
            switch (animType)
            {
                case AnimType.attack:
                    animator.SetTrigger("attack");
                    animator.SetBool("walk", false);
                    break;
                case AnimType.dash_slam:
                    animator.SetTrigger("dash_slam");
                    animator.SetBool("walk", false);
                    break;
                case AnimType.shoot:
                    animator.SetTrigger("shoot");
                    animator.SetBool("walk", false);
                    break;
                case AnimType.dash:
                    break;
                case AnimType.fix:
                    animator.SetTrigger("Fixed");
                    break;
                //walk
                default:
                    moveDirection.Set(directionX, directionY);
                    animator.SetBool("walk", true);
                    break;
            }
        }
        void SetAddAnimation(AddAnimType addAnimType)
        {
            switch (addAnimType)
            {
                case AddAnimType.noticePlayer:
                    addAnimator.SetTrigger("noticePlayer");
                    break;
                case AddAnimType.playerEscaped:
                    addAnimator.SetTrigger("playerEscaped");
                    break;
            }
        }
        #endregion
        bool SeePlayer()
        {
            /*
            The RaycastHit2D type variable hit stores the result of a raycast, which is obtained by calling 
            Physics2D.Raycast. There are multiple versions of Physics2D.Raycast; the version used here has 
            four arguments (values you can pass to it).
            The first argument is the starting point for the ray. Here it is an upward offset from the position of 
            the PlayerCharacter GameObject, which will test from the center of the sprite rather than its feet.
            The second argument is the direction that the player character is looking, using moveDirection.
            The third argument is the maximum distance for the ray — 1.5 units here keeps the test within a 
            short distance from the start point, because the characters aren’t shouting.
            The final argument defines a layer mask, which is a way to only check within specified layers. 
            Any layers that are not within the mask are ignored during the intersection test. The NPC layer is 
            the only layer relevant for this test, so it’s the only one in the mask.
            */
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2D.position + Vector2.up * 0.2f,
                moveDirection, lineOfChase, LayerMask.GetMask("Character"));
            RaycastHit2D hit2 = Physics2D.Raycast(rigidbody2D.position + Vector2.up * 0.2f,
                moveDirection, lineOfChase, LayerMask.GetMask("CharacterTrigger"));
            if (hit.collider.GetComponent<PlayerController>() ||
                hit2.collider.GetComponent<PlayerTrigger>().playerController)
                return true;
            return false;
        }
        //wide angle raycasting
        bool SeePlayerWideView()
        {
            if (Vector2.Distance(transform.position, centerPlayer.position) < lineOfChase)
            {
                Vector2 directionToPlayer = (centerPlayer.position - transform.position).normalized;
                float angleBetweenGuardAndPlayer = Vector2.Angle(moveDirection, directionToPlayer);
                //GenDebug("angleBetweenGuardAndPlayer " + angleBetweenGuardAndPlayer);
                if (angleBetweenGuardAndPlayer < viewAngle / 2)
                {
                    //Debug.Log("viewAngle " + viewAngle);
                    if (Physics2D.Linecast(transform.position, centerPlayer.position, LayerMask.GetMask("Obstacle")))
                        return false;
                    RaycastHit2D hit = Physics2D.Linecast(transform.position, centerPlayer.position, LayerMask.GetMask("Character"));
                    RaycastHit2D hit2 = Physics2D.Linecast(transform.position, centerPlayer.position, LayerMask.GetMask("CharacterTrigger"));
                    if (hit.collider.GetComponent<PlayerController>() ||
                        hit2.collider.GetComponent<PlayerTrigger>().playerController)
                        return true;
                }
            }
            return false;
        }

        public void GetAttacked(MicrobarAnimType mat, int damageAmount)
        {
            GenDebug("GetAttacked " + damageAmount);
            float dmgPercentage = (float)damageAmount / (float)MAX_HP * (float)100;
            health -= damageAmount;
            //microbarSpriteAnim.Damage(mat, dmgPercentage);
            //Instantiate(hitEffect, transform);
            GameObject hitEffect = objectPool.GetPooledObject(effectId);
            if (hitEffect)
            {
                hitEffect.transform.position = effectParent.transform.position;
                hitEffect.SetActive(true);
            }
            if (health > 0)
            {
                PlaySound(EnemySound.hurted);
                float distanceFromPlayer = Vector2.Distance(center.transform.position, centerPlayer.position);
                if (enemyState == EnemyState.roam)
                    SetAddAnimation(AddAnimType.noticePlayer);
                Chase(distanceFromPlayer, centerPlayer.position);
                return;
            }
            Fix();
        }
        void Fix()
        {
            PlaySound(EnemySound.fixed_by);
            broken = false;
            //rigidbody2d.simulated = false;
            SetAnimation(AnimType.fix, center.transform.position, centerPlayer.position);
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

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(center.transform.position, lineOfSight);
        }

        void GenDebug(string str)
        {
            if (isDebug)
                Debug.Log(str);
        }
    }
}