using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDXInputObserver
{
    [System.Serializable]
    public class AppConfigurationJson
    {

        // The idea here is to be able to just add the id of the device and check how the input behave.
        public bool m_useFullDebugger=true;
        public string[] m_fullDebugDeviceId= new string[0];
        public float m_secondFrequenceOfFullDebug = 1;
        public bool m_useDebugOfBoolChanged=true;
        public IpTarget[] m_targetIps = new IpTarget[] { new IpTarget(2509)};


        public UserGUIDFloat2Boolean[] m_floatIndexObserved;
        public UserGUIDBool2Boolean[] m_booleanIndexObserved;


        [System.Serializable]
        public class IpTarget {
            public string m_ipv4;
            public int m_port;
            public bool m_useUnicode=false;
            public IpTarget()
            {
                m_ipv4 = "127.0.0.1";
                m_port = 2509;
            }
            public IpTarget(int port)
            {
                m_ipv4 = "127.0.0.1";
                m_port = port;
            }

            public IpTarget(string ipv4, int port)
            {
                m_ipv4 = ipv4;
                m_port = port;
            }
        }
    }

  
}
