using api.Domain.Entities;

namespace api.Application.Interfaces
{
    public interface IValidateCreateClientParam
    {
        void Validate(Client param);
    }
}