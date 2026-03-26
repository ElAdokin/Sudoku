# Sudoku Game (Unity WebGL)

A browser-based Sudoku game developed with Unity and deployable via WebGL technology.
For testing you can play it at https://nicolascasal.com

## 🎮 Features

*   Play Sudoku puzzles directly in your web browser.
*   Infinite Sudoku Generator.
*   Interactive game board with cell selection.
*   Input via mouse or on-screen pad (for touch devices).
*   Timer functionality to track your solving time.
*   Different difficulty levels (Easy, Medium, Hard).
*   Ability to save and load game progress (client-side).
*   Basic validation feedback for correct or incorrect number placement.
*   Responsive design for various screen sizes.

## 🔍 Browser Requirements

This game runs as a Unity WebGL application. It requires a modern browser that supports WebGL.

**Supported Browsers:**
*   Chrome
*   Firefox
*   Edge (Microsoft Edge based)
*   Safari (newer versions)
*   Opera

**Note:** WebGL may be disabled by default in some browsers. You might need to enable it in the browser's settings.

## 🧩 Building the Game

This project is set up for Unity. To build the WebGL version:

1.  **Install Unity Hub** and make sure you have a compatible version of Unity Hub and Unity Editor installed (check the Unity version requirement in the project settings if unsure).
2.  Open this repository in the Unity Hub or open the project folder in the Unity Editor.
3.  Go to `File > Build Settings`.
4.  Select `WebGL` as the target platform.
5.  Configure your WebGL build settings (API, scripting backend, etc.) if needed.
6.  Click `Build and Run`.

Alternatively, you can just click `Build` and place the resulting `.unitypackage` or `.unityproject` (if you want to open it) in the Unity Hub to start the build process.

## 🎮 How to Play

1.  **Start the Game:** Open the generated HTML file (or navigate to the URL provided after building) in your compatible web browser.
2.  **Select a Cell:** Click on the cell where you want to place a number.
3.  **Enter a Number:** Select the number (1-9) on your screen. Some browsers also support clicking a number pad that appears on-screen (especially for touch devices).
4.  **Check Numbers:** Press "Check" button for know if numbers are correct.
5.  **Change Difficulty:** Select the desired difficulty level before or after starting the game (if the option is available).
6.  **Save/Load:** Use the continue button to continue with your progress to the local browser storage.
7.  **Win:** Fill the grid completely with correct numbers according to Sudoku rules and pres "Check" button (each row, column, and 3x3 box must contain all numbers 1-9 without repetition).

## 📜 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📧 Contact

For questions or feedback, reach out to:
GitHub: [@ElAdokin](https://github.com/ElAdokin)
