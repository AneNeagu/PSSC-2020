using System;

namespace StackUnderflow.Backend.Grains
{
    public static class GrainKey
    {
        public static string Create(Guid organisationId, int tenantId, string name) 
        {
            return $"{organisationId}/{tenantId}/{name}";
        }

        public static bool TryParse(string key, out Guid organisationId, out int tenantId, out string name)
        {
            organisationId = Guid.Empty;
            tenantId = 0;
            name = string.Empty;

            var keyParts = key.Split('/');
            if (keyParts.Length != 3)
                return false;

            if (!Guid.TryParse(keyParts[0], out organisationId))
                return false;

            if (!int.TryParse(keyParts[1], out tenantId))
                return false;

            name = keyParts[2];
            return true;
        }
    }
}
