using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dtos;
using Domain;

namespace Application.Interfaces
{
    public interface IJwtGenerator
    {
       Task<TokenAdapter> GenerateToken(AppUser user, List<ApplicationPermission> applicationPermissions,bool updateExistingToken);
       Task<bool> VerifyTokenValidity(TokenRequest tokenRequest, RefreshToken refreshToken);
    }
}