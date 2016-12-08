# SProject
Build is not included as either committing or pulling breaks build.  
Build should be reconstructable by building from unzipped project without modifying parameters.
##Controls
Left/Right Arrow->Left/Right Movement  
1->StationaryAttack  
2->Projectile  
4->Jump  
Other mappings should not be relevent, mapping may be changed either in Unity settings or in initial execution window of build  
note: mouse click on start button begins game  
##Benchmarks
Note: Benchmarks taken in order of appearance from proposal document, will be arranged in a friendlier matter later
###Gameplay
--`2D movement`-left and right buttons produce horizontal movement, jump produces vertical movement, player falls when not on a platform  
Known Issues: collision is unreliable, jumping into the top corner of a surface may result in clipping through platorms  
--`Multiple stages`-passing transitions(large blue panels) loads different stages, dying to boss reloads level  
Known Issues: killing boss results in "soft lock." program should be exited once this has occurred as no further significant actions will occur  
--`Deterministic Action`:  
 --Jumps are of fixed height unless obstructed, stationary jumps may not be influenced  
 --Moving jumps may not change except for becoming stationary, attacking will stop horizontal momentum  
Known Issues: attacks cooldowns are intentional but attacks, particularly while jumping, will fairly often fail to come out. Attack collision occurs through raytracing so attack sprites visually collide without producing anticipated effect. Stationary(1 key) attack can often be called multiple times without hitting enemy _Semi-intentional,enemy invincibility is intended but not to so great a degree_  
--`Path Prediction`--_originally intended as additional functionality, not implemented_  
--`Limited Enemy Action` - Enemy(large red block) sends attack either left, right, or in both directions  
Known Issues: Landing attacks causes enemy behavior to change rythmn, firing more often than intended  
--`Map construction`:  
 --inital map contains 3 red blocks, touching them artificially alters player target assessment  
Known Issue: Blocks have highly selective collision, difficult to hit(color change indicate a hit)  
 --second map contains distribution of points.  
Known Issue: points also have selective collision, jumping diagonally into them mostly mitigates issue  
 --third map contains enemy  
Known Issue: third map has no completion state so program will run indefinitely on this stage  
--`Player Intention`- Debug text displays current mode of operation and current prediction of player intention  
--`Pattern Recognition`--_Abandoned in favor of reinforcement learning_  
###Benchmarks by Category
__Things in parentheses generally approximate test cases I checked while working, some may be insufficient to convincingly display functionality__  
`Basic Functionality`
* player moves in 2 dimensions(left, right, jumping, falling)
* player attacks function(1 and 2 should produce different attacks)
* UI(health displays and scales to reflect damage, score increments, debug window updates)
* collision(typically unable to pass through stage, projectiles reduce player life, enemy dies if hit ~5 times)
* collection(collectables appear on second stage, touching them destroys them and increments score)
* debugging(debug window properly displays a current mode and target, first stage triggers change color when touched)__Very Specific window for first stage triggers__  
`Prediction`
* player target prediction changes based on actions(touching stage 1 boxes)__not level of functionality desired but stopgap to prove system functionality, next point more indicative of desired behavior__
* player target updates as actions change(collecting points changes target to point collection, idling for a sufficient amount of time
causes target to revert, collecting multiple points causes target to take much longer to revert relative to single point)__This behavior is generally broken if any of the boxes in first stage are touched, boxes skew assessment tremendously where point behavior is significantly closer to how a fully functional build should behave__  
`Reinforcement Learning`
* performing a successful action causes AI to perform action more frequently(being hit by an attack makes enemy use that attack more frequently)
* performing an unsuccessful action causes AI to perform action less frequently __This doesn't work. Couldn't tune values of build enough to make this possible, any inclusion of this effectively wiped the AI from how quickly it dropped every action's fitness__
* performing uniformly successful/unsuccessful actions does not affect the AI(not getting hit should cause the AI to maintain a generally random course of actions) __The ai, because of how it rolls randoms, generally dislikes one of its 3 attacks. While it does result in a skewed distribution of attacks even with no stimulus, this skew should not change significantly unless the player begins taking hits__  
`Combined interaction`
* predictions change the metric used by the evaluator for deciding fitness(being attacked in dont die mode affects frequency of successful attack where being hit in point mode makes no difference to AI) __This might not function. when debugging with debug notifications On the expected behavior seems to be reflected in logs, but actual behavior seems to suggest that every target state is treated as dont_die mode and every hit is changing command frequency even when it shouldnt__  

###WriteUp  
Everything listed as a known issue here, as well as some thing not listed, will be explained in further detail in the final writeup;
