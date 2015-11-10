using LanguageExchange.Models.Dtos;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LanguageExchange.Models
{
    public class UserDetail
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public LanguageDetail[] LanguageDetails { get; set; }

        public static explicit operator UserDetail(UserDto userDto)
        {
            UserDetail ud = new UserDetail();

            ud.Id = userDto.Id.ToString();
            ud.Email = userDto.Email;
            ud.Firstname = userDto.Firstname;
            ud.Lastname = userDto.Lastname;
            ud.Username = userDto.Username;

            ud.LanguageDetails = userDto.LanguageDetails;

            return ud;
        }

    }
}