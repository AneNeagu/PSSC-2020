using System;

namespace StackUnderflow.API.GraphQL.Controllers
{
    internal class ApplicationUserUpdatedEvent
    {
        public ApplicationUser ApplicationUser { get; set; }
    }

    public class ApplicationUser
    {
        public User User { get; set; }
        public ApplicationSubscription ApplicationSubscription { get; set; }
    }

    public class User
    {
        public Guid Id { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return string.Concat(FirstName, " ", LastName);
            }
        }
    }
}