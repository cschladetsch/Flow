<<<<<<< HEAD
// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
=======
// (C) 2012-2018 Christian Schladetsch. See https://github.com/cschladetsch/Flow.
>>>>>>> 2156678... Updated to .Net4.5

namespace Flow
{
    public delegate void NameChangeHandler(INamed named, string newName, string oldName);

    /// Fires its Renamed event when its Name property is changed.
    public interface INamed
    {
        //event NameChangeHandler Renamed;
        string Name { get; set; }//
    }
}
