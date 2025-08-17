using DataAccessLayer.Models;
using DataAccessLayer.services;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.OtpCodes;

public class OtpCodeLogic(MainRepository<OtpCode>  repository)
{
    public bool AddOtpCode(OtpCode otp)
    {
        try
        {
            var result = repository.AddItem(otp);
            return true;
        }
        catch (Exception ex)
        {
            return false;   
        }
    }

    public async Task<string> GetCodeByphoneNumber(string phoneNumber)
    {
        try
        {
            return await repository.Get(c => c.PhoneNumber == phoneNumber).Select(c => c.Code).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            return "";
        }
    }
}