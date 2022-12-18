using SharpDX.DirectInput;
using System.Collections.Generic;

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
