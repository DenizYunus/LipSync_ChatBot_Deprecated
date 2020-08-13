# LipSync ChatBot

This chatbot is basically a python speech recognition chatbot supported with visualized character able to lip-sync.

# Usage and How It Works

  - Open the Unity project.
  - Run the Python project.
  - When you run the Python project, Unity will detect it over localhost. Python uses TTS to create the sound and uses AudioSegment library to convert the mp3 file to wav file. (Unity cannot load MP3 files with WWW api to load from filesystem.)
  - Python sends the command to Unity to let Unity read sound file from filesystem and make the model lip sync the sound.

# Parameters

  - You can use "* volume" attribute to change the power of lip sync (distance between lips etc.).
  - You can use "limit = Mathf.clamp" line's end to change the max distance lips can get.

> Later on this project can be used to create Google Assistant supported chat bots etc.