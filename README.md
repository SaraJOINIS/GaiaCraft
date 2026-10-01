# README

## Documentation de notre projet

Notre projet *GaiaCraft* propose de consulter un manuel scolaire visualisable en réalité augmentée (AR) sur smartphone, comportant trois chapitres : la **tectonique des plaques**, la formation et le comportement d'un **trou noir**, et la **Bataille du débarquement de Normandie de 1944**. Un **menu principal** permet d'accéder à ces trois chapitres, chacun étant modélisé par une scène sur **Unity**.

Voici le lien vers notre documentation [Notion](https://www.notion.so/Unity-c178e74906ce49e6aa3adf7b84facc3c), explicitant entre autres le contenu de nos scènes et les outils utilisés sur Unity.


## Unity

Nous utilisons la version **2022.3.20.f1** de Unity.

## Vuforia (SDK pour le rendu AR)

Nous utilisons la version **10.27.3** de Vuforia.

### Installation de Vuforia

Si Vuforia n'est pas encore installé sur le PC de l'utilisateur souhaitant ouvrir notre projet sur Unity :

1. Télécharger le fichier `add-vuforia-package-10-27-3` depuis le site web de [Vuforia](https://developer.vuforia.com/downloads/sdk).
2. Importer le package en faisant un drag&drop vers Unity.
3. S'assurer que la **caméra AR** du projet est bien cochée dans l'Inspecteur Unity.

### Problèmes avec Vuforia lors de l'ouverture du projet

En cas d'erreur avec Vuforia lors de l'ouverture du projet :

1. Ouvrir le fichier `manifest.json` (situé dans le dossier `Packages` du projet).
2. Supprimer la ligne suivante :
   ```json
   "com.ptc.vuforia.engine": "file:com.ptc.vuforia.engine-10.27.3.tgz"
   ```
3. Réimporter le package `add-vuforia-package-10-27-3` dans Unity.

## Packages nécessaires au rendu des shaders associés à la scène du trou noir

Via le **Package Manager** de Unity, installer les packages suivants :

- **ShaderGraph**

- **URP (Universal Rendering Pipeline)** :

  - Utilisé pour l'effet de distorsion de la lumière autour du trou noir (effet de Fresnel).
  - Modifie le pipeline de rendu de Unity de la manière suivante :
    1. Les objets opaques de la scène sont rendus et stockés dans une texture dédiée.
    2. Les éléments de transparence sont ensuite rendus en utilisant les informations de cette texture intermédiaire.
  - Référence : [Vulkan Graphics Pipeline](https://vulkan-tutorial.com/Drawing_a_triangle/Graphics_pipeline_basics/Introduction)

- **Probuilder** :

  - Utilisé pour créer l'effet de distorsion de l'espace-temps sous forme d’un plan quadrillé en forme d'entonnoir.
  - Permet de générer un plan subdivisé représentant le champ gravitationnel induit par le trou noir.

## Rendu du projet en AR sur smartphone

### Configuration Unity

1. Dans Unity, aller dans **File** -> **Build Settings** :
   - Cliquer sur **Add Open Scenes**.
   - Aller dans **Player Settings** > **XR Plugin Management** (l'installer si ce n'est pas déjà fait) > cocher **AR Core**.

### Installation sur un appareil Android

1. Connecter le smartphone Android au PC.
2. Copier-coller le fichier `.apk` généré précédemment dans un sous-dossier que l'on crée dans le dossier **Carte SD** ou **Mémoire interne** de l'Android.
3. Installation :
   - Ouvrir le gestionnaire de fichiers sur le smartphone.
   - Se rendre dans **Carte SD** ou **Mémoire interne**, selon l'emplacement choisi.
   - Cliquer sur le fichier `.apk`, puis suivre les instructions de la fenêtre qui s'ouvre alors pour installer l'application.
4. Une fois installée, ouvrir l'application Unity sur le smartphone.
5. La caméra principale du smartphone doit alors s'ouvrir.
6. Diriger la caméra vers l’**image target** imprimée afin d'afficher la scène en AR.

