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
            CreateMap<FlowLine, FlowLinkDto>();
                //.ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.LineId))
                //.ForMember(dto => dto.FlowLinkId, opt => opt.MapFrom(src => src.Id));
            CreateMap<LineForm, LineFormDto>();
        }
    }
}
