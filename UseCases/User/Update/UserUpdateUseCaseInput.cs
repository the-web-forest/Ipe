using System;
using System.ComponentModel.DataAnnotations;

namespace Ipe.UseCases.Update
{
    public class UserUpdateUseCaseInput
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public bool AllowNewsletter { get; set; } = true;
    }
}

