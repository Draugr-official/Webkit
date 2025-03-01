using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Webkit.Data
{
    public class TestData
    {
        /// <summary>
        /// Gets a random item from an array
        /// </summary>
        static string GetRandomItem(string[] stringArray)
        {
            int length = stringArray.Length;
            return stringArray[Random.Shared.Next(0, length)];
        }

        /// <summary>
        /// Returns a random first name
        /// </summary>
        public static string FirstName()
        {
            return GetRandomItem(DataSets.FirstNames);
        }

        /// <summary>
        /// Returns a random last name
        /// </summary>
        public static string LastName()
        {
            return GetRandomItem(DataSets.LastNames);
        }

        /// <summary>
        /// Gets a random work type
        /// </summary>
        public static string WorkType()
        {
            return GetRandomItem(DataSets.WorkTypes);
        }

        /// <summary>
        /// Gets a random domain extension without the dot
        /// </summary>
        public static string DomainExtension()
        {
            return GetRandomItem(DataSets.DomainExtensions);
        }



        /// <summary>
        /// Returns a random domain
        /// </summary>
        public static string Domain()
        {
            return Domain(FirstName(), WorkType(), DomainExtension());
        }

        /// <summary>
        /// Returns a random domain
        /// </summary>
        public static string Domain(string firstName)
        {
            return Domain(firstName, Regex.Replace(WorkType(), @"\s+", ""), DomainExtension());
        }

        /// <summary>
        /// Returns a random domain
        /// </summary>
        public static string Domain(string firstName, string workType)
        {
            return Domain(firstName, workType, DomainExtension());
        }

        /// <summary>
        /// Returns a random domain
        /// </summary>
        public static string Domain(string name, string workType, string extension)
        {
            return Regex.Replace(name + workType + "." + extension, @"\s+", "").ToLower();
        }



        /// <summary>
        /// Returns a random email
        /// </summary>
        public static string Email()
        {
            return FirstName() + "." + LastName() + "@" + Domain();
        }

        /// <summary>
        /// Returns an email with specified first name. The last name and domain is random.
        /// </summary>
        public static string Email(string firstName)
        {
            return firstName + "." + LastName() + "@" + Domain();
        }

        /// <summary>
        /// Returns an email with specified first and last name. The domain is random.
        /// </summary>
        public static string Email(string firstName, string lastName)
        {
            return firstName + "." + lastName + "@" + Domain();
        }

        /// <summary>
        /// Returns a fixed email
        /// </summary>
        public static string Email(string firstName, string lastName, string domain)
        {
            return Regex.Replace(firstName + "." + lastName + "@" + domain, @"\s+", "");
        }
    }
}
