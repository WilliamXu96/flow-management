using AutoMapper;
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
                .ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.NodeId));
            CreateMap<FlowLink, FlowLinkDto>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.LinkId))
                .ForMember(dto => dto.FlowLinkId, opt => opt.MapFrom(src => src.Id));
            CreateMap<LinkForm, LinkFormDto>();
        }
    }
}
