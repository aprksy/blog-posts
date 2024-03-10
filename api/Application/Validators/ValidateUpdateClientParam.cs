using api.Domain.Interfaces;
using api.Domain.Entities;
using api.Application.Interfaces;

namespace api.Application.Validators
{
    public class ValidateUpdateClientParam : IValidateUpdateClientParam
    {
        public ValidateUpdateClientParam() {}
        public void Validate(Client param)
        {
            if (param.FirstName.Length == 0)
                throw new ArgumentException("FirstName is required");
            if (param.LastName.Length == 0)
                throw new ArgumentException("LastName is required");
            if (param.PhoneNumber.Length == 0)
                throw new ArgumentException("PhoneNumber is required");
            if (param.Email.Length == 0)
                throw new ArgumentException("Email is required");
        }
    }
}