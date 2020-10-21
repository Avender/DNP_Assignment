using System;
using System.Collections.Generic;
using System.Linq;
using Models;

namespace Assigment_1.Data.Impl

{
    public class InMemoryUserService : IUser
    {
        private List<Adult> adults;

        public InMemoryUserService()
        {
            adults = new[]
            {
                new Adult
                {
                    Id = 123,
                    FirstName = "Michael",
                    LastName = "Anghelus",
                    HairColor = "black",
                    EyeColor = "brown",
                    Age = 20,
                    Weight = 85,
                    Height = 193,
                    Sex = "M"
                },
                new Adult
                {
                    Id = 125,
                    FirstName = "fef",
                    LastName = "Lavric",
                    HairColor = "black",
                    EyeColor = "brown",
                    Age = 20,
                    Weight = 75,
                    Height = 183,
                    Sex = "M"
                }
            }.ToList();
        }

        public Adult ValidateUser(string userName, string password)
        {
            Adult first = adults.FirstOrDefault(_adults => _adults.Id.Equals(userName));
            if (first == null)
            {
                throw new Exception("User not found");
            }

            if (!first.Password.Equals(password))
            {
                throw new Exception("Incorrect password");
            }

            return first;
        }
    }
}