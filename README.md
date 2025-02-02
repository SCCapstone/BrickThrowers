# Daredivers

Daredivers is a co-operative game where players must work together to explore the depths of the sea to wringle out its riches. In the meanwhile, they are threatened by the dangers of the deep sea. The game is played in a 2D environment. Sucessfully completing expedition provides rewards for the players which can be used to further upgrade their equipment and abilities.

Importing the project into your computer is done by cloning the repository from GitHub, or alternatively downloading the project as a zip file through the green button labeled "Code". The project is built using Unity, and so you will have to have Unity and related subsidiaries installed prior to running the project, found in the next section. An IDE is also recommended for the project, such as Visual Studio Community for editing scripts, which Unity has support for.

## External Requirements

In order to build this project, as well as modify, you first have to install:

-   [Unity Hub](https://unity.com/download)
-   [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)

You will be asked to install a version of Unity; this project was built using Unity 2022.3.47f1. Bulding the project requires going to File -> Build and Run. Alterantively, you can go to File -> Build Settings and select the platform you want to build for. The project is built for Windows, and WebGL.

## Running

Use the build settings to select the platform you want to build for, and then click "Build and Run". The game will then be built and run on your computer as a Unity executable.

# Deployment

Publishing is the typical method of "deploying" a Unity game. If using the Windows platform, you can publish the game as an executable, which can be run on any Windows computer. You can then publish the game on a platform such as Steam, or itch.io.

WebGL builds can be published on a website, on Unity Play, or on a platform such as itch.io.

# Style Guide

We are to use Google's C# style guide for this project. The style guide can be found [here](https://google.github.io/styleguide/csharp-style.html).

# Testing

In 492 you will write automated tests. When you do you will need to add a
section that explains how to run them.

The unit tests are in `/test/unit`.

The behavioral tests are in `/test/casper/`.

## Testing Technology

**You must have the following installed to run the tests:**
- Unity Editor (at least 2022.3.47f1)
- Unity Test Framework

Tests can be found in the ```Assets/tests``` folder. The tests are written in C# and are run using the Unity Test Framework. The tests are run in the Unity Editor. Access this by Window -> General -> Test Runner.
They are seperated into two types: PlayMode tests and EditMode tests. PlayMode tests are used to test the game as a whole, while EditMode tests are used to test individual components of the game.

For play mode tests, some will require user input to continue with their tasks.  

## Running Tests

Explain how to run the automated tests.

# Authors

Reshlynt (Scott Do) - dobao98123@gmail.com

tylerstargel (Tyler Stargel) - thetylerstargel@gmail.com

Crunko (Joshua Kolbusz) - joshuakolbusz@gmail.com

kjthaoo (Kelly Thao) - kthao43726@gmail.com
