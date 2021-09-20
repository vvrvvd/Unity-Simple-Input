# Unity Simple Input
[![Unity 2020.1+](https://img.shields.io/badge/unity-2020.1%2B-blue.svg)](https://unity3d.com/get-unity/download) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Table Of Contents

- [Introduction](#introduction)
- [Features](#features)
- [System Requirements](#system-requirements)
- [Dependencies](#dependencies)
- [Installation](#installation)
- [Overview](#overview)
	- [Generating actions](#generating-actions)
	- [Using actions](#using-actions)
- [License](#license)

## Introduction <a name="introduction"></a>

**Unity Simple Input**  is a simple wrapper for Unity InputSystem with custom input action set generator and continuous OnPressed event implementation. 

## Features <a name="features"></a>

- Input action sets script generator
- Three types of Actions: button, float, and Vector2
- Gamepad, Keyboard, and Mouse bindings
- OnPressed event invoked every frame the action is being active

## System Requirements <a name="system-requirements"></a>

Unity 2020.1 or newer.

## Dependencies <a name="dependencies"></a>

[Input System](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.1/manual/index.html)

[Editor Toolbox](https://github.com/arimger/Unity-Editor-Toolbox)

## Installation <a name="installation"></a>

1. The package is available in Unity Package Manager via git URL. Follow up [this](https://docs.unity3d.com/Manual/upm-ui-giturl.html) Unity page for detailed instructions. Git URL:
```
https://github.com/vvrvvd/Unity-Simple-Input.git#upm
```
2. You can also install Simple Input by simply downloading repository zip file and copying Assets folder content to your Unity project.

## Overview <a name="overview"></a>

### Generating actions <a name="generating-actions"></a>

Actions are grouped in action sets and may be created through scriptable object generator:
 1. Create new scriptable Action Set Generator in Project view (```Simple Input->Input System->Input Action Set```).
 2. Set **Script Name**, **Relative Namespace** and add actions with bindings to **Action Set Template** list.

	<img src="https://i.imgur.com/LfvvIbt.png">
 
 3. Click **Generate script** button to generate Action Set Script.

### Using actions <a name="using-actions"></a>

To use actions you must first create instance of **InputSystemSimpleManager** class and invoke it's **Update** method every frame.
There is a MonoBehaviour wrapper implementation for **InputSytemSimpleManager** called **InputSystemSimpleMonoManager** that you may simply add to your scene.

1. Create an instance of your **InputActionSet** in code:
```C#
testActionSet = new TestActionSet();
``` 

2. Register the instance to your **InputSystemSimpleManager**:
```C#
inputSystemManager.RegisterInputActionSet(testActionSet);
``` 

3. Bind events to actions:
```C#
testActionSet.Run.BindEvent(Run, InputEventType.Down);
testActionSet.Move.BindEvent(Move);

...

private void Run();
private void Move(Vector2 input);
``` 

4. You can also unbind events from actions:
```C#
testActionSet.Run.UnbindEvent(Run, InputEventType.Down);
testActionSet.Move.UnbindEvent(Move);
```
	
 ## License <a name="license"></a>
 
[MIT](https://opensource.org/licenses/MIT)
