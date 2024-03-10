using System.Runtime.InteropServices;
using api.Application.Adapters;
using api.Application.Interfaces;
using api.Domain.Entities;

public class EndpointConfiguration
{
    public static void Configure(WebApplication app)
    {
        app.MapGet("/clients", async Task<ApiResponse<Client[]>>(IGetClients getClients) =>
        {
            try
            {
                var data = await getClients.Handle();
                var response = new ApiResponse<Client[]>(true, "Success", data);
                return response;
            }
            catch (System.Exception)
            {
                var response = new ApiResponse<Client[]>(false, "General Exception", null);
                return response;
            }
        })
        .WithName("get clients");

        app.MapGet("/search/{name}", async Task<ApiResponse<Client[]>>(string name, ISearchClients searchClients) =>
        {
            try
            {
                var data = await searchClients.Handle(name);
                var response = new ApiResponse<Client[]>(true, "Success", data);
                return response;
            }
            catch (System.Exception)
            {
                var response = new ApiResponse<Client[]>(false, "General Exception", null);
                return response;
            }
        })
        .WithName("search clients");

        /*
            Ideally, we shall use Dto as update param here. However, since all fields are required we can skip that.
            For the Id field, will use the one that come from params, thus the json body allowed to
            not include it.
        */
        app.MapPut("/clients/{id}", async Task<ApiResponse<string>> (string id, Client client, IUpdateClient updateClient, IValidateUpdateClientParam validateUpdateClientParam) => 
        {
            try
            {
                validateUpdateClientParam.Validate(client);
                var _client = await updateClient.GetById(id);
                if (_client == null)
                    return new ApiResponse<string>(StatusCodes.Status404NotFound, false, "Client not found", "");

                // assign new values to the existing client object
                _client.FirstName = client.FirstName;
                _client.LastName = client.LastName;
                _client.PhoneNumber = client.PhoneNumber;
                _client.Email = client.Email;

                await updateClient.Handle(_client);
                var response = new ApiResponse<string>(true, "Success", "");
                return response;
            }
            catch (ArgumentException e)
            {
                return new ApiResponse<string>(StatusCodes.Status400BadRequest, false, e.Message, "");
            }
            catch (ExternalException e)
            {
                return new ApiResponse<string>(StatusCodes.Status502BadGateway, false, e.Message, "");
            }
            catch (Exception)
            {
                var response = new ApiResponse<string>(false, "General Exception", "");
                return response;
            }
        })
        .WithName("update clients");

        /*
            Ideally, we shall use Dto as create param here. However, since all fields are required we can skip that.
        */
        app.MapPost("/clients", async Task<ApiResponse<string>> (Client client, ICreateClient createClient, IValidateCreateClientParam validateCreateClientParam) => 
        {
            try
            {
                validateCreateClientParam.Validate(client);

                await createClient.Handle(client);
                var response = new ApiResponse<string>(true, "Success", "");
                return response;
            }
            catch (ArgumentException e)
            {
                return new ApiResponse<string>(StatusCodes.Status400BadRequest, false, e.Message, "");
            }
            catch (ExternalException e)
            {
                return new ApiResponse<string>(StatusCodes.Status502BadGateway, false, e.Message, "");
            }
            catch (Exception)
            {
                var response = new ApiResponse<string>(false, "General Exception", "");
                return response;
            }
        })
        .WithName("create clients");
    }
}