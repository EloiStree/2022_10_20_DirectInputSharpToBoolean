namespace SharpDXInputObserver
{
    [System.Serializable]
    public class UserGUIDFloat2Boolean {
        public string m_deviceGuid="";
        public string m_booleanName="";
        public string m_floatName="";
        public float m_minValuePercent=0.1f;
        public float m_maxValuePercent=1f;
        public bool m_inverseBoolean=false;
        public int m_floatMaxValue=65535;
    }
}
