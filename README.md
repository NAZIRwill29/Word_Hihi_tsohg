# Word_Hihi_tsohg

```mermaid
flowchart TD

%% ===================
%% CORE SYSTEMS
%% ===================
InputManagerSO --> InputReader --> GameManager
InputReader --> RebindManager --> RebindActionUI --> UIManager --> GameManager
GameManager --> AudioManager & SceneLoader

%% ===================
%% PLAYER SYSTEMS
%% ===================
InputManagerSO --> PlayerInput --> PlayerController
PlayerController --> PlayerMovement --> PlayerAbilities
PlayerAbilities --> PlayerShoot & PlayerMagic & PlayerMelee
PlayerController --> PlayerHealth --> CombatUI --> UIManager
UIManager --> GameManager

%% ===================
%% ENEMY SYSTEMS
%% ===================
EnemyAI --> EnemyController --> EnemyHealth
EnemyController --> EnemyCombat --> CombatManager --> EnemyUI --> GameManager

%% ===================
%% ENVIRONMENT SYSTEMS
%% ===================
PlayerController --> Interactables --> Collectibles --> PlayerHealth
Collectibles --> PlayerAbilities
Interactables --> AreaEffects --> PlayerHealth & EnemyHealth
AreaEffects --> UIManager --> GameManager

%% ===================
%% COMBAT SYSTEMS
%% ===================
InputManagerSO --> PlayerInput --> CombatManager
CombatManager --> PlayerCombat --> EnemyCombat
CombatManager --> CombatUI --> UIManager --> GameManager

%% ===================
%% UI SYSTEMS
%% ===================
InputManagerSO --> UIManager --> MainMenu & InGameUI & DialogueUI
UIManager --> DialogueManager --> DialogueUI --> GameManager

%% ===================
%% NAVIGATION SYSTEMS
%% ===================
NavMesh --> EnemyAI --> EnemyController
NavMesh --> NavMeshModifier --> NavMeshSurface --> GameManager

%% ===================
%% UTILITY SYSTEMS
%% ===================
ObjectPooling --> PooledObject --> GameManager
FlyweightPattern --> StatsDataFlyweight & AbilityDataFlyweight --> GameManager

%% ===================
%% SPECIALIZED SYSTEMS
%% ===================
MiniGameManager --> MiniGameSO --> UIManager
TypingSystem --> WordSystem --> WordUI --> GameManager

%% ===================
%% VFX & AUDIO
%% ===================
GameManager --> VFXManager --> ExplosionEffect & HitEffect
GameManager --> AudioManager --> ObjectAudio & ObjectAudioMulti

