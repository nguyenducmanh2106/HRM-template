using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SV.HRM.Service.Report.Ioc
{
    public static class ReportMappingRegister
    {
        public static void AddReportAutoMapper(this IServiceCollection services)
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
            //cfg.CreateMap<Staff, StaffModel>();
        }
    }
}
