using AutoMapper;
using System.Linq;
using Volo.Abp.AutoMapper;
using XCZ.FlowManagement;
using XCZ.FlowManagement.Dto;

namespace XCZ
{
    public class FlowApplicationAutoMapperProfile : Profile
    {
        public FlowApplicationAutoMapperProfile()
        {
            CreateMap<BaseFlow, FlowDto>();
            CreateMap<FlowNode, FlowNodeDto>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.NodeId))
                .ForMember(dto => dto.Roles, opt => opt.MapFrom(src => src.Roles.Split(',', System.StringSplitOptions.None).ToList()))
                .ForMember(dto => dto.Users, opt => opt.MapFrom(src => src.Users.Split(',', System.StringSplitOptions.None).ToList()));
            CreateMap<FlowLine, FlowLinkDto>();
            CreateMap<LineForm, LineFormDto>();
        }
    }
}
