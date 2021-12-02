namespace XCZ.FlowManagement.Dto
{
    public class FlowNodeDto
    {
        /// <summary>
        /// 节点Id
        /// </summary>
        public string NodeId { get; set; }

        public string NodeName { get; set; }

        public string Type { get; set; }

        public int Height { get; set; }

        public int X { get; set; }

        public int Width { get; set; }

        public int Y { get; set; }

        public string Remark { get; set; }
    }
}
