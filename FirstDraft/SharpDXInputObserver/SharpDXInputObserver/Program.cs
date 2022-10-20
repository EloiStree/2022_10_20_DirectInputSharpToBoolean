using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SharpDXInputObserver
{

    public class UserGUID2BooleanCollection {

        public UserGUIDBool2Boolean[] m_booleanListener = new UserGUIDBool2Boolean[] {        };
        public UserGUIDFloat2Boolean[] m_floatListener = new UserGUIDFloat2Boolean[] {     };
    }

    public class UserGUIDBool2Boolean {
        public string m_deviceGuid;
        public string m_booleanName;
        public int m_buttonId;
        public bool m_valueLookedFor=true;
    }
    public class UserGUIDFloat2Boolean {
        public string m_deviceGuid;
        public string m_booleanName;
        public string m_floatName;
        public float m_minValuePercent=0.1f;
        public float m_maxValuePercent=1f;
        public bool m_inverseBoolean;
        public int m_floatMaxValue=65535;
    }

    public class DictionaryFetch
    {
        public  DictionaryFetch(in DirectInput input) {
            m_directInput = input;
        }

        public DirectInput m_directInput;
        public Dictionary<string, GuidJoystickKey > m_joystickRegister = new Dictionary<string, GuidJoystickKey>();
  
        public void ListenToGuidDevice(string guidId)
        {
            if (!m_joystickRegister.ContainsKey(guidId)) {
                m_joystickRegister.Add(guidId, new GuidJoystickKey(guidId, new Joystick(m_directInput, new Guid(guidId))));
            }
        }
        public void GetListOfTracked(out List<string> guidKey) { guidKey = m_joystickRegister.Keys.ToList(); }
        public void GetListOfTracked(out List<GuidJoystickKey> joystick) { joystick = m_joystickRegister.Values.ToList(); }
    }

    class Program
    {
        static void Main(string[] args)
        {
            bool useDebug_RawValueObserved = true;
            bool useDebug_RawValueObservedFrame = false;
            UserGUID2BooleanCollection userInput = new UserGUID2BooleanCollection();
            userInput.m_booleanListener = new UserGUIDBool2Boolean[] {
            new UserGUIDBool2Boolean(){m_deviceGuid="5dd7a170-4728-11ed-8001-444553540000",
                m_buttonId=0,m_valueLookedFor=true , m_booleanName="Apple0"},
            new UserGUIDBool2Boolean(){m_deviceGuid="5dd7a170-4728-11ed-8001-444553540000",
                m_buttonId=1,m_valueLookedFor=true , m_booleanName="Apple1"},
            new UserGUIDBool2Boolean(){m_deviceGuid="5dd7a170-4728-11ed-8001-444553540000",
                m_buttonId=2,m_valueLookedFor=true , m_booleanName="Apple2"},
            new UserGUIDBool2Boolean(){m_deviceGuid="5dd7a170-4728-11ed-8001-444553540000",
                m_buttonId=3,m_valueLookedFor=true , m_booleanName="Apple3"},
            new UserGUIDBool2Boolean(){m_deviceGuid="5dd7a170-4728-11ed-8001-444553540000",
                m_buttonId=4,m_valueLookedFor=true , m_booleanName="Apple4"},

            new UserGUIDBool2Boolean(){m_deviceGuid="6f1d2b60-d5a0-11cf-bfc7-444553540000",
                m_buttonId=0,m_valueLookedFor=true , m_booleanName="Mouse 0"},
            new UserGUIDBool2Boolean(){m_deviceGuid="6f1d2b60-d5a0-11cf-bfc7-444553540000",
                m_buttonId=1,m_valueLookedFor=true , m_booleanName="Mouse 1"},
            new UserGUIDBool2Boolean(){m_deviceGuid="6f1d2b60-d5a0-11cf-bfc7-444553540000",
                m_buttonId=2,m_valueLookedFor=true , m_booleanName="Mouse 2"},
            new UserGUIDBool2Boolean(){m_deviceGuid="6f1d2b60-d5a0-11cf-bfc7-444553540000",
                m_buttonId=3,m_valueLookedFor=true , m_booleanName="Mouse 3"},
            new UserGUIDBool2Boolean(){m_deviceGuid="6f1d2b60-d5a0-11cf-bfc7-444553540000",
                m_buttonId=4,m_valueLookedFor=true , m_booleanName="Mouse 4"}
            };

            userInput.m_floatListener = new UserGUIDFloat2Boolean[] {
            new UserGUIDFloat2Boolean(){m_deviceGuid="5dd7a170-4728-11ed-8001-444553540000",
                m_booleanName="WingLeft",m_minValuePercent=0.55f, m_maxValuePercent=1f,m_floatName="X"},
            new UserGUIDFloat2Boolean(){m_deviceGuid="5dd7a170-4728-11ed-8001-444553540000",
                m_booleanName="WingRight",m_minValuePercent=0f, m_maxValuePercent=0.45f,m_floatName="X"},
            new UserGUIDFloat2Boolean(){m_deviceGuid="5dd7a170-4728-11ed-8001-444553540000",
                m_booleanName="WingDown",m_minValuePercent=0.55f, m_maxValuePercent=1f,m_floatName="Y"},
            new UserGUIDFloat2Boolean(){m_deviceGuid="5dd7a170-4728-11ed-8001-444553540000",
                m_booleanName="WingUp",m_minValuePercent=0.0f, m_maxValuePercent=0.45f,m_floatName="Y"},
            new UserGUIDFloat2Boolean(){m_deviceGuid="5dd7a170-4728-11ed-8001-444553540000",
                m_booleanName="SpeedUp",m_minValuePercent=0.55f, m_maxValuePercent=1f,m_floatName="Z"},
            new UserGUIDFloat2Boolean(){m_deviceGuid="5dd7a170-4728-11ed-8001-444553540000",
                m_booleanName="SpeedDown",m_minValuePercent=0.0f, m_maxValuePercent=0.45f,m_floatName="Z"},
            };


            var directInput = new DirectInput();
            DictionaryFetch fetch = new DictionaryFetch(in directInput);

            DisplayAllDevices(directInput);
            for (int i = 0; i < userInput.m_booleanListener.Length; i++)
            {
                fetch.ListenToGuidDevice(userInput.m_booleanListener[i].m_deviceGuid);
            }
            for (int i = 0; i < userInput.m_floatListener.Length; i++)
            {
                fetch.ListenToGuidDevice(userInput.m_floatListener[i].m_deviceGuid);
            }

            fetch.GetListOfTracked(out List<GuidJoystickKey> joysticks);
            foreach (var joystick in joysticks)
            {
                var allEffects = joystick.m_joystick.GetEffects();
                foreach (var effectInfo in allEffects)
                    Console.WriteLine("Effect available {0}", effectInfo.Name);
                joystick.m_joystick.Properties.BufferSize = 128;
                joystick.m_joystick.Acquire();
            }

            GuidOffsetValureDictionary valueDictionary = new GuidOffsetValureDictionary();
            NamedBooleanChangeObserver boolRegister = new NamedBooleanChangeObserver();
            boolRegister.m_onBooleanChanged += (in string t, in bool d) => {
                Console.WriteLine(string.Format("Changed: {0} {1}", t, d));
            };
            // Poll events from joystick
            while (true)
            {
                foreach (var joystick in joysticks)
                {
                    joystick.m_joystick.Poll();
                    var datas = joystick.m_joystick.GetBufferedData();
                    foreach (var state in datas) {
                        if (useDebug_RawValueObserved) { 
                        Console.WriteLine(string.Format("{0} | Value {1} | Offset{2}",
                            state.Offset, state.Value, state.RawOffset));
                        }
                        valueDictionary.SetWith(in joystick.m_guid,  state.Offset,  state.Value,  state.RawOffset);
                    }
                }
               
                foreach (var floatValue in userInput.m_booleanListener)
                {
                    ProjectTempUtility.GetEnumOfStringJoystickOffset(
                        floatValue.m_buttonId, out bool converted, out JoystickOffset offset);
                    if (converted) {
                        ProjectTempUtility.GetIdFrom(in floatValue.m_deviceGuid, offset, out string id);

                        if (valueDictionary.m_valueHolder.ContainsKey(id))
                        {
                            bool isTrue = valueDictionary.m_valueHolder[id].m_value > 0;
                            if (useDebug_RawValueObservedFrame)
                                Console.WriteLine(string.Format("TB: {0} | {1}", isTrue, id));
                            boolRegister.PushValue(floatValue.m_booleanName, isTrue == floatValue.m_valueLookedFor);
                        }
                    } 
                }
                foreach (var floatValue in userInput.m_floatListener)
                {
                    ProjectTempUtility.GetEnumOfStringJoystickOffset(
                        floatValue.m_floatName, out bool converted, out JoystickOffset offset);
                    if (converted)
                    {
                        ProjectTempUtility.GetIdFrom(in floatValue.m_deviceGuid, offset, out string id);
                        if (valueDictionary.m_valueHolder.ContainsKey(id))
                        {
                            float pct = valueDictionary.m_valueHolder[id].m_value / (float)floatValue. m_floatMaxValue;
                            bool isTrue = pct >= floatValue.m_minValuePercent
                                && pct <= floatValue.m_maxValuePercent ;
                            if (useDebug_RawValueObservedFrame)
                                Console.WriteLine(string.Format("TB: {0} | {1}", isTrue, id));
                            boolRegister.PushValue(floatValue.m_booleanName, floatValue.m_inverseBoolean  ?!isTrue: isTrue);
                        }
                    }
                }

                Thread.Sleep(1);
            }
        }

        private static void DisplayAllDevices(DirectInput directInput)
        {
            Console.WriteLine("Device(s) List");
            foreach (var item in ProjectTempUtility.GetEnum<DeviceType>())
            {
                foreach (var deviceInstance in directInput.GetDevices(item,
                        DeviceEnumerationFlags.AllDevices))
                {
                    Console.WriteLine(" "+ item.ToString() + ", "+ deviceInstance.InstanceName + ": " + deviceInstance.InstanceGuid); 
                
                }
            }
        }

       
    }
}

public class NamedBooleanChangeObserver {

    public Dictionary<string, bool> m_namedBoolean = new Dictionary<string, bool> ();
    public delegate void OnBooleanChanged(in string name,in  bool newValue);
    public OnBooleanChanged m_onBooleanChanged;
    public void PushValue(in string name, in bool value) {
        if (!m_namedBoolean.ContainsKey(name))
        {
            m_namedBoolean.Add(name, value);
            if (m_onBooleanChanged != null)
                m_onBooleanChanged(in name, in value);
        }
        else {
            bool current = m_namedBoolean[name];
            if (current != value) {
                m_namedBoolean[name] = value;
                if (m_onBooleanChanged != null)
                    m_onBooleanChanged(in name, in value);
            }
        }

    }
}

public class GuidJoystickKey
{
    public string m_guid;
    public Joystick m_joystick;
    public GuidJoystickKey(string guid, Joystick joystick)
    {
        m_guid = guid;
        m_joystick = joystick;
    }
}

public class GuidOffsetValureDictionary
{
    public Dictionary<string, Value> m_valueHolder = new Dictionary<string, Value>();
    public class Value
    {
        public string m_guid;
        public int m_value;
        public int m_rawOffset;
    }

    public void SetWith(in string target, in JoystickOffset offset, in int value, in int rawOffset)
    {
        ProjectTempUtility. GetIdFrom(in target, in offset, out string id);
        if (!m_valueHolder.ContainsKey(id))
            m_valueHolder.Add(id, new Value() { m_guid = id});
        m_valueHolder[id].m_value = value;
        m_valueHolder[id].m_rawOffset = rawOffset;
    }
}


public class ProjectTempUtility {
    public static void GetIdFrom(in string guid, in JoystickOffset offset, out string id) { id = guid + "_" + offset; }


    public static IEnumerable<T> GetEnum<T>()
    {

        return Enum.GetValues(typeof(T)).Cast<T>();
    }

    public static void GetEnumOfStringJoystickOffset(string id, out bool converted, out JoystickOffset value)
    {
        converted = Enum.TryParse<JoystickOffset>(id, true, out value);

    }
    public static void GetEnumOfStringJoystickOffset(int buttonId0To127, out bool converted, out JoystickOffset value)
    {
        if (buttonId0To127 > 127) {
            converted = false;
            value = JoystickOffset.Buttons127;
            return;
        }
        buttonId0To127 += 48;
        value = (JoystickOffset)buttonId0To127;
        converted = true;
    }
}