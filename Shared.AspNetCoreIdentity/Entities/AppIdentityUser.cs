using Microsoft.AspNetCore.Identity;

namespace Shared.AspNetCoreIdentity.Entities;

public class AppIdentityUser<TKey> : IdentityUser<TKey>
	where TKey : IEquatable<TKey>
{
}
