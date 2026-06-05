# Snooker Game

A professional **Object-Oriented snooker game** built with **C# and Windows Forms** featuring realistic physics, collision detection, and an attractive visual interface.

## 🎮 Features

### Game Mechanics
- ✅ **White Cue Ball** - Player-controlled ball with physics simulation
- ✅ **Colored Solid Balls** - 15 balls in triangular pyramid formation
- ✅ **Realistic Physics** - Friction, velocity damping, and smooth ball movement
- ✅ **Collision Detection** - Ball-to-ball and wall collisions with proper physics
- ✅ **Pocket System** - 6 pockets on the table (corners and sides)
- ✅ **Scoreboard** - Real-time score tracking with collision points

### Controls
- **Mouse Movement** - Rotate cue stick to aim
- **Arrow Keys (Up/Down)** - Adjust shot power
- **Left Mouse Click** - Strike the cue ball
- **R Key** - Reset the game

### Visual Interface
- 🎨 Dark green pool table with realistic appearance
- 🎯 Color-coded solid balls (Red, Yellow, Blue, Black)
- 💡 Power indicator bar showing shot strength
- 📊 Live score display and ball counter
- ⚡ Smooth anti-aliased graphics at 60 FPS

## 🏗️ OOP Architecture

### Classes

#### `Ball` (Abstract Base Class)
- Handles physics simulation (position, velocity, friction)
- Collision detection and response
- Rendering with borders

#### `CueBall` (Inherits from Ball)
- White ball controlled by the player
- Strike method with velocity control
- Special rendering with highlight effect

#### `SolidBall` (Inherits from Ball)
- Colored balls with numbers
- Tracking pocketed status
- Individual color properties

#### `Cue`
- Manages cue stick positioning and rotation
- Power management for shots
- Calculates strike velocity based on angle and power

#### `GameManager`
- Central game state controller
- Handles ball initialization in triangle formation
- Updates game physics each frame
- Manages collisions between all objects
- Tracks score and pocketed balls
- Coordinates rendering of all game elements

#### `GameForm` (Windows Form)
- Main game window and event handler
- Renders graphics using GDI+
- Handles keyboard and mouse input
- Implements 60 FPS game loop with Timer

## 📁 Project Structure

```
snooker-game/
├── Ball.cs           # Abstract base ball class
├── CueBall.cs        # White cue ball
├── SolidBall.cs      # Colored balls
├── Cue.cs            # Cue stick
├── GameManager.cs    # Game logic and state
├── GameForm.cs       # UI and rendering
└── Program.cs        # Entry point
```

## 🚀 Getting Started

### Requirements
- Visual Studio 2019 or later
- .NET Framework 4.7.2+
- Windows 10+

### Installation

1. Clone the repository:
```bash
git clone https://github.com/khadija-og/snooker-game.git
cd snooker-game
```

2. Open `snooker-game.csproj` in Visual Studio

3. Build the solution (Ctrl+Shift+B)

4. Run the game (F5)

## 🎯 How to Play

1. **Aiming Phase**
   - Move your mouse to rotate the cue stick
   - Use **Up Arrow** to increase shot power
   - Use **Down Arrow** to decrease shot power
   - Watch the power indicator bar fill up

2. **Shooting Phase**
   - Click to strike the cue ball
   - Observe ball movements and collisions

3. **Scoring**
   - +10 points per cue ball-solid ball collision
   - +50 points per ball pocketed

4. **Reset**
   - Press **R** to start a new game

## 🔧 Physics Implementation

### Friction Model
Balls gradually slow down using a friction coefficient (0.985) applied each frame.

### Collision Response
- Elastic collision calculation between balls
- Impulse-based physics with energy loss (0.9 coefficient)
- Automatic separation to prevent ball overlap
- Wall bouncing with energy dissipation (0.8 coefficient)

### Velocity Constraints
- Maximum velocity limit to prevent instability
- Minimum velocity threshold to stop near-stationary balls

## 📊 Game State Machine

```
┌─────────────┐
│   Aiming    │ ← Player aims and sets power
└─────┬───────┘
      │ Click to strike
      ↓
┌─────────────┐
│  Shooting   │ ← Balls in motion
└─────┬───────┘
      │ All balls stop
      ↓
┌─────────────┐
│   Playing   │ ← Ready for next shot
└─────────────┘
```

## 🎨 Visual Design

- **Color Scheme**: Dark green table with contrasting ball colors
- **Rendering**: Anti-aliased graphics for smooth appearance
- **UI Elements**: 
  - Score counter (top-left)
  - Pocketed ball count (top-right)
  - Power indicator (below cue stick)
  - Game instructions (bottom)
  - State information (bottom-left)

## 📈 Future Enhancements

- [ ] Sound effects and music
- [ ] Multiplayer support
- [ ] Difficulty levels
- [ ] Different game modes (8-ball, 9-ball)
- [ ] Statistics and achievements
- [ ] Customizable table size and physics
- [ ] Replay system
- [ ] AI opponent

## 📝 License

This project is open source and available under the MIT License.

## 👨‍💻 Author

Created by **khadija-og** as a demonstration of OOP principles in C# game development.

---

**Enjoy the game! 🎱**
