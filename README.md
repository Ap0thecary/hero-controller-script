# hero-controller-script
A C# script for creating a simple 1st or 3rd person player in Unity with support for modern and legacy control schemes

Current Features:
- Flexible, intelligent first-person controls

Planned Features:
- Third-person control
- Yes there will probably be wall jumping
- A legacy control scheme such as the one seen in older RPG games
- Transition between First and Third Person

SETUP:
If the prefab object fails to work, try these steps
1) Create an empty GameObject
2) Add a RigidBody and a CapsuleCollider
3) Create a Camera, attach it, and place it where the head should be
4) At the HeroController & HeroCamera scripts to their respective objects
