using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAS.Uploading.API
{
    public class AutoMapperConfiguration
    {
        public IMapper RegisterAutoMapper(IServiceProvider services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
                //cfg.AddProfile<FileViewerProfile>();
                //cfg.AddProfile<FileCreationProfile>();
                //cfg.AddProfile<NotificationProfile>();
            });
            return config.CreateMapper();
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                //CreateMap<MdClient, MdClientViewModel>();
                //CreateMap<MdClientApplicationData, MdClientViewModel>();
                //CreateMap<MdPayor, MdPayorViewModel>();
                //CreateMap<MdPayorConfig, MdPayorConfigViewModel>();
            }
        }

        
    }
}
