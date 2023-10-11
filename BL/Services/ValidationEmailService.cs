using BLL.Interfaces;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace BLL.Services;

public class ValidationEmailService : IValidationEmailService
{
    private readonly ApplicationDbContext _db;

    public ValidationEmailService(ApplicationDbContext db)
    {
        _db = db;
    }

    public bool IsValidEmail(string email)
    {
        // Define a regular expression pattern for a valid email address
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+.[a-zA-Z]{2,}$";

        Regex regex = new Regex(pattern);

        return regex.IsMatch(email);
    }

    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        return !await _db.Users.AsNoTracking().AnyAsync(user => user.Email == email);
    }
}
