# 🌙《月下幻行》Demo — Development Log

> **《月下幻行》**（Moonlit Demo）是一款正在开发中的 2D Roguelike 游戏 Demo。
> **Moonlit Demo** is a 2D Roguelike game prototype currently in development.

> 主角在由梦境具象化而成的「盲区」战斗，探索被他人遗忘的记忆，最终被凌白“救走”，离开盲区。
> The protagonist fights within the “Blind Zone,” a place where dreams take physical form, exploring memories forgotten by others before eventually being “rescued” by Ling Bai and escaping the Blind Zone.

---

## 2026-07

### 2026-07-15

**《月下幻行》demo版企划诞生了！**
**The concept for the Moonlit Demo was born!**

---

### 2026-07-17

**WASD移动完成。**
**WASD movement completed.**

一个青色方块在屏幕上动了起来。这是伊尔最初的生命形态。
A cyan square started moving across the screen. This was Ylir's very first form of life.

---

### 2026-07-20

**完成了镜头跟随，并重新规划了战斗系统。现在已经有点肉鸽的味儿了。**
**Camera following was completed, and the combat system was redesigned. It was starting to feel a little Roguelike.**

---

### 2026-07-21

**完成鼠标坐标和左右键输入，后面的战斗需要用到。**
**Mouse position detection and left/right mouse input were completed. These will be needed for the combat system later on.**

---

### 2026-07-23

**完成了Hover的逻辑。现在鼠标晃到可交互物品上有白色描边。**
**Completed the Hover system. Interactable objects now display a white outline when the mouse moves over them.**

---

### 2026-07-24

**完成了鼠标的切换，鼠标移到怪物上会变成剑。现在战斗初具雏形。**
**Completed the cursor switching system. The cursor now changes into a sword when hovering over an enemy. The combat system is starting to take shape.**

---

### 2026-07-26

**完成了攻击的锁定。**
**Completed attack target locking.**

---

### 2026-07-28

**完成了子弹飞行。《月下幻行》的第一颗子弹诞生了！**
**Completed projectile movement. The very first bullet of Moonlit Demo was born!**

---

### 2026-07-30

**完成了子弹销毁。现在碰到怪物会销毁。最基本的游戏玩法已经诞生了！！**
**Completed projectile destruction. Bullets are now destroyed when they hit an enemy. The most basic gameplay loop has finally been born!!**

---

## 2026-08

### 2026-08-01

**完成了怪物血条和子弹伤害的逻辑。现在子弹可以打死怪物了。**
**Completed enemy health and projectile damage logic. Bullets can now actually kill enemies.**

---

### 2026-08-03

**完成了攻击范围内的索敌攻击！现在有有效的攻击范围了！**
**Completed target detection and attacks within the attack range! The player now has a proper attack range!**

---

### 2026-08-04

**完成了 攻击范围内外的鼠标图案切换。现在在攻击范围以内是剑，攻击范围以外是眼睛！**
**Completed cursor switching based on attack range. The cursor is now a sword inside the attack range and an eye outside of it!**

---

### 2026-08-05

**完成了普通攻击的子弹冷却。现在子弹有发射频率上限了，当玩家在子弹冷却期间试图发射时，子弹将无法发射。**
**Completed the basic attack projectile cooldown. Bullets now have a maximum firing rate. If the player attempts to attack while the projectile is still on cooldown, no new bullet will be fired.**

---

### 2026-08-06

**完成了R键持续攻击的基本功能。R键=自动攻击，可开关。功能仍待完善。**
**Completed the basic functionality of R-key continuous attacking. R = Auto Attack, which can be toggled on and off. The system still needs further improvements.**

---

### 2026-08-07

**添加了R键可以手动切换攻击目标的功能。修改了自动攻击开启时仍能手动攻击的bug。添加了R键自动寻找范围内最近目标持续攻击的功能。**
**Added the ability to manually switch attack targets with the R key. Fixed a bug that allowed manual attacks while Auto Attack was active. Added automatic targeting of the nearest enemy within range and continuous attacking while Auto Attack is enabled.**

---

### 2026-08-08

**添加了自动攻击开启时，目标死亡会自动寻找下一目标攻击的功能。**
**Added automatic target switching when the current target dies while Auto Attack is active.**

---

### 2026-08-09

**完成了持续攻击开启时，怪物远离攻击范围会取消锁定，简称拉脱。**
**Completed the “leash” system. When continuous attack is active, an enemy leaving the attack range will automatically break the player's lock-on.**

**拉脱后，范围内重新进入怪物时，需要再次按R键方可开启自动攻击。**
**After breaking the lock-on, enemies re-entering the attack range will not automatically restart the attack. The player must press R again to resume Auto Attack.**

**完成了攻击范围指示器的出现时机。**
**Completed the timing logic for displaying the attack range indicator.**

**R键功能已完成！**
**The R-key system is now complete!**

---

### 2026-08-10

**完成了基础空A（暂时不能根据角色朝向改变空A方向）。**
**Completed the basic “blind attack” (free-aim attack). The firing direction cannot yet change according to the character's facing direction.**

**以及左键在未点击到范围内怪物的情况下，会锁定最近的怪物攻击。**
**Also added automatic targeting of the nearest enemy when the player clicks the left mouse button without directly clicking on an enemy within attack range.**

**已将该项目上传至GitHub！**
**The project has now been uploaded to GitHub!**

---

### 2026-08-14

**增加了空A可以根据左右朝向改变攻击方向的功能。**
**Added the ability for free attack to change its attack direction based on the character's left or right orientation.**


