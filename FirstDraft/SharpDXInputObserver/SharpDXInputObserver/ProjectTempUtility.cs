using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;

public class ProjectTempUtility {
    public static void GetIdFrom(in string guid, in JoystickOffset offset, out string id) { id = guid + "_" + offset; }

    public static IEnumerable<JoystickOffset> GetJoystickEnum() {
        return GetEnum<JoystickOffset>();
    }

    public static IEnumerable<T> GetEnum<T>()
    {

        return Enum.GetValues(typeof(T)).Cast<T>();
    }

    public static void GetEnumOfStringJoystickOffset(string id, out bool converted, out JoystickOffset value)
    {
        converted = Enum.TryParse<JoystickOffset>(id, true, out value);
        if (!converted) {

            id = id.ToLower();
            foreach (var item in GetEnum<JoystickOffset>())
            {
                if (item.ToString().ToLower() == id) {
                    converted = true;
                    value = item;
                    return;
                }
            }
        }

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