using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageExchange.Models.Dtos
{
    public class MostRecentUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public MRLanguageDetailDto[] LanguageDetails {get;set;}

        public static explicit operator MostRecentUserDto(UserDto userDto)
        {
            MostRecentUserDto mr = new MostRecentUserDto();
            int count = 0;

            mr.Username = userDto.Username;
            mr.Email = userDto.Email;
            mr.LanguageDetails = new MRLanguageDetailDto[userDto.LanguageDetails.Length];

            foreach(var ld in userDto.LanguageDetails.ToList())
            {
                var mrld = new MRLanguageDetailDto()
                {
                    Language = ld.Language.Value,
                    Proficiency = ld.Proficiency.Value
                };

                mr.LanguageDetails[count++] = mrld;
            }

            return mr;
        }

    }
}
