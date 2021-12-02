using Volo.Abp.Application.Dtos;

namespace XCZ.FlowManagement.Dto
{
    public class GetFlowInputDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
