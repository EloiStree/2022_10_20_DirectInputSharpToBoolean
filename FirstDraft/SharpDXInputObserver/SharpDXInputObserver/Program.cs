using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using System.Text;
using System.Linq;
using System.Net.Sockets;
using static NamedBooleanChangeObserver;

namespace SharpDXInputObserver
{

    public class Program
    {

        public static AppConfigurationJson defaultDemoConfig = new AppConfigurationJson()
        {

            m_useFullDebugger = true,
            m_fullDebugDeviceId = new string[] { "72a91970-1a73-11ee-8001-444553540000" },
            m_secondFrequenceOfFullDebug = 1,
            m_floatIndexObserved = new UserGUIDFloat2Boolean[] {
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
            },
            m_booleanIndexObserved = new UserGUIDBool2Boolean[] {
            new UserGUIDBool2Boolean(){m_deviceGuid="72a91970-1a73-11ee-8001-444553540000",
                m_buttonId=0,m_valueLookedFor=true , m_booleanName="Apple0"},

            new UserGUIDBool2Boolean(){m_deviceGuid="72a98ea0-1a73-11ee-8002-444553540000",
                m_buttonId=0,m_valueLookedFor=true , m_booleanName="Apple0"},
            new UserGUIDBool2Boolean(){m_deviceGuid="72a98ea0-1a73-11ee-8002-444553540000",
                m_buttonId=1,m_valueLookedFor=true , m_booleanName="Apple1"},
            new UserGUIDBool2Boolean(){m_deviceGuid="72a98ea0-1a73-11ee-8002-444553540000",
                m_buttonId=2,m_valueLookedFor=true , m_booleanName="Apple2"},
            new UserGUIDBool2Boolean(){m_deviceGuid="72a98ea0-1a73-11ee-8002-444553540000",
                m_buttonId=3,m_valueLookedFor=true , m_booleanName="Apple3"},
            new UserGUIDBool2Boolean(){m_deviceGuid="72a98ea0-1a73-11ee-8002-444553540000",
                m_buttonId=4,m_valueLookedFor=true , m_booleanName="Apple4"},
            new UserGUIDBool2Boolean(){m_deviceGuid="72a98ea0-1a73-11ee-8002-444553540000",
                m_buttonId=5,m_valueLookedFor=true , m_booleanName="Apple5"},
            new UserGUIDBool2Boolean(){m_deviceGuid="72a98ea0-1a73-11ee-8002-444553540000",
                m_buttonId=6,m_valueLookedFor=true , m_booleanName="Apple6"},
            new UserGUIDBool2Boolean(){m_deviceGuid="72a98ea0-1a73-11ee-8002-444553540000",
                m_buttonId=7,m_valueLookedFor=true , m_booleanName="Apple7"},
            new UserGUIDBool2Boolean(){m_deviceGuid="72a98ea0-1a73-11ee-8002-444553540000",
                m_buttonId=8,m_valueLookedFor=true , m_booleanName="Apple8"},
            new UserGUIDBool2Boolean(){m_deviceGuid="72a98ea0-1a73-11ee-8002-444553540000",
                m_buttonId=9,m_valueLookedFor=true , m_booleanName="Apple9"},
            new UserGUIDBool2Boolean(){m_deviceGuid="72a98ea0-1a73-11ee-8002-444553540000",
                m_buttonId=10,m_valueLookedFor=true , m_booleanName="Apple10"},
            new UserGUIDBool2Boolean(){m_deviceGuid="72a98ea0-1a73-11ee-8002-444553540000",
                m_buttonId=11,m_valueLookedFor=true , m_booleanName="Apple11"},
            new UserGUIDBool2Boolean(){m_deviceGuid="72a98ea0-1a73-11ee-8002-444553540000",
                m_buttonId=12,m_valueLookedFor=true , m_booleanName="Apple12"},
            }
        };

        public static float GetTimeOfDebuglog() { return userConfig.m_secondFrequenceOfFullDebug; }
        public static AppConfigurationJson userConfig;
        public static DirectInput directInput;

        static void Main(string[] args)
        {

            /// TO DO LATER
            /// - Add an boolean observer of target device to debug more easily what device and button we want to change
            /// - Add a float oversver with death zone to conver to event of significant change on a device observed.
            /// - Add compatibility with infini mouse like the 3D connect that increase all time that convert with the software to classic joystick.


            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            Console.WriteLine("Hello and welcome.");
            Console.WriteLine("This tool is design to allows convertion of DirectInput to boolean value.");
            Console.WriteLine("Hope you enjoy.");
            Console.WriteLine("");
            Console.WriteLine("More tool like this one on: ");
            Console.WriteLine("https://eloi.page.link/bundle");
            Console.WriteLine("Source code: https://github.com/EloiStree/2022_10_20_DirectInputSharpToBoolean");
            Console.WriteLine("---------------");

            Console.WriteLine("---------------");

            //UdpCommandPusher    udpPush     = new UdpCommandPusher(2503);
            //MemoryCommandPusher memoryPush  = new MemoryCommandPusher("DirectInput2Boolean");

            bool useDebug_RawValueButtonObserved = false;
            bool useDebug_RawValueObservedFrame = false;

            LoadConfigJson();

            LoadDirectInput2BooleanFile();

            UserGUID2BooleanCollection userInput = new UserGUID2BooleanCollection()
            {
                m_booleanListener = userConfig.m_booleanIndexObserved,
                m_floatListener = userConfig.m_floatIndexObserved
            };


            directInput = new DirectInput();
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
            boolRegister.m_onBooleanChanged += (in string t, in bool d) =>
            {
                if (userConfig.m_useDebugOfBoolChanged)
                    Console.WriteLine(string.Format("Changed: {0} {1}", t, d));
                string cmd = "bool:" + t + ":" + (d ? "True" : "False");
                PushCommand(userConfig.m_targetIps, cmd);
            };

            DisplayStateLogOfDevices(directInput, userConfig.m_fullDebugDeviceId);

            if (userConfig.m_useFullDebugger)
            {
                Thread backgroundThread = new Thread(()=> {
                    DebugLogOfTrackDevice(userConfig.m_secondFrequenceOfFullDebug);
                }
                );
                backgroundThread.IsBackground = true;
                backgroundThread.Start();
            }

            Console.WriteLine("##############################");
            Console.WriteLine("Final Choosed Preference:");
            Console.WriteLine(JsonConvert.SerializeObject(userConfig, Formatting.Indented));
            Console.WriteLine("##############################");

            // Poll events from joystick
            while (true)
            {
                foreach (var joystick in joysticks)
                {
                    joystick.m_joystick.Poll();
                    var datas = joystick.m_joystick.GetBufferedData();
                    foreach (var state in datas)
                    {
                        if (useDebug_RawValueButtonObserved)
                        {
                            Console.WriteLine(string.Format("{0} | Value {1} | Offset{2}",
                            state.Offset, state.Value, state.RawOffset));
                        }
                        valueDictionary.SetWith(in joystick.m_guid, state.Offset, state.Value, state.RawOffset);
                    }
                }

                foreach (var floatValue in userInput.m_booleanListener)
                {
                    ProjectTempUtility.GetEnumOfStringJoystickOffset(
                        floatValue.m_buttonId, out bool converted, out JoystickOffset offset);
                    if (converted)
                    {
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
                            float pct = valueDictionary.m_valueHolder[id].m_value / (float)floatValue.m_floatMaxValue;
                            bool isTrue = pct >= floatValue.m_minValuePercent
                                && pct <= floatValue.m_maxValuePercent;
                            if (useDebug_RawValueObservedFrame)
                                Console.WriteLine(string.Format("TB: {0} | {1}", isTrue, id));
                            bool realBool = floatValue.m_inverseBoolean ? !isTrue : isTrue;
                            boolRegister.PushValue(floatValue.m_booleanName, realBool);




                        }
                    }
                }
                DisplayConnection(directInput);
                Thread.Sleep(1);
            }
        }

        private static void LoadDirectInput2BooleanFile()
        {
            //userConfig = defaultDemoConfig;
            string path = Directory.GetCurrentDirectory();
            string [] files = Directory.GetFiles(path, ".directinput2boolean", SearchOption.AllDirectories);

            List<UserGUIDFloat2Boolean> floatTracked = new List<UserGUIDFloat2Boolean>();
            List<UserGUIDBool2Boolean> boolTracked = new List<UserGUIDBool2Boolean>();

            foreach (var file in files)
            {
                string fileText = File.ReadAllText(file);
                string[] lines = fileText.Split("\n");
                foreach (var line in lines)
                {
                    string linetime = line.Trim();
                       
                    if ((linetime.Length>1&&linetime[0] == '/' && linetime[1] == '/') || (linetime.Length > 0 && linetime[0]== '#') ) {
                        continue;
                    }
                    string[] tokens = line.Split("♦");

                    if (tokens.Length >= 2) { 
                        
                        string deviceId = tokens[0].Trim();
                        string targetBoolean = tokens[1].Trim();
                        string targetInput = tokens[2].Trim();
                        if (int.TryParse(targetInput, out int buttonIndex))
                        {
                            bool useInverse = false;
                            if (tokens.Length >= 4)
                                useInverse = tokens[3].ToLower().IndexOf("inverse")>0;
                            boolTracked.Add(new UserGUIDBool2Boolean()
                            {
                                m_booleanName = targetBoolean,
                                m_buttonId = buttonIndex,
                                m_deviceGuid = deviceId,
                                m_valueLookedFor = useInverse ? false : true
                            });
                        }
                        else {

                            float min = 0;
                            float max = 1.0f;
                            int limit = 65535;
                            bool useInverse = false;
                            if (tokens.Length >= 4) float.TryParse(tokens[3], out min);
                            if (tokens.Length >= 5) float.TryParse(tokens[4], out max);
                            if (tokens.Length >= 6) int.TryParse(tokens[5], out limit);
                            if (tokens.Length >= 7) {
                                    useInverse = tokens[6].ToLower().IndexOf("inverse") > 0;
                            }

                            floatTracked.Add(new UserGUIDFloat2Boolean()
                            {
                                m_deviceGuid = deviceId,
                                m_booleanName = targetBoolean,
                                m_floatName = targetInput,
                                m_minValuePercent = min,
                                m_maxValuePercent = max,
                                m_floatMaxValue = limit,
                                m_inverseBoolean = useInverse 
                            });
                        }
                    }

                }

            }

            userConfig.m_booleanIndexObserved = userConfig.m_booleanIndexObserved.Concat(boolTracked).ToArray();
            userConfig.m_floatIndexObserved = userConfig.m_floatIndexObserved.Concat(floatTracked).ToArray();

        }

        private static void LoadConfigJson()
        {
            userConfig = defaultDemoConfig;
            string path = Directory.GetCurrentDirectory() + "/Config.json";
            Console.WriteLine("Config path: " + path);
            if (!File.Exists(path))
            {
                string save = JsonConvert.SerializeObject(userConfig, Formatting.Indented);
                File.WriteAllText(path, save);
            }
            if (File.Exists(path))
            {
                userConfig = JsonConvert.DeserializeObject<AppConfigurationJson>(File.ReadAllText(path));
            }
        }

        private static void DebugLogOfTrackDevice(float timeToBetweenFrame)
        {

            int timeToWait =(int)(timeToBetweenFrame * 1000f);
           Console.WriteLine("Timee" + timeToWait);
            while (true) { 
                
                DisplayStateLogOfDevices(directInput, userConfig.m_fullDebugDeviceId);
                Thread.Sleep(timeToWait);
            }
        }

        private static void PushCommand(AppConfigurationJson.IpTarget [] target, string cmd)


        {
            foreach (var item in target)
            {
                PushCommand(item.m_ipv4, item.m_port, item.m_useUnicode, cmd);
            }
        }
        private static void PushCommand(string ipAddress, int port, bool useUnicode, string message)

        {


            try
            {

                // Create a new UDP client
                using (UdpClient udpClient = new UdpClient(ipAddress, port)) 
                {
                    // Convert the message to bytes using the desired encoding
                    byte[] data =useUnicode?
                         Encoding.Unicode.GetBytes(message):
                         Encoding.UTF8.GetBytes(message);

                   // System.Net.IPEndPoint ep = new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ipAddress), port);


                    udpClient.Send(data, data.Length);// ep);
                    Console.WriteLine("Sent " + message+" to " +ipAddress+":"+port);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
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
                    Console.WriteLine(" " + item.ToString() + ", " + deviceInstance.InstanceName + ": " + deviceInstance.InstanceGuid);

                }
            }
        }
        public static Dictionary<string, IDAndDevice> deviceId = new Dictionary<string, IDAndDevice>();
        public static Dictionary<string, IDAndDevice> deviceIdPerma = new Dictionary<string, IDAndDevice>();
        public static  string[] m_currentPort= new string[0];
        public static  string[] m_previousPort = new string[0];
        public static  string[] m_newPort = new string[0];
        public static  string[] m_lostPort = new string[0];

        public struct IDAndDevice {

            public string m_id;
            public string m_name;

            public IDAndDevice(string instanceName, string instanceGuid) : this()
            {
                this.m_name = instanceName;
                this.m_id = instanceGuid;
            }
        }

       
        private static void DisplayConnection(DirectInput directInput)
        {
            deviceId.Clear();
            foreach (var item in ProjectTempUtility.GetEnum<DeviceType>())
            {
                foreach (var deviceInstance in directInput.GetDevices(item,
                        DeviceEnumerationFlags.AllDevices))
                {
                    string id = deviceInstance.InstanceGuid.ToString();
                    if (!deviceIdPerma.ContainsKey(id)){
                        deviceIdPerma.Add(id, new IDAndDevice(deviceInstance.InstanceName, id));
                    }
                    deviceId.Add(id, deviceIdPerma[id] );

                }
            }
            m_previousPort = m_currentPort.ToArray();
            m_currentPort = deviceId.Values.Select(k=>k.m_id).ToArray();
            m_newPort = (m_currentPort.ToArray().Except(m_previousPort)).ToArray();
            m_lostPort = (m_previousPort.ToArray().Except(m_currentPort)).ToArray();

            foreach (var item in m_newPort)
            {
                Console.WriteLine("New device: " + deviceIdPerma[item].m_id + " " + deviceIdPerma[item].m_name);

            }
            foreach (var item in m_lostPort)
            {
                Console.WriteLine("Lost device: " + deviceIdPerma[item].m_id + " " + deviceIdPerma[item].m_name);
            }
        }
        private static void DisplayStateLogOfDevices(DirectInput directInput, string[] deviceId)
        {



            Console.WriteLine("Device(s) state List");
            foreach (var item in ProjectTempUtility.GetEnum<DeviceType>())
            {
                foreach (var deviceInstance in directInput.GetDevices(item,
                        DeviceEnumerationFlags.AllDevices))
                {
                     foreach (var device in deviceId) {
                        if (deviceInstance.InstanceGuid.ToString() == device)
                        {
                             if (!m_trackedDevice.ContainsKey(device))
                            {

                                GetDeviceFromManager(device, directInput, out bool found, out Joystick joystick);
                                if (found)
                                    m_trackedDevice.Add(device, joystick);
                            }
                            if (m_trackedDevice.ContainsKey(device))
                            {
                                Joystick joystick = m_trackedDevice[device];
                                ReadInputState(ref joystick);
                            }
                        }


                    }

                }
            }
        }
        private static void ReadInputState(ref Joystick joystick)
        {
            // Poll the joystick to get the current state
            joystick.Poll();
            var state = joystick.GetCurrentState();

            // Get the button state
            var buttons = state.Buttons;

            Console.WriteLine("Joystick#" + joystick.Information.ProductName + "#"+ joystick.Information.InstanceGuid);
            // Display the button state
            Console.WriteLine("- Button State: ");
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i])
                    Console.Write($"Button {i + 1}: {(buttons[i] ? "Pressed" : "Released")}\t");
            }

            Console.WriteLine("- Int State:");

            foreach (var item in ProjectTempUtility.GetJoystickEnum())
            {
                GetValueOf(item, ref state, out string label, out int value);
                if(value>0)
                    Console.WriteLine("   "+label+" "+value+"   " );
            }

            
            if(state.Sliders.Length>0)
                Console.WriteLine("- Sliders:" + String.Join(" ", state.Sliders.Select(k => k.ToString()).ToArray()));

            if (state.AccelerationSliders.Length > 0)
                Console.WriteLine("- AccelerationSliders:" + String.Join(" ", state.AccelerationSliders.Select(k => k.ToString()).ToArray()));

            if (state.VelocitySliders.Length > 0) 
                Console.WriteLine("- VelocitySliders:" + String.Join(" ", state.VelocitySliders.Select(k => k.ToString()).ToArray()));

        


            Console.WriteLine();
        }

        private static void GetValueOf(JoystickOffset item, ref JoystickState state, out string label, out int value)
        {
            label = item.ToString();
            GetValueOf(item, ref state, out value);
        }


            private static void GetValueOf(JoystickOffset item, ref JoystickState state, out int value)
            {
            value = 0;
                switch (item)
            {
                case JoystickOffset.X:
                    value = state.X;
                    break;
                case JoystickOffset.Y:
                    value = state.Y;
                    break;
                case JoystickOffset.Z:
                    value = state.Z;
                    break;
                case JoystickOffset.RotationX:
                    value = state.RotationX;
                    break;
                case JoystickOffset.RotationY:
                    value = state.RotationY;
                    break;
                case JoystickOffset.RotationZ: value = state.RotationZ;
                    break;
                case JoystickOffset.Sliders0:
                    value = state.VelocitySliders.Length <= 1 ? 0 : state.VelocitySliders[0];
                    break;
                case JoystickOffset.Sliders1:
                    value = state.VelocitySliders.Length <= 2 ? 0 : state.VelocitySliders[1];
                    break;
                case JoystickOffset.PointOfViewControllers0:
                    value = state.PointOfViewControllers.Length <= 1 ? 0 : state.PointOfViewControllers[0];
                    break;
                case JoystickOffset.PointOfViewControllers1:
                    value = state.PointOfViewControllers.Length <= 2 ? 0 : state.PointOfViewControllers[1];
                    break;
                case JoystickOffset.PointOfViewControllers2:
                    value = state.PointOfViewControllers.Length <= 3 ? 0 : state.PointOfViewControllers[2];
                    break;
                case JoystickOffset.PointOfViewControllers3:
                    value = state.PointOfViewControllers.Length <= 4 ? 0 : state.PointOfViewControllers[3];
                    break;
                case JoystickOffset.Buttons0: value = !state.Buttons[0] ? 0 : 1; break;
                case JoystickOffset.Buttons1: value = !state.Buttons[1] ? 0 : 1; break;
                case JoystickOffset.Buttons2: value = !state.Buttons[2] ? 0 : 1; break;
                case JoystickOffset.Buttons3: value = !state.Buttons[3] ? 0 : 1; break;
                case JoystickOffset.Buttons4: value = !state.Buttons[4] ? 0 : 1; break;
                case JoystickOffset.Buttons5: value = !state.Buttons[5] ? 0 : 1; break;
                case JoystickOffset.Buttons6: value = !state.Buttons[6] ? 0 : 1; break;
                case JoystickOffset.Buttons7: value = !state.Buttons[7] ? 0 : 1; break;
                case JoystickOffset.Buttons8: value = !state.Buttons[8] ? 0 : 1; break;
                case JoystickOffset.Buttons9: value = !state.Buttons[9] ? 0 : 1; break;
                case JoystickOffset.Buttons10:value = !state.Buttons[10] ? 0 : 1; break;
                case JoystickOffset.Buttons11:value = !state.Buttons[11] ? 0 : 1; break;
                case JoystickOffset.Buttons12:value = !state.Buttons[12] ? 0 : 1; break;
                case JoystickOffset.Buttons13:value = !state.Buttons[13] ? 0 : 1; break;
                case JoystickOffset.Buttons14:value = !state.Buttons[14] ? 0 : 1; break;
                case JoystickOffset.Buttons15:value = !state.Buttons[15] ? 0 : 1; break;
                case JoystickOffset.Buttons16:value = !state.Buttons[16] ? 0 : 1; break;
                case JoystickOffset.Buttons17:value = !state.Buttons[17] ? 0 : 1; break;
                case JoystickOffset.Buttons18:value = !state.Buttons[18] ? 0 : 1; break;
                case JoystickOffset.Buttons19:value = !state.Buttons[19] ? 0 : 1; break;
                case JoystickOffset.Buttons20:value = !state.Buttons[20] ? 0 : 1; break;
                case JoystickOffset.Buttons21:value = !state.Buttons[21] ? 0 : 1; break;
                case JoystickOffset.Buttons22:value = !state.Buttons[22] ? 0 : 1; break;
                case JoystickOffset.Buttons23:value = !state.Buttons[23] ? 0 : 1; break;
                case JoystickOffset.Buttons24:value = !state.Buttons[24] ? 0 : 1; break;
                case JoystickOffset.Buttons25:value = !state.Buttons[25] ? 0 : 1; break;
                case JoystickOffset.Buttons26:value = !state.Buttons[26] ? 0 : 1; break;
                case JoystickOffset.Buttons27:value = !state.Buttons[27] ? 0 : 1; break;
                case JoystickOffset.Buttons28:value = !state.Buttons[28] ? 0 : 1; break;
                case JoystickOffset.Buttons29:value = !state.Buttons[29] ? 0 : 1; break;
                case JoystickOffset.Buttons30:value = !state.Buttons[30] ? 0 : 1; break;
                case JoystickOffset.Buttons31:value = !state.Buttons[31] ? 0 : 1; break;
                case JoystickOffset.Buttons32:value = !state.Buttons[32] ? 0 : 1; break;
                case JoystickOffset.Buttons33:value = !state.Buttons[33] ? 0 : 1; break;
                case JoystickOffset.Buttons34:value = !state.Buttons[34] ? 0 : 1; break;
                case JoystickOffset.Buttons35:value = !state.Buttons[35] ? 0 : 1; break;
                case JoystickOffset.Buttons36:value = !state.Buttons[36] ? 0 : 1; break;
                case JoystickOffset.Buttons37:value = !state.Buttons[37] ? 0 : 1; break;
                case JoystickOffset.Buttons38:value = !state.Buttons[38] ? 0 : 1; break;
                case JoystickOffset.Buttons39:value = !state.Buttons[39] ? 0 : 1; break;
                case JoystickOffset.Buttons40:value = !state.Buttons[40] ? 0 : 1; break;
                case JoystickOffset.Buttons41:value = !state.Buttons[41] ? 0 : 1; break;
                case JoystickOffset.Buttons42:value = !state.Buttons[42] ? 0 : 1; break;
                case JoystickOffset.Buttons43:value = !state.Buttons[43] ? 0 : 1; break;
                case JoystickOffset.Buttons44:value = !state.Buttons[44] ? 0 : 1; break;
                case JoystickOffset.Buttons45:value = !state.Buttons[45] ? 0 : 1; break;
                case JoystickOffset.Buttons46:value = !state.Buttons[46] ? 0 : 1; break;
                case JoystickOffset.Buttons47:value = !state.Buttons[47] ? 0 : 1; break;
                case JoystickOffset.Buttons48:value = !state.Buttons[48] ? 0 : 1; break;
                case JoystickOffset.Buttons49:value = !state.Buttons[49] ? 0 : 1; break;
                case JoystickOffset.Buttons50:value = !state.Buttons[50] ? 0 : 1; break;
                case JoystickOffset.Buttons51:value = !state.Buttons[51] ? 0 : 1; break;
                case JoystickOffset.Buttons52:value = !state.Buttons[52] ? 0 : 1; break;
                case JoystickOffset.Buttons53:value = !state.Buttons[53] ? 0 : 1; break;
                case JoystickOffset.Buttons54:value = !state.Buttons[54] ? 0 : 1; break;
                case JoystickOffset.Buttons55:value = !state.Buttons[55] ? 0 : 1; break;
                case JoystickOffset.Buttons56:value = !state.Buttons[56] ? 0 : 1; break;
                case JoystickOffset.Buttons57:value = !state.Buttons[57] ? 0 : 1; break;
                case JoystickOffset.Buttons58:value = !state.Buttons[58] ? 0 : 1; break;
                case JoystickOffset.Buttons59:value = !state.Buttons[59] ? 0 : 1; break;
                case JoystickOffset.Buttons60:value = !state.Buttons[60] ? 0 : 1; break;
                case JoystickOffset.Buttons61:value = !state.Buttons[61] ? 0 : 1; break;
                case JoystickOffset.Buttons62:value = !state.Buttons[62] ? 0 : 1; break;
                case JoystickOffset.Buttons63:value = !state.Buttons[63] ? 0 : 1; break;
                case JoystickOffset.Buttons64:value = !state.Buttons[64] ? 0 : 1; break;
                case JoystickOffset.Buttons65:value = !state.Buttons[65] ? 0 : 1; break;
                case JoystickOffset.Buttons66:value = !state.Buttons[66] ? 0 : 1; break;
                case JoystickOffset.Buttons67:value = !state.Buttons[67] ? 0 : 1; break;
                case JoystickOffset.Buttons68:value = !state.Buttons[68] ? 0 : 1; break;
                case JoystickOffset.Buttons69:value = !state.Buttons[69] ? 0 : 1; break;
                case JoystickOffset.Buttons70:value = !state.Buttons[70] ? 0 : 1; break;
                case JoystickOffset.Buttons71:value = !state.Buttons[71] ? 0 : 1; break;
                case JoystickOffset.Buttons72:value = !state.Buttons[72] ? 0 : 1; break;
                case JoystickOffset.Buttons73:value = !state.Buttons[73] ? 0 : 1; break;
                case JoystickOffset.Buttons74:value = !state.Buttons[74] ? 0 : 1; break;
                case JoystickOffset.Buttons75:value = !state.Buttons[75] ? 0 : 1; break;
                case JoystickOffset.Buttons76:value = !state.Buttons[76] ? 0 : 1; break;
                case JoystickOffset.Buttons77:value = !state.Buttons[77] ? 0 : 1; break;
                case JoystickOffset.Buttons78:value = !state.Buttons[78] ? 0 : 1; break;
                case JoystickOffset.Buttons79:value = !state.Buttons[79] ? 0 : 1; break;
                case JoystickOffset.Buttons80:value = !state.Buttons[80] ? 0 : 1; break;
                case JoystickOffset.Buttons81:value = !state.Buttons[81] ? 0 : 1; break;
                case JoystickOffset.Buttons82:value = !state.Buttons[82] ? 0 : 1; break;
                case JoystickOffset.Buttons83:value = !state.Buttons[83] ? 0 : 1; break;
                case JoystickOffset.Buttons84:value = !state.Buttons[84] ? 0 : 1; break;
                case JoystickOffset.Buttons85:value = !state.Buttons[85] ? 0 : 1; break;
                case JoystickOffset.Buttons86:value = !state.Buttons[86] ? 0 : 1; break;
                case JoystickOffset.Buttons87:value = !state.Buttons[87] ? 0 : 1; break;
                case JoystickOffset.Buttons88:value = !state.Buttons[88] ? 0 : 1; break;
                case JoystickOffset.Buttons89:value = !state.Buttons[89] ? 0 : 1; break;
                case JoystickOffset.Buttons90:value = !state.Buttons[90] ? 0 : 1; break;
                case JoystickOffset.Buttons91:value = !state.Buttons[91] ? 0 : 1; break;
                case JoystickOffset.Buttons92:value = !state.Buttons[92] ? 0 : 1; break;
                case JoystickOffset.Buttons93:value = !state.Buttons[93] ? 0 : 1; break;
                case JoystickOffset.Buttons94:value = !state.Buttons[94] ? 0 : 1; break;
                case JoystickOffset.Buttons95:value = !state.Buttons[95] ? 0 : 1; break;
                case JoystickOffset.Buttons96:value = !state.Buttons[96] ? 0 : 1; break;
                case JoystickOffset.Buttons97:value = !state.Buttons[97] ? 0 : 1; break;
                case JoystickOffset.Buttons98:value = !state.Buttons[98] ? 0 : 1; break;
                case JoystickOffset.Buttons99:value = !state.Buttons[99] ? 0 : 1; break;
                case JoystickOffset.Buttons100:value = !state.Buttons[100] ? 0 : 1; break;
                case JoystickOffset.Buttons101:value = !state.Buttons[101] ? 0 : 1; break;
                case JoystickOffset.Buttons102:value = !state.Buttons[102] ? 0 : 1; break;
                case JoystickOffset.Buttons103:value = !state.Buttons[103] ? 0 : 1; break;
                case JoystickOffset.Buttons104:value = !state.Buttons[104] ? 0 : 1; break;
                case JoystickOffset.Buttons105:value = !state.Buttons[105] ? 0 : 1; break;
                case JoystickOffset.Buttons106:value = !state.Buttons[106] ? 0 : 1; break;
                case JoystickOffset.Buttons107:value = !state.Buttons[107] ? 0 : 1; break;
                case JoystickOffset.Buttons108:value = !state.Buttons[108] ? 0 : 1; break;
                case JoystickOffset.Buttons109:value = !state.Buttons[109] ? 0 : 1; break;
                case JoystickOffset.Buttons110:value = !state.Buttons[110] ? 0 : 1; break;
                case JoystickOffset.Buttons111:value = !state.Buttons[111] ? 0 : 1; break;
                case JoystickOffset.Buttons112:value = !state.Buttons[112] ? 0 : 1; break;
                case JoystickOffset.Buttons113:value = !state.Buttons[113] ? 0 : 1; break;
                case JoystickOffset.Buttons114:value = !state.Buttons[114] ? 0 : 1; break;
                case JoystickOffset.Buttons115:value = !state.Buttons[115] ? 0 : 1; break;
                case JoystickOffset.Buttons116:value = !state.Buttons[116] ? 0 : 1; break;
                case JoystickOffset.Buttons117:value = !state.Buttons[117] ? 0 : 1; break;
                case JoystickOffset.Buttons118:value = !state.Buttons[118] ? 0 : 1; break;
                case JoystickOffset.Buttons119:value = !state.Buttons[119] ? 0 : 1; break;
                case JoystickOffset.Buttons120:value = !state.Buttons[120] ? 0 : 1; break;
                case JoystickOffset.Buttons121:value = !state.Buttons[121] ? 0 : 1; break;
                case JoystickOffset.Buttons122:value = !state.Buttons[122] ? 0 : 1; break;
                case JoystickOffset.Buttons123:value = !state.Buttons[123] ? 0 : 1; break;
                case JoystickOffset.Buttons124:value = !state.Buttons[124] ? 0 : 1; break;
                case JoystickOffset.Buttons125:value = !state.Buttons[125] ? 0 : 1; break;
                case JoystickOffset.Buttons126:value = !state.Buttons[126] ? 0 : 1; break;
                case JoystickOffset.Buttons127:value = !state.Buttons[127] ? 0 : 1; break;

                case JoystickOffset.VelocityX:value = state.VelocityX;
                    break;
                case JoystickOffset.VelocityY:
                    value = state.VelocityY;
                    break;
                case JoystickOffset.VelocityZ:
                    value = state.VelocityZ;
                    break;
                case JoystickOffset.AngularVelocityX:
                    value = state.VelocityX;
                    break;
                case JoystickOffset.AngularVelocityY:
                    value = state.AngularVelocityY;
                    break;
                case JoystickOffset.AngularVelocityZ:
                    value = state.AngularVelocityZ;
                    break;
                case JoystickOffset.VelocitySliders0:
                    value = state.VelocitySliders.Length <= 1 ? 0 : state.VelocitySliders[0];
                    break;
                case JoystickOffset.VelocitySliders1:
                    value = state.VelocitySliders.Length <= 2 ? 0 : state.VelocitySliders[1];
                    break;
                case JoystickOffset.AccelerationX:
                    value = state.AccelerationX;
                    break;
                case JoystickOffset.AccelerationY:
                    value = state.AccelerationY;
                    break;
                case JoystickOffset.AccelerationZ:
                    value = state.AccelerationZ;
                    break;
                case JoystickOffset.AngularAccelerationX:
                    value = state.AngularAccelerationX;
                    break;
                case JoystickOffset.AngularAccelerationY:
                    value = state.AngularAccelerationY;
                    break;
                case JoystickOffset.AngularAccelerationZ:
                    value = state.AngularAccelerationZ;
                    break;
                case JoystickOffset.AccelerationSliders0:
                    value = state.AccelerationSliders.Length <= 1 ? 0 : state.AccelerationSliders[0];
                    break;
                case JoystickOffset.AccelerationSliders1:
                    value = state.AccelerationSliders.Length <= 2 ? 0 : state.AccelerationSliders[1];
                    break;
                case JoystickOffset.ForceX:
                    value = state.ForceX;
                    break;
                case JoystickOffset.ForceY:
                    value = state.ForceY;
                    break;
                case JoystickOffset.ForceZ:
                    value = state.ForceZ;
                    break;
                case JoystickOffset.TorqueX:
                    value = state.TorqueX;
                    break;
                case JoystickOffset.TorqueY:
                    value = state.TorqueY;
                    break;
                case JoystickOffset.TorqueZ:
                    value = state.TorqueZ;
                    break;
                case JoystickOffset.ForceSliders0:
                    value = state.ForceSliders.Length <= 1 ? 0 : state.ForceSliders[0] ;
                    break;
                case JoystickOffset.ForceSliders1:
                    value = state.ForceSliders.Length<=2? 0:  state.ForceSliders[1];
                    break;
                default:
                    break;
            }
        }

        private static void GetDeviceFromManager(string deviceId, DirectInput manager, out bool found, out Joystick foundDevice)
        {
            found = false;
            foundDevice = null;

            foreach (var item in ProjectTempUtility.GetEnum<DeviceType>())
            {
                foreach (var deviceInstance in manager.GetDevices(item,
                        DeviceEnumerationFlags.AllDevices))
                {
                   
                    if (deviceInstance.InstanceGuid.ToString() == deviceId)
                    {
                        found = true;
                        foundDevice = new Joystick(manager, deviceInstance.InstanceGuid);
                        foundDevice.SetCooperativeLevel(IntPtr.Zero, 
                            CooperativeLevel.NonExclusive | CooperativeLevel.Background);
                        foundDevice.Acquire();
                      //  Console.WriteLine("Found:" + deviceId);
                        break;
                    }
                    //else Console.WriteLine("Not Found:" + deviceId);



                }
            }
        }



        public static Dictionary<string, Joystick> m_trackedDevice = new Dictionary<string, Joystick>();
    }
}
