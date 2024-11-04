using Application.Common.Interfaces;
using Newtonsoft.Json;
using System.Security.Claims;

namespace WebApi.Services
{
    public class CurrentActorService : ICurrentActorService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentActorService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? ClientId => _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == "client_id")?.Value;
        public string? UserId => _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub")?.Value;
        public string? UserIP => _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

        public bool IsClient => String.IsNullOrWhiteSpace(UserId);

        public List<string> Permissions => GetPermissions();

        private List<string> GetPermissions()
        {
            // Get the "permission" claims
            var permissionClaims = _httpContextAccessor.HttpContext?.User?.Claims.Where(c => c.Type == "permission").Select(c => c.Value).ToList();

            var permissions = new List<string>();

            foreach (var claim in permissionClaims)
            {
                // Check if the claim is a JSON array
                if (claim.StartsWith("[") && claim.EndsWith("]"))
                {
                    // Deserialize JSON array to a list of strings
                    var deserializedPermissions = JsonConvert.DeserializeObject<List<string>>(claim);
                    if (deserializedPermissions != null)
                    {
                        permissions.AddRange(deserializedPermissions);
                    }
                }
                else
                {
                    // Single permission value
                    permissions.Add(claim);
                }
            }

            // Return the permissions in the response
            return permissions;
        }
    }
}
