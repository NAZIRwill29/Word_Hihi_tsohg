# Word_Hihi_tsohg

## 🕸 Master Wireframe – *Word_Hihi_tsohg*

```mermaid
flowchart TD

    %% Core
    GM[GameManager<br/>central game state control]
    IM[InputManagerSO<br/>input storage & bindings]

    %% Word System
    WI[WordInput<br/>checks keystrokes]
    WM[WordManager<br/>tracks words & typing progress]
    WS[WordSpawner<br/>spawns word objects]
    WUI[WordUI<br/>shows words on screen]

    %% Ghost System
    GC[GhostController<br/>ghost logic & AI]
    GSp[GhostSpawner<br/>spawns ghosts]
    GUI[GhostUI<br/>ghost HP / warnings]
    SM[StruggleManager<br/>handles ghost attacks]

    %% Book
    BS[BookSystem<br/>lore & powers]

    %% UI
    UIM[UIManager<br/>interface flow]
    MM[MainMenu]
    PM[PauseMenu]
    GO[GameOverUI]

    %% Utility
    AM[AudioManager<br/>SFX & BGM]
    SL[SceneLoader<br/>scene transitions]
    YS[YSort / YSortUniversal<br/>2D sprite depth]

    %% Connections
    GM --> IM
    IM --> WI
    WI --> WM
    WM --> WS
    WM --> WUI
    WM --> GC

    GC --> GSp
    GC --> GUI
    GC --> SM

    GM --> BS
    GM --> UIM
    UIM --> MM
    UIM --> PM
    UIM --> GO

    GM --> AM
    GM --> SL
    GM --> YS
