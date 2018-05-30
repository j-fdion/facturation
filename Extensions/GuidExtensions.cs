using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFactu.Extensions
{
    public static class staticGuidExtensions
    {
        public static string ToHexColor(this Guid guid)
        {
            var values = guid.ToByteArray().Select(b => (int)b);
            int red = values.Take(5).Sum() % 255;
            int green = values.Skip(5).Take(5).Sum() % 255;
            int blue = values.Skip(10).Take(5).Sum() % 255;

            StringBuilder sb = new StringBuilder();
            sb.Append("#");
            sb.Append(((byte)red).ToString("X2"));
            sb.Append(((byte)green).ToString("X2"));
            sb.Append(((byte)blue).ToString("X2"));

            return sb.ToString();
        }
    }
}
