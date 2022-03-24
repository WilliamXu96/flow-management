using System.ComponentModel.DataAnnotations;

namespace XCZ.WorkFlowManagement.Dto
{
    public class DoWorkFlowInputDto
    {
        [Required]
        public string Data { get; set; }
    }
}
