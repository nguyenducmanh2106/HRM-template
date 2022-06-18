using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SV.HRM.Models;

namespace SV.HRM.Service.QuanTriDanhMuc.Ioc
{
    public static class QuanTriDanhMucMappingRegister
    {
        public static void AddQuanTriDanhMucAutoMapper(this IServiceCollection services)
        {
            // .... Ignore code before this

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                //cfg.CreateMissingTypeMaps = true;
                cfg.AllowNullCollections = true;
                cfg.ForAllMaps((map, exp) =>
                {
                    foreach (var unmappedPropertyName in map.GetUnmappedPropertyNames())
                        exp.ForMember(unmappedPropertyName, opt => opt.Ignore());
                });

                RegisterDocumentAutoMapper(cfg);
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
        public static void RegisterDocumentAutoMapper(this IMapperConfigurationExpression cfg)
        {
        }
    }
}
