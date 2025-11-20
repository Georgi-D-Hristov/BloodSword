using System;

namespace BloodSword.Domain.Exceptions
{
    // Custom exception за грешки, при които не е намерен ресурс (трябва да върне 404)
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}
