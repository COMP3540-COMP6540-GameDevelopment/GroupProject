## Technical Design Document (TDD)
---

### Game Title: Pixel Quest: Puzzle Adventure

### Document Version
- **Version**: 0.1
- **Date**: 2024/10/29
- **Contributors**: 
  - Programmer: osuCarl, Eddyhzz, Eva Lee, SenyuYang

---

### 1. Introduction

#### 1.1 Purpose
The purpose of this TDD is to outline the technical architecture, systems, and requirements for the development of *Pixel Quest: Puzzle Adventure*. This document provides specifications for core systems, including movement, puzzle interactions, and turn-based combat, as well as technical requirements for performance optimization and deployment.

#### 1.2 Audience
This document is intended for the development team, including software engineers, game designers, and testers.

---

### 2. System Architecture

#### 2.1 Overview
The game’s architecture will follow a modular structure, enabling independent development of core features such as Player Movement, Puzzle Mechanics, Combat, and RPG Elements. Each module will be developed in Unity, leveraging its built-in components for physics, rendering, and UI.

#### 2.2 Architecture Diagram
- **Modules**:
  - Player Controller
  - Puzzle System
  - Combat System
  - RPG/Progression System
  - UI System
  - Audio Manager
  - Environment/World Management

*Comment: Create an architecture diagram showing interactions between modules, particularly Player, Puzzle, and Combat systems.*

---

### 3. Game Loop

#### 3.1 Main Game Loop
1. **Initialization**: Load player stats, inventory, and environment.
2. **Update Loop**: Manage player input, update physics, and handle AI in real-time.
3. **Rendering**: Use Unity’s rendering pipeline, optimized for pixel art, to handle graphics.
4. **Collision and Physics Updates**: Handle environmental interactions, puzzle triggers, and player-object interactions.
5. **Cleanup**: Save progress, manage memory, and reset variables at each level transition.

---

### 4. Modules and Components

#### 4.1 Player Controller
- **Input Handling**: Use Unity’s Input System for movement (left, right, jump) and interactions.
- **Collision Detection**: Implement Unity’s 2D physics for platform navigation and puzzle interactions.
- **Health and Inventory Management**: Track player’s health, items, and inventory.
  
*Comment: Specify if additional movement mechanics (e.g., dash, crouch) are planned, as well as detailed health and inventory mechanics.*

#### 4.2 Puzzle System
- **Core Mechanics**:
  - **Color Interaction**: Implement physics-based color interactions. Use Unity’s color system to create triggers based on hitting specific colors.
  - **Environmental Triggers**: Use Unity’s event system to trigger mechanisms when a player interacts with certain objects.
- **Puzzle Logic**:
  - Create scripts for combining colors and activating/deactivating pathways.
  - Develop templates for puzzle components (switches, levers, doors) that can be reused in different environments.

*Comment: Detail specific types of puzzles for each game area and list reusable puzzle components.*

#### 4.3 Combat System
- **Turn-Based Mechanics**:
  - Implement a turn-based system that tracks player and enemy turns.
  - Use a priority queue for managing turn order, allowing for agility-based turn bonuses if included.
- **Ability System**:
  - Define ability effects and categorize them (e.g., attack, defense, buff).
  - Implement cooldowns and energy costs for abilities.
- **Enemy AI**:
  - Basic AI for enemies, with specific attack/defense patterns that vary based on enemy type.
  - **Pathfinding**: Implement pathfinding for enemies using A* algorithm or Unity’s built-in NavMesh for 2D grid-based movement.

*Comment: List specific abilities, their effects, and details on the AI complexity and enemy behavior patterns.*

#### 4.4 RPG/Progression System
- **Leveling System**:
  - Implement experience gain, level-ups, and attribute progression.
- **Skill Tree**:
  - Develop a skill tree that unlocks new abilities as the player levels up.
- **Inventory and Item Management**:
  - Track coins, consumable items, and equipment.
  - Implement an item pickup and inventory UI.

*Comment: Specify details about the skill tree (number of skills, skill categories) and items (types, effects).*

#### 4.5 Environment/World Management
- **Level Loading**:
  - Use Unity’s scene management to load and unload levels efficiently.
  - Integrate a save/load system to preserve player progress and game state.
- **Environmental Effects**:
  - Implement dynamic lighting for color-based puzzles.
  - Use particle effects for elements like fire, magic, and color fusion.

*Comment: Define specific environmental details, such as level design layouts or biome-specific features.*

---

### 5. Data Management

#### 5.1 Data Structure
- **Player Data**: Store player stats, inventory, abilities, and save progress.
- **Puzzle State**: Track puzzle completion, color combinations, and activated triggers.
- **Combat State**: Track enemy health, positions, and player/enemy actions.

#### 5.2 Serialization and Storage
- Use JSON files for lightweight storage of game states and player data.
- Implement auto-save and manual save options.

*Comment: Define further specifics of data structure for non-player elements (e.g., level data, puzzle triggers).*

---

### 6. Algorithms and Processes

#### 6.1 Pathfinding (for Enemies)
- Implement A* pathfinding algorithm for enemy movement.
- **Optimization**: Cache paths where possible to avoid recalculations.

#### 6.2 Color Interaction and Fusion
- Define a set of color combination rules and use a lookup table or dictionary for efficient mapping.
- Use particle effects and color blending shaders to handle visual effects for color fusion.

*Comment: Detail specific color combination rules if additional complexity is required.*

#### 6.3 Turn-Based System Management
- **Turn Queue**: Use a priority queue to determine turn order, based on player/enemy stats.
- **Cooldown Management**: Track ability cooldowns and reset after each turn.

---

### 7. User Interface (UI) Implementation

#### 7.1 Main Menu and HUD
- **Main Menu**: Implement a UI for starting the game, loading progress, and accessing settings.
- **In-Game HUD**: Display player health, inventory, and current objectives.
- **Puzzle HUD**: Show color indicators and hints as needed.

#### 7.2 Inventory and Skill UI
- Design an inventory UI that displays items and allows players to use/equip items.
- Implement a skill tree UI that highlights skill progression and unlocks.

*Comment: Provide mockups or wireframes for major UI screens.*

---

### 8. Performance and Optimization

#### 8.1 Memory Optimization
- Optimize asset loading by preloading level-specific assets and unloading unused assets after each level.
- Use object pooling for frequently used objects like enemy instances and particle effects.

#### 8.2 Framerate Optimization
- Optimize game physics and color interactions to maintain stable framerates.
- Reduce draw calls by batching similar objects and using efficient sprite atlases.

*Comment: Specify hardware benchmarks and target framerate.*

---

### 9. Error Handling and Debugging

#### 9.1 Error Logging
- Implement logging for significant events (e.g., item pickup, level completion) and errors.
- Use Unity’s built-in debug log for quick testing and deploy a more robust logging solution for releases.

#### 9.2 Testing Protocols
- **Unit Tests**: Create tests for core gameplay mechanics (movement, combat, puzzle-solving).
- **Integration Tests**: Test interactions between systems (e.g., puzzles triggering environment changes).
- **Playtesting**: Regular playtests to evaluate puzzle difficulty, combat balance, and performance.

*Comment: Detail specific tests and a testing schedule if available.*

---

### 10. Deployment and Version Control

#### 10.1 Version Control
- **Tool**: Use Git for version control.
- **Branching Strategy**: Establish a branching strategy (e.g., feature, release, hotfix branches).

#### 10.2 Deployment
- **Build Pipeline**: Automate builds for each platform using Unity’s Cloud Build or a CI/CD solution.
- **Packaging**: Include separate asset bundles for each platform to optimize size and performance.

*Comment: Define additional build targets if needed for multi-platform support.*

---

### 11. Additional Notes

- **Further Detail on Biomes and Puzzle Types**: Detail would enhance level-specific implementation.
- **NPC and Enemy Variants**: Additional details on unique behaviors, dialogues, and interactions would support modular development.
  
---

This TDD provides a comprehensive foundation for *Pixel Quest: Puzzle Adventure*. Expanding on specifics like UI designs, color combination mechanics, skill tree attributes, and environmental elements would enable a more detailed implementation plan for the development team.