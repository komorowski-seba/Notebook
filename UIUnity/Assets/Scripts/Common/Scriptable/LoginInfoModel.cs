using UnityEngine;

namespace Common.Scriptable
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Login Info Model Object", order = 1)]
    public class LoginInfoModel : ScriptableObject
    {
        public string urlAccesToken = "";
        public string urlCerts = "";

        [Space]
        public string clientId = "";
        public string role;

        [Space] public string urDomain = "";
    }
}