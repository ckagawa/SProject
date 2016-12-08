Quick overview of checkpoint:
This checkpoint was intended to demonstrate a basic understanding of the platform and show that basic functionality can be attained.
Basic Player Control:
>Basic Horizontal and Vertical movement: player can perform movement in either x direction and jump(manually increase y position)
>Basic non-AI enemy elements: enemies can be placed on screen and have some presence within scene(collide with player and scene)
>Collectables and environment behavior and functionality: actors interact with the environment and other elements in the scent
>Other elemtents: basic UI sample

Quick Checklist:
Player moves left and right when commanded
player stops moving left and right when not commanded
player can jump
player and enemy move along ground when in contact with stage
player does not pass through enemy
collectable spheres appear on map
spheres disappear when in contact with player is made
spheres are not affected by gravity or other forces
bar appears above player when spheres are collected
size of bar is proportional to number of spheres collected.

Notes: I am probably going to overhaul all of the current scripting to make it cleaner and easier to reuse, but I have it as it is
so that this demo will function for the time being. There is some unused code at the moment that was intended to be used for limiting
the player to jumping from the ground.At the moment unintended behavior includes player and enemy sprites rotating under certain
conditions due to gravity, player being able to jump infinitely in the air, and objects falling forever if pushed past the edge of the stage.

Controls are unedited from Unity defaults and should be
left and right arrow key: move
space bar: jump