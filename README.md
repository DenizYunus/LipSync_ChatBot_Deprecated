# LipSync ChatBot for Unity

LipSync ChatBot is an innovative project that integrates Python's speech recognition capabilities with a visualized character able to lip-sync in Unity. The chatbot creates a seamless interaction experience, with the lips of the character syncing perfectly with the audio. The future development plan includes the improvement to work solely with Unity using the ChatGPT API.

## Table of Contents
1. [Introduction](#introduction)
2. [Features](#features)
3. [Usage and How It Works](#usage-and-how-it-works)
4. [Parameters](#parameters)
5. [Future Enhancements](#future-enhancements)

## Introduction
LipSync ChatBot offers a realistic and engaging user experience, making it suitable for games, simulations, and more.

## Features
- Speech recognition using Python
- Lip synchronization with visualized characters in Unity
- Flexibility in adjusting lip sync power and distance
- Compatibility with wav file formats

## Usage and How It Works
1. **Open the Unity project**: Prepare the Unity environment.
2. **Run the Python project**: Activate the speech recognition module.
3. **Unity Detection over localhost**: Unity will detect the Python project running over localhost.
4. **TTS and AudioSegment Library**: Python uses TTS to create sound, then the AudioSegment library converts the mp3 file to a wav file.
5. **Unity Reads Sound File**: Python sends a command to Unity, letting it read the sound file from the filesystem and make the model lip-sync to the sound.

## Parameters
- `* volume`: Change the power of lip sync (distance between lips, etc.).
- `limit = Mathf.clamp`: Adjust the line's end to change the maximum distance the lips can get.

## Future Enhancements
- Integration with ChatGPT API to make the chatbot much more intelligent and can be only run on Unity without Python project.

Feel free to contact me if you have any questions or need further assistance with this project.
