# IMG420-Assignment4
This project integrates a range of features developed in C#, allowing for a quality gameplay.

## "WayFinder"
WayFinder is a small 2D platformer developed in C#. The goal is simple: 
- Collect all the coins scattered throughout the level while avoiding the enemy that chases you through navigation pathfinding.
- The player can:
  - Move left and right (left & right arrow keys).
  - Jump onto platforms (spacebar).
  - Collect coins.
  - Lose lives when in contact with the enemy.
  - Wins once all coins have been collected.

## Core Features
### Tile Based World
The level is constructed through using TileMap nodes and TileSets. Each tile will represent the floor, walls, and the platforms which the player can land on without falling through.

### Character Player
Player uses CharacterBody2D with CollisionShape2D to allow interaction between the enemy and other elements such as collecting coins. The player also utilizes gravity, simple physics, and move_and_slide().

### Enemy Pathfinding
The enemy uses CharacterBody2D and with NavigationAgent2D as its child. The Navigation2D computes the paths to locate the target (the player) across a NavigationRegion2D (painted walkable tiles). Enemies continuously updates the player's current position and will redirect if needed to continue in reaching its target. Once the enemy has made contact with the target, the target will take damage.

### Collisions
Players, enemies and the tilemap have a CollisionShape2D. This allows the player and the enemy to not be able to walk through tiles nor fall through them.

### Collectables
Each coin has an Area2D with CollisionShape2D. When the player interacts with the coin, it will automatically collect the coin, thus the coin will be removed and the players HUD will be updated, incrementing the total coins collected.

### Particles
There are two implementations of particles which one is in the background and the other is spawned once an interation with the coin occurs.
- Coin Pickup: A once shot, star-like burst occurs once the player has picked up the coin, releasing an explosion effect.
- Background: Slow rising leaf-like particles will release once the game has started for a more visually appealing background.
