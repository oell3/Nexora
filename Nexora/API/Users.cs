using Nexora.Data;
using Nexora.Services;

namespace Nexora.API
{
    public static class UserEndpoint
    {



        public static void MapUserEndpoint(this RouteGroupBuilder api)
        {

     
            var _group = api.MapGroup("/user");

            _group.MapGet("/", (UsersServices service) =>
            {
                return service.Create("test", "test");
            });


        }

    }
}
