using SoCalCodeCamp.DeepDive.Helpers;

namespace SoCalCodeCamp.DeepDive.Services
{
    public class AuthOptions : AuthDemo.Services.IAADOptions
    {
        public string ClientId => Secrets.B2CApplicationId;
        public string Policy => Secrets.B2CPolicy;
        public string Tenant => Secrets.B2CTenant;
        public string[] Scopes => Secrets.B2CScopes.Split(',');
    }
}
