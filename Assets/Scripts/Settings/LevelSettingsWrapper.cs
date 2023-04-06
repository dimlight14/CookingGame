using UnityEngine;

namespace DefaultNamespace
{
    public class LevelSettingsWrapper : ScriptableObject
    {
        /*I made those wrappers to easily show settings in editor window.
         Usually I would use Odin editor to handle stuff like that.*/
        
        public LevelSettings LevelSettings;
    }
}