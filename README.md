# AlphaZero methodology in Python with Game in C#

The code containing the AlphaZero methodology is adapted from: https://github.com/AppliedDataSciencePartners/DeepReinforcementLearning

The Connect Four game is converted to C# and replaced the original Python version. The purpose is to enable any game written in the .NET language to integrate with the Python model seamlessly.  

main.py - Train the model<br>
play_game_with_AI.py - Play against the trained model<br>
play_between_versions.py - Test the model by playing between different versions<br>

The project is developed using Visual Studio 2017 with Python Tools for Visual Studio and Python environment 3.7. Ensure the pythonnet module is installed to enable Python to call .NET code (pip install pythonnet).

A pre-trained model is available in the file path: \run_archive\connect4\run0001\models\version0206.h5
