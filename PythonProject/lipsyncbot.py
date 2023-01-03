import pyttsx3
import time
import speech_recognition as sr
import webbrowser
import shutil
import os
from pydub import AudioSegment
from gtts import gTTS
import datetime
import pathlib
import socket


host, port = "127.0.0.1", 25001
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
connected = False
uname = "Dennis"

engine = pyttsx3.init('sapi5') 
voices = engine.getProperty('voices') 
engine.setProperty('voice', voices[1].id)

#AudioSegment.converter = "C:\\misc\\ffmpeg\\ffmpeg.exe"                    
#AudioSegment.ffprobe   = "C:\\misc\\ffmpeg\\ffprobe.exe"


def speak(audio): 
    tts = gTTS(audio)
    tts.save("sound.mp3")
    sound = AudioSegment.from_mp3("sound.mp3")
    #tts.save(os.getcwd() + '\\sound.mp3')
    #sound = AudioSegment.from_mp3(os.getcwd() + '\\sound.mp3')
    sound.export("sound.wav", format="wav")
    try:
        global connected
        if connected == False:
            sock.connect((host,port))
            connected = True
        sock.sendall("a".encode("utf-8"))
    finally:
        print ("#")
        print(audio)
        #sock.close()
    #socket.send(b"sound.wav") #str(pathlib.Path(__file__).parent.absolute()) + 
    #soundduration = socket.recv()
    
  
def wishMe(): 
    hour = int(datetime.datetime.now().hour) 
    if hour>= 0 and hour<12: 
        speak("Good Morning Sir") 
   
    elif hour>= 12 and hour<18: 
        speak("Good Afternoon Sir")    
   
    else: 
        speak("Good Evening Sir")
   
    #assname =("Jarvis 1 point o") 
    #speak("I am your Assistant") 
    #speak(assname) 

  
def usrname(): 
    speak("What should i call you sir")
    time.sleep(1)
    uname = takeCommand()
    if uname == "None":
        uname = "Dennis"
    speak("Welcome Mister")
    speak(uname)
    columns = shutil.get_terminal_size().columns 
      
    print("#####################".center(columns)) 
    #print("Welcome Mr.", uname.center(columns)) 
    print("#####################".center(columns)) 
      
    speak("How can i Help you Sir")
    time.sleep(1)

def takeCommand(): 
      
    r = sr.Recognizer() 
      
    with sr.Microphone() as source:
        r.energy_threshold = 2500
          
        print("Listening...") 
        r.pause_threshold = 1
        audio = r.listen(source) 
   
    try: 
        print("Recognizing...")     
        query = r.recognize_google(audio, language ='en-in') 
        print(f"User said: {query}\n") 
   
    except Exception as e: 
        print(e)     
        print("Unable to Recognizing your voice.")
        return "None"
      
    return query 

if __name__ == '__main__':
##    time.sleep(2)
##    speak("What do you want")
##    exit()
    
##    print("Speak...")
##    time.sleep(5)
##    speak("You jerk. What do you want")
##    time.sleep(2)
##    print("Speak...")
##    time.sleep(5)
##    speak("Why? You left me. Fuck off")
##    time.sleep(142)
    

    
    clear = lambda: os.system('cls') 

    # This Function will clean any 
    # command before execution of this python file 
    clear() 
    wishMe() 
    usrname()

    while True: 
          
        query = takeCommand().lower()

        if 'open youtube' in query: 
            speak("Here you go to Youtube") 
            webbrowser.open("youtube.com") 

        elif 'bored' in query:
            speak("Me too")
            
        elif 'like manga' in query:
            print("You have a good taste of Music sir")
            speak("You have a good taste of Music sir")

        elif 'play aleyna' in query:
            print("Hasiktir lan")
            speak("Hasiktir lan")

        elif 'love me' in query:
            speak("I Love you")
            speak(uname)

        elif 'exit' in query:
            os._exit(0)
