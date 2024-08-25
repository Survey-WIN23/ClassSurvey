using ClassSurvey.Entities;
using ClassSurvey.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClassSurvey.Services;

public class AdminService(UserManager<UserEntity> userManager)
{
    private readonly UserManager<UserEntity> _userManager = userManager;


}
