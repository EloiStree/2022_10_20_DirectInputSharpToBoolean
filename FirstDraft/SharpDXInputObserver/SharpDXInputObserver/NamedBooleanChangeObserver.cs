using System.Collections.Generic;

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
