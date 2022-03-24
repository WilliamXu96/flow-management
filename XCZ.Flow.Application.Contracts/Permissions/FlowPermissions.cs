namespace XCZ.Permissions
{
    public static class FlowPermissions
    {
        public const string FlowManagement = "FlowManagement";

        public static class Flow
        {
            public const string Default = FlowManagement + ".Flow";
            public const string Delete = Default + ".Delete";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
        }

        public static class WorkFlow
        {
            public const string Default = FlowManagement + ".WorkFlow";
            public const string DoWorkFlow = Default + ".DoWorkFlow";
            public const string Create = Default + ".Create";
        }
    }
}
