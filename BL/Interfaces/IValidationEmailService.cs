namespace BLL.Interfaces;

public interface IValidationEmailService
{
    public bool IsValidEmail(string email);
    public Task<bool> IsEmailUniqueAsync(string email);
}
