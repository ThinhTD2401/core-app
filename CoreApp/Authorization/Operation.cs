using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace CoreApp.Authorization
{
    public static class Operation
    {
        public static OperationAuthorizationRequirement Create = new OperationAuthorizationRequirement { Name = nameof(Create) };
        public static OperationAuthorizationRequirement Read = new OperationAuthorizationRequirement { Name = nameof(Read) };
        public static OperationAuthorizationRequirement Update = new OperationAuthorizationRequirement { Name = nameof(Update) };
        public static OperationAuthorizationRequirement Delete = new OperationAuthorizationRequirement { Name = nameof(Delete) };
        public static OperationAuthorizationRequirement Upload = new OperationAuthorizationRequirement { Name = nameof(Upload) };
        public static OperationAuthorizationRequirement Import = new OperationAuthorizationRequirement { Name = nameof(Import) };
        public static OperationAuthorizationRequirement Export = new OperationAuthorizationRequirement { Name = nameof(Export) };
    }

}
