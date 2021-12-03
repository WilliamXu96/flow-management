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
            CreateMap<FlowNode, FlowNodeDto>();
            CreateMap<FlowLink, FlowLinkDto>();
            CreateMap<LinkForm, LinkFormDto>();
        }
    }
}
