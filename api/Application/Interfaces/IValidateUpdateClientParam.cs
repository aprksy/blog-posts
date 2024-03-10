using api.Domain.Entities;

namespace api.Application.Interfaces
{
    public interface IValidateUpdateClientParam
    {
        void Validate(Client param);
    }
}