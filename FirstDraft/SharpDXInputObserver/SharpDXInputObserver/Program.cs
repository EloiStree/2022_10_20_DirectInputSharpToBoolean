using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SharpDXInputObserver
{

    class Program
    {
        static void Main(string[] args)
        {

            //UdpCommandPusher    udpPush     = new UdpCommandPusher(2503);
            //MemoryCommandPusher memoryPush  = new MemoryCommandPusher("DirectInput2Boolean");

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
                            bool realBool = floatValue.m_inverseBoolean ? !isTrue : isTrue;
                            boolRegister.PushValue(floatValue.m_booleanName, realBool);

                            string cmd = "ᛒ" + (realBool ? 1 : 0) + floatValue.m_booleanName;
                            //udpPush.PushCommand(cmd);
                            //memoryPush.PushCommand(cmd);


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
