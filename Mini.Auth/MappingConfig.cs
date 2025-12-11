using AutoMapper;
using Mini.AuthApi.Models;
using Mini.AuthApi.Models.Dto;
namespace Mini.AuthApi
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var expression = new MapperConfigurationExpression();
            expression.CreateMap<User, UserDto>();
            expression.CreateMap<UserDto,User>();
            var config = new MapperConfiguration(expression);
            return config;
        }
    }
}
