using SharpDX.DirectInput;

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
