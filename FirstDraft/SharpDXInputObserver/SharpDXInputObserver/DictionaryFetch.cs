using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpDXInputObserver
{
    public class DictionaryFetch
    {
        public  DictionaryFetch(in DirectInput input) {
            m_directInput = input;
        }

        public DirectInput m_directInput;
        public Dictionary<string, GuidJoystickKey > m_joystickRegister = new Dictionary<string, GuidJoystickKey>();
  
        public void ListenToGuidDevice(string guidId)
        {
            if (!m_joystickRegister.ContainsKey(guidId))
            {
                try
                { 
                m_joystickRegister.Add(guidId, new GuidJoystickKey(guidId, new Joystick(m_directInput, new Guid(guidId))));
                }
                catch { 
                    //DIRTY CATCH
                }
            }
        }
        public void GetListOfTracked(out List<string> guidKey) { guidKey = m_joystickRegister.Keys.ToList(); }
        public void GetListOfTracked(out List<GuidJoystickKey> joystick) { joystick = m_joystickRegister.Values.ToList(); }
    }
}
