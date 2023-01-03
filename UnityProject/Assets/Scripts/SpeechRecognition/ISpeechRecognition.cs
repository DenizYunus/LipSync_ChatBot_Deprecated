using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace Assets.Scripts.SpeechRecognition
{
    public interface ISpeechRecognition
    {
        void Initialize(ResultCallback sentenceComplete, ResultCallback wordComplete);
    }
}
