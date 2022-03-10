using System.ComponentModel.DataAnnotations;

namespace XCZ.WrokFlowManagement.Dto
{
    public class DoWorkFlowInputDto
    {
        [Required]
        public string Data { get; set; }
    }
}
