using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EmailCore
{
    public class Addressee
    {
        public string Name { get; set; }
        public string Address { get; set; }

        /// <summary> Asynchronously checks Address against an email regular expression. </summary>
        public async Task<bool> Valid()
        {
            try
            {
                string pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$"; //credit to http://asp.net-informations.com/communications/asp-email-validation.htm for regex exp
                bool result = await Task.Run(() => Regex.IsMatch(
                    Address,
                    pattern,
                    RegexOptions.IgnoreCase,
                    TimeSpan.FromMilliseconds(1000)
                    ));
                return result;
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}