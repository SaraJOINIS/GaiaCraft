# GaiaCraft

## Project documentation

*GaiaCraft* project offers a textbook that can be viewed in augmented reality (AR) on a smartphone, consisting of three chapters: **plate tectonics**, the **formation and behavior of a black hole** and the **1944 Normandy landings**. A **main menu** provides access to these three chapters, each of them being modeled as a scene in **Unity**.

Here is the link to the [Notion](https://www.notion.so/Unity-c178e74906ce49e6aa3adf7b84facc3c) documentation which explains among other things the content of the scenes and the tools used in Unity.

## Unity

*GaiaCraft* project uses version **2022.3.20.f1** of Unity.

## Vuforia (AR rendering SDK)

*GaiaCraft* project uses version **10.27.3** of Vuforia.

### Vuforia installation

If Vuforia is not yet installed on the PC of the user who wants to open the project in Unity:

1. Download the `add-vuforia-package-10-27-3` file from the [Vuforia](https://developer.vuforia.com/downloads/sdk) website.
2. Import the package by dragging and dropping it into Unity.
3. Make sure the project's **AR camera** is checked in the Unity Inspector.

### Potential issues with Vuforia

If an error occurs with Vuforia when opening the project:

1. Open the `manifest.json` file (located in the project's `Packages` folder).
2. Delete the following line:
   ```json
   "com.ptc.vuforia.engine": "file:com.ptc.vuforia.engine-10.27.3.tgz"
   ```
3. Reimport the `add-vuforia-package-10-27-3` package into Unity.

## Packages required to render the black hole scene shaders

Using Unity's **Package Manager**, install the following packages:

- **ShaderGraph**

- **URP (Universal Rendering Pipeline)**

  - Used to simulate the distortion of light around the black hole (Fresnel effect).
  - Modifies Unity's rendering pipeline as follows:
    1. Opaque objects in the scene are rendered and stored in a dedicated texture.
    2. Transparent elements are then rendered using the information from this intermediate texture.
  - Reference : [Vulkan Graphics Pipeline](https://vulkan-tutorial.com/Drawing_a_triangle/Graphics_pipeline_basics/Introduction)

- **Probuilder**

  - Used to create the effect of space-time distortion in the form of a funnel-shaped grid map.
  - Allows to generate a subdivided map representing the gravitational field induced by the black hole.

## AR rendering of the project on a smartphone

### Unity configuration

In Unity, go to `File ➔ Build Settings`:
- Click on `Add Open Scenes`.
- Go to `Player Settings ➔ XR Plugin Management` (install if it isn't already) and check `AR Core`.

### Installation on an Android device

1. Connect the Android smartphone to the PC.
2. Copy and paste the previously generated `.apk` file into a subfolder in the `Internal Storage` (or `SD Card`) folder on the Android device.
3. Installation :
   - Open the file manager on the smartphone.
   - Go to `Internal Storage` (or `SD Card`, depending on the chosen location).
   - Tap the `.apk` file and follow the on-screen instructions to install the application.
4. Once installed, open the Unity application on the smartphone.
5. The smartphone's main camera should then activate.
6. Point the camera at the printed **image target** to display the AR scene.

