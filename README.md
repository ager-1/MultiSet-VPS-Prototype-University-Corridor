# 🌍 AR VPS Application

![Unity](https://img.shields.io/badge/Unity-100000?style=for-the-badge&logo=unity&logoColor=white)
![ARKit](https://img.shields.io/badge/ARKit-000000?style=for-the-badge&logo=apple&logoColor=white)
![iOS](https://img.shields.io/badge/iOS-000000?style=for-the-badge&logo=ios&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)

> **Overview**: Technical documentation and architectural walkthrough for the AR VPS (Visual Positioning System) Application, built using Unity, AR Foundation, and iOS ARKit.

## 📋 Table of Contents
- [Phase 1: Capturing the Campus Corridor](#-phase-1-capturing-the-campus-corridor)
- [Phase 2: Unity Integration and iOS XR Setup](#-phase-2-unity-integration-and-ios-xr-setup)
- [Phase 3: Setting Up Localization](#-phase-3-setting-up-localization)
- [Phase 4: NavMesh Setup, Pathfinding Scripts, and UI Integration](#-phase-4-navmesh-setup-pathfinding-scripts-and-ui-integration)
- [Conclusion and Looking Ahead](#-conclusion-and-looking-ahead)

---

## 📍 Phase 1: Capturing the Campus Corridor
The foundation of any Visual Positioning System relies entirely on the accuracy of its spatial map. For this initial step, the MultiSet scanner application was taken out into a corridor on the Asia Pacific University campus. 

- **Data Collection:** Systematically walked the space and captured the environment from varying angles.
- **Processing:** The cloud service received a dense collection of visual feature points, establishing a solid anchor for the coordinate system.
- **Digital Twin:** Once the upload finished, the application processed the scan into a workable digital twin and a spatial mesh. This raw map data set the stage for all complex routing logic.

## 🛠️ Phase 2: Unity Integration and iOS XR Setup
With the physical corridor successfully mapped, the data was brought into the Unity engine. The **MultiSet SDK** was imported to bridge the gap between the cloud map and the digital environment.

- **Target Device:** iPhone
- **XR Plugins:** Configured build settings for iOS, installed AR Foundation and Apple ARKit.
- **Rig Setup:** The core AR Session and AR Camera were set up, ensuring the camera was primed to feed real-time visual data to the MultiSet localization manager.

## 🔍 Phase 3: Setting Up Localization
Leveraged sample scenes included directly inside the MultiSet SDK to quicken the process.

1. Imported prepackaged Sample Scenes via Unity Package Manager.
2. The `Localization.unity` scene already had the core architecture laid out for basic localization functionality.
3. Selected the main manager object and located the `MapLocalizationManager` component. 
4. Pasted a specific **Map Code** generated from the developer portal. 

> Within minutes, the scene could recognize the physical campus corridor and snap the digital spatial map into perfect alignment!

## 🚀 Phase 4: NavMesh Setup, Pathfinding Scripts, and UI Integration
With the physical campus corridor scanned and the MultiSet localization running smoothly, a virtual AI agent was integrated to navigate an invisible augmented reality floor.

### 4.1 Runtime NavMesh Baking and the AI Agent
The core challenge in Unity AR navigation is that a standard `NavMeshAgent` requires baked geometry to walk on, but the MultiSet spatial map is purely mathematical offset data. To solve this, the `VPSNavManager` script was created.

- Dynamically calls `navSurface.BuildNavMesh()` at runtime the moment the phone successfully localizes. 
- An `Update` loop continually samples the AR camera position and uses `agent.Warp()` to instantly teleport the agent to the updated floor coordinates, tethering the pathfinding logic to the real-world location.

### 4.2 AR State Management
The `ARStateManager` script was built as the central brain for user experience, tracking four distinct states: `Initialising`, `Scanning`, `Localised`, and `TrackingLost`.

- Wired directly to the MultiSet localization events, the UI instantly reacts to the environment. 
- `OnLocalizationSuccess()` automatically triggers the `vpsNavManager.PlaceAgentOnNavMesh()` sequence once the cloud recognizes the corridor.

### 4.3 UI Interactions and Distance Tracking
A small modular script called `POIButtonBinder` attaches to UI buttons. When tapped, it triggers `NavigateHere()` and passes a target Point of Interest directly to the `NavigationController`.

The `SimpleNavUI` script runs continuously, checking `PathEstimationUtils` to get the remaining distance in meters. Because the AI agent acts as a tethered shadow, this distance calculation actively shortens as you walk down the physical hallway.

### 4.4 Cross Platform Deployment via macOS Virtual Machine
Since the primary workstation is a Windows laptop, deploying an iOS application required utilizing a macOS Virtual Machine.

After transferring the Xcode project folder from Unity, macOS security protocols tagged these foreign files with a quarantine attribute. To bypass this and fix file permissions, the following terminal commands were run:

```bash
# Recursively granted full read, write, and execute permissions.
chmod -R 777 <file destination>

# Recursively deleted the quarantine extended attribute.
sudo xattr -rd com.apple.quarantine <file destination>
```

Once cleared, Xcode successfully compiled the project and pushed the application to an iPhone.

### 4.5 The Final Output and Real Device Testing
- **Initialization:** The application boots directly into the scanning phase. 
- **Localization:** Once the environment is recognized, the UI switches to Localised and a spatial grid overlay aligns with the physical hallway.
- **Navigation:** Tapping the destination buttons instantly renders a trail of glowing pink arrows on the floor to guide the way, accompanied by a hovering robotic drone character. 

The real-time navigation works flawlessly, dynamically updating the path.

---

## 🔮 Conclusion and Looking Ahead
Completing this project demonstrated the immense power and practical challenges of mobile spatial computing. By bridging the MultiSet Visual Positioning System with Unity runtime navigation, an indoor navigation prototype was created that operates reliably inside physical spaces.

> The project proved that complex spatial problems can be solved through thoughtful software architecture. 

Decoupling the AI agent from standard physics and forcing dynamic runtime surface baking allowed the system to maintain accurate pathfinding. Navigating cross-platform deployment constraints using a virtual machine added valuable low-level terminal troubleshooting skills.

This prototype serves as a solid foundation for future spatial computing applications.
