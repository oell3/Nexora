using System.Runtime.CompilerServices;
using Nexora.services;

namespace Nexora.API
{
    public static class UserEndpoint
    {
        

    public static void MapUserEndpoint(this RouteGroupBuilder api)
        {
            var _UserService = new UsersServices();
            var _group = api.MapGroup("/user");

            _group.MapGet("/", _UserService.Index);


        }

    }
}
